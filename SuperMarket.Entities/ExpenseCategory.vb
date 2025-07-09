' SuperMarket.Entities/ExpenseCategory.vb

<Serializable()>
Public Class ExpenseCategory
    Public Property CategoryID As Integer
    Public Property CategoryName As String

    ' ID الحساب المحاسبي المرتبط بهذا البند
    Public Property AccountID As Integer

    Public Property IsActive As Boolean

    ' خاصية إضافية للعرض فقط (ليست في قاعدة البيانات)
    ' سنستخدمها لعرض اسم الحساب المحاسبي في GridView
    Public Property AccountName As String
End Class
