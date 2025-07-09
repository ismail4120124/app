' SuperMarket.BLL/CategoryBLL.vb
Imports SuperMarket.DAL
Imports SuperMarket.Entities

Public Class CategoryBLL
    Private dal As New CategoryDAL()

    Public Function GetAllCategories() As List(Of ProductCategory)
        Return dal.GetAllCategories()
    End Function

    Public Sub AddCategory(ByVal category As ProductCategory)
        If String.IsNullOrWhiteSpace(category.CategoryName) Then
            Throw New Exception("اسم الفئة مطلوب.")
        End If
        dal.AddCategory(category)
    End Sub
End Class