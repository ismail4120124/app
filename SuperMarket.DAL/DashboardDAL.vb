' SuperMarket.DAL/DashboardDAL.vb
Imports System.Data.SqlClient
Imports System.Configuration
Imports SuperMarket.Entities

Public Class DashboardDAL
    Private ReadOnly _connectionString As String = ConfigurationManager.ConnectionStrings("SuperMarketDB").ConnectionString

    ''' <summary>
    ''' يجلب الإحصائيات الرئيسية للوحة التحكم في استعلام واحد.
    ''' </summary>
    Public Function GetDashboardStats() As DashboardStats
        Dim stats As New DashboardStats()

        Using conn As New SqlConnection(_connectionString)
            ' استعلام واحد يجمع كل الإحصائيات المطلوبة
            Dim query As String = "
                SELECT 
                    (SELECT ISNULL(SUM(FinalAmount), 0) FROM SalesInvoices WHERE CAST(InvoiceDate AS DATE) = CAST(GETDATE() AS DATE)) AS TodaysSales,
                    (SELECT COUNT(InvoiceID) FROM SalesInvoices WHERE CAST(InvoiceDate AS DATE) = CAST(GETDATE() AS DATE)) AS TodaysInvoices,
                    (SELECT COUNT(ProductID) FROM Products) AS TotalProducts,
                    (SELECT COUNT(CustomerID) FROM Customers WHERE CustomerID <> 1) AS TotalCustomers;
            "

            Dim cmd As New SqlCommand(query, conn)
            conn.Open()
            Using reader As SqlDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    stats.TodaysSalesTotal = Convert.ToDecimal(reader("TodaysSales"))
                    stats.TodaysInvoiceCount = Convert.ToInt32(reader("TodaysInvoices"))
                    stats.TotalProducts = Convert.ToInt32(reader("TotalProducts"))
                    stats.TotalCustomers = Convert.ToInt32(reader("TotalCustomers"))
                End If
            End Using
        End Using

        Return stats
    End Function
    ' في DashboardDAL.vb

    Public Function GetSalesForLast7Days() As List(Of ChartDataPoint)
        Dim dataPoints As New List(Of ChartDataPoint)()
        Using conn As New SqlConnection(_connectionString)
            ' هذا الاستعلام يستخدم دالة خاصة بـ SQL Server لجلب آخر 7 أيام
            Dim query As String = "
            WITH Dates AS (
                SELECT CAST(GETDATE() AS DATE) AS DateVal
                UNION ALL SELECT CAST(DATEADD(day, -1, GETDATE()) AS DATE)
                UNION ALL SELECT CAST(DATEADD(day, -2, GETDATE()) AS DATE)
                UNION ALL SELECT CAST(DATEADD(day, -3, GETDATE()) AS DATE)
                UNION ALL SELECT CAST(DATEADD(day, -4, GETDATE()) AS DATE)
                UNION ALL SELECT CAST(DATEADD(day, -5, GETDATE()) AS DATE)
                UNION ALL SELECT CAST(DATEADD(day, -6, GETDATE()) AS DATE)
            )
            SELECT 
                FORMAT(d.DateVal, 'yyyy-MM-dd') AS ChartLabel,
                ISNULL(SUM(s.FinalAmount), 0) AS ChartValue
            FROM Dates d
            LEFT JOIN SalesInvoices s ON CAST(s.InvoiceDate AS DATE) = d.DateVal
            GROUP BY d.DateVal
            ORDER BY d.DateVal;
        "
            Dim cmd As New SqlCommand(query, conn)
            conn.Open()
            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    dataPoints.Add(New ChartDataPoint With {
                        .Label = reader("ChartLabel").ToString(),
                        .Value = Convert.ToDecimal(reader("ChartValue"))
                    })
                End While
            End Using
        End Using
        Return dataPoints
    End Function
    ' أضف هذه الدالة داخل كلاس DashboardDAL.vb

    Public Function GetTopSellingProducts(ByVal topCount As Integer) As List(Of ChartDataPoint)
        Dim dataPoints As New List(Of ChartDataPoint)()
        Using conn As New SqlConnection(_connectionString)
            ' هذا الاستعلام يجمع مبيعات كل منتج، يرتبها تنازليًا، ثم يأخذ أفضل 5
            Dim query As String = "
            SELECT TOP (@TopCount)
                p.ProductName AS ChartLabel,
                SUM(d.SubTotal) AS ChartValue
            FROM SalesInvoiceDetails d
            JOIN Products p ON d.ProductID = p.ProductID
            GROUP BY p.ProductName
            ORDER BY ChartValue DESC;
        "
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@TopCount", topCount)
            conn.Open()
            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    dataPoints.Add(New ChartDataPoint With {
                        .Label = reader("ChartLabel").ToString(),
                        .Value = Convert.ToDecimal(reader("ChartValue"))
                    })
                End While
            End Using
        End Using
        Return dataPoints
    End Function
    Public Function GetReorderLevelAlerts() As List(Of InventoryAlert)
        Dim alerts As New List(Of InventoryAlert)()
        Using conn As New SqlConnection(_connectionString)
            Dim query As String = "
            SELECT p.ProductName, b.BranchName, bi.Quantity, p.ReorderLevel
            FROM BranchInventory bi
            JOIN Products p ON bi.ProductID = p.ProductID
            JOIN Branches b ON bi.BranchID = b.BranchID
            WHERE bi.Quantity <= p.ReorderLevel AND p.ReorderLevel > 0"

            Dim cmd As New SqlCommand(query, conn)
            conn.Open()
            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    alerts.Add(New InventoryAlert With {
                        .ProductName = reader("ProductName").ToString(),
                        .BranchName = reader("BranchName").ToString(),
                        .CurrentQuantity = Convert.ToDecimal(reader("Quantity")),
                        .ReorderLevel = Convert.ToDecimal(reader("ReorderLevel"))
                    })
                End While
            End Using
        End Using
        Return alerts
    End Function

    ' دالة جلب تنبيهات انتهاء الصلاحية
    Public Function GetExpiryDateAlerts(ByVal daysThreshold As Integer) As List(Of InventoryAlert)
        Dim alerts As New List(Of InventoryAlert)()
        Using conn As New SqlConnection(_connectionString)
            Dim query As String = "
            SELECT p.ProductName, b.BranchName, bi.Quantity, bi.ExpiryDate
            FROM BranchInventory bi
            JOIN Products p ON bi.ProductID = p.ProductID
            JOIN Branches b ON bi.BranchID = b.BranchID
            WHERE bi.ExpiryDate IS NOT NULL 
              AND bi.ExpiryDate BETWEEN GETDATE() AND DATEADD(day, @Days, GETDATE())"

            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@Days", daysThreshold)
            conn.Open()
            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    alerts.Add(New InventoryAlert With {
                        .ProductName = reader("ProductName").ToString(),
                        .BranchName = reader("BranchName").ToString(),
                        .CurrentQuantity = Convert.ToDecimal(reader("Quantity")),
                        .ExpiryDate = Convert.ToDateTime(reader("ExpiryDate"))
                    })
                End While
            End Using
        End Using
        Return alerts
    End Function
    ''' <summary>
    ''' يجلب آخر فواتير المبيعات التي تمت بعد وقت محدد.
    ''' </summary>
    Public Function GetLiveSales(ByVal lastFetchTime As DateTime) As List(Of SalesInvoice)
        Dim newInvoices As New List(Of SalesInvoice)()
        Using conn As New SqlConnection(_connectionString)
            ' نجلب الفواتير التي تاريخها أكبر من آخر تاريخ جلب
            Dim query As String = "
            SELECT i.InvoiceID, i.FinalAmount, b.BranchName, u.FullName AS CashierName
            FROM SalesInvoices i
            JOIN Branches b ON i.BranchID = b.BranchID
            JOIN Users u ON i.UserID = u.UserID
            WHERE i.InvoiceDate > @LastFetchTime
            ORDER BY i.InvoiceID DESC"

            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@LastFetchTime", lastFetchTime)
            conn.Open()

            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    newInvoices.Add(New SalesInvoice With {
                        .InvoiceID = Convert.ToInt64(reader("InvoiceID")),
                        .FinalAmount = Convert.ToDecimal(reader("FinalAmount")),
                        .BranchName = reader("BranchName").ToString(),
                        .UserName = reader("CashierName").ToString()
                    })
                End While
            End Using
        End Using
        Return newInvoices
    End Function
End Class
