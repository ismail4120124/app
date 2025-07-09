' SuperMarket.Entities/Product.vb
Imports System ' قد تحتاج لإضافة هذا السطر إذا لم يكن موجودًا

<Serializable()>
Public Class Product
    Public Property ProductID As Integer
    Public Property Barcode As String
    Public Property ProductName As String
    Public Property Description As String
    Public Property CategoryID As Integer
    Public Property UnitID As Integer
    Public Property ReorderLevel As Decimal
    Public Property ImagePath As String

    ' خصائص إضافية للعرض (ليست في الجدول الرئيسي)
    Public Property CategoryName As String
    Public Property UnitName As String
End Class
