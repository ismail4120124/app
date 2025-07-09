Public Class Account
    Public Property AccountID As Integer
    Public Property AccountNumber As String
    Public Property AccountName As String
    Public Property ParentAccountID As Integer? ' Nullable
    Public Property AccountLevel As Integer
    Public Property IsTransactional As Boolean
    Public Property AccountType As String
End Class