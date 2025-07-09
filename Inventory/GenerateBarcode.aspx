<%-- GenerateBarcode.aspx --%>
<%@ Page Title="إنشاء باركود السعر/الوزن" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="GenerateBarcode.aspx.vb" Inherits="SuperMarket.WebUI.GenerateBarcode" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <div class="row justify-content-center">
        <div class="col-md-7">
            <div class="card shadow-sm">
                <div class="card-header"><h5 class="mb-0">إنشاء ملصق باركود</h5></div>
                <div class="card-body">
                    <p class="text-muted">اختر المنتج وأدخل السعر الإجمالي لإنشاء باركود.</p>
                    
                    <div class="mb-3">
                        <label class="form-label">1. ابحث عن المنتج:</label>
                        <asp:TextBox ID="txtProductSearch" runat="server" CssClass="form-control" placeholder="اكتب اسم المنتج أو الباركود..."></asp:TextBox>
                        <asp:HiddenField ID="hdnSelectedProductID" runat="server" />
                        <ajax:AutoCompleteExtender ID="AutoCompleteProduct" runat="server" 
                            TargetControlID="txtProductSearch"
                            ServicePath="~/ProductService.asmx"
                            ServiceMethod="GetProducts"
                            MinimumPrefixLength="2"
                            OnClientItemSelected="productSelected" />
                    </div>
                    
                    <div class="mb-3">
                        <label class="form-label">2. أدخل السعر الإجمالي:</label>
                        <div class="input-group">
                            <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" TextMode="Number" step="0.01"></asp:TextBox>
                            <span class="input-group-text">ريال</span>
                        </div>
                    </div>
                    
                    <div class="d-grid">
                        <asp:Button ID="btnGenerate" runat="server" Text="إنشاء الباركود" CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                    </div>
                    
                    <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-danger mt-3 d-block" Visible="False"></asp:Label>

                    <%-- لوحة عرض نتيجة الباركود --%>
                    <asp:Panel ID="PanelBarcodeResult" runat="server" Visible="False" CssClass="text-center mt-4 border-top pt-3">
                        <h5>الملصق جاهز للطباعة</h5>
                        <p><asp:Literal ID="litProductName" runat="server"></asp:Literal></p>
                        <svg id="barcodeCanvas"></svg> <%-- تم تغيير ID لتجنب أي تضارب --%>
                        <div class="mt-3">
                            <asp:Button ID="btnPrintLabel" runat="server" Text="طباعة الملصق" CssClass="btn btn-success" OnClientClick="printLabel(); return false;" />
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>

    <%-- JavaScript Libraries and Functions --%>
    <script src="https://cdn.jsdelivr.net/npm/jsbarcode@3.11.5/dist/JsBarcode.all.min.js"></script>
    <script type="text/javascript">
        function productSelected(source, eventArgs) {
            document.getElementById('<%= hdnSelectedProductID.ClientID %>').value = eventArgs.get_value();
        }

        function printLabel() {
            var panel = document.getElementById('<%= PanelBarcodeResult.ClientID %>');
            if (panel) {
                var printContents = panel.innerHTML;
                var printWindow = window.open('', '_blank', 'height=400,width=600');
                printWindow.document.write('<html><head><title>طباعة ملصق</title>');
                printWindow.document.write('<style>body { text-align: center; font-family: sans-serif; }</style>');
                printWindow.document.write('</head><body>');
                printWindow.document.write(printContents);
                printWindow.document.write('</body></html>');
                printWindow.document.close();
                setTimeout(function () {
                    printWindow.print();
                    printWindow.close();
                }, 250);
            }
            return false;
        }
    </script>
</asp:Content>