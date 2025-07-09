' SuperMarket.DAL/ExpenseCategoryDAL.vb
Imports System.Data.SqlClient
Imports System.Configuration
Imports SuperMarket.Entities

Public Class ExpenseCategoryDAL
    Private ReadOnly _connectionString As String = ConfigurationManager.ConnectionStrings("SuperMarketDB").ConnectionString

    ''' <summary>
    ''' يجلب كل بنود المصاريف مع اسم الحساب المحاسبي المرتبط بها.
    ''' </summary>
    Public Function GetAllExpenseCategories() As List(Of ExpenseCategory)
        Dim categories As New List(Of ExpenseCategory)()
        Using conn As New SqlConnection(_connectionString)
            ' نستخدم LEFT JOIN لضمان عرض البند حتى لو تم حذف الحساب المرتبط به
            Dim query As String = "SELECT ec.CategoryID, ec.CategoryName, ec.AccountID, ec.IsActive, a.AccountName " &
                                  "FROM ExpenseCategories ec " &
                                  "LEFT JOIN Accounts a ON ec.AccountID = a.AccountID " &
                                  "ORDER BY ec.CategoryName"
            Dim cmd As New SqlCommand(query, conn)
            conn.Open()
            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim cat As New ExpenseCategory()
                    cat.CategoryID = Convert.ToInt32(reader("CategoryID"))
                    cat.CategoryName = reader("CategoryName").ToString()
                    cat.AccountID = Convert.ToInt32(reader("AccountID"))
                    cat.IsActive = Convert.ToBoolean(reader("IsActive"))
                    ' جلب اسم الحساب من جدول Accounts
                    cat.AccountName = If(IsDBNull(reader("AccountName")), "حساب محذوف", reader("AccountName").ToString())
                    categories.Add(cat)
                End While
            End Using
        End Using
        Return categories
    End Function

    ''' <summary>
    ''' يضيف بند مصروف جديد إلى قاعدة البيانات.
    ''' </summary>
    Public Sub AddExpenseCategory(ByVal category As ExpenseCategory)
        Using conn As New SqlConnection(_connectionString)
            Dim query As String = "INSERT INTO ExpenseCategories (CategoryName, AccountID, IsActive) VALUES (@Name, @AccountID, @IsActive)"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@Name", category.CategoryName)
            cmd.Parameters.AddWithValue("@AccountID", category.AccountID)
            cmd.Parameters.AddWithValue("@IsActive", category.IsActive)
            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    ' لاحقًا، يمكنك إضافة دالة للتعديل بنفس الطريقة
    ' Public Sub UpdateExpenseCategory(ByVal category As ExpenseCategory)
    '     ...
    ' End Sub

End Class
