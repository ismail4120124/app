<%-- PrintInvoice.aspx --%>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PrintInvoice.aspx.vb" Inherits="SuperMarket.WebUI.PrintInvoice" %>

<!DOCTYPE html>
<html lang="ar" dir="rtl">
<head runat="server">
    <title>طباعة فاتورة</title>
    <style>
        body { font-family: 'Courier New', Courier, monospace; font-size: 12px; }
        .invoice-box { width: 300px; margin: auto; padding: 10px; border: 1px solid #eee; box-shadow: 0 0 10px rgba(0, 0, 0, 0.15); }
        .text-center { text-align: center; }
        .header-info, .total-info { margin-bottom: 10px; }
        .items-table { width: 100%; border-collapse: collapse; }
        .items-table th, .items-table td { border-bottom: 1px dotted #ccc; padding: 5px; text-align: right; }
        .items-table .qty, .items-table .price, .items-table .total { text-align: left; }
        .footer { margin-top: 15px; border-top: 1px dashed #000; padding-top: 5px; }

        @media print {
            body, .invoice-box { margin: 0; border: 0; box-shadow: none; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="invoice-box">
            <div class="header text-center">
                <h4><asp:Literal ID="litCompanyName" runat="server">اسم الشركة</asp:Literal></h4>
                <p><asp:Literal ID="litBranchName" runat="server">اسم الفرع</asp:Literal><br />
                   <asp:Literal ID="litCompanyPhone" runat="server">هاتف الشركة</asp:Literal></p>
            </div>
            <div class="header-info">
                فاتورة رقم: <asp:Literal ID="litInvoiceID" runat="server"></asp:Literal><br />
                التاريخ: <asp:Literal ID="litInvoiceDate" runat="server"></asp:Literal><br />
                الكاشير: <asp:Literal ID="litCashierName" runat="server"></asp:Literal><br />
                العميل: <asp:Literal ID="litCustomerName" runat="server"></asp:Literal>
            </div>
            
            <asp:GridView ID="GridViewItems" runat="server" 
                AutoGenerateColumns="False" 
                GridLines="None" 
                CssClass="items-table"
                ShowHeader="false">
                <Columns>
                    <asp:BoundField DataField="ProductName" />
                    <asp:TemplateField ItemStyle-CssClass="qty"><ItemTemplate>x<%# Eval("Quantity") %></ItemTemplate></asp:TemplateField>
                    <asp:BoundField DataField="UnitPrice" DataFormatString="{0:N2}" ItemStyle-CssClass="price" />
                    <asp:BoundField DataField="SubTotal" DataFormatString="{0:N2}" ItemStyle-CssClass="total" />
                </Columns>
            </asp:GridView>
            
            <div class="total-info">
                <hr />
                الإجمالي: <span style="float:left;"><asp:Literal ID="litTotal" runat="server"></asp:Literal></span><br />
                الخصم: <span style="float:left;"><asp:Literal ID="litDiscount" runat="server"></asp:Literal></span><br />
                <strong>المبلغ المطلوب: <span style="float:left;"><asp:Literal ID="litGrandTotal" runat="server"></asp:Literal></span></strong>
            </div>

            <div class="footer text-center">
                <p>شكرًا لزيارتكم!</p>
            </div>
        </div>
    </form>
</body>
</html>