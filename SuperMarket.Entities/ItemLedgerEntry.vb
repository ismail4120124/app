' SuperMarket.Entities/ItemLedgerEntry.vb
Public Class ItemLedgerEntry
    Public Property TranDate As DateTime
    Public Property TranType As String
    Public Property DocNumber As Long
    Public Property QtyIn As Decimal
    Public Property QtyOut As Decimal
    Public Property Balance As Decimal
End Class