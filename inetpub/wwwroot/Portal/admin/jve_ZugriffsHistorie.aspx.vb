
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls

Public Class jve_ZugriffsHistorieAbfrage
    Inherits System.Web.UI.Page
    Private m_context As HttpContext = HttpContext.Current
    Private m_User As User
    Private m_App As App

    Private m_blnShowDetails() As Boolean
    Private m_objTrace As Base.Kernel.Logging.Trace

    Protected WithEvents lblError As Label
    Protected WithEvents txtAbDatum As TextBox
    Protected WithEvents btnOpenSelectAb As Button
    Protected WithEvents calAbDatum As Calendar
    Protected WithEvents cmdCreate As LinkButton
    Protected WithEvents TblSearch As HtmlTable
    Protected WithEvents trSearch As HtmlTableRow
    Protected WithEvents TblLog As HtmlTable
    Protected WithEvents lblDownloadTip As Label
    Protected WithEvents lnkExcel As HyperLink
    Protected WithEvents lblInfo As Label
    Protected WithEvents ddlPageSize As DropDownList
    Protected WithEvents ddbZeit As DropDownList
    Protected WithEvents ucStyles As Styles
    <CLSCompliant(False)> Protected WithEvents HGZ As DBauer.Web.UI.WebControls.HierarGrid
    Protected WithEvents txtBisDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnOpenSelectBis As System.Web.UI.WebControls.Button
    Protected WithEvents calBisDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents ddlCustomer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtUserID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFilterUserName As System.Web.UI.WebControls.TextBox
    Protected WithEvents cbxOnline As System.Web.UI.WebControls.RadioButton
    Protected WithEvents cbxAll As System.Web.UI.WebControls.RadioButton
    Protected WithEvents cbxError As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header

    Private Sub InitializeComponent()

    End Sub

    'Dim da As New SqlClient.SqlDataAdapter("SELECT UserName AS Benutzer, IsTestUser AS Testbenutzer, BAPI, StartTime AS Start, EndTime AS Ende, DATEDIFF(second, StartTime, EndTime) As Dauer, Sucess AS Erfolg, ErrorMessage AS Fehlermeldung FROM LogAccessSAP WHERE BAPI LIKE @BAPI AND StartTime BETWEEN CONVERT ( Datetime , '" & txtAbDatum.Text & "' , 104 ) AND CONVERT ( Datetime , '" & strTemp & "' , 104 ) ORDER BY BAPI", cn)
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "Zugriffshistorie"
        AdminAuth(Me, m_User, AdminLevel.Organization)

        GetAppIDFromQueryString(Me)
        Dim cn As SqlClient.SqlConnection

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                With ddlPageSize.Items
                    .Add("10")
                    .Add("20")
                    .Add("50")
                    .Add("100")
                    .Add("200")
                End With
                ddlPageSize.SelectedIndex = 1
                With ddbZeit.Items
                    .Add("1")
                    .Add("2")
                    .Add("3")
                    .Add("4")
                    .Add("5")
                    .Add("6")
                End With
                calAbDatum.SelectedDate = Today
                txtAbDatum.Text = calAbDatum.SelectedDate.ToShortDateString
                calBisDatum.SelectedDate = Today
                txtBisDatum.Text = calBisDatum.SelectedDate.ToShortDateString
                TblLog.Visible = False

                cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
                cn.Open()

                Dim cmdGetUser As New SqlClient.SqlCommand("SELECT DISTINCT Customername FROM dbo.Customer WHERE NOT (Customername like '') ORDER BY Customername", cn)
                Dim dt As New DataTable()
                Dim da As New SqlClient.SqlDataAdapter()
                da.SelectCommand = cmdGetUser
                da.Fill(dt)

                dt.Columns.Add("ID", System.Type.GetType("System.Int32"))
                Dim intTemp As Int32 = 1
                Dim rowTemp As DataRow
                For Each rowTemp In dt.Rows
                    rowTemp("ID") = intTemp
                    intTemp += 1
                Next
                rowTemp = dt.NewRow
                rowTemp("ID") = 0
                rowTemp("Customername") = "- alle -"
                dt.Rows.Add(rowTemp)
                Dim dv As DataView = dt.DefaultView
                dv.Sort = "ID"

                ddlCustomer.DataSource = dv
                ddlCustomer.DataTextField = "Customername"
                ddlCustomer.DataValueField = "Customername"
                ddlCustomer.DataBind()
                ddlCustomer.SelectedIndex = 0

                cn.Close()

            Else
                If Not m_context.Cache("m_objTrace") Is Nothing Then
                    m_objTrace = CType(m_context.Cache("m_objTrace"), Base.Kernel.Logging.Trace)
                End If
            End If
            ucStyles.TitleText = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString()
            ReDim m_blnShowDetails(HGZ.PageSize)
            'ReDim m_blnShowDetails(DataGrid1.PageSize)
            Dim i As Int32
            For i = 0 To HGZ.PageSize - 1
                m_blnShowDetails(i) = False
            Next
            'For i = 0 To DataGrid1.PageSize - 1
            '    m_blnShowDetails(i) = False
            'Next
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "jve_ZugriffsHistorieAbfrage", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True

        End Try
    End Sub

    Private Sub FillDataGrid(ByVal blnForceNew As Boolean, Optional ByVal intPageIndex As Int32 = 0, Optional ByVal strSort As String = "")
        Dim dsLogData As DataSet
        Dim dtUser As DataTable
        Dim dtAppDaten As DataTable
        Dim dtSAPDaten As DataTable
        Dim sql As String
        Dim sql1 As String
        Dim sql2 As String

        If Session("dsLogData") Is Nothing Then
            Try
                Dim strCustomerRestriction As String = ""
                If Not ddlCustomer.SelectedItem.Text = "- alle -" Then
                    strCustomerRestriction = " AND Customername = '" & ddlCustomer.SelectedItem.Text & "'"
                End If

                Dim strUserRestriction As String = ""
                If (Not Replace(txtFilterUserName.Text, "*", "").Trim(" "c).Length = 0) Then
                    strUserRestriction = " AND userName like '" & Replace(txtFilterUserName.Text, "*", "%") & "'"
                End If

                If cbxOnline.Checked Then
                    sql = "SELECT * FROM vwLogWebAccess WHERE endTime is Null" & strCustomerRestriction & strUserRestriction & " ORDER BY startTime DESC"
                    sql1 = "SELECT * FROM vwLogStandardData ORDER BY Inserted DESC"
                    sql2 = "SELECT * FROM vwLogStandardDataSAP WHERE endTime is Null ORDER BY startTime DESC"
                Else
                    Dim strBisDatum As String = CStr(CDate(txtBisDatum.Text).AddDays(1).ToShortDateString)
                    If cbxError.Checked Then
                        sql = "SELECT * FROM vwLogError WHERE (startTime BETWEEN '" & txtAbDatum.Text & "' AND '" & strBisDatum & "')" & strCustomerRestriction & strUserRestriction & " ORDER BY startTime DESC"
                        sql1 = "SELECT * FROM vwLogStandardError WHERE (Inserted BETWEEN '" & txtAbDatum.Text & "' AND '" & strBisDatum & "') ORDER BY Inserted DESC"
                        sql2 = "SELECT * FROM vwLogStandardDataSAP WHERE (startTime BETWEEN '" & txtAbDatum.Text & "' AND '" & strBisDatum & "') AND (Sucess = 0) ORDER BY startTime DESC"
                    Else
                        sql = "SELECT * FROM vwLogWebAccess WHERE (startTime BETWEEN '" & txtAbDatum.Text & "' AND '" & strBisDatum & "')" & strCustomerRestriction & strUserRestriction & " ORDER BY startTime DESC"
                        sql1 = "SELECT * FROM vwLogStandardData WHERE (Inserted BETWEEN '" & txtAbDatum.Text & "' AND '" & strBisDatum & "') ORDER BY Inserted DESC"
                        sql2 = "SELECT * FROM vwLogStandardDataSAP WHERE (startTime BETWEEN '" & txtAbDatum.Text & "' AND '" & strBisDatum & "') ORDER BY startTime DESC"
                    End If
                End If
                Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                conn.Open()

                Dim da As SqlClient.SqlDataAdapter

                dsLogData = New DataSet()

                da = New SqlClient.SqlDataAdapter(sql, conn)
                dtUser = New DataTable("Benutzer")
                da.Fill(dtUser)
                dsLogData.Tables.Add(dtUser)

                da = New SqlClient.SqlDataAdapter(sql1, conn)
                dtAppDaten = New DataTable("AppDaten")
                da.Fill(dtAppDaten)
                dsLogData.Tables.Add(dtAppDaten)

                da = New SqlClient.SqlDataAdapter(sql2, conn)
                dtSAPDaten = New DataTable("SAPDaten")
                da.Fill(dtSAPDaten)
                dsLogData.Tables.Add(dtSAPDaten)

                Dim dc1 As DataColumn
                Dim dc2 As DataColumn
                'Relation Benutzer => AppDaten
                dc1 = dsLogData.Tables("Benutzer").Columns("idSession")
                dc2 = dsLogData.Tables("AppDaten").Columns("idSession")
                Dim dr As DataRelation = New DataRelation("Benutzer_AppDaten", dc1, dc2, False)
                dsLogData.Relations.Add(dr)

                'Relation AppDaten => SAPDaten
                dc1 = dsLogData.Tables("AppDaten").Columns("StandardLogID")
                dc2 = dsLogData.Tables("SAPDaten").Columns("StandardLogID")
                dr = New DataRelation("AppDaten_SAPDaten", dc1, dc2, False)
                dsLogData.Relations.Add(dr)

                conn.Close()
                conn.Dispose()
                da.Dispose()

                Session("dsLogData") = dsLogData

                '#ALT
                'dt = DBManager.Execute.Query(sql)
                dtUser.Columns.Add("StartColor", System.Drawing.Color.Red.GetType)

                Dim rowTemp As DataRow
                dtUser.AcceptChanges()
                For Each rowTemp In dtUser.Rows
                    If (CDate(rowTemp("startTime")).AddHours(CType(ddbZeit.SelectedItem.Value, Integer)) < Now) And (TypeOf rowTemp("endTime") Is DBNull) Then
                        rowTemp("StartColor") = System.Drawing.Color.Red
                    Else
                        rowTemp("StartColor") = System.Drawing.Color.Black
                    End If
                Next
                dtUser.AcceptChanges()
            Catch ex As Exception
                m_App.WriteErrorText(1, m_User.UserName, "jve_LogMessage2", "checkData", ex.ToString)
                lblError.Text = "Fehler beim Lesen aus der Datenbank.<br>(" & ex.Message & ")"
                Exit Sub
            End Try
        Else
            dsLogData = CType(Session("dsLogData"), DataSet)
            dtUser = dsLogData.Tables("Benutzer")
            dtAppDaten = dsLogData.Tables("AppDaten")
            dtSAPDaten = dsLogData.Tables("SAPDaten")
        End If

        lblError.Text = "Datenanzeige (keine Datens�tze gefunden)"
        lblInfo.Text = ""
        HGZ.Visible = False
        'DataGrid1.Visible = False

        If (dtUser.Rows.Count > 0) Then
            lblError.Text = ""
            lblInfo.Text = "Datenanzeige: " & dtUser.Rows.Count & " Datens�tze gefunden"
            HGZ.Visible = True
            'DataGrid1.Visible = True
            If strSort.Length > 0 Then
                If CStr(ViewState("mySort")) = strSort Then
                    strSort &= " DESC"
                End If
            Else
                strSort = CStr(ViewState("mySort"))
            End If

            ViewState("mySort") = strSort

            dtUser.DefaultView.Sort = strSort
            With HGZ
                .CurrentPageIndex = intPageIndex
                .DataSource = dsLogData
                .DataMember = "Benutzer"
                .DataBind()
                .Visible = True
                If .PageCount > 1 Then
                    .PagerStyle.Visible = True
                Else
                    .PagerStyle.Visible = False
                End If

                Dim intItemCount As Int32 = HGZ.Items.Count
                Dim intI As Int32
                For intI = 0 To intItemCount - 1
                    HGZ.RowExpanded(intI) = False
                Next
            End With
            'With DataGrid1
            '    .CurrentPageIndex = intPageIndex
            '    .DataSource = dt
            '    .DataBind()
            '    .Visible = True
            'End With
            TblLog.Visible = True
        End If
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        HGZ.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        'DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillDataGrid(False)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        FillDataGrid(False, 0, e.SortExpression)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        FillDataGrid(False, e.NewPageIndex)
    End Sub

    Private Sub btnOpenSelectAb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectAb.Click
        calAbDatum.Visible = True
    End Sub

    Private Sub calAbDatum_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calAbDatum.SelectionChanged
        txtAbDatum.Text = calAbDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        'Dieser Code wird abgearbeitet, wenn der Link "Als Excel downloaden" ausgef�hrt wird
        lblError.Text = ""
        Session("dsLogData") = Nothing
        If Not IsDate(txtAbDatum.Text) Then
            If Not IsStandardDate(txtAbDatum.Text) Then
                If Not IsSAPDate(txtAbDatum.Text) Then
                    lblError.Text = "Geben Sie bitte ein g�ltiges Datum ein.<br>"
                End If
            End If
        End If
        If Not IsDate(txtBisDatum.Text) Then
            If Not IsStandardDate(txtBisDatum.Text) Then
                If Not IsSAPDate(txtBisDatum.Text) Then
                    lblError.Text = "Geben Sie bitte ein g�ltiges Datum ein.<br>"
                End If
            End If
        End If
        If lblError.Text.Length > 0 Then
            'Bei Fehler Prozedur beenden
            Exit Sub
        Else
            If System.DateTime.Compare(CDate(txtAbDatum.Text), CDate(txtBisDatum.Text)) > 0 Then
                lblError.Text = "Das Startdatum muss vor dem Enddatum liegen.<br>"
                Exit Sub
            End If
        End If

        FillDataGrid(True, 0, "startTime")

        If HGZ.Visible Then
            Dim dsResult As DataSet
            Dim tblResult As DataTable
            dsResult = CType(HGZ.DataSource, DataSet)
            tblResult = dsResult.Tables("Benutzer").Copy

            'Dim tblResult As DataTable
            'tblResult = CType(DataGrid1.DataSource, DataTable)

            With tblResult
                .Columns.Remove("id")
                .Columns.Remove("idSession")
                .Columns.Remove("StartColor")
                'Spalten umbenennen, damit sie in der Exceldatei vern�nftige Namen haben
                .Columns(0).ColumnName = "Benutzer"
                .Columns(1).ColumnName = "Kunde"
                .Columns(2).ColumnName = "Abmeldestatus"
                .Columns(3).ColumnName = "Test"
                .Columns(4).ColumnName = "Anfrageart"
                .Columns(5).ColumnName = "Browser"
                .Columns(6).ColumnName = "Startzeit"
                .Columns(7).ColumnName = "Endzeit"
            End With

            If Not tblResult.Rows.Count = 0 Then
                Dim objExcelExport As New Base.Kernel.Excel.ExcelExport()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                Try
                    Base.Kernel.Excel.ExcelExport.WriteExcel(tblResult, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                Catch
                End Try
                lblDownloadTip.Visible = True
                lnkExcel.Visible = True
                lnkExcel.NavigateUrl = "/Portal/Temp/Excel/" & strFileName
            End If
        End If
    End Sub

    Private Sub calAbDatum_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles calAbDatum.Load
        calAbDatum.Visible = False
    End Sub

    Private Sub calAbDatum_VisibleMonthChanged(ByVal sender As Object, ByVal e As MonthChangedEventArgs) Handles calAbDatum.VisibleMonthChanged
        calAbDatum.Visible = True
    End Sub

    Private Sub HGZ_TemplateSelection(ByVal sender As Object, ByVal e As DBauer.Web.UI.WebControls.HierarGridTemplateSelectionEventArgs) Handles HGZ.TemplateSelection
        Select Case (e.Row.Table.TableName)
            Case "AppDaten"
                e.TemplateFilename = "Templates\\AppData.ascx"
            Case Else
                Throw New NotImplementedException("Unexpected child row in TemplateSelection event")
        End Select
    End Sub


    Private Sub HGZ_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles HGZ.SortCommand
        FillDataGrid(False, 0, e.SortExpression)
    End Sub

    Private Sub HGZ_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles HGZ.PageIndexChanged
        FillDataGrid(False, e.NewPageIndex)
    End Sub

    Private Sub btnOpenSelectBis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectBis.Click
        calBisDatum.Visible = True
    End Sub

    Private Sub calBisDatum_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calBisDatum.SelectionChanged
        txtBisDatum.Text = calBisDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub calBisDatum_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles calBisDatum.Load
        calBisDatum.Visible = False
    End Sub

    Private Sub calBisDatum_VisibleMonthChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MonthChangedEventArgs) Handles calBisDatum.VisibleMonthChanged
        calBisDatum.Visible = True
    End Sub
End Class

' ************************************************
' $History: jve_ZugriffsHistorie.aspx.vb $
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
' *****************  Version 11  *****************
' User: Rudolpho     Date: 31.01.08   Time: 13:44
' Updated in $/CKG/Admin/AdminWeb
' BugFix Zugriffshistorie & Nachrichten
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 6.12.07    Time: 14:36
' Updated in $/CKG/Admin/AdminWeb
' ITA: 1440
' 
' *****************  Version 9  *****************
' User: Uha          Date: 22.05.07   Time: 14:23
' Updated in $/CKG/Admin/AdminWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 8  *****************
' User: Uha          Date: 22.05.07   Time: 11:27
' Updated in $/CKG/Admin/AdminWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 7  *****************
' User: Uha          Date: 13.03.07   Time: 10:53
' Updated in $/CKG/Admin/AdminWeb
' History-Eintrag vorbereitet
' 
' ************************************************