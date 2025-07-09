<%-- OpeningBalance.aspx --%>
<%@ Page Title="أرصدة أول المدة" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="OpeningBalance.aspx.vb" Inherits="SuperMarket.WebUI.OpeningBalance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card shadow-sm">
        <div class="card-header d-flex justify-content-between align-items-center">
            <h5 class="mb-0">إدخال أرصدة أول المدة للمخزون</h5>
            <div>
                <label class="form-label d-inline-block me-2">اختر الفرع:</label>
                <asp:DropDownList ID="ddlBranches" runat="server" CssClass="form-select d-inline-block" style="width: 300px;" AutoPostBack="true" OnSelectedIndexChanged="ddlBranches_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>
        <div class="card-body">
            <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false"></asp:Label>
            
            <asp:Panel ID="PanelOpeningBalance" runat="server" Visible="False">
                <p class="text-muted">أدخل الكميات والتكاليف وأسعار البيع الحالية للأصناف في الفرع المحدد. أي حقل تتركه فارغًا أو بقيمة صفر لن يتم تحديثه.</p>
                <div class="table-responsive">
                    <asp:GridView ID="GridViewOpeningBalance" runat="server"
                        CssClass="table table-bordered table-hover"
                        AutoGenerateColumns="False"
                        DataKeyNames="ProductID">
                        <HeaderStyle CssClass="bg-light text-center" />
                        <Columns>
                            <asp:BoundField DataField="ProductName" HeaderText="اسم الصنف" ReadOnly="true" />
                            <asp:BoundField DataField="Barcode" HeaderText="الباركود" ReadOnly="true" />
                            
                            <%-- Editable Fields --%>
                            <asp:TemplateField HeaderText="الكمية الحالية" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtQuantity" runat="server" 
                                         Text='<%# Eval("Quantity") %>'
                                         CssClass="form-control text-center" 
                                         TextMode="Number" step="1"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="متوسط التكلفة" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCostPrice" runat="server" 
                                         Text='<%# Eval("CostPrice", "{0:F2}") %>'
                                         CssClass="form-control text-center" 
                                         TextMode="Number" step="0.01"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="سعر البيع" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtSellingPrice" runat="server" 
                                         Text='<%# Eval("SellingPrice", "{0:F2}") %>'
                                         CssClass="form-control text-center" 
                                         TextMode="Number" step="0.01"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="text-end mt-3">
                    <asp:Button ID="btnSaveChanges" runat="server" Text="حفظ الأرصدة الافتتاحية" CssClass="btn btn-success" OnClick="btnSaveChanges_Click" />
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
