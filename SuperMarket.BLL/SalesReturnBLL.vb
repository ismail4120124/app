' SuperMarket.BLL/SalesReturnBLL.vb
Imports SuperMarket.DAL
Imports SuperMarket.Entities
Imports System.Transactions ' << أضف هذا السطر في أعلى الملف

Public Class SalesReturnBLL
    Private dal As New SalesReturnDAL()

    ''' <summary>
    ''' يتحقق من صحة بيانات فاتورة المرتجع ثم يقوم بحفظها مع إنشاء القيد المحاسبي.
    ''' </summary>
    Public Function SaveReturnInvoice(ByVal returnInvoice As SalesReturnInvoice) As Long
        ' --- 1. التحقق من صحة البيانات (يبقى كما هو) ---
        If Not returnInvoice.Details.Any() Then
            Throw New Exception("لا يمكن حفظ فاتورة مرتجع فارغة.")
        End If
        If returnInvoice.BranchID <= 0 OrElse returnInvoice.UserID <= 0 OrElse returnInvoice.CustomerID <= 0 Then
            Throw New Exception("بيانات الفرع، المستخدم، أو العميل غير مكتملة.")
        End If

        ' حساب إجمالي قيمة المرتجع
        returnInvoice.TotalReturnAmount = returnInvoice.Details.Sum(Function(d) d.Quantity * d.UnitPrice)
        If returnInvoice.TotalReturnAmount <= 0 Then
            Throw New Exception("إجمالي قيمة المرتجع يجب أن يكون أكبر من صفر.")
        End If

        ' --- 2. استخدام TransactionScope لضمان تنفيذ كل العمليات معًا ---
        Using scope As New TransactionScope()
            ' أ. حفظ سجل المرتجع وتحديث المخزون
            Dim newReturnInvoiceId = dal.SaveReturnInvoice(returnInvoice)
            If newReturnInvoiceId <= 0 Then
                Throw New Exception("فشل حفظ بيانات فاتورة المرتجع.")
            End If

            ' ب. إنشاء القيد المحاسبي التلقائي
            CreateJournalEntriesForSalesReturn(returnInvoice, newReturnInvoiceId)

            ' ج. إتمام المعاملة بنجاح
            scope.Complete()

            Return newReturnInvoiceId
        End Using
    End Function

    ''' <summary>
    ''' دالة مساعدة لإنشاء القيود المحاسبية لمرتجع المبيعات.
    ''' </summary>
    Private Sub CreateJournalEntriesForSalesReturn(ByVal returnInvoice As SalesReturnInvoice, ByVal newReturnInvoiceId As Long)
        ' 1. جلب إعدادات الربط المحاسبي
        Dim settingsBll As New SettingsBLL()
        Dim settings = settingsBll.GetAccountingSettings()

        ' 2. التحقق من ربط الحسابات الضرورية
        If Not settings.DefaultSalesReturnAccountID.HasValue Then Throw New Exception("الرجاء ربط 'حساب مرتجعات المبيعات' في الإعدادات.")
        If Not settings.DefaultCashAccountID.HasValue Then Throw New Exception("الرجاء ربط 'حساب الصندوق' في الإعدادات.")
        If Not settings.DefaultInventoryAccountID.HasValue Then Throw New Exception("الرجاء ربط 'حساب المخزون' في الإعدادات.")
        If Not settings.DefaultPurchasesAccountID.HasValue Then Throw New Exception("الرجاء ربط 'حساب تكلفة البضاعة' في الإعدادات.")

        Dim jvBll As New JournalVoucherBLL()
        Dim totalReturnAmount = returnInvoice.TotalReturnAmount

        ' ==========================================================
        ' === القيد الأول: عكس قيد الإيراد ===
        ' ==========================================================
        Dim revenueReturnJV As New JournalVoucher()
        revenueReturnJV.VoucherDate = returnInvoice.ReturnDate
        revenueReturnJV.Description = "قيد عكس إيراد مرتجع مبيعات رقم " & newReturnInvoiceId
        revenueReturnJV.SalesReturnInvoiceID = newReturnInvoiceId

        ' الطرف المدين (من حـ/ مرتجعات المبيعات)
        revenueReturnJV.Details.Add(New JournalVoucherDetail With {
            .AccountID = settings.DefaultSalesReturnAccountID.Value,
            .Debit = totalReturnAmount
        })
        ' الطرف الدائن (إلى حـ/ الصندوق) - لأننا أرجعنا المبلغ للعميل
        revenueReturnJV.Details.Add(New JournalVoucherDetail With {
            .AccountID = settings.DefaultCashAccountID.Value,
            .Credit = totalReturnAmount
        })
        jvBll.SaveJournalVoucher(revenueReturnJV) ' حفظ القيد الأول

        ' ==========================================================
        ' === القيد الثاني: عكس قيد التكلفة ===
        ' ==========================================================
        ' (نفترض أننا سنقوم بجلب التكلفة مع تفاصيل المرتجع لاحقًا)
        Dim totalCost As Decimal = 0 ' << للمستقبل: يجب حسابها بشكل فعلي
        ' Dim totalCost As Decimal = returnInvoice.Details.Sum(Function(d) d.Quantity * d.CostPrice)

        If totalCost > 0 Then
            Dim costReturnJV As New JournalVoucher()
            costReturnJV.VoucherDate = returnInvoice.ReturnDate
            costReturnJV.Description = "قيد عكس تكلفة مرتجع مبيعات رقم " & newReturnInvoiceId
            costReturnJV.SalesReturnInvoiceID = newReturnInvoiceId

            ' الطرف المدين (من حـ/ المخزون) - لأن المخزون زاد
            costReturnJV.Details.Add(New JournalVoucherDetail With {
                .AccountID = settings.DefaultInventoryAccountID.Value,
                .Debit = totalCost
            })
            ' الطرف الدائن (إلى حـ/ تكلفة البضاعة المباعة) - لأن المصروف نقص
            costReturnJV.Details.Add(New JournalVoucherDetail With {
                .AccountID = settings.DefaultPurchasesAccountID.Value,
                .Credit = totalCost
            })
            jvBll.SaveJournalVoucher(costReturnJV) ' حفظ القيد الثاني
        End If
    End Sub
End Class