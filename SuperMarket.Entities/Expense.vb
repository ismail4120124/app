' SuperMarket.Entities/Expense.vb

<Serializable()>
Public Class Expense
    Public Property ExpenseID As Long
    Public Property ExpenseDate As Date
    Public Property Amount As Decimal
    Public Property CategoryID As Integer
    Public Property Description As String
    Public Property BranchID As Integer? ' Nullable
    Public Property UserID As Integer
    Public Property PaidFromAccountID As Integer
    Public Property JournalVoucherID As Long? ' Nullable

    ' خصائص إضافية للعرض في التقارير
    Public Property CategoryName As String
    Public Property BranchName As String
    Public Property UserName As String
    Public Property PaidFromAccountName As String
End Class