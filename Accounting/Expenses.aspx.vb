' Expenses.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class Expenses
    Inherits BasePage

    Private expenseBll As New ExpenseBLL()
    Private expenseCategoryBll As New ExpenseCategoryBLL()
    Private accountBll As New AccountBLL()
    Private branchBll As New BranchBLL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            PopulateDropDowns()
            txtExpenseDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
            BindExpensesGrid()
        End If
    End Sub
    ' === الدالة الجديدة لربط البيانات ===
    Private Sub BindExpensesGrid()
        GridViewExpenses.DataSource = expenseBll.GetRecentExpenses()
        GridViewExpenses.DataBind()
    End Sub
    Private Sub PopulateDropDowns()
        ' 1. تعبئة بنود المصاريف
        ddlExpenseCategory.DataSource = expenseCategoryBll.GetAllExpenseCategories().Where(Function(c) c.IsActive).ToList()
        ddlExpenseCategory.DataTextField = "CategoryName"
        ddlExpenseCategory.DataValueField = "CategoryID"
        ddlExpenseCategory.DataBind()
        ddlExpenseCategory.Items.Insert(0, New ListItem("-- اختر بند المصروف --", "0"))

        ' 2. تعبئة حسابات الدفع (الصندوق والبنوك)
        ' نجلب الحسابات التحليلية من نوع "أصول" فقط
        Dim cashAndBankAccounts = accountBll.GetAllAccounts().Where(
            Function(acc) acc.IsTransactional AndAlso acc.AccountType = "أصول" AndAlso (acc.AccountNumber.StartsWith("1101") Or acc.AccountNumber.StartsWith("1102"))
        ).ToList()

        ddlPaidFromAccount.DataSource = cashAndBankAccounts
        ddlPaidFromAccount.DataTextField = "AccountName"
        ddlPaidFromAccount.DataValueField = "AccountID"
        ddlPaidFromAccount.DataBind()
        ddlPaidFromAccount.Items.Insert(0, New ListItem("-- اختر حساب الدفع --", "0"))

        ' 3. تعبئة الفروع
        ddlBranch.DataSource = branchBll.GetAllBranches()
        ddlBranch.DataTextField = "BranchName"
        ddlBranch.DataValueField = "BranchID"
        ddlBranch.DataBind()
        ddlBranch.Items.Insert(0, New ListItem("-- مصروف عام (لا يخص فرع) --", "0"))
    End Sub

    Protected Sub btnSaveExpense_Click(sender As Object, e As EventArgs) Handles btnSaveExpense.Click
        Try
            ' 1. تجميع البيانات من النموذج
            Dim newExpense As New Expense()
            newExpense.ExpenseDate = Convert.ToDateTime(txtExpenseDate.Text)
            newExpense.Amount = Convert.ToDecimal(txtAmount.Text)
            newExpense.CategoryID = Convert.ToInt32(ddlExpenseCategory.SelectedValue)
            newExpense.Description = txtDescription.Text.Trim()
            newExpense.PaidFromAccountID = Convert.ToInt32(ddlPaidFromAccount.SelectedValue)

            If ddlBranch.SelectedValue <> "0" Then
                newExpense.BranchID = Convert.ToInt32(ddlBranch.SelectedValue)
            End If

            Dim currentUser As User = TryCast(Session("LoggedInUser"), User)
            If currentUser Is Nothing Then Throw New Exception("انتهت جلسة العمل.")
            newExpense.UserID = currentUser.UserID

            ' 2. استدعاء BLL لحفظ المصروف وإنشاء القيد
            expenseBll.SaveExpense(newExpense)

            ' 3. عرض رسالة نجاح وتحديث الواجهة
            lblMessage.Text = "تم حفظ المصروف بنجاح."
            lblMessage.ForeColor = System.Drawing.Color.Green
            ClearForm()
            ' يمكنك إضافة دالة لتحديث GridView هنا
            BindExpensesGrid()

        Catch ex As Exception
            lblMessage.Text = "خطأ: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

    Private Sub ClearForm()
        txtAmount.Text = ""
        txtDescription.Text = ""
        ddlExpenseCategory.SelectedIndex = 0
        ddlPaidFromAccount.SelectedIndex = 0
        ddlBranch.SelectedIndex = 0
    End Sub

End Class