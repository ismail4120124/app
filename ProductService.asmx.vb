' ProductService.asmx.vb
Imports System.Web.Services
Imports System.Web.Script.Services
Imports SuperMarket.BLL
Imports SuperMarket.Entities

<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<System.ComponentModel.ToolboxItem(False)>
<ScriptService()>
Public Class ProductService
    Inherits System.Web.Services.WebService

    <WebMethod()>
    <ScriptMethod()>
    Public Function GetProducts(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim productBll As New ProductBLL()
        ' سنحتاج لدالة بحث بسيطة في BLL ترجع قائمة منتجات
        Dim products = productBll.SearchProductsSimple(prefixText, count)

        Dim items As New List(Of String)()
        For Each p As Product In products
            items.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(p.ProductName & " (" & p.Barcode & ")", p.ProductID.ToString()))
        Next
        Return items.ToArray()
    End Function
End Class