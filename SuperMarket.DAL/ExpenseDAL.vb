' SuperMarket.DAL/ExpenseDAL.vb
Imports System.Data.SqlClient
Imports System.Configuration
Imports SuperMarket.Entities

Public Class ExpenseDAL
    Private ReadOnly _connectionString As String = ConfigurationManager.ConnectionStrings("SuperMarketDB").ConnectionString
    Public Function AddExpense(ByVal expense As Expense) As Long
        Using conn As New SqlConnection(_connectionString)
            Dim query As String = "INSERT INTO Expenses (ExpenseDate, Amount, CategoryID, Description, BranchID, UserID, PaidFromAccountID) " &
                                  "VALUES (@Date, @Amount, @CatID, @Desc, @BranchID, @UserID, @PaidFrom); SELECT SCOPE_IDENTITY();"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@Date", expense.ExpenseDate)
            cmd.Parameters.AddWithValue("@Amount", expense.Amount)
            cmd.Parameters.AddWithValue("@CatID", expense.CategoryID)
            cmd.Parameters.AddWithValue("@Desc", expense.Description)
            cmd.Parameters.AddWithValue("@BranchID", If(expense.BranchID.HasValue, CType(expense.BranchID.Value, Object), DBNull.Value))
            cmd.Parameters.AddWithValue("@UserID", expense.UserID)
            cmd.Parameters.AddWithValue("@PaidFrom", expense.PaidFromAccountID)

            conn.Open()
            Return Convert.ToInt64(cmd.ExecuteScalar())
        End Using
    End Function


    ' في ExpenseDAL.vb

    ''' <summary>
    ''' يجلب آخر المصروفات المسجلة مع تفاصيلها.
    ''' </summary>
    ''' <param name="recordCount">عدد السجلات المراد جلبها.</param>
    Public Function GetRecentExpenses(ByVal recordCount As Integer) As List(Of Expense)
        Dim expenses As New List(Of Expense)()
        Using conn As New SqlConnection(_connectionString)
            ' نستخدم TOP لجلب العدد المحدد، ونستخدم LEFT JOIN لضمان عرض المصروف حتى لو تم حذف الفرع
            Dim query As String = "SELECT TOP (@Count) " &
                                  "e.ExpenseID, e.ExpenseDate, e.Amount, e.Description, " &
                                  "ec.CategoryName, b.BranchName, u.FullName AS UserName, pa.AccountName AS PaidFromAccountName " &
                                  "FROM Expenses e " &
                                  "JOIN ExpenseCategories ec ON e.CategoryID = ec.CategoryID " &
                                  "JOIN Users u ON e.UserID = u.UserID " &
                                  "JOIN Accounts pa ON e.PaidFromAccountID = pa.AccountID " &
                                  "LEFT JOIN Branches b ON e.BranchID = b.BranchID " &
                                  "ORDER BY e.ExpenseID DESC" ' نرتب تنازليًا لجلب الأحدث

            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@Count", recordCount)
            conn.Open()

            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim exp As New Expense()
                    exp.ExpenseID = Convert.ToInt64(reader("ExpenseID"))
                    exp.ExpenseDate = Convert.ToDateTime(reader("ExpenseDate"))
                    exp.Amount = Convert.ToDecimal(reader("Amount"))
                    exp.Description = reader("Description").ToString()
                    exp.CategoryName = reader("CategoryName").ToString()
                    exp.BranchName = If(IsDBNull(reader("BranchName")), "عام", reader("BranchName").ToString())
                    exp.UserName = reader("UserName").ToString()
                    exp.PaidFromAccountName = reader("PaidFromAccountName").ToString()
                    expenses.Add(exp)
                End While
            End Using
        End Using
        Return expenses
    End Function

    ' لاحقًا، سنضيف دالة لتحديث القيد بعد إنشائه
    Public Sub UpdateExpenseWithJournalVoucherId(ByVal expenseId As Long, ByVal jvId As Long)
        Using conn As New SqlConnection(_connectionString)
            Dim query As String = "UPDATE Expenses SET JournalVoucherID = @JvID WHERE ExpenseID = @ExpenseID"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@JvID", jvId)
            cmd.Parameters.AddWithValue("@ExpenseID", expenseId)
            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub
End Class
