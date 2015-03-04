Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business
Imports System.Data.SqlClient.SqlDataReader
Imports System.Web.UI.WebControls.Label
Imports System
Imports System.IO


Public Class Report41
    Inherits System.Web.UI.Page

    Private objSuche As Search
    Private m_User As Base.Kernel.Security.User
    Private objApp As Base.Kernel.Security.App
    Private m_strHeadline As String
    Private AppName As String
    Private m_strRedirectUrl As String
    Private objDistrikt As FFD_Bank_Distrikt


    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSelect As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtNummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblHDNummer As System.Web.UI.WebControls.Label
    Protected WithEvents txtName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblName As System.Web.UI.WebControls.Label
    Protected WithEvents lblAuswahl As System.Web.UI.WebControls.Label
    Protected WithEvents cmbHaendler As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DistrictDropDown As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents trHaendlernummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trName As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trHdAuswahl As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblTreffer As System.Web.UI.WebControls.Label
    Protected WithEvents DistrictRow As System.Web.UI.HtmlControls.HtmlTableRow

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
        Dim DistricCount As Integer


        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        objApp = New Base.Kernel.Security.App(m_User)

        If Request.QueryString("AppID").Length > 0 Then
            Session("AppID") = Request.QueryString("AppID").ToString
        End If
        ucStyles.TitleText = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        objSuche = New Search(objApp, m_User, Session.SessionID.ToString, Session("AppID").ToString)
        If Session("App_DistriktID") = Nothing Then
            If m_User.Organization.AllOrganizations Then
                If Not Session("App_Filialen") Is Nothing Then
                    DistricCount = Session("DistrictCount")
                Else
                    DistricCount = ReadFiliale()
                End If

            Else
                    DistricCount = ReadDistricts()
                End If
            Else
                DistricCount = Session("DistrictCount")
                trHaendlernummer.Visible = True
                trHdAuswahl.Visible = True
                trName.Visible = True
                DistrictRow.Visible = False
                cmdSearch.Visible = True
                cmdSelect.Visible = True
            End If

            If Not IsPostBack Then
                If DistricCount = 0 And Session("App_Distrikt") = Nothing Then
                Dim tmpIntValue As Int32 = objSuche.LeseHaendlerSAP(Session("AppID").ToString, Session.SessionID.ToString, "", m_User.Organization.OrganizationReference)
                    If tmpIntValue < 0 Then
                        lblMessage.CssClass = "TextError"
                        lblMessage.Text = "Fehler: " & objSuche.ErrorMessage
                    ElseIf tmpIntValue = 0 Then
                        lblMessage.Text = "Ihre Suche hat keine Treffer ergeben."
                    Else
                        cmbHaendler.DataSource = objSuche.Haendler '####
                        Session("objSuche") = objSuche
                        If Not IsNothing(objSuche.Haendler) Then objSuche.Haendler.Sort = "REFERENZ"
                        cmbHaendler.DataTextField = "DISPLAY"
                        cmbHaendler.DataValueField = "REFERENZ"
                        cmbHaendler.DataBind()
                        cmbHaendler.SelectedIndex = 0
                        cmbHaendler.Visible = True
                        lblAuswahl.Visible = True
                        cmdSelect.Visible = True
                        Session("objSuche") = objSuche
                    End If
                ElseIf DistricCount = 1 And Not Session("App_Distrikt") = Nothing Then
                Dim tmpIntValue As Int32 = objSuche.LeseHaendlerSAP(Session("AppID").ToString, Session.SessionID.ToString, "", Session("App_Distrikt"))
                    If tmpIntValue < 0 Then
                        lblMessage.CssClass = "TextError"
                        lblMessage.Text = "Fehler: " & objSuche.ErrorMessage
                    ElseIf tmpIntValue = 0 Then
                        lblMessage.Text = "Ihre Suche hat keine Treffer ergeben."
                    Else
                        cmbHaendler.DataSource = objSuche.Haendler '####
                        Session("objSuche") = objSuche
                        If Not IsNothing(objSuche.Haendler) Then objSuche.Haendler.Sort = "REFERENZ"
                        cmbHaendler.DataTextField = "DISPLAY"
                        cmbHaendler.DataValueField = "REFERENZ"
                        cmbHaendler.DataBind()
                        cmbHaendler.SelectedIndex = 0
                        cmbHaendler.Visible = True
                        lblAuswahl.Visible = True
                        cmdSelect.Visible = True
                        Session("objSuche") = objSuche
                    End If
                ElseIf DistricCount > 1 AndAlso Session("App_DistriktID") = Nothing Then
                    trHaendlernummer.Visible = False
                    trHdAuswahl.Visible = False
                    trName.Visible = False
                    DistrictRow.Visible = True
                    cmdSearch.Visible = False
                    cmdSelect.Visible = True
                End If
            Else
                objSuche = Session("objSuche")
            End If
    End Sub
    Private Function ReadDistricts() As Integer
        'diesen ganzen block nur einmal, da die dropdown sich eigentlich die werte �ber einen postback hinaus merken sollte.
        Dim districtCount As Integer

        'Hier Zugriff auf neue BAPI....
        Dim appId As Integer = CInt(Session("AppID"))
        districtCount = objSuche.ReadDistrictSAP(appId, Session.SessionID)
        If districtCount > 0 Then
            With DistrictDropDown
                .Items.Clear()
                'dropdown f�llen:
                .DataSource = objSuche.District
                .DataTextField = "NAME1"
                .DataValueField = "DISTRIKT"
                .DataBind()
                'vorbelegten distrikt suchen
                objSuche.District.RowFilter = "VORBELEGT='1'"
                Dim drv As DataRowView
                For Each drv In objSuche.District
                    Dim li As ListItem = .Items.FindByValue(drv("DISTRIKT").ToString)
                    If Not li Is Nothing Then
                        If Not .SelectedItem Is Nothing Then
                            .SelectedItem.Selected = False ' falls schon ein anderer selektiert, dann deselektieren.
                        End If
                        li.Selected = True
                        If districtCount = 1 Then
                            Session("App_DistriktID") = li.Value
                        End If
                    End If
                    Exit For ' nach dem ersten aussteigen, da nur einer selektiert sein darf!!!
                Next
            End With
        End If
        Session("DistrictCount") = districtCount
        Return districtCount
    End Function
    Private Function ReadFiliale() As Integer
        'diesen ganzen block nur einmal, da die dropdown sich eigentlich die werte �ber einen postback hinaus merken sollte.
        Dim districtCount As Integer

        'Hier Zugriff auf neue BAPI....
        Dim appId As Integer = CInt(Session("AppID"))
        districtCount = objSuche.LeseFilialenSAP()
        If districtCount > 0 Then
            With DistrictDropDown
                .Items.Clear()
                'dropdown f�llen:
                .DataSource = objSuche.Filialen
                .DataValueField = "FILIALE"
                .DataTextField = "DISPLAY_FILIALE"
                .DataBind()
            End With
        End If
        Session("App_Filialen") = objSuche.Filialen
        Session("DistrictCount") = districtCount
        Return districtCount
    End Function
    Private Sub DoSubmit()
 
        Try
            If Not txtName.Text = String.Empty AndAlso Not cmbHaendler.SelectedItem Is Nothing Then
                Dim tmpbValue As Boolean = objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, cmbHaendler.SelectedItem.Value)
                If tmpbValue = True Then
                    If Not Session("App_DistriktID") = Nothing Then
                        objSuche.HaendlerFiliale = Session("App_DistriktID")
                    Else
                        objSuche.HaendlerFiliale = m_User.Organization.OrganizationReference
                    End If
                    Session("objSuche") = objSuche
                    Session("SelectedDealer") = cmbHaendler.SelectedItem.Value
                    Response.Redirect("Report41_02.aspx?AppID=" & Session("AppID").ToString)
                Else
                    lblMessage.Text = "Es konnten keine H�ndlerinformationen gefunden werden!"
                End If
            ElseIf Not txtNummer.Text = String.Empty Then

                If Not Session("App_DistriktID") = Nothing Then
                    objSuche = New Search(objApp, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                    objSuche.HaendlerFiliale = Session("App_DistriktID")
                Else
                    objSuche.HaendlerFiliale = m_User.Organization.OrganizationReference
                End If
                Dim tmpbValue As Boolean = objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, txtNummer.Text)
                If tmpbValue = True Then


                    Session("objSuche") = objSuche
                    Session("SelectedDealer") = txtNummer.Text
                    Response.Redirect("Report41_02.aspx?AppID=" & Session("AppID").ToString)
                Else
                    lblMessage.Text = "Es konnten keine H�ndlerinformationen gefunden werden!"
                End If
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub cmdSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelect.Click
        If Not DistrictDropDown.SelectedItem Is Nothing AndAlso Session("App_DistriktID") Is Nothing Then
            Session("App_DistriktID") = DistrictDropDown.SelectedItem.Value()
            trHaendlernummer.Visible = True
            trHdAuswahl.Visible = True
            trName.Visible = True
            DistrictRow.Visible = False
            cmdSearch.Visible = True
        Else
            DoSubmit()
        End If

    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        txtNummer.Visible = True
        lblHDNummer.Visible = False
        txtName.Visible = True
        lblName.Visible = False
        trHdAuswahl.Visible = True
        DistrictRow.Visible = False
        cmbHaendler.Enabled = True
        cmdSelect.Visible = True
        cmdBack.Visible = False
        lblMessage.Text = ""
        If Not (Session("objSuche") Is Nothing) Then
            objSuche = Session("objSuche")
            cmbHaendler.DataSource = objSuche.Haendler
            cmbHaendler.DataTextField = "DISPLAY"
            cmbHaendler.DataValueField = "REFERENZ"
            cmbHaendler.DataBind()
            cmbHaendler.SelectedIndex = 0
            txtNummer.Text = ""
            txtName.Text = ""
        End If
    End Sub



    Private Sub DoSubmit2()

        Try
            If objDistrikt.Status = 0 Then

                txtNummer.Visible = False
                lblHDNummer.Visible = True
                lblHDNummer.Text = objSuche.REFERENZ
                Session("SelectedDealer") = objSuche.REFERENZ

                txtName.Visible = False
                lblName.Visible = True
                lblName.Text = objSuche.NAME


                trHdAuswahl.Visible = False
                With DistrictDropDown
                    .Items.Clear()
                    'dropdown f�llen:
                    .DataSource = objDistrikt.Districts
                    .DataTextField = "NAME1"
                    .DataValueField = "Kunnr_Di"
                    .DataBind()
                    'vorbelegten distrikt suchen

                    Dim li As ListItem = .Items.FindByValue(objDistrikt.sDistriktID)
                    If Not li Is Nothing Then
                        If Not .SelectedItem Is Nothing Then
                            .SelectedItem.Selected = False ' falls schon ein anderer selektiert, dann deselektieren.
                        End If
                        li.Selected = True
                    End If
                End With
                Session("App_Distrikte") = objDistrikt.Districts
                DistrictRow.Visible = True
                DistrictDropDown.Enabled = False
                cmdSelect.Visible = False
                cmdBack.Visible = True
                lblMessage.Text = ""
            ElseIf objDistrikt.Status = "-9999" Then

            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub



    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub


    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Dim dataRows() As DataRow
        Dim dataRow As DataRow
        Dim tblSelHaendler As DataTable
        Dim i As Integer

        Try
            If Not txtName.Text = String.Empty Then
                If Not Session("App_DistriktID") Is Nothing Then
                    objSuche = New Search(objApp, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                    Dim tmpIntValue As Int32 = objSuche.LeseHaendlerSAP(Session("AppID").ToString, Session.SessionID.ToString, "", Session("App_DistriktID"))
                    If tmpIntValue < 0 Then
                        lblMessage.CssClass = "TextError"
                        lblMessage.Text = "Fehler: " & objSuche.ErrorMessage
                        Exit Sub
                    ElseIf tmpIntValue = 0 Then
                        lblMessage.Text = "Ihre Suche hat keine Treffer ergeben."
                        Exit Sub
                    ElseIf Session("SelectedDealer") = Nothing Then
                        cmbHaendler.DataSource = objSuche.Haendler '####
                        Session("objSuche") = objSuche
                        If Not IsNothing(objSuche.Haendler) Then objSuche.Haendler.Sort = "REFERENZ"
                        cmbHaendler.DataTextField = "DISPLAY"
                        cmbHaendler.DataValueField = "REFERENZ"
                        cmbHaendler.DataBind()
                        cmbHaendler.SelectedIndex = 0
                        cmbHaendler.Visible = True
                        lblAuswahl.Visible = True
                        cmdSelect.Visible = True
                        Session("objSuche") = objSuche
                    End If
                    dataRows = objSuche.Haendler.Table.Select("Name Like '*" & txtName.Text & "'")
                Else
                    dataRows = objSuche.Haendler.Table.Select("Name Like '*" & txtName.Text & "'")
                End If

                If dataRows.Length > 0 Then
                    tblSelHaendler = New DataTable()
                    For i = 0 To objSuche.Haendler.Table.Columns.Count - 1
                        tblSelHaendler.Columns.Add(objSuche.Haendler.Table.Columns.Item(i).ColumnName, objSuche.Haendler.Table.Columns.Item(i).DataType)
                    Next i
                    For Each dataRow In dataRows
                        Dim newRow As DataRow
                        newRow = tblSelHaendler.NewRow()
                        newRow("CUSTOMER") = dataRow("CUSTOMER")
                        newRow("REFERENZ") = dataRow("REFERENZ")
                        newRow("FILIALE") = dataRow("FILIALE")
                        newRow("NAME") = dataRow("NAME")
                        newRow("NAME_2") = dataRow("NAME_2")
                        newRow("CITY") = dataRow("CITY")
                        newRow("POSTL_CODE") = dataRow("POSTL_CODE")
                        newRow("STREET") = dataRow("STREET")
                        newRow("COUNTRYISO") = dataRow("COUNTRYISO")
                        newRow("DISPLAY") = dataRow("DISPLAY")
                        newRow("DISPLAY_ADDRESS") = dataRow("DISPLAY_ADDRESS")
                        tblSelHaendler.Rows.Add(newRow)
                    Next

                    cmbHaendler.DataSource = tblSelHaendler '####
                    Session("objSuche") = objSuche
                    If Not IsNothing(objSuche.Haendler) Then objSuche.Haendler.Sort = "REFERENZ"
                    cmbHaendler.DataTextField = "DISPLAY"
                    cmbHaendler.DataValueField = "REFERENZ"
                    cmbHaendler.DataBind()
                    cmbHaendler.SelectedIndex = 0
                    cmbHaendler.Visible = True
                    lblAuswahl.Visible = True
                    cmdSelect.Visible = True
                    trHdAuswahl.Visible = True
                    Session("SelHaendler") = tblSelHaendler
                    Session("objSuche") = objSuche
                    lblMessage.Text = dataRows.Length() & " Treffer!"
                Else
                    lblMessage.Text = "Es konnten keine H�ndlerinformationen gefunden werden!"
                End If
            ElseIf Not txtNummer.Text = String.Empty Then

                If Not Session("App_DistriktID") = Nothing Then
                    objSuche = New Search(objApp, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                    objSuche.HaendlerFiliale = Session("App_DistriktID")
                Else
                    objSuche.HaendlerFiliale = m_User.Organization.OrganizationReference
                End If
                Dim tmpbValue As Boolean = objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, txtNummer.Text)
                If tmpbValue = True Then

                    Session("objSuche") = objSuche
                    Session("SelectedDealer") = txtNummer.Text
                    Response.Redirect("Report41_02.aspx?AppID=" & Session("AppID").ToString)
                Else
                    lblMessage.Text = "Es konnten keine H�ndlerinformationen gefunden werden!"
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Fehler bei der H�ndlersuche!"
        End Try
    End Sub
End Class

