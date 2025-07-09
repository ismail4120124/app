' OpeningBalance.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class OpeningBalance
    Inherits BasePage ' Inherit from BasePage for security

    Private branchBll As New BranchBLL()
    Private productBll As New ProductBLL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            PopulateBranchesDropDown()
        End If
    End Sub

    Private Sub PopulateBranchesDropDown()
        ddlBranches.DataSource = branchBll.GetAllBranches()
        ddlBranches.DataTextField = "BranchName"
        ddlBranches.DataValueField = "BranchID"
        ddlBranches.DataBind()
        ddlBranches.Items.Insert(0, New ListItem("-- اختر فرعًا --", "0"))
    End Sub

    Protected Sub ddlBranches_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlBranches.SelectedIndexChanged
        lblMessage.Visible = False
        Dim branchId = Convert.ToInt32(ddlBranches.SelectedValue)
        If branchId > 0 Then
            BindOpeningBalanceGrid(branchId)
            PanelOpeningBalance.Visible = True
        Else
            PanelOpeningBalance.Visible = False
        End If
    End Sub

    Private Sub BindOpeningBalanceGrid(ByVal branchId As Integer)
        ' Get all products with their current balance in this branch
        Dim data = productBll.GetProductsForOpeningBalance(branchId)
        GridViewOpeningBalance.DataSource = data
        GridViewOpeningBalance.DataBind()
    End Sub

    Protected Sub btnSaveChanges_Click(sender As Object, e As EventArgs) Handles btnSaveChanges.Click
        Try
            Dim branchId = Convert.ToInt32(ddlBranches.SelectedValue)
            If branchId <= 0 Then Throw New Exception("Please select a branch first.")

            Dim openingBalanceList As New List(Of ProductPricingInfo)()

            ' Loop through each row in the GridView to collect the data
            For Each row As GridViewRow In GridViewOpeningBalance.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim productId = Convert.ToInt32(GridViewOpeningBalance.DataKeys(row.RowIndex).Value)

                    Dim txtQuantity = CType(row.FindControl("txtQuantity"), TextBox)
                    Dim txtCostPrice = CType(row.FindControl("txtCostPrice"), TextBox)
                    Dim txtSellingPrice = CType(row.FindControl("txtSellingPrice"), TextBox)

                    Dim qty, cost, price As Decimal
                    Decimal.TryParse(txtQuantity.Text, qty)
                    Decimal.TryParse(txtCostPrice.Text, cost)
                    Decimal.TryParse(txtSellingPrice.Text, price)

                    ' We only need to save items that have a quantity
                    If qty > 0 Then
                        openingBalanceList.Add(New ProductPricingInfo With {
                            .ProductID = productId,
                            .Quantity = qty,
                            .CostPrice = cost,
                            .SellingPrice = price
                        })
                    End If
                End If
            Next

            If Not openingBalanceList.Any() Then
                lblMessage.Text = "No data to save. Please enter quantities for the items."
                lblMessage.CssClass = "alert alert-warning"
                lblMessage.Visible = True
                Return
            End If

            ' Send the list to the BLL to be saved in a single transaction
            productBll.SaveOpeningBalance(openingBalanceList, branchId)

            lblMessage.Text = "Opening balances have been saved successfully!"
            lblMessage.CssClass = "alert alert-success"
            lblMessage.Visible = True

        Catch ex As Exception
            lblMessage.Text = "Failed to save data: " & ex.Message
            lblMessage.CssClass = "alert alert-danger"
            lblMessage.Visible = True
        End Try
    End Sub
End Class