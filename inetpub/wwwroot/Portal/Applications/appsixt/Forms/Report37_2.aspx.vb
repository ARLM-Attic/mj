Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report37_2
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
    Private m_objTable As DataTable
    Private m_blnUnvollstaendigeTuete As Boolean

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents btnSubmit As LinkButton

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        If (Session("ResultTable") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString)
        Else
            m_objTable = CType(Session("ResultTable"), DataTable)
            m_blnUnvollstaendigeTuete = CBool(Session("UnvollstaendigeTuete"))
        End If
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2

                If (Not Session("ShowLink") Is Nothing) AndAlso Session("ShowLink").ToString = "True" Then
                    lnkKreditlimit.Visible = True
                    lnkKreditlimit.NavigateUrl = Request.UrlReferrer.ToString
                End If
                If Not Session("lnkExcel").ToString.Length = 0 Then
                    lblDownloadTip.Visible = True
                    lnkExcel.Visible = True
                    lnkExcel.NavigateUrl = Session("lnkExcel").ToString
                End If
                FillGrid(0)
            Else
                checkGrid()
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        '##
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub CheckGrid()
        Dim item As DataGridItem
        Dim control As CheckBox
        Dim row As DataRow


        For Each item In DataGrid1.Items
            row = m_objTable.Select("Equipmentnummer=" & item.Cells(0).Text)(0)
            control = CType(item.FindControl("cbxDelete"), CheckBox)
            If control.Checked = True Then
                row("Delete") = True
            Else
                row("Delete") = False
            End If
        Next
    End Sub


    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If m_objTable.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            ShowScript.Visible = False
        Else
            DataGrid1.Visible = True
            lblNoData.Visible = False

            Dim tmpDataView As New DataView()
            tmpDataView = m_objTable.DefaultView

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState("Sort") = strTempSort
                ViewState("Direction") = strDirection
            Else
                If Not ViewState("Sort") Is Nothing Then
                    strTempSort = ViewState("Sort").ToString
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState("Direction") = strDirection
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            DataGrid1.CurrentPageIndex = intTempPageIndex

            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            If m_blnUnvollstaendigeTuete Then
                DataGrid1.Columns(3).Visible = True
                DataGrid1.Columns(4).Visible = True
                DataGrid1.Columns(5).Visible = True
                DataGrid1.Columns(6).Visible = True
                DataGrid1.Columns(7).Visible = True
                DataGrid1.Columns(8).Visible = True
                DataGrid1.Columns(9).Visible = True
                DataGrid1.Columns(10).Visible = True
                DataGrid1.Columns(11).Visible = True
                DataGrid1.Columns(12).Visible = True

                DataGrid1.Columns(13).Visible = False
                DataGrid1.Columns(14).Visible = False
                'DataGrid1.Columns(15).Visible = False
                DataGrid1.Columns(16).Visible = False
            Else
                DataGrid1.Columns(3).Visible = False
                DataGrid1.Columns(4).Visible = False
                DataGrid1.Columns(5).Visible = False
                DataGrid1.Columns(6).Visible = False
                DataGrid1.Columns(7).Visible = False
                DataGrid1.Columns(8).Visible = False
                DataGrid1.Columns(9).Visible = False
                DataGrid1.Columns(10).Visible = False
                DataGrid1.Columns(11).Visible = False
                DataGrid1.Columns(12).Visible = False

                DataGrid1.Columns(13).Visible = True
                DataGrid1.Columns(14).Visible = True
                DataGrid1.Columns(15).Visible = True
                DataGrid1.Columns(16).Visible = True
            End If

            If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                lblNoData.Text = CStr(Session("ShowOtherString"))
            Else
                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Eintr�ge zu """ & m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString & """ gefunden."
            End If
            If (Not Session("BackLink") Is Nothing) AndAlso CStr(Session("BackLink")) = "HistoryBack" Then
                lnkKreditlimit.Text = "Zur�ck"
                lnkKreditlimit.NavigateUrl = "javascript:history.back()"
            End If
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If
        End If
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

        Dim m_Report As New Sixt_B11(m_User, m_App, "") 'neues objekt? dann m�ssen wir halt die tabelle �bergeben. JJU20081203


        m_Report.Clear(Session("AppID").ToString, Session.SessionID.ToString, m_objTable)

        If Not m_Report.Status = 0 Then
            lblError.Text = m_Report.Message
            Exit Sub
        Else
            lblNoData.Text = "Vogang erfolgreich abgeschlossen."
        End If

        'Erfolgreich gel�schte Zeilen entfernen

        Dim intCounter As Integer

        For intCounter = m_objTable.Rows.Count - 1 To 0 Step -1
            If (m_objTable.Rows(intCounter)("Status").ToString = "OK") Then
                m_objTable.Rows.Remove(m_objTable.Rows(intCounter))
            End If
        Next

        m_objTable.AcceptChanges()
        Session("ResultTable") = m_objTable
        FillGrid(0)
    End Sub



    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs, Optional ByVal strCategory As String = "APP")
        Dim logApp As New Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

        ' strCategory
        Dim strUserName As String = m_User.UserName ' strUserName
        Dim strSessionID As String = Session.SessionID ' strSessionID
        Dim intSource As Integer = CInt(Request.QueryString("AppID")) ' intSource 
        Dim strTask As String = m_User.Applications.Select("AppID = '" & CStr(Request.QueryString("AppID")) & "'")(0)("AppFriendlyName").ToString ' strTask
        ' strIdentification
        ' strDescription
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 
        Dim tblParameters As DataTable = GetLogParameters(e) ' tblParameters

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
    End Sub
    Private Function GetLogParameters(ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) As DataTable
        Try
            Dim tblPar As New DataTable()
            With tblPar
                .Columns.Add("Fahrg-Nr", System.Type.GetType("System.String"))
                .Columns.Add("Ersatzsschl�ssel", System.Type.GetType("System.String"))
                .Columns.Add("Carpass", System.Type.GetType("System.String"))
                .Columns.Add("Radiocodekarte", System.Type.GetType("System.String"))
                .Columns.Add("CD-Navi", System.Type.GetType("System.String"))
                .Columns.Add("Chipkarte", System.Type.GetType("System.String"))
                .Columns.Add("COC-Papier", System.Type.GetType("System.String"))
                .Columns.Add("Navi-Codekarte", System.Type.GetType("System.String"))
                .Columns.Add("Codekarte WFS", System.Type.GetType("System.String"))
                .Columns.Add("Ersatzfernbed Standh", System.Type.GetType("System.String"))
                .Columns.Add("Pr�fbuch LKW", System.Type.GetType("System.String"))
                .Rows.Add(.NewRow)
                .Rows(0)("Fahrg-Nr") = e.Item.Cells(0).Text
                .Rows(0)("Ersatzsschl�ssel") = e.Item.Cells(1).Text
                .Rows(0)("Carpass") = e.Item.Cells(2).Text
                .Rows(0)("Radiocodekarte") = e.Item.Cells(3).Text
                .Rows(0)("CD-Navi") = e.Item.Cells(4).Text
                .Rows(0)("Chipkarte") = e.Item.Cells(5).Text
                .Rows(0)("COC-Papier") = e.Item.Cells(6).Text
                .Rows(0)("Navi-Codekarte") = e.Item.Cells(7).Text
                .Rows(0)("Codekarte WFS") = e.Item.Cells(8).Text
                .Rows(0)("Ersatzfernbed Standh") = e.Item.Cells(9).Text
                .Rows(0)("Pr�fbuch LKW") = e.Item.Cells(10).Text
            End With
            Return tblPar
        Catch ex As Exception
            Dim dt As New DataTable()
            dt.Columns.Add("Fehler beim erstellen der Log-Parameter", System.Type.GetType("System.String"))
            dt.Rows.Add(dt.NewRow)
            Dim str As String = ex.Message
            If Not ex.InnerException Is Nothing Then
                str &= ": " & ex.InnerException.Message
            End If
            dt.Rows(0)("Fehler beim erstellen der Log-Parameter") = str
            Return dt
        End Try
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report37_2.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 7.01.11    Time: 8:49
' Updated in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 6.01.11    Time: 15:26
' Updated in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:16
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA:2837
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 3.12.08    Time: 13:16
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA 2431 fertigstellung
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 14:10
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingef�hrt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' 
' ************************************************
