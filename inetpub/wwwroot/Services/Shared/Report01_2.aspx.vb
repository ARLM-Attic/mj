﻿Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common
Imports System.Text.RegularExpressions

Partial Public Class Report01_2
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_objTable As DataTable
    Private m_objExcel As DataTable
    Private legende As String
    Private csv As String
    Private schmal As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = Common.GetUser(Me)
        Common.FormAuth(Me, m_User)
        GridNavigation1.setGridElment(GridView1)

        lblError.Text = ""
        lblError.Visible = False

        If Not Session("PageSizeIndex") Is Nothing Then
            GridNavigation1.PageSizeIndex = CInt(Session("PageSizeIndex"))
        End If

        If (Session("ResultTable") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString)
        Else
            m_objTable = CType(Session("ResultTable"), DataTable)
        End If

        If (Session("ExcelTable") Is Nothing) Then
            m_objExcel = CType(Session("ResultTable"), DataTable)
        Else
            m_objExcel = CType(Session("ExcelTable"), DataTable)
        End If

        Try
            legende = Request.QueryString.Item("legende")
            If (legende = "Ja") Then
                lblInfo.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung"
            End If
            If (legende = "Report201") Then
                lblInfo.Text = "<br>'X' = vorhanden"
            End If
            If (legende = "Report203") Then
                lblInfo.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung, 'X' = vorhanden"
            End If

            BuildTextForLabelHead()

            m_App = New Base.Kernel.Security.App(m_User)

            legende = Request.QueryString.Item("legende")
            If (legende = "Ja") Then
                lblInfo.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung"
            End If

            If Not IsPostBack Then
                csv = Request.QueryString.Item("csv")
                If csv = "Ja" Then
                    lnkShowCSV.Visible = True
                    lblDownloadTip.Visible = True

                    lnkCreateExcel.Visible = False
                    Result.Visible = True

                    Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
                    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".csv"

                    excelFactory.CreateDocumentAndWriteToFilesystem(ConfigurationManager.AppSettings("ExcelPath") & strFileName, Me.m_objExcel, Me.Page, , , , , False)
                    lnkShowCSV.NavigateUrl = "/Portal/Temp/Excel/" & strFileName

                Else
                    lnkShowCSV.Visible = False
                    lblDownloadTip.Visible = False

                    lnkCreateExcel.Visible = True

                    Result.Visible = True
                End If



                If Not Session("ApplblInfoText") Is Nothing Then
                    If lblInfo.Text.Length = 0 Then
                        lblInfo.Text = CStr(Session("ApplblInfoText"))
                    Else
                        lblInfo.Text &= "<br>" & CStr(Session("ApplblInfoText"))
                    End If
                End If

                FillGrid(0)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            lblError.Visible = True
        End Try
    End Sub
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If m_objTable.Rows.Count = 0 Then
            GridView1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            ShowScript.Visible = False
        Else
            GridView1.Visible = True
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

            GridView1.PageIndex = intTempPageIndex
            GridView1.DataSource = tmpDataView

            GridView1.DataBind()

            Dim item As GridViewRow
            For Each item In GridView1.Rows
                Dim cell As TableCell
                For Each cell In item.Cells
                    If IsDate(cell.Text) Then
                        cell.Text = CDate(cell.Text).ToShortDateString
                    End If
                Next
            Next

            Dim k As Int32
            Dim l As Int32
            For l = 0 To GridView1.Rows.Count - 1
                For k = 0 To m_objTable.Columns.Count - 1
                    If m_objTable.Columns(k).ExtendedProperties.Count > 0 Then
                        Select Case m_objTable.Columns(k).ExtendedProperties("Alignment").ToString
                            Case "Right"
                                GridView1.Rows.Item(l).Cells(k).HorizontalAlign = HorizontalAlign.Right
                            Case "Center"
                                GridView1.Rows.Item(l).Cells(k).HorizontalAlign = HorizontalAlign.Center
                            Case Else
                                GridView1.Rows.Item(l).Cells(k).HorizontalAlign = HorizontalAlign.Left
                        End Select
                    End If

                Next
            Next

            If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                lblNoData.Text = CStr(Session("ShowOtherString"))
                lblNoData.Visible = True
            End If

        End If
    End Sub
  

    Private Function StripQueryStringFromUrl(ByVal pUrl As String) As String
        Return Regex.Replace(pUrl, "\?.*$", String.Empty)
    End Function

    Private Function GetUrlReferrerForCmdBack() As System.Uri
        Dim aUri As System.Uri
        Dim aString As String

        aUri = Request.UrlReferrer
        aString = Request.QueryString("legende")
        If aString = "AppVFS-ADR" Or aString = "AppVFS-KZL" Then
            aUri = New Uri(lblHidden.Text)
        End If
        Return aUri
    End Function

    Private Sub BuildTextForLabelHead()

        'Wenn die Anzeige aus AppVFS Kennzeichenbestand aufgerufen wurde,
        'soll nicht der Friendly Name angezeigt werden.
        Dim tmpName As String = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        If (legende = "AppVFS-ADR") Then
            lblHead.Text = "Adressen"

        ElseIf (legende = "AppVFS-KZL") Then
            lblHead.Text = "Kennzeichenliste"
        Else
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        End If
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        Try
            Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

            excelFactory.CreateDocumentAndSendAsResponse(strFileName, Me.m_objExcel, Me.Page)

        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            lblError.Visible = True
        End Try
    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Protected Overrides Sub OnUnload(e As System.EventArgs)
        MyBase.OnUnload(e)

        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Overrides Sub OnPreRender(e As System.EventArgs)
        MyBase.OnPreRender(e)

        Common.SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)
    End Sub
End Class