<%@ Page Title="ميزان المراجعة" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="TrialBalance.aspx.vb" Inherits="SuperMarket.WebUI.TrialBalance" %>
<%-- TrialBalance.aspx --%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card shadow-sm mb-4">
        <div class="card-header"><h5 class="mb-0">ميزان المراجعة</h5></div>
        <div class="card-body">
            <div class="row g-3 align-items-end">
                <div class="col-md-4">
                    <label class="form-label">من تاريخ</label>
                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="col-md-4">
                    <label class="form-label">إلى تاريخ</label>
                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
          <div class="col-md-2"> <%-- زيادة عرض العمود قليلاً ليتسع للنص --%>
    <div class="form-check mt-4">
        <%-- 1. إزالة خاصية Text من CheckBox --%>
        <asp:CheckBox ID="chkOnlyTransactional" runat="server" Checked="true" CssClass="form-check-input" />
        
        <%-- 2. إضافة Label يدوي وربطه بالـ CheckBox --%>
        <label class="form-check-label" for="<%= chkOnlyTransactional.ClientID %>">
            الحسابات التحليلية فقط
        </label>
    </div>
</div>
                <div class="col-md-2">
                    <div class="d-grid">
                        <asp:Button ID="btnShowReport" runat="server" Text="عرض التقرير" CssClass="btn btn-primary" OnClick="btnShowReport_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-danger" Visible="false"></asp:Label>
    
    <asp:GridView ID="GridViewTrialBalance" runat="server" 
        CssClass="table table-bordered table-hover"
        AutoGenerateColumns="False"
        ShowFooter="true"
        EmptyDataText="الرجاء اختيار الفترة الزمنية ثم اضغط 'عرض التقرير'.">
        <HeaderStyle CssClass="bg-light text-center" />
        <Columns>
            <asp:BoundField DataField="AccountNumber" HeaderText="رقم الحساب" />
            <asp:BoundField DataField="AccountName" HeaderText="اسم الحساب" />
            
            <%-- الرصيد الافتتاحي --%>
            <asp:TemplateField HeaderText="رصيد افتتاحي (مدين)">
                <ItemTemplate><%# IIf(Eval("OpeningBalance") > 0, Eval("OpeningBalance", "{0:N2}"), "0.00") %></ItemTemplate>
                <ItemStyle CssClass="text-center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="رصيد افتتاحي (دائن)">
                <ItemTemplate><%# IIf(Eval("OpeningBalance") < 0, Math.Abs(Convert.ToDecimal(Eval("OpeningBalance"))).ToString("N2"), "0.00") %></ItemTemplate>
                <ItemStyle CssClass="text-center" />
            </asp:TemplateField>

            <%-- حركة الفترة --%>
            <asp:BoundField DataField="PeriodDebit" HeaderText="حركة مدين" DataFormatString="{0:N2}" ItemStyle-CssClass="text-center text-success" />
            <asp:BoundField DataField="PeriodCredit" HeaderText="حركة دائن" DataFormatString="{0:N2}" ItemStyle-CssClass="text-center text-danger" />

            <%-- الرصيد الختامي --%>
            <asp:TemplateField HeaderText="رصيد ختامي (مدين)">
                <ItemTemplate><%# IIf(Eval("ClosingBalance") > 0, Eval("ClosingBalance", "{0:N2}"), "0.00") %></ItemTemplate>
                <ItemStyle CssClass="text-center fw-bold" />
            </asp:TemplateField>
             <asp:TemplateField HeaderText="رصيد ختامي (دائن)">
                <ItemTemplate><%# IIf(Eval("ClosingBalance") < 0, Math.Abs(Convert.ToDecimal(Eval("ClosingBalance"))).ToString("N2"), "0.00") %></ItemTemplate>
                <ItemStyle CssClass="text-center fw-bold" />
            </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="#f8f9fa" Font-Bold="True" CssClass="text-center" />
    </asp:GridView>
</asp:Content>
