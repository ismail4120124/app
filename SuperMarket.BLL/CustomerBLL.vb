' SuperMarket.BLL/CustomerBLL.vb
Imports SuperMarket.DAL
Imports SuperMarket.Entities

Public Class CustomerBLL
    Private dal As New CustomerDAL()

    Public Function SearchCustomers(ByVal searchTerm As String, ByVal maxRows As Integer) As List(Of Customer)
        Return dal.SearchCustomers(searchTerm, maxRows)
    End Function
    ' داخل كلاس CustomerBLL

    Public Function GetAllCustomers() As List(Of Customer)
        Return dal.GetAllCustomers()
    End Function

    Public Sub AddCustomer(ByVal customer As Customer)
        If String.IsNullOrWhiteSpace(customer.CustomerName) Then
            Throw New Exception("اسم العميل مطلوب.")
        End If
        ' يمكنك إضافة تحقق من عدم تكرار رقم الهاتف أو الاسم
        dal.AddCustomer(customer)
    End Sub
End Class