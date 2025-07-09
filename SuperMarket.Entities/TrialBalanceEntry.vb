' SuperMarket.Entities/TrialBalanceEntry.vb
Public Class TrialBalanceEntry
    Public Property AccountNumber As String
    Public Property AccountName As String
    Public Property OpeningBalance As Decimal ' الرصيد الافتتاحي
    Public Property PeriodDebit As Decimal ' إجمالي المدين خلال الفترة
    Public Property PeriodCredit As Decimal ' إجمالي الدائن خلال الفترة
    Public Property ClosingBalance As Decimal ' الرصيد النهائي
End Class