' Branches.aspx.vb

Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class Branches
    Inherits BasePage

    ' إنشاء كائن من طبقة منطق العمل
    Private bll As New BranchBLL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindBranchesGrid()
        End If
    End Sub

    ' دالة لربط بيانات الفروع بالـ GridView
    Private Sub BindBranchesGrid()
        GridViewBranches.DataSource = bll.GetAllBranches()
        GridViewBranches.DataBind()
    End Sub

    ' حدث النقر على زر الإضافة
    Protected Sub btnAddBranch_Click(sender As Object, e As EventArgs) Handles btnAddBranch.Click
        Try
            ' إنشاء كائن فرع جديد وتعبئته من حقول الإدخال
            Dim newBranch As New Branch()
            newBranch.BranchName = txtBranchName.Text
            newBranch.Address = txtAddress.Text
            newBranch.Phone = txtPhone.Text

            ' استدعاء دالة الإضافة من طبقة BLL
            bll.AddBranch(newBranch)

            ' تحديث الشبكة لعرض الفرع الجديد
            BindBranchesGrid()

            ' إفراغ الحقول وعرض رسالة نجاح
            txtBranchName.Text = ""
            txtAddress.Text = ""
            txtPhone.Text = ""
            lblMessage.Text = "تمت إضافة الفرع بنجاح!"
            lblMessage.ForeColor = System.Drawing.Color.Green

        Catch ex As Exception
            ' عرض رسالة خطأ
            lblMessage.Text = "حدث خطأ: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

End Class