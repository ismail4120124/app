' Units.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class Units ' تأكد من تطابق هذا الاسم مع خاصية Inherits
    Inherits System.Web.UI.Page

    Private bll As New UnitBLL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindGrid()
        End If
    End Sub

    Private Sub BindGrid()
        GridViewUnits.DataSource = bll.GetAllUnits()
        GridViewUnits.DataBind()
    End Sub

    Protected Sub btnAddUnit_Click(sender As Object, e As EventArgs) Handles btnAddUnit.Click
        Try
            ' إنشاء كائن جديد وتمريره إلى BLL
            Dim newUnit As New Unit()
            newUnit.UnitName = txtUnitName.Text

            bll.AddUnit(newUnit)

            ' إعادة تحميل الجدول وتفريغ الحقل
            BindGrid()
            txtUnitName.Text = ""
            lblMessage.Text = "تمت إضافة الوحدة بنجاح."
            lblMessage.ForeColor = System.Drawing.Color.Green

        Catch ex As Exception
            lblMessage.Text = "خطأ: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub
End Class