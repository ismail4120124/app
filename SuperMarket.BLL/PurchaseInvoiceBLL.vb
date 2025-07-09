' SuperMarket.BLL/PurchaseInvoiceBLL.vb
Imports System.Transactions
Imports SuperMarket.DAL
Imports SuperMarket.Entities


Public Class PurchaseInvoiceBLL
    Private dal As New PurchaseInvoiceDAL()

    ' في PurchaseInvoiceBLL.vb
    ' أضف هذا السطر في أعلى الملف


    Public Function SaveInvoice(ByVal invoice As PurchaseInvoice1) As Integer
        ' --- التحقق من صحة الفاتورة (يبقى كما هو) ---
        If Not invoice.Details.Any() Then Throw New Exception("الفاتورة فارغة.")
        ' ...

        ' نستخدم TransactionScope لضمان أن العمليتين (حفظ الفاتورة وحفظ القيد) تنجحان معًا أو تفشلان معًا
        Using scope As New TransactionScope()

            ' === الجزء الأول: حفظ الفاتورة وتحديث المخزون ===
            ' هذا الجزء لا يتغير. نستدعي DAL لحفظ بيانات الفاتورة وزيادة كميات المخزون.
            Dim newInvoiceId = dal.SaveInvoice(invoice)
            If newInvoiceId <= 0 Then Throw New Exception("فشل حفظ بيانات الفاتورة.")


            ' === الجزء الثاني (الجديد): إنشاء وحفظ القيد المحاسبي ===
            ' نستدعي دالة مساعدة جديدة وواضحة للقيام بهذه المهمة
            CreateJournalEntryForPurchase(invoice, newInvoiceId)


            ' إذا وصل الكود إلى هنا، فكل شيء تم بنجاح.
            scope.Complete() ' تأكيد كل العمليات

            Return newInvoiceId
        End Using
    End Function

    ''' <summary>
    ''' دالة مساعدة واضحة، وظيفتها فقط إنشاء قيد محاسبي لفاتورة الشراء.
    ''' </summary>
    Private Sub CreateJournalEntryForPurchase(ByVal invoice As PurchaseInvoice1, ByVal newInvoiceId As Integer)
        ' 1. نحضر "قاموس" الحسابات من الإعدادات
        Dim settingsBll As New SettingsBLL()
        Dim settings = settingsBll.GetAccountingSettings()

        ' 2. نتأكد من أن المحاسب قد قام بربط الحسابات التي نحتاجها
        If Not settings.DefaultInventoryAccountID.HasValue Then Throw New Exception("الرجاء ربط حساب المخزون في الإعدادات.")
        If Not settings.DefaultSuppliersAccountID.HasValue Then Throw New Exception("الرجاء ربط حساب الموردين في الإعدادات.")

        ' 3. نحضر "أداة" حفظ القيود
        Dim jvBll As New JournalVoucherBLL()

        ' 4. نبني "القيد" الجديد
        Dim jv As New JournalVoucher()
        jv.VoucherDate = invoice.InvoiceDate
        jv.Description = "قيد إثبات فاتورة مشتريات رقم " & newInvoiceId & " من المورد " & invoice.SupplierName
        jv.PurchaseInvoiceID = newInvoiceId ' نربط القيد بالفاتورة

        ' 5. نحسب قيمة القيد (إجمالي الفاتورة)
        Dim totalAmount = invoice.Details.Sum(Function(d) d.Quantity * d.PurchasePrice)

        ' 6. نضيف أطراف القيد
        ' الطرف المدين: المخزون (لأنه أصل وزاد)
        jv.Details.Add(New JournalVoucherDetail With {
        .AccountID = settings.DefaultInventoryAccountID.Value,
        .Debit = totalAmount,
        .Credit = 0
    })
        ' الطرف الدائن: الموردون (لأنه التزام وزاد)
        jv.Details.Add(New JournalVoucherDetail With {
        .AccountID = settings.DefaultSuppliersAccountID.Value,
        .Debit = 0,
        .Credit = totalAmount
    })

        ' 7. نرسل القيد الجاهز ليتم حفظه
        jvBll.SaveJournalVoucher(jv)
    End Sub
End Class