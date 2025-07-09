' SuperMarket.BLL/AccountBLL.vb
Imports SuperMarket.DAL
Imports SuperMarket.Entities

Public Class AccountBLL
    Private dal As New AccountDAL()

    Public Function GetAllAccounts() As List(Of Account)
        Return dal.GetAllAccounts()
    End Function

    Public Sub AddAccount(ByVal acc As Account)
        ' ==========================================================
        ' === قواعد التحقق (Business Rules) ===
        ' ==========================================================

        If String.IsNullOrWhiteSpace(acc.AccountNumber) Then
            Throw New Exception("رقم الحساب مطلوب.")
        End If

        If String.IsNullOrWhiteSpace(acc.AccountName) Then
            Throw New Exception("اسم الحساب مطلوب.")
        End If

        ' (اختياري) يمكنك إضافة تحقق هنا للتأكد من أن رقم الحساب فريد قبل إرساله إلى قاعدة البيانات
        ' Dim existingAccounts = GetAllAccounts()
        ' If existingAccounts.Any(Function(a) a.AccountNumber = acc.AccountNumber) Then
        '     Throw New Exception("رقم الحساب موجود بالفعل.")
        ' End If

        ' (اختياري) يمكنك إضافة تحقق هنا للتأكد من أن رقم الحساب الجديد يتبع تسلسل رقم الحساب الأب
        ' ...

        dal.AddAccount(acc)
    End Sub
    ' أضف هذه الدالة داخل كلاس AccountBLL

    Public Sub UpdateAccount(ByVal acc As Account)
        ' قواعد التحقق
        If acc.AccountID <= 0 Then
            Throw New Exception("لم يتم تحديد الحساب المراد تعديله.")
        End If
        If String.IsNullOrWhiteSpace(acc.AccountNumber) OrElse String.IsNullOrWhiteSpace(acc.AccountName) Then
            Throw New Exception("رقم واسم الحساب مطلوبان.")
        End If

        dal.UpdateAccount(acc)
    End Sub
    ' أضف هذه الدالة داخل كلاس AccountBLL

    Public Sub DeleteAccount(ByVal accountId As Integer)
        ' --- التحقق من الشروط قبل الحذف ---

        ' 1. جلب كل الحسابات لفحص العلاقات
        Dim allAccs = GetAllAccounts()

        ' 2. التحقق مما إذا كان الحساب له أبناء
        If allAccs.Any(Function(acc) acc.ParentAccountID.HasValue AndAlso acc.ParentAccountID.Value = accountId) Then
            Throw New Exception("لا يمكن حذف هذا الحساب لأنه حساب رئيسي يحتوي على حسابات فرعية.")
        End If

        ' 3. التحقق مما إذا كان الحساب عليه حركات
        ' (هذا الجزء يتطلب الوصول إلى جدول JournalVoucherDetails)
        ' سنحتاج إلى إنشاء دالة بسيطة في JournalVoucherDAL للتحقق من ذلك.
        Dim jvDal As New JournalVoucherDAL() ' افترض أننا أنشأنا هذا الكلاس
        If jvDal.HasTransactions(accountId) Then
            Throw New Exception("لا يمكن حذف هذا الحساب لارتباطه بحركات مالية (قيود يومية).")
        End If

        ' إذا تجاوز كل الشروط، قم بالحذف
        dal.DeleteAccount(accountId)
    End Sub
End Class