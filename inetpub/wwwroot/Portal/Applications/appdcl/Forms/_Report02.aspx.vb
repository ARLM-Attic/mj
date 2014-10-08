Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business
Imports CKG.EasyAccess
Imports System.IO
Imports System.Linq
Imports System.Collections.Generic
Imports System.Web.UI

<CLSCompliant(False)> Partial Class _Report02
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblScript As System.Web.UI.WebControls.Label
    Protected WithEvents cbxArc As System.Web.UI.WebControls.CheckBox
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User


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
        m_User = GetUser(Me)

        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                FillAuftragsListe()
            Else
                lblMsg.Text = String.Empty
            End If
            'If Not IsPostBack Then
            '    loadData()
            '    loadForm()
            'End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Const AuftragsListeKey = "AuftragsListe"
    Private m_auftragsListe As List(Of ReviewAuftrag)

    Private ReadOnly Property AuftragsListe As List(Of ReviewAuftrag)
        Get
            If m_auftragsListe Is Nothing Then
                Dim sessionObj As Object = Session(AuftragsListeKey)
                If Not sessionObj Is Nothing AndAlso GetType(List(Of ReviewAuftrag)).Equals(sessionObj.GetType()) Then
                    m_auftragsListe = CType(sessionObj, List(Of ReviewAuftrag))
                Else
                    Dim folder = New DirectoryInfo(ConfigurationManager.AppSettings("UploadPathLocal"))
                    Dim backupFolder = New DirectoryInfo(ConfigurationManager.AppSettings("UploadPathLocalBackup"))
                    Dim serverFolder = ConfigurationManager.AppSettings("UploadPath")

                    Dim files = ReviewFile.FindUploadedFiles(folder.FullName, backupFolder.FullName, serverFolder, True)
                    Dim auftraege = ReviewAuftrag.GroupFiles(files)

                    Dim ueberf = New Ueberfuehrung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                    ueberf.ReadAuftragsdaten(auftraege)
                    m_auftragsListe = auftraege
                    Session(AuftragsListeKey) = m_auftragsListe
                End If
            End If
            Return m_auftragsListe
        End Get
    End Property

    Private Sub FillAuftragsListe()
        m_auftragsListe = Nothing
        Session.Remove(AuftragsListeKey)

        lbxAuftrag.DataSource = AuftragsListe
        lbxAuftrag.DataBind()

        FillGridServer()
    End Sub

    Private Sub FillGridServer()
        Dim auftrag = GetSelectedAuftrag()

        gridServer.DataSource = Nothing

        If Not auftrag Is Nothing AndAlso auftrag.Files.Count > 0 Then
            gridServer.DataSource = auftrag.Files
        End If

        gridServer.DataBind()

        UpdateSelection()
        FillMoveToList(auftrag)

        btnFinish.Enabled = Not gridServer.DataSource Is Nothing
        btnBack.Enabled = Not gridServer.DataSource Is Nothing
        btnConfirm.Enabled = Not gridServer.DataSource Is Nothing
    End Sub

    Private Sub FillMoveToList(ByVal selectedAuftrag As ReviewAuftrag)
        If AuftragsListe Is Nothing OrElse selectedAuftrag Is Nothing Then
            moveToList.DataSource = Nothing
        Else
            Dim otherAuftraege = AuftragsListe.Where(Function(a) Not a.Equals(selectedAuftrag)).ToList()
            moveToList.DataSource = IIf(otherAuftraege.Count > 0, otherAuftraege, Nothing)
        End If
        moveToList.DataBind()
        moveLabel.Enabled = Not moveToList.DataSource Is Nothing
        moveToList.Enabled = Not moveToList.DataSource Is Nothing
        moveButton.Enabled = Not moveToList.DataSource Is Nothing
    End Sub

    Private Function GetSelectedAuftrag() As ReviewAuftrag
        Return AuftragsListe.ElementAtOrDefault(lbxAuftrag.SelectedIndex)
    End Function

    Protected Sub AuftragSelected(ByVal sender As Object, ByVal e As EventArgs)
        SetFinished(False)
        FillGridServer()
    End Sub

    Protected Sub FileSelected(ByVal sender As Object, ByVal e As EventArgs)
        Dim auftrag = GetSelectedAuftrag()
        If auftrag Is Nothing Then Return

        Dim chk = DirectCast(sender, CheckBox)
        Dim row = DirectCast(chk.Parent.Parent, GridViewRow)

        Dim filename = gridServer.DataKeys(row.DataItemIndex).Value.ToString()
        Dim file = auftrag.Files.FirstOrDefault(Function(f) f.Filename = filename)

        If Not file Is Nothing Then
            file.Selected = chk.Checked
        End If

        UpdateSelection()
    End Sub

    Protected Sub MoveClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim auftrag = GetSelectedAuftrag()
        If auftrag Is Nothing Then Return

        Dim destAuftrag = AuftragsListe.FirstOrDefault(Function(a) a.ToString().Equals(moveToList.SelectedValue.ToString()))
        If destAuftrag Is Nothing Then Return

        Dim moveFiles = auftrag.Files.Where(Function(f) f.Selected).ToList()
        Dim folder = New DirectoryInfo(ConfigurationManager.AppSettings("UploadPathLocal"))
        Dim backupFolder = New DirectoryInfo(ConfigurationManager.AppSettings("UploadpathLocalBackup"))
        For Each file In moveFiles
            Try
                file.MoveTo(folder.FullName, backupFolder.FullName, destAuftrag)
            Catch ex As Exception
                lblError.Text = ex.ToString
                lblError.Visible = True
                Return
            End Try
        Next
        If moveFiles.Count = 1 Then
            lblMsg.Text = "Ein Bild nach " & destAuftrag.ToString() & " verschoben."
        Else
            lblMsg.Text = moveFiles.Count & " Bilder nach " & destAuftrag.ToString() & " verschoben."
        End If

        FillAuftragsListe()
        lbxAuftrag.SelectedIndex = AuftragsListe.FindIndex(Function(a) a.AuftragsNummer = auftrag.AuftragsNummer AndAlso a.FahrerID = auftrag.FahrerID AndAlso a.Fahrt = auftrag.Fahrt)
        FillGridServer()
    End Sub

    Private Sub UpdateSelection()
        Dim auftrag = GetSelectedAuftrag()
        If auftrag Is Nothing Then
            movePanel.Visible = False
            Return
        End If

        Dim selected = auftrag.Files.Where(Function(f) f.Selected).Count()
        If selected = 0 Then
            moveLabel.Text = "Keine Bilder gew�hlt"
            movePanel.Visible = False
        Else
            If selected = 1 Then
                moveLabel.Text = "1 Bild nach"
            Else
                moveLabel.Text = selected & " Bilder nach"
            End If
            movePanel.Visible = True
        End If
    End Sub

    Private Sub cbxFinished_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Handles cbxFinished.CheckedChanged



        'Dim table As DataTable
        'Dim row As DataRow
        'Dim auftrag As String
        'Dim tour As String

        'table = CType(Session("Serverfiles"), DataTable)
        'auftrag = getAuftragsNr()
        'tour = getTourNr()

        'For Each row In table.Rows
        '    If (CType(row("Auftrag"), String) = auftrag) And (CType(row("Tour"), String) = tour) Then
        '        If (cbxFinished.Checked) Then
        '            row("Save") = "X"
        '        Else
        '            row("Save") = ""
        '        End If
        '    End If
        'Next
        'table.AcceptChanges()
        'Session("Serverfiles") = table
        'fillView()
    End Sub

    Protected Sub GridServerRowDeleting(ByVal source As Object, ByVal e As GridViewDeleteEventArgs)
        Dim auftrag = GetSelectedAuftrag()
        If auftrag Is Nothing Then Return

        Dim file = auftrag.Files.ElementAtOrDefault(e.RowIndex)
        If file Is Nothing Then Return

        auftrag.Files.Remove(file)
        file.Delete()

        If auftrag.Files.Count = 0 Then
            FillAuftragsListe()
        End If

        FillGridServer()
    End Sub

    Protected Sub FinishClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SetFinished(True)
    End Sub

    Protected Sub ConfirmClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim auftrag = GetSelectedAuftrag()
        If auftrag Is Nothing Then Return

        Dim targetArchiv = Path.Combine(ConfigurationManager.AppSettings("UploadPathSambaArchive"), Path.Combine(auftrag.Kundennummer.Value.ToString("0000000000"), auftrag.AuftragsNummer.ToString("0000000000")))
        If Not Directory.Exists(targetArchiv) Then
            Try
                Directory.CreateDirectory(targetArchiv)
            Catch ex As Exception
                lblError.Text = "Fehler beim Erstellen des Verzeichnisses<br/>" & ex.ToString
                Return
            End Try
        End If

        For Each file In auftrag.Files
            Try
                file.Archive(targetArchiv)
            Catch ex As Exception
                lblError.Text = "Fehler beim Archivieren der Bilder<br/>" & ex.ToString
                Return
            End Try
        Next

        Try
            Dim ueberf = New Ueberfuehrung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            ueberf.AuftragAbschlie�en(auftrag)

            If Not String.IsNullOrEmpty(ueberf.Message) OrElse ueberf.Status <> 0 Then
                Throw New ApplicationException("Fehler beim Abschlie�en des Auftrags<br />" & ueberf.Message)
            End If
        Catch aex As ApplicationException
            lblError.Text = aex.Message
            Return
        Catch ex As Exception
            lblError.Text = "Fehler beim Abschlie�en des Auftrags<br/>" & ex.ToString
            Return
        End Try

        FillAuftragsListe()
        FillGridServer()
        SetFinished(False)
    End Sub

    Protected Sub BackClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SetFinished(False)
    End Sub

    Private Sub SetFinished(ByVal finished As Boolean)
        gridServer.Columns(2).Visible = Not finished
        gridServer.Columns(1).Visible = Not finished
        movePanel.Enabled = Not finished


        If (finished) Then
            lblPageTitle.Text = ": AUFTRAGSBEST�TIGUNG"
        Else
            Dim selected = GetSelectedAuftrag()
            If Not selected Is Nothing Then
                lblPageTitle.Text = ": " & selected.ToString()
            Else
                lblPageTitle.Text = ": Auftr�ge bearbeiten"
            End If
        End If

        If finished Then lblError.Text = String.Empty

        btnFinish.Visible = Not finished
        btnConfirm.Visible = finished
        btnBack.Visible = finished
    End Sub

    'Private Sub showResult()
    '    Dim table As DataTable
    '    Dim row As DataRow
    '    Dim str As String = ""

    '    table = CType(Session("Serverfiles"), DataTable)
    '    For Each row In table.Rows
    '        If (row("Save").ToString = "X") Then
    '            str &= row("Status") & ";"
    '        End If
    '    Next
    '    str = str.Replace("\", "'")
    '    lblOpen.Text = "<script language=""Javascript"">window.open(""_Report022.aspx?USER=" & m_User.UserName & "&PAR=" & str & """, ""�bertragungsprotokoll"", ""width=640,height=480,left=0,top=0,scrollbars=YES"");location.replace(""../../../Start/Selection.aspx"");</script>"
    'End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: _Report02.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 4.02.10    Time: 14:38
' Updated in $/CKAG/Applications/appdcl/Forms
' ITA: 2918
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 30.04.09   Time: 9:25
' Updated in $/CKAG/Applications/appdcl/Forms
' ITA: 2837
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 13.01.09   Time: 11:07
' Updated in $/CKAG/Applications/appdcl/Forms
' fehlerbehandlung eingef�hrt
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 12.01.09   Time: 15:13
' Updated in $/CKAG/Applications/appdcl/Forms
' ITA 2528
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 18.06.08   Time: 13:46
' Updated in $/CKAG/Applications/appdcl/Forms
' Nicht verwendete Variablen gel�scht.
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:00
' Created in $/CKAG/Applications/appdcl/Forms
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 9.08.07    Time: 11:39
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' Bugfix: Fehlerbehandlung  _Report02.aspx Methode Fillddl eingef�gt
' 
' *****************  Version 9  *****************
' User: Uha          Date: 26.06.07   Time: 11:44
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' Methodenaufruf korrigiert (AppDCL, _Report02.aspx.vb)
' 
' *****************  Version 8  *****************
' User: Uha          Date: 21.06.07   Time: 12:36
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingef�hrt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 7.03.07    Time: 10:26
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' 
' ************************************************