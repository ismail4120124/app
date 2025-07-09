<%-- StockTransfer.aspx --%>
<%@ Page Title="تحويل مخزني" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="~/Modules/Inventory/StockTransfers.aspx.vb" Inherits="SuperMarket.WebUI.StockTransfers" %>
<%@ Import Namespace="SuperMarket.Entities" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <div class="card shadow-sm">
        <div class="card-header">
            <h4 class="mb-0">إنشاء أمر تحويل مخزني</h4>
        </div>
        <div class="card-body">
            <!-- رأس أمر التحويل -->
            <div class="row g-3 p-3 border rounded mb-4">
                <div class="col-md-3">
                    <label class="form-label">تاريخ التحويل</label>
                    <asp:TextBox ID="txtTransferDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">التحويل من فرع (المصدر)</label>
                    <asp:DropDownList ID="ddlSourceBranch" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label">التحويل إلى فرع (المستلم)</label>
                    <asp:DropDownList ID="ddlDestinationBranch" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label">ملاحظات (اختياري)</label>
                    <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" />
                </div>
            </div>

            <!-- إضافة الأصناف -->
            <asp:UpdatePanel ID="UpdatePanelItems" runat="server">
                <ContentTemplate>
                    <div class="row g-3 p-3 border rounded mb-4 align-items-end">
                  
            <div class="col-md-8">
                <label class="form-label">بحث عن صنف للتحويل</label>
                <%-- 1. تعديل TextBox: إزالة AutoPostBack و OnTextChanged --%>
                <asp:TextBox ID="txtProductSearch" runat="server" CssClass="form-control" placeholder="ابحث بالاسم أو الباركود..."></asp:TextBox>
                <asp:HiddenField ID="hdnSelectedProductID" runat="server" />
                
                <%-- 2. إضافة AutoCompleteExtender --%>
                <ajax:AutoCompleteExtender ID="AutoCompleteProduct" runat="server"
                    TargetControlID="txtProductSearch"
                    ServicePath="~/ProductService.asmx" 
                    ServiceMethod="GetProducts"
                    MinimumPrefixLength="2"
                    CompletionInterval="500"
                    EnableCaching="true"
                    CompletionSetCount="10"
                    OnClientItemSelected="productSelectedForTransfer" 
                    UseContextKey="True">
                </ajax:AutoCompleteExtender>
            </div>
                        <div class="col-md-2">
                            <label class="form-label">الكمية</label>
                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" TextMode="Number" step="1" />
                        </div>
                        <div class="col-md-2">
                            <asp:Button ID="btnAddItem" runat="server" Text="إضافة" CssClass="btn btn-primary w-100" OnClick="btnAddItem_Click" />
                        </div>
                    </div>
                    
                    <hr />
                    <h4>الأصناف في أمر التحويل</h4>
                    <asp:GridView ID="GridViewTransferDetails" runat="server" 
                        AutoGenerateColumns="False" 
                        CssClass="table table-striped" 
                        DataKeyNames="ProductID" 
                        OnRowDeleting="GridViewTransferDetails_RowDeleting"
                        EmptyDataText="لم تتم إضافة أي أصناف.">
                        <Columns>
                            <asp:BoundField DataField="ProductID" HeaderText="كود الصنف" />
                            <asp:BoundField DataField="ProductName" HeaderText="اسم الصنف" />
                            <asp:BoundField DataField="Quantity" HeaderText="الكمية المحولة" DataFormatString="{0:N2}" />
                            <asp:TemplateField>
                                <ItemTemplate><asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-sm btn-outline-danger" ToolTip="حذف">X</asp:LinkButton></ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
   
            </asp:UpdatePanel>
        </div>
        <div class="card-footer text-end">
            <asp:Button ID="btnExecuteTransfer" runat="server" Text="تنفيذ أمر التحويل" CssClass="btn btn-success btn-lg" OnClick="btnExecuteTransfer_Click" />
            <asp:Label ID="lblMessage" runat="server" CssClass="ms-3 fw-bold"></asp:Label>
        </div>
    </div>
<script type="text/javascript">
    function productSelectedForTransfer(source, eventArgs) {
        var productId = eventArgs.get_value();
    
        document.getElementById('<%= hdnSelectedProductID.ClientID %>').value = productId;
    }
</script>
</asp:Content>
