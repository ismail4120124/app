' SuperMarket.BLL/ExpenseBLL.vb
Imports SuperMarket.DAL
Imports SuperMarket.Entities
Imports System.Transactions

Public Class ExpenseBLL
    Private dal As New ExpenseDAL()

    Public Sub SaveExpense(ByVal expense As Expense)
        ' قواعد التحقق
        If expense.Amount <= 0 Then Throw New Exception("مبلغ المصروف يجب أن يكون أكبر من صفر.")
        If expense.CategoryID <= 0 Then Throw New Exception("يجب اختيار بند المصروف.")
        If expense.PaidFromAccountID <= 0 Then Throw New Exception("يجب تحديد الحساب الذي تم الدفع منه.")

        Using scope As New TransactionScope()
            ' 1. حفظ سجل المصروف
            Dim newExpenseId = dal.AddExpense(expense)
            If newExpenseId <= 0 Then Throw New Exception("فشل حفظ المصروف.")

            ' 2. إنشاء القيد المحاسبي
            Dim jvBll As New JournalVoucherBLL()
            Dim jv As New JournalVoucher()
            jv.VoucherDate = expense.ExpenseDate
            jv.Description = "قيد مصروف: " & expense.Description

            ' --- أطراف القيد ---
            ' أ. جلب الحساب المرتبط ببند المصروف
            Dim expenseCatBll As New ExpenseCategoryBLL()
            Dim expenseCategory = expenseCatBll.GetAllExpenseCategories().FirstOrDefault(Function(c) c.CategoryID = expense.CategoryID)
            If expenseCategory Is Nothing Then Throw New Exception("بند المصروف غير موجود.")

            ' الطرف المدين (من حـ/ حساب المصروف)
            jv.Details.Add(New JournalVoucherDetail With {
                .AccountID = expenseCategory.AccountID,
                .Debit = expense.Amount
            })

            ' الطرف الدائن (إلى حـ/ حساب الصندوق أو البنك)
            jv.Details.Add(New JournalVoucherDetail With {
                .AccountID = expense.PaidFromAccountID,
                .Credit = expense.Amount
            })

            ' حفظ القيد
            Dim newJvId = jvBll.SaveJournalVoucher(jv)

            ' 3. تحديث سجل المصروف برقم القيد (للتتبع)
            dal.UpdateExpenseWithJournalVoucherId(newExpenseId, newJvId)

            scope.Complete()
        End Using
    End Sub
    Public Function GetRecentExpenses(Optional ByVal recordCount As Integer = 20) As List(Of Expense)
        Return dal.GetRecentExpenses(recordCount)
    End Function
End Class
