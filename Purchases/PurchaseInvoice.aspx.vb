' PurchaseInvoice.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class PurchaseInvoice
    Inherits BasePage

    ' تعريف كائنات BLL
    Private supplierBll As New SupplierBLL()
    Private branchBll As New BranchBLL()
    Private productBll As New ProductBLL()
    ' ستحتاج لدالة بحث في هذا الكلاس
    ' Private purchaseBll As New PurchaseInvoiceBLL() ' سننشئ هذا الكلاس لاحقاً

    ' قائمة مؤقتة لحفظ أصناف الفاتورة قبل الحفظ النهائي
    Private Property InvoiceItems As List(Of PurchaseInvoiceDetail)
        Get
            If ViewState("InvoiceItems") Is Nothing Then
                ViewState("InvoiceItems") = New List(Of PurchaseInvoiceDetail)()
            End If
            Return CType(ViewState("InvoiceItems"), List(Of PurchaseInvoiceDetail))
        End Get
        Set(value As List(Of PurchaseInvoiceDetail))
            ViewState("InvoiceItems") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' ملء القوائم المنسدلة
            BindSuppliers()
            BindBranches()
            ' تعيين تاريخ اليوم افتراضياً
            txtInvoiceDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
        End If
    End Sub

    ' #region دوال ملء البيانات
    Private Sub BindSuppliers()
        ddlSuppliers.DataSource = supplierBll.GetAllSuppliers().Where(Function(s) s.IsActive).ToList()
        ddlSuppliers.DataTextField = "SupplierName"
        ddlSuppliers.DataValueField = "SupplierID"
        ddlSuppliers.DataBind()
        ddlSuppliers.Items.Insert(0, New ListItem("اختر المورد...", "0"))
    End Sub

    Private Sub BindBranches()
        ddlBranches.DataSource = branchBll.GetAllBranches()
        ddlBranches.DataTextField = "BranchName"
        ddlBranches.DataValueField = "BranchID"
        ddlBranches.DataBind()
        ddlBranches.Items.Insert(0, New ListItem("اختر الفرع...", "0"))
    End Sub
    ' #endregion

    ' دالة لربط تفاصيل الفاتورة بالجدول
    Private Sub BindGrid()
        GridViewInvoiceDetails.DataSource = InvoiceItems
        GridViewInvoiceDetails.DataBind()
    End Sub

    ' عند تغيير النص في مربع البحث عن المنتج
    Protected Sub txtProductSearch_TextChanged(sender As Object, e As EventArgs) Handles txtProductSearch.TextChanged
        ' التحقق من أن مربع البحث ليس فارغًا
        If String.IsNullOrWhiteSpace(txtProductSearch.Text) Then
            ClearItemForm() ' تفريغ الحقول إذا مسح المستخدم مربع البحث
            Return
        End If

        ' استدعاء دالة البحث من BLL
        Dim foundProduct As Product = productBll.SearchProduct(txtProductSearch.Text)

        If foundProduct IsNot Nothing Then
            ' --- تم العثور على المنتج ---
            ' 1. تخزين ID المنتج في الـ Label المخفي
            lblSelectedProductID.Text = foundProduct.ProductID.ToString()

            ' 2. عرض الاسم الكامل للمنتج في مربع البحث (لتأكيد الاختيار للمستخدم)
            txtProductSearch.Text = foundProduct.ProductName

            ' 3. نقل التركيز (Focus) إلى حقل الكمية لتسريع الإدخال
            txtQuantity.Focus()

            ' (إضافة اختيارية) جلب آخر سعر شراء لهذا الصنف وعرضه
            ' Dim lastPrice = productBll.GetLastPurchasePrice(foundProduct.ProductID)
            ' txtPurchasePrice.Text = lastPrice.ToString()
        Else
            ' --- لم يتم العثور على المنتج ---
            ' 1. تفريغ ID المنتج
            lblSelectedProductID.Text = ""

            ' 2. إعلام المستخدم (يمكنك استخدام Label أو أي طريقة أخرى)
            ' txtProductSearch.Text &= " (صنف غير موجود!)"
            ' أو
            ' lblMessage.Text = "صنف غير موجود!"
            ' lblMessage.ForeColor = Drawing.Color.Red
        End If
    End Sub


    ' عند الضغط على زر "إضافة الصنف للفاتورة"
    Protected Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        ' التحقق من وجود بيانات
        If String.IsNullOrEmpty(lblSelectedProductID.Text) OrElse String.IsNullOrEmpty(txtQuantity.Text) OrElse String.IsNullOrEmpty(txtPurchasePrice.Text) OrElse String.IsNullOrEmpty(txtSellingPrice.Text) Then
            ' عرض رسالة خطأ
            Return
        End If

        Dim productID = Convert.ToInt32(lblSelectedProductID.Text)
        Dim quantity = Convert.ToDecimal(txtQuantity.Text)
        Dim price = Convert.ToDecimal(txtPurchasePrice.Text)
        Dim sellingPrice = Convert.ToDecimal(txtSellingPrice.Text)

        ' التحقق مما إذا كان الصنف موجوداً بالفعل في الفاتورة لتحديث كميته
        Dim existingItem = InvoiceItems.FirstOrDefault(Function(i) i.ProductID = productID)
        If existingItem IsNot Nothing Then
            existingItem.Quantity += quantity
        Else
            ' إضافة صنف جديد للقائمة المؤقتة
            Dim newItem As New PurchaseInvoiceDetail()
            newItem.ProductID = productID
            newItem.ProductName = txtProductSearch.Text
            newItem.Quantity = quantity
            newItem.PurchasePrice = price
            newItem.SubTotal = quantity * price
            newItem.SellingPrice = sellingPrice
            InvoiceItems.Add(newItem)
        End If

        ' تحديث الجدول
        '   BindGrid()

        ' تفريغ حقول الإدخال
        ClearItemForm()
    End Sub
    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        ' هذا هو المكان المثالي لربط البيانات التي تعتمد على ViewState
        ' لأنه يضمن أن كل أحداث الكنترولات قد انتهت
        ' وأن ViewState يعكس الحالة النهائية قبل عرض الصفحة.
        BindGrid()
    End Sub
    Private Sub ClearItemForm()
        txtProductSearch.Text = ""
        lblSelectedProductID.Text = ""
        txtQuantity.Text = ""
        txtPurchasePrice.Text = ""
        txtProductSearch.Focus()
        txtSellingPrice.Text = "" ' << تفريغ حقل سعر البيع

    End Sub

    ' لحساب وعرض الإجمالي في الفوتر
    Protected Sub GridViewInvoiceDetails_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.Footer Then
            Dim total As Decimal = InvoiceItems.Sum(Function(i) i.SubTotal)
            e.Row.Cells(3).Text = "الإجمالي"
            e.Row.Cells(4).Text = total.ToString("N2")
        End If
    End Sub

    Protected Sub GridViewInvoiceDetails_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        ' 1. احصل على قيمة المفتاح من خاصية DataKeys باستخدام e.RowIndex
        Dim productIDToDelete As Integer = Convert.ToInt32(GridViewInvoiceDetails.DataKeys(e.RowIndex).Value)

        ' 2. احذف العنصر من القائمة المؤقتة في ViewState
        InvoiceItems.RemoveAll(Function(item) item.ProductID = productIDToDelete)

        ' 3. أعد ربط البيانات بالـ GridView لعرض التغييرات
        BindGrid()
    End Sub

    Protected Sub btnSaveInvoice_Click(sender As Object, e As EventArgs) Handles btnSaveInvoice.Click
        ' التحقق من أن هناك أصناف في الفاتورة
        If InvoiceItems.Count = 0 Then
            lblMessage.Text = "لا يمكن حفظ فاتورة فارغة."
            lblMessage.ForeColor = System.Drawing.Color.Red
            Return
        End If

        ' 1. تجميع بيانات رأس الفاتورة من الواجهة
        Dim invoice As New PurchaseInvoice1()
        invoice.SupplierID = Convert.ToInt32(ddlSuppliers.SelectedValue)
        invoice.BranchID = Convert.ToInt32(ddlBranches.SelectedValue)
        invoice.InvoiceDate = Convert.ToDateTime(txtInvoiceDate.Text)
        invoice.InvoiceNumber = txtInvoiceNumber.Text
        invoice.SupplierName = ddlSuppliers.SelectedItem.Text
        ' invoice.Notes = ... ' يمكنك إضافة حقل ملاحظات إذا أردت

        ' 2. إضافة تفاصيل الأصناف من القائمة المؤقتة (ViewState)
        invoice.Details = Me.InvoiceItems

        ' 3. استدعاء BLL لحفظ الفاتورة
        Dim bll As New PurchaseInvoiceBLL()
        Try
            Dim newInvoiceID As Integer = bll.SaveInvoice(invoice)

            ' نجحت العملية!
            lblMessage.Text = "تم حفظ الفاتورة بنجاح برقم: " & newInvoiceID.ToString()
            lblMessage.ForeColor = System.Drawing.Color.Green

            ' تفريغ الشاشة لفاتورة جديدة
            ClearPage()

        Catch ex As Exception
            ' فشلت العملية
            lblMessage.Text = "خطأ: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

    Private Sub ClearPage()
        ' تفريغ القوائم المنسدلة وحقول الإدخال
        ddlSuppliers.SelectedIndex = 0
        ddlBranches.SelectedIndex = 0
        txtInvoiceDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
        txtInvoiceNumber.Text = ""

        ' تفريغ قائمة الأصناف المؤقتة والجدول
        InvoiceItems.Clear()
        BindGrid()
    End Sub
End Class