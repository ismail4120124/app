<Serializable()>
Public Class SalesReturnInvoice
    Public Property ReturnInvoiceID As Long
    Public Property OriginalInvoiceID As Long? ' Nullable
    Public Property ReturnDate As DateTime
    Public Property BranchID As Integer
    Public Property UserID As Integer
    Public Property CustomerID As Integer
    Public Property TotalReturnAmount As Decimal
    Public Property Notes As String
    Public Property Details As New List(Of SalesReturnInvoiceDetail)()
End Class
