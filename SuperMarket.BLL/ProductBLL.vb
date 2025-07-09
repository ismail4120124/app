' SuperMarket.BLL/ProductBLL.vb
Imports SuperMarket.DAL
Imports SuperMarket.Entities

Public Class ProductBLL
    Private dal As New ProductDAL()

    Public Sub AddProduct(ByVal product As Product)
        If String.IsNullOrWhiteSpace(product.ProductName) Then
            Throw New Exception("اسم المنتج مطلوب.")
        End If
        ' يمكنك إضافة المزيد من قواعد التحقق هنا
        dal.AddProduct(product)
    End Sub

    Public Function GetAllProducts() As List(Of Product)
        Return dal.GetAllProducts()
    End Function
    Public Function SearchProduct(ByVal searchTerm As String) As Product
        Return dal.SearchProduct(searchTerm)
    End Function
    ' في ProductBLL.vb

    Public Function SearchProductForSale(ByVal searchTerm As String, ByVal branchId As Integer) As SalesInvoiceDetail
        If String.IsNullOrWhiteSpace(searchTerm) OrElse branchId <= 0 Then
            Return Nothing
        End If
        Return dal.SearchProductForSale(searchTerm, branchId)
    End Function
    ' في نهاية كلاس ProductBLL.vb

    ''' <summary>
    ''' يجلب تقرير حركة صنف بعد التحقق من صحة المدخلات.
    ''' </summary>
    Public Function GetItemLedger(ByVal productId As Integer, ByVal branchId As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of ItemLedgerEntry)
        ' التحقق من صحة المدخلات
        If productId <= 0 OrElse branchId <= 0 Then
            Throw New Exception("يجب اختيار صنف وفرع لعرض التقرير.")
        End If

        If startDate > endDate Then
            Throw New Exception("تاريخ البداية لا يمكن أن يكون بعد تاريخ النهاية.")
        End If

        ' استدعاء دالة DAL لجلب البيانات
        Return dal.GetItemLedger(productId, branchId, startDate, endDate)
    End Function
    ' في نهاية كلاس ProductBLL.vb

    ''' <summary>
    ''' دالة وسيطة للبحث المبسط عن المنتجات.
    ''' </summary>
    Public Function SearchProductsSimple(ByVal searchTerm As String, ByVal maxRows As Integer) As List(Of Product)
        ' يمكن إضافة تحقق هنا إذا كان النص قصيرًا جدًا
        If String.IsNullOrWhiteSpace(searchTerm) OrElse searchTerm.Length < 2 Then
            Return New List(Of Product)() ' إرجاع قائمة فارغة
        End If

        Return dal.SearchProductsSimple(searchTerm, maxRows)
    End Function
    ' في نهاية كلاس ProductBLL.vb

    Public Function GetProductsForPricing(ByVal branchId As Integer) As List(Of ProductPricingInfo)
        If branchId <= 0 Then
            Throw New Exception("يجب اختيار فرع صالح.")
        End If
        Return dal.GetProductsForPricing(branchId)
    End Function

    Public Sub UpdateProductPrices(ByVal pricingList As List(Of ProductPricingInfo), ByVal branchId As Integer)
        If pricingList Is Nothing OrElse Not pricingList.Any() Then
            ' لا يوجد شيء لتحديثه
            Return
        End If
        If branchId <= 0 Then
            Throw New Exception("يجب اختيار فرع صالح.")
        End If

        ' يمكنك إضافة تحقق هنا للتأكد من أن الأسعار الجديدة ليست أقل من التكلفة، إلخ.

        dal.UpdateProductPrices(pricingList, branchId)
    End Sub
    ' في ProductBLL.vb

    Public Function GetProductsForOpeningBalance(ByVal branchId As Integer) As List(Of ProductPricingInfo)
        If branchId <= 0 Then Throw New Exception("يجب اختيار فرع.")
        Return dal.GetProductsForOpeningBalance(branchId)
    End Function

    Public Sub SaveOpeningBalance(ByVal itemList As List(Of ProductPricingInfo), ByVal branchId As Integer)
        If branchId <= 0 Then Throw New Exception("يجب اختيار فرع.")
        If Not itemList.Any() Then Return ' لا يوجد شيء للحفظ

        ' يمكنك إضافة تحقق هنا للتأكد من أن القيم المدخلة ليست سالبة

        dal.SaveOpeningBalance(itemList, branchId)
    End Sub
End Class