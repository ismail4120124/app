<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PrintWholesaleInvoice.aspx.vb" Inherits="SuperMarket.WebUI.PrintWholesaleInvoice" %>

<!DOCTYPE html>
<html lang="ar" dir="rtl">
<head runat="server">
    <title>فاتورة بيع - <asp:Literal runat="server" ID="litInvoiceIDHeader" /></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.rtl.min.css" rel="stylesheet">
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Cairo:wght@400;700&display=swap" rel="stylesheet">
    <style>
        body {
            background-color: #fff;
            font-family: 'Cairo', sans-serif;
            margin: 0;
            padding: 0;
            height: 100%;
        }
        .invoice-container {
            width: 21cm;
            min-height: 29.7cm;
            margin: 0 auto;
            padding: 20mm 25mm;
            background-color: #ffffff;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            position: relative;
        }
        .invoice-header {
            border-bottom: 2px solid #007bff;
            padding-bottom: 10px;
            margin-bottom: 20px;
        }
        .logo {
            max-height: 100px;
            max-width: 250px;
        }
        .invoice-title {
            color: #007bff;
            font-weight: 700;
            font-size: 2rem;
        }
        .company-info, .customer-info {
            font-size: 0.95rem;
            color: #333;
        }
        .company-info p, .customer-info p {
            margin: 5px 0;
        }
        .table {
            width: 100%;
            border-collapse: collapse;
            font-size: 0.95rem;
        }
        .table thead th {
            background-color: #007bff;
            color: #fff;
            border-color: #0056b3;
            padding: 12px;
            text-align: center;
        }
        .table tbody td {
            padding: 10px;
            border: 1px solid #dee2e6;
            text-align: right;
        }
        .table tbody tr:nth-child(even) {
            background-color: #f8f9fa;
        }
        .totals-table {
            width: 40%;
            float: right;
            margin-top: 20px;
            font-size: 1.1rem;
        }
        .totals-table th, .totals-table td {
            padding: 10px;
            border: 1px solid #dee2e6;
            text-align: right;
        }
        .totals-table tr:not(.grand-total) th,
        .totals-table tr:not(.grand-total) td {
            font-size: 0.9rem; /* تقليص حجم الخط للإجمالي الفرعي، الضريبة، والخصم */
        }
        .grand-total {
            background-color: #e9f0f9;
            font-weight: 700;
            font-size: 1.2rem;
        }
        .invoice-footer {
            position: absolute;
            bottom: 20mm;
            width: 100%;
            text-align: center;
            color: #6c757d;
            font-size: 0.9rem;
            border-top: 1px solid #dee2e6;
            padding-top: 10px;
        }
        .signature-section {
            margin-top: 20px;
            text-align: center;
            font-size: 0.9rem;
        }
        .signature-section p {
            margin: 5px 0;
        }
        .no-print {
            display: none;
        }
        @media print {
            body { background-color: #fff; }
            .invoice-container {
                box-shadow: none;
                border: none;
                margin: 0;
                padding: 20mm 25mm;
                width: 21cm;
                height: 29.7cm;
            }
            .no-print { display: none !important; }
            @page {
                size: A4;
                margin: 0;
            }
            html, body {
                height: 100%;
                margin: 0;
                padding: 0;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="text-center my-3 no-print">
            <asp:Button ID="btnPrint" runat="server" OnClientClick="window.print(); return false;" Text="طباعة الفاتورة" CssClass="btn btn-primary px-5" />
        </div>

        <div class="invoice-container">
            <!-- Header -->
            <header class="invoice-header row align-items-center mb-4">
                <div class="col-4">
                    <img src="https://via.placeholder.com/250x100.png?text=شعار+الشركة" alt="شعار الشركة" class="logo img-fluid" />
                </div>
                <div class="col-4 text-center">
                    <h1 class="invoice-title">فاتورة بيع</h1>
                    <p class="company-info"><strong>رقم الفاتورة:</strong> <asp:Literal ID="litInvoiceID" runat="server" /></p>
                    <p class="company-info"><strong>التاريخ:</strong> <asp:Literal ID="litInvoiceDate" runat="server" /></p>
                </div>
                <div class="col-4 text-start">
                    <div class="company-info">
                        <h5><asp:Literal ID="litCompanyName" runat="server">سوبرماركت برو</asp:Literal></h5>
                        <p><asp:Literal ID="litBranchName" runat="server">الفرع الرئيسي</asp:Literal></p>
                        <p>هاتف: 1234-567-890 | البريد: info@supermarketpro.com</p>
                    </div>
                </div>
            </header>

            <!-- Customer & Seller Info -->
            <section class="row mb-4">
                <div class="col-6">
                    <div class="p-3 bg-light rounded">
                        <h6>فاتورة إلى:</h6>
                        <p><strong>الاسم:</strong> <asp:Literal ID="litCustomerName" runat="server" /></p>
                        <p><strong>الهاتف:</strong> <asp:Literal ID="litCustomerPhone" runat="server" /></p>
                        <p><strong>العنوان:</strong> <asp:Literal ID="litCustomerAddress" runat="server" /></p>
                    </div>
                </div>
                <div class="col-6">
                    <div class="p-3 bg-light rounded">
                        <h6>البائع المسؤول:</h6>
                        <p><strong>الاسم:</strong> <asp:Literal ID="litCashierName" runat="server" /></p>
                        <p><strong>التوقيت:</strong> <asp:Literal ID="litCashierTime" runat="server" /></p>
                    </div>
                </div>
            </section>

            <!-- Items Table -->
            <section class="mb-4">
                <asp:GridView ID="GridViewItems" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-striped table-hover">
                    <HeaderStyle CssClass="table-dark" />
                    <Columns>
                        <asp:BoundField DataField="ProductName" HeaderText="اسم الصنف" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Quantity" HeaderText="الكمية" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="UnitPrice" HeaderText="سعر الوحدة" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="SubTotal" HeaderText="الإجمالي" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
            </section>

            <!-- Totals Section -->
            <section class="row justify-content-end">
                <div class="col-md-5">
                    <table class="table totals-table">
                        <tbody>
                            <tr>
                                <th>الإجمالي الفرعي</th>
                                <td><asp:Literal ID="litTotal" runat="server" /></td>
                            </tr>
                            <tr>
                                <th>الضريبة (إن وجدت)</th>
                                <td><asp:Literal ID="litTax" runat="server">0.00</asp:Literal></td>
                            </tr>
                            <tr>
                                <th>الخصم</th>
                                <td><asp:Literal ID="litDiscount" runat="server" /></td>
                            </tr>
                            <tr class="grand-total">
                                <th>المبلغ الإجمالي المطلوب</th>
                                <td><asp:Literal ID="litGrandTotal" runat="server" /></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </section>

            <!-- Signature Section -->
            <section class="signature-section">
                <p>توقيع العميل: _________________________</p>
                <p>توقيع البائع: _________________________</p>
                <p>ملاحظات: <asp:Literal ID="litNotes" runat="server" /></p>
            </section>

            <!-- Footer -->
            <footer class="invoice-footer">
                <p>شكرًا لتعاملكم مع سوبرماركت برو | للاستفسارات: info@supermarketpro.com |<br /> © 2025 جميع الحقوق محفوظة</p>
            </footer>
        </div>
    </form>
</body>
</html>