' SuperMarket.BLL/JournalVoucherBLL.vb
Imports SuperMarket.DAL
Imports SuperMarket.Entities

Public Class JournalVoucherBLL
    Private dal As New JournalVoucherDAL()

    Public Function SaveJournalVoucher(ByVal jv As JournalVoucher) As Long
        ' التحقق من وجود تفاصيل
        If Not jv.Details.Any() Then
            Throw New Exception("لا يمكن حفظ قيد يومية بدون تفاصيل.")
        End If

        ' === التحقق الأهم: توازن القيد ===
        Dim totalDebit = jv.Details.Sum(Function(d) d.Debit)
        Dim totalCredit = jv.Details.Sum(Function(d) d.Credit)

        If totalDebit <> totalCredit Then
            Throw New Exception("القيد غير متوازن. إجمالي المدين لا يساوي إجمالي الدائن.")
        End If
        ' ===================================

        Return dal.SaveJournalVoucher(jv)
    End Function
    ' في نهاية كلاس JournalVoucherBLL.vb

    ''' <summary>
    ''' يجلب تقرير دفتر الأستاذ لحساب معين بعد التحقق من المدخلات.
    ''' </summary>
    Public Function GetLedgerForAccount(ByVal accountId As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of LedgerEntry)
        ' التحقق من صحة المدخلات
        If accountId <= 0 Then
            Throw New Exception("يجب اختيار حساب لعرض التقرير.")
        End If

        If startDate > endDate Then
            Throw New Exception("تاريخ البداية لا يمكن أن يكون بعد تاريخ النهاية.")
        End If

        ' استدعاء دالة DAL لجلب البيانات
        Return dal.GetLedgerForAccount(accountId, startDate, endDate)
    End Function
    ' في نهاية كلاس JournalVoucherBLL.vb

    ''' <summary>
    ''' يجلب تقرير ميزان المراجعة بعد التحقق من صحة التواريخ.
    ''' </summary>
    Public Function GetTrialBalance(ByVal startDate As DateTime, ByVal endDate As DateTime, ByVal showOnlyTransactional As Boolean) As List(Of TrialBalanceEntry)
        ' التحقق من صحة التواريخ
        If startDate > endDate Then
            Throw New Exception("تاريخ البداية لا يمكن أن يكون بعد تاريخ النهاية.")
        End If

        ' استدعاء دالة DAL لجلب البيانات
        Return dal.GetTrialBalance(startDate, endDate, showOnlyTransactional)
    End Function
End Class