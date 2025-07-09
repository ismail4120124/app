<%@ Page Title="إدارة بنود المصاريف" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="ExpenseCategories.aspx.vb" Inherits="SuperMarket.WebUI.ExpenseCategories" %>
<%-- ExpenseCategories.aspx --%>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- الجزء الأيمن: نموذج الإضافة -->
        <div class="col-md-4">
            <div class="card shadow-sm">
                <div class="card-header"><h5 class="mb-0">إضافة بند مصروف جديد</h5></div>
                <div class="card-body">
                    <div class="mb-3">
                        <label class="form-label">اسم بند المصروف (مثال: إيجار، رواتب)</label>
                        <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">ربط بالحساب المحاسبي</label>
                        <asp:DropDownList ID="ddlAccounts" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>
                     <div class="mb-3 form-check">
                        <asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                        <label class="form-check-label" for="<%= chkIsActive.ClientID %>">بند نشط</label>
                    </div>
                    <div class="d-grid">
                        <asp:Button ID="btnAddCategory" runat="server" Text="إضافة البند" CssClass="btn btn-primary" OnClick="btnAddCategory_Click" />
                    </div>
                    <asp:Label ID="lblMessage" runat="server" CssClass="mt-2 d-block fw-bold"></asp:Label>
                </div>
            </div>
        </div>

        <!-- الجزء الأيسر: عرض البنود الحالية -->
        <div class="col-md-8">
            <h3>قائمة بنود المصاريف</h3>
            <asp:GridView ID="GridViewCategories" runat="server" 
                CssClass="table table-bordered table-striped" 
                AutoGenerateColumns="False" 
                EmptyDataText="لم يتم إضافة أي بنود مصاريف بعد.">
                <Columns>
                    <asp:BoundField DataField="CategoryID" HeaderText="الرقم" />
                    <asp:BoundField DataField="CategoryName" HeaderText="اسم البند" />
                    <asp:BoundField DataField="AccountName" HeaderText="الحساب المحاسبي المرتبط" />
                    <asp:CheckBoxField DataField="IsActive" HeaderText="نشط" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>