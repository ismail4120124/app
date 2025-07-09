' RolePermissions.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class RolePermissions
    Inherits BasePage

    Private roleBll As New UserBLL() ' UserBLL يحتوي على دوال الأدوار
    Private permissionBll As New PermissionBLL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            PopulateRolesDropDown()
        End If
    End Sub

    Private Sub PopulateRolesDropDown()
        ' جلب الأدوار (باستثناء Super Admin الذي يجب أن يمتلك كل الصلاحيات دائمًا)
        ddlRoles.DataSource = roleBll.GetAllRoles().Where(Function(r) r.RoleName <> "Super Admin").ToList()
        ddlRoles.DataTextField = "RoleName"
        ddlRoles.DataValueField = "RoleID"
        ddlRoles.DataBind()
        ddlRoles.Items.Insert(0, New ListItem("-- اختر دورًا --", "0"))
    End Sub

    Protected Sub ddlRoles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRoles.SelectedIndexChanged
        lblMessage.Text = ""
        Dim selectedRoleId = Convert.ToInt32(ddlRoles.SelectedValue)

        If selectedRoleId > 0 Then
            litRoleName.Text = ddlRoles.SelectedItem.Text
            LoadPermissionsForRole(selectedRoleId)
            PanelPermissions.Visible = True
        Else
            PanelPermissions.Visible = False
        End If
    End Sub

    Private Sub LoadPermissionsForRole(ByVal roleId As Integer)
        ' جلب كل الصلاحيات المتاحة في النظام
        Dim allPermissions = permissionBll.GetAllPermissions()

        ' جلب الصلاحيات الممنوحة حاليًا لهذا الدور
        Dim rolePermissionKeys = permissionBll.GetPermissionsForRole(roleId)

        ' تجميع الصلاحيات حسب الفئة لعرضها في Repeater
        Dim groupedPermissions = allPermissions.GroupBy(Function(p) p.Category).Select(Function(g) New With {
            .Category = g.Key,
            .Permissions = g.ToList()
        }).ToList()

        RepeaterCategories.DataSource = groupedPermissions
        RepeaterCategories.DataBind()

        ' الآن، نمر على كل CheckBoxList ونحدد الصلاحيات الممنوحة
        For Each item As RepeaterItem In RepeaterCategories.Items
            Dim cbl = CType(item.FindControl("cblPermissions"), CheckBoxList)
            If cbl IsNot Nothing Then
                For Each listItem As ListItem In cbl.Items
                    ' إذا كانت صلاحية الدور تحتوي على مفتاح هذا العنصر، قم بتحديده
                    If rolePermissionKeys.Contains(listItem.Value) Then
                        listItem.Selected = True
                    End If
                Next
            End If
        Next
    End Sub

    Protected Sub btnSavePermissions_Click(sender As Object, e As EventArgs) Handles btnSavePermissions.Click
        Try
            Dim selectedRoleId = Convert.ToInt32(ddlRoles.SelectedValue)
            If selectedRoleId <= 0 Then
                Throw New Exception("يجب اختيار دور أولاً.")
            End If

            Dim selectedPermissions As New List(Of String)()

            ' جمع كل المفاتيح المحددة من كل CheckBoxLists
            For Each item As RepeaterItem In RepeaterCategories.Items
                Dim cbl = CType(item.FindControl("cblPermissions"), CheckBoxList)
                If cbl IsNot Nothing Then
                    For Each listItem As ListItem In cbl.Items
                        If listItem.Selected Then
                            selectedPermissions.Add(listItem.Value)
                        End If
                    Next
                End If
            Next

            ' حفظ الصلاحيات الجديدة
            permissionBll.SavePermissionsForRole(selectedRoleId, selectedPermissions)

            lblMessage.Text = "تم حفظ الصلاحيات بنجاح."
            lblMessage.ForeColor = System.Drawing.Color.Green

        Catch ex As Exception
            lblMessage.Text = "خطأ: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

End Class