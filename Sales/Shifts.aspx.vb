' Shifts.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class Shifts
    Inherits BasePage ' نرث من BasePage للحصول على بيانات المستخدم

    Private shiftBll As New ShiftBLL()
    Private currentOpenShift As CashierShift ' لتخزين الوردية المفتوحة

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CheckUserShiftStatus()
        End If
    End Sub

    Private Sub CheckUserShiftStatus()
        ' البحث عن وردية مفتوحة للمستخدم الحالي
        currentOpenShift = shiftBll.GetOpenShiftForUser(CurrentUser.UserID, CurrentUser.BranchID)

        If currentOpenShift IsNot Nothing Then
            ' هناك وردية مفتوحة: أظهر لوحة الإغلاق
            PanelOpenShift.Visible = False
            PanelCloseShift.Visible = True
            PanelReport.Visible = False
            ' عرض بيانات الوردية المفتوحة
            litCashierName.Text = CurrentUser.FullName
            litOpeningTime.Text = currentOpenShift.OpeningTime.ToString("yyyy-MM-dd hh:mm tt")
            litOpeningBalance.Text = currentOpenShift.OpeningBalance.ToString("N2")
        Else
            ' لا توجد وردية مفتوحة: أظهر لوحة الفتح
            PanelOpenShift.Visible = True
            PanelCloseShift.Visible = False
            PanelReport.Visible = False
        End If
    End Sub

    Protected Sub btnOpenShift_Click(sender As Object, e As EventArgs) Handles btnOpenShift.Click
        Try
            Dim openingBalance As Decimal = 0
            Decimal.TryParse(txtOpeningBalance.Text, openingBalance)

            Dim newShift As New CashierShift()
            newShift.UserID = CurrentUser.UserID
            newShift.BranchID = CurrentUser.BranchID
            newShift.OpeningBalance = openingBalance

            shiftBll.OpenShift(newShift)

            lblMessage.Text = "تم فتح الوردية بنجاح."
            lblMessage.CssClass = "alert alert-success"
            lblMessage.Visible = True

            ' إعادة تحميل حالة الصفحة
            CheckUserShiftStatus()
        Catch ex As Exception
            lblMessage.Text = "خطأ: " & ex.Message
            lblMessage.CssClass = "alert alert-danger"
            lblMessage.Visible = True
        End Try
    End Sub

    ' في Shifts.aspx.vb

    Protected Sub btnCloseShift_Click(sender As Object, e As EventArgs) Handles btnCloseShift.Click
        Try
            ' --- الخطوة 1: جلب البيانات الأساسية ---
            ' جلب الوردية المفتوحة من قاعدة البيانات للتأكد من أنها لا تزال موجودة
            currentOpenShift = shiftBll.GetOpenShiftForUser(CurrentUser.UserID, CurrentUser.BranchID)
            If currentOpenShift Is Nothing Then
                Throw New Exception("لم يتم العثور على وردية مفتوحة لإغلاقها.")
            End If

            ' قراءة المبلغ الفعلي الذي أدخله المستخدم بأمان
            Dim actualClosingBalance As Decimal = 0
            Decimal.TryParse(txtClosingBalance.Text, actualClosingBalance)

            ' --- الخطوة 2: جلب ملخص المبيعات من قاعدة البيانات ---
            ' دالة GetShiftSalesSummary تُرجع كائنًا يحتوي على الإجماليات كـ Decimal
            Dim summary = shiftBll.GetShiftSalesSummary(currentOpenShift.ShiftID)
            If summary Is Nothing Then
                Throw New Exception("ไม่สามารถสรุปยอดขายของกะนี้ได้") ' لا يمكن تلخيص مبيعات هذه الوردية
            End If

            ' --- الخطوة 3: القيام بكل العمليات الحسابية على متغيرات Decimal ---
            Dim openingBalance As Decimal = currentOpenShift.OpeningBalance
            Dim totalCashSales As Decimal = summary.TotalCash
            Dim expectedCashInDrawer As Decimal = openingBalance + totalCashSales
            Dim difference As Decimal = actualClosingBalance - expectedCashInDrawer
            Dim totalCardSales As Decimal = summary.TotalCard
            Dim totalCreditSales As Decimal = summary.TotalCredit

            ' --- الخطوة 4: عرض النتائج المنسقة في الواجهة ---
            PanelCloseShift.Visible = False
            PanelReport.Visible = True

            litReportOpening.Text = openingBalance.ToString("N2")
            litReportCashSales.Text = totalCashSales.ToString("N2")
            litReportExpected.Text = expectedCashInDrawer.ToString("N2")
            litReportActual.Text = actualClosingBalance.ToString("N2")
            litReportDifference.Text = difference.ToString("N2")
            litReportCardSales.Text = totalCardSales.ToString("N2")
            litReportCreditSales.Text = totalCreditSales.ToString("N2")

            ' تلوين حقل الفرق بناءً على القيمة الرقمية
            If difference > 0 Then
                rowDifference.Attributes("class") = "table-success fw-bold" ' زيادة
            ElseIf difference < 0 Then
                rowDifference.Attributes("class") = "table-danger fw-bold" ' عجز
            Else
                rowDifference.Attributes("class") = "" ' عادي
            End If

            ' --- الخطوة 5: إغلاق الوردية في قاعدة البيانات ---
            shiftBll.CloseShift(currentOpenShift.ShiftID, actualClosingBalance, totalCashSales)

            lblMessage.Text = "تم إغلاق الوردية بنجاح وعرض التقرير."
            lblMessage.CssClass = "alert alert-success"
            lblMessage.Visible = True

        Catch ex As Exception
            lblMessage.Text = "خطأ أثناء إغلاق الوردية: " & ex.Message
            lblMessage.CssClass = "alert alert-danger"
            lblMessage.Visible = True
        End Try
    End Sub
End Class