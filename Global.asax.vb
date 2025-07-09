Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication



    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
        ' تأكد من أن هذا الكود لا يحتوي على أخطاء
        System.Web.UI.ScriptManager.ScriptResourceMapping.AddDefinition("jquery", New System.Web.UI.ScriptResourceDefinition With {
        .Path = "~/Scripts/jquery-3.4.1.min.js"
    })
    End Sub
End Class