' Login.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim userBll As New UserBLL()
        Dim loggedInUser As User = userBll.Login(txtUsername.Text, txtPassword.Text)

        If loggedInUser IsNot Nothing Then
            ' تم تسجيل الدخول بنجاح
            ' تخزين بيانات المستخدم في الـ Session لاستخدامها في باقي الصفحات
            Session("LoggedInUser") = loggedInUser

            ' توجيه المستخدم إلى الصفحة الرئيسية
            Response.Redirect("~/Default.aspx")
        Else
            ' فشل تسجيل الدخول
            lblError.Text = "اسم المستخدم أو كلمة المرور غير صحيحة."
        End If
    End Sub
End Class