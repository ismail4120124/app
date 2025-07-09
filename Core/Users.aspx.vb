' Users.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class Users
    Inherits BasePage

    Private userBll As New UserBLL()
    Private branchBll As New BranchBLL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' نقوم بملء كل البيانات عند أول تحميل للصفحة فقط
            BindUsersGrid()
            BindRolesDropDown()
            BindBranchesDropDown()
        End If
    End Sub

    Private Sub BindUsersGrid()
        GridViewUsers.DataSource = userBll.GetAllUsers()
        GridViewUsers.DataBind()
    End Sub

    Private Sub BindRolesDropDown()
        ddlRoles.DataSource = userBll.GetAllRoles()
        ddlRoles.DataBind()
        ddlRoles.Items.Insert(0, New ListItem("اختر الدور...", "0"))
    End Sub

    Private Sub BindBranchesDropDown()
        ddlBranches.DataSource = branchBll.GetAllBranches()
        ddlBranches.DataBind()
        ddlBranches.Items.Insert(0, New ListItem("اختر الفرع...", "0"))
    End Sub

    Protected Sub btnAddUser_Click(sender As Object, e As EventArgs) Handles btnAddUser.Click
        Try
            ' التحقق من أن المستخدم اختار دوراً وفرعاً
            If ddlRoles.SelectedValue = "0" OrElse ddlBranches.SelectedValue = "0" Then
                lblMessage.Text = "الرجاء اختيار الدور والفرع."
                lblMessage.ForeColor = System.Drawing.Color.Red
                Return
            End If

            Dim newUser As New User()
            newUser.Username = txtUsername.Text
            newUser.FullName = txtFullName.Text
            newUser.RoleID = Convert.ToInt32(ddlRoles.SelectedValue)
            newUser.BranchID = Convert.ToInt32(ddlBranches.SelectedValue)
            newUser.IsActive = chkIsActive.Checked

            ' استدعاء دالة الإضافة من BLL وتمرير كلمة المرور بشكل منفصل
            userBll.AddUser(newUser, txtPassword.Text)

            ' إعادة تحميل البيانات وتفريغ الحقول
            BindUsersGrid()
            ClearForm()
            lblMessage.Text = "تمت إضافة المستخدم بنجاح!"
            lblMessage.ForeColor = System.Drawing.Color.Green

        Catch ex As Exception
            lblMessage.Text = "حدث خطأ: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

    Private Sub ClearForm()
        txtUsername.Text = ""
        txtFullName.Text = ""
        txtPassword.Text = ""
        ddlRoles.SelectedIndex = 0
        ddlBranches.SelectedIndex = 0
        chkIsActive.Checked = True
    End Sub
End Class