<%-- Expenses.aspx --%>
<%@ Page Title="تسجيل المصروفات" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="Expenses.aspx.vb" Inherits="SuperMarket.WebUI.Expenses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- الجزء الأيمن: نموذج التسجيل -->
        <div class="col-md-5">
            <div class="card shadow-sm">
                <div class="card-header"><h5 class="mb-0">تسجيل مصروف جديد</h5></div>
                <div class="card-body">
                    <div class="mb-3">
                        <label class="form-label">بند المصروف</label>
                        <asp:DropDownList ID="ddlExpenseCategory" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>
                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label class="form-label">المبلغ</label>
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" TextMode="Number" step="0.01"></asp:TextBox>
                        </div>
                        <div class="col-md-6 mb-3">
                            <label class="form-label">التاريخ</label>
                            <asp:TextBox ID="txtExpenseDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">تم الدفع من حساب</label>
                        <asp:DropDownList ID="ddlPaidFromAccount" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">البيان / الوصف</label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                    </div>
                     <div class="mb-3">
                        <label class="form-label">يخص فرع (اختياري)</label>
                        <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>
                    <div class="d-grid">
                        <asp:Button ID="btnSaveExpense" runat="server" Text="حفظ المصروف" CssClass="btn btn-primary" OnClick="btnSaveExpense_Click" />
                    </div>
                    <asp:Label ID="lblMessage" runat="server" CssClass="mt-2 d-block fw-bold"></asp:Label>
                </div>
            </div>
        </div>

        <!-- الجزء الأيسر: عرض آخر المصروفات -->
        <div class="col-md-7">
            <h3>آخر المصروفات المسجلة</h3>
            <%-- يمكن إضافة GridView هنا لعرض آخر 10 مصروفات تم تسجيلها --%>
         <asp:GridView ID="GridViewExpenses" runat="server" 
    CssClass="table table-bordered table-striped table-hover"
    AutoGenerateColumns="False" 
    EmptyDataText="لا توجد مصروفات مسجلة حاليًا.">
    <Columns>
        <asp:BoundField DataField="ExpenseDate" HeaderText="التاريخ" DataFormatString="{0:yyyy-MM-dd}" />
        <asp:BoundField DataField="CategoryName" HeaderText="بند المصروف" />
        <asp:BoundField DataField="Amount" HeaderText="المبلغ" DataFormatString="{0:N2}" />
        <asp:BoundField DataField="Description" HeaderText="البيان" />
        <asp:BoundField DataField="PaidFromAccountName" HeaderText="مدفوع من" />
        <asp:BoundField DataField="BranchName" HeaderText="يخص فرع" />
    </Columns>
</asp:GridView>
        </div>
    </div>
</asp:Content>