' ItemLedger.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class ItemLedger
    Inherits BasePage

    Private branchBll As New BranchBLL()
    Private productBll As New ProductBLL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' ملء قائمة الفروع
            BindBranches()
            ' تعيين تواريخ افتراضية (مثلاً، بداية الشهر الحالي ونهايته)
            txtStartDate.Text = New Date(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("yyyy-MM-dd")
            txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd")
        End If
    End Sub

    Private Sub BindBranches()
        ddlBranches.DataSource = branchBll.GetAllBranches()
        ddlBranches.DataTextField = "BranchName"
        ddlBranches.DataValueField = "BranchID"
        ddlBranches.DataBind()
        ddlBranches.Items.Insert(0, New ListItem("اختر الفرع...", "0"))
    End Sub

    Protected Sub btnShowReport_Click(sender As Object, e As EventArgs) Handles btnShowReport.Click
        lblMessage.Visible = False
        Try
            ' التحقق من المدخلات
            Dim branchId As Integer = Convert.ToInt32(ddlBranches.SelectedValue)
            Dim productId As Integer = 0
            Integer.TryParse(hdnSelectedProductID.Value, productId)

            If branchId = 0 OrElse productId = 0 Then
                Throw New Exception("يجب اختيار الفرع والمنتج.")
            End If

            Dim startDate As DateTime = Convert.ToDateTime(txtStartDate.Text)
            Dim endDate As DateTime = Convert.ToDateTime(txtEndDate.Text)

            ' جلب البيانات من BLL
            Dim reportData = productBll.GetItemLedger(productId, branchId, startDate, endDate)

            ' ربط البيانات بالـ GridView
            GridViewLedger.DataSource = reportData
            GridViewLedger.DataBind()

        Catch ex As Exception
            lblMessage.Text = "خطأ: " & ex.Message
            lblMessage.Visible = True
            ' إفراغ الجدول في حالة حدوث خطأ
            GridViewLedger.DataSource = Nothing
            GridViewLedger.DataBind()
        End Try
    End Sub

End Class