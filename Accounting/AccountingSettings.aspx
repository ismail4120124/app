<%@ Page Title="إعدادات الربط المحاسبي" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="AccountingSettings.aspx.vb" Inherits="SuperMarket.WebUI.AccountingSettings" %>
<%-- AccountingSettings.aspx --%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card shadow-sm">
        <div class="card-header"><h5 class="mb-0">إعدادات الحسابات الافتراضية</h5></div>
        <div class="card-body">
            <p class="text-muted">الرجاء تحديد الحسابات التحليلية التي سيستخدمها النظام لإنشاء القيود التلقائية.</p>
            <div class="row">
                <div class="col-md-6 mb-3">
                    <label class="form-label">حساب الصندوق (النقدية)</label>
                    <asp:DropDownList ID="ddlCashAccount" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-6 mb-3">
                    <label class="form-label">حساب البنك</label>
                    <asp:DropDownList ID="ddlBankAccount" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-6 mb-3">
                    <label class="form-label">حساب المبيعات</label>
                    <asp:DropDownList ID="ddlSalesAccount" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-6 mb-3">
                    <label class="form-label">حساب مرتجع المبيعات</label>
                    <asp:DropDownList ID="ddlSalesReturnAccount" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-6 mb-3">
                    <label class="form-label">حساب المشتريات / تكلفة البضاعة</label>
                    <asp:DropDownList ID="ddlPurchasesAccount" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-6 mb-3">
                    <label class="form-label">حساب المخزون</label>
                    <asp:DropDownList ID="ddlInventoryAccount" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-6 mb-3">
    <label class="form-label">حساب الموردين (الذمم الدائنة)</label>
    <asp:DropDownList ID="ddlSuppliersAccount" runat="server" CssClass="form-select"></asp:DropDownList>
</div>
                <div class="col-md-6 mb-3">
    <label class="form-label">حساب العملاء (الذمم المدينة)</label>
    <asp:DropDownList ID="ddlDebtorsAccount" runat="server" CssClass="form-select"></asp:DropDownList>
</div>
            </div>
        </div>
        <div class="card-footer text-end">
            <asp:Button ID="btnSaveSettings" runat="server" Text="حفظ الإعدادات" CssClass="btn btn-primary" OnClick="btnSaveSettings_Click" />
            <asp:Label ID="lblMessage" runat="server" CssClass="ms-3 fw-bold"></asp:Label>
        </div>
    </div>
</asp:Content>
