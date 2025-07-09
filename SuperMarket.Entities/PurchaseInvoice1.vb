' SuperMarket.Entities/PurchaseInvoice.vb
Imports System ' قد تحتاج لإضافة هذا السطر إذا لم يكن موجودًا

<Serializable()>
Public Class PurchaseInvoice1
    ' --- تأكد من وجود كل هذه الخصائص ---
    Public Property InvoiceID As Integer
    Public Property InvoiceNumber As String
    Public Property SupplierID As Integer
    Public Property BranchID As Integer
    Public Property InvoiceDate As Date
    Public Property TotalAmount As Decimal
    Public Property Notes As String
    Public Property SupplierName As String
    ' --- وهذه القائمة مهمة جدًا ---
    Public Property Details As New List(Of PurchaseInvoiceDetail)()
End Class