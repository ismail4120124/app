<%@ Page Title="شجرة الحسابات" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="ChartOfAccounts.aspx.vb" Inherits="SuperMarket.WebUI.ChartOfAccounts" %>
<%-- ChartOfAccounts.aspx --%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- ====================================================== -->
        <!-- الجزء الأيسر: عرض شجرة الحسابات                     -->
        <!-- ====================================================== -->
        <div class="col-md-5">
            <div class="card shadow-sm">
                <div class="card-header d-flex justify-content-between align-items-center">
                    <h5 class="mb-0">شجرة الحسابات</h5>
                    <asp:Button ID="btnNewMainAccount" runat="server" Text="حساب رئيسي جديد" CssClass="btn btn-sm btn-outline-primary" OnClick="btnNewMainAccount_Click" />
                </div>
                <div class="card-body" style="height: 600px; overflow-y: auto;">
                    <asp:TreeView ID="TreeViewAccounts" runat="server" 
                        OnSelectedNodeChanged="TreeViewAccounts_SelectedNodeChanged"
                        ImageSet="Arrows" ExpandDepth="1">
                        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                        <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="2px" />
                        <ParentNodeStyle Font-Bold="True" />
                        <SelectedNodeStyle BackColor="#e6f2ff" Font-Underline="True" HorizontalPadding="5px" VerticalPadding="2px" />
                    </asp:TreeView>
                </div>
            </div>
        </div>

        <!-- ====================================================== -->
        <!-- الجزء الأيمن: نموذج إضافة وتعديل الحسابات             -->
        <!-- ====================================================== -->
        <div class="col-md-7">
            <div class="card shadow-sm">
                <div class="card-header">
                    <h5 class="mb-0"><asp:Label ID="lblFormTitle" runat="server" Text="إضافة حساب جديد"></asp:Label></h5>
                </div>
                <div class="card-body">
                    <asp:HiddenField ID="hdnSelectedAccountID" runat="server" Value="0" />
                    
                    <div class="mb-3">
                        <label class="form-label">الحساب الأب</label>
                        <asp:TextBox ID="txtParentAccount" runat="server" CssClass="form-control" ReadOnly="true" Text="-- حساب رئيسي --"></asp:TextBox>
                    </div>
                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label class="form-label">رقم الحساب</label>
                            <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control" ></asp:TextBox>
                        </div>
                        <div class="col-md-6 mb-3">
                            <label class="form-label">اسم الحساب</label>
                            <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                     <div class="row">
                        <div class="col-md-6 mb-3">
                            <label class="form-label">نوع الحساب الرئيسي</label>
                            <asp:DropDownList ID="ddlAccountType" runat="server" CssClass="form-select">
                                <asp:ListItem>أصول</asp:ListItem>
                                <asp:ListItem>خصوم</asp:ListItem>
                                <asp:ListItem>حقوق ملكية</asp:ListItem>
                                <asp:ListItem>إيرادات</asp:ListItem>
                                <asp:ListItem>مصاريف</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6 mb-3 d-flex align-items-end">
                            <div class="form-check">
                                <asp:CheckBox ID="chkIsTransactional" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label">حساب تحليلي (يقبل قيود)</label>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="text-end">
                        <asp:Button ID="btnSaveAccount" runat="server" Text="حفظ الحساب" CssClass="btn btn-success" OnClick="btnSaveAccount_Click" />
                        <asp:Button ID="btnDeleteAccount" runat="server" Text="حذف الحساب" CssClass="btn btn-danger" OnClick="btnDeleteAccount_Click" Visible="False" OnClientClick="return confirm('هل أنت متأكد من حذف هذا الحساب؟ لا يمكن التراجع عن هذه العملية.');" />
                    </div>
                     <asp:Label ID="lblMessage" runat="server" CssClass="mt-2 d-block fw-bold"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>