<%-- ProductPricing.aspx --%>
<%@ Page Title="تسعير المنتجات" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="ProductPricing.aspx.vb" Inherits="SuperMarket.WebUI.ProductPricing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card shadow-sm">
        <div class="card-header d-flex justify-content-between align-items-center">
            <h5 class="mb-0">تسعير المنتجات حسب الفرع</h5>
            <div>
                <label class="form-label d-inline-block me-2">اختر الفرع:</label>
                <asp:DropDownList ID="ddlBranches" runat="server" CssClass="form-select d-inline-block" style="width: 250px;" AutoPostBack="true" OnSelectedIndexChanged="ddlBranches_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>
        <div class="card-body">
            <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false"></asp:Label>
            
            <asp:Panel ID="PanelPricing" runat="server" Visible="False">
                <div class="table-responsive">
                    <asp:GridView ID="GridViewPricing" runat="server"
                        CssClass="table table-bordered table-hover"
                        AutoGenerateColumns="False"
                        DataKeyNames="ProductID">
                        <HeaderStyle CssClass="bg-light text-center" />
                        <Columns>
                            <asp:BoundField DataField="ProductName" HeaderText="اسم الصنف" />
                            <asp:BoundField DataField="Barcode" HeaderText="الباركود" ItemStyle-CssClass="text-center" />
                            <asp:BoundField DataField="CostPrice" HeaderText="متوسط التكلفة" DataFormatString="{0:N2}" ItemStyle-CssClass="text-center" />
                            
                            <%-- حقل سعر البيع القابل للتعديل --%>
                            <asp:TemplateField HeaderText="سعر البيع الجديد" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtSellingPrice" runat="server" 
                                         Text='<%# Eval("SellingPrice", "{0:F2}") %>'
                                         CssClass="form-control text-center fw-bold selling-price-input" 
                                         TextMode="Number" step="0.01"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- حقل هامش الربح المحسوب --%>
                            <asp:TemplateField HeaderText="هامش الربح %" ItemStyle-CssClass="text-center" ItemStyle-Width="120px">
                                <ItemTemplate>
                                    <asp:Label ID="lblProfitMargin" runat="server" CssClass="badge fs-6 profit-margin-label"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="text-end mt-3">
                    <asp:Button ID="btnSaveChanges" runat="server" Text="حفظ كل التغييرات" CssClass="btn btn-success" OnClick="btnSaveChanges_Click" />
                </div>
            </asp:Panel>
        </div>
    </div>

    <%-- كود JavaScript لحساب هامش الربح تلقائيًا --%>
    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {
            // استدعاء الدالة عند تحميل الصفحة لكل الحقول الموجودة
            calculateAllMargins();

            // إضافة مستمع (listener) لكل حقول سعر البيع
            const sellingPriceInputs = document.querySelectorAll('.selling-price-input');
            sellingPriceInputs.forEach(input => {
                input.addEventListener('input', function () {
                    calculateMargin(this);
                });
            });
        });

        // دالة يتم استدعاؤها بعد كل تحديث لـ UpdatePanel (إذا استخدمناه)
        function pageLoad(sender, args) {
            calculateAllMargins();
             const sellingPriceInputs = document.querySelectorAll('.selling-price-input');
            sellingPriceInputs.forEach(input => {
                input.addEventListener('input', function () {
                    calculateMargin(this);
                });
            });
        }

        function calculateAllMargins() {
            const rows = document.querySelectorAll('#<%= GridViewPricing.ClientID %> tr');
            rows.forEach(row => {
                const sellingPriceInput = row.querySelector('.selling-price-input');
                if (sellingPriceInput) {
                    calculateMargin(sellingPriceInput);
                }
            });
        }

        function calculateMargin(sellingPriceInput) {
            const row = sellingPriceInput.closest('tr');
            const costPriceCell = row.cells[2]; // الخلية الثالثة (فهرس 2) هي التكلفة
            const profitMarginLabel = row.querySelector('.profit-margin-label');

            const costPrice = parseFloat(costPriceCell.innerText);
            const sellingPrice = parseFloat(sellingPriceInput.value);

            if (!isNaN(costPrice) && !isNaN(sellingPrice) && sellingPrice > 0) {
                const profitMargin = ((sellingPrice - costPrice) / sellingPrice) * 100;
                profitMarginLabel.innerText = profitMargin.toFixed(1) + '%';

                // تغيير لون الشارة بناءً على هامش الربح
                if (profitMargin < 10) {
                    profitMarginLabel.className = 'badge fs-6 bg-danger profit-margin-label';
                } else if (profitMargin < 25) {
                     profitMarginLabel.className = 'badge fs-6 bg-warning text-dark profit-margin-label';
                } else {
                     profitMarginLabel.className = 'badge fs-6 bg-success profit-margin-label';
                }
            } else {
                profitMarginLabel.innerText = '---';
                profitMarginLabel.className = 'badge fs-6 bg-secondary profit-margin-label';
            }
        }
    </script>
</asp:Content>