' SuperMarket.BLL/DashboardBLL.vb
Imports SuperMarket.DAL
Imports SuperMarket.Entities

Public Class DashboardBLL
    Private dal As New DashboardDAL()

    ''' <summary>
    ''' دالة وسيطة لجلب إحصائيات لوحة التحكم.
    ''' </summary>
    Public Function GetDashboardStats() As DashboardStats
        ' حاليًا لا يوجد منطق عمل إضافي، فقط نستدعي DAL
        ' في المستقبل، يمكن إضافة صلاحيات هنا (مثلاً، مدير الفرع يرى إحصائيات فرعه فقط)
        Return dal.GetDashboardStats()
    End Function
    ' في DashboardBLL.vb

    Public Function GetSalesForLast7Days() As List(Of ChartDataPoint)
        Return dal.GetSalesForLast7Days()
    End Function
    ' في DashboardBLL.vb

    Public Function GetTopSellingProducts(ByVal topCount As Integer) As List(Of ChartDataPoint)
        If topCount <= 0 Then topCount = 5 ' قيمة افتراضية
        Return dal.GetTopSellingProducts(topCount)
    End Function
    Public Function GetReorderLevelAlerts() As List(Of InventoryAlert)
        Return dal.GetReorderLevelAlerts()
    End Function

    Public Function GetExpiryDateAlerts(Optional ByVal daysThreshold As Integer = 30) As List(Of InventoryAlert)
        Return dal.GetExpiryDateAlerts(daysThreshold)
    End Function
    Public Function GetLiveSales(ByVal lastFetchTime As DateTime) As List(Of SalesInvoice)
        Return dal.GetLiveSales(lastFetchTime)
    End Function
End Class
