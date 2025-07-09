' SuperMarket.DAL/CategoryDAL.vb
Imports System.Data.SqlClient
Imports System.Configuration
Imports SuperMarket.Entities

Public Class CategoryDAL
    Private ReadOnly _connectionString As String = ConfigurationManager.ConnectionStrings("SuperMarketDB").ConnectionString

    Public Function GetAllCategories() As List(Of ProductCategory)
        Dim categories As New List(Of ProductCategory)()
        Using conn As New SqlConnection(_connectionString)
            Dim cmd As New SqlCommand("SELECT CategoryID, CategoryName FROM ProductCategories", conn)
            conn.Open()
            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    categories.Add(New ProductCategory With {
                        .CategoryID = Convert.ToInt32(reader("CategoryID")),
                        .CategoryName = reader("CategoryName").ToString()
                    })
                End While
            End Using
        End Using
        Return categories
    End Function

    Public Sub AddCategory(ByVal category As ProductCategory)
        Using conn As New SqlConnection(_connectionString)
            Dim cmd As New SqlCommand("INSERT INTO ProductCategories (CategoryName) VALUES (@CategoryName)", conn)
            cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName)
            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub
End Class
