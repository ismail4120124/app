' LedgerReport.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class LedgerReport
    Inherits BasePage

    Private accountBll As New AccountBLL()
    Private jvBll As New JournalVoucherBLL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            PopulateAccountsDropDown()
            ' تعيين تواريخ افتراضية (بداية الشهر الحالي حتى اليوم)
            txtStartDate.Text = New Date(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("yyyy-MM-dd")
            txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd")
        End If
    End Sub

    Private Sub PopulateAccountsDropDown()
        ' جلب الحسابات التحليلية فقط، لأنها هي التي عليها حركات
        Dim accounts = accountBll.GetAllAccounts().Where(Function(acc) acc.IsTransactional).ToList()

        ddlAccounts.DataSource = accounts
        ddlAccounts.DataTextField = "AccountName"
        ddlAccounts.DataValueField = "AccountID"
        ddlAccounts.DataBind()
        ddlAccounts.Items.Insert(0, New ListItem("-- اختر حسابًا --", "0"))
    End Sub

    Protected Sub btnShowReport_Click(sender As Object, e As EventArgs) Handles btnShowReport.Click
        lblMessage.Visible = False
        lblAccountName.Text = ""
        Try
            Dim accountId As Integer = Convert.ToInt32(ddlAccounts.SelectedValue)
            If accountId = 0 Then
                Throw New Exception("يجب اختيار حساب لعرض التقرير.")
            End If

            Dim startDate As DateTime = Convert.ToDateTime(txtStartDate.Text)
            Dim endDate As DateTime = Convert.ToDateTime(txtEndDate.Text)

            ' جلب البيانات من BLL
            Dim reportData = jvBll.GetLedgerForAccount(accountId, startDate, endDate)

            ' ربط البيانات بالـ GridView
            GridViewLedger.DataSource = reportData
            GridViewLedger.DataBind()

            ' عرض اسم الحساب المختار في العنوان
            lblAccountName.Text = ddlAccounts.SelectedItem.Text

        Catch ex As Exception
            lblMessage.Text = "خطأ: " & ex.Message
            lblMessage.Visible = True
            GridViewLedger.DataSource = Nothing
            GridViewLedger.DataBind()
        End Try
    End Sub

    ' دالة لحساب الإجماليات في تذييل الجدول (Footer)
    Private totalDebit As Decimal = 0
    Private totalCredit As Decimal = 0

    ' في LedgerReport.aspx.vb

    ' عرف المتغيرات على مستوى الكلاس ليتم تجميع القيم فيها


    Protected Sub GridViewLedger_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridViewLedger.RowDataBound
        ' أولاً، تحقق من أننا نتعامل مع صف بيانات (وليس Header أو Footer)
        If e.Row.RowType = DataControlRowType.DataRow Then

            ' ==========================================================
            ' === الحل: الوصول إلى مصدر البيانات مباشرة ===
            ' ==========================================================
            ' e.Row.DataItem هو الكائن الأصلي (من نوع LedgerEntry) الذي يرتبط بهذا الصف
            ' نقوم بتحويله إلى نوعه الصحيح
            Dim entry As LedgerEntry = TryCast(e.Row.DataItem, LedgerEntry)

            ' الآن، يمكننا الوصول إلى الخصائص الرقمية مباشرة دون مشاكل تحويل
            If entry IsNot Nothing Then
                totalDebit += entry.Debit
                totalCredit += entry.Credit
            End If
            ' ==========================================================

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            ' ثانيًا، عند الوصول إلى صف الفوتر، قم بعرض الإجماليات
            e.Row.Cells(1).Text = "الإجماليات"
            e.Row.Cells(2).Text = totalDebit.ToString("N2")
            e.Row.Cells(2).CssClass = "text-success fw-bold"
            e.Row.Cells(3).Text = totalCredit.ToString("N2")
            e.Row.Cells(3).CssClass = "text-danger fw-bold"
        End If
    End Sub

End Class