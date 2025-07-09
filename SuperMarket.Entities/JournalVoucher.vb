' في SuperMarket.Entities/JournalVoucher.vb

Public Class JournalVoucher
    Public Property VoucherID As Long
    Public Property VoucherDate As Date
    Public Property Description As String
    Public Property IsPosted As Boolean

    ' الأعمدة الاختيارية لربط القيد
    Public Property SalesInvoiceID As Long?
    Public Property PurchaseInvoiceID As Integer?

    ' ==========================================================
    ' === أضف الخاصيتين التاليتين هنا ===
    ' ==========================================================
    Public Property SalesReturnInvoiceID As Long?
    Public Property StockTransferID As Integer?
    ' ==========================================================

    ' قائمة تفاصيل القيد
    Public Property Details As New List(Of JournalVoucherDetail)()
End Class