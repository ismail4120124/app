<%@ Page Title="إدارة وحدات القياس" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="Units.aspx.vb" Inherits="SuperMarket.WebUI.Units" %>
<%-- Units.aspx --%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-4">
            <h3>إضافة وحدة جديدة</h3>
            <div class="mb-3">
                <label class="form-label">اسم الوحدة (مثال: حبة، كرتون، كيلو)</label>
                <asp:TextBox ID="txtUnitName" runat="server" CssClass="form-control" />
            </div>
            <asp:Button ID="btnAddUnit" runat="server" Text="إضافة الوحدة" CssClass="btn btn-primary" OnClick="btnAddUnit_Click" />
            <asp:Label ID="lblMessage" runat="server" CssClass="mt-2 d-block"></asp:Label>
        </div>
        <div class="col-md-8">
            <h3>قائمة الوحدات</h3>
            <asp:GridView ID="GridViewUnits" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" EmptyDataText="لا توجد وحدات مدخلة حاليًا.">
                <Columns>
                    <asp:BoundField DataField="UnitID" HeaderText="الرقم" />
                    <asp:BoundField DataField="UnitName" HeaderText="اسم الوحدة" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>