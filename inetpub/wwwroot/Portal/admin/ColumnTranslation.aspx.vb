
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Admin.Kernel.ColumnTranslation
Imports CKG.Portal.PageElements

Public Class ColumnTranslation
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles

#Region " Vom Web Form Designer generierter Code "
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents dgSearchResult As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents trSearchResult As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEditUser As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtAppID As System.Web.UI.WebControls.TextBox
    Protected WithEvents lnkUserManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkGroupManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lbtnNew As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnCancel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnDelete As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trSearchSpacer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkCustomerManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents lnkAppManagment As System.Web.UI.WebControls.HyperLink
    Protected WithEvents txtOrgNameNeu As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNewName As System.Web.UI.WebControls.TextBox
    Protected WithEvents cbxNullenEntfernen As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtOrgNameAlt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDisplayOrder As System.Web.UI.WebControls.TextBox
    Protected WithEvents cbxTextBereinigen As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxABEDaten As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lnkBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblAppName As System.Web.UI.WebControls.Label
    Protected WithEvents lnkOrganizationManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ddlAlignment As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblAppFriendlyName As System.Web.UI.WebControls.Label
    Protected WithEvents cbxIstDatum As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxIstZeit As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ucHeader As Header
    'Dieser Aufruf ist f�r den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist f�r den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

#Region "Properties"

    Private Property Refferer() As String
        Get
            Return ViewState.Item("refferer").ToString
        End Get
        Set(ByVal value As String)
            ViewState.Item("refferer") = value
        End Set
    End Property
#End Region

#Region " Membervariables "
    Private m_User As User
    Private m_App As App
    Private m_context As HttpContext = HttpContext.Current
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einf�gen
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "Spalten�bersetzungen"
        AdminAuth(Me, m_User, AdminLevel.Master)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                If Not Me.Request.UrlReferrer Is Nothing Then
                    Refferer = Me.Request.UrlReferrer.ToString
                Else
                    Refferer = "Selection.aspx"
                End If


                txtAppID.Text = CStr(Request.QueryString("AppID"))


                cn.Open()
                Dim _App As New Kernel.Application(CInt(txtAppID.Text), cn)
                lblAppName.Text = _App.AppName
                lblAppFriendlyName.Text = _App.AppFriendlyName

                If txtAppID.Text = String.Empty Then
                    lbtnNew.Visible = False
                Else
                    Dim lItem As System.Web.UI.WebControls.ListItem
                    lItem = New System.Web.UI.WebControls.ListItem("Links", "Left")
                    ddlAlignment.Items.Add(lItem)
                    lItem = New System.Web.UI.WebControls.ListItem("Zentriert", "Center")
                    ddlAlignment.Items.Add(lItem)
                    lItem = New System.Web.UI.WebControls.ListItem("Rechts", "Right")
                    ddlAlignment.Items.Add(lItem)
                    FillForm()


                End If

                If m_User.Customer.AccountingArea = -1 Then
                    'Admin �bergeordneter Firma, link einblenden
                    lnkAppManagment.Visible = True
                    lbtnNew.Enabled = True
                    'letzte spalte im grid mit l�schbutton
                    dgSearchResult.Columns(dgSearchResult.Columns.Count - 1).Visible = True
                End If


            End If
        Catch ex As Exception
            lblError.Text = ex.ToString
            lblError.Visible = True
            m_App.WriteErrorText(1, m_User.UserName, "ColumnTranslation", "PageLoad", lblError.Text)
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

#Region " Data and Function "
    Private Sub FillForm()
        trEditUser.Visible = False
        trSearchResult.Visible = False
        Search(True, True, True, True)
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "DisplayOrder"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub
    Private Sub FillDataGrid(ByVal strSort As String)
        trSearchResult.Visible = True
        Dim dvColumnTranslation As DataView

        'If Not m_context.Cache("myColListView") Is Nothing Then
        '    dvColumnTranslation = CType(m_context.Cache("myColListView"), DataView)
        If Not Session("myColListView") Is Nothing Then
            dvColumnTranslation = CType(Session("myColListView"), DataView)
        Else
            Dim dtColumnTranslation As Kernel.ColumnTranslationList
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try
                cn.Open()

                dtColumnTranslation = New Kernel.ColumnTranslationList(CInt(txtAppID.Text), _
                                                                            cn)
                dvColumnTranslation = dtColumnTranslation.DefaultView
                'm_context.Cache.Insert("myColListView", dvColumnTranslation, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                Session("myColListView") = dvColumnTranslation
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End If
        dvColumnTranslation.Sort = strSort
        If dvColumnTranslation.Count > dgSearchResult.PageSize Then
            dgSearchResult.PagerStyle.Visible = True
        Else
            dgSearchResult.PagerStyle.Visible = False
        End If

        With dgSearchResult
            .DataSource = dvColumnTranslation
            .DataBind()
        End With
        If m_User.Customer.AccountingArea = -1 Then
            'alle lbEdits Suchen und aktivieren
            For Each tmpItem As DataGridItem In dgSearchResult.Items
                CType(tmpItem.FindControl("lbEdit"), LinkButton).Enabled = True
            Next
        End If

    End Sub

    Private Function FillEdit(ByVal intAppId As Integer, ByVal strOrgName As String) As Boolean
        SearchMode(False)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            Dim _ColTrans As New Kernel.ColumnTranslation(intAppId, strOrgName, cn)
            txtAppID.Text = _ColTrans.AppId.ToString
            txtOrgNameAlt.Text = _ColTrans.OrgNameAlt
            txtOrgNameNeu.Text = _ColTrans.OrgNameNeu
            txtNewName.Text = _ColTrans.NewName
            If Not _ColTrans.DisplayOrder = 0 Then txtDisplayOrder.Text = _ColTrans.DisplayOrder.ToString
            cbxNullenEntfernen.Checked = _ColTrans.NullenEntfernen
            cbxTextBereinigen.Checked = _ColTrans.TextBereinigen
            cbxIstDatum.Checked = _ColTrans.IstDatum
            cbxIstZeit.Checked = _ColTrans.IstZeit
            cbxABEDaten.Checked = _ColTrans.ABEDaten
            Select Case _ColTrans.Alignment
                Case "Right"
                    ddlAlignment.SelectedIndex = 2
                Case "Center"
                    ddlAlignment.SelectedIndex = 1
                Case Else
                    ddlAlignment.SelectedIndex = 0
            End Select
            Return True
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Function

    Private Sub ClearEdit()
        txtOrgNameAlt.Text = ""
        txtOrgNameNeu.Text = ""
        txtNewName.Text = ""
        txtDisplayOrder.Text = ""
        cbxNullenEntfernen.Checked = False
        cbxTextBereinigen.Checked = False
        cbxIstDatum.Checked = False
        cbxIstZeit.Checked = False
        cbxABEDaten.Checked = False
        'Buttons
        lbtnSave.Visible = True
        lbtnDelete.Visible = False
        ddlAlignment.SelectedIndex = 0
        LockEdit(False)
    End Sub

    Private Sub LockEdit(ByVal blnLock As Boolean)
        Dim strBackColor As String = "White"
        If blnLock Then
            strBackColor = "LightGray"
        End If
        txtOrgNameNeu.Enabled = Not blnLock
        txtOrgNameNeu.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtNewName.Enabled = Not blnLock
        txtNewName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtDisplayOrder.Enabled = Not blnLock
        txtDisplayOrder.BackColor = System.Drawing.Color.FromName(strBackColor)
        cbxNullenEntfernen.Enabled = Not blnLock
        cbxNullenEntfernen.BackColor = System.Drawing.Color.FromName(strBackColor)
        cbxTextBereinigen.Enabled = Not blnLock
        cbxTextBereinigen.BackColor = System.Drawing.Color.FromName(strBackColor)
        cbxIstDatum.Enabled = Not blnLock
        cbxIstDatum.BackColor = System.Drawing.Color.FromName(strBackColor)
        cbxIstZeit.Enabled = Not blnLock
        cbxIstZeit.BackColor = System.Drawing.Color.FromName(strBackColor)
        cbxABEDaten.Enabled = Not blnLock
        cbxABEDaten.BackColor = System.Drawing.Color.FromName(strBackColor)
        ddlAlignment.Enabled = Not blnLock
        ddlAlignment.BackColor = System.Drawing.Color.FromName(strBackColor)
    End Sub

    Private Sub EditEditMode(ByVal intGroupId As Integer, ByVal strOrgName As String)
        If Not FillEdit(intGroupId, strOrgName) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = "Verwerfen"
    End Sub

    Private Sub EditDeleteMode(ByVal intGroupId As Integer, ByVal strOrgName As String)
        If Not FillEdit(intGroupId, strOrgName) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "M�chten Sie die Spalen�bersetzung wirklich l�schen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = "Abbrechen"
        lbtnSave.Visible = False
        lbtnDelete.Visible = True
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        trEditUser.Visible = Not blnSearchMode
        trSearchSpacer.Visible = blnSearchMode
        trSearchResult.Visible = blnSearchMode
        lbtnSave.Visible = Not blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
        lbtnNew.Visible = blnSearchMode
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        If blnClearCache Then
            ' m_context.Cache.Remove("myColListView")
            Session.Remove("myColListView")
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.CurrentPageIndex = 0
        SearchMode()
        If blnRefillDataGrid Then FillDataGrid()
    End Sub

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal tblParameters As DataTable, Optional ByVal strCategory As String = "APP")
        Dim logApp As New Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

        ' strCategory
        Dim strUserName As String = m_User.UserName ' strUserName
        Dim strSessionID As String = Session.SessionID ' strSessionID
        Dim intSource As Integer = 0 ' intSource 
        'Dim intSource As Integer = CInt(Request.QueryString("AppID")) ' intSource 
        Dim strTask As String = "Admin - Spalten�bersetzungen" ' strTask
        ' strIdentification
        ' strDescription
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
    End Sub

    Private Function SetOldLogParameters(ByVal intAppId As Int32, ByVal strOrgName As String, ByVal tblPar As DataTable) As DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim _ColTrans As New Kernel.ColumnTranslation(intAppId, strOrgName, cn)

            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If
            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Alt"
                .Rows(.Rows.Count - 1)("Anwendung") = lblAppFriendlyName.Text
                .Rows(.Rows.Count - 1)("SAP-Name") = _ColTrans.OrgNameAlt
                .Rows(.Rows.Count - 1)("�bersetzung") = _ColTrans.NewName
                If Not _ColTrans.DisplayOrder = 0 Then .Rows(.Rows.Count - 1)("Reihenfolge-Nr.") = _ColTrans.DisplayOrder.ToString
                .Rows(.Rows.Count - 1)("Nullen entfernen") = _ColTrans.NullenEntfernen
                .Rows(.Rows.Count - 1)("Text bereinigen") = _ColTrans.TextBereinigen
                .Rows(.Rows.Count - 1)("ist Datum") = _ColTrans.IstDatum
                .Rows(.Rows.Count - 1)("ist Zeit") = _ColTrans.IstZeit
                .Rows(.Rows.Count - 1)("ABE-Daten") = _ColTrans.ABEDaten
                .Rows(.Rows.Count - 1)("Ausrichtung") = _ColTrans.Alignment
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ColumnTranslation", "SetOldLogParameters", ex.ToString)

            Dim dt As New DataTable()
            dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", System.Type.GetType("System.String"))
            dt.Rows.Add(dt.NewRow)
            Dim str As String = ex.Message
            If Not ex.InnerException Is Nothing Then
                str &= ": " & ex.InnerException.Message
            End If
            dt.Rows(0)("Fehler beim Erstellen der Log-Parameter") = str
            Return dt
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Function

    Private Function SetNewLogParameters(ByVal tblPar As DataTable) As DataTable
        Try
            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If
            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Neu"
                .Rows(.Rows.Count - 1)("Anwendung") = lblAppFriendlyName.Text
                .Rows(.Rows.Count - 1)("SAP-Name") = txtOrgNameNeu.Text
                .Rows(.Rows.Count - 1)("�bersetzung") = txtNewName.Text
                .Rows(.Rows.Count - 1)("Reihenfolge-Nr.") = txtDisplayOrder.Text
                .Rows(.Rows.Count - 1)("Nullen entfernen") = cbxNullenEntfernen.Checked
                .Rows(.Rows.Count - 1)("Text bereinigen") = cbxTextBereinigen.Checked
                .Rows(.Rows.Count - 1)("ist Datum") = cbxIstDatum.Checked
                .Rows(.Rows.Count - 1)("ist Zeit") = cbxIstZeit.Checked
                .Rows(.Rows.Count - 1)("ABE-Daten") = cbxABEDaten.Checked
                .Rows(.Rows.Count - 1)("Ausrichtung") = ddlAlignment.SelectedItem.Text
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ColumnTranslation", "SetNewLogParameters", ex.ToString)

            Dim dt As New DataTable()
            dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", System.Type.GetType("System.String"))
            dt.Rows.Add(dt.NewRow)
            Dim str As String = ex.Message
            If Not ex.InnerException Is Nothing Then
                str &= ": " & ex.InnerException.Message
            End If
            dt.Rows(0)("Fehler beim Erstellen der Log-Parameter") = str
            Return dt
        End Try
    End Function

    Private Function CreateLogTableStructure() As DataTable
        Dim tblPar As New DataTable()
        With tblPar
            .Columns.Add("Status", System.Type.GetType("System.String"))
            .Columns.Add("Anwendung", System.Type.GetType("System.String"))
            .Columns.Add("SAP-Name", System.Type.GetType("System.String"))
            .Columns.Add("�bersetzung", System.Type.GetType("System.String"))
            .Columns.Add("Reihenfolge-Nr.", System.Type.GetType("System.String"))
            .Columns.Add("Nullen entfernen", System.Type.GetType("System.Boolean"))
            .Columns.Add("Text bereinigen", System.Type.GetType("System.Boolean"))
            .Columns.Add("ist Datum", System.Type.GetType("System.Boolean"))
            .Columns.Add("ist Zeit", System.Type.GetType("System.Boolean"))
            .Columns.Add("ABE-Daten", System.Type.GetType("System.Boolean"))
            .Columns.Add("Ausrichtung", System.Type.GetType("System.String"))
        End With
        Return tblPar
    End Function
#End Region

#Region " Events "
    Private Sub dgSearchResult_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgSearchResult.SortCommand
        Dim strSort As String = e.SortExpression
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort
        FillDataGrid(strSort)
    End Sub

    Private Sub dgSearchResult_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgSearchResult.ItemCommand
        If e.CommandName = "Edit" Then
            EditEditMode(CInt(e.Item.Cells(0).Text), e.CommandArgument.ToString)
            dgSearchResult.SelectedIndex = e.Item.ItemIndex
        ElseIf e.CommandName = "Delete" Then
            EditDeleteMode(CInt(e.Item.Cells(0).Text), e.CommandArgument.ToString)
            dgSearchResult.SelectedIndex = e.Item.ItemIndex
        End If
    End Sub

    Private Sub dgSearchResult_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgSearchResult.PageIndexChanged
        dgSearchResult.CurrentPageIndex = e.NewPageIndex
        FillDataGrid()
    End Sub

    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click
        Search(, True)
    End Sub

    Private Sub lbtnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnNew.Click
        SearchMode(False)
        ClearEdit()
        'Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        'cn.Open()
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click
        Dim tblLogParameter As DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim intAppId As Integer = CInt(txtAppID.Text)
            Dim strLogMsg As String = "Spalten�bersetzungen anlegen"
            If Not (txtOrgNameAlt.Text = String.Empty) Then
                strLogMsg = "Spalten�bersetzungen �ndern"
                tblLogParameter = New DataTable
                tblLogParameter = SetOldLogParameters(intAppId, txtOrgNameAlt.Text, tblLogParameter)
            End If

            Dim intDisplayOrder As Integer
            If IsNumeric(txtDisplayOrder.Text) Then
                intDisplayOrder = CInt(txtDisplayOrder.Text)
            Else
                intDisplayOrder = 0
            End If
            Dim _ColTrans As New Kernel.ColumnTranslation(intAppId, _
                                                txtOrgNameAlt.Text, _
                                                txtOrgNameNeu.Text, _
                                                txtNewName.Text, _
                                                intDisplayOrder, _
                                                cbxNullenEntfernen.Checked, _
                                                cbxTextBereinigen.Checked, _
                                                cbxIstDatum.Checked, _
                                                cbxIstZeit.Checked, _
                                                cbxABEDaten.Checked, _
                                                ddlAlignment.SelectedItem.Value)
            _ColTrans.Save(cn)
            tblLogParameter = New DataTable
            tblLogParameter = SetNewLogParameters(tblLogParameter)
            Log(_ColTrans.AppId.ToString, strLogMsg, tblLogParameter)
            Search(True, True, , True)
            lblMessage.Text = "Die �nderungen wurden gespeichert."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ColumnTranslation", "lbtnSave_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtAppID.Text, lblError.Text, tblLogParameter, "ERR")
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Dim tblLogParameter As DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            Dim _ColTrans As New Kernel.ColumnTranslation(CInt(txtAppID.Text), txtOrgNameAlt.Text)

            cn.Open()
            tblLogParameter = New DataTable
            tblLogParameter = SetOldLogParameters(CInt(txtAppID.Text), txtOrgNameAlt.Text, tblLogParameter)
            _ColTrans.Delete(cn)
            Log(_ColTrans.AppId.ToString, "Spalten�bersetzungen l�schen", tblLogParameter)
            Search(True, True, True, True)
            lblMessage.Text = "Die Spalten�bersetzung wurde gel�scht."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ColumnTranslation", "lbtnDelete_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtAppID.Text, lblError.Text, tblLogParameter, "ERR")
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub


    Private Sub lnkBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkBack.Click
        Response.Redirect(Refferer)
    End Sub
#End Region

End Class

' ************************************************
' $History: ColumnTranslation.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 6.01.09    Time: 11:45
' Updated in $/CKAG/admin
' ITA 2503  Cache durch Session ersetzt
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 8.10.08    Time: 13:35
' Updated in $/CKAG/admin
' ITA 2295 testfertig
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 15:47
' Updated in $/CKAG/admin
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 14:47
' Created in $/CKAG/admin
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 6.12.07    Time: 14:36
' Updated in $/CKG/Admin/AdminWeb
' ITA: 1440
' 
' *****************  Version 5  *****************
' User: Uha          Date: 9.08.07    Time: 11:38
' Updated in $/CKG/Admin/AdminWeb
' Spalte "IstZeit" in Translation �bernommen
' 
' *****************  Version 4  *****************
' User: Uha          Date: 13.03.07   Time: 10:53
' Updated in $/CKG/Admin/AdminWeb
' History-Eintrag vorbereitet
' 
' ************************************************
