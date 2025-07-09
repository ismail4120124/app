' SuperMarket.Entities/SalesInvoiceDetail.vb
<Serializable()>
Public Class SalesInvoiceDetail
    Public Property DetailID As Long
    Public Property InvoiceID As Long
    Public Property ProductID As Integer
    Public Property Quantity As Decimal
    Public Property UnitPrice As Decimal ' سعر بيع الوحدة
    Public Property DiscountValue As Decimal
    Public Property SubTotal As Decimal ' هذا أيضًا حقل محسوب

    ' خصائص إضافية نحتاجها للعرض في شاشة نقاط البيع، لا يتم تخزينها في هذا الجدول
    Public Property ProductName As String
    Public Property Barcode As String

    ' === الخاصية الجديدة ===
    Public Property CostPrice As Decimal


End Class