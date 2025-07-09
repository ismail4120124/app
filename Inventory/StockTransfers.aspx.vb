' StockTransfer.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class StockTransfers
    Inherits BasePage

    Private transferBll As New StockTransferBLL()
    Private productBll As New ProductBLL()
    Private branchBll As New BranchBLL()

    ' خاصية لتخزين قائمة الأصناف المحولة المؤقتة
    Private Property TransferItems As List(Of StockTransferDetail)
        Get
            If ViewState("TransferItems") Is Nothing Then
                ViewState("TransferItems") = New List(Of StockTransferDetail)()
            End If
            Return CType(ViewState("TransferItems"), List(Of StockTransferDetail))
        End Get
        Set(value As List(Of StockTransferDetail))
            ViewState("TransferItems") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' تعبئة قوائم الفروع وتاريخ اليوم
            BindBranches()
            txtTransferDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
        End If
    End Sub

    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        BindGrid()
    End Sub

    ' --- أحداث الكنترولات ---
    ' في StockTransferPage.vb
    Protected Sub txtProductSearch_TextChanged(sender As Object, e As EventArgs) Handles txtProductSearch.TextChanged
        ' ابحث عن المنتج
        Dim foundProduct = productBll.SearchProduct(txtProductSearch.Text)

        ' إذا وجد المنتج
        If foundProduct IsNot Nothing Then
            ' ضع الـ ID في الحقل المخفي
            hdnSelectedProductID.Value = foundProduct.ProductID.ToString()
            ' اعرض الاسم الكامل للمستخدم للتأكيد
            txtProductSearch.Text = foundProduct.ProductName
            ' انقل التركيز إلى حقل الكمية لتسريع العمل
            txtQuantity.Focus()
        Else
            ' إذا لم يوجد، أفرغ الحقل المخفي
            hdnSelectedProductID.Value = ""
        End If
    End Sub

    ' في StockTransfer.aspx.vb

    Protected Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        ' ==========================================================
        ' === التعديل هنا: الاعتماد على الحقل المخفي ===
        ' ==========================================================

        Dim productId As Integer = 0
        Dim productIdRawValue As String = Request.Form(hdnSelectedProductID.UniqueID)
        Integer.TryParse(productIdRawValue, productId)

        ' التحقق من أن المستخدم اختار منتجًا وأن الكمية مدخلة
        If productId = 0 OrElse String.IsNullOrWhiteSpace(txtQuantity.Text) Then
            lblMessage.Text = "الرجاء اختيار صنف من قائمة البحث وتحديد الكمية."
            lblMessage.ForeColor = System.Drawing.Color.Red
            Return
        End If
        ' ==========================================================

        Dim newItem As New StockTransferDetail()
        newItem.ProductID = productId
        newItem.ProductName = txtProductSearch.Text ' يمكننا أخذ الاسم من مربع النص للعرض فقط
        newItem.Quantity = Convert.ToDecimal(txtQuantity.Text)

        Dim existingItem = TransferItems.FirstOrDefault(Function(i) i.ProductID = newItem.ProductID)
        If existingItem IsNot Nothing Then
            existingItem.Quantity += newItem.Quantity
        Else
            TransferItems.Add(newItem)
        End If

        ClearItemForm()
    End Sub

    Protected Sub GridViewTransferDetails_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim productID = Convert.ToInt32(GridViewTransferDetails.DataKeys(e.RowIndex).Value)
        TransferItems.RemoveAll(Function(item) item.ProductID = productID)
    End Sub

    Protected Sub btnExecuteTransfer_Click(sender As Object, e As EventArgs) Handles btnExecuteTransfer.Click
        lblMessage.Text = ""
        Try
            ' 1. تجميع بيانات أمر التحويل
            Dim newTransfer As New StockTransfer()
            newTransfer.Details = Me.TransferItems
            newTransfer.SourceBranchID = Convert.ToInt32(ddlSourceBranch.SelectedValue)
            newTransfer.DestinationBranchID = Convert.ToInt32(ddlDestinationBranch.SelectedValue)
            newTransfer.TransferDate = Convert.ToDateTime(txtTransferDate.Text)
            newTransfer.Notes = txtNotes.Text

            Dim currentUser As User = TryCast(Session("LoggedInUser"), User)
            If currentUser Is Nothing Then Throw New Exception("انتهت جلسة العمل، يرجى تسجيل الدخول.")
            newTransfer.UserID = currentUser.UserID

            ' 2. استدعاء BLL لحفظ التحويل
            Dim newTransferId = transferBll.SaveStockTransfer(newTransfer)

            lblMessage.Text = "تم تنفيذ التحويل بنجاح برقم: " & newTransferId.ToString()
            lblMessage.ForeColor = System.Drawing.Color.Green
            ClearPage()

        Catch ex As Exception
            lblMessage.Text = "فشل تنفيذ التحويل: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

    ' --- دوال مساعدة ---
    Private Sub BindBranches()
        Dim branches = branchBll.GetAllBranches()
        ddlSourceBranch.DataSource = branches
        ddlSourceBranch.DataTextField = "BranchName"
        ddlSourceBranch.DataValueField = "BranchID"
        ddlSourceBranch.DataBind()
        ddlSourceBranch.Items.Insert(0, New ListItem("اختر فرع المصدر...", "0"))

        ddlDestinationBranch.DataSource = branches
        ddlDestinationBranch.DataTextField = "BranchName"
        ddlDestinationBranch.DataValueField = "BranchID"
        ddlDestinationBranch.DataBind()
        ddlDestinationBranch.Items.Insert(0, New ListItem("اختر فرع المستلم...", "0"))
    End Sub

    Private Sub BindGrid()
        GridViewTransferDetails.DataSource = TransferItems
        GridViewTransferDetails.DataBind()
    End Sub

    Private Sub ClearItemForm()
        txtProductSearch.Text = ""
        hdnSelectedProductID.Value = ""
        txtQuantity.Text = ""
        txtProductSearch.Focus()
    End Sub

    Private Sub ClearPage()
        TransferItems.Clear()
        ClearItemForm()
        ddlSourceBranch.SelectedIndex = 0
        ddlDestinationBranch.SelectedIndex = 0
        txtNotes.Text = ""
    End Sub
End Class