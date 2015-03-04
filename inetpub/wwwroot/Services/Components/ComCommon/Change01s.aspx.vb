﻿Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports System.Data.OleDb
Imports System.Data

Partial Public Class Change01s
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objHaendler As Versand1
    Private versandart As String
    Private authentifizierung As String
    Private booError As Boolean
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        versandart = Request.QueryString.Item("art").ToString
        authentifizierung = Request.QueryString.Item("art2").ToString

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        If m_User.Organization.OrganizationAdmin Then
            lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
        End If

        m_App = New Base.Kernel.Security.App(m_User)

        If (Session("objHaendler") Is Nothing) OrElse (Not IsPostBack) Then
            objHaendler = New Versand1(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        Else
            objHaendler = CType(Session("objHaendler"), Versand1)
        End If

        'Für den Upload
        Me.Form.Enctype = "multipart/form-data"
        CheckAuswahl()

        Session("objHaendler") = objHaendler

    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click


        Try

            If rb_Upload.Checked = True Then

                'Prüfe Fehlerbedingung
                If (Not upFile1.PostedFile Is Nothing) AndAlso (Not (upFile1.PostedFile.FileName = String.Empty)) Then
                    'lblExcelfile.Text = upFile1.PostedFile.FileName
                    If Right(upFile1.PostedFile.FileName.ToUpper, 4) <> ".XLS" Then
                        lblerror.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden."
                        Exit Sub
                    End If
                Else
                    lblerror.Text = "Keine Datei ausgewählt"
                    Exit Sub
                End If

                booError = False

                'Lade Datei
                upload(upFile1.PostedFile)

                If booError = True Then Exit Sub



            Else

                If Not txtAmtlKennzeichen.Text = String.Empty Then
                    txtAmtlKennzeichen.Text = Replace(txtAmtlKennzeichen.Text, " ", "").Trim(","c)
                    If txtAmtlKennzeichen.Text.Length = 0 Then
                        txtAmtlKennzeichen.Text = String.Empty
                    End If
                End If
                If Not chk_alle.Checked Then
                    If (txtAmtlKennzeichen.Text = String.Empty And txtOrdernummer.Text = String.Empty And txtFahrgestellnummer.Text = String.Empty And txtNummerZB2.Text = String.Empty) Then
                        lblerror.Text = "Bitte geben Sie mindestens ein Suchkriterium an."
                        Exit Sub
                    End If
                End If

            End If

            DoSubmit()
        Catch ex As Exception
            'lblerror.Text = Err.Description
        End Try

    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        'Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub


#End Region

#Region "Methods"
    Private Sub DoSubmit()
        Dim b As Boolean
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
        Session.Add("logObj", logApp)

        lblerror.Text = ""

        b = True

        'Keine Platzhaltersuche -> Werfe Platzhalter 'raus
        If Not cbxPlatzhaltersuche.Checked Then
            txtOrdernummer.Text = Replace(txtOrdernummer.Text, "*", "")
            txtOrdernummer.Text = Replace(txtOrdernummer.Text, "%", "")

            txtAmtlKennzeichen.Text = Replace(txtAmtlKennzeichen.Text, "*", "")
            txtAmtlKennzeichen.Text = Replace(txtAmtlKennzeichen.Text, "%", "")

            txtFahrgestellnummer.Text = Replace(txtFahrgestellnummer.Text, "*", "")
            txtFahrgestellnummer.Text = Replace(txtFahrgestellnummer.Text, "%", "")
        End If

        'Briefnummer generell ohne Platzhalter
        txtNummerZB2.Text = Replace(txtNummerZB2.Text, "*", "")
        txtNummerZB2.Text = Replace(txtNummerZB2.Text, "%", "")

        If Not chk_alle.Checked Then
            If txtNummerZB2.Text.Length = 0 Then
                objHaendler.SucheNummerZB2 = ""
            Else
                objHaendler.SucheNummerZB2 = txtNummerZB2.Text.Replace(" ", "")
            End If


            If txtOrdernummer.Text.Length = 0 Then
                objHaendler.SucheLeasingvertragsNr = ""
            Else
                objHaendler.SucheLeasingvertragsNr = txtOrdernummer.Text.Replace(" ", "")
            End If

            txtFahrgestellnummer.Text = Replace(txtFahrgestellnummer.Text.Trim(" "c).Trim("*"c), " ", "")
            If txtFahrgestellnummer.Text.Length = 0 Then
                objHaendler.SucheFahrgestellNr = ""
            Else
                objHaendler.SucheFahrgestellNr = txtFahrgestellnummer.Text
                If objHaendler.SucheFahrgestellNr.Length < 17 Then
                    If objHaendler.SucheFahrgestellNr.Length > 4 Then
                        txtFahrgestellnummer.Text = "*" & objHaendler.SucheFahrgestellNr
                        objHaendler.SucheFahrgestellNr = "%" & objHaendler.SucheFahrgestellNr
                    Else
                        lblerror.Text = "Bitte geben Sie die Fahrgestellnummer mindestens 8-stellig ein."
                        b = False
                    End If
                End If
            End If

            If txtAmtlKennzeichen.Text.Length = 0 Then
                objHaendler.SucheKennzeichen = ""
            Else
                txtAmtlKennzeichen.Text = Replace(txtAmtlKennzeichen.Text.Trim(" "c), " ", "")
                objHaendler.SucheKennzeichen = txtAmtlKennzeichen.Text
                'prüfe auf Eingabeformat Kreis und ein Buchstabe JJU2008.04.07
                Dim tmpaKennzeichen As String()
                tmpaKennzeichen = txtAmtlKennzeichen.Text.Split(",".ToCharArray)
                Dim tmpStr As String
                Dim tmpStr2 As String
                For Each tmpStr2 In tmpaKennzeichen
                    tmpStr = tmpStr2.Replace("*", "")
                    If Not tmpStr.IndexOf("-") = -1 Then
                        If Not tmpStr.Length > tmpStr.IndexOf("-") + 1 OrElse tmpStr.IndexOf("-") = 0 OrElse tmpStr.Length < 3 Then
                            lblerror.Text = "Bitte beachten Sie das Eingabeformat für Kennzeichen."
                            b = False
                            Exit For
                        End If
                    Else
                        lblerror.Text = "Bitte beachten Sie das Eingabeformat für Kennzeichen."
                        b = False
                        Exit For
                    End If

                Next
            End If
        Else
            b = True
        End If
        If b Then
            objHaendler.Haendlernummer = m_User.Reference
            objHaendler.KUNNR = ""

            objHaendler.GiveCars()
            Dim blnGo As Boolean = False
            If Not objHaendler.Status = 0 Then
                lblerror.Text = objHaendler.Message
                lblerror.Visible = True
            Else
                If objHaendler.Result.Rows.Count = 0 Then
                    lblerror.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                Else
                    blnGo = True
                End If
            End If

            'SFA 10.11.2006 Wenn Kundengruppe gesetzt, dann nicht in die Historie abfragen
            If objHaendler.IsBooCustomerGroup = False Then


                If Not blnGo Then
                    If Not cbxPlatzhaltersuche.Checked Then
                        Dim tblTemp As DataTable = objHaendler.GiveResultStructure
                        Dim rowNew As DataRow
                        rowNew = tblTemp.NewRow
                        rowNew("MANDT") = "11"
                        rowNew("LIZNR") = objHaendler.SucheLeasingvertragsNr
                        rowNew("CHASSIS_NUM") = ""
                        rowNew("TIDNR") = ""
                        rowNew("LICENSE_NUM") = objHaendler.SucheKennzeichen
                        rowNew("ZZREFERENZ1") = ""
                        rowNew("ZZCOCKZ") = ""
                        rowNew("STATUS") = "Keine Daten gefunden."
                        Dim m_Report As New Versand2(m_User, m_App, "")
                        m_Report.FillHistory(Session("AppID").ToString, Session.SessionID.ToString, objHaendler.SucheKennzeichen, objHaendler.SucheFahrgestellNr, "", objHaendler.SucheLeasingvertragsNr)
                        If (Not m_Report.History Is Nothing) AndAlso (m_Report.History.Rows.Count = 1) AndAlso (Not m_Report.History.Rows(0)("ZZFAHRG").ToString = String.Empty) Then

                            'Fahrgestellnummer
                            rowNew("CHASSIS_NUM") = m_Report.History.Rows(0)("ZZFAHRG").ToString

                            'Auftragsnummer
                            rowNew("LIZNR") = m_Report.History.Rows(0)("ZZREF1").ToString

                            'Kfz-Briefnummer
                            rowNew("TIDNR") = m_Report.History.Rows(0)("ZZBRIEF").ToString

                            'Kfz-Kennzeichen
                            rowNew("LICENSE_NUM") = m_Report.History.Rows(0)("ZZKENN").ToString

                            'Equipmentnummer
                            rowNew("EQUNR") = m_Report.History.Rows(0)("EQUNR").ToString

                            'Switch für Änderung Versandart von temporär zu endgültig
                            rowNew("SWITCH") = False

                            If m_Report.History.Rows(0)("ZZSTATUS_ABG").ToString = "X" Then
                                rowNew("STATUS") = "Abgemeldet"
                            ElseIf m_Report.History.Rows(0)("ZZSTATUS_BAG").ToString = "X" Then
                                rowNew("STATUS") = "In Abmeldung"
                            ElseIf m_Report.History.Rows(0)("ABCKZ").ToString = "1" Then
                                rowNew("STATUS") = "Temporär versendet"
                            ElseIf m_Report.History.Rows(0)("ABCKZ").ToString = "2" Then
                                rowNew("STATUS") = "Endgültig versendet"
                            Else
                                If TypeOf m_Report.History.Rows(0)("EQUNR") Is String Then
                                    If objHaendler.CheckAgainstAuthorizationTable(m_Report.History.Rows(0)("EQUNR").ToString) Then
                                        rowNew("STATUS") = "Zur Freigabe"
                                    Else
                                        rowNew("STATUS") = "Bitte in Historie prüfen!"
                                    End If
                                Else
                                    rowNew("STATUS") = "Bitte in Historie prüfen!"
                                End If
                            End If
                            blnGo = True
                        End If
                        tblTemp.Rows.Add(rowNew)
                        If blnGo Then
                            objHaendler.Fahrzeuge = tblTemp
                        End If
                    End If
                End If
            End If
            If blnGo Then
                Session("objHaendler") = objHaendler
                Response.Redirect("Change01s_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&art2=" & authentifizierung)
            End If
        End If
    End Sub


    Private Sub upload(ByVal uFile As System.Web.HttpPostedFile)
        'Try
        Dim filepath As String = ConfigurationManager.AppSettings("ExcelPath")
        Dim filename As String
        Dim info As System.IO.FileInfo

        'Dateiname: User_yyyyMMddhhmmss.xls
        filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

        If Not (uFile Is Nothing) Then
            uFile.SaveAs(ConfigurationManager.AppSettings("ExcelPath") & filename)
            info = New System.IO.FileInfo(filepath & filename)
            If Not (info.Exists) Then
                lblerror.Text = "Fehler beim Speichern."
                Exit Sub
            End If

            'Datei gespeichert -> Auswertung
            Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
             "Data Source=" & filepath & filename & ";" & _
             "Extended Properties=""Excel 8.0;HDR=YES;"""

            Dim objConn As New OleDbConnection(sConnectionString)
            objConn.Open()

            Dim objCmdSelect As New OleDbCommand("SELECT * FROM [Tabelle1$]", objConn)

            Dim objAdapter1 As New OleDbDataAdapter()
            objAdapter1.SelectCommand = objCmdSelect

            Dim objDataset1 As New DataSet()
            objAdapter1.Fill(objDataset1, "XLData")

            Dim tblTemp As DataTable = CheckInputTable(objDataset1.Tables(0))

            objConn.Close()

            If IsNothing(tblTemp) Then
                Exit Sub
            End If

            If Not tblTemp.Rows Is Nothing AndAlso tblTemp.Rows.Count > 0 Then
                objHaendler.Fahrzeuge = tblTemp
                Session("objHaendler") = objHaendler
                Response.Redirect("Change01s_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&art2=" & authentifizierung)
            Else
                lblerror.Text = "Datei enthielt keine verwendbaren Daten."
            End If
        End If
        'Catch
        '    Throw New Exception(Err.Description)
        'End Try
    End Sub

    Private Function CheckInputTable(ByVal tblInput As DataTable) As DataTable
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
        Session.Add("logObj", logApp)

        Dim i As Integer = 0
        Dim rowData As DataRow
        Dim tblReturn As DataTable = Nothing

        For Each rowData In tblInput.Rows
            i += 1
            If TypeOf rowData(0) Is System.DBNull Then Exit For

            Dim Fahrgestellnummer As String = ""
            If Not TypeOf rowData(0) Is System.DBNull Then
                Fahrgestellnummer = CStr(rowData(0)).Trim(" "c)
            End If
            'Dim strKennzeichen As String = ""
            'If Not TypeOf rowData(1) Is System.DBNull Then
            '    strKennzeichen = CStr(rowData(1)).Trim(" "c)
            'End If

            If Fahrgestellnummer.Length = 0 Then Exit For


            'If strKennzeichen.Length > 0 Then
            '    objHaendler.SucheLeasingvertragsNr = ""
            '    objHaendler.SucheKennzeichen = strKennzeichen
            'Else

            '    objHaendler.SucheLeasingvertragsNr = Left(strLeasingvertragsNr, 7)
            '    objHaendler.SucheKennzeichen = ""
            'End If

            objHaendler.SucheKennzeichen = ""
            objHaendler.SucheLeasingvertragsNr = ""
            objHaendler.SucheFahrgestellNr = Fahrgestellnummer
            objHaendler.Haendlernummer = ""
            objHaendler.KUNNR = ""

            objHaendler.GiveCars()

            If IsNothing(objHaendler.Fahrzeuge) Then
                If i = 1 Then
                    lblerror.Text = "Fahrzeug nicht gefunden."
                    booError = True
                    Return Nothing
                End If
            End If


            If tblReturn Is Nothing Then
                tblReturn = objHaendler.GiveResultStructure
            End If



            Dim ColumnCounter As Integer = tblReturn.Columns.Count - 1
            Dim j As Integer
            Dim rowNew As DataRow
            rowNew = tblReturn.NewRow
            If Not objHaendler.Status = 0 Then
                rowNew("MANDT") = "11"
                rowNew("LIZNR") = ""
                rowNew("CHASSIS_NUM") = objHaendler.SucheFahrgestellNr
                rowNew("TIDNR") = ""
                rowNew("LICENSE_NUM") = ""
                rowNew("ZZREFERENZ1") = ""
                rowNew("ZZCOCKZ") = ""
                rowNew("SWITCH") = False
                rowNew("STATUS") = objHaendler.Message
                rowNew("EXPIRY_DATE") = String.Empty
            Else
                If objHaendler.Fahrzeuge.Rows.Count = 0 Then
                    rowNew("MANDT") = "11"
                    rowNew("LIZNR") = ""
                    rowNew("CHASSIS_NUM") = objHaendler.SucheFahrgestellNr
                    rowNew("TIDNR") = ""
                    rowNew("LICENSE_NUM") = ""
                    rowNew("ZZREFERENZ1") = ""
                    rowNew("ZZCOCKZ") = ""
                    rowNew("STATUS") = "Keine Daten gefunden."
                Else
                    For j = 0 To ColumnCounter
                        rowNew(j) = objHaendler.Fahrzeuge.Rows(0)(j)
                    Next
                    rowNew("MANDT") = "99"
                End If
            End If
            If Not CStr(rowNew("MANDT")) = "99" Then
                Dim m_Report As New Versand2(m_User, m_App, "")
                m_Report.FillHistory(Session("AppID").ToString, Session.SessionID.ToString, objHaendler.SucheKennzeichen, "", "", objHaendler.SucheLeasingvertragsNr)
                If (Not m_Report.History Is Nothing) AndAlso (m_Report.History.Rows.Count = 1) AndAlso (Not m_Report.History.Rows(0)("ZZFAHRG").ToString = String.Empty) Then

                    'Fahrgestellnummer
                    rowNew("CHASSIS_NUM") = m_Report.History.Rows(0)("ZZFAHRG").ToString

                    'Auftragsnummer
                    rowNew("LIZNR") = m_Report.History.Rows(0)("ZZREF1").ToString

                    'Kfz-Briefnummer
                    rowNew("TIDNR") = m_Report.History.Rows(0)("ZZBRIEF").ToString

                    'Kfz-Kennzeichen
                    rowNew("LICENSE_NUM") = m_Report.History.Rows(0)("ZZKENN").ToString

                    'Equipmentnummer
                    rowNew("EQUNR") = m_Report.History.Rows(0)("EQUNR").ToString

                    'Switch für Änderung Versandart von temporär zu endgültig
                    rowNew("SWITCH") = False

                    If m_Report.History.Rows(0)("ZZSTATUS_ABG").ToString = "X" Then
                        rowNew("STATUS") = "Abgemeldet"
                    ElseIf m_Report.History.Rows(0)("ZZSTATUS_BAG").ToString = "X" Then
                        rowNew("STATUS") = "In Abmeldung"
                    ElseIf m_Report.History.Rows(0)("ABCKZ").ToString = "1" Then
                        rowNew("STATUS") = "Temporär versendet"
                    ElseIf m_Report.History.Rows(0)("ABCKZ").ToString = "2" Then
                        rowNew("STATUS") = "Endgültig versendet"
                    Else
                        If TypeOf m_Report.History.Rows(0)("EQUNR") Is String Then
                            If objHaendler.CheckAgainstAuthorizationTable(m_Report.History.Rows(0)("EQUNR").ToString) Then
                                rowNew("STATUS") = "Zur Freigabe"
                            Else
                                rowNew("STATUS") = "Bitte in Historie prüfen!"
                            End If
                        Else
                            rowNew("STATUS") = "Bitte in Historie prüfen!"
                        End If
                    End If
                End If
            End If
            tblReturn.Rows.Add(rowNew)
Ignore:
        Next

        Return tblReturn
    End Function

    Private Sub CheckAuswahl()

        If rb_Einzelauswahl.Checked = True Then
            tr_Leasingvertragsnummer.Visible = True
            tr_Kennzeichen.Visible = True
            tr_KennzeichenZusatz.Visible = True
            tr_Fahrgestellnummer.Visible = True
            tr_FahrgestellnummerZusatz.Visible = True
            tr_NummerZB2.Visible = True
            tr_Platzhaltersuche.Visible = True
            tr_Alle.Visible = True
            tr_upload.Visible = False
        Else
            tr_Leasingvertragsnummer.Visible = False
            tr_Kennzeichen.Visible = False
            tr_KennzeichenZusatz.Visible = False
            tr_Fahrgestellnummer.Visible = False
            tr_FahrgestellnummerZusatz.Visible = False
            tr_NummerZB2.Visible = False
            tr_Platzhaltersuche.Visible = False
            tr_Alle.Visible = False
            tr_upload.Visible = True
        End If

    End Sub


#End Region

  
    Protected Sub rb_Upload_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_Upload.CheckedChanged
        CheckAuswahl()
    End Sub

    Protected Sub rb_Einzelauswahl_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_Einzelauswahl.CheckedChanged
        CheckAuswahl()
    End Sub
End Class

' ************************************************
' $History: Change01s.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 6.10.09    Time: 12:55
' Updated in $/CKAG2/Services/Components/ComCommon
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 6.10.09    Time: 9:41
' Updated in $/CKAG2/Services/Components/ComCommon
' BugFix: Upload Chrysler
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 28.09.09   Time: 16:20
' Updated in $/CKAG2/Services/Components/ComCommon
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 25.09.09   Time: 10:35
' Updated in $/CKAG2/Services/Components/ComCommon
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 24.09.09   Time: 10:44
' Updated in $/CKAG2/Services/Components/ComCommon
' ITA: 3112
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 24.09.09   Time: 9:47
' Created in $/CKAG2/Services/Components/ComCommon
'