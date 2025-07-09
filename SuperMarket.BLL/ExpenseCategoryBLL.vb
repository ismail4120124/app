' SuperMarket.BLL/ExpenseCategoryBLL.vb
Imports SuperMarket.DAL
Imports SuperMarket.Entities

Public Class ExpenseCategoryBLL
    Private dal As New ExpenseCategoryDAL()

    Public Function GetAllExpenseCategories() As List(Of ExpenseCategory)
        Return dal.GetAllExpenseCategories()
    End Function

    Public Sub AddExpenseCategory(ByVal category As ExpenseCategory)
        ' قواعد التحقق الأساسية
        If String.IsNullOrWhiteSpace(category.CategoryName) Then
            Throw New Exception("اسم بند المصروف مطلوب.")
        End If

        If category.AccountID <= 0 Then
            Throw New Exception("يجب ربط بند المصروف بحساب محاسبي.")
        End If

        ' (اختياري) يمكنك إضافة تحقق هنا للتأكد من أن اسم البند فريد
        ' ...

        dal.AddExpenseCategory(category)
    End Sub

End Class