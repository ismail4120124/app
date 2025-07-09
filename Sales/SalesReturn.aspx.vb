' SalesReturn.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class SalesReturn
    Inherits BasePage

    Private returnBll As New SalesReturnBLL()
    Private productBll As New ProductBLL()

    ' خاصية لتخزين قائمة الأصناف المرتجعة المؤقتة
    Private Property ReturnItems As List(Of SalesReturnInvoiceDetail)
        Get
            If ViewState("ReturnItems") Is Nothing Then
                ViewState("ReturnItems") = New List(Of SalesReturnInvoiceDetail)()
            End If
            Return CType(ViewState("ReturnItems"), List(Of SalesReturnInvoiceDetail))
        End Get
        Set(value As List(Of SalesReturnInvoiceDetail))
            ViewState("ReturnItems") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtReturnDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
        End If
    End Sub

    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        BindGrid()
    End Sub

    ' --- البحث عن الفاتورة الأصلية ---
    Protected Sub btnSearchInvoice_Click(sender As Object, e As EventArgs) Handles btnSearchInvoice.Click
        ' هذا الجزء يحتاج إلى دالة في DAL/BLL للبحث عن الفاتورة الأصلية
        ' Dim originalInvoiceId As Long = 0
        ' If Long.TryParse(txtSearchOriginalInvoice.Text, originalInvoiceId) Then
        '     Dim originalInvoice = salesBll.GetSalesInvoiceForReturn(originalInvoiceId)
        '     If originalInvoice IsNot Nothing Then
        '         ' تعبئة الحقول والجدول من الفاتورة الأصلية
        '         ' ...
        '     Else
        '         lblMessage.Text = "الفاتورة الأصلية غير موجودة."
        '     End If
        ' End If
        lblMessage.Text = "ميزة البحث عن الفاتورة الأصلية قيد التطوير."
        lblMessage.ForeColor = System.Drawing.Color.Blue
    End Sub

    ' --- إضافة الأصناف يدويًا ---
    Protected Sub txtProductSearch_TextChanged(sender As Object, e As EventArgs) Handles txtProductSearch.TextChanged
        Dim foundProduct = productBll.SearchProduct(txtProductSearch.Text)
        If foundProduct IsNot Nothing Then
            hdnSelectedProductID.Value = foundProduct.ProductID.ToString()
            txtProductSearch.Text = foundProduct.ProductName
            ' يمكن جلب آخر سعر بيع هنا وتعبئة حقل السعر
            txtQuantity.Focus()
        End If
    End Sub

    Protected Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        If String.IsNullOrWhiteSpace(hdnSelectedProductID.Value) OrElse
           String.IsNullOrWhiteSpace(txtQuantity.Text) OrElse
           String.IsNullOrWhiteSpace(txtUnitPrice.Text) Then
            lblMessage.Text = "الرجاء إكمال بيانات الصنف المرتجع."
            Return
        End If

        Dim newItem As New SalesReturnInvoiceDetail()
        newItem.ProductID = Convert.ToInt32(hdnSelectedProductID.Value)
        newItem.ProductName = txtProductSearch.Text ' اسم المنتج للعرض في الجدول
        newItem.Quantity = Convert.ToDecimal(txtQuantity.Text)
        newItem.UnitPrice = Convert.ToDecimal(txtUnitPrice.Text)
        newItem.SubTotal = newItem.Quantity * newItem.UnitPrice

        ReturnItems.Add(newItem)
        ClearItemForm()
    End Sub

    Protected Sub GridViewReturnDetails_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim productID = Convert.ToInt32(GridViewReturnDetails.DataKeys(e.RowIndex).Value)
        ReturnItems.RemoveAll(Function(item) item.ProductID = productID)
    End Sub

    ' --- حفظ فاتورة المرتجع ---
    ' في SalesReturn.aspx.vb

    Protected Sub btnSaveReturn_Click(sender As Object, e As EventArgs) Handles btnSaveReturn.Click
        If Not ReturnItems.Any() Then
            lblMessage.Text = "يجب إضافة صنف واحد على الأقل للمرتجع."
            lblMessage.ForeColor = System.Drawing.Color.Red
            Return
        End If

        Dim newReturn As New SalesReturnInvoice()
        newReturn.Details = Me.ReturnItems

        ' ==========================================================
        ' === الحل للنقطة 2: التعامل مع العميل الاختياري ===
        ' ==========================================================
        Dim customerId As Integer
        If Integer.TryParse(hdnSelectedCustomerID.Value, customerId) AndAlso customerId > 0 Then
            ' تم اختيار عميل صالح
            newReturn.CustomerID = customerId
        Else
            ' لم يتم اختيار عميل، استخدم "عميل نقدي" الافتراضي
            newReturn.CustomerID = 1
        End If
        ' ==========================================================

        ' ==========================================================
        ' === الحل للنقطة 1: التعامل مع الفاتورة الأصلية الاختيارية ===
        ' ==========================================================
        Dim originalInvoiceId As Long
        If Long.TryParse(hdnOriginalInvoiceID.Value, originalInvoiceId) AndAlso originalInvoiceId > 0 Then
            ' تم إدخال رقم فاتورة أصلية صالح
            newReturn.OriginalInvoiceID = originalInvoiceId
        Else
            ' لم يتم إدخال رقم فاتورة، اترك الخاصية Nothing (NULL)
            newReturn.OriginalInvoiceID = Nothing
        End If
        ' ==========================================================
        Dim returnDate As Date
        If Not Date.TryParse(txtReturnDate.Text, returnDate) Then
            Throw New Exception("تاريخ المرتجع غير صالح.")
        End If
        ' --- باقي الكود يبقى كما هو ---
        Dim currentUser As User = TryCast(Session("LoggedInUser"), User)
        If currentUser Is Nothing Then
            lblMessage.Text = "انتهت جلسة العمل، يرجى تسجيل الدخول."
            lblMessage.ForeColor = System.Drawing.Color.Red
            Return
        End If
        newReturn.UserID = currentUser.UserID
        newReturn.BranchID = currentUser.BranchID
        newReturn.ReturnDate = returnDate

        Try
            Dim newReturnId = returnBll.SaveReturnInvoice(newReturn)
            lblMessage.Text = "تم حفظ فاتورة المرتجع بنجاح برقم: " & newReturnId.ToString()
            lblMessage.ForeColor = System.Drawing.Color.Green
            ClearPage()
        Catch ex As Exception
            lblMessage.Text = "فشل الحفظ: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

    ' --- دوال مساعدة ---
    Private Sub BindGrid()
        GridViewReturnDetails.DataSource = ReturnItems
        GridViewReturnDetails.DataBind()
    End Sub



    Private Sub ClearItemForm()
        txtProductSearch.Text = ""
        hdnSelectedProductID.Value = ""
        txtQuantity.Text = ""
        txtUnitPrice.Text = ""
    End Sub

    Private Sub ClearPage()
        ReturnItems.Clear()
        ClearItemForm()
        txtSearchOriginalInvoice.Text = ""
        hdnOriginalInvoiceID.Value = ""
        txtCustomerSearch.Text = ""
        hdnSelectedCustomerID.Value = ""
    End Sub

End Class