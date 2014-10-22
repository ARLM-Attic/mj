﻿Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts

'#################################################################
' Klasse für das Lesen und Ändern der Carportdaten#
' Change : Carport Bestände (Change01)
'#################################################################
Public Class Carport
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_strPDI As String
    Private m_datEingangsdatumVon As DateTime
    Private m_datEingangsdatumBis As DateTime
    Private m_datMeldungsdatumVon As DateTime
    Private m_datMeldungsdatumBis As DateTime
    Private m_strModell As String
    Private m_strFlag_Vorh As String
    Private m_tblResultPDIs As DataTable
    Private m_tblHersteller As DataTable
    Private m_tblLiefermonat As DataTable
    Private m_tblBereifung As DataTable
    Private m_tblGetriebe As DataTable
    Private m_tblKraftstoff As DataTable
    Private m_tblNavi As DataTable
    Private m_tblFarbe As DataTable
    Private m_tblVermiet As DataTable
    Private m_tblFzgArt As DataTable
    Private m_tblAufbauArt As DataTable
    Private m_tblHaendlernr As DataTable
    Private m_tblHandlername As DataTable
    Private m_tblEKIndikator As DataTable
    Private m_tblVerwZweck As DataTable
    Private m_tblHOwnerCode As DataTable
    Private m_tblSperrdat As DataTable
    Private m_tblZulKreis As DataTable
    Private m_tblResultCarports As DataTable
    Private m_tblResultExcel As DataTable
    Private m_strFilter As String
#End Region

#Region " Properties"
    Public ReadOnly Property ResultPDIs() As DataTable
        Get
            Return m_tblResultPDIs
        End Get
    End Property
    Public ReadOnly Property ResultCarports() As DataTable
        Get
            Return m_tblResultCarports
        End Get
    End Property
    Public Property EingangsdatumVon() As DateTime
        Get
            Return m_datEingangsdatumVon
        End Get
        Set(ByVal value As DateTime)
            m_datEingangsdatumVon = value
        End Set
    End Property
    Public Property EingangsdatumBis() As DateTime
        Get
            Return m_datEingangsdatumBis
        End Get
        Set(ByVal value As DateTime)
            m_datEingangsdatumBis = value
        End Set
    End Property
    Public Property MeldungsdatumVon() As DateTime
        Get
            Return m_datMeldungsdatumVon
        End Get
        Set(ByVal value As DateTime)
            m_datMeldungsdatumVon = value
        End Set
    End Property
    Public Property MeldungsdatumBis() As DateTime
        Get
            Return m_datMeldungsdatumBis
        End Get
        Set(ByVal value As DateTime)
            m_datMeldungsdatumBis = value
        End Set
    End Property
    Public Property PDI() As String
        Get
            Return m_strPDI
        End Get
        Set(ByVal value As String)
            m_strPDI = value
        End Set
    End Property

    Public Property Modell() As String
        Get
            Return m_strModell
        End Get
        Set(ByVal value As String)
            m_strModell = value
        End Set
    End Property
    Public Property Flag_Vorh() As String
        Get
            Return m_strFlag_Vorh
        End Get
        Set(ByVal value As String)
            m_strFlag_Vorh = value
        End Set
    End Property
    Public ReadOnly Property Hersteller() As DataTable
        Get
            Return m_tblHersteller
        End Get
    End Property
    Public ReadOnly Property Bereifung() As DataTable
        Get
            Return m_tblBereifung
        End Get
    End Property
    Public ReadOnly Property Liefermonat() As DataTable
        Get
            Return m_tblLiefermonat
        End Get
    End Property
    Public ReadOnly Property Getriebe() As DataTable
        Get
            Return m_tblGetriebe
        End Get
    End Property

    Public ReadOnly Property Kraftstoff() As DataTable
        Get
            Return m_tblKraftstoff
        End Get
    End Property

    Public ReadOnly Property Navi() As DataTable
        Get
            Return m_tblNavi
        End Get
    End Property
    Public ReadOnly Property Farbe() As DataTable
        Get
            Return m_tblFarbe
        End Get
    End Property
    Public ReadOnly Property Vermiet() As DataTable
        Get
            Return m_tblVermiet
        End Get
    End Property

    Public ReadOnly Property FzgArt() As DataTable
        Get
            Return m_tblFzgArt
        End Get
    End Property

    Public ReadOnly Property AufbauArt() As DataTable
        Get
            Return m_tblAufbauArt
        End Get
    End Property
    Public ReadOnly Property Haendlernr() As DataTable
        Get
            Return m_tblHaendlernr
        End Get
    End Property

    Public ReadOnly Property Handlername() As DataTable
        Get
            Return m_tblHandlername
        End Get
    End Property

    Public ReadOnly Property EKIndikator() As DataTable
        Get
            Return m_tblEKIndikator
        End Get
    End Property
    Public ReadOnly Property VerwZweck() As DataTable
        Get
            Return m_tblVerwZweck
        End Get
    End Property
    Public ReadOnly Property HOwnerCode() As DataTable
        Get
            Return m_tblHOwnerCode
        End Get
    End Property
    Public ReadOnly Property Sperrdat() As DataTable
        Get
            Return m_tblSperrdat
        End Get
    End Property
    Public ReadOnly Property ZulKreis() As DataTable
        Get
            Return m_tblZulKreis
        End Get
    End Property
    Public Property FilterString() As String
        Get
            Return m_strFilter
        End Get
        Set(ByVal value As String)
            m_strFilter = value
        End Set
    End Property
    Public ReadOnly Property ResultExcel() As DataTable
        Get
            Return m_tblResultExcel
        End Get
    End Property
#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    '----------------------------------------------------------------------
    ' Methode: Fill
    ' Autor: O.Rudolph
    ' Beschreibung: füllen der Detailsicht und Carportübersicht(Change01)
    ' Erstellt am: 14.11.2008
    ' ITA: 2352
    '----------------------------------------------------------------------

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String
        'Dim strKunnr As String = ""
        m_strClassAndMethod = "Carport.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        'strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        Dim intID As Int32 = -1
        Dim tblTemp2 As DataTable

        Try

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_READ_FZGPOOL_GRUNDDAT_005", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

            'strCom = "EXEC Z_M_READ_FZGPOOL_GRUNDDAT_005 @I_KUNNR_AG='" & strKunnr & "', " _
            '                            & "@GT_WEB=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"
            tblTemp2 = S.AP.GetExportTableWithInitExecute("Z_M_READ_FZGPOOL_GRUNDDAT_005.GT_WEB", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())



            'cmd.Connection = con
            'cmd.CommandText = strCom


            'Dim pSAPTableAUFTRAG As New SAPParameter("@pSAPTable", ParameterDirection.Output)
            'cmd.Parameters.Add(pSAPTableAUFTRAG)

            'cmd.ExecuteNonQuery()

            'tblTemp2 = DirectCast(pSAPTableAUFTRAG.Value, DataTable)
            'HelpProcedures.killAllDBNullValuesInDataTable(tblTemp2)



            m_tableResult = tblTemp2
            CreateOutPut(tblTemp2, strAppID)



            Dim dv As DataView

            dv = m_tblResult.DefaultView
            dv.Sort = "Carportnr"

            Dim row As DataRow
            Dim e As Int32


            Dim sCarport As String = ""
            dv.Sort = "Carportnr"
            e = 0
            m_tblResultPDIs = New DataTable
            m_tblResultPDIs.Columns.Add("Carportnr", System.Type.GetType("System.String"))
            m_tblResultPDIs.Columns.Add("Carport", System.Type.GetType("System.String"))
            m_tblResultPDIs = FillFirstRow(m_tblResultPDIs, "Carportnr", "Carport")

            Do While e < dv.Count
                row = m_tblResultPDIs.NewRow
                If sCarport <> dv.Item(e)("Carportnr").ToString Then
                    row(0) = dv.Item(e)("Carportnr")
                    row(1) = dv.Item(e)("Carportnr").ToString & " - " & dv.Item(e)("Carport").ToString
                    sCarport = dv.Item(e)("Carportnr").ToString
                    m_tblResultPDIs.Rows.Add(row)
                End If
                e = e + 1
            Loop


            dv.Sort = "Hersteller_ID_Avis"
            e = 0
            Dim HerrCode As String = ""
            m_tblHersteller = FillFirstRow(m_tblHersteller, "HerstellerID", "Hersteller_ID_Avis")
            row = m_tblHersteller.NewRow
            Do While e < dv.Count
                row = m_tblHersteller.NewRow
                If HerrCode <> dv.Item(e)("Hersteller_ID_Avis").ToString Then
                    row(0) = dv.Item(e)("Hersteller_ID_Avis")
                    row(1) = dv.Item(e)("Hersteller_ID_Avis")
                    m_tblHersteller.Rows.Add(row)
                    HerrCode = dv.Item(e)("Hersteller_ID_Avis").ToString
                End If

                e = e + 1
            Loop


            m_tblLiefermonat = FillFirstRow(m_tblLiefermonat, "ID", "Liefermonat")
            BoundViews(dv, m_tblLiefermonat, "Liefermonat")

            m_tblBereifung = FillFirstRow(m_tblBereifung, "ID", "REIFENART")
            BoundViews(dv, m_tblBereifung, "REIFENART")

            m_tblGetriebe = FillFirstRow(m_tblGetriebe, "ID", "ANTRIEBSART")
            BoundViews(dv, m_tblGetriebe, "ANTRIEBSART")


            m_tblKraftstoff = FillFirstRow(m_tblKraftstoff, "ID", "Kraftstoffart")
            BoundViews(dv, m_tblKraftstoff, "Kraftstoffart")


            m_tblNavi = FillFirstRow(m_tblNavi, "ID", "Navigation")
            dv.Sort = "Navigation"
            e = 0
            Dim sNavi As String = ""
            row = m_tblNavi.NewRow
            Do While e < dv.Count
                row = m_tblNavi.NewRow
                If sNavi <> dv.Item(e)("Navigation").ToString Then
                    row(0) = e
                    row(1) = dv.Item(e)("Navigation")
                    If dv.Item(e)("Navigation").ToString = "J" Then
                        row(1) = "Ja"
                    Else
                        row(1) = "Nein"
                    End If

                    m_tblNavi.Rows.Add(row)
                    sNavi = dv.Item(e)("Navigation").ToString
                End If
                e = e + 1
            Loop


            m_tblFarbe = FillFirstRow(m_tblFarbe, "ID", "FARBE")
            BoundViews(dv, m_tblFarbe, "FARBE")


            m_tblVermiet = FillFirstRow(m_tblVermiet, "ID", "Vermietgruppe")
            BoundViews(dv, m_tblVermiet, "Vermietgruppe")


            m_tblFzgArt = FillFirstRow(m_tblFzgArt, "ID", "Fahrzeugart")
            BoundViews(dv, m_tblFzgArt, "Fahrzeugart")


            m_tblAufbauArt = FillFirstRow(m_tblAufbauArt, "ID", "AUFBAUART")
            BoundViews(dv, m_tblAufbauArt, "AUFBAUART")


            m_tblHaendlernr = FillFirstRow(m_tblHaendlernr, "ID", "HaendlerId")
            BoundViews(dv, m_tblHaendlernr, "HaendlerId")


            m_tblHandlername = FillFirstRow(m_tblHandlername, "ID", "Haendler_Kurzname")
            BoundViews(dv, m_tblHandlername, "Haendler_Kurzname")


            m_tblEKIndikator = FillFirstRow(m_tblEKIndikator, "ID", "Einkaufsindikator")
            BoundViews(dv, m_tblEKIndikator, "Einkaufsindikator")

            m_tblVerwZweck = FillFirstRow(m_tblVerwZweck, "ID", "VERWENDUNGSZWECK")
            BoundViews(dv, m_tblVerwZweck, "VERWENDUNGSZWECK")

            m_tblHOwnerCode = FillFirstRow(m_tblHOwnerCode, "ID", "Owner_Code")
            BoundViews(dv, m_tblHOwnerCode, "Owner_Code")

            m_tblZulKreis = FillFirstRow(m_tblZulKreis, "ID", "ZULASSUNGSORT")
            BoundViews(dv, m_tblZulKreis, "ZULASSUNGSORT")

            dv.Sort = "Datum_zur_Sperre"
            e = 0
            Dim Sperrdate As String = ""
            m_tblSperrdat = FillFirstRow(m_tblSperrdat, "ID", "Datum_zur_Sperre")
            row = m_tblSperrdat.NewRow
            Do While e < dv.Count
                row = m_tblSperrdat.NewRow
                If Sperrdate <> dv.Item(e)("Datum_zur_Sperre").ToString AndAlso dv.Item(e)("Datum_zur_Sperre").ToString <> "00000000" Then
                    row(0) = e
                    row(1) = FormatDateTime(CDate(dv.Item(e)("Datum_zur_Sperre")), DateFormat.ShortDate)
                    m_tblSperrdat.Rows.Add(row)
                    Sperrdate = dv.Item(e)("Datum_zur_Sperre").ToString
                End If

                e = e + 1
            Loop



            m_tblResultCarports = New DataTable()
            m_tblResultCarports.Columns.Add("Carportnr", System.Type.GetType("System.String"))
            m_tblResultCarports.Columns.Add("Carport Name", System.Type.GetType("System.String"))
            m_tblResultCarports.Columns.Add("Fahrzeuge", System.Type.GetType("System.Int32"))

            Dim keys(1) As DataColumn
            keys(0) = m_tblResultCarports.Columns(0)

            m_tblResultCarports.PrimaryKey = keys


            dv = tblTemp2.DefaultView

            dv.Sort = "KUNPDI , ZZMODELL , ZZBEZEI , ZZDAT_EIN , CHASSIS_NUM"

            Dim PdiRow As DataRow
            Dim FindRow As DataRow

            row = tblTemp2.NewRow


            Dim tblSortDetails As DataTable


            tblSortDetails = m_tblResult.Clone

            dv.Sort = "KUNPDI asc, ZZMODELL asc, ZZBEZEI asc, ZZDAT_EIN asc ,CHASSIS_NUM asc"


            e = 0

            Do While e < dv.Count
                row = tblSortDetails.NewRow

                row(0) = dv.Item(e)("KUNPDI")
                row(1) = dv.Item(e)("NAME1_PDI")
                row(2) = dv.Item(e)("ZZCARPORT")
                row(3) = dv.Item(e)("ZZMODELL")
                row(4) = dv.Item(e)("ZZBEZEI")
                row(5) = dv.Item(e)("CHASSIS_NUM")
                row(6) = dv.Item(e)("TIDNR")
                row(7) = dv.Item(e)("ZZDAT_EIN")
                row(8) = dv.Item(e)("ZZDAT_BER")
                row(9) = dv.Item(e)("FARBE_DE")

                tblSortDetails.Rows.Add(row)

                e = e + 1
            Loop


            For Each row In tblSortDetails.Rows

                PdiRow = m_tblResultCarports.NewRow

                FindRow = m_tblResultCarports.Rows.Find(row(0))

                If FindRow Is Nothing = True Then
                    PdiRow(0) = row(0).ToString
                    PdiRow(1) = row(1).ToString
                    PdiRow(2) = 1

                    m_tblResultCarports.Rows.Add(PdiRow)
                Else
                    FindRow.BeginEdit()
                    FindRow(2) = CInt(FindRow(2)) + 1
                    FindRow.EndEdit()
                End If
            Next

            tblTemp2.AcceptChanges()
            'tblTemp2 = Nothing

            'm_tblResult = tblSortDetails

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            'CreateOutPut(tblTemp2, strAppID)
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", I_EING_DAT_VON=" & m_datEingangsdatumVon.ToShortDateString & ", I_EING_DAT_BIS=" & m_datEingangsdatumBis.ToShortDateString, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", I_EING_DAT_VON=" & m_datEingangsdatumVon.ToShortDateString & ", I_EING_DAT_BIS=" & m_datEingangsdatumBis.ToShortDateString & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            'cmd.Dispose()
            'con.Close()
            'con.Dispose()
            m_blnGestartet = False
        End Try
    End Sub

    Private Function FillFirstRow(ByVal Table As DataTable, ByVal ID As String, ByVal ColumnName As String) As DataTable
        Dim row As DataRow

        Table = New DataTable
        Table.Columns.Add(ID, System.Type.GetType("System.String"))
        Table.Columns.Add(ColumnName, System.Type.GetType("System.String"))

        row = Table.NewRow
        row(0) = "-1"
        row(1) = "- keine Auswahl -"
        Table.Rows.Add(row)

        Return Table
    End Function
    Private Sub BoundViews(ByVal View As DataView, ByVal Table As DataTable, ByVal Text As String)

        Dim sTemp As String = ""
        Dim row As DataRow
        Dim e As Int32

        View.Sort = Text
        e = 0
        Do While e < View.Count
            row = Table.NewRow
            If sTemp <> View.Item(e)(Text).ToString Then
                row(0) = e
                row(1) = View.Item(e)(Text)
                Table.Rows.Add(row)
                sTemp = View.Item(e)(Text).ToString
            End If
            e = e + 1
        Loop
    End Sub

    Public Function GetSaveTable() As DataTable

        Return S.AP.GetImportTableWithInit("Z_M_SAVE_FZGPOOL_GRUNDDAT_005.GT_WEB", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

    End Function


    Public Sub Save(ByVal strAppID As String, ByVal strSessionID As String, ByVal SaveTable As DataTable)
        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String
        'Dim strKunnr As String = ""
        m_strClassAndMethod = "Carport.Save"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        'strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        Dim intID As Int32 = -1

        Try

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_SAVE_FZGPOOL_GRUNDDAT_005", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

            'strCom = "EXEC Z_M_SAVE_FZGPOOL_GRUNDDAT_005 @I_KUNNR_AG='" & strKunnr & "', " _
            '                                       & "@GT_WEB=@pSAPTable OPTION 'disabledatavalidation'"

            ' *** wurde bereits über GetSaveTable() abgerufen
            ' *** Dim SapSaveTable As DataTable = S.AP.GetImportTableWithInit("Z_M_SAVE_FZGPOOL_GRUNDDAT_005.GT_WEB", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

            'cmd.Connection = con
            'cmd.CommandText = strCom


            'Dim pSAPTableAUFTRAG As New SAPParameter("@pSAPTable", SaveTable)
            'cmd.Parameters.Add(pSAPTableAUFTRAG)

            'cmd.ExecuteNonQuery()
            S.AP.Execute()


            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, SaveTable, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), SaveTable, False)
        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            'cmd.Dispose()
            'con.Close()
            'con.Dispose()
            m_blnGestartet = False

        End Try
    End Sub

    Public Function Filter(ByVal FilterString As String) As Integer

        Dim dv As DataView

        dv = Result.DefaultView
        dv.RowFilter = FilterString
        If dv.Count > 0 Then

            dv.Sort = "Carportnr"

            Dim row As DataRow
            Dim e As Int32

            e = 0
            m_tblResultPDIs = New DataTable
            m_tblResultPDIs.Columns.Add("Carportnr", System.Type.GetType("System.String"))
            m_tblResultPDIs.Columns.Add("Carport", System.Type.GetType("System.String"))
            m_tblResultPDIs = FillFirstRow(m_tblResultPDIs, "Carportnr", "Carport")
            Dim sCarport As String = ""
            Do While e < dv.Count
                row = m_tblResultPDIs.NewRow
                If sCarport <> dv.Item(e)("Carportnr").ToString Then
                    row(0) = dv.Item(e)("Carportnr")
                    row(1) = dv.Item(e)("Carportnr").ToString & " - " & dv.Item(e)("Carport").ToString
                    sCarport = dv.Item(e)("Carportnr").ToString
                    m_tblResultPDIs.Rows.Add(row)
                End If
                e = e + 1
            Loop


            dv.Sort = "Hersteller_ID_Avis"
            e = 0
            Dim HerrCode As String = ""
            m_tblHersteller = FillFirstRow(m_tblHersteller, "HerstellerID", "Hersteller_ID_Avis")
            row = m_tblHersteller.NewRow
            Do While e < dv.Count
                row = m_tblHersteller.NewRow
                If HerrCode <> dv.Item(e)("Hersteller_ID_Avis").ToString Then
                    row(0) = dv.Item(e)("Hersteller_ID_Avis")
                    row(1) = dv.Item(e)("Hersteller_ID_Avis")
                    m_tblHersteller.Rows.Add(row)
                    HerrCode = dv.Item(e)("Hersteller_ID_Avis").ToString
                End If

                e = e + 1
            Loop


            m_tblLiefermonat = FillFirstRow(m_tblLiefermonat, "ID", "Liefermonat")
            BoundViews(dv, m_tblLiefermonat, "Liefermonat")

            m_tblBereifung = FillFirstRow(m_tblBereifung, "ID", "REIFENART")
            BoundViews(dv, m_tblBereifung, "REIFENART")

            m_tblGetriebe = FillFirstRow(m_tblGetriebe, "ID", "ANTRIEBSART")
            BoundViews(dv, m_tblGetriebe, "ANTRIEBSART")


            m_tblKraftstoff = FillFirstRow(m_tblKraftstoff, "ID", "Kraftstoffart")
            BoundViews(dv, m_tblKraftstoff, "Kraftstoffart")


            m_tblNavi = FillFirstRow(m_tblNavi, "ID", "Navigation")
            dv.Sort = "Navigation"
            e = 0
            Dim sNavi As String = ""
            row = m_tblNavi.NewRow
            Do While e < dv.Count
                row = m_tblNavi.NewRow
                If sNavi <> dv.Item(e)("Navigation").ToString Then
                    row(0) = e
                    row(1) = dv.Item(e)("Navigation")
                    If dv.Item(e)("Navigation").ToString = "J" Then
                        row(1) = "Ja"
                    Else
                        row(1) = "Nein"
                    End If

                    m_tblNavi.Rows.Add(row)
                    sNavi = dv.Item(e)("Navigation").ToString
                End If
                e = e + 1
            Loop


            m_tblFarbe = FillFirstRow(m_tblFarbe, "ID", "FARBE")
            BoundViews(dv, m_tblFarbe, "FARBE")


            m_tblVermiet = FillFirstRow(m_tblVermiet, "ID", "Vermietgruppe")
            BoundViews(dv, m_tblVermiet, "Vermietgruppe")


            m_tblFzgArt = FillFirstRow(m_tblFzgArt, "ID", "Fahrzeugart")
            BoundViews(dv, m_tblFzgArt, "Fahrzeugart")


            m_tblAufbauArt = FillFirstRow(m_tblAufbauArt, "ID", "AUFBAUART")
            BoundViews(dv, m_tblAufbauArt, "AUFBAUART")


            m_tblHaendlernr = FillFirstRow(m_tblHaendlernr, "ID", "HaendlerId")
            BoundViews(dv, m_tblHaendlernr, "HaendlerId")


            m_tblHandlername = FillFirstRow(m_tblHandlername, "ID", "Haendler_Kurzname")
            BoundViews(dv, m_tblHandlername, "Haendler_Kurzname")


            m_tblEKIndikator = FillFirstRow(m_tblEKIndikator, "ID", "Einkaufsindikator")
            BoundViews(dv, m_tblEKIndikator, "Einkaufsindikator")

            m_tblVerwZweck = FillFirstRow(m_tblVerwZweck, "ID", "VERWENDUNGSZWECK")
            BoundViews(dv, m_tblVerwZweck, "VERWENDUNGSZWECK")

            m_tblHOwnerCode = FillFirstRow(m_tblHOwnerCode, "ID", "Owner_Code")
            BoundViews(dv, m_tblHOwnerCode, "Owner_Code")

            m_tblZulKreis = FillFirstRow(m_tblZulKreis, "ID", "ZULASSUNGSORT")
            BoundViews(dv, m_tblZulKreis, "ZULASSUNGSORT")

            dv.Sort = "Datum_zur_Sperre"
            e = 0
            Dim Sperrdate As String = ""
            m_tblSperrdat = FillFirstRow(m_tblSperrdat, "ID", "Datum_zur_Sperre")
            row = m_tblSperrdat.NewRow
            Do While e < dv.Count
                row = m_tblSperrdat.NewRow
                If Sperrdate <> dv.Item(e)("Datum_zur_Sperre").ToString AndAlso dv.Item(e)("Datum_zur_Sperre").ToString <> "00000000" Then
                    row(0) = e
                    row(1) = FormatDateTime(CDate(dv.Item(e)("Datum_zur_Sperre")), DateFormat.ShortDate)
                    m_tblSperrdat.Rows.Add(row)
                    Sperrdate = dv.Item(e)("Datum_zur_Sperre").ToString
                End If

                e = e + 1
            Loop

            m_tblResultCarports = New DataTable()
            m_tblResultCarports.Columns.Add("Carportnr", System.Type.GetType("System.String"))
            m_tblResultCarports.Columns.Add("Carport Name", System.Type.GetType("System.String"))
            m_tblResultCarports.Columns.Add("Fahrzeuge", System.Type.GetType("System.Int32"))

            Dim keys(1) As DataColumn
            keys(0) = m_tblResultCarports.Columns(0)

            m_tblResultCarports.PrimaryKey = keys


            dv = m_tblResult.DefaultView

            dv.Sort = "Carportnr , Typ_ID_Avis , Modellbezeichnung , Eingangsdatum , Fahrgestellnummer"

            Dim PdiRow As DataRow
            Dim FindRow As DataRow

            row = Result.NewRow


            Dim tblSortDetails As DataTable


            tblSortDetails = m_tblResult.Clone

            dv.Sort = "Carportnr asc, Typ_ID_Avis asc, Modellbezeichnung asc, Eingangsdatum asc ,Fahrgestellnummer asc"


            e = 0

            Do While e < dv.Count
                row = tblSortDetails.NewRow

                row(0) = dv.Item(e)("Carportnr")
                row(1) = dv.Item(e)("Carport")
                row(2) = dv.Item(e)("ZZCARPORT")
                row(3) = dv.Item(e)("Typ_ID_Avis")
                row(4) = dv.Item(e)("Modellbezeichnung")
                row(5) = dv.Item(e)("Fahrgestellnummer")
                row(6) = dv.Item(e)("NummerZBII")
                row(7) = dv.Item(e)("Eingangsdatum")
                row(8) = dv.Item(e)("Datum_Bereit")
                row(9) = dv.Item(e)("Farbe")

                tblSortDetails.Rows.Add(row)

                e = e + 1
            Loop


            For Each row In tblSortDetails.Rows

                PdiRow = m_tblResultCarports.NewRow

                FindRow = m_tblResultCarports.Rows.Find(row(0))

                If FindRow Is Nothing = True Then
                    PdiRow(0) = row(0).ToString
                    PdiRow(1) = row(1).ToString
                    PdiRow(2) = 1

                    m_tblResultCarports.Rows.Add(PdiRow)
                Else
                    FindRow.BeginEdit()
                    FindRow(2) = CInt(FindRow(2)) + 1
                    FindRow.EndEdit()
                End If
            Next


            Return dv.Count

        Else
            Return 0
        End If
    End Function

    Public Sub FilterExcel()

        Dim dv As DataView

        dv = Result.DefaultView
        dv.RowFilter = m_strFilter
        If dv.Count > 0 Then

            dv.Sort = "Carportnr"

            Dim row As DataRow
            Dim e As Int32

            e = 0
            m_tblResultExcel = New DataTable
            m_tblResultExcel.Columns.Add("Carport", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Hersteller Id.", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Model Id.", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Modellgruppe", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Modellbezeichnung", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Reifenart", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Datum Eingang", System.Type.GetType("System.DateTime"))
            m_tblResultExcel.Columns.Add("Datum Bereit", System.Type.GetType("System.DateTime"))
            m_tblResultExcel.Columns.Add("ZBII Nummer", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Datum Sperre", System.Type.GetType("System.DateTime"))
            m_tblResultExcel.Columns.Add("Sperrvermerk", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Liefermonat", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Zulassungsort", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Fahrzeugart", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Aufbauart", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Kraftstoffart", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Navi", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Navi CD", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Farbe", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Reifengröße", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Vermietgruppe", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Verwendungszweck", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Bezahltkennzeichen", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Owner-Code", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Händler Id.", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Händlerkurzname", System.Type.GetType("System.String"))
            m_tblResultExcel.Columns.Add("Einkaufsindikator", System.Type.GetType("System.String"))


            Dim sCarport As String = ""
            Do While e < dv.Count
                row = m_tblResultExcel.NewRow
                row(0) = dv.Item(e)("Carportnr")
                row(1) = dv.Item(e)("Hersteller_ID_Avis").ToString
                row(2) = dv.Item(e)("Typ_ID_Avis")
                row(3) = dv.Item(e)("Modellgruppe")
                row(4) = dv.Item(e)("Modellbezeichnung")
                row(5) = dv.Item(e)("Reifenart")
                row(6) = dv.Item(e)("Fahrgestellnummer")
                If IsDate(dv.Item(e)("Eingangsdatum")) Then
                    row(7) = dv.Item(e)("Eingangsdatum")
                End If

                If IsDate(dv.Item(e)("Datum_Bereit")) Then
                    row(8) = dv.Item(e)("Datum_Bereit")
                End If
                row(9) = dv.Item(e)("NummerZBII")
                If IsDate(dv.Item(e)("Datum_zur_Sperre")) Then
                    row(10) = dv.Item(e)("Datum_zur_Sperre")
                End If
                row(11) = dv.Item(e)("Sperrvermerk")
                row(12) = dv.Item(e)("Liefermonat")
                row(13) = dv.Item(e)("Zulassungsort")
                row(14) = dv.Item(e)("Fahrzeugart")
                row(15) = dv.Item(e)("Aufbauart")
                row(16) = dv.Item(e)("Kraftstoffart")
                row(17) = dv.Item(e)("Navigation")
                row(18) = dv.Item(e)("EING_NAVI_CD")
                row(19) = dv.Item(e)("Farbe")
                row(20) = dv.Item(e)("Reifengroeße")
                row(21) = dv.Item(e)("Vermietgruppe")
                row(22) = dv.Item(e)("Verwendungszweck")
                row(23) = dv.Item(e)("Bezahltkennzeichen")
                row(24) = dv.Item(e)("Owner_Code")
                row(25) = dv.Item(e)("HaendlerId")
                row(26) = dv.Item(e)("Haendler_Kurzname")
                row(27) = dv.Item(e)("Einkaufsindikator")
                m_tblResultExcel.Rows.Add(row)

                e = e + 1
            Loop
        End If
    End Sub

    'Private Sub ReplaceDBNullValues(ByRef datentabelle As DataTable)
    '    For Each tmpRow As DataRow In datentabelle.Rows
    '        For i As Int32 = 0 To tmpRow.ItemArray.Length - 1
    '            If Not datentabelle.Columns(i).DataType.ToString = "System.DateTime" Then
    '                If tmpRow(i) Is DBNull.Value Then
    '                    tmpRow(i) = String.Empty
    '                End If
    '            End If

    '        Next
    '    Next
    '    datentabelle.AcceptChanges()

    'End Sub
#End Region

End Class
' ************************************************
' $History: Carport.vb $
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 5.05.10    Time: 14:54
' Updated in $/CKAG/Applications/AppAvis/lib
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 6.04.09    Time: 13:37
' Updated in $/CKAG/Applications/AppAvis/lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 10.02.09   Time: 10:06
' Updated in $/CKAG/Applications/AppAvis/lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 13.01.09   Time: 11:29
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA: 2359
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 4.12.08    Time: 13:42
' Updated in $/CKAG/Applications/AppAvis/lib
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 1.12.08    Time: 16:13
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA: 2359 & kleinere Anpassungen
' 
' ************************************************