' Suppliers.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class Suppliers
    Inherits BasePage

    Private bll As New SupplierBLL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindSuppliersGrid()
        End If
    End Sub

    Private Sub BindSuppliersGrid()
        GridViewSuppliers.DataSource = bll.GetAllSuppliers()
        GridViewSuppliers.DataBind()
    End Sub

    Protected Sub btnAddSupplier_Click(sender As Object, e As EventArgs) Handles btnAddSupplier.Click
        Try
            Dim newSupplier As New Supplier()
            newSupplier.SupplierName = txtSupplierName.Text
            newSupplier.ContactPerson = txtContactPerson.Text
            newSupplier.Phone = txtPhone.Text
            newSupplier.Email = txtEmail.Text
            newSupplier.Address = txtAddress.Text
            newSupplier.IsActive = chkIsActive.Checked

            bll.AddSupplier(newSupplier)

            BindSuppliersGrid()
            ClearForm()
            lblMessage.Text = "تمت إضافة المورد بنجاح!"
            lblMessage.ForeColor = System.Drawing.Color.Green

        Catch ex As Exception
            lblMessage.Text = "حدث خطأ: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

    Private Sub ClearForm()
        txtSupplierName.Text = ""
        txtContactPerson.Text = ""
        txtPhone.Text = ""
        txtEmail.Text = ""
        txtAddress.Text = ""
        chkIsActive.Checked = True
    End Sub
End Class