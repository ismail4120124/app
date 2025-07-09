' SuperMarket.Entities/CashierShift.vb
<Serializable()>
Public Class CashierShift
    Public Property ShiftID As Integer
    Public Property UserID As Integer
    Public Property BranchID As Integer
    Public Property OpeningTime As DateTime
    Public Property ClosingTime As DateTime? ' Nullable لأنها تكون فارغة أثناء الوردية
    Public Property OpeningBalance As Decimal
    Public Property ClosingBalance As Decimal? ' Nullable
    Public Property SystemSales As Decimal? ' Nullable
    Public Property CashDifference As Decimal? ' Nullable
End Class