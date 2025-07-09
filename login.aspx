<%-- Login.aspx --%>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="SuperMarket.WebUI.Login" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>تسجيل الدخول</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.rtl.min.css" rel="stylesheet">
    <style>
        body { background-color: #f8f9fa; }
        .login-container { max-width: 400px; margin: 100px auto; padding: 20px; background: white; border-radius: 10px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }
    </style>
</head>
<body>
    <form id="form1" runat="server" class="login-container">
        <h3 class="text-center mb-4">نظام سوبرماركت برو</h3>
        <div class="mb-3">
            <label for="txtUsername" class="form-label">اسم المستخدم</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
        </div>
        <div class="mb-3">
            <label for="txtPassword" class="form-label">كلمة المرور</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
        </div>
        <div class="d-grid">
            <asp:Button ID="btnLogin" runat="server" Text="تسجيل الدخول" CssClass="btn btn-primary" />
        </div>
        <asp:Label ID="lblError" runat="server" CssClass="text-danger mt-3 d-block"></asp:Label>
    </form>
</body>
</html>