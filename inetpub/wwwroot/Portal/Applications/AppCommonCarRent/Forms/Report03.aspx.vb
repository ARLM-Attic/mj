Imports CKG.Base.Business
Imports CKG
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report03
    Inherits System.Web.UI.Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New FehlendeAbmeldeunterlagen(m_User, m_App, strFileName)

            lblError.Text = ""

            If lblError.Text.Length = 0 Then
                m_Report.FILL_FehlUntl(Session("AppID").ToString, Session.SessionID.ToString, Me)

                Session("ResultTable") = m_Report.Result

                If Not m_Report.Status = 0 Then
                    lblError.Text = "Fehler: " & m_Report.Message
                Else
                    If m_Report.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Daten vorhanden."
                    Else

                        Dim objExcelExport As New Excel.ExcelExport()
                        Try
                            Dim tblExcel As DataTable = m_Report.Result.Copy
                            tblExcel.Columns.Remove("Fahrgestellnummer")
                            tblExcel.Columns.Remove("Fahrzeugart")
                            tblExcel.Columns.Remove("Hersteller")
                            tblExcel.Columns.Remove("TypSchluessel")
                            tblExcel.Columns.Remove("Ausfuehrung")
                            tblExcel.Columns.Remove("Name")
                            tblExcel.Columns.Remove("Strasse")
                            tblExcel.Columns.Remove("Postleitzahl")
                            tblExcel.Columns.Remove("Ort")
                            Session.Add("ExcelResult", tblExcel)
                        Catch
                        End Try
                        Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                        logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Fehlende Abmeldeunterlagen")
                        Session.Add("mObjFehlendeAbmeldeunterlagenSession", m_Report)
                        Response.Redirect("Report03_1.aspx?AppID=" & Session("AppID").ToString & "&legende=Report03")
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            'logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler im Report <Fehlende Abmeldeunterlagen>. Fehler: " & ex.Message)
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
' $History: Report03.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 20.02.09   Time: 14:37
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2588
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 17.02.09   Time: 16:28
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2588 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 16.02.09   Time: 14:19
' Created in $/CKAG/Applications/AppCommonCarRent/Forms
' ITa 2586/2588 unfertig
'
' ************************************************