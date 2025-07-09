' SuperMarket.BLL/ShiftBLL.vb
Imports SuperMarket.DAL
Imports SuperMarket.Entities

Public Class ShiftBLL
    Private dal As New ShiftDAL()

    ''' <summary>
    ''' يبحث عن أي وردية مفتوحة حاليًا لمستخدم وفرع معينين.
    ''' </summary>
    Public Function GetOpenShiftForUser(ByVal userId As Integer, ByVal branchId As Integer) As CashierShift
        If userId <= 0 OrElse branchId <= 0 Then
            Return Nothing
        End If
        Return dal.GetOpenShiftForUser(userId, branchId)
    End Function

    ''' <summary>
    ''' يقوم بفتح وردية جديدة لمستخدم بعد التحقق من عدم وجود وردية مفتوحة له بالفعل.
    ''' </summary>
    Public Function OpenShift(ByVal shift As CashierShift) As Integer
        ' قاعدة عمل: تأكد من أن المبلغ الافتتاحي ليس سالبًا
        If shift.OpeningBalance < 0 Then
            Throw New Exception("الرصيد الافتتاحي لا يمكن أن يكون سالبًا.")
        End If

        ' قاعدة عمل: تأكد من أن هذا المستخدم ليس لديه وردية مفتوحة بالفعل
        Dim existingShift = GetOpenShiftForUser(shift.UserID, shift.BranchID)
        If existingShift IsNot Nothing Then
            Throw New Exception("يوجد بالفعل وردية مفتوحة لهذا المستخدم. يجب إغلاقها أولاً.")
        End If

        Return dal.OpenShift(shift)
    End Function

    ''' <summary>
    ''' يجلب ملخص المبيعات لوردية معينة.
    ''' </summary>
    Public Function GetShiftSalesSummary(ByVal shiftId As Integer) As Object
        If shiftId <= 0 Then
            Return Nothing
        End If
        Return dal.GetShiftSalesSummary(shiftId)
    End Function

    ''' <summary>
    ''' يقوم بإغلاق وردية مفتوحة بعد التحقق من البيانات.
    ''' </summary>
    Public Sub CloseShift(ByVal shiftId As Integer, ByVal closingBalance As Decimal, ByVal systemSales As Decimal)
        If shiftId <= 0 Then
            Throw New Exception("رقم الوردية غير صالح.")
        End If

        If closingBalance < 0 Then
            Throw New Exception("رصيد الإغلاق لا يمكن أن يكون سالبًا.")
        End If

        dal.CloseShift(shiftId, closingBalance, systemSales)
    End Sub

End Class