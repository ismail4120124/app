' SuperMarket.BLL/PermissionBLL.vb
Imports SuperMarket.DAL
Imports SuperMarket.Entities

Public Class PermissionBLL
    Private dal As New PermissionDAL()

    Public Function GetAllPermissions() As List(Of Permission)
        Return dal.GetAllPermissions()
    End Function

    Public Function GetPermissionsForRole(ByVal roleId As Integer) As List(Of String)
        Return dal.GetPermissionsForRole(roleId)
    End Function

    Public Sub SavePermissionsForRole(ByVal roleId As Integer, ByVal permissionKeys As List(Of String))
        If roleId <= 0 Then
            Throw New Exception("يجب اختيار دور صالح.")
        End If
        ' يمكن إضافة تحقق آخر هنا
        dal.SavePermissionsForRole(roleId, permissionKeys)
    End Sub
End Class
