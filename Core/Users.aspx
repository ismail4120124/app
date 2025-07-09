<%-- Users.aspx --%>
<%@ Page Title="إدارة المستخدمين" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="Users.aspx.vb" Inherits="SuperMarket.WebUI.Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-8">
            <h3>قائمة المستخدمين</h3>
            <asp:GridView ID="GridViewUsers" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="UserID" HeaderText="الرقم" />
                    <asp:BoundField DataField="Username" HeaderText="اسم المستخدم" />
                    <asp:BoundField DataField="FullName" HeaderText="الاسم الكامل" />
                    <asp:BoundField DataField="RoleName" HeaderText="الدور" />
                    <asp:BoundField DataField="BranchName" HeaderText="الفرع" />
                    <asp:CheckBoxField DataField="IsActive" HeaderText="نشط" ReadOnly="True" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="col-md-4">
            <h3>إضافة مستخدم جديد</h3>
            <div class="mb-3">
                <label class="form-label">اسم المستخدم</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
            </div>
            <div class="mb-3">
                <label class="form-label">الاسم الكامل</label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
            </div>
            <div class="mb-3">
                <label class="form-label">كلمة المرور</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
            </div>
            <div class="mb-3">
                <label class="form-label">الدور (الصلاحية)</label>
                <asp:DropDownList ID="ddlRoles" runat="server" CssClass="form-select" DataTextField="RoleName" DataValueField="RoleID" />
            </div>
            <div class="mb-3">
                <label class="form-label">الفرع</label>
                <asp:DropDownList ID="ddlBranches" runat="server" CssClass="form-select" DataTextField="BranchName" DataValueField="BranchID" />
            </div>
            <div class="mb-3 form-check">
                <asp:CheckBox ID="chkIsActive" runat="server" Text="مستخدم نشط" CssClass="form-check-input" Checked="true" />
                <label class="form-check-label">مستخدم نشط</label>
            </div>
            <asp:Button ID="btnAddUser" runat="server" Text="إضافة المستخدم" CssClass="btn btn-primary" />
            <asp:Label ID="lblMessage" runat="server" CssClass="mt-2 d-block"></asp:Label>
        </div>
    </div>
</asp:Content>