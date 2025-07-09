' WholesaleInvoice.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class WholesaleInvoice
    Inherits BasePage ' نرث من BasePage لتطبيق الصلاحيات

    Private salesBll As New SalesInvoiceBLL()
    Private branchBll As New BranchBLL()

    ' خاصية لإدارة الأصناف المؤقتة
    Private Property InvoiceItems As List(Of SalesInvoiceDetail)
        Get
            If ViewState("InvoiceItems") Is Nothing Then
                ViewState("InvoiceItems") = New List(Of SalesInvoiceDetail)()
            End If
            Return CType(ViewState("InvoiceItems"), List(Of SalesInvoiceDetail))
        End Get
        Set(value As List(Of SalesInvoiceDetail))
            ViewState("InvoiceItems") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            PopulateDropDowns()
            txtInvoiceDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
        End If
    End Sub

    Private Sub PopulateDropDowns()
        ddlBranch.DataSource = branchBll.GetAllBranches()
        ddlBranch.DataTextField = "BranchName"
        ddlBranch.DataValueField = "BranchID"
        ddlBranch.DataBind()
    End Sub

    Protected Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        ' التحقق من المدخلات
        Dim productId As Integer = 0
        Integer.TryParse(hdnSelectedProductID.Value, productId)
        If productId = 0 OrElse String.IsNullOrWhiteSpace(txtQuantity.Text) OrElse String.IsNullOrWhiteSpace(txtSellingPrice.Text) Then
            ' عرض رسالة خطأ
            Return
        End If

        Dim newItem As New SalesInvoiceDetail()
        newItem.ProductID = productId
        newItem.ProductName = txtProductSearch.Text
        newItem.Quantity = Convert.ToDecimal(txtQuantity.Text)
        newItem.UnitPrice = Convert.ToDecimal(txtSellingPrice.Text)
        newItem.SubTotal = newItem.Quantity * newItem.UnitPrice

        InvoiceItems.Add(newItem)
        BindGrid()
        ClearItemForm()
    End Sub

    Protected Sub GridViewInvoiceDetails_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim productID = Convert.ToInt32(GridViewInvoiceDetails.DataKeys(e.RowIndex).Value)
        InvoiceItems.RemoveAll(Function(item) item.ProductID = productID)
        BindGrid()
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSaveAsCredit.Click, btnSaveAsCash.Click
        lblMessage.Text = ""
        Try
            If Not InvoiceItems.Any() Then Throw New Exception("يجب إضافة أصناف للفاتورة أولاً.")
            If String.IsNullOrWhiteSpace(hdnSelectedCustomerID.Value) Then Throw New Exception("يجب اختيار العميل.")

            Dim invoiceToSave As New SalesInvoice()
            invoiceToSave.Details = Me.InvoiceItems
            invoiceToSave.CustomerID = Convert.ToInt32(hdnSelectedCustomerID.Value)
            invoiceToSave.BranchID = Convert.ToInt32(ddlBranch.SelectedValue)
            invoiceToSave.InvoiceDate = Convert.ToDateTime(txtInvoiceDate.Text)
            invoiceToSave.UserID = CurrentUser.UserID ' من BasePage

            Dim paymentMethod = CType(sender, Button).CommandArgument
            invoiceToSave.PaymentMethod = paymentMethod

            Dim finalAmount = InvoiceItems.Sum(Function(i) i.SubTotal)
            invoiceToSave.TotalAmount = finalAmount ' يمكن إضافة خصم لاحقًا

            If paymentMethod = "Credit" Then
                invoiceToSave.AmountPaid = 0
            Else
                invoiceToSave.AmountPaid = finalAmount
            End If

            Dim newInvoiceId = salesBll.SaveInvoice(invoiceToSave)
            lblMessage.Text = "تم حفظ الفاتورة بنجاح برقم: " & newInvoiceId
            lblMessage.ForeColor = System.Drawing.Color.Green
            ' === كود فتح نافذة الطباعة النظيف ===
            Dim printUrl As String = ResolveUrl("~/Modules/Sales/PrintWholesaleInvoice.aspx?ID=" & newInvoiceId.ToString())
            Dim printScript As String = "window.open('" & printUrl & "', '_blank', 'width=850,height=900,scrollbars=yes,resizable=yes');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PrintWholesaleInvoice", printScript, True)

            ClearPage()


        Catch ex As Exception
            lblMessage.Text = "فشل الحفظ: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

    Private Sub BindGrid()
        GridViewInvoiceDetails.DataSource = InvoiceItems
        GridViewInvoiceDetails.DataBind()
    End Sub

    Private Sub ClearItemForm()
        txtProductSearch.Text = ""
        hdnSelectedProductID.Value = ""
        txtQuantity.Text = ""
        txtSellingPrice.Text = ""
    End Sub

    Private Sub ClearPage()
        InvoiceItems.Clear()
        BindGrid()
        ClearItemForm()
        txtCustomerSearch.Text = ""
        hdnSelectedCustomerID.Value = ""
        ddlBranch.SelectedIndex = 0
    End Sub

    ' لعرض الإجماليات في الفوتر
    Protected Sub GridViewInvoiceDetails_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridViewInvoiceDetails.RowDataBound
        If e.Row.RowType = DataControlRowType.Footer Then
            Dim total As Decimal = InvoiceItems.Sum(Function(i) i.SubTotal)
            e.Row.Cells(2).Text = "الإجمالي"
            e.Row.Cells(3).Text = total.ToString("N2")
        End If
    End Sub
End Class