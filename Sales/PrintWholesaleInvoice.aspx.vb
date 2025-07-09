' PrintWholesaleInvoice.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class PrintWholesaleInvoice
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim invoiceId As Long = 0
            If Long.TryParse(Request.QueryString("ID"), invoiceId) AndAlso invoiceId > 0 Then
                LoadAndBindInvoiceData(invoiceId)
            Else
                ' إخفاء الفاتورة وعرض رسالة خطأ
                btnPrint.Visible = False
                Response.Write("<div class='alert alert-danger text-center'>رقم الفاتورة غير صحيح أو مفقود.</div>")
            End If
        End If
    End Sub

    Private Sub LoadAndBindInvoiceData(ByVal invoiceId As Long)
        Dim salesBll As New SalesInvoiceBLL()
        ' نفترض أن دالة GetInvoiceForPrinting موجودة في BLL وتعمل بشكل صحيح
        Dim invoiceData = salesBll.GetInvoiceForPrinting(invoiceId)

        If invoiceData IsNot Nothing Then
            ' تعبئة بيانات الفاتورة
            litInvoiceID.Text = invoiceData.InvoiceID.ToString("D8") ' رقم فاتورة من 8 خانات
            litInvoiceDate.Text = invoiceData.InvoiceDate.ToString("yyyy/MM/dd")

            ' تعبئة بيانات العميل
            litCustomerName.Text = invoiceData.CustomerName

            ' تعبئة بيانات الشركة/الفرع (يمكن جلبها من SystemSettings لاحقًا)
            litCompanyName.Text = "سوبرماركت برو"
            litBranchName.Text = invoiceData.BranchName

            ' تعبئة جدول الأصناف
            GridViewItems.DataSource = invoiceData.Details
            GridViewItems.DataBind()

            ' تعبئة الإجماليات
            litTotal.Text = invoiceData.TotalAmount.ToString("N2")
            litDiscount.Text = invoiceData.DiscountAmount.ToString("N2")
            Dim grandTotal = invoiceData.TotalAmount - invoiceData.DiscountAmount
            litGrandTotal.Text = grandTotal.ToString("N2")
        Else
            btnPrint.Visible = False
            Response.Write("<div class='alert alert-danger text-center'>لم يتم العثور على الفاتورة المطلوبة.</div>")
        End If
    End Sub

End Class