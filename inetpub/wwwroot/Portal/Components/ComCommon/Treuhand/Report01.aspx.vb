﻿Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Namespace Treuhand
    Partial Public Class Report01
        Inherits System.Web.UI.Page
        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App

        Dim m_report As Treuhandsperre

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
            FormAuth(Me, m_User)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            ucHeader.InitUser(m_User)
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            If Page.IsPostBack = False Then
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                m_report = New Treuhandsperre(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
                Session.Add("objReport", m_report)
                m_report.SessionID = Me.Session.SessionID
                m_report.AppID = CStr(Session("AppID"))
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 1
                'doSubmit()
            ElseIf Not Session("objReport") Is Nothing Then
                m_report = CType(Session("objReport"), Treuhandsperre)
            End If
        End Sub
        Private Sub doSubmit()

            m_report.FreigabeVon = txtFreigabevon.Text
            m_report.FreigabeBis = txtFreigabebis.Text
            If rbFreigegeben.Checked Then
                m_report.Aktion = "F"
            ElseIf rbAbgelehnt.Checked Then
                m_report.Aktion = "A"
            ElseIf rbGesperrt.Checked Then
                m_report.Aktion = "G"
            End If
            m_report.FILLReport(Session("AppID").ToString, Session.SessionID.ToString)
            If m_report.Status < 0 Then
                lblError.Text = m_report.Message
            Else
                If m_report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    GridView1.Visible = False
                    trExcel.Visible = False
                Else
                    Session("ResultTable") = m_report.Result
                    FillGrid(0)
                End If
            End If
        End Sub
        Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

            Dim ResultTable As DataTable

            ResultTable = CType(Session("ResultTable"), DataTable)

            If Not ResultTable Is Nothing Then

                If ResultTable.Rows.Count = 0 Then
                    lblError.Visible = True
                    lblError.Text = "Keine Daten zur Anzeige gefunden."
                    GridView1.Visible = False
                    trExcel.Visible = False
                Else
                    lblError.Visible = False
                    GridView1.Visible = True
                    lnkCreateExcel.Visible = True
                    trExcel.Visible = True
                    lblNoData.Visible = False
                    cmdLoeschen.Visible = True


                    Dim tmpDataView As New DataView(ResultTable)

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

                    GridView1.PageIndex = intTempPageIndex

                    GridView1.DataSource = tmpDataView
                    GridView1.DataBind()
                    'If rbFreigabe.Checked Then
                    '    GridView1.Columns(6).Visible = False
                    '    GridView1.Columns(7).Visible = False
                    '    GridView1.Columns(8).Visible = False
                    'End If
                End If
            Else
                lblError.Text = "Keine Daten zur Anzeige gefunden."
            End If
        End Sub

        Private Sub lnkCreateExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel.Click

            Dim control As New Control
            Dim tblTranslations As New DataTable()
            Dim tblTemp As DataTable = CType(Session("ResultTable"), DataTable).Copy
            Dim AppURL As String
            Dim col As DataControlField
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim i As Integer
            Dim sColName As String = ""

            If tblTemp.Columns.Contains("BELNR") Then
                tblTemp.Columns.Remove("BELNR")
            End If
            Dim tmpRow As DataRow

            For Each tmpRow In tblTemp.Rows
                tmpRow("Versandadresse") = Replace(tmpRow("Versandadresse").ToString, "<br/>", ", ")
            Next

            AppURL = Replace(Me.Request.Url.LocalPath, "/Portal", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)
            For Each col In GridView1.Columns
                For i = tblTemp.Columns.Count - 1 To 0 Step -1
                    bVisibility = 0
                    col2 = tblTemp.Columns(i)
                    If col2.ColumnName.ToUpper = col.SortExpression.ToUpper Then
                        sColName = TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, bVisibility)
                        If bVisibility = 0 Then
                            tblTemp.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName
                        End If
                    End If
                Next
                tblTemp.AcceptChanges()
            Next

            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)

            m_report.Result = Session("ResultTable")
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub

        Private Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
            FillGrid(e.NewPageIndex)
        End Sub

        Protected Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPageSize.SelectedIndexChanged
            GridView1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
            FillGrid(GridView1.PageIndex)
        End Sub



        Private Function Checkgrid(ByVal FreigabeFlag As String) As Boolean
            Dim item As GridViewRow
            Dim chkBox As CheckBox
            Dim bchecked As Boolean
            Dim label As Label
            bchecked = False
            lblError.Text = ""
            For Each item In GridView1.Rows
                chkBox = item.FindControl("chkSperre")
                If chkBox.Checked = True Then
                    label = item.FindControl("lblBELNR")
                    Dim dRow As DataRow = m_report.Result.Select("BELNR='" & label.Text & "'")(0)
                    dRow("zurFreigabe") = "X"

                    bchecked = True

                    m_report.Result.AcceptChanges()

                End If
                Session("ResultTable") = m_report.Result
            Next

            Return bchecked


        End Function

        Protected Sub cmdLoeschen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdLoeschen.Click

            If Checkgrid("L") Then
                m_report.Aktion = "L"
                'm_report.Treunehmer = Right("0000000000" & m_User.KUNNR, 10)
                m_report.Loeschen(Session("AppID"), Session.SessionID.ToString)
                If m_report.Status < 0 Then
                    lblError.Text = m_report.Message
                    GridView1.Visible = False
                    trExcel.Visible = False
                Else
                    doSubmit()
                End If
            Else

            End If

        End Sub

        Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
            doSubmit()
        End Sub
    End Class
End Namespace