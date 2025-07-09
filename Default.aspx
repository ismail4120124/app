<%-- Default.aspx --%>
<%@ Page Title="لوحة التحكم" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="Default.aspx.vb" Inherits="SuperMarket.WebUI._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header mb-3">
        <h3>لوحة التحكم الرئيسية</h3>
    </div>

    <!-- ====================================================== -->
    <!-- صف البطاقات الإحصائية                                   -->
    <!-- ====================================================== -->
    <div class="row g-3 d-flex">
        <%-- بطاقة مبيعات اليوم --%>
        <div class="col-xl-3 col-md-6">
            <div class="card bg-primary text-white shadow h-100">
                <div class="card-body d-flex justify-content-between align-items-center">
                    <div>
                        <div class="text-white-75 small">إجمالي مبيعات اليوم</div>
                        <div class="fs-4 fw-bold">
                            <asp:Literal ID="litDailySales" runat="server">0.00</asp:Literal>
                        </div>
                    </div>
                    <i class="fa fa-dollar-sign fa-3x text-white-50"></i>
                </div>
            </div>
        </div>

        <%-- بطاقة عدد فواتير اليوم --%>
        <div class="col-xl-3 col-md-6">
            <div class="card bg-warning text-white shadow h-100">
                <div class="card-body d-flex justify-content-between align-items-center">
                    <div>
                        <div class="text-white-75 small">عدد فواتير اليوم</div>
                        <div class="fs-4 fw-bold">
                            <asp:Literal ID="litInvoiceCount" runat="server">0</asp:Literal>
                        </div>
                    </div>
                    <i class="fa fa-file-invoice fa-3x text-white-50"></i>
                </div>
            </div>
        </div>

        <%-- بطاقة إجمالي الأصناف --%>
        <div class="col-xl-3 col-md-6">
            <div class="card bg-success text-white shadow h-100">
                <div class="card-body d-flex justify-content-between align-items-center">
                    <div>
                        <div class="text-white-75 small">إجمالي الأصناف</div>
                        <div class="fs-4 fw-bold">
                            <asp:Literal ID="litTotalProducts" runat="server">0</asp:Literal>
                        </div>
                    </div>
                    <i class="fa fa-box fa-3x text-white-50"></i>
                </div>
            </div>
        </div>

        <%-- بطاقة إجمالي العملاء --%>
        <div class="col-xl-3 col-md-6">
            <div class="card bg-danger text-white shadow h-100">
                <div class="card-body d-flex justify-content-between align-items-center">
                    <div>
                        <div class="text-white-75 small">إجمالي العملاء</div>
                        <div class="fs-4 fw-bold">
                            <asp:Literal ID="litTotalCustomers" runat="server">0</asp:Literal>
                        </div>
                    </div>
                    <i class="fa fa-users fa-3x text-white-50"></i>
                </div>
            </div>
        </div>
    </div>
    
    <!-- ====================================================== -->
    <!-- صف الرسوم البيانية                                      -->
    <!-- ====================================================== -->
    <div class="row mt-4 g-3 d-flex">
        <!-- الرسم البياني الخطي للمبيعات -->
        <div class="col-lg-7">
            <div class="card shadow-sm h-100">
                <div class="card-header"><h5 class="mb-0">المبيعات خلال آخر 7 أيام</h5></div>
                <div class="card-body">
                    <canvas id="salesChart"></canvas>
                </div>
            </div>
        </div>
        <!-- الرسم البياني الدائري لأفضل المنتجات -->
        <div class="col-lg-5">
            <div class="card shadow-sm h-100">
                <div class="card-header"><h5 class="mb-0">أفضل 5 منتجات مبيعًا</h5></div>
                <div class="card-body d-flex justify-content-center align-items-center">
                    <canvas id="topProductsChart" style="max-height: 350px;"></canvas>
                </div>
            </div>
        </div>
    </div>
    <!-- ====================================================== -->
<!-- صف تنبيهات المخزون                                      -->
<!-- ====================================================== -->
<div class="row mt-4 g-3">
    <!-- بطاقة تنبيهات حد إعادة الطلب -->
    <div class="col-lg-6">
        <div class="card shadow-sm">
            <div class="card-header bg-danger text-white">
                <h5 class="mb-0"><i class="fa fa-exclamation-triangle"></i> تنبيهات: أصناف وصلت لحد الطلب</h5>
            </div>
            <div class="card-body p-0" style="max-height: 300px; overflow-y: auto;">
                <asp:GridView ID="GridViewReorderAlerts" runat="server"
                    CssClass="table table-striped table-hover mb-0"
                    AutoGenerateColumns="False"
                    EmptyDataText="<div class='text-center p-3 text-muted'>لا توجد تنبيهات حاليًا.</div>">
                    <HeaderStyle CssClass="bg-light" />
                    <Columns>
                        <asp:BoundField DataField="ProductName" HeaderText="الصنف" />
                        <asp:BoundField DataField="BranchName" HeaderText="الفرع" />
                        <asp:BoundField DataField="CurrentQuantity" HeaderText="الكمية الحالية" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="ReorderLevel" HeaderText="حد الطلب" DataFormatString="{0:N2}" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <!-- بطاقة تنبيهات انتهاء الصلاحية -->
    <div class="col-lg-6">
        <div class="card shadow-sm">
            <div class="card-header bg-warning text-dark">
                 <h5 class="mb-0"><i class="fa fa-calendar-times"></i> تنبيهات: أصناف قريبة من انتهاء الصلاحية</h5>
            </div>
             <div class="card-body p-0" style="max-height: 300px; overflow-y: auto;">
                <asp:GridView ID="GridViewExpiryAlerts" runat="server"
                    CssClass="table table-striped table-hover mb-0"
                    AutoGenerateColumns="False"
                    EmptyDataText="<div class='text-center p-3 text-muted'>لا توجد تنبيهات حاليًا.</div>">
                    <HeaderStyle CssClass="bg-light" />
                    <Columns>
                        <asp:BoundField DataField="ProductName" HeaderText="الصنف" />
                        <asp:BoundField DataField="BranchName" HeaderText="الفرع" />
                        <asp:BoundField DataField="ExpiryDate" HeaderText="تاريخ الانتهاء" DataFormatString="{0:yyyy-MM-dd}" />
                        <asp:BoundField DataField="CurrentQuantity" HeaderText="الكمية المتبقية" DataFormatString="{0:N2}" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</div>

    <!-- تأكد من وجود رابط FontAwesome (عادة يكون في MasterPage أو أضفه هنا) -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" />

    <!-- ====================================================== -->
    <!-- قسم JavaScript                                          -->
    <!-- ====================================================== -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {
            // --- 1. كود الرسم البياني الأول (الخطي) ---
            fetch('DashboardService.asmx/GetSalesChartData', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            })
                .then(response => response.json())
                .then(data => {
                    const chartData = data.d;
                    const ctx = document.getElementById('salesChart').getContext('2d');
                    new Chart(ctx, {
                        type: 'line',
                        data: {
                            labels: chartData.Labels,
                            datasets: [{
                                label: 'إجمالي المبيعات',
                                data: chartData.Data,
                                borderColor: 'rgb(54, 162, 235)',
                                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                                fill: true,
                                tension: 0.2
                            }]
                        },
                        options: {
                            responsive: true,
                            maintainAspectRatio: false,
                            scales: { y: { beginAtZero: true } }
                        }
                    });
                })
                .catch(error => console.error('Error fetching sales chart data:', error));

            // --- 2. كود الرسم البياني الثاني (الدائري) ---
            fetch('DashboardService.asmx/GetTopProductsChartData', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            })
                .then(response => response.json())
                .then(data => {
                    const chartData = data.d;
                    const ctx = document.getElementById('topProductsChart').getContext('2d');
                    new Chart(ctx, {
                        type: 'doughnut',
                        data: {
                            labels: chartData.Labels,
                            datasets: [{
                                label: 'قيمة المبيعات',
                                data: chartData.Data,
                                backgroundColor: [
                                    'rgba(255, 99, 132, 0.9)',
                                    'rgba(54, 162, 235, 0.9)',
                                    'rgba(255, 206, 86, 0.9)',
                                    'rgba(75, 192, 192, 0.9)',
                                    'rgba(153, 102, 255, 0.9)'
                                ],
                                borderColor: '#fff',
                                borderWidth: 2,
                                hoverOffset: 4
                            }]
                        },
                        options: {
                            responsive: true,
                            maintainAspectRatio: false,
                            plugins: { legend: { position: 'bottom' } }
                        }
                    });
                })
                .catch(error => console.error('Error fetching top products data:', error));
        });
    </script>
</asp:Content>