' GenerateBarcode.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class GenerateBarcode
    Inherits BasePage ' نرث من BasePage لتطبيق الأمان

    ' لا نحتاج لتعريف BLL هنا لأننا لا نتفاعل مع قاعدة البيانات في هذه الصفحة
    ' خدمة الويب هي التي تتفاعل مع BLL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' لا نحتاج لكود هنا في تحميل الصفحة
    End Sub

    Protected Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        lblMessage.Visible = False
        PanelBarcodeResult.Visible = False
        Try
            ' 1. التحقق من المدخلات
            Dim productId As Integer = 0
            Integer.TryParse(hdnSelectedProductID.Value, productId)

            Dim price As Decimal = 0
            Decimal.TryParse(txtPrice.Text, price)

            If productId = 0 Then Throw New Exception("الرجاء اختيار منتج من قائمة البحث.")
            If price <= 0 Then Throw New Exception("الرجاء إدخال سعر صالح أكبر من صفر.")
            'If price > 999.99 Then Throw New Exception("السعر يجب أن يكون أقل من 1000.") ' لأن لدينا 5 خانات للسعر
            If productId > 99999 Then
                Throw New Exception("رقم المنتج كبير جدًا ولا يمكن إنشاء باركود له.")
            End If

            ' لدينا 5 خانات للسعر (بعد ضربه في 100)، لذا أقصى سعر هو 999.99
            If price >= 1000 Then
                Throw New Exception("السعر كبير جدًا. يجب أن يكون أقل من 1000.")
            End If
            ' 2. بناء أجزاء الباركود
            Dim prefix As String = "21" ' بادئة السعر
            ' كود المنتج من 5 خانات (يمكن تعديل هذا الرقم حسب الحاجة)
            Dim productCode As String = productId.ToString().PadLeft(5, "0"c)
            ' قيمة السعر من 5 خانات (مثال: 12.50 ريال تصبح 1250)
            Dim priceValue As String = (price * 100).ToString("F0").PadLeft(5, "0"c)

            ' التأكد من أن طول الأجزاء صحيح


            Dim barcodeWithoutCheckDigit As String = prefix & productCode & priceValue
            If barcodeWithoutCheckDigit.Length <> 12 Then
                ' هذا الشرط كحماية إضافية
                Throw New Exception("حدث خطأ داخلي أثناء تكوين الباركود.")
            End If
            ' 3. حساب رقم التحقق (Check Digit)
            Dim checkDigit As Integer = CalculateEAN13CheckDigit(barcodeWithoutCheckDigit)

            ' 4. الباركود النهائي
            Dim finalBarcode As String = barcodeWithoutCheckDigit & checkDigit.ToString()

            ' 5. عرض النتائج في الواجهة
            litProductName.Text = txtProductSearch.Text ' عرض اسم المنتج الذي اختاره المستخدم
            PanelBarcodeResult.Visible = True

            ' 6. إرسال الباركود إلى JavaScript ليتم عرضه
            Dim script As String = "JsBarcode('#barcodeCanvas', '" & finalBarcode & "', { format: 'EAN13', displayValue: true, fontSize: 18, margin: 10 });"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GenerateBarcodeScript", script, True)

        Catch ex As Exception
            lblMessage.Text = "خطأ: " & ex.Message
            lblMessage.Visible = True
        End Try
    End Sub


    Private Function CalculateEAN13CheckDigit(ByVal barcode As String) As Integer
        If barcode Is Nothing OrElse barcode.Length <> 12 OrElse Not IsNumeric(barcode) Then
            Throw New ArgumentException("الباركود يجب أن يتكون من 12 رقمًا.")
        End If

        Dim sum As Integer = 0
        For i As Integer = 0 To barcode.Length - 1
            Dim digit = Integer.Parse(barcode(i))
            If (i + 1) Mod 2 = 0 Then
                sum += digit * 3
            Else
                sum += digit * 1
            End If
        Next
        Dim checkDigit = (10 - (sum Mod 10)) Mod 10
        Return checkDigit
    End Function

End Class