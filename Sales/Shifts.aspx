<%-- Shifts.aspx --%>
<%@ Page Title="إدارة الورديات" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="Shifts.aspx.vb" Inherits="SuperMarket.WebUI.Shifts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center">
        <div class="col-md-8">
            <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false"></asp:Label>
            
            <!-- ====================================================== -->
            <!-- لوحة فتح وردية جديدة (تظهر إذا لم تكن هناك وردية مفتوحة) -->
            <!-- ====================================================== -->
            <asp:Panel ID="PanelOpenShift" runat="server" Visible="false">
                <div class="card shadow-sm">
                    <div class="card-header"><h5 class="mb-0">فتح وردية جديدة</h5></div>
                    <div class="card-body">
                        <p>أنت لا تملك أي وردية مفتوحة حاليًا. يرجى فتح وردية جديدة لبدء العمل.</p>
                        <div class="mb-3">
                            <label class="form-label">الرصيد الافتتاحي للصندوق (عهدة)</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtOpeningBalance" runat="server" CssClass="form-control" TextMode="Number" step="0.01">0.00</asp:TextBox>
                                <span class="input-group-text">ريال</span>
                            </div>
                        </div>
                        <div class="d-grid">
                            <asp:Button ID="btnOpenShift" runat="server" Text="فتح الوردية الآن" CssClass="btn btn-primary" OnClick="btnOpenShift_Click" />
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- ====================================================== -->
            <!-- لوحة إغلاق الوردية الحالية (تظهر إذا كانت هناك وردية مفتوحة) -->
            <!-- ====================================================== -->
            <asp:Panel ID="PanelCloseShift" runat="server" Visible="false">
                 <div class="card shadow-sm">
                    <div class="card-header"><h5 class="mb-0">إغلاق الوردية الحالية</h5></div>
                    <div class="card-body">
                        <div class="alert alert-info">
                            <strong>الكاشير:</strong> <asp:Literal ID="litCashierName" runat="server"></asp:Literal><br />
                            <strong>توقيت الفتح:</strong> <asp:Literal ID="litOpeningTime" runat="server"></asp:Literal><br />
                            <strong>الرصيد الافتتاحي:</strong> <asp:Literal ID="litOpeningBalance" runat="server"></asp:Literal> ريال
                        </div>
                         <div class="mb-3">
                            <label class="form-label">المبلغ النقدي في الصندوق عند الإغلاق</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtClosingBalance" runat="server" CssClass="form-control" TextMode="Number" step="0.01"></asp:TextBox>
                                 <span class="input-group-text">ريال</span>
                            </div>
                        </div>
                        <div class="d-grid">
                            <asp:Button ID="btnCloseShift" runat="server" Text="إغلاق الوردية وعرض التقرير" CssClass="btn btn-danger" OnClick="btnCloseShift_Click" />
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- ====================================================== -->
            <!-- لوحة عرض تقرير الوردية (تظهر بعد الإغلاق) -->
            <!-- ====================================================== -->
             <asp:Panel ID="PanelReport" runat="server" Visible="false">
                 <div class="card shadow-sm">
                    <div class="card-header"><h5 class="mb-0">تقرير إغلاق الوردية</h5></div>
                    <div class="card-body">
                       <table class="table table-bordered">
                           <tr><th>البيان</th><th>المبلغ</th></tr>
                           <tr><td>(+) الرصيد الافتتاحي</td><td><asp:Literal ID="litReportOpening" runat="server"></asp:Literal></td></tr>
                           <tr><td>(+) إجمالي المبيعات النقدية</td><td><asp:Literal ID="litReportCashSales" runat="server"></asp:Literal></td></tr>
                           <tr class="table-info"><th>(=) المبلغ المفترض في الصندوق</th><th><asp:Literal ID="litReportExpected" runat="server"></asp:Literal></th></tr>
                           <tr><td>(-) المبلغ الفعلي عند الإغلاق</td><td><asp:Literal ID="litReportActual" runat="server"></asp:Literal></td></tr>
                           <tr id="rowDifference" runat="server"><th>(=) الفرق (عجز/زيادة)</th><th><asp:Literal ID="litReportDifference" runat="server"></asp:Literal></th></tr>
                           <tr><td colspan="2"></td></tr>
                           <tr><td>إجمالي مبيعات البطاقة</td><td><asp:Literal ID="litReportCardSales" runat="server"></asp:Literal></td></tr>
                           <tr><td>إجمالي المبيعات الآجلة</td><td><asp:Literal ID="litReportCreditSales" runat="server"></asp:Literal></td></tr>
                       </table>
                       <div class="text-center">
                           <asp:Button ID="btnPrintReport" runat="server" Text="طباعة التقرير" CssClass="btn btn-secondary" OnClientClick="window.print(); return false;" />
                       </div>
                    </div>
                </div>
            </asp:Panel>

        </div>
    </div>
</asp:Content>