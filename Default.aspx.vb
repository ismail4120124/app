' Default.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class _Default
    Inherits Page

    Private bll As New DashboardBLL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' التحقق من تسجيل الدخول (يجب أن يكون موجودًا في MasterPage، لكن نضعه هنا للتأكيد)
        If Session("LoggedInUser") Is Nothing Then
            Response.Redirect("~/Login.aspx")
        End If

        If Not IsPostBack Then
            LoadDashboardData()
        End If
    End Sub

    Private Sub LoadDashboardData()
        Try
            ' استدعاء BLL لجلب كل الإحصائيات
            Dim stats As DashboardStats = bll.GetDashboardStats()

            ' تعبئة البطاقات بالبيانات
            litDailySales.Text = stats.TodaysSalesTotal.ToString("N2")
            litInvoiceCount.Text = stats.TodaysInvoiceCount.ToString()
            litTotalProducts.Text = stats.TotalProducts.ToString()
            litTotalCustomers.Text = stats.TotalCustomers.ToString()
            ' جلب تنبيهات حد إعادة الطلب
            Dim reorderAlerts = bll.GetReorderLevelAlerts()
            GridViewReorderAlerts.DataSource = reorderAlerts
            GridViewReorderAlerts.DataBind()

            ' جلب تنبيهات انتهاء الصلاحية (للأصناف التي تنتهي خلال 30 يومًا)
            Dim expiryAlerts = bll.GetExpiryDateAlerts(30)
            GridViewExpiryAlerts.DataSource = expiryAlerts
            GridViewExpiryAlerts.DataBind()

        Catch ex As Exception
            ' في حالة حدوث خطأ، يمكننا عرضه للمستخدم
            ' على سبيل المثال، في Label مخصص للخطأ
            ' lblError.Text = "حدث خطأ أثناء تحميل بيانات لوحة التحكم."
            ' lblError.Visible = True

            ' حاليًا، سنقوم فقط بتعيين القيم الافتراضية
            litDailySales.Text = "خطأ"
            litInvoiceCount.Text = "خطأ"
            litTotalProducts.Text = "خطأ"
            litTotalCustomers.Text = "خطأ"
        End Try
    End Sub

End Class