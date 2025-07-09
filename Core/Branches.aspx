<%-- Branches.aspx --%>
<%@ Page Title="إدارة الفروع" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="Branches.aspx.vb" Inherits="SuperMarket.WebUI.Branches" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-8">
            <h3>قائمة الفروع</h3>
            <asp:GridView ID="GridViewBranches" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="BranchID" HeaderText="الرقم" />
                    <asp:BoundField DataField="BranchName" HeaderText="اسم الفرع" />
                    <asp:BoundField DataField="Address" HeaderText="العنوان" />
                    <asp:BoundField DataField="Phone" HeaderText="الهاتف" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="col-md-4">
            <h3>إضافة فرع جديد</h3>
            <div class="mb-3">
                <label for="txtBranchName" class="form-label">اسم الفرع</label>
                <asp:TextBox ID="txtBranchName" runat="server" CssClass="form-control" />
            </div>
            <div class="mb-3">
                <label for="txtAddress" class="form-label">العنوان</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" />
            </div>
            <div class="mb-3">
                <label for="txtPhone" class="form-label">الهاتف</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
            </div>
            <asp:Button ID="btnAddBranch" runat="server" Text="إضافة الفرع" CssClass="btn btn-primary" />
            <asp:Label ID="lblMessage" runat="server" CssClass="mt-2" ForeColor="Green"></asp:Label>
        </div>
    </div>
</asp:Content>