' SuperMarket.Entities/User.vb
Public Class User
    Public Property UserID As Integer
    Public Property Username As String
    Public Property PasswordHash As String ' لن نخزن كلمة المرور مباشرة
    Public Property FullName As String
    Public Property RoleID As Integer
    Public Property BranchID As Integer
    Public Property IsActive As Boolean

    ' خصائص إضافية لجلب أسماء الدور والفرع (للعرض فقط)
    Public Property RoleName As String
    Public Property BranchName As String
    Public Property Permissions As New List(Of String)()
End Class
