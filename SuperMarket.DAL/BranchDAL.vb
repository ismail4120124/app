
' SuperMarket.DAL/BranchDAL.vb

Imports System.Data.SqlClient
Imports System.Configuration
Imports SuperMarket.Entities ' لا تنس استيراد مشروع الكيانات

Public Class BranchDAL
    ' جلب سلسلة الاتصال من ملف Web.config
    Private ReadOnly _connectionString As String = ConfigurationManager.ConnectionStrings("SuperMarketDB").ConnectionString

    ' دالة لجلب كل الفروع من قاعدة البيانات
    Public Function GetAllBranches() As List(Of Branch)
        Dim branches As New List(Of Branch)()
        Using conn As New SqlConnection(_connectionString)
            Dim cmd As New SqlCommand("SELECT BranchID, BranchName, Address, Phone FROM Branches", conn)
            conn.Open()
            Dim reader As SqlDataReader = cmd.ExecuteReader()
            While reader.Read()
                Dim branch As New Branch()
                branch.BranchID = Convert.ToInt32(reader("BranchID"))
                branch.BranchName = reader("BranchName").ToString()
                branch.Address = reader("Address").ToString()
                branch.Phone = reader("Phone").ToString()
                branches.Add(branch)
            End While
        End Using
        Return branches
    End Function

    ' دالة لإضافة فرع جديد
    Public Sub AddBranch(ByVal branch As Branch)
        Using conn As New SqlConnection(_connectionString)
            Dim cmd As New SqlCommand("INSERT INTO Branches (BranchName, Address, Phone) VALUES (@BranchName, @Address, @Phone)", conn)
            cmd.Parameters.AddWithValue("@BranchName", branch.BranchName)
            cmd.Parameters.AddWithValue("@Address", branch.Address)
            cmd.Parameters.AddWithValue("@Phone", branch.Phone)
            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    ' يمكنك إضافة دوال التعديل والحذف بنفس الطريقة لاحقاً
End Class
