<%@ Page Title="تقرير دفتر الأستاذ" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="LedgerReport.aspx.vb" Inherits="SuperMarket.WebUI.LedgerReport" %>
<%-- LedgerReport.aspx --%>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card shadow-sm mb-4">
        <div class="card-header">
            <h5 class="mb-0">دفتر الأستاذ العام</h5>
        </div>
        <div class="card-body">
            <div class="row g-3 align-items-end">
                <%-- فلتر الحساب --%>
                <div class="col-md-5">
                    <label class="form-label">اختر الحساب</label>
                    <asp:DropDownList ID="ddlAccounts" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <%-- فلاتر التاريخ --%>
                <div class="col-md-3">
                    <label class="form-label">من تاريخ</label>
                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="col-md-3">
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

    <h4>كشف حساب: <asp:Label ID="lblAccountName" runat="server" CssClass="text-primary"></asp:Label></h4>
    <asp:GridView ID="GridViewLedger" runat="server" 
        CssClass="table table-bordered table-striped table-hover"
        AutoGenerateColumns="False"
        ShowFooter="true"
        EmptyDataText="الرجاء اختيار حساب وفترة زمنية ثم اضغط 'عرض'.">
        <Columns>
            <asp:BoundField DataField="VoucherDate" HeaderText="التاريخ" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="VoucherDescription" HeaderText="بيان القيد" />
            <asp:BoundField DataField="Debit" HeaderText="مدين" DataFormatString="{0:N2}" ItemStyle-CssClass="text-success" />
            <asp:BoundField DataField="Credit" HeaderText="دائن" DataFormatString="{0:N2}" ItemStyle-CssClass="text-danger" />
            <asp:BoundField DataField="Balance" HeaderText="الرصيد" DataFormatString="{0:N2}" ItemStyle-CssClass="fw-bolder" />
        </Columns>
        <FooterStyle BackColor="#f8f9fa" Font-Bold="True" />
    </asp:GridView>
</asp:Content>