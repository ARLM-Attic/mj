﻿Option Explicit On
Option Strict Off

Imports System

Imports CKG.Base.Kernel
Imports CKG.Base.Business
'Imports Microsoft.Data.SAPClient
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Text
Imports System.Linq
Imports ERPConnect
Imports System.Configuration

Namespace Common

    <Serializable()> Public Class DynSapProxyObj

#Region "Declarations"

        Private mBapiName As String
        Private mBapiDate As Date
        Private mBapiLoadet As Date
        Private mImport As DataTable
        Private mExport As DataTable
        'Private mbizTalkImportParameter As New Generic.List(Of SAPParameter)
        'Private mbizTalkExportParameter As New Generic.List(Of SAPParameter)
        Private mCommandText As String = ""
        Private mClearMe As Boolean = False
        Private mObjLogApp As CKG.Base.Kernel.Logging.Trace
        Private mObjMyApplication As CKG.Base.Kernel.Security.App
        Private mObjMyUser As CKG.Base.Kernel.Security.User
        Private mObjMyPage As Web.UI.Page

#End Region

#Region "Properties"
        Private ReadOnly Property commandtext() As String
            Get
                Return mCommandText
            End Get
        End Property

        Private ReadOnly Property SapConnectionString() As String
            Get
                Dim conStr As String = ""
                conStr = "ASHOST=" & mObjMyApplication.SAPAppServerHost & _
                                        ";CLIENT=" & mObjMyApplication.SAPClient & _
                                        ";SYSNR=" & mObjMyApplication.SAPSystemNumber & _
                                        ";USER=" & mObjMyApplication.SAPUsername & _
                                        ";PASSWD=" & mObjMyApplication.SAPPassword & _
                                        ";LANG=DE"
                Return conStr
            End Get
        End Property

        Public ReadOnly Property BapiName() As String
            Get
                Return mBapiName
            End Get
        End Property

        Public ReadOnly Property BapiDate() As Date
            Get
                Return mBapiDate
            End Get
        End Property

        Protected Friend ReadOnly Property BapiLoadet() As Date
            Get
                Return mBapiLoadet
            End Get
        End Property


        Public ReadOnly Property Import() As DataTable
            Get
                Return mImport
            End Get
        End Property

        Public ReadOnly Property Export() As DataTable
            Get
                Return mExport
            End Get
        End Property
#End Region

#Region "Methods"

        Protected Sub New()
        End Sub


        Protected Friend Sub New(ByVal BapiName As String, ByVal SapDatum As Date, ByVal impStruktur As DataTable, ByVal expStruktur As DataTable)
            mBapiName = BapiName
            mBapiDate = SapDatum
            mBapiLoadet = Now
            mImport = GenerateANewDataTableCopy(impStruktur)
            mExport = GenerateANewDataTableCopy(expStruktur)
            'vll für spätere überprüfung der Strukturen falls sie beim BapiAufruf modifiziert werden
            '---------------------------------------------------------------
            '  mOriginalExportStruktur = GenerateANewDataTableCopy(expStruktur) 
            '  mOriginalImportStruktur = GenerateANewDataTableCopy(impStruktur) 
            '---------------------------------------------------------------
        End Sub


        Protected Friend Sub settingCalledSource(ByRef mObjApp As CKG.Base.Kernel.Security.App, ByRef mObjUser As CKG.Base.Kernel.Security.User, ByRef mObjPage As Web.UI.Page)
            mObjMyApplication = mObjApp
            mObjMyPage = mObjPage
            mObjMyUser = mObjUser
        End Sub


        Private Function GenerateANewDataTableCopy(ByVal originalDataTable As DataTable) As DataTable
            '----------------------------------------------------------------------
            'Methode:       GenerateANewDataTableCopy
            'Autor:         Julian Jung
            'Beschreibung:  erstellt eine Kopie der Import/Export Struktur
            'Erstellt am:   09.12.2008
            '----------------------------------------------------------------------
            Dim newdataTable As New DataTable(originalDataTable.TableName)
            newdataTable = originalDataTable.Clone
            For Each tmpRow As DataRow In originalDataTable.Rows
                Dim newRow As DataRow = newdataTable.NewRow
                Dim i As Int32
                For i = 0 To tmpRow.ItemArray.Length - 1 Step 1
                    If TypeOf (tmpRow.Item(i)) Is DataTable Then
                        newRow.Item(i) = GenerateANewDataTableCopy(CType(tmpRow.Item(i), DataTable))
                    Else
                        newRow.Item(i) = tmpRow.Item(i)
                    End If
                Next
                newdataTable.Rows.Add(newRow)
                newdataTable.AcceptChanges()
            Next
            Return newdataTable
        End Function


        Public Function callBapi() As Boolean
            '----------------------------------------------------------------------
            'Methode:       callBapi
            'Autor:         Julian Jung
            'Beschreibung:  ruft das aktuelle BAPI im SAP auf
            'Erstellt am:   09.12.2008
            '----------------------------------------------------------------------
            Dim idSap As Int32 = -1

            Dim itemsCollect As New RFCTableCollection
            Dim items As RFCTable
            Dim item As RFCStructure
            Dim tempTable As DataTable
            Dim ErrMessage As String = ""

            Dim con As New R3Connection(mObjMyApplication.SAPAppServerHost, _
                                        mObjMyApplication.SAPSystemNumber, _
                                        mObjMyApplication.SAPUsername, _
                                        mObjMyApplication.SAPPassword, _
                                        "DE", mObjMyApplication.SAPClient.ToString)


            ERPConnect.LIC.SetLic(ConfigurationManager.AppSettings("ErpConnectLicense"))


            Try
                If mClearMe Then
                    clearAllValues()
                End If


                con.Open(False)


                'LogSAPCalls(con.RFCHandle, "Open", "")


                Dim func As ERPConnect.RFCFunction = con.CreateFunction(mBapiName)




                'import
                For Each impRow As DataRow In mImport.Rows
                    If impRow.Item(1).ToString = "PARA" Then
                        Dim paraTabelle As DataTable = CType(impRow.Item(0), DataTable)
                        For Each paraRow As DataRow In paraTabelle.Rows
                            If Not paraRow(1).ToString = "DATE" Then

                                If IsDBNull(paraRow(2)) = False Then
                                    func.Exports(paraRow(0).ToString).ParamValue = paraRow(2)
                                End If


                            Else

                                If IsDBNull(paraRow(2)) = False Then
                                    func.Exports(paraRow(0).ToString).ParamValue = ERPConnect.ConversionUtils.NetDate2SAPDate(CDate(paraRow(2)))
                                End If

                            End If

                        Next
                    End If

                    If impRow.Item(1).ToString = "TABLE" Then

                        items = New RFCTable


                        Dim rfcTable As New RFCTable
                        Dim booFound As Boolean = False

                        'Schauen, ob die Tabelle Bestandteil der SAP-Tabellen ist.
                        For Each rfcT As RFCTable In func.Tables

                            If rfcT.Name = CType(impRow(0), DataTable).TableName Then
                                booFound = True
                                Exit For
                            End If

                        Next

                        If booFound = True Then
                            items = func.Tables(CType(impRow(0), DataTable).TableName)

                            item = New RFCStructure(items.Columns)
                        Else

                            'Dim Header As New RFCStructure
                            item = func.Exports(CType(impRow(0), DataTable).TableName).ToStructure

                        End If

                        tempTable = CType(impRow(0), DataTable)

                        For Each dr As DataRow In tempTable.Rows

                            items.AddRow()

                            For Each col As DataColumn In tempTable.Columns

                                item(col.ColumnName) = dr(col.ColumnName)

                                If booFound = True Then
                                    items.Rows.Add(item)
                                End If

                            Next

                        Next

                        itemsCollect.Add(items)

                    End If
                Next



                'export

                If mObjLogApp Is Nothing Then
                    mObjLogApp = New Base.Kernel.Logging.Trace(mObjMyApplication.Connectionstring, mObjMyApplication.SaveLogAccessSAP, mObjMyApplication.LogLevel)
                End If
                idSap = mObjLogApp.WriteStartDataAccessSAP(mObjMyUser.UserName, mObjMyUser.IsTestUser, BapiName, mObjMyPage.Session("AppId").ToString, mObjMyPage.Session.SessionID, mObjMyUser.CurrentLogAccessASPXID)

                func.Execute()

                If idSap > -1 Then
                    mObjLogApp.WriteEndDataAccessSAP(idSap, True)
                End If

                'Export-Tabellen
                For Each tmpRow As DataRow In Export.Select("ElementCode='TABLE'")


                    For Each rfcT As RFCTable In func.Tables

                        If rfcT.Name = CType(tmpRow(0), DataTable).TableName Then

                            Dim tblTemp As DataTable = rfcT.ToADOTable

                            For Each Row As DataRow In tblTemp.Rows

                                For Each col As RFCTableColumn In rfcT.Columns

                                    Select Case col.Type

                                        Case RFCTYPE.NUM
                                            Row(col.Name) = CInt(Row(col.Name)).ToString

                                        Case RFCTYPE.DATE
                                            If Row(col.Name).ToString = "00000000" Then
                                                Row(col.Name) = ""
                                            End If

                                    End Select

                                Next

                            Next

                            tblTemp.AcceptChanges()

                            tmpRow(0) = tblTemp

                        End If

                    Next


                Next


                For Each tmpRow As DataRow In Export.Select("ElementCode='PARA'")


                    Dim paraTabelle As DataTable = CType(tmpRow.Item(0), DataTable)
                    For Each paraRow As DataRow In paraTabelle.Rows
                        If Not paraRow(1).ToString = "DATE" Then

                            For Each it As RFCParameter In func.Imports


                                If paraRow("Parameter").ToString = it.Name Then
                                    paraRow("ParameterValue") = it.ParamValue
                                End If


                            Next


                        Else

                            For Each it As RFCParameter In func.Imports


                                If paraRow("Parameter").ToString = it.Name Then
                                    paraRow("ParameterValue") = ERPConnect.ConversionUtils.SAPDate2NetDate(it.ParamValue.ToString)
                                End If


                            Next


                        End If

                    Next


                Next


                Return True
            Catch ex As ERPException

                ErrMessage = ex.Message

                If idSap > -1 Then
                    mObjLogApp.WriteEndDataAccessSAP(idSap, False)
                End If




                If ex.Source.Contains("ERPConnect35") Then
                    Throw New Exception(ex.ABAPException)
                    'Throw ex
                Else
                    Throw New Exception("Fehler in DynSapProxyObj.callBapi: " & ex.Message)
                End If
                Return False
            Finally
                If ErrMessage.Length > 249 Then
                    ErrMessage = Left(ErrMessage, 249)
                End If


                If Not con Is Nothing Then
                    'LogSAPCalls(con.RFCHandle, "Close", ErrMessage)
                    con.Close()
                    con.Dispose()
                End If
                mClearMe = True
            End Try
        End Function


        'Private Sub LogSAPCalls(ByVal HandleNumber As Integer, ByVal OpenClose As String, ByVal ErrMessage As String)

        '    Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        '    Dim cmd As New SqlClient.SqlCommand
        '    Try
        '        cn.Open()
        '        cmd.Connection = cn
        '        cmd.CommandType = CommandType.Text
        '        Dim SqlQuery As String

        '        SqlQuery = "INSERT INTO [LogRfcHandle] (HandleNumber, OpenClose, BapiName, WebUser, ErrMessage, EventTime) VALUES (@HandleNumber, @OpenClose, @BapiName, @WebUser, @ErrMessage, @EventTime);"
        '        With cmd
        '            .Parameters.AddWithValue("@HandleNumber", HandleNumber)
        '            .Parameters.AddWithValue("@OpenClose", OpenClose)
        '            .Parameters.AddWithValue("@BapiName", BapiName.ToUpper)
        '            .Parameters.AddWithValue("@WebUser", mObjMyUser.UserName)
        '            .Parameters.AddWithValue("@ErrMessage", ErrMessage)
        '            .Parameters.AddWithValue("@EventTime", Now)
        '        End With
        '        cmd.CommandText = SqlQuery
        '        cmd.ExecuteNonQuery()
        '    Catch ex As Exception
        '        Throw New Exception("SCHREIBEN EINES EINTRAGES IN DIE DB: TABELLENNAME=  LogRfcHandle  \ " & ex.Message & " \ " & ex.StackTrace)
        '    Finally
        '        cmd.Dispose()
        '        cn.Close()
        '        cn.Dispose()
        '    End Try




        'End Sub


        'Private Function fillExportParametersIntoTable() As Boolean
        '    '----------------------------------------------------------------------
        '    'Methode:       fillExportParametersIntoTable
        '    'Autor:         Julian Jung
        '    'Beschreibung:  befüllt die Exporttabellen mit den Rückgabewerten aus SAP
        '    'Erstellt am:   09.12.2008
        '    '----------------------------------------------------------------------
        '    For Each tmpSapParameter As SAPParameter In mbizTalkExportParameter
        '        If Not CType(Export.Select("ElementCode='PARA'")(0)(0), DataTable).Select("PARAMETER='" & tmpSapParameter.ParameterName.Replace("@pE_", "") & "'").Length = 0 Then
        '            If Not tmpSapParameter.Value Is DBNull.Value Then
        '                If CType(Export.Select("ElementCode='PARA'")(0)(0), DataTable).Select("PARAMETER='" & tmpSapParameter.ParameterName.Replace("@pE_", "") & "'")(0)(1).ToString() = "DATE" Then
        '                    CType(Export.Select("ElementCode='PARA'")(0)(0), DataTable).Select("PARAMETER='" & tmpSapParameter.ParameterName.Replace("@pE_", "") & "'")(0).Item("ParameterValue") = ConvertFromSapDate(tmpSapParameter.Value.ToString)
        '                Else
        '                    CType(Export.Select("ElementCode='PARA'")(0)(0), DataTable).Select("PARAMETER='" & tmpSapParameter.ParameterName.Replace("@pE_", "") & "'")(0).Item("ParameterValue") = tmpSapParameter.Value.ToString
        '                End If
        '            End If
        '        Else
        '            For Each tmpRow As DataRow In Export.Select("ElementCode='TABLE'")
        '                If CType(tmpRow(0), DataTable).TableName = tmpSapParameter.ParameterName.Replace("@pE_", "") Then
        '                    tmpRow(0) = ConformTableForWeb(DirectCast(tmpSapParameter.Value, DataTable))
        '                End If
        '            Next
        '        End If
        '    Next
        '    Export.AcceptChanges()
        '    Return True
        'End Function


        'Private Function generateCommandObject(ByRef cmd As SAPCommand, ByRef con As SAPConnection) As Boolean
        '    Try
        '        generateProxyElements()
        '        cmd.CommandText = commandtext
        '        cmd.Parameters.AddRange(mbizTalkExportParameter.ToArray)
        '        cmd.Parameters.AddRange(mbizTalkImportParameter.ToArray)
        '        con.ConnectionString = SapConnectionString
        '        cmd.Connection = con
        '        Return True
        '    Catch ex As Exception
        '        Throw New Exception("Fehler in DynSapProxyObj.generateCommandObject: " & ex.Message)
        '        Return False
        '    End Try
        'End Function

        Public Function getImportTable(ByVal Name As String) As DataTable
            '----------------------------------------------------------------------
            'Methode:       getImportTable
            'Autor:         Julian Jung
            'Beschreibung:  liefert eine benannte Importtabelle zurück
            'Erstellt am:   09.12.2008
            '----------------------------------------------------------------------
            Try
                If mClearMe Then
                    clearAllValues()
                End If
                For Each tmpRow As DataRow In Import.Select("ElementCode='TABLE'")
                    If CType(tmpRow(0), DataTable).TableName = Name Then
                        Return CType(tmpRow(0), DataTable)
                    End If
                Next
                Throw New Exception
            Catch ex As Exception
                Throw New Exception("Importtabelle mit dem Namen: " & Name & " nicht vorhanden!")
            End Try
        End Function

        Public Sub setImportParameter(ByVal Name As String, ByVal Wert As String)
            Try
                If mClearMe Then
                    clearAllValues()
                End If
                CType(Import.Select("ElementCode='PARA'")(0)(0), DataTable).Select("PARAMETER='" & Name & "'")(0)(2) = Wert
            Catch ex As Exception
                Throw New Exception("ImportParameter mit dem Namen: " & Name & " und dem Wert: " & Wert & " konnte nicht gesetzt werden!")
            End Try
        End Sub

        Public Function getExportParameter(ByVal Name As String) As String
            Try
                If Not CType(Export.Select("ElementCode='PARA'")(0)(0), DataTable).Select("PARAMETER='" & Name & "'")(0)(2) Is DBNull.Value Then
                    Return CType(Export.Select("ElementCode='PARA'")(0)(0), DataTable).Select("PARAMETER='" & Name & "'")(0)(2).ToString
                Else
                    Return ""
                End If
            Catch ex As Exception
                Throw New Exception("ExportParameter mit dem Namen: " & Name & " nicht vorhanden!")
            End Try
        End Function

        Public Function getExportTable(ByVal Name As String) As DataTable
            '----------------------------------------------------------------------
            'Methode:       getExportTable
            'Autor:         Julian Jung
            'Beschreibung:  liefert eine benannte Exporttabelle zurück
            'Erstellt am:   09.12.2008
            '----------------------------------------------------------------------
            Try
                For Each tmpRow As DataRow In Export.Select("ElementCode='TABLE'")
                    If Not tmpRow(0) Is DBNull.Value Then
                        If CType(tmpRow(0), DataTable).TableName = Name Then
                            Return CType(tmpRow(0), DataTable).Copy
                        End If
                    End If
                Next
                Throw New Exception
            Catch ex As Exception
                Throw New Exception("Exportabelle mit dem Namen: " & Name & " nicht vorhanden!")
            End Try
        End Function

        Private Function clearAllValues() As Boolean
            '----------------------------------------------------------------------
            'Methode:       clearAllValues
            'Autor:         Julian Jung
            'Beschreibung:  löscht alle Werte aus der Import/Export-Struktur
            'Erstellt am:   09.12.2008
            '----------------------------------------------------------------------
            Try
                For Each impRow As DataRow In mImport.Rows
                    If impRow.Item(1).ToString = "PARA" Then
                        Dim paraTabelle As DataTable = CType(impRow.Item(0), DataTable)
                        For Each paraRow As DataRow In paraTabelle.Rows
                            paraRow.Item(2) = ""
                        Next
                        paraTabelle.AcceptChanges()
                    End If
                    If impRow.Item(1).ToString = "TABLE" OrElse impRow.Item(1).ToString = "SAPTABLE" Then
                        CType(impRow.Item(0), DataTable).Rows.Clear()
                        CType(impRow.Item(0), DataTable).Clear()
                        CType(impRow.Item(0), DataTable).AcceptChanges()
                    End If
                Next
                For Each expRow As DataRow In Export.Rows
                    If expRow.Item(1).ToString = "PARA" Then
                        Dim paraTabelle As DataTable = CType(expRow.Item(0), DataTable)
                        For Each paraRow As DataRow In paraTabelle.Rows
                            paraRow.Item(2) = ""
                        Next
                        paraTabelle.AcceptChanges()
                    End If

                    If expRow.Item(1).ToString = "TABLE" Then
                        CType(expRow.Item(0), DataTable).Rows.Clear()
                        CType(expRow.Item(0), DataTable).Clear()
                        CType(expRow.Item(0), DataTable).AcceptChanges()
                    End If
                Next
                'mbizTalkExportParameter.Clear()
                'mbizTalkImportParameter.Clear()
                mClearMe = False
                Return True
            Catch ex As Exception
                Throw New Exception("Fehler in DynSapProxyObj.clearAllValues: " & ex.Message)
                Return False
            End Try
        End Function


        'Private Function generateProxyElements() As Boolean
        '    '----------------------------------------------------------------------
        '    'Methode:       generateProxyElements
        '    'Autor:         Julian Jung
        '    'Beschreibung:  generiert das SQL-Statment und die Objekte für einen BAPI-Aufruf
        '    'Erstellt am:   09.12.2008
        '    '----------------------------------------------------------------------
        '    Dim strBuilder As New StringBuilder("EXEC ")
        '    Dim space As String = " "
        '    Try
        '        With strBuilder
        '            .Append(mBapiName)
        '            .Append(space)
        '            'import
        '            For Each impRow As DataRow In mImport.Rows
        '                If impRow.Item(1).ToString = "PARA" Then
        '                    Dim paraTabelle As DataTable = CType(impRow.Item(0), DataTable)
        '                    For Each paraRow As DataRow In paraTabelle.Rows
        '                        If Not paraRow(1).ToString = "DATE" Then
        '                            addSapImportParameter(paraRow(0).ToString, paraRow(2))
        '                        Else
        '                            addSapImportParameter(paraRow(0).ToString, ConvertToSapDate(paraRow(2)))
        '                        End If
        '                        .Append("@" & paraRow(0).ToString & "=@pI_" & paraRow(0).ToString & ",")
        '                    Next
        '                End If

        '                If impRow.Item(1).ToString = "TABLE" Then
        '                    addSapImportParameter(CType(impRow(0), DataTable).TableName, ConformTableForSAP(CType(impRow(0), DataTable)), CType(impRow(0), DataTable))
        '                    .Append("@" & CType(impRow(0), DataTable).TableName & "=@pI_" & CType(impRow(0), DataTable).TableName & ",")
        '                End If
        '            Next
        '            'export
        '            For Each expRow As DataRow In Export.Rows
        '                If expRow.Item(1).ToString = "PARA" Then
        '                    Dim paraTabelle As DataTable = CType(expRow.Item(0), DataTable)
        '                    For Each paraRow As DataRow In paraTabelle.Rows
        '                        addSapexportParameter(paraRow(0).ToString, paraRow(2))
        '                        .Append("@" & paraRow(0).ToString & "=@pE_" & paraRow(0).ToString & " OUTPUT ,")
        '                    Next
        '                End If
        '                If expRow.Item(1).ToString = "TABLE" Then
        '                    addSapexportParameter(CType(expRow(0), DataTable).TableName, CType(expRow(0), DataTable), CType(expRow(0), DataTable))
        '                    .Append("@" & CType(expRow(0), DataTable).TableName & "=@pE_" & CType(expRow(0), DataTable).TableName & " OUTPUT ,")
        '                End If
        '            Next
        '            .Remove(.Length - 1, 1)
        '            .Append(space)
        '            .Append("OPTION 'disabledatavalidation'")
        '        End With
        '        mCommandText = strBuilder.ToString
        '        Return True
        '    Catch ex As Exception
        '        Throw New Exception("Fehler in DynSapProxyObj.getCommandText(): " & ex.Message)
        '        Return False
        '    End Try
        'End Function

        Private Function ConformTableForSAP(ByVal webTable As DataTable) As DataTable
            '----------------------------------------------------------------------
            'Methode:       ConformTableForSAP
            'Autor:         Julian Jung
            'Beschreibung:  macht eine Tabelle SAP-Conform
            'Erstellt am:   09.12.2008
            '----------------------------------------------------------------------
            Dim SAPTable As DataTable
            Dim dateArray() As Int32 = {-1}
            For Each tmpRow As DataRow In Import.Select("ElementCode='SAPTABLE'")
                If Not tmpRow(0) Is Nothing Then
                    SAPTable = CType(tmpRow(0), DataTable)
                    If SAPTable.TableName = webTable.TableName Then
                        For Each tmpColumn As DataColumn In webTable.Columns
                            If tmpColumn.DataType Is System.Type.GetType("System.DateTime") Then
                                ReDim Preserve dateArray(dateArray.Length)
                                dateArray(dateArray.Length - 1) = webTable.Columns.IndexOf(tmpColumn)
                            End If
                        Next
                        If Not dateArray.Length = 1 Then
                            Dim itemCounter As Int32 = 0
                            Dim newRow As DataRow
                            Dim maxcount As Int32 = 0
                            'sicherheit für die erweiterung von saptabellen
                            If webTable.Columns.Count > SAPTable.Columns.Count Then
                                maxcount = SAPTable.Columns.Count
                            Else
                                maxcount = webTable.Columns.Count
                            End If
                            For Each tmpRow2 As DataRow In webTable.Rows
                                newRow = SAPTable.NewRow
                                While itemCounter < maxcount
                                    If dateArray.Contains(itemCounter) Then
                                        newRow.Item(itemCounter) = ConvertToSapDate(tmpRow2.Item(itemCounter).ToString)
                                    Else
                                        newRow.Item(itemCounter) = tmpRow2.Item(itemCounter).ToString
                                    End If
                                    itemCounter += 1
                                End While
                                SAPTable.Rows.Add(newRow)
                                SAPTable.AcceptChanges()
                                itemCounter = 0
                            Next
                            Return SAPTable
                        Else
                            Return webTable
                        End If
                    End If
                End If
            Next
            Return webTable
        End Function


        Private Function ConformTableForWeb(ByVal SapTable As DataTable) As DataTable
            '----------------------------------------------------------------------
            'Methode:       ConformTableForWeb
            'Autor:         Julian Jung
            'Beschreibung:  macht eine Tabelle WEB-Conform
            'Erstellt am:   09.12.2008
            '----------------------------------------------------------------------
            Dim WebTable As DataTable
            Dim dateArray() As Int32 = {-1}
            HelpProcedures.killAllDBNullValuesInDataTable(SapTable)
            For Each tmpRow As DataRow In Export.Select("ElementCode='Table'")
                If Not tmpRow(0) Is Nothing Then
                    WebTable = CType(tmpRow(0), DataTable)
                    If SapTable.TableName = WebTable.TableName Then
                        For Each tmpColumn As DataColumn In WebTable.Columns
                            If tmpColumn.DataType Is System.Type.GetType("System.DateTime") Then
                                ReDim Preserve dateArray(dateArray.Length)
                                dateArray(dateArray.Length - 1) = WebTable.Columns.IndexOf(tmpColumn)
                            End If
                        Next
                        If Not dateArray.Length = 1 Then
                            Dim itemCounter As Int32 = 0
                            Dim newRow As DataRow
                            'sicherheit für die erweiterung von saptabellen
                            Dim maxcount As Int32 = 0
                            If WebTable.Columns.Count > SapTable.Columns.Count Then
                                maxcount = SapTable.Columns.Count
                            Else
                                maxcount = WebTable.Columns.Count
                            End If
                            For Each tmpRow2 As DataRow In SapTable.Rows
                                newRow = WebTable.NewRow
                                While itemCounter < maxcount

                                    'wenn ein Feld in einer SAP tabelle hinzukommt, 
                                    'entsteht hier ein fehler, da die Webtabelle noch nciht auf dem Stand ist wie das BAPi die tabelle zurückliefert
                                    'nämlich mit ein paar Feldern mehr
                                    If dateArray.Contains(itemCounter) Then
                                        newRow.Item(itemCounter) = IIf(ConvertFromSapDate(tmpRow2.Item(itemCounter).ToString) = Nothing, DBNull.Value, ConvertFromSapDate(tmpRow2.Item(itemCounter).ToString))
                                    Else
                                        newRow.Item(itemCounter) = tmpRow2.Item(itemCounter).ToString
                                    End If
                                    itemCounter += 1
                                End While
                                WebTable.Rows.Add(newRow)
                                WebTable.AcceptChanges()
                                itemCounter = 0
                            Next
                        Else
                            Return SapTable 'es existier eine Tabelle die keine Datewerte Enthält aber trotzdem befüllt werden muss
                        End If
                        Return WebTable
                    End If
                End If
            Next
            Return SapTable
        End Function


        'Private Sub addSapImportParameter(ByVal originalParameterName As String, ByVal myvalue As Object, Optional ByVal tabelle As DataTable = Nothing)
        '    If tabelle Is Nothing Then
        '        mbizTalkImportParameter.Add(New SAPParameter("@pI_" & originalParameterName, ParameterDirection.Input))
        '    Else
        '        mbizTalkImportParameter.Add(New SAPParameter("@pI_" & originalParameterName, tabelle))
        '    End If
        '    mbizTalkImportParameter.Last.Value = myvalue
        'End Sub


        'Private Sub addSapexportParameter(ByVal originalParameterName As String, ByVal myvalue As Object, Optional ByVal tabelle As DataTable = Nothing)
        '    If tabelle Is Nothing Then
        '        mbizTalkExportParameter.Add(New SAPParameter("@pE_" & originalParameterName, ParameterDirection.Output))
        '    Else
        '        '----------------------------------------------
        '        'JJU20090305 
        '        'Ich glaube hier liegt ein denkfehler vor, eine Input/Output Tabelle
        '        'hatte bisher gar keine Pramaterdirection, kann nicht sein, d.h. 
        '        'jede tabelle wird gleich behandelt bei den output parametern. 
        '        '----------------------------------------------

        '        'Dim isInPutOutPutTable As Boolean = False
        '        'For Each tmpRow As DataRow In Import.Select("ElementCode='TABLE'")
        '        '    If Not tmpRow(0) Is DBNull.Value Then
        '        '        If CType(tmpRow(0), DataTable).TableName = originalParameterName Then
        '        '            isInPutOutPutTable = True
        '        '            Exit For
        '        '        End If
        '        '    End If
        '        'Next
        '        ' If Not isInPutOutPutTable Then
        '        mbizTalkExportParameter.Add(New SAPParameter("@pE_" & originalParameterName, ParameterDirection.Output))
        '        ' Else
        '        ' mbizTalkExportParameter.Add(New SAPParameter("@pE_" & originalParameterName, tabelle))
        '        'End If
        '    End If
        'End Sub


        Private Function ConvertToSapDate(ByVal datInput As Object) As String
            If datInput Is Nothing OrElse datInput Is DBNull.Value OrElse datInput.ToString = New Date().ToString OrElse datInput.ToString = "" OrElse datInput.ToString = New Date().ToShortDateString Then
                Return "00000000"
            ElseIf IsDate(datInput) Then
                Dim myDate As Date
                myDate = CDate(datInput)
                Return Year(myDate) & Right("0" & Month(myDate), 2) & Right("0" & Day(myDate), 2)
            Else
                Return ""
            End If
        End Function


        Private Function ConvertFromSapDate(ByVal datInput As String) As Date
            If Not datInput = "" AndAlso Not datInput.Length < 8 AndAlso Not datInput = "00000000" Then
                Dim strTemp As String = Right(datInput, 2) & "." & Mid(datInput, 5, 2) & "." & Left(datInput, 4)
                If IsDate(strTemp) Then
                    Return CDate(strTemp)
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        End Function
#End Region

    End Class
End Namespace
' ************************************************
' $History: DynSapProxyObj.vb $
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 6.08.10    Time: 13:13
' Updated in $/CKAG/Base/Save
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 14.05.10   Time: 17:22
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 12.05.10   Time: 18:21
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 9.07.09    Time: 17:17
' Updated in $/CKAG/Base/Kernel/Common
' nachbesserung im dateconverter
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 18.06.09   Time: 8:34
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 28.04.09   Time: 16:36
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 6.03.09    Time: 8:40
' Updated in $/CKAG/Base/Kernel/Common
' SapTabellen erweiterung ist nun egal, auer imports nat�rlich
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 5.03.09    Time: 17:26
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 5.03.09    Time: 17:22
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 5.03.09    Time: 10:18
' Updated in $/CKAG/Base/Kernel/Common
' Bugfix bei Input/Output table, falsche direction 
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 3.02.09    Time: 14:29
' Updated in $/CKAG/Base/Kernel/Common
' Dyn Proxy Bugfix
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 26.01.09   Time: 10:00
' Updated in $/CKAG/Base/Kernel/Common
' dynProxy Bugfix 
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 16.12.08   Time: 11:49
' Updated in $/CKAG/Base/Kernel/Common
' dp anpassung
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 16.12.08   Time: 10:20
' Updated in $/CKAG/Base/Kernel/Common
' DynProxy Anpassungen
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 15.12.08   Time: 18:00
' Updated in $/CKAG/Base/Kernel/Common
' DynProxy Anpassungen
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 15.12.08   Time: 17:37
' Created in $/CKAG/Base/Kernel/Common
' Dyn Proxy integriert
' 
' ************************************************