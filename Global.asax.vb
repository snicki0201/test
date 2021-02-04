Imports System.Security.Principal
Imports System.Web.Optimization

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Protected Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub

    Protected Sub Application_AuthenticateRequest()
        If String.IsNullOrEmpty(Me.Request.Headers("Authorization")) Then
            Me.Response.StatusCode = 401
            Me.Response.SubStatusCode = 1
            Me.Response.AppendHeader("WWW-Authenticate", "Basic")
            Me.CompleteRequest()
        Else
            Dim s As String = Me.Request.Headers("Authorization")
            ' 「Basic 」以降の文字列を抽出
            Dim encodedString As String = s.Substring(6)

            ' BASE64をデコードする
            Dim decodedBytes() As Byte = Convert.FromBase64String(encodedString)
            Dim decodedString As String = New ASCIIEncoding().GetString(decodedBytes)

            ' ユーザー名とパスワードを分離
            Dim arrSplited() As String = decodedString.Split(New Char() {":"})
            Dim username As String = arrSplited(0)
            Dim password As String = arrSplited(1)

            If username = "test" And password = "pass" Then
                Me.Context.User = New GenericPrincipal(New GenericIdentity("test"), New String() {"Users"})
            Else
                Me.Response.StatusCode = 401
                Me.Response.SubStatusCode = 1
                Me.Response.AppendHeader("WWW-Authenticate", "Basic")
                Me.CompleteRequest()
            End If
        End If
    End Sub

End Class
