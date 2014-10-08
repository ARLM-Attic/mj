﻿Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change03_3
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_User As Security.User
    Private m_App As Security.App
    Private m_change As Versand
    Private versandart As String
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)

        FormAuth(Me, m_User)

        versandart = Request.QueryString.Item("art")

        lnkFahrzeugAuswahl.NavigateUrl = "Change03_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart
        lnkFahrzeugsuche.NavigateUrl = "Change03.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart


        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        m_App = New Security.App(m_User)

        If Session("AppChange") Is Nothing Then
            Response.Redirect("Change03.aspx?AppID=" & Session("AppID").ToString & "art=" & versandart)
        End If
        m_change = CType(Session("AppChange"), Versand)

        If versandart = "ENDG" Then
            rb_Zulst.Checked = False
            ddlZulDienst.Enabled = False
            ddlZulDienst.Visible = False
            tr_ZulstSichtbarkeit.Visible = False
            tr_Grund.Visible = False
            trVersandAdrEnd4.Visible = False
        End If

        If Not rb_SAPAdresse.Checked Then
            ddlSAPAdresse.Visible = False
        Else
            ddlSAPAdresse.Visible = True
            checkSAPAdresse()
        End If
        If rb_Zulst.Checked Then
            checkZulst()
        End If
        If Not IsPostBack Then
            initialLoad()
        End If

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click


        If ddlLand.Visible = True Then
            If CInt(m_change.Laender.Select("Land1='" & ddlLand.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                If Not CInt(m_change.Laender.Select("Land1='" & ddlLand.SelectedItem.Value & "'")(0)("Lnplz")) = txtPlz.Text.Trim(" "c).Length Then
                    lblError.Text = "Postleitzahl hat falsche Länge."
                    Exit Sub
                End If
            End If
        End If

        If rb_SAPAdresse.Checked = True Then
            If ddlSAPAdresse.SelectedItem Is Nothing Then
                lblError.Text = "Keine Versandadresse ausgewählt."
                Exit Sub
            End If
        End If

        If rb_VersandAdresse.Checked AndAlso rb_VersandAdresse.Visible Then '# 1015 - 04.05.07 TB Auch auf Sichtbarkeit prüfen, da komischerweise sowohl rb_Versandadresse als auch rb_SAPAdresse gesetzt sein können, wenn rb_Versandadresse unsichtbar ist
            If (txtName1.Text = "" Or txtStr.Text = "" Or txtPlz.Text = "" Or txtNr.Text = "" Or txtOrt.Text = "") Then
                lblError.Text = "Bitte alle Felder füllen."
                Exit Sub
            End If
        End If


        DoSubmit()
    End Sub

    Private Sub rb_SAPAdresse_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rb_SAPAdresse.CheckedChanged
        checkSAPAdresse()
    End Sub

    Private Sub rb_VersandAdresse_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rb_VersandAdresse.CheckedChanged
        checkVersand()
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub rb_Zulst_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_Zulst.CheckedChanged
        checkZulst()
    End Sub


    Protected Sub ddlGrund_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlGrund.SelectedIndexChanged
        checkGrund()
    End Sub
#End Region

#Region "Methods"

    Private Sub initialLoad()
        Dim zulDienste As New DataTable()
        Dim status As String = ""
        Dim view0 As DataView
        Dim view As DataView
        Dim counter As Integer
        Dim sprache As String
        Dim tblTemp As DataTable
        Dim newRow As DataRow
        Dim item As ListItem

        'Länder DLL füllen
        ddlLand.DataSource = m_change.Laender
        ddlLand.DataTextField = "FullDesc"
        ddlLand.DataValueField = "Land1"
        ddlLand.DataBind()
        Dim tmpstr() As String
        If Request("HTTP_ACCEPT_Language").IndexOf(",") = -1 Then
            'es ist nur eine sprache ausgewählt
            sprache = Request("HTTP_ACCEPT_Language")
        Else
            'es gibt eine erst und eine zweitsprache
            sprache = Request("HTTP_ACCEPT_Language").Substring(0, Request("HTTP_ACCEPT_Language").IndexOf(","))
        End If

        tmpstr = sprache.Split(CChar("-"))
        'Länderkennzeichen setzen sich aus Region und Sprache zusammen. de-ch, de-at usw. leider werden bei Regionen in denen die Sprache das selbe Kürzel hat nur einfache Kürzel geschrieben, z.b. bei "de"
        If tmpstr.Length > 1 Then
            sprache = tmpstr(1).ToUpper
        Else
            sprache = tmpstr(0).ToUpper
        End If
        ddlLand.Items.FindByValue(sprache).Selected = True


        zulDienste = New DataTable()

        counter = 0

        m_change.getZulassungsdienste(Session("AppID").ToString, Session.SessionID, Me, zulDienste, status)

        tblTemp = zulDienste.Clone
        tblTemp.Columns.Add("DISPLAY")

        For Each row In zulDienste.Rows
            If row("ORT01").ToString <> String.Empty And row("LIFNR").ToString <> String.Empty Then
                Dim intTemp As Integer = InStr(row("ORT01").ToString, "ZLS")
                Dim intTemp2 As Integer = InStr(row("ORT01").ToString, "geschl")
                If Not (intTemp > 0 AndAlso intTemp2 > intTemp) Then
                    newRow = tblTemp.NewRow
                    newRow("DISPLAY") = row("ZKFZKZ").ToString & " - " & row("PSTLZ").ToString & " - " & row("ORT01").ToString & " - " & row("STRAS").ToString
                    newRow("LIFNR") = row("LIFNR").ToString
                    newRow("ORT01") = row("ORT01").ToString
                    newRow("STRAS") = row("STRAS").ToString
                    newRow("PSTLZ") = row("PSTLZ").ToString
                    newRow("PSTL2") = CType(counter, String)                  'Lfd. Nr. (für spätere Suche!)
                    tblTemp.Rows.Add(newRow)
                    tblTemp.AcceptChanges()
                    counter += 1
                End If
            End If
        Next

        zulDienste = tblTemp.Copy
        zulDienste.AcceptChanges()
        tblTemp = Nothing

        Session.Add("ZulDienste", zulDienste)

        View = zulDienste.DefaultView
        View.Sort = "DISPLAY"
        With ddlZulDienst
            .DataSource = View
            .DataTextField = "DISPLAY"
            .DataValueField = "PSTL2"
            .DataBind()
        End With


        Select Case m_change.Materialnummer
            Case "1385"
                rb_0900.Checked = True
            Case "1389"
                rb_1000.Checked = True
            Case "1390"
                rb_1200.Checked = True
        End Select


        'Versandadresse: Hinterlegte Adresse
        m_change.LeseAdressenSAP(Session("AppID").ToString, Session.SessionID, Me, "ADAC_B")
        If m_change.Adressen.Rows.Count > 0 Then
            view0 = m_change.Adressen.DefaultView
            view0.Sort = "DISPLAY_ADDRESS"
            With ddlSAPAdresse
                .DataSource = view0
                .DataTextField = "DISPLAY_ADDRESS"
                .DataValueField = "ADDRESSNUMBER"
                .DataBind()
            End With
            ddlSAPAdresse.Visible = True
        Else
            ddlSAPAdresse.Visible = False
            rb_SAPAdresse.Enabled = False
        End If
        lblError.Text = String.Empty

        'Versandadresse: Versandanschrift

        If (m_change.ZielFirma <> String.Empty) And (m_change.ZielFirma2 <> String.Empty) And (m_change.ZielStrasse <> String.Empty) And (m_change.ZielPLZ <> String.Empty) And (m_change.ZielOrt <> String.Empty) Then
            txtName1.Text = m_change.ZielFirma
            txtName2.Text = m_change.ZielFirma2
            txtStr.Text = m_change.ZielStrasse
            txtNr.Text = m_change.ZielHNr
            txtPlz.Text = m_change.ZielPLZ
            txtOrt.Text = m_change.ZielOrt
            ddlLand.Items.FindByValue(m_change.ZielLand).Selected = True
            rb_VersandAdresse.Checked = True
            checkVersand()
        End If

        'Gründe Füllen...
        For Each row In m_change.GrundTabelle.Rows
            item = New ListItem()
            item.Value = row("ZZVGRUND").ToString
            item.Text = row("TEXT50").ToString
            ddlGrund.Items.Add(item)
        Next
        checkGrund()



        'Zunächst Standardauswahl....
        If (rb_Zulst.Checked = False And rb_SAPAdresse.Checked = False And rb_VersandAdresse.Checked = False) Then
            rb_Zulst.Checked = True
            checkZulst()
        End If


    End Sub
    Private Sub checkGrund()
        If ddlGrund.SelectedItem.Text.Trim(" "c) = "Ummeldung" Or ddlGrund.SelectedItem.Text.Trim(" "c) = "Zulassung" Then
            txtAuf.Visible = True
            lbl_Auf.Visible = True
        Else
            txtAuf.Text = ""
            txtAuf.Visible = False
            lbl_Auf.Visible = False
        End If

    End Sub
    Private Sub selectVisibleRadiobutton(ByVal al As ArrayList)
        Dim tmpRB As RadioButton
        For Each tmpRB In al
            If tmpRB.Visible = True Then
                tmpRB.Checked = True
                Exit Sub
            End If
        Next
    End Sub

    Private Sub DoSubmit()
        Dim status As String
        Dim zulDienste As DataTable
        Dim key As String
        Dim row As DataRow()
        status = String.Empty

        zulDienste = CType(Session("ZulDienste"), DataTable)

        m_change.Materialnummer = ""
        If rb_0900.Checked And rb_0900.Visible = True Then
            m_change.Materialnummer = "1385"
            m_change.MaterialBezeichnung = rb_0900.Text
        ElseIf rb_1000.Checked And rb_1000.Visible = True Then
            m_change.Materialnummer = "1389"
            m_change.MaterialBezeichnung = rb_1000.Text
        ElseIf rb_1200.Checked And rb_1200.Visible = True Then
            m_change.Materialnummer = "1390"
            m_change.MaterialBezeichnung = rb_1200.Text
        End If
        If m_change.Materialnummer.Length = 0 Then
            lblError.Text = "Keine Versandart ausgewählt."
            Exit Sub
        End If
        m_change = CType(Session("AppChange"), Versand)

        If rb_Zulst.Checked AndAlso rb_Zulst.Visible Then 'JJ2007.11.20 mit Sichtbarkeitsprüfung, da nicht sichtbare Contorls automatisch checked sin
            key = ddlZulDienst.SelectedItem.Value
            row = zulDienste.Select("Pstl2='" & key & "'")

            m_change.VersandAdresse_ZE = row(0)("LIFNR").ToString      'Versandadresse Nr. (60...)
            m_change.VersandAdresseText = ddlZulDienst.SelectedItem.Text   'Versanddresse (Text...)

            m_change.VersandAdresseText = "Zulassungsstelle" & "<br>" & row(0)("STRAS").ToString & "<br>" & row(0)("PSTLZ").ToString & "&nbsp; " & row(0)("ORT01").ToString

            'SAP-Adresse nullen
            m_change.VersandAdresse_ZS = String.Empty

            'Manuelle Adresse nullen
            m_change.ZielFirma = String.Empty
            m_change.ZielFirma2 = String.Empty
            m_change.ZielStrasse = String.Empty
            m_change.ZielHNr = String.Empty
            m_change.ZielPLZ = String.Empty
            m_change.ZielOrt = String.Empty
            m_change.ZielLand = String.Empty
        Elseif rb_SAPAdresse.Checked AndAlso rb_SAPAdresse.Visible Then
            m_change.VersandAdresse_ZS = ddlSAPAdresse.SelectedItem.Value
            m_change.VersandAdresseText = ddlSAPAdresse.SelectedItem.Text
            'Zulassungsstelle nullen
            m_change.VersandAdresse_ZE = String.Empty
            Dim datRow() As DataRow
            datRow = m_change.Adressen.Select("ADDRESSNUMBER='" & ddlSAPAdresse.SelectedItem.Value & "'")
            If datRow.Length > 0 Then
                m_change.ZielFirma = datRow(0)("NAME1").ToString
                m_change.ZielFirma2 = datRow(0)("NAME2").ToString
                m_change.ZielStrasse = datRow(0)("STREET").ToString
                m_change.ZielHNr = datRow(0)("HOUSE_NUM1").ToString
                m_change.ZielPLZ = datRow(0)("POST_CODE1").ToString
                m_change.ZielOrt = datRow(0)("CITY1").ToString
                m_change.ZielLand = datRow(0)("COUNTRYISO").ToString
                m_change.VersandAdresseText = m_change.ZielFirma & "<br>" & m_change.ZielFirma2 & "<br>" & m_change.ZielStrasse & "&nbsp; " & m_change.ZielHNr & "<br>" & m_change.ZielPLZ & "&nbsp; " & m_change.ZielOrt
            End If


        ElseIf rb_VersandAdresse.Checked AndAlso rb_VersandAdresse.Visible Then 'JJ2007.11.20 mit Sichtbarkeitsprüfung, da nicht sichtbare Contorls automatisch checked sind
            m_change.ZielFirma = txtName1.Text                               'Adresse für Endgültigen Versand
            m_change.ZielFirma2 = txtName2.Text                               'Adresse für Endgültigen Versand
            m_change.ZielStrasse = txtStr.Text
            m_change.ZielHNr = txtNr.Text
            m_change.ZielPLZ = txtPlz.Text
            m_change.ZielOrt = txtOrt.Text
            m_change.ZielLand = ddlLand.SelectedItem.Value.ToString
            m_change.VersandAdresseText = m_change.ZielFirma & "<br>" & m_change.ZielFirma2 & "<br>" & m_change.ZielStrasse & "&nbsp; " & m_change.ZielHNr & "<br>" & m_change.ZielPLZ & "&nbsp; " & m_change.ZielOrt

            'SAP-Adresse nullen
            m_change.VersandAdresse_ZS = String.Empty

            'Zulassungsstelle nullen
            m_change.VersandAdresse_ZE = String.Empty
        End If

        If trVersandAdrEnd4.Visible Then
            m_change.VersandGrund = ddlGrund.SelectedItem.Value.ToString
            m_change.VersandGrundText = ddlGrund.SelectedItem.Text
            m_change.Auf = txtAuf.Text
        End If

        Session("AppChange") = m_change
        Response.Redirect("Change03_4.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart)
    End Sub
    Private Sub checkZulst()
        ddlSAPAdresse.Enabled = False
        ddlZulDienst.Visible = True
        ddlZulDienst.Enabled = True
        txtName1.Enabled = False
        txtName2.Enabled = False
        txtStr.Enabled = False
        txtNr.Enabled = False
        txtPlz.Enabled = False
        txtOrt.Enabled = False
        ddlLand.Enabled = False
        trVersandAdrEnd1.Visible = False
        trVersandAdrEnd2.Visible = False
        trVersandAdrEnd3.Visible = False
        tr_Land.Visible = False


    End Sub

    Private Sub checkSAPAdresse()
        ddlZulDienst.Enabled = False
        ddlZulDienst.Visible = False
        ddlSAPAdresse.Enabled = True
        txtName1.Enabled = False
        txtName2.Enabled = False
        txtStr.Enabled = False
        txtNr.Enabled = False
        txtPlz.Enabled = False
        txtOrt.Enabled = False
        ddlLand.Enabled = False
        trVersandAdrEnd1.Visible = False
        trVersandAdrEnd2.Visible = False
        trVersandAdrEnd3.Visible = False
        tr_Land.Visible = False
    End Sub

    Private Sub checkVersand()
        ddlZulDienst.Enabled = False
        ddlZulDienst.Visible = False
        ddlSAPAdresse.Enabled = False
        txtName1.Enabled = True
        txtName2.Enabled = True
        txtStr.Enabled = True
        txtNr.Enabled = True
        txtPlz.Enabled = True
        txtOrt.Enabled = True
        ddlLand.Enabled = True
        trVersandAdrEnd1.Visible = True
        trVersandAdrEnd2.Visible = True
        trVersandAdrEnd3.Visible = True
        tr_Land.Visible = True
    End Sub

#End Region


End Class