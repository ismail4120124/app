<%-- SalesReturn.aspx --%>
<%@ Page Title="مرتجع مبيعات" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="SalesReturn.aspx.vb" Inherits="SuperMarket.WebUI.SalesReturn" %>
<%@ Import Namespace="SuperMarket.Entities" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <div class="card shadow-sm">
        <div class="card-header">
            <h4 class="mb-0">تسجيل مرتجع مبيعات</h4>
        </div>
        <div class="card-body">
            <!-- رأس فاتورة المرتجع -->
            <div class="row g-3 p-3 border rounded mb-4">
                <div class="col-md-4">
                    <label class="form-label">بحث عن الفاتورة الأصلية (اختياري)</label>
                    <div class="input-group">
                        <asp:TextBox ID="txtSearchOriginalInvoice" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:Button ID="btnSearchInvoice" runat="server" Text="بحث" CssClass="btn btn-outline-secondary" OnClick="btnSearchInvoice_Click" />
                    </div>
                    <asp:HiddenField ID="hdnOriginalInvoiceID" runat="server" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">العميل</label>
                    <asp:TextBox ID="txtCustomerSearch" runat="server" CssClass="form-control" placeholder="ابحث عن العميل..."></asp:TextBox>
                    <asp:HiddenField ID="hdnSelectedCustomerID" runat="server" />
                    <ajax:AutoCompleteExtender ID="AutoCompleteExtenderCustomer" runat="server" TargetControlID="txtCustomerSearch" ServicePath="~/CustomerService.asmx" ServiceMethod="GetCustomers" MinimumPrefixLength="2" OnClientItemSelected="customerSelected" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">تاريخ المرتجع</label>
                    <asp:TextBox ID="txtReturnDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
            </div>

            <!-- إضافة الأصناف المرتجعة -->
            <asp:UpdatePanel ID="UpdatePanelItems" runat="server">
                <ContentTemplate>
                    <div class="row g-3 p-3 border rounded mb-4 align-items-end">
                        <div class="col-md-6">
                            <label class="form-label">بحث عن صنف للإرجاع</label>
                            <asp:TextBox ID="txtProductSearch" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtProductSearch_TextChanged"></asp:TextBox>
                            <asp:HiddenField ID="hdnSelectedProductID" runat="server" />
                        </div>
                        <div class="col-md-2">
                            <label class="form-label">الكمية</label>
                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" TextMode="Number" step="1" />
                        </div>
                        <div class="col-md-2">
                            <label class="form-label">سعر الوحدة</label>
                            <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" TextMode="Number" step="0.01" />
                        </div>
                        <div class="col-md-2">
                            <asp:Button ID="btnAddItem" runat="server" Text="إضافة" CssClass="btn btn-primary w-100" OnClick="btnAddItem_Click" />
                        </div>
                    </div>
                    
                    <hr />
                    <h4>الأصناف المرتجعة</h4>
                    <asp:GridView ID="GridViewReturnDetails" runat="server" 
                        AutoGenerateColumns="False" 
                        CssClass="table table-striped" 
                        ShowFooter="True" 
                        DataKeyNames="ProductID" 
                        OnRowDeleting="GridViewReturnDetails_RowDeleting"
                        EmptyDataText="لم تتم إضافة أي أصناف مرتجعة.">
                        <Columns>
                            <asp:BoundField DataField="ProductName" HeaderText="الصنف" />
                            <asp:BoundField DataField="Quantity" HeaderText="الكمية المرتجعة" />
                            <asp:BoundField DataField="UnitPrice" HeaderText="السعر" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="SubTotal" HeaderText="الإجمالي" DataFormatString="{0:N2}" />
                            <asp:TemplateField>
                                <ItemTemplate><asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-sm btn-outline-danger" ToolTip="حذف">X</asp:LinkButton></ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <FooterStyle BackColor="#f8f9fa" Font-Bold="True" HorizontalAlign="Right" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="card-footer text-end">
            <asp:Button ID="btnSaveReturn" runat="server" Text="حفظ فاتورة المرتجع" CssClass="btn btn-danger btn-lg" OnClick="btnSaveReturn_Click" />
            <asp:Label ID="lblMessage" runat="server" CssClass="ms-3 fw-bold"></asp:Label>
        </div>
    </div>
    <script type="text/javascript">
        function customerSelected(source, eventArgs) {
            var customerId = eventArgs.get_value();
            document.getElementById('<%= hdnSelectedCustomerID.ClientID %>').value = customerId;
        }
    </script>
</asp:Content>