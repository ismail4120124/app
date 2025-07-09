' TrialBalance.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class TrialBalance
    Inherits BasePage

    Private jvBll As New JournalVoucherBLL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtStartDate.Text = New Date(DateTime.Today.Year, 1, 1).ToString("yyyy-MM-dd") ' بداية السنة
            txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd")
        End If
    End Sub

    Protected Sub btnShowReport_Click(sender As Object, e As EventArgs) Handles btnShowReport.Click
        lblMessage.Visible = False
        Try
            Dim startDate As DateTime = Convert.ToDateTime(txtStartDate.Text)
            Dim endDate As DateTime = Convert.ToDateTime(txtEndDate.Text)
            Dim showOnlyTransactional As Boolean = chkOnlyTransactional.Checked

            Dim reportData = jvBll.GetTrialBalance(startDate, endDate, showOnlyTransactional)

            GridViewTrialBalance.DataSource = reportData
            GridViewTrialBalance.DataBind()

            ' حساب وعرض الإجماليات في الفوتر
            If reportData.Any() Then
                CalculateAndDisplayTotals(reportData)
            End If

        Catch ex As Exception
            lblMessage.Text = "خطأ: " & ex.Message
            lblMessage.Visible = True
            GridViewTrialBalance.DataSource = Nothing
            GridViewTrialBalance.DataBind()
        End Try
    End Sub

    Private Sub CalculateAndDisplayTotals(ByVal data As List(Of TrialBalanceEntry))
        ' التأكد من وجود صف الفوتر
        If GridViewTrialBalance.FooterRow IsNot Nothing Then
            Dim footer As GridViewRow = GridViewTrialBalance.FooterRow

            ' حساب الإجماليات
            Dim totalOpeningDebit = data.Where(Function(d) d.OpeningBalance > 0).Sum(Function(d) d.OpeningBalance)
            Dim totalOpeningCredit = Math.Abs(data.Where(Function(d) d.OpeningBalance < 0).Sum(Function(d) d.OpeningBalance))

            Dim totalPeriodDebit = data.Sum(Function(d) d.PeriodDebit)
            Dim totalPeriodCredit = data.Sum(Function(d) d.PeriodCredit)

            Dim totalClosingDebit = data.Where(Function(d) d.ClosingBalance > 0).Sum(Function(d) d.ClosingBalance)
            Dim totalClosingCredit = Math.Abs(data.Where(Function(d) d.ClosingBalance < 0).Sum(Function(d) d.ClosingBalance))

            ' عرض الإجماليات في خلايا الفوتر
            footer.Cells(1).Text = "الإجماليات"
            footer.Cells(2).Text = totalOpeningDebit.ToString("N2")
            footer.Cells(3).Text = totalOpeningCredit.ToString("N2")
            footer.Cells(4).Text = totalPeriodDebit.ToString("N2")
            footer.Cells(5).Text = totalPeriodCredit.ToString("N2")
            footer.Cells(6).Text = totalClosingDebit.ToString("N2")
            footer.Cells(7).Text = totalClosingCredit.ToString("N2")

            ' التحقق من توازن الميزان وتلوين الخلايا
            If totalClosingDebit <> totalClosingCredit Then
                footer.Cells(6).CssClass = "bg-danger text-white"
                footer.Cells(7).CssClass = "bg-danger text-white"
            End If
        End If
    End Sub

End Class