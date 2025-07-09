' SuperMarket.BLL/SalesInvoiceBLL.vb
Imports SuperMarket.DAL
Imports SuperMarket.Entities
Imports System.Transactions


Public Class SalesInvoiceBLL
    Private dal As New SalesInvoiceDAL()





    Public Function SaveInvoice(ByVal invoice As SalesInvoice) As Long
        ' قواعد التحقق الأساسية من صحة بيانات الفاتورة
        If Not invoice.Details.Any() Then Throw New Exception("لا يمكن حفظ فاتورة فارغة.")
        If invoice.BranchID <= 0 OrElse invoice.UserID <= 0 OrElse invoice.CustomerID <= 0 Then
            Throw New Exception("بيانات الفرع أو المستخدم أو العميل غير صالحة.")
        End If

        ' نستخدم TransactionScope لضمان أن كل العمليات (حفظ الفاتورة وحفظ القيود) تتم معًا أو تفشل معًا
        Using scope As New TransactionScope()
            ' --- الخطوة 1: حفظ الفاتورة وتحديث المخزون (عبر DAL) ---
            ' دالة dal.SaveInvoice تقوم بالفعل بخصم المخزون داخل Transaction خاص بها
            ' TransactionScope سيقوم بدمج هذا الـ Transaction مع الـ Transactions القادمة
            Dim newInvoiceId = dal.SaveInvoice(invoice)
            If newInvoiceId <= 0 Then
                Throw New Exception("فشل إنشاء سجل الفاتورة الرئيسي.")
            End If

            ' --- الخطوة 2: إنشاء القيود المحاسبية التلقائية ---
            CreateJournalEntriesForSale(invoice, newInvoiceId)

            ' --- الخطوة 3: إتمام المعاملة بنجاح ---
            ' إذا وصلنا إلى هنا، فكل العمليات تمت بنجاح
            scope.Complete()

            Return newInvoiceId
        End Using ' عند الخروج من Using، إذا لم يتم استدعاء scope.Complete()، سيتم التراجع عن كل شيء
    End Function
    ' أضف هاتين الدالتين داخل كلاس SalesInvoiceBLL.vb


    ' في SalesInvoiceBLL.vb

    ''' <summary>
    ''' دالة رئيسية لإنشاء كل القيود المحاسبية المتعلقة بفاتورة المبيعات.
    ''' </summary>
    Private Sub CreateJournalEntriesForSale(ByVal invoice As SalesInvoice, ByVal newInvoiceId As Long)
        ' 1. جلب الإعدادات المحاسبية
        Dim settingsBll As New SettingsBLL()
        Dim settings = settingsBll.GetAccountingSettings()

        ' 2. التحقق من وجود الحسابات الأساسية في الإعدادات
        If Not settings.DefaultSalesAccountID.HasValue OrElse
       Not settings.DefaultInventoryAccountID.HasValue OrElse
       Not settings.DefaultPurchasesAccountID.HasValue Then
            Throw New Exception("الرجاء إكمال إعدادات الربط المحاسبي أولاً (حساب المبيعات، المخزون، وتكلفة البضاعة).")
        End If

        Dim jvBll As New JournalVoucherBLL()

        ' ==========================================================
        ' === القيد الأول: إثبات الإيراد والتحصيل ===
        ' ==========================================================
        Dim revenueJV As New JournalVoucher()
        revenueJV.VoucherDate = invoice.InvoiceDate
        revenueJV.Description = "قيد إيراد فاتورة مبيعات رقم " & newInvoiceId
        revenueJV.SalesInvoiceID = newInvoiceId

        Dim finalAmount = invoice.TotalAmount - invoice.DiscountAmount

        ' تحديد الطرف المدين (من حـ/)
        Dim debitAccountId As Integer

        If invoice.PaymentMethod = "Credit" Then
            ' قيد المبيعات الآجلة
            ' --- التحقق من وجود الحساب الافتراضي للعملاء ---
            If Not settings.DefaultDebtorsAccountID.HasValue OrElse settings.DefaultDebtorsAccountID.Value <= 0 Then
                Throw New Exception("الرجاء تحديد حساب العملاء (الذمم المدينة) الافتراضي في شاشة الإعدادات المحاسبية.")
            End If
            debitAccountId = settings.DefaultDebtorsAccountID.Value
        Else
            ' قيد المبيعات النقدية
            debitAccountId = If(invoice.PaymentMethod = "Cash", settings.DefaultCashAccountID.Value, settings.DefaultBankAccountID.Value)
        End If
        If debitAccountId <= 0 Then Throw New Exception("لم يتم تحديد حساب الصندوق أو البنك في الإعدادات.")

        revenueJV.Details.Add(New JournalVoucherDetail With {.AccountID = debitAccountId, .Debit = finalAmount})
        revenueJV.Details.Add(New JournalVoucherDetail With {.AccountID = settings.DefaultSalesAccountID.Value, .Credit = finalAmount})

        jvBll.SaveJournalVoucher(revenueJV) ' حفظ القيد الأول

        ' ==========================================================
        ' === القيد الثاني: إثبات التكلفة وتخفيض المخزون ===
        ' ==========================================================

        ' حساب إجمالي التكلفة للفاتورة من البيانات التي تم جلبها
        Dim totalCost As Decimal = invoice.Details.Sum(Function(d) d.Quantity * d.CostPrice)

        ' لا ننشئ قيد تكلفة إذا كانت التكلفة صفراً (للأصناف الخدمية أو في حالة خطأ)
        If totalCost > 0 Then
            Dim costJV As New JournalVoucher()
            costJV.VoucherDate = invoice.InvoiceDate
            costJV.Description = "قيد تكلفة فاتورة مبيعات رقم " & newInvoiceId
            costJV.SalesInvoiceID = newInvoiceId

            ' الطرف المدين (من حـ/ تكلفة البضاعة المباعة)
            costJV.Details.Add(New JournalVoucherDetail With {
            .AccountID = settings.DefaultPurchasesAccountID.Value,
            .Debit = totalCost,
            .Credit = 0
        })

            ' الطرف الدائن (إلى حـ/ المخزون)
            costJV.Details.Add(New JournalVoucherDetail With {
            .AccountID = settings.DefaultInventoryAccountID.Value,
            .Debit = 0,
            .Credit = totalCost
        })

            jvBll.SaveJournalVoucher(costJV) ' حفظ القيد الثاني
        End If
    End Sub


    ' في نهاية كلاس SalesInvoiceBLL.vb

    Public Function GetInvoiceForPrinting(ByVal invoiceId As Long) As SalesInvoice
        If invoiceId <= 0 Then
            Return Nothing
        End If
        Return dal.GetInvoiceForPrinting(invoiceId)
    End Function

End Class