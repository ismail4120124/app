<%@ Page Title="مراقبة المبيعات المباشرة" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="LiveMonitor1.aspx.vb" Inherits="SuperMarket.WebUI.LiveMonitor1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h4 class="mb-0">المبيعات المباشرة</h4>
        <span id="statusIndicator" class="badge bg-secondary">متصل...</span>
    </div>
    
    <div class="card shadow-sm">
        <div class="card-body">
            <table class="table table-striped table-hover">
                <thead class="table-dark">
                    <tr>
                        <th>رقم الفاتورة</th>
                        <th>الفرع</th>
                        <th>الكاشير</th>
                        <th>المبلغ</th>
                    </tr>
                </thead>
                <tbody id="liveSalesTbody">
                    <%-- سيتم تعبئة الصفوف هنا باستخدام JavaScript --%>
                </tbody>
            </table>
        </div>
    </div>

    <%-- عنصر الصوت لتشغيل التنبيه --%>
    <audio id="notificationSound" src="path/to/your/notification.mp3" preload="auto"></audio>

    <%-- jQuery لتسهيل التعديل على DOM --%>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            // دالة لجلب البيانات الجديدة
            function fetchLatestSales() {
                $('#statusIndicator').text('جاري التحديث...').removeClass('bg-success').addClass('bg-warning');

                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("~/DashboardService.asmx/GetLiveSalesData") %>',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        const newInvoices = response.d;
                        if (newInvoices.length > 0) {
                            // تشغيل صوت التنبيه
                            // $('#notificationSound')[0].play();

                            // إضافة الصفوف الجديدة إلى أعلى الجدول مع تأثير
                            newInvoices.forEach(function (invoice) {
                                const newRow = `
                                    <tr style="display:none;">
                                        <td>${invoice.InvoiceID}</td>
                                        <td>${invoice.BranchName}</td>
                                        <td>${invoice.UserName}</td>
                                        <td class="fw-bold">${invoice.FinalAmount.toFixed(2)}</td>
                                    </tr>`;
                                $('#liveSalesTbody').prepend(newRow);
                                $('#liveSalesTbody tr:first').fadeIn(1000); // تأثير ظهور لطيف
                            });
                        }
                        $('#statusIndicator').text('متصل').removeClass('bg-warning').addClass('bg-success');
                    },
                    error: function (xhr, status, error) {
                        console.error("Error fetching live sales:", error);
                        $('#statusIndicator').text('خطأ في الاتصال').removeClass('bg-success').addClass('bg-danger');
                    }
                });
            }

            // جلب البيانات لأول مرة عند تحميل الصفحة
            fetchLatestSales();

            // تشغيل الدالة كل 15 ثانية
            setInterval(fetchLatestSales, 15000);
        });
    </script>
</asp:Content>