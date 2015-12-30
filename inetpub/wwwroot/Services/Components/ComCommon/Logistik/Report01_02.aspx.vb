﻿Imports CKG.Base.Kernel.Security
Imports CKG.Base.Business
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Report01_02
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Private BRIEFLEBENSLAUF_LPTable As DataTable
    Private QMMIDATENTable As DataTable
    Private QMEL_DATENTable As DataTable
    Private objPDIs As ABEDaten

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)

        Try
            m_App = New App(m_User)
            If (Session("BRIEFLEBENSLAUF_LPTable") Is Nothing) Then
                Response.Redirect(Request.UrlReferrer.ToString & "?AppID=" & Session("AppID").ToString)
            Else
                BRIEFLEBENSLAUF_LPTable = CType(Session("BRIEFLEBENSLAUF_LPTable"), DataTable)
            End If

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            If Not IsPostBack Then



                'Fülle Übersicht
                '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                FillUebersicht()

                'Fülle Typdaten
                '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                FillABEDaten()

                'Fülle Lebenslauf
                '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                FillGrid2(0)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Datagrid2_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles Datagrid2.PageIndexChanged
        FillGrid2(e.NewPageIndex)
    End Sub

    Private Sub Datagrid2_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles Datagrid2.SortCommand
        FillGrid2(Datagrid2.CurrentPageIndex, e.SortExpression)
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click

        If Not Request.QueryString.Item("LinkedApp") Is Nothing Then
            Dim sUrl As String
            sUrl = Request.QueryString.Item("LinkedApp")
            Response.Redirect("../../" & sUrl, False)

        Else 'If Request.UrlReferrer.OriginalString.Contains("Report01.aspx") Then
            Response.Redirect("Report01.aspx?AppID=" & Session("AppID").ToString, False)

        End If

    End Sub

    Protected Sub lbCreatePDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreatePDF.Click
        Try
            Dim imageHt As New Hashtable()
            Dim mReport As Logistik.Historie_Log
            mReport = CType(Session("objReport"), Logistik.Historie_Log)
            Try
                imageHt.Add("Logo", m_User.Customer.LogoImage)
            Catch ex As Exception
                ' LogoPath am Customer nicht (korrekt) gepflegt - hier: ignorieren
            End Try
            If objPDIs Is Nothing Then
                objPDIs = CType(Session("objPDIs"), ABEDaten)
            End If

            With mReport
                .Fahrgestellnummer = lblFahrgestellnummerShow.Text
                .Abmeldedatum = lblAbmeldedatumShow.Text
                .BriefStatus = lblStatusShow.Text
                .Kennzeichen = lblKennzeichenShow.Text
                .Lagerort = lblLagerortShow.Text
                .Hersteller = lblHerstellerShow.Text
                .Fahrzeugmodell = lblFahrzeugmodellShow.Text
                .Farbe = objPDIs.ABE_Daten.ZZFARBE
                .Herstellerschluessel = lblHerstellerSchluesselShow.Text
                .Typschluessel = lblTypschluesselShow.Text
                .VarianteVersion = lblVarianteVersionShow.Text
                .ZBIINummerBEZ = lbl_Briefnummer.Text
                .ZBIINummer = lblBriefnummerShow.Text
                .Erstzulassungsdatum = lblErstzulassungsdatumShow.Text
                .Abmeldedatum = lblAbmeldedatumShow.Text
                .Finanzierungsnummer = lblOrdernummerShow.Text
                .FinanzierungsnummerBEZ = lbl_Ordernummer.Text
                'traurig aber wahr, nach string "<br>" splittet er nicht richtig JJU2008.07.01
                .FahrzeughalterName1Name2 = lblFahrzeughalterShow.Text.Replace("<br>", ";").Split(CChar(";"))(0)
                .FahrzeughalterStrasseNummer = lblFahrzeughalterShow.Text.Replace("<br>", ";").Split(CChar(";"))(1)
                .FahrzeughalterPLZOrt = lblFahrzeughalterShow.Text.Replace("<br>", ";").Split(CChar(";"))(2)
                .VersandadresseName1Name2 = lblStandortShow.Text.Replace("<br>", ";").Split(CChar(";"))(0)
                .VersandadresseStrasseNummer = lblStandortShow.Text.Replace("<br>", ";").Split(CChar(";"))(1)
                .VersandadressePLZOrt = lblStandortShow.Text.Replace("<br>", ";").Split(CChar(";"))(2)
                .VersandadresseBEZ = lbl_Standort.Text
                .AusdruckDatumUhrzeit = Date.Now.ToShortDateString & " / " & Date.Now.ToShortTimeString
                .Username = m_User.UserName
                If cbxCOC.Checked = True Then
                    .COC = "Ja"
                Else
                    .COC = "Nein"
                End If
                .COCBEZ = lbl_CoC.Text
                .UmgemeldetAM = lblUmgemeldetAmShow.Text
                .EhmaligesKennzeichen = lblEhemaligesKennzeichenShow.Text
                If chkBriefaufbietung.Checked = True Then
                    .ZBIIAufbietung = "Ja"
                Else
                    .ZBIIAufbietung = "Nein"
                End If
                .EhmaligeZBIINummer = lblEhemaligeBriefnummerShow.Text

            End With



            Dim tblData As DataTable = CKG.Base.Kernel.Common.DataTableHelper.ObjectToDataTable(mReport)

            Dim docFactory As New CKG.Base.Kernel.DocumentGeneration.WordDocumentFactory(tblData, imageHt)

            Dim dataTables(1) As DataTable

            dataTables(0) = createLebenslaufTable()
            dataTables(1) = createUebermittlungsTable()
            docFactory.CreateDocument("Fahrzeughistorie_" & lblFahrgestellnummerShow.Text, Me.Page, "\Applications\appF2\docu\FahrzeughistoriePrint.doc", "Tabelle", dataTables)
        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen des Ausdrucks: " + ex.Message
        End Try

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

#End Region


#Region "Methods"



    Private Sub FillUebersicht()
        lblKennzeichenShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZKENN").ToString
        lblEhemaligesKennzeichenShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZKENN_OLD").ToString
        lblBriefnummerShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZBRIEF").ToString
        lblEhemaligeBriefnummerShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZBRIEF_OLD").ToString
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZBRIEF_A").ToString = "X" Then
            chkBriefaufbietung.Checked = True
        Else
            chkBriefaufbietung.Checked = False
        End If
        lblFahrgestellnummerShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZFAHRG").ToString
        'lnkSchluesselinformationen.NavigateUrl &= lblFahrgestellnummerShow.Text
        lblOrdernummerShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZREF1").ToString
        If MakeStandardDateString(BRIEFLEBENSLAUF_LPTable.Rows(0)("REPLA_DATE").ToString) = "00.00.0000" Then
            lblErstzulassungsdatumShow.Text = ""
        Else
            lblErstzulassungsdatumShow.Text = MakeStandardDateString(BRIEFLEBENSLAUF_LPTable.Rows(0)("REPLA_DATE").ToString)
        End If
        If MakeStandardDateString(BRIEFLEBENSLAUF_LPTable.Rows(0)("EXPIRY_DATE").ToString) = "00.00.0000" Then
            lblAbmeldedatumShow.Text = ""
        Else
            lblAbmeldedatumShow.Text = MakeStandardDateString(BRIEFLEBENSLAUF_LPTable.Rows(0)("EXPIRY_DATE").ToString)
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZSTATUS_ZUB").ToString = "X" Then
            lblStatusShow.Text = "Zulassungsfähig"
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZSTATUS_ZUL").ToString = "X" Then
            lblStatusShow.Text = "Zugelassen"
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZSTATUS_ABG").ToString = "X" Then
            lblStatusShow.Text = "Abgemeldet"
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZSTATUS_BAG").ToString = "X" Then
            lblStatusShow.Text = "Bei Abmeldung"
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZSTATUS_OZU").ToString = "X" Then
            lblStatusShow.Text = "Ohne Zulassung"
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZAKTSPERRE").ToString = "X" Then
            lblStatusShow.Text = "Gesperrt"
        End If

        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZCOCKZ").ToString = "X" Then   'COC?
            cbxCOC.Checked = True
        End If
        lblStandortShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("NAME1_VS").ToString
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("NAME2_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= "<br>" & BRIEFLEBENSLAUF_LPTable.Rows(0)("NAME2_VS").ToString
        End If
        lblStandortShow.Text &= "<br>"
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("STRAS_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= BRIEFLEBENSLAUF_LPTable.Rows(0)("STRAS_VS").ToString
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("HSNR_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= " " & BRIEFLEBENSLAUF_LPTable.Rows(0)("HSNR_VS").ToString
        End If
        lblStandortShow.Text &= "<br>"
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("PSTLZ_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= BRIEFLEBENSLAUF_LPTable.Rows(0)("PSTLZ_VS").ToString
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ORT01_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= " " & BRIEFLEBENSLAUF_LPTable.Rows(0)("ORT01_VS").ToString
        End If
        lblFahrzeughalterShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("NAME1_ZH").ToString
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("NAME2_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= "<br>" & BRIEFLEBENSLAUF_LPTable.Rows(0)("NAME2_ZH").ToString
        End If
        lblFahrzeughalterShow.Text &= "<br>"
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("STRAS_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= BRIEFLEBENSLAUF_LPTable.Rows(0)("STRAS_ZH").ToString
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("HSNR_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= " " & BRIEFLEBENSLAUF_LPTable.Rows(0)("HSNR_ZH").ToString
        End If
        lblFahrzeughalterShow.Text &= "<br>"
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("PSTLZ_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= BRIEFLEBENSLAUF_LPTable.Rows(0)("PSTLZ_ZH").ToString
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ORT01_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= " " & BRIEFLEBENSLAUF_LPTable.Rows(0)("ORT01_ZH").ToString
        End If

        Select Case BRIEFLEBENSLAUF_LPTable.Rows(0)("ABCKZ").ToString
            Case ""
                lblLagerortShow.Text = "DAD"
            Case "0"
                lblLagerortShow.Text = "DAD"
            Case "1"
                If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZTMPDT").ToString = "" OrElse BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZTMPDT").ToString = "00000000" Then
                    lblLagerortShow.Text = "temporär angefordert"
                Else
                    lblLagerortShow.Text = "temporär versendet"
                End If
            Case "2"
                If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZTMPDT").ToString = "" OrElse BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZTMPDT").ToString = "00000000" Then
                    lblLagerortShow.Text = "endgültig angefordert"
                Else
                    lblLagerortShow.Text = "endgültig versendet"
                End If
        End Select


        If MakeStandardDateString(BRIEFLEBENSLAUF_LPTable.Rows(0)("UDATE").ToString) = "00.00.0000" Then
            lblUmgemeldetAmShow.Text = ""
        Else
            lblUmgemeldetAmShow.Text = MakeStandardDateString(BRIEFLEBENSLAUF_LPTable.Rows(0)("UDATE").ToString)
        End If
        lblFahrzeugmodellShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZHANDELSNAME").ToString

        lblHerstellerShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZHERST_TEXT").ToString
        lblHerstellerSchluesselShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZHERSTELLER_SCH").ToString
        lblTypschluesselShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZTYP_SCHL").ToString
        'lblFarbe.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZFARBE").ToString
        lblVarianteVersionShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZVVS_SCHLUESSEL").ToString

        'Bemerkungen in die Übersicht eintragen

        If Not Session("AnzBemerkungen") Is Nothing Then
            lblAnzBemerkungenShow.Text = CType(Session("AnzBemerkungen"), String)
        End If


        Dim mReport As Logistik.Historie_Log
        mReport = CType(Session("objReport"), Logistik.Historie_Log)

        If Len(BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZVGRUND").ToString) > 0 Then
            lblVersandgrundShow.Text = mReport.Abrufgrund(BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZVGRUND").ToString).ToString
        End If




    End Sub

    Private Sub FillABEDaten()
        'Farben erstmal ausblenden
        lbl_55.Visible = False
        lbl_91.Visible = False
        lbl_92.Visible = False
        lbl_93.Visible = False
        lbl_94.Visible = False
        lbl_95.Visible = False
        lbl_96.Visible = False
        lbl_97.Visible = False
        lbl_98.Visible = False
        lbl_99.Visible = False

        lbl_155.Visible = False
        lbl_191.Visible = False
        lbl_192.Visible = False
        lbl_193.Visible = False
        lbl_194.Visible = False
        lbl_195.Visible = False
        lbl_196.Visible = False
        lbl_197.Visible = False
        lbl_198.Visible = False
        lbl_199.Visible = False

        objPDIs = New ABEDaten(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", m_User.KUNNR, "ZDAD", "", "")

        If Not objPDIs Is Nothing Then
            If BRIEFLEBENSLAUF_LPTable.Rows(0)("EQUNR") Is Nothing OrElse CType(BRIEFLEBENSLAUF_LPTable.Rows(0)("EQUNR"), String).Trim(" "c).Length = 0 Then
                lblError.Text = "Fehler: Die Daten enthalten keine Fahrzeugnummer."
            Else
                'TrimStart("0"c)
                objPDIs.FillDatenABE(Session("AppID").ToString, Session.SessionID.ToString, Me.Page, CType(BRIEFLEBENSLAUF_LPTable.Rows(0)("EQUNR"), String))
                If objPDIs.Status = 0 Then
                    With objPDIs.ABE_Daten
                        'lbl_00.Text = .Farbziffer
                        lbl_0.Text = .ZZKLARTEXT_TYP
                        lbl_1.Text = .ZZHERST_TEXT
                        lbl_2.Text = .ZZHERSTELLER_SCH
                        lbl_3.Text = .ZZHANDELSNAME
                        lbl_4.Text = .ZZGENEHMIGNR
                        lbl_5.Text = .ZZGENEHMIGDAT
                        lbl_6.Text = .ZZFHRZKLASSE_TXT
                        lbl_7.Text = .ZZTEXT_AUFBAU
                        lbl_8.Text = .ZZFABRIKNAME
                        lbl_9.Text = .ZZVARIANTE
                        lbl_10.Text = .ZZVERSION
                        lbl_11.Text = .ZZHUBRAUM.TrimStart("0"c)
                        lbl_13.Text = .ZZNENNLEISTUNG.TrimStart("0"c)
                        lbl_14.Text = .ZZBEIUMDREH.TrimStart("0"c)
                        lbl_12.Text = .ZZHOECHSTGESCHW
                        lbl_19.Text = .ZZSTANDGERAEUSCH.TrimStart("0"c)
                        lbl_20.Text = .ZZFAHRGERAEUSCH.TrimStart("0"c)
                        lbl_15.Text = .ZZKRAFTSTOFF_TXT
                        lbl_16.Text = .ZZCODE_KRAFTSTOF
                        lbl_21.Text = .ZZFASSVERMOEGEN
                        lbl_17.Text = .ZZCO2KOMBI
                        lbl_18.Text = .ZZSLD & " / " & .ZZNATIONALE_EMIK
                        lbl_22.Text = .ZZABGASRICHTL_TG
                        lbl_23.Text = .ZZANZACHS.TrimStart("0"c)
                        lbl_24.Text = .ZZANTRIEBSACHS.TrimStart("0"c)
                        lbl_26.Text = .ZZANZSITZE.TrimStart("0"c)
                        lbl_25.Text = .ZZACHSL_A1_STA.TrimStart("0"c) & ", " & .ZZACHSL_A2_STA.TrimStart("0"c) & ", " & .ZZACHSL_A3_STA.TrimStart("0"c)
                        If Right(lbl_25.Text, 2) = ", " Then
                            lbl_25.Text = Left(lbl_25.Text, lbl_25.Text.Length - 2)
                        End If
                        lbl_27.Text = .ZZBEREIFACHSE1 & ", " & .ZZBEREIFACHSE2 & ", " & .ZZBEREIFACHSE3
                        If Right(lbl_27.Text, 2) = ", " Then
                            lbl_27.Text = Left(lbl_27.Text, lbl_27.Text.Length - 2)
                        End If

                        lbl_28.Text = .ZZZULGESGEW.TrimStart("0"c)
                        lbl_29.Text = .ZZTYP_SCHL
                        lbl_30.Text = .ZZBEMER1 & "<br>" & .ZZBEMER2 & "<br>" & .ZZBEMER3 & "<br>" & .ZZBEMER4 & "<br>" & .ZZBEMER5 & "<br>" & .ZZBEMER6 & "<br>" & .ZZBEMER7 & "<br>" & .ZZBEMER8 & "<br>" & .ZZBEMER9 & "<br>" & .ZZBEMER10 & "<br>" & .ZZBEMER11 & "<br>" & .ZZBEMER12 & "<br>" & .ZZBEMER13 & "<br>" & .ZZBEMER14
                        lbl_31.Text = .ZZLAENGEMIN.TrimStart("0"c)
                        lbl_32.Text = .ZZBREITEMIN.TrimStart("0"c)
                        lbl_33.Text = .ZZHOEHEMIN

                        lbl_00.Text = .ZZFARBE & " (" & .Farbziffer & ")"
                        lbl_55.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                        lbl_91.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                        lbl_92.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                        lbl_93.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                        lbl_94.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                        lbl_95.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                        lbl_96.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                        lbl_97.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                        lbl_98.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                        lbl_99.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"

                        lbl_200.Text = .ZZFARBE & " (" & .Farbziffer & ")"
                        lbl_155.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                        lbl_191.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                        lbl_192.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                        lbl_193.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                        lbl_194.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                        lbl_195.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                        lbl_196.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                        lbl_197.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                        lbl_198.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                        lbl_199.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"

                        Select Case .Farbziffer
                            Case "0"
                                lbl_99.Visible = True
                                lbl_199.Visible = True
                            Case "1"
                                lbl_98.Visible = True
                                lbl_198.Visible = True
                            Case "2"
                                lbl_97.Visible = True
                                lbl_197.Visible = True
                            Case "3"
                                lbl_96.Visible = True
                                lbl_196.Visible = True
                            Case "4"
                                lbl_95.Visible = True
                                lbl_195.Visible = True
                            Case "5"
                                lbl_94.Visible = True
                                lbl_194.Visible = True
                            Case "6"
                                lbl_93.Visible = True
                                lbl_193.Visible = True
                            Case "7"
                                lbl_92.Visible = True
                                lbl_192.Visible = True
                            Case "8"
                                lbl_91.Visible = True
                                lbl_191.Visible = True
                            Case "9"
                                lbl_55.Visible = True
                                lbl_155.Visible = True
                            Case Else

                        End Select
                        Session.Add("objPDIs", objPDIs)
                    End With
                Else
                    lblError.Text = objPDIs.Message
                End If
            End If
        End If
    End Sub

    Private Sub FillGrid2(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If Not (Session("QMEL_DATENTable") Is Nothing) Then
            QMEL_DATENTable = CType(Session("QMEL_DATENTable"), DataTable)

            If QMEL_DATENTable.Rows.Count > 0 Then
                Dim tmpDataView As New DataView()
                tmpDataView = QMEL_DATENTable.DefaultView

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

                Datagrid2.CurrentPageIndex = intTempPageIndex
                Datagrid2.DataSource = tmpDataView

                Datagrid2.DataBind()

                If Datagrid2.PageCount > 1 Then
                    Datagrid2.PagerStyle.CssClass = "PagerStyle"
                    Datagrid2.DataBind()
                    Datagrid2.PagerStyle.Visible = True
                Else
                    Datagrid2.PagerStyle.Visible = False
                End If

                Dim item As DataGridItem
                Dim cell As TableCell
                Dim control As Control
                Dim label As Label
                Dim literal As Literal
                Dim text As String

                For Each item In Datagrid2.Items
                    cell = item.Cells(2)

                    text = ""
                    For Each control In cell.Controls
                        If TypeOf control Is Label Then
                            If control.ID = "Label1" Or control.ID = "Label2" Or control.ID = "Label3" Then
                                label = CType(control, Label)
                                text &= label.Text
                            End If
                        End If
                    Next
                    If text.Trim(" "c).Length = 0 Then
                        For Each control In cell.Controls
                            If TypeOf control Is Literal Then
                                literal = CType(control, Literal)
                                literal.Text = ""
                            End If
                        Next
                    End If
                Next

            End If
        End If
    End Sub

    Private Function MakeStandardDateString(ByVal strSAPDate As String) As String
        Return Right(strSAPDate, 2) & "." & Mid(strSAPDate, 5, 2) & "." & Left(strSAPDate, 4)
    End Function


    Private Function createLebenslaufTable() As DataTable
        Dim tmpTable As DataTable = CType(Session("QMEL_DATENTable"), DataTable)
        Dim Lebenslauf As New DataTable
        'Dim tmpRow As DataRow

        Lebenslauf.Columns.Add("Vorgang", System.Type.GetType("System.String"))
        Lebenslauf.Columns.Add("Durchführungsdatum", System.Type.GetType("System.String"))
        Lebenslauf.Columns.Add("Versandadresse", System.Type.GetType("System.String"))
        Lebenslauf.Columns.Add("Versandart", System.Type.GetType("System.String"))
        Lebenslauf.Columns.Add("Beauftragt durch", System.Type.GetType("System.String"))

        For Each tmpRow As DataRow In tmpTable.Rows

            Dim tmpLebenslaufRow As DataRow = Lebenslauf.NewRow()

            tmpLebenslaufRow.Item("Vorgang") = tmpRow("KURZTEXT").ToString
            tmpLebenslaufRow.Item("Durchführungsdatum") = tmpRow("STRMN").ToString
            tmpLebenslaufRow.Item("Versandadresse") = tmpRow("NAME1_Z5").ToString & " " & tmpRow("NAME2_Z5").ToString & " / " & tmpRow("STREET_Z5").ToString & " " & tmpRow("HOUSE_NUM1_Z5").ToString & " / " & tmpRow("POST_CODE1_Z5").ToString & " " & tmpRow("CITY1_Z5").ToString
            tmpLebenslaufRow.Item("Versandart") = tmpRow("ZZDIEN1").ToString
            tmpLebenslaufRow.Item("Beauftragt durch") = tmpRow("QMNAM").ToString

            Lebenslauf.Rows.Add(tmpLebenslaufRow)
            Lebenslauf.AcceptChanges()

        Next
        Lebenslauf.TableName = "Lebenslauf"
        Return Lebenslauf


    End Function

    Private Function createUebermittlungsTable() As DataTable
        Dim tmpTable As DataTable = CType(Session("QMMIDATENTable"), DataTable)
        Dim Uebersicht As New DataTable
        'Dim tmpRow As DataRow

        Uebersicht.Columns.Add("Aktionscode", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("Vorgang", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("Statusdatum", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("Übermittlungsdatum", System.Type.GetType("System.String"))

        For Each tmpRow As DataRow In tmpTable.Rows

            Dim tmpUebersichtsRow As DataRow = Uebersicht.NewRow()

            tmpUebersichtsRow.Item("Aktionscode") = tmpRow("MNCOD").ToString
            tmpUebersichtsRow.Item("Vorgang") = tmpRow("MATXT").ToString
            tmpUebersichtsRow.Item("Statusdatum") = tmpRow("PSTER").ToString
            tmpUebersichtsRow.Item("Übermittlungsdatum") = tmpRow("ZZUEBER").ToString


            Uebersicht.Rows.Add(tmpUebersichtsRow)
            Uebersicht.AcceptChanges()

        Next
        Uebersicht.TableName = "Übermittlung"
        Return Uebersicht


    End Function
#End Region


End Class