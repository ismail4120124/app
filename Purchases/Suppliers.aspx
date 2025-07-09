<%@ Page Title="إدارة الموردين" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="Suppliers.aspx.vb" Inherits="SuperMarket.WebUI.Suppliers" %>
<%-- Suppliers.aspx --%>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-4">
            <h3>إضافة مورد جديد</h3>
            <div class="mb-3">
                <label class="form-label">اسم المورد</label>
                <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control" />
            </div>
            <div class="mb-3">
                <label class="form-label">الشخص المسؤول</label>
                <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" />
            </div>
            <div class="mb-3">
                <label class="form-label">الهاتف</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
            </div>
             <div class="mb-3">
                <label class="form-label">البريد الإلكتروني</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
            </div>
            <div class="mb-3">
                <label class="form-label">العنوان</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
            </div>
             <div class="mb-3 form-check">
                <asp:CheckBox ID="chkIsActive" runat="server" Text="مورد نشط" CssClass="form-check-input" Checked="true" />
                <label class="form-check-label">مورد نشط</label>
            </div>
            <asp:Button ID="btnAddSupplier" runat="server" Text="إضافة المورد" CssClass="btn btn-primary" />
            <asp:Label ID="lblMessage" runat="server" CssClass="mt-2 d-block"></asp:Label>
        </div>
        <div class="col-md-8">
            <h3>قائمة الموردين</h3>
            <asp:GridView ID="GridViewSuppliers" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="SupplierID" HeaderText="الرقم" />
                    <asp:BoundField DataField="SupplierName" HeaderText="اسم المورد" />
                    <asp:BoundField DataField="ContactPerson" HeaderText="المسؤول" />
                    <asp:BoundField DataField="Phone" HeaderText="الهاتف" />
                    <asp:CheckBoxField DataField="IsActive" HeaderText="نشط" ReadOnly="True" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
