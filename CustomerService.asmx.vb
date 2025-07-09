' CustomerService.asmx.vb
Imports System.Web
Imports System.Web.Services
Imports System.Web.Script.Services
Imports SuperMarket.BLL
Imports SuperMarket.Entities
Imports System.ComponentModel

<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
<ScriptService()> ' << هذا السطر مهم جدًا لجعله قابلاً للاستدعاء من JavaScript
Public Class CustomerService
    Inherits System.Web.Services.WebService

    <WebMethod()>
    <ScriptMethod()>
    Public Function GetCustomers(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim customerBll As New CustomerBLL()
        ' ابحث عن العملاء الذين يتطابقون مع النص المدخل
        Dim customers = customerBll.SearchCustomers(prefixText, count)
        ' قم بتحويل قائمة الكائنات إلى مصفوفة من السلاسل النصية بالتنسيق الذي يفهمه AutoCompleteExtender
        Dim items As New List(Of String)()
        For Each c As Customer In customers
            ' التنسيق: "اسم العميل|معرف العميل"
            items.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.CustomerName, c.CustomerID.ToString()))
        Next
        Return items.ToArray()
    End Function
End Class