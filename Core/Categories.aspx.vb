' Categories.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class Categories ' لاحظ تغيير الاسم
    Inherits System.Web.UI.Page

    Private bll As New CategoryBLL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindGrid()
        End If
    End Sub

    Private Sub BindGrid()
        GridViewCategories.DataSource = bll.GetAllCategories()
        GridViewCategories.DataBind()
    End Sub

    Protected Sub btnAddCategory_Click(sender As Object, e As EventArgs) Handles btnAddCategory.Click
        Try
            bll.AddCategory(New ProductCategory With {.CategoryName = txtCategoryName.Text})
            BindGrid()
            txtCategoryName.Text = ""
            lblMessage.Text = "تمت الإضافة بنجاح."
            lblMessage.ForeColor = System.Drawing.Color.Green
        Catch ex As Exception
            lblMessage.Text = "خطأ: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub
End Class