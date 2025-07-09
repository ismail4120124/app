<%-- WholesaleInvoice.aspx --%>
<%@ Page Title="فاتورة بيع جملة" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="WholesaleInvoice.aspx.vb" Inherits="SuperMarket.WebUI.WholesaleInvoice" %>
<%-- WholesaleInvoice.aspx --%>

<%@ Import Namespace="SuperMarket.Entities" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <div class="card shadow-sm">
        <div class="card-header"><h4 class="mb-0">فاتورة بيع جملة جديدة</h4></div>
        <div class="card-body">
            <!-- رأس الفاتورة -->
            <div class="row g-3 p-3 border rounded mb-4">
                <div class="col-md-4">
                    <label class="form-label">العميل</label>
                    <asp:TextBox ID="txtCustomerSearch" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:HiddenField ID="hdnSelectedCustomerID" runat="server" />
                    <ajax:AutoCompleteExtender ID="AutoCompleteCustomer" runat="server" TargetControlID="txtCustomerSearch" ServicePath="~/CustomerService.asmx" ServiceMethod="GetCustomers" MinimumPrefixLength="2" OnClientItemSelected="customerSelected" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">تاريخ الفاتورة</label>
                    <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">من مخزن فرع</label>
                    <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
            </div>

            <!-- إضافة الأصناف -->
            <asp:UpdatePanel ID="UpdatePanelItems" runat="server">
                <ContentTemplate>
                    <div class="row g-3 p-3 border rounded mb-4 align-items-end">
                        <div class="col-md-5"><label class="form-label">بحث عن صنف</label><asp:TextBox ID="txtProductSearch" runat="server" CssClass="form-control"></asp:TextBox><asp:HiddenField ID="hdnSelectedProductID" runat="server" /><ajax:AutoCompleteExtender ID="AutoCompleteProduct" runat="server" TargetControlID="txtProductSearch" ServicePath="~/ProductService.asmx" ServiceMethod="GetProducts" MinimumPrefixLength="2" OnClientItemSelected="productSelected" /></div>
                        <div class="col-md-2"><label class="form-label">الكمية</label><asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" TextMode="Number" step="1" /></div>
                        <div class="col-md-2"><label class="form-label">سعر البيع</label><asp:TextBox ID="txtSellingPrice" runat="server" CssClass="form-control" TextMode="Number" step="0.01" /></div>
                        <div class="col-md-3"><asp:Button ID="btnAddItem" runat="server" Text="إضافة للفاتورة" CssClass="btn btn-primary w-100" OnClick="btnAddItem_Click" /></div>
                    </div>
                    <hr />
                    <h4>الأصناف في الفاتورة</h4>
                    <asp:GridView ID="GridViewInvoiceDetails" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" ShowFooter="true" DataKeyNames="ProductID" OnRowDeleting="GridViewInvoiceDetails_RowDeleting" OnRowDataBound="GridViewInvoiceDetails_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="ProductName" HeaderText="الصنف" />
                            <asp:BoundField DataField="Quantity" HeaderText="الكمية" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="UnitPrice" HeaderText="سعر البيع" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="SubTotal" HeaderText="الإجمالي الفرعي" DataFormatString="{0:N2}" />
                            <asp:TemplateField><ItemTemplate><asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-sm btn-outline-danger" ToolTip="حذف">X</asp:LinkButton></ItemTemplate></asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#f8f9fa" Font-Bold="True" HorizontalAlign="Right" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="card-footer text-end">
            <asp:Button ID="btnSaveAsCredit" runat="server" Text="حفظ كفاتورة آجلة" CssClass="btn btn-danger" OnClick="btnSave_Click" CommandArgument="Credit" />
            <asp:Button ID="btnSaveAsCash" runat="server" Text="حفظ كفاتورة نقدية" CssClass="btn btn-success" OnClick="btnSave_Click" CommandArgument="Cash" />
            <asp:Label ID="lblMessage" runat="server" CssClass="ms-3 fw-bold"></asp:Label>
        </div>
    </div>
    <script type="text/javascript">
        function customerSelected(s, e) { document.getElementById('<%= hdnSelectedCustomerID.ClientID %>').value = e.get_value(); }
        function productSelected(s, e) { document.getElementById('<%= hdnSelectedProductID.ClientID %>').value = e.get_value(); }
    </script>
</asp:Content>