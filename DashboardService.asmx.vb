' DashboardService.asmx.vb
Imports System.Web.Services
Imports System.Web.Script.Services
Imports SuperMarket.BLL
Imports SuperMarket.Entities

<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<System.ComponentModel.ToolboxItem(False)>
<ScriptService()> ' << مهم جدًا
Public Class DashboardService
    Inherits System.Web.Services.WebService

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> ' << إرجاع البيانات بصيغة JSON
    Public Function GetSalesChartData() As Object
        Dim bll As New DashboardBLL()
        Dim salesData = bll.GetSalesForLast7Days()

        ' تجهيز البيانات بالتنسيق الذي سيفهمه JavaScript
        Dim labels = salesData.Select(Function(d) d.Label).ToArray()
        Dim values = salesData.Select(Function(d) d.Value).ToArray()

        ' إرجاع كائن مجهول (Anonymous Object) سيتم تحويله إلى JSON
        Return New With {Key .Labels = labels, Key .Data = values}
    End Function
    ' في DashboardService.asmx.vb

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function GetTopProductsChartData() As Object
        Dim bll As New DashboardBLL()
        Dim topProductsData = bll.GetTopSellingProducts(5) ' جلب أفضل 5 منتجات

        Dim labels = topProductsData.Select(Function(d) d.Label).ToArray()
        Dim values = topProductsData.Select(Function(d) d.Value).ToArray()

        Return New With {Key .Labels = labels, Key .Data = values}
    End Function
    ' في DashboardService.asmx.vb

    ' في DashboardService.asmx.vb

    <WebMethod(EnableSession:=True)>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function GetLiveSalesData() As List(Of SalesInvoice)
        Dim bll As New DashboardBLL()

        ' ==========================================================
        ' === الحل: استخدام تاريخ صالح كقيمة افتراضية ===
        ' ==========================================================
        Dim lastFetch As DateTime

        If HttpContext.Current.Session("LastFetchTime") IsNot Nothing Then
            ' إذا كانت هناك قيمة في الـ Session، استخدمها
            lastFetch = CType(HttpContext.Current.Session("LastFetchTime"), DateTime)
        Else
            ' إذا كانت هذه هي المرة الأولى، استخدم تاريخًا قديمًا جدًا وصالحًا
            ' أو ببساطة تاريخ اليوم مطروحًا منه يوم (لجلب كل فواتير اليوم عند أول تحميل)
            lastFetch = DateTime.Today.AddDays(-1)
            ' أو يمكنك استخدام تاريخ ثابت:
            ' lastFetch = New DateTime(2000, 1, 1)
        End If
        ' ==========================================================

        ' جلب الفواتير الجديدة فقط
        Dim newInvoices = bll.GetLiveSales(lastFetch)

        ' تحديث وقت آخر جلب إلى الوقت الحالي (بعد جلب البيانات)
        HttpContext.Current.Session("LastFetchTime") = DateTime.Now

        Return newInvoices
    End Function
End Class