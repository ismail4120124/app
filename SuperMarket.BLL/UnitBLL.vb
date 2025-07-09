' SuperMarket.BLL/UnitBLL.vb
Imports SuperMarket.DAL
Imports SuperMarket.Entities

Public Class UnitBLL
    Private dal As New UnitDAL()

    Public Function GetAllUnits() As List(Of Unit)
        Return dal.GetAllUnits()
    End Function

    Public Sub AddUnit(ByVal unit As Unit)
        If String.IsNullOrWhiteSpace(unit.UnitName) Then
            Throw New Exception("اسم الوحدة مطلوب.")
        End If
        dal.AddUnit(unit)
    End Sub
End Class