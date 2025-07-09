' SuperMarket.BLL/SupplierBLL.vb
Imports SuperMarket.DAL
Imports SuperMarket.Entities

Public Class SupplierBLL
    Private dal As New SupplierDAL()

    Public Sub AddSupplier(ByVal supplier As Supplier)
        If String.IsNullOrWhiteSpace(supplier.SupplierName) Then
            Throw New Exception("اسم المورد مطلوب.")
        End If
        dal.AddSupplier(supplier)
    End Sub

    Public Function GetAllSuppliers() As List(Of Supplier)
        Return dal.GetAllSuppliers()
    End Function
End Class
