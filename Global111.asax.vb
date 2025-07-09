
Public Class Global_asax111
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
        ' تأكد من أن هذا الكود لا يحتوي على أخطاء
        System.Web.UI.ScriptManager.ScriptResourceMapping.AddDefinition("jquery", New System.Web.UI.ScriptResourceDefinition With {
        .Path = "~/Scripts/jquery-3.4.1.min.js"
    })
    End Sub
End Class