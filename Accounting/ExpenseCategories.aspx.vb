' ExpenseCategories.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class ExpenseCategories
    Inherits BasePage

    Private categoryBll As New ExpenseCategoryBLL()
    Private accountBll As New AccountBLL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            PopulateAccountsDropDown()
            BindGrid()
        End If
    End Sub

    Private Sub PopulateAccountsDropDown()
        ' جلب الحسابات التحليلية من نوع "مصاريف" فقط
        Dim expenseAccounts = accountBll.GetAllAccounts().Where(
            Function(acc) acc.IsTransactional AndAlso acc.AccountType = "مصاريف"
        ).ToList()

        ddlAccounts.DataSource = expenseAccounts
        ddlAccounts.DataTextField = "AccountName"
        ddlAccounts.DataValueField = "AccountID"
        ddlAccounts.DataBind()
        ddlAccounts.Items.Insert(0, New ListItem("-- اختر حساب المصروف --", "0"))
    End Sub

    Private Sub BindGrid()
        GridViewCategories.DataSource = categoryBll.GetAllExpenseCategories()
        GridViewCategories.DataBind()
    End Sub

    Protected Sub btnAddCategory_Click(sender As Object, e As EventArgs) Handles btnAddCategory.Click
        Try
            Dim newCategory As New ExpenseCategory()
            newCategory.CategoryName = txtCategoryName.Text.Trim()
            newCategory.AccountID = Convert.ToInt32(ddlAccounts.SelectedValue)
            newCategory.IsActive = chkIsActive.Checked

            categoryBll.AddExpenseCategory(newCategory)

            ' عرض رسالة نجاح وتحديث الواجهة
            lblMessage.Text = "تم إضافة بند المصروف بنجاح."
            lblMessage.ForeColor = System.Drawing.Color.Green
            BindGrid()
            ClearForm()

        Catch ex As Exception
            lblMessage.Text = "خطأ: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

    Private Sub ClearForm()
        txtCategoryName.Text = ""
        ddlAccounts.SelectedIndex = 0
        chkIsActive.Checked = True
    End Sub

End Class