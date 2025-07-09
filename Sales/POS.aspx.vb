' POS.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class POS
    Inherits BasePage

    ' ==========================================================
    ' === 1. تعريف كائنات طبقة منطق العمل (BLL) ===
    ' ==========================================================
    Private productBll As New ProductBLL()
    Private salesBll As New SalesInvoiceBLL()
    Private shiftBll As New ShiftBLL()

    ' ==========================================================
    ' === 2. إدارة الفواتير المعلقة (Suspend/Resume Logic) ===
    ' ==========================================================
    Private Property SuspendedInvoices As List(Of List(Of SalesInvoiceDetail))
        Get
            If Session("SuspendedInvoices") Is Nothing Then
                Session("SuspendedInvoices") = New List(Of List(Of SalesInvoiceDetail))()
            End If
            Return CType(Session("SuspendedInvoices"), List(Of List(Of SalesInvoiceDetail)))
        End Get
        Set(value As List(Of List(Of SalesInvoiceDetail)))
            Session("SuspendedInvoices") = value
        End Set
    End Property

    Private Property ActiveInvoiceIndex As Integer
        Get
            If ViewState("ActiveInvoiceIndex") Is Nothing Then ViewState("ActiveInvoiceIndex") = 0
            Return CType(ViewState("ActiveInvoiceIndex"), Integer)
        End Get
        Set(value As Integer)
            ViewState("ActiveInvoiceIndex") = value
        End Set
    End Property

    Private ReadOnly Property POSItems As List(Of SalesInvoiceDetail)
        Get
            If SuspendedInvoices.Count = 0 Then
                SuspendedInvoices.Add(New List(Of SalesInvoiceDetail)())
                ActiveInvoiceIndex = 0
            End If
            If ActiveInvoiceIndex >= SuspendedInvoices.Count Then ActiveInvoiceIndex = 0
            Return SuspendedInvoices(ActiveInvoiceIndex)
        End Get
    End Property

    ' ==========================================================
    ' === 3. أحداث دورة حياة الصفحة (Page Lifecycle Events) ===
    ' ==========================================================
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then InitializePOS()
    End Sub

    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        BindGridAndUpdateTotal()
    End Sub

    ' ==========================================================
    ' === 4. أحداث الكنترولات الرئيسية (Control Events) ===
    ' ==========================================================
    Protected Sub txtBarcode_TextChanged(sender As Object, e As EventArgs) Handles txtBarcode.TextChanged
        PanelNewProduct.Visible = False
        lblMessage.Text = ""
        Dim inputText As String = txtBarcode.Text.Trim()
        If String.IsNullOrWhiteSpace(inputText) Then Return

        Dim quantity As Decimal = 1
        Dim barcode As String = inputText
        If inputText.Contains("*") Then
            Dim parts() As String = inputText.Split("*"c)
            If parts.Length = 2 AndAlso IsNumeric(parts(0)) AndAlso Convert.ToDecimal(parts(0)) > 0 AndAlso Not String.IsNullOrWhiteSpace(parts(1)) Then
                quantity = Convert.ToDecimal(parts(0))
                barcode = parts(1).Trim()
            End If
        End If

        Dim currentUser As User = TryCast(Session("LoggedInUser"), User)
        If currentUser Is Nothing Then
            lblMessage.Text = "انتهت جلسة العمل."
            Return
        End If

        Dim foundItem As SalesInvoiceDetail = productBll.SearchProductForSale(barcode, currentUser.BranchID)

        If foundItem IsNot Nothing Then
            foundItem.Quantity = quantity
            foundItem.SubTotal = foundItem.Quantity * foundItem.UnitPrice
            AddItemToPOS(foundItem)
        Else
            PanelNewProduct.Visible = True
            ViewState("NewBarcode") = barcode
        End If

        Dim script As String = "setTimeout(function(){ document.getElementById('" & txtBarcode.ClientID & "').value = ''; document.getElementById('" & txtBarcode.ClientID & "').focus(); }, 0);"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ResetBarcodeScript", script, True)
    End Sub

    Protected Sub GridViewPOS_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim productID As Integer = Convert.ToInt32(GridViewPOS.DataKeys(e.RowIndex).Value)
        POSItems.RemoveAll(Function(p) p.ProductID = productID)
    End Sub

    Protected Sub txtDiscount_TextChanged(sender As Object, e As EventArgs) Handles txtDiscount.TextChanged
        ' لا نحتاج لكود هنا، PreRender سيقوم بالتحديث
    End Sub



    ' ==========================================================
    ' === 5. أحداث إدارة الفواتير والعمليات ===
    ' ==========================================================
    Protected Sub btnAddNewProduct_Click(sender As Object, e As EventArgs) Handles btnAddNewProduct.Click
        Dim newBarcode As String = If(ViewState("NewBarcode") IsNot Nothing, ViewState("NewBarcode").ToString(), "")
        Dim url As String = ResolveUrl("~/Modules/Inventory/Products.aspx?barcode=" & newBarcode)
        Dim script As String = "window.open('" & url & "', '_blank');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenProductsPage", script, True)
    End Sub

    Protected Sub btnSuspendInvoice_Click(sender As Object, e As EventArgs) Handles btnSuspendInvoice.Click
        If POSItems.Any() Then
            SuspendedInvoices.Add(New List(Of SalesInvoiceDetail)())
            ActiveInvoiceIndex = SuspendedInvoices.Count - 1
            lblMessage.Text = "تم تعليق الفاتورة. يمكنك البدء بفاتورة جديدة."
            lblMessage.ForeColor = System.Drawing.Color.Blue
            PanelSuspendedList.Visible = False
        End If
    End Sub

    Protected Sub btnNewInvoice_Click(sender As Object, e As EventArgs) Handles btnNewInvoice.Click
        SuspendedInvoices.Add(New List(Of SalesInvoiceDetail)())
        ActiveInvoiceIndex = SuspendedInvoices.Count - 1
        lblMessage.Text = "تم إنشاء فاتورة جديدة."
        lblMessage.ForeColor = System.Drawing.Color.IndianRed
        PanelSuspendedList.Visible = False
    End Sub

    Protected Sub btnResume_Click(sender As Object, e As EventArgs) Handles btnResume.Click
        RepeaterSuspended.DataSource = SuspendedInvoices
        RepeaterSuspended.DataBind()
        PanelSuspendedList.Visible = True
    End Sub

    Protected Sub RepeaterSuspended_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        If e.CommandName = "Resume" Then
            ActiveInvoiceIndex = Convert.ToInt32(e.CommandArgument)
            PanelSuspendedList.Visible = False
            lblMessage.Text = "تم استئناف الفاتورة رقم " & (ActiveInvoiceIndex + 1).ToString()
            lblMessage.ForeColor = System.Drawing.Color.Blue
            UpdatePanelInvoice.Update()
            UpdatePanelTotal.Update()
        End If
    End Sub

    ' في POS.aspx.vb

    Protected Sub btnPay_Click(sender As Object, e As EventArgs) Handles btnPayCash.Click, btnPayCard.Click, btnPayCredit.Click
        ' --- 1. التحقق من المدخلات الأساسية ---
        If Not POSItems.Any() Then
            lblMessage.Text = "الفاتورة فارغة!"
            lblMessage.ForeColor = System.Drawing.Color.Red
            Return
        End If

        Dim paymentMethod As String = CType(sender, Button).CommandArgument
        Dim customerId As Integer = Convert.ToInt32(hdnSelectedCustomerID.Value)

        ' التحقق من عدم البيع الآجل لعميل نقدي
        If paymentMethod = "Credit" AndAlso customerId = 1 Then
            lblMessage.Text = "لا يمكن البيع بالآجل لـ 'عميل نقدي'. يرجى اختيار عميل أولاً."
            lblMessage.ForeColor = System.Drawing.Color.OrangeRed
            Return
        End If

        ' التحقق من وجود مستخدم مسجل
        Dim currentUser As User = TryCast(Session("LoggedInUser"), User)
        If currentUser Is Nothing Then
            lblMessage.Text = "انتهت جلسة العمل. يرجى تسجيل الدخول."
            lblMessage.ForeColor = System.Drawing.Color.Red
            Return
        End If

        ' التحقق من وجود وردية مفتوحة
        Dim openShift = shiftBll.GetOpenShiftForUser(currentUser.UserID, currentUser.BranchID)
        If openShift Is Nothing Then
            lblMessage.Text = "لا يمكنك إتمام البيع. يرجى فتح وردية جديدة أولاً."
            lblMessage.ForeColor = System.Drawing.Color.Red
            Return
        End If

        ' --- 2. تجميع بيانات الفاتورة ---
        Dim invoiceToSave As New SalesInvoice()
        invoiceToSave.Details = Me.POSItems
        invoiceToSave.PaymentMethod = paymentMethod
        invoiceToSave.CustomerID = customerId
        Decimal.TryParse(txtDiscount.Text, invoiceToSave.DiscountAmount)
        invoiceToSave.InvoiceDate = DateTime.Now
        Dim subTotal As Decimal = Me.POSItems.Sum(Function(i) i.SubTotal)
        invoiceToSave.TotalAmount = subTotal
        Dim finalAmount = subTotal - invoiceToSave.DiscountAmount
        If paymentMethod = "Credit" Then
            invoiceToSave.AmountPaid = 0
        Else
            invoiceToSave.AmountPaid = finalAmount
        End If
        invoiceToSave.UserID = currentUser.UserID
        invoiceToSave.BranchID = currentUser.BranchID
        invoiceToSave.ShiftID = openShift.ShiftID

        ' --- 3. محاولة حفظ الفاتورة وتنفيذ الإجراءات اللاحقة ---
        Try
            ' استدعاء BLL لحفظ الفاتورة، تحديث المخزون، وإنشاء القيود
            Dim newInvoiceID As Long = salesBll.SaveInvoice(invoiceToSave)

            ' عرض رسالة نجاح
            lblMessage.Text = "تم حفظ الفاتورة بنجاح برقم: " & newInvoiceID.ToString()
            lblMessage.ForeColor = System.Drawing.Color.Green

            ' ==========================================================
            ' === التعديل المطلوب: فتح نافذة الطباعة ===
            ' ==========================================================
            Dim printUrl As String = ResolveUrl("~/Modules/Sales/PrintInvoice.aspx?ID=" & newInvoiceID.ToString())
            Dim printScript As String = "window.open('" & printUrl & "', 'PrintWin', 'width=380,height=650,scrollbars=yes,resizable=yes');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PrintInvoice", printScript, True)
            ' ==========================================================

            ' تحديث حالة نقاط البيع
            SuspendedInvoices.RemoveAt(ActiveInvoiceIndex)
            ActiveInvoiceIndex = 0
            If SuspendedInvoices.Count = 0 Then
                SuspendedInvoices.Add(New List(Of SalesInvoiceDetail)())
            End If
            ResetPOSState()

        Catch ex As Exception
            ' عرض رسالة الخطأ في حالة الفشل
            lblMessage.Text = "فشل حفظ الفاتورة: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

    ' ==========================================================
    ' === 6. الدوال المساعدة (Helper Methods) ===
    ' ==========================================================
    Private Sub InitializePOS()
        If SuspendedInvoices.Count = 0 Then
            SuspendedInvoices.Add(New List(Of SalesInvoiceDetail)())
        End If
        ActiveInvoiceIndex = 0
        hdnSelectedCustomerID.Value = "1"
        txtBarcode.Focus()
    End Sub

    Private Sub AddItemToPOS(ByVal itemToAdd As SalesInvoiceDetail)
        Dim existingItem = POSItems.FirstOrDefault(Function(p) p.ProductID = itemToAdd.ProductID)
        If existingItem IsNot Nothing Then
            existingItem.Quantity += itemToAdd.Quantity
            existingItem.SubTotal = existingItem.Quantity * existingItem.UnitPrice
        Else
            POSItems.Add(itemToAdd)
        End If
    End Sub

    Private Sub BindGridAndUpdateTotal()
        GridViewPOS.DataSource = POSItems
        GridViewPOS.DataBind()
        Dim subTotal As Decimal = If(POSItems.Any(), POSItems.Sum(Function(i) i.SubTotal), 0)
        lblSubTotal.Text = subTotal.ToString("N2")
        Dim discount As Decimal = 0
        If Not Decimal.TryParse(txtDiscount.Text, discount) Then discount = 0
        Dim grandTotal As Decimal = subTotal - discount
        lblGrandTotal.Text = grandTotal.ToString("N2")
    End Sub

    Private Sub ResetPOSState()
        PanelSuspendedList.Visible = False
        txtCustomerSearch.Text = ""
        hdnSelectedCustomerID.Value = "1"
        txtDiscount.Text = "0.00"
        txtAmountTendered.Text = "0.00"
        lblChange.Text = "0.00"
        divChangeResult.Visible = False
        txtBarcode.Focus()
    End Sub
    ' هذه هي النسخة الوحيدة التي يجب أن تكون موجودة في الملف


    Private Sub txtAmountTendered_TextChanged(sender As Object, e As EventArgs) Handles txtAmountTendered.TextChanged
        ' إظهار لوحة النتيجة
        divChangeResult.Visible = True

        ' 1. قراءة الإجمالي النهائي المطلوب
        Dim grandTotal As Decimal = 0
        Decimal.TryParse(lblGrandTotal.Text, grandTotal)

        ' 2. قراءة المبلغ المستلم من العميل
        Dim amountTendered As Decimal = 0
        Decimal.TryParse(txtAmountTendered.Text, amountTendered)

        ' الكلاس الأساسي للتصميم
        Dim baseClass As String = "card"

        ' 3. حساب المتبقي وتحديث الواجهة
        If amountTendered >= grandTotal Then
            Dim change As Decimal = amountTendered - grandTotal
            lblChange.Text = change.ToString("N2")
            ' تغيير لون إطار البطاقة إلى الأخضر للنجاح
            divChangeResult.Attributes("class") = baseClass & " border-success"
        Else
            lblChange.Text = "غير كافٍ"
            ' تغيير لون إطار البطاقة إلى الأحمر للخطأ
            divChangeResult.Attributes("class") = baseClass & " border-danger"
        End If
    End Sub
End Class