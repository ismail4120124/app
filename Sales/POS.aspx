<%@ Page Title="نقطة البيع" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="POS.aspx.vb" Inherits="SuperMarket.WebUI.POS" %>
<%@ Import Namespace="SuperMarket.Entities" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

<style>
    /* أسلوب مخصص لضمان الارتفاع المتساوي */
    .equal-height-container {
        display: flex;
        flex-wrap: nowrap;
        min-height: 600px; /* ارتفاع أدنى يمكن ضبطه حسب الحاجة */
    }
    .equal-height-col {
        display: flex;
        flex-direction: column;
        height: 100%;
    }
    .card {
        display: flex;
        flex-direction: column;
        height: 100%;
    }
    .card-body {
        flex-grow: 1;
        overflow-y: auto;
    }
    .card-footer {
        flex-shrink: 0;
    }
</style>

<div class="row g-3 equal-height-container">
    <!-- ====================================================== -->
    <!-- الجزء الأيسر: الفاتورة الحالية والإجماليات           -->
    <!-- ====================================================== -->
    <div class="col-md-7 equal-height-col">
        <div class="card shadow-sm">
            <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center">
                <h5 class="mb-0">الفاتورة الحالية</h5>
            </div>
            <div class="card-body" style="min-height: 450px; max-height: 450px;">
                <asp:UpdatePanel ID="UpdatePanelInvoice" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="GridViewPOS" runat="server" AutoGenerateColumns="False" CssClass="table table-hover" DataKeyNames="ProductID" OnRowDeleting="GridViewPOS_RowDeleting" EmptyDataText="لم تتم إضافة أي أصناف بعد">
                            <Columns>
                                <asp:BoundField DataField="ProductName" HeaderText="الصنف" />
                                <asp:BoundField DataField="Quantity" HeaderText="الكمية" />
                                <asp:BoundField DataField="UnitPrice" HeaderText="السعر" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="SubTotal" HeaderText="الإجمالي" DataFormatString="{0:N2}" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-sm btn-outline-danger" ToolTip="حذف الصنف"><i class="fa fa-times"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtBarcode" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="GridViewPOS" EventName="RowDeleting" />
                        <asp:AsyncPostBackTrigger ControlID="btnSuspendInvoice" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnNewInvoice" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="card-footer bg-light p-3">
                <asp:UpdatePanel ID="UpdatePanelTotal" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="vstack gap-2">
                            <div class="card border"><div class="card-body p-2 d-flex justify-content-between align-items-center"><span class="text-muted small">الإجمالي الفرعي</span><span class="fs-5 fw-bold text-muted"><asp:Label ID="lblSubTotal" runat="server" Text="0.00"></asp:Label></span></div></div>
                            <div class="card border"><div class="card-body p-2 d-flex justify-content-between align-items-center"><span class="fw-bold">الخصم</span><div style="width: 120px;"><asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control text-end fw-bolder fs-5 border-0 bg-transparent" style="box-shadow: none;" Text="0.00" AutoPostBack="true" OnTextChanged="txtDiscount_TextChanged" onfocus="this.select();"></asp:TextBox></div></div></div>
                            <div class="card bg-primary text-white mt-2"><div class="card-body p-3 d-flex justify-content-between align-items-center"><span class="fs-5 fw-bold">المبلغ المطلوب</span><span class="fs-4 fw-bolder"><asp:Label ID="lblGrandTotal" runat="server" Text="0.00"></asp:Label></span></div></div>
                            <hr class="my-3" />
                            <div class="vstack gap-2">
                                <div class="card border"><div class="card-body p-2 d-flex justify-content-between align-items-center"><span class="fw-bold">المبلغ المستلم</span><div style="width: 150px;"><asp:TextBox ID="txtAmountTendered" runat="server" CssClass="form-control text-end fw-bolder fs-5 border-0 bg-transparent" style="box-shadow: none;" Text="0.00" onfocus="this.select();" AutoPostBack="true"></asp:TextBox></div></div></div>
                                <div id="divChangeResult" runat="server" class="card border-success" visible="false"><div class="card-body p-2 d-flex justify-content-between align-items-center"><span class="fw-bold text-success">المتبقي للعميل</span><span class="fs-5 fw-bolder text-success"><asp:Label ID="lblChange" runat="server" Text="0.00"></asp:Label></span></div></div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtBarcode" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="GridViewPOS" EventName="RowDeleting" />
                        <asp:AsyncPostBackTrigger ControlID="btnSuspendInvoice" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnNewInvoice" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="txtDiscount" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtAmountTendered" EventName="TextChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- ====================================================== -->
    <!-- الجزء الأيمن: الإدخال وأزرار التحكم                    -->
    <!-- ====================================================== -->
    <div class="col-md-5 equal-height-col">
        <div class="card shadow-sm">
            <div class="card-body">
                <!-- حقل البحث بالباركود -->
                <div class="mb-3">
                    <label for="<%= txtBarcode.ClientID %>" class="form-label"><b>امسح الباركود أو ابحث بالاسم:</b></label>
                    <asp:TextBox ID="txtBarcode" runat="server" CssClass="form-control form-control-lg" OnTextChanged="txtBarcode_TextChanged" AutoPostBack="True" placeholder="..."></asp:TextBox>
                    <asp:UpdatePanel ID="UpdatePanelNewProduct" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="PanelNewProduct" runat="server" CssClass="alert alert-warning mt-2" Visible="False">
                                <div class="d-flex justify-content-between align-items-center">
                                    <span><strong>منتج غير معرّف!</strong></span>
                                    <asp:Button ID="btnAddNewProduct" runat="server" Text="إضافة الصنف الآن" CssClass="btn btn-primary btn-sm" OnClick="btnAddNewProduct_Click" />
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers><asp:AsyncPostBackTrigger ControlID="txtBarcode" EventName="TextChanged" /></Triggers>
                    </asp:UpdatePanel>
                </div>
                
                <!-- بيانات العميل -->
                <h5 class="card-title text-center mb-3">بيانات العميل</h5>
                <div class="mb-3">
                    <asp:TextBox ID="txtCustomerSearch" runat="server" CssClass="form-control" placeholder="ابحث بالاسم أو رقم الهاتف..."></asp:TextBox>
                    <asp:HiddenField ID="hdnSelectedCustomerID" runat="server" Value="1" />
                    <ajax:AutoCompleteExtender ID="AutoCompleteExtenderCustomer" runat="server" TargetControlID="txtCustomerSearch" ServicePath="~/CustomerService.asmx" ServiceMethod="GetCustomers" MinimumPrefixLength="2" OnClientItemSelected="customerSelected" UseContextKey="True"></ajax:AutoCompleteExtender>
                </div>

                <hr />
                <!-- أزرار الدفع وإدارة الفواتير -->
                <h5 class="card-title text-center mb-3">العمليات</h5>
                <div class="d-grid gap-2">
                    <asp:Button ID="btnPayCash" runat="server" Text="دفع نقدي" CssClass="btn btn-success" OnClick="btnPay_Click" CommandArgument="Cash" />
                    <asp:Button ID="btnPayCard" runat="server" Text="دفع بطاقة" CssClass="btn btn-info" OnClick="btnPay_Click" CommandArgument="Card" />
                    <asp:Button ID="btnPayCredit" runat="server" Text="تسجيل على الذمة (آجل)" CssClass="btn btn-danger" OnClick="btnPay_Click" CommandArgument="Credit" />
                    <hr />
                    <asp:Button ID="btnSuspendInvoice" runat="server" Text="تعليق الفاتورة" CssClass="btn btn-warning" OnClick="btnSuspendInvoice_Click" />
                    <asp:Button ID="btnNewInvoice" runat="server" Text="فاتورة جديدة" CssClass="btn btn-secondary" OnClick="btnNewInvoice_Click" CausesValidation="False" />
                    <asp:Button ID="btnResume" runat="server" Text="استئناف فاتورة" CssClass="btn btn-outline-primary" OnClick="btnResume_Click" />
                </div>
                
                <asp:UpdatePanel ID="UpdatePanelSuspendedList" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="PanelSuspendedList" runat="server" Visible="False" CssClass="mt-3">
                            <h6>اختر فاتورة لاستئنافها:</h6>
                            <asp:Repeater ID="RepeaterSuspended" runat="server" OnItemCommand="RepeaterSuspended_ItemCommand">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkResumeInvoice" runat="server" CommandName="Resume" CommandArgument='<%# Container.ItemIndex %>' CssClass="btn btn-light w-100 mb-2 text-start">
                                        فاتورة <%# Container.ItemIndex + 1 %> 
                                        <span class="badge bg-secondary float-end"><%# CType(Container.DataItem, List(Of SalesInvoiceDetail)).Count %> أصناف</span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:Repeater>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnResume" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="RepeaterSuspended" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
                
                <div class="mt-3 text-center">
                     <asp:Label ID="lblMessage" runat="server" CssClass="fw-bold" EnableViewState="False"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</div>

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" />

<script type="text/javascript">
    function customerSelected(source, args) {
        document.getElementById('<%= hdnSelectedCustomerID.ClientID %>').value = args.get_value();
        validateCreditButton();
    }
    function validateCreditButton() {
        var customerId = document.getElementById('<%= hdnSelectedCustomerID.ClientID %>').value;
        var creditButton = document.getElementById('<%= btnPayCredit.ClientID %>');
        if (creditButton) {
            if (customerId == '1') {
                creditButton.disabled = true;
                creditButton.title = 'يجب اختيار عميل حقيقي لتسجيل الفاتورة على الذمة';
            } else {
                creditButton.disabled = false;
                creditButton.title = '';
            }
        }
    }
    function pageLoad(sender, args) {
        validateCreditButton();
        equalizeHeights(); // استدعاء دالة تساوي الارتفاعات
    }
    function equalizeHeights() {
        var leftCol = document.querySelector('.col-md-7');
        var rightCol = document.querySelector('.col-md-5');
        if (leftCol && rightCol) {
            var maxHeight = Math.max(leftCol.offsetHeight, rightCol.offsetHeight);
            leftCol.style.height = maxHeight + 'px';
            rightCol.style.height = maxHeight + 'px';
        }
    }
    window.onload = function () {
        validateCreditButton();
        equalizeHeights();
    };
</script>
</asp:Content>