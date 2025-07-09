' Customers.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class Customers
    Inherits BasePage

    Private bll As New CustomerBLL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindCustomersGrid()
        End If
    End Sub

    Private Sub BindCustomersGrid()
        GridViewCustomers.DataSource = bll.GetAllCustomers()
        GridViewCustomers.DataBind()
    End Sub

    Protected Sub btnAddCustomer_Click(sender As Object, e As EventArgs) Handles btnAddCustomer.Click
        Try
            Dim newCustomer As New Customer()
            newCustomer.CustomerName = txtCustomerName.Text
            newCustomer.Phone = txtPhone.Text
            newCustomer.Email = txtEmail.Text
            newCustomer.Address = txtAddress.Text
            newCustomer.IsActive = chkIsActive.Checked

            bll.AddCustomer(newCustomer)

            BindCustomersGrid()
            ClearForm()
            lblMessage.Text = "تمت إضافة العميل بنجاح!"
            lblMessage.ForeColor = System.Drawing.Color.Green

        Catch ex As Exception
            lblMessage.Text = "خطأ: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

    Private Sub ClearForm()
        txtCustomerName.Text = ""
        txtPhone.Text = ""
        txtEmail.Text = ""
        txtAddress.Text = ""
        chkIsActive.Checked = True
    End Sub
End Class