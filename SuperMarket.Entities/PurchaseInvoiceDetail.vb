' SuperMarket.Entities/PurchaseInvoiceDetail.vb
Imports System ' قد تحتاج لإضافة هذا السطر إذا لم يكن موجودًا

<Serializable()>
Public Class PurchaseInvoiceDetail
    Public Property DetailID As Integer
    Public Property InvoiceID As Integer
    Public Property ProductID As Integer
    Public Property Quantity As Decimal
    Public Property PurchasePrice As Decimal

    ' === الخاصية الجديدة ===
    Public Property SellingPrice As Decimal

    ' خصائص إضافية للعرض في الجدول
    Public Property ProductName As String
    Public Property SubTotal As Decimal
End Class
