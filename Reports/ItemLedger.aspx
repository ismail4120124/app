<%-- ItemLedger.aspx --%>
<%@ Page Title="تقرير حركة صنف" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="ItemLedger.aspx.vb" Inherits="SuperMarket.WebUI.ItemLedger" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="card shadow-sm mb-4">
        <div class="card-header">
            <h5 class="mb-0">فلاتر البحث</h5>
        </div>
        <div class="card-body">
            <div class="row g-3 align-items-end">
                <%-- فلتر الفرع --%>
                <div class="col-md-3">
                    <label class="form-label">الفرع</label>
                    <asp:DropDownList ID="ddlBranches" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <%-- فلتر المنتج --%>
                <div class="col-md-4">
                    <label class="form-label">بحث عن المنتج</label>
                    <asp:TextBox ID="txtProductSearch" runat="server" CssClass="form-control" placeholder="ابحث بالاسم أو الباركود..."></asp:TextBox>
                    <asp:HiddenField ID="hdnSelectedProductID" runat="server" />
                    <ajax:AutoCompleteExtender ID="AutoCompleteProduct" runat="server"
                        TargetControlID="txtProductSearch"
                        ServicePath="~/ProductService.asmx"
                        ServiceMethod="GetProducts"
                        MinimumPrefixLength="2"
                        OnClientItemSelected="productSelected" />
                </div>
                <%-- فلاتر التاريخ --%>
                <div class="col-md-2">
                    <label class="form-label">من تاريخ</label>
                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <label class="form-label">إلى تاريخ</label>
                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <%-- زر عرض التقرير --%>
                <div class="col-md-1">
                    <div class="d-grid">
                        <asp:Button ID="btnShowReport" runat="server" Text="عرض" CssClass="btn btn-primary" OnClick="btnShowReport_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-danger" Visible="false"></asp:Label>

    <h4>كشف حركة الصنف</h4>
    <asp:GridView ID="GridViewLedger" runat="server" 
        CssClass="table table-bordered table-striped table-hover"
        AutoGenerateColumns="False"
        EmptyDataText="الرجاء اختيار الفلاتر والضغط على زر 'عرض' لإظهار التقرير.">
        <Columns>
            <asp:BoundField DataField="TranDate" HeaderText="التاريخ" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="TranType" HeaderText="نوع الحركة" />
            <asp:BoundField DataField="DocNumber" HeaderText="رقم المستند" />
            <asp:BoundField DataField="QtyIn" HeaderText="وارد" DataFormatString="{0:N2}" ItemStyle-CssClass="text-success fw-bold" />
            <asp:BoundField DataField="QtyOut" HeaderText="صادر" DataFormatString="{0:N2}" ItemStyle-CssClass="text-danger fw-bold" />
            <asp:BoundField DataField="Balance" HeaderText="الرصيد" DataFormatString="{0:N2}" ItemStyle-CssClass="fw-bolder" />
        </Columns>
    </asp:GridView>

    <%-- كود JavaScript للإكمال التلقائي --%>
    <script type="text/javascript">
        function productSelected(source, eventArgs) {
            var productId = eventArgs.get_value();
            document.getElementById('<%= hdnSelectedProductID.ClientID %>').value = productId;
        }
    </script>
</asp:Content>
