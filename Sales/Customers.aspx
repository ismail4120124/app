<%-- Customers.aspx --%>
<%@ Page Title="إدارة العملاء" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="Customers.aspx.vb" Inherits="SuperMarket.WebUI.Customers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-4">
            <div class="card">
                <div class="card-header">
                    <h5 class="mb-0">إضافة عميل جديد</h5>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <label class="form-label">اسم العميل</label>
                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" />
                    </div>
                    <div class="mb-3">
                        <label class="form-label">الهاتف</label>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
                    </div>
                    <div class="mb-3">
                        <label class="form-label">البريد الإلكتروني</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                    </div>
                    <div class="mb-3">
                        <label class="form-label">العنوان</label>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
                    </div>
                    <div class="mb-3 form-check">
                        <asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                        <label class="form-check-label" for="<%= chkIsActive.ClientID %>">عميل نشط</label>
                    </div>
                    <div class="d-grid">
                        <asp:Button ID="btnAddCustomer" runat="server" Text="إضافة العميل" CssClass="btn btn-primary" OnClick="btnAddCustomer_Click" />
                    </div>
                    <asp:Label ID="lblMessage" runat="server" CssClass="mt-2 d-block"></asp:Label>
                </div>
            </div>
        </div>
        <div class="col-md-8">
            <h3>قائمة العملاء</h3>
            <asp:GridView ID="GridViewCustomers" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" EmptyDataText="لا يوجد عملاء مسجلون حاليًا.">
                <Columns>
                    <asp:BoundField DataField="CustomerID" HeaderText="الرقم" />
                    <asp:BoundField DataField="CustomerName" HeaderText="اسم العميل" />
                    <asp:BoundField DataField="Phone" HeaderText="الهاتف" />
                    <asp:BoundField DataField="Email" HeaderText="البريد الإلكتروني" />
                    <asp:CheckBoxField DataField="IsActive" HeaderText="نشط" ReadOnly="True" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
