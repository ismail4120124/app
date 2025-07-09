<Serializable()>
Public Class SalesReturnInvoiceDetail
    Public Property ReturnDetailID As Long
    Public Property ReturnInvoiceID As Long
    Public Property ProductID As Integer
    Public Property Quantity As Decimal
    Public Property UnitPrice As Decimal
    Public Property SubTotal As Decimal ' محسوب
    ' خصائص إضافية للعرض
    Public Property ProductName As String
End Class
