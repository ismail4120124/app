' SuperMarket.DAL/AccountDAL.vb
Imports System.Data.SqlClient
Imports System.Configuration
Imports SuperMarket.Entities

Public Class AccountDAL
    Private ReadOnly _connectionString As String = ConfigurationManager.ConnectionStrings("SuperMarketDB").ConnectionString

    ''' <summary>
    ''' يجلب كل الحسابات من قاعدة البيانات مرتبة حسب رقم الحساب.
    ''' </summary>
    Public Function GetAllAccounts() As List(Of Account)
        Dim accounts As New List(Of Account)()
        Using conn As New SqlConnection(_connectionString)
            ' نرتب حسب رقم الحساب لضمان بناء الشجرة بشكل صحيح
            Dim query As String = "SELECT AccountID, AccountNumber, AccountName, ParentAccountID, AccountLevel, IsTransactional, AccountType FROM Accounts ORDER BY AccountNumber"
            Dim cmd As New SqlCommand(query, conn)
            conn.Open()
            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim acc As New Account()
                    acc.AccountID = Convert.ToInt32(reader("AccountID"))
                    acc.AccountNumber = reader("AccountNumber").ToString()
                    acc.AccountName = reader("AccountName").ToString()

                    ' التعامل مع ParentAccountID الذي يقبل NULL
                    If Not IsDBNull(reader("ParentAccountID")) Then
                        acc.ParentAccountID = Convert.ToInt32(reader("ParentAccountID"))
                    End If

                    acc.AccountLevel = Convert.ToInt32(reader("AccountLevel"))
                    acc.IsTransactional = Convert.ToBoolean(reader("IsTransactional"))
                    acc.AccountType = reader("AccountType").ToString()
                    accounts.Add(acc)
                End While
            End Using
        End Using
        Return accounts
    End Function

    ''' <summary>
    ''' يضيف حسابًا جديدًا إلى قاعدة البيانات.
    ''' </summary>
    Public Sub AddAccount(ByVal acc As Account)
        Using conn As New SqlConnection(_connectionString)
            Dim query As String = "INSERT INTO Accounts (AccountNumber, AccountName, ParentAccountID, AccountLevel, IsTransactional, AccountType) " &
                                  "VALUES (@Number, @Name, @ParentID, @Level, @IsTransactional, @Type)"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@Number", acc.AccountNumber)
            cmd.Parameters.AddWithValue("@Name", acc.AccountName)
            ' تمرير DBNull.Value إذا لم يكن هناك حساب أب
            cmd.Parameters.AddWithValue("@ParentID", If(acc.ParentAccountID.HasValue, CType(acc.ParentAccountID.Value, Object), DBNull.Value))
            cmd.Parameters.AddWithValue("@Level", acc.AccountLevel)
            cmd.Parameters.AddWithValue("@IsTransactional", acc.IsTransactional)
            cmd.Parameters.AddWithValue("@Type", acc.AccountType)
            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    ' لاحقًا، يمكنك إضافة دوال للتعديل (Update) والحذف (Delete) بنفس الطريقة
    ' أضف هذه الدالة داخل كلاس AccountDAL

    Public Sub UpdateAccount(ByVal acc As Account)
        Using conn As New SqlConnection(_connectionString)
            Dim query As String = "UPDATE Accounts SET " &
                                  "AccountNumber = @Number, " &
                                  "AccountName = @Name, " &
                                  "IsTransactional = @IsTransactional, " &
                                  "AccountType = @Type " &
                                  "WHERE AccountID = @ID"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@Number", acc.AccountNumber)
            cmd.Parameters.AddWithValue("@Name", acc.AccountName)
            cmd.Parameters.AddWithValue("@IsTransactional", acc.IsTransactional)
            cmd.Parameters.AddWithValue("@Type", acc.AccountType)
            cmd.Parameters.AddWithValue("@ID", acc.AccountID)
            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub
    ' أضف هذه الدالة داخل كلاس AccountDAL

    Public Sub DeleteAccount(ByVal accountId As Integer)
        Using conn As New SqlConnection(_connectionString)
            Dim query As String = "DELETE FROM Accounts WHERE AccountID = @ID"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@ID", accountId)
            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub
End Class