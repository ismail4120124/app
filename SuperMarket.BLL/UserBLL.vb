' SuperMarket.BLL/UserBLL.vb
Imports SuperMarket.DAL
Imports SuperMarket.Entities
Imports System.Web.Security ' مهم جداً للتشفير

Public Class UserBLL
    Private dal As New UserDAL()

    ' دالة تسجيل الدخول
    Public Function Login(ByVal username As String, ByVal password As String) As User
        Dim user As User = dal.GetUserByUsername(username)

        If user IsNot Nothing Then
            ' مقارنة كلمة المرور المدخلة مع الهاش المخزن
            ' ملاحظة: هذه طريقة تشفير بسيطة. في الأنظمة الحقيقية، يُفضل استخدام مكتبات أحدث مثل BCrypt.
            Dim storedHash As String = user.PasswordHash
            Dim passwordHash As String = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1")

            If storedHash = passwordHash Then
                Return user ' نجح تسجيل الدخول
            End If
        End If

        Return Nothing ' فشل تسجيل الدخول
    End Function

    ' دالة إضافة مستخدم
    Public Sub AddUser(ByVal user As User, ByVal password As String)
        If String.IsNullOrWhiteSpace(user.Username) OrElse String.IsNullOrWhiteSpace(password) Then
            Throw New Exception("اسم المستخدم وكلمة المرور مطلوبان.")
        End If

        ' تشفير كلمة المرور قبل حفظها
        user.PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1")

        dal.AddUser(user)
    End Sub

    ' دالة جلب الأدوار
    Public Function GetAllRoles() As List(Of Role)
        Return dal.GetAllRoles()
    End Function
    ' أضف هذه الدالة داخل كلاس UserBLL

    Public Function GetAllUsers() As List(Of User)
        Return dal.GetAllUsers()
    End Function
End Class
