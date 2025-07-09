' AccountingSettings.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

' تأكد من أن اسم الكلاس هنا يطابق خاصية "Inherits" في ملف .aspx
Public Class AccountingSettings
    Inherits BasePage

    Private settingsBll As New SettingsBLL()
    Private accountBll As New AccountBLL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            PopulateDropDowns()
            LoadSettings()
        End If
    End Sub

    Private Sub PopulateDropDowns()
        ' جلب الحسابات التحليلية فقط التي يمكن إجراء قيود عليها
        'Dim accounts = accountBll.GetAllAccounts().Where(Function(acc) acc.IsTransactional).ToList()
        Dim accounts = accountBll.GetAllAccounts().Where(
        Function(acc) acc.IsTransactional AndAlso
        (acc.AccountType = "مصاريف" Or acc.AccountType = "أصول" Or acc.AccountType = "إيرادات")
    ).ToList()
        ' === التعديل 1: إضافة ddlSuppliersAccount إلى مصفوفة القوائم المنسدلة ===
        Dim allDropDowns() As DropDownList = {
            ddlCashAccount, ddlBankAccount, ddlSalesAccount,
            ddlSalesReturnAccount, ddlPurchasesAccount, ddlInventoryAccount,
            ddlSuppliersAccount, ddlDebtorsAccount
        }

        For Each ddl As DropDownList In allDropDowns
            ddl.DataSource = accounts
            ddl.DataTextField = "AccountName"
            ddl.DataValueField = "AccountID"
            ddl.DataBind()
            ddl.Items.Insert(0, New ListItem("-- اختر الحساب --", "0"))
        Next
    End Sub

    Private Sub LoadSettings()
        Dim settings = settingsBll.GetAccountingSettings()
        If settings IsNot Nothing Then
            SetSelectedValue(ddlCashAccount, settings.DefaultCashAccountID)
            SetSelectedValue(ddlBankAccount, settings.DefaultBankAccountID)
            SetSelectedValue(ddlSalesAccount, settings.DefaultSalesAccountID)
            SetSelectedValue(ddlSalesReturnAccount, settings.DefaultSalesReturnAccountID)
            SetSelectedValue(ddlPurchasesAccount, settings.DefaultPurchasesAccountID)
            SetSelectedValue(ddlInventoryAccount, settings.DefaultInventoryAccountID)
            ' === التعديل 2: تحميل القيمة المحفوظة لحساب الموردين ===
            SetSelectedValue(ddlSuppliersAccount, settings.DefaultSuppliersAccountID)
            SetSelectedValue(ddlDebtorsAccount, settings.DefaultDebtorsAccountID)
        End If
    End Sub

    ' دالة مساعدة لتعيين القيمة المختارة بأمان
    Private Sub SetSelectedValue(ByVal ddl As DropDownList, ByVal value As Integer?)
        If value.HasValue AndAlso ddl.Items.FindByValue(value.Value.ToString()) IsNot Nothing Then
            ddl.SelectedValue = value.Value.ToString()
        Else
            ddl.SelectedIndex = 0 ' الرجوع إلى "-- اختر الحساب --" إذا لم تكن هناك قيمة
        End If
    End Sub

    Protected Sub btnSaveSettings_Click(sender As Object, e As EventArgs) Handles btnSaveSettings.Click
        Try
            ' === تصحيح مهم: استخدام اسم الكلاس الصحيح AccountingSettings ===
            Dim settings As New AccountingSettings1()

            settings.DefaultCashAccountID = If(ddlCashAccount.SelectedValue <> "0", CInt(ddlCashAccount.SelectedValue), CType(Nothing, Integer?))
            settings.DefaultBankAccountID = If(ddlBankAccount.SelectedValue <> "0", CInt(ddlBankAccount.SelectedValue), CType(Nothing, Integer?))
            settings.DefaultSalesAccountID = If(ddlSalesAccount.SelectedValue <> "0", CInt(ddlSalesAccount.SelectedValue), CType(Nothing, Integer?))
            settings.DefaultSalesReturnAccountID = If(ddlSalesReturnAccount.SelectedValue <> "0", CInt(ddlSalesReturnAccount.SelectedValue), CType(Nothing, Integer?))
            settings.DefaultPurchasesAccountID = If(ddlPurchasesAccount.SelectedValue <> "0", CInt(ddlPurchasesAccount.SelectedValue), CType(Nothing, Integer?))
            settings.DefaultInventoryAccountID = If(ddlInventoryAccount.SelectedValue <> "0", CInt(ddlInventoryAccount.SelectedValue), CType(Nothing, Integer?))
            ' === التعديل 3: حفظ القيمة المختارة لحساب الموردين ===
            settings.DefaultSuppliersAccountID = If(ddlSuppliersAccount.SelectedValue <> "0", CInt(ddlSuppliersAccount.SelectedValue), CType(Nothing, Integer?))
            settings.DefaultDebtorsAccountID = If(ddlDebtorsAccount.SelectedValue <> "0", CInt(ddlDebtorsAccount.SelectedValue), CType(Nothing, Integer?))

            settingsBll.SaveAccountingSettings(settings)

            lblMessage.Text = "تم حفظ الإعدادات بنجاح."
            lblMessage.ForeColor = System.Drawing.Color.Green

        Catch ex As Exception
            lblMessage.Text = "خطأ: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub
End Class