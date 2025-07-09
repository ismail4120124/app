' Products.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities
Imports System.IO

Public Class Products ' تأكد من اسم الكلاس
    Inherits BasePage

    Private productBll As New ProductBLL()
    ' --- هنا سنقوم بتعريف كائنات BLL الجديدة ---
    Private categoryBll As New CategoryBLL()
    Private unitBll As New UnitBLL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindProductsGrid()
            ' --- هنا سنقوم بتفعيل استدعاء الدوال ---
            BindCategoriesDropDown()
            BindUnitsDropDown()
            If Request.QueryString("barcode") IsNot Nothing Then
                Dim newBarcode As String = Request.QueryString("barcode").ToString()
                ' تعبئة حقل الباركود بالباركود الجديد
                txtBarcode.Text = newBarcode
                ' نقل التركيز إلى حقل اسم المنتج لتسريع الإدخال

            End If
        End If
    End Sub

    Private Sub BindProductsGrid()
        ' ... (هذا الكود موجود بالفعل)
    End Sub

    ' --- هنا سنقوم بكتابة محتوى الدوال ---
    Private Sub BindCategoriesDropDown()
        ddlCategories.DataSource = categoryBll.GetAllCategories()
        ddlCategories.DataTextField = "CategoryName" ' اسم الحقل الذي سيعرض للمستخدم
        ddlCategories.DataValueField = "CategoryID"  ' اسم حقل القيمة (ID) الذي سيتم تخزينه
        ddlCategories.DataBind()
        ' إضافة عنصر افتراضي في بداية القائمة
        ddlCategories.Items.Insert(0, New ListItem("اختر الفئة...", "0"))
    End Sub

    Private Sub BindUnitsDropDown()
        ddlUnits.DataSource = unitBll.GetAllUnits()
        ddlUnits.DataTextField = "UnitName"
        ddlUnits.DataValueField = "UnitID"
        ddlUnits.DataBind()
        ddlUnits.Items.Insert(0, New ListItem("اختر الوحدة...", "0"))
    End Sub

    Protected Sub btnAddProduct_Click(sender As Object, e As EventArgs) Handles btnAddProduct.Click
        ' --- هنا سنقوم بتفعيل الكود الخاص بأخذ القيم من القوائم المنسدلة ---
        Try
            ' التحقق من أن المستخدم اختار فئة ووحدة
            If ddlCategories.SelectedValue = "0" Or ddlUnits.SelectedValue = "0" Then
                lblMessage.Text = "الرجاء اختيار فئة ووحدة للصنف."
                lblMessage.ForeColor = System.Drawing.Color.Red
                Return
            End If

            Dim newProduct As New Product()
            newProduct.Barcode = txtBarcode.Text
            newProduct.ProductName = txtProductName.Text
            ' --- تفعيل هذه الأسطر ---
            newProduct.CategoryID = Convert.ToInt32(ddlCategories.SelectedValue)
            newProduct.UnitID = Convert.ToInt32(ddlUnits.SelectedValue)
            ' -----------------------
            newProduct.ReorderLevel = If(String.IsNullOrWhiteSpace(txtReorderLevel.Text), 0, Convert.ToDecimal(txtReorderLevel.Text))

            ' ... (باقي كود رفع الصورة)

            productBll.AddProduct(newProduct)

            BindProductsGrid()
            ClearForm() ' دالة لتفريغ الحقول
            lblMessage.Text = "تمت إضافة الصنف بنجاح!"
            lblMessage.ForeColor = System.Drawing.Color.Green

        Catch ex As Exception
            lblMessage.Text = "حدث خطأ: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

    ' دالة مساعدة لتفريغ الحقول بعد الإضافة
    Private Sub ClearForm()
        txtBarcode.Text = ""
        txtProductName.Text = ""
        txtReorderLevel.Text = ""
        ddlCategories.SelectedIndex = 0
        ddlUnits.SelectedIndex = 0
        ' لا تقم بتفريغ fileUploadImage
    End Sub
End Class