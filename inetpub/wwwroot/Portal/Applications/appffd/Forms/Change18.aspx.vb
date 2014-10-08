Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Change18
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist f�r den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist f�r den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents suche1 As Suche
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        NoDealer(Me, m_User)
        suche1.NurFilialSuche = True
        suche1.AllowEmptySearch = True
        suche1.RedirectUrl = "Change18_2.aspx"
        suche1.User = m_User
        Try
            If Request.QueryString("AppID").Length > 0 Then
                Session("AppID") = Request.QueryString("AppID").ToString
            End If

            If Not IsPostBack Then
                If (Not Session("Authorization") Is Nothing) AndAlso (CBool(Session("Authorization"))) Then
                    If (Not Session("AuthorizationID") Is Nothing) AndAlso CInt(Session("AuthorizationID")) > 0 Then
                        Dim strLastRecordParam As String = ""
                        If CStr(Request.QueryString("LastRecord")) = "True" Then
                            strLastRecordParam = "?LastRecord=True"
                        End If
                        Response.Redirect("Change02Aut.aspx" & strLastRecordParam)
                    End If
                End If
            End If

            Session("SelectedDealer") = Nothing

            suche1.Headline = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            m_App = New Base.Kernel.Security.App(m_User)
            ucStyles.TitleText = suche1.Headline

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class
' ************************************************
' $History: Change18.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingef�hrt
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 19.06.07   Time: 16:10
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' History-Eintrag eingepflegt.
' 
' ************************************************