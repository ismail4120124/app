' PrintInvoice.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class PrintInvoice
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim invoiceId As Long = 0
            If Long.TryParse(Request.QueryString("ID"), invoiceId) AndAlso invoiceId > 0 Then
                LoadAndBindInvoiceData(invoiceId)

                ' إضافة سكربت الطباعة التلقائية
                ClientScript.RegisterStartupScript(Me.GetType(), "Print", "window.onload = function() { window.print(); };", True)
            Else
                ' عرض رسالة خطأ إذا لم يتم تمرير ID
            End If
        End If
    End Sub
    Private Sub LoadAndBindInvoiceData(ByVal invoiceId As Long)
        Dim salesBll As New SalesInvoiceBLL()
        Dim invoiceData = salesBll.GetInvoiceForPrinting(invoiceId)

        If invoiceData IsNot Nothing Then
            ' تعبئة بيانات رأس الفاتورة
            ' litCompanyName.Text = ... ' يمكنك جلبها من SystemSettings
            litBranchName.Text = invoiceData.BranchName
            litInvoiceID.Text = invoiceData.InvoiceID.ToString()
            litInvoiceDate.Text = invoiceData.InvoiceDate.ToString("yyyy-MM-dd hh:mm tt")
            litCashierName.Text = invoiceData.UserName
            litCustomerName.Text = invoiceData.CustomerName

            ' تعبئة جدول الأصناف
            GridViewItems.DataSource = invoiceData.Details
            GridViewItems.DataBind()

            ' تعبئة الإجماليات
            litTotal.Text = invoiceData.TotalAmount.ToString("N2")
            litDiscount.Text = invoiceData.DiscountAmount.ToString("N2")
            Dim grandTotal = invoiceData.TotalAmount - invoiceData.DiscountAmount
            litGrandTotal.Text = grandTotal.ToString("N2")
        Else
            ' في حالة عدم العثور على الفاتورة
            Response.Write("<h2>الفاتورة غير موجودة.</h2>")
            ' إيقاف سكربت الطباعة
            ClientScript.RegisterStartupScript(Me.GetType(), "Print", "alert('الفاتورة غير موجودة');", True)
        End If
    End Sub

End Class