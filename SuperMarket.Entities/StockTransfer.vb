<Serializable()>
Public Class StockTransfer
    Public Property TransferID As Integer
    Public Property TransferDate As Date
    Public Property SourceBranchID As Integer
    Public Property DestinationBranchID As Integer
    Public Property UserID As Integer
    Public Property Notes As String
    Public Property Details As New List(Of StockTransferDetail)()
End Class