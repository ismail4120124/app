<%-- RolePermissions.aspx --%>
<%@ Page Title="إدارة صلاحيات الأدوار" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="RolePermissions.aspx.vb" Inherits="SuperMarket.WebUI.RolePermissions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card shadow-sm">
        <div class="card-header"><h5 class="mb-0">تحديد صلاحيات الأدوار</h5></div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-4">
                    <label class="form-label">اختر الدور لتعديل صلاحياته:</label>
                    <asp:DropDownList ID="ddlRoles" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlRoles_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
            <hr />
            
            <asp:Panel ID="PanelPermissions" runat="server" Visible="False">
                <h4>صلاحيات الدور: <asp:Literal ID="litRoleName" runat="server"></asp:Literal></h4>
                
                <%-- سنستخدم Repeater لتجميع الصلاحيات حسب الفئة --%>
                <asp:Repeater ID="RepeaterCategories" runat="server">
                    <ItemTemplate>
                        <div class="mb-3">
                            <h5><%# Eval("Category") %></h5>
                            <asp:CheckBoxList ID="cblPermissions" runat="server" 
                                DataSource='<%# Eval("Permissions") %>'
                                DataTextField="PermissionName"
                                DataValueField="PermissionKey"
                                RepeatColumns="3" 
                                CssClass="ms-3">
                            </asp:CheckBoxList>
                        </div>
                        <hr />
                    </ItemTemplate>
                </asp:Repeater>

                <div class="text-end">
                    <asp:Button ID="btnSavePermissions" runat="server" Text="حفظ الصلاحيات" CssClass="btn btn-primary" OnClick="btnSavePermissions_Click" />
                </div>
            </asp:Panel>
            
            <asp:Label ID="lblMessage" runat="server" CssClass="mt-2 d-block fw-bold"></asp:Label>
        </div>
    </div>
</asp:Content>
