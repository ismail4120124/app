' SuperMarket.DAL/CustomerDAL.vb
Imports System.Data.SqlClient
Imports System.Configuration
Imports SuperMarket.Entities

Public Class CustomerDAL
    Private ReadOnly _connectionString As String = ConfigurationManager.ConnectionStrings("SuperMarketDB").ConnectionString

    ' دالة للبحث عن العملاء (للاستخدام مع AutoComplete)
    Public Function SearchCustomers(ByVal searchTerm As String, ByVal maxRows As Integer) As List(Of Customer)
        Dim customers As New List(Of Customer)()
        Using conn As New SqlConnection(_connectionString)
            ' TOP للحد من عدد النتائج لتحسين الأداء
            Dim query As String = "SELECT TOP (@MaxRows) CustomerID, CustomerName, Phone FROM Customers WHERE IsActive = 1 AND (CustomerName LIKE @SearchTerm OR Phone LIKE @SearchTerm)"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@MaxRows", maxRows)
            cmd.Parameters.AddWithValue("@SearchTerm", "%" & searchTerm & "%")
            conn.Open()
            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    customers.Add(New Customer With {
                        .CustomerID = Convert.ToInt32(reader("CustomerID")),
                        .CustomerName = reader("CustomerName").ToString(),
                        .Phone = reader("Phone").ToString()
                    })
                End While
            End Using
        End Using
        Return customers
    End Function
    ' ... يمكنك إضافة دوال Add, GetAll لاحقًا لبناء شاشة إدارة العملاء
    ' داخل كلاس CustomerDAL

    ' دالة لجلب كل العملاء
    Public Function GetAllCustomers() As List(Of Customer)
        Dim customers As New List(Of Customer)()
        Using conn As New SqlConnection(_connectionString)
            ' استثناء "عميل نقدي" من القائمة لأنه عميل نظام
            Dim query As String = "SELECT CustomerID, CustomerName, Phone, Address, Email, IsActive FROM Customers WHERE CustomerID <> 1"
            Dim cmd As New SqlCommand(query, conn)
            conn.Open()
            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    customers.Add(New Customer With {
                        .CustomerID = Convert.ToInt32(reader("CustomerID")),
                        .CustomerName = reader("CustomerName").ToString(),
                        .Phone = reader("Phone").ToString(),
                        .Address = reader("Address").ToString(),
                        .Email = reader("Email").ToString(),
                        .IsActive = Convert.ToBoolean(reader("IsActive"))
                    })
                End While
            End Using
        End Using
        Return customers
    End Function

    ' دالة لإضافة عميل جديد
    Public Sub AddCustomer(ByVal customer As Customer)
        Using conn As New SqlConnection(_connectionString)
            Dim query As String = "INSERT INTO Customers (CustomerName, Phone, Address, Email, IsActive) VALUES (@Name, @Phone, @Address, @Email, @IsActive)"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@Name", customer.CustomerName)
            cmd.Parameters.AddWithValue("@Phone", If(String.IsNullOrEmpty(customer.Phone), DBNull.Value, customer.Phone))
            cmd.Parameters.AddWithValue("@Address", If(String.IsNullOrEmpty(customer.Address), DBNull.Value, customer.Address))
            cmd.Parameters.AddWithValue("@Email", If(String.IsNullOrEmpty(customer.Email), DBNull.Value, customer.Email))
            cmd.Parameters.AddWithValue("@IsActive", customer.IsActive)
            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub
End Class