Imports System
Imports System.Windows.Forms
Imports System.Security.Permissions

<PermissionSet(SecurityAction.Demand, Name:="FullTrust")> _
<System.Runtime.InteropServices.ComVisibleAttribute(True)> _
Public Class fVTO
    Public Sub Form_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        WebBrowser1.AllowWebBrowserDrop = False
        WebBrowser1.IsWebBrowserContextMenuEnabled = False
        WebBrowser1.WebBrowserShortcutsEnabled = False
        WebBrowser1.ObjectForScripting = Me
        WebBrowser1.ScrollBarsEnabled = False

        ' Uncomment the following line when you are finished debugging. 
        'webBrowser1.ScriptErrorsSuppressed = True

        'Read content of external file into WebBrowser1
        WebBrowser1.Navigate("http://localhost:8080/online/index3.html")

    End Sub

    Public Sub RequestProduct(JsonPath As String)
        TextBoxJsonPath.Text = JsonPath
    End Sub

    Public Sub LoadProduct()
        WebBrowser1.Document.InvokeScript("SelectAssetsByProd", New String() {"", TextBoxJsonPath.Text})
    End Sub


End Class

Public Class cVTOFactory

    ' Varaibles
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Shared VTO As fVTO = Nothing

End Class
