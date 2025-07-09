<%@ Page Title="إدارة الأصناف" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="~/Modules/Inventory/Products.aspx.vb" Inherits="SuperMarket.WebUI.Products" %>
<%-- Products.aspx --%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- نموذج الإضافة -->
        <div class="col-md-4">
            <h3>إضافة صنف جديد</h3>
            <div class="mb-2">
                <label class="form-label">الباركود</label>
                <asp:TextBox ID="txtBarcode" runat="server" CssClass="form-control" />
            </div>
            <div class="mb-2">
                <label class="form-label">اسم الصنف</label>
                <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" />
            </div>
            <div class="mb-2">
                <label class="form-label">الفئة</label>
                <asp:DropDownList ID="ddlCategories" runat="server" CssClass="form-select" />
            </div>
            <div class="mb-2">
                <label class="form-label">الوحدة</label>
                <asp:DropDownList ID="ddlUnits" runat="server" CssClass="form-select" />
            </div>
            <div class="mb-2">
                <label class="form-label">حد إعادة الطلب</label>
                <asp:TextBox ID="txtReorderLevel" runat="server" CssClass="form-control" TextMode="Number" step="1" />
            </div>
             <div class="mb-3">
                <label class="form-label">صورة الصنف</label>
                <asp:FileUpload ID="fileUploadImage" runat="server" CssClass="form-control" />
            </div>
            <asp:Button ID="btnAddProduct" runat="server" Text="إضافة الصنف" CssClass="btn btn-primary" />
            <asp:Label ID="lblMessage" runat="server" CssClass="mt-2 d-block"></asp:Label>
        </div>
        <!-- جدول العرض -->
        <div class="col-md-8">
            <h3>قائمة الأصناف</h3>
            <asp:GridView ID="GridViewProducts" runat="server" CssClass="table table-sm table-bordered table-striped" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="ProductID" HeaderText="الرقم" />
                    <asp:BoundField DataField="Barcode" HeaderText="الباركود" />
                    <asp:BoundField DataField="ProductName" HeaderText="اسم الصنف" />
                    <asp:BoundField DataField="CategoryName" HeaderText="الفئة" />
                    <asp:BoundField DataField="UnitName" HeaderText="الوحدة" />
                   
                </Columns>
            </asp:GridView> 
        </div>
    </div>
</asp:Content>
