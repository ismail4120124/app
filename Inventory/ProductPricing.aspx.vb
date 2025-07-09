' ProductPricing.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class ProductPricing
    Inherits BasePage ' نرث من BasePage لتطبيق الصلاحيات

    Private branchBll As New BranchBLL()
    Private productBll As New ProductBLL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            PopulateBranchesDropDown()
        End If


    End Sub

    Private Sub PopulateBranchesDropDown()
        ddlBranches.DataSource = branchBll.GetAllBranches()
        ddlBranches.DataTextField = "BranchName"
        ddlBranches.DataValueField = "BranchID"
        ddlBranches.DataBind()
        ddlBranches.Items.Insert(0, New ListItem("-- اختر فرعًا لعرض أسعاره --", "0"))
    End Sub

    Protected Sub ddlBranches_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlBranches.SelectedIndexChanged
        lblMessage.Visible = False
        Dim branchId = Convert.ToInt32(ddlBranches.SelectedValue)
        If branchId > 0 Then
            BindPricingGrid(branchId)
            PanelPricing.Visible = True
        Else
            PanelPricing.Visible = False
        End If
    End Sub

    Private Sub BindPricingGrid(ByVal branchId As Integer)
        Dim pricingData = productBll.GetProductsForPricing(branchId)
        GridViewPricing.DataSource = pricingData
        GridViewPricing.DataBind()
    End Sub

    'Protected Sub btnSaveChanges_Click(sender As Object, e As EventArgs) Handles btnSaveChanges.Click
    '    Try
    '        Dim branchId = Convert.ToInt32(ddlBranches.SelectedValue)
    '        If branchId <= 0 Then Throw New Exception("يجب اختيار فرع أولاً.")

    '        Dim updatedPrices As New List(Of ProductPricingInfo)()

    '        ' المرور على كل صف في GridView لجمع الأسعار المحدثة
    '        For Each row As GridViewRow In GridViewPricing.Rows
    '            If row.RowType = DataControlRowType.DataRow Then
    '                Dim productId = Convert.ToInt32(GridViewPricing.DataKeys(row.RowIndex).Value)
    '                Dim txtSellingPrice = CType(row.FindControl("txtSellingPrice"), TextBox)

    '                Dim pricingInfo As New ProductPricingInfo()
    '                pricingInfo.ProductID = productId
    '                pricingInfo.SellingPrice = Convert.ToDecimal(txtSellingPrice.Text)

    '                updatedPrices.Add(pricingInfo)
    '            End If
    '        Next

    '        ' إرسال القائمة إلى BLL لحفظها دفعة واحدة
    '        productBll.UpdateProductPrices(updatedPrices, branchId)

    '        lblMessage.Text = "تم حفظ تغييرات الأسعار بنجاح."
    '        lblMessage.CssClass = "alert alert-success"
    '        lblMessage.Visible = True

    '    Catch ex As Exception
    '        lblMessage.Text = "فشل حفظ التغييرات: " & ex.Message
    '        lblMessage.CssClass = "alert alert-danger"
    '        lblMessage.Visible = True
    '    End Try
    'End Sub
    Protected Sub btnSaveChanges_Click(sender As Object, e As EventArgs) Handles btnSaveChanges.Click
        Try
            Dim branchId = Convert.ToInt32(ddlBranches.SelectedValue)
            If branchId <= 0 Then Throw New Exception("يجب اختيار فرع أولاً.")

            Dim updatedPrices As New List(Of ProductPricingInfo)()

            ' الآن، GridViewPricing.Rows يجب أن تحتوي على البيانات
            For i As Integer = 0 To GridViewPricing.Rows.Count - 1
                Dim row As GridViewRow = GridViewPricing.Rows(i)
                If row.RowType = DataControlRowType.DataRow Then
                    ' 1. جلب ID المنتج من DataKeys (الطريقة الآمنة)
                    Dim productId = Convert.ToInt32(GridViewPricing.DataKeys(i).Value)

                    ' 2. جلب TextBox من الصف
                    Dim txtSellingPrice = CType(row.FindControl("txtSellingPrice"), TextBox)

                    ' 3. قراءة القيمة الجديدة من TextBox
                    Dim newPrice As Decimal
                    If Decimal.TryParse(txtSellingPrice.Text, newPrice) Then

                        ' (اختياري) يمكنك إضافة منطق هنا لتقارن السعر الجديد بالقديم
                        ' وإضافة المنتج للقائمة فقط إذا تغير سعره

                        Dim pricingInfo As New ProductPricingInfo()
                        pricingInfo.ProductID = productId
                        pricingInfo.SellingPrice = newPrice

                        updatedPrices.Add(pricingInfo)
                    End If
                End If
            Next
            ' ==========================================================

            ' التأكد من أن هناك تغييرات ليتم حفظها
            If Not updatedPrices.Any() Then
                lblMessage.Text = "لم يتم إجراء أي تغييرات لحفظها."
                lblMessage.CssClass = "alert alert-info"
                lblMessage.Visible = True
                Return
            End If

            productBll.UpdateProductPrices(updatedPrices, branchId)

            lblMessage.Text = "تم حفظ تغييرات الأسعار بنجاح."
            lblMessage.CssClass = "alert alert-success"
            lblMessage.Visible = True

            ' إعادة ربط البيانات بعد الحفظ لعرض القيم المحدثة
            BindPricingGrid(branchId)

        Catch ex As Exception
            lblMessage.Text = "فشل حفظ التغييرات: " & ex.Message
            lblMessage.CssClass = "alert alert-danger"
            lblMessage.Visible = True
        End Try
    End Sub
End Class