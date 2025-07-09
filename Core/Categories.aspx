<%@ Page Title="إدارة الفئات" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="~/Modules/Core/Categories.aspx.vb" Inherits="SuperMarket.WebUI.Categories" %>
<%-- Categories.aspx --%>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-4">
            <h3>إضافة فئة جديدة</h3>
            <div class="mb-3">
                <label class="form-label">اسم الفئة</label>
                <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control" />
            </div>
            <asp:Button ID="btnAddCategory" runat="server" Text="إضافة الفئة" CssClass="btn btn-primary" />
            <asp:Label ID="lblMessage" runat="server" CssClass="mt-2 d-block"></asp:Label>
        </div>
        <div class="col-md-8">
            <h3>قائمة الفئات</h3>
            <asp:GridView ID="GridViewCategories" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="CategoryID" HeaderText="الرقم" />
                    <asp:BoundField DataField="CategoryName" HeaderText="اسم الفئة" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>