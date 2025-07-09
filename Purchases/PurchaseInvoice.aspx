<%@ Page Title="فاتورة مشتريات" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master"  CodeBehind="PurchaseInvoice.aspx.vb" Inherits="SuperMarket.WebUI.PurchaseInvoice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <h3>فاتورة مشتريات جديدة</h3>
    
    <!-- رأس الفاتورة -->
    <div class="row g-3 p-3 border rounded mb-3">
        <div class="col-md-4">
            <label class="form-label">المورد</label>
            <asp:DropDownList ID="ddlSuppliers" runat="server" CssClass="form-select" />
        </div>
        <div class="col-md-3">
            <label class="form-label">الفرع (المخزن)</label>
            <asp:DropDownList ID="ddlBranches" runat="server" CssClass="form-select" />
        </div>
        <div class="col-md-3">
            <label class="form-label">تاريخ الفاتورة</label>
            <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="form-control" TextMode="Date" />
        </div>
        <div class="col-md-2">
            <label class="form-label">رقم فاتورة المورد</label>
            <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="form-control" />
        </div>
    </div>

    <!-- إضافة الأصناف (داخل UpdatePanel) -->
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
            <div class="row g-3 p-3 border rounded mb-3 align-items-end">
                <div class="col-md-4">
                    <label class="form-label">بحث عن صنف (بالاسم أو الباركود)</label>
                    <asp:TextBox ID="txtProductSearch" runat="server" CssClass="form-control" AutoPostBack="True" OnTextChanged="txtProductSearch_TextChanged" />
                    <asp:Label ID="lblSelectedProductID" runat="server" Visible="False"></asp:Label>
                </div>
                <div class="col-md-2">
                    <label class="form-label">الكمية</label>
                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" TextMode="Number" step="0.1" />
                </div>
                <div class="col-md-2">
                    <label class="form-label">سعر الشراء</label>
                    <asp:TextBox ID="txtPurchasePrice" runat="server" CssClass="form-control" TextMode="Number" step="0.01" />
                </div>
                    <div class="col-md-2">
                <label class="form-label">سعر البيع</label>
                <asp:TextBox ID="txtSellingPrice" runat="server" CssClass="form-control" TextMode="Number" step="0.01" />
            </div>
                <div class="col-md-2">
                    <asp:Button ID="btnAddItem" runat="server" Text="إضافة الصنف للفاتورة" CssClass="btn btn-secondary w-100" />
                </div>
            </div>

            <!-- تفاصيل الفاتورة -->
            <h4>الأصناف في الفاتورة</h4>
   <asp:GridView ID="GridViewInvoiceDetails" runat="server" AutoGenerateColumns="False" 
              CssClass="table" ShowFooter="True" 
              OnRowDeleting="GridViewInvoiceDetails_RowDeleting"
              DataKeyNames="ProductID">
                <Columns>
                    <asp:BoundField DataField="ProductID" HeaderText="كود الصنف" />
                    <asp:BoundField DataField="ProductName" HeaderText="اسم الصنف" />
                    <asp:BoundField DataField="Quantity" HeaderText="الكمية" />
                    <asp:BoundField DataField="PurchasePrice" HeaderText="سعر الشراء" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="SubTotal" HeaderText="الإجمالي الفرعي" DataFormatString="{0:N2}" />
                    <asp:TemplateField>
                        <ItemTemplate>
                          <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" 
                CssClass="btn btn-sm btn-danger"
                OnClientClick="return confirm('هل أنت متأكد من حذف هذا الصنف؟');">حذف</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#f8f9fa" Font-Bold="True" HorizontalAlign="Right" />
            </asp:GridView>
        </ContentTemplate>
  
    </asp:UpdatePanel>

    <!-- حفظ الفاتورة -->
    <div class="text-end mt-3">
        <asp:Button ID="btnSaveInvoice" runat="server" Text="حفظ الفاتورة النهائية" CssClass="btn btn-success btn-lg" />
        <asp:Label ID="lblMessage" runat="server" CssClass="ms-3"></asp:Label>
    </div>
</asp:Content>
