﻿'------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'     Runtime Version: 1.0.3705.0
'
'     Changes to this file may cause incorrect behavior and will be lost if 
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Runtime.Serialization
Imports System.Xml

Namespace DataSets
    
    <Serializable(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Diagnostics.DebuggerStepThrough(),  _
     System.ComponentModel.ToolboxItem(true)>  _
    Public Class AddressDataSet
        Inherits DataSet
        
        Private tableADDRESSE As ADDRESSEDataTable
        
        Public Sub New()
            MyBase.New
            Me.InitClass
            Dim schemaChangedHandler As System.ComponentModel.CollectionChangeEventHandler = AddressOf Me.SchemaChanged
            AddHandler Me.Tables.CollectionChanged, schemaChangedHandler
            AddHandler Me.Relations.CollectionChanged, schemaChangedHandler
        End Sub
        
        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.New
            Dim strSchema As String = CType(info.GetValue("XmlSchema", GetType(System.String)),String)
            If (Not (strSchema) Is Nothing) Then
                Dim ds As DataSet = New DataSet
                ds.ReadXmlSchema(New XmlTextReader(New System.IO.StringReader(strSchema)))
                If (Not (ds.Tables("ADDRESSE")) Is Nothing) Then
                    Me.Tables.Add(New ADDRESSEDataTable(ds.Tables("ADDRESSE")))
                End If
                Me.DataSetName = ds.DataSetName
                Me.Prefix = ds.Prefix
                Me.Namespace = ds.Namespace
                Me.Locale = ds.Locale
                Me.CaseSensitive = ds.CaseSensitive
                Me.EnforceConstraints = ds.EnforceConstraints
                Me.Merge(ds, false, System.Data.MissingSchemaAction.Add)
                Me.InitVars
            Else
                Me.InitClass
            End If
            Me.GetSerializationData(info, context)
            Dim schemaChangedHandler As System.ComponentModel.CollectionChangeEventHandler = AddressOf Me.SchemaChanged
            AddHandler Me.Tables.CollectionChanged, schemaChangedHandler
            AddHandler Me.Relations.CollectionChanged, schemaChangedHandler
        End Sub
        
        <System.ComponentModel.Browsable(false),  _
         System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)>  _
        Public ReadOnly Property ADDRESSE As ADDRESSEDataTable
            Get
                Return Me.tableADDRESSE
            End Get
        End Property
        
        Public Overrides Function Clone() As DataSet
            Dim cln As AddressDataSet = CType(MyBase.Clone,AddressDataSet)
            cln.InitVars
            Return cln
        End Function
        
        Protected Overrides Function ShouldSerializeTables() As Boolean
            Return false
        End Function
        
        Protected Overrides Function ShouldSerializeRelations() As Boolean
            Return false
        End Function
        
        Protected Overrides Sub ReadXmlSerializable(ByVal reader As XmlReader)
            Me.Reset
            Dim ds As DataSet = New DataSet
            ds.ReadXml(reader)
            If (Not (ds.Tables("ADDRESSE")) Is Nothing) Then
                Me.Tables.Add(New ADDRESSEDataTable(ds.Tables("ADDRESSE")))
            End If
            Me.DataSetName = ds.DataSetName
            Me.Prefix = ds.Prefix
            Me.Namespace = ds.Namespace
            Me.Locale = ds.Locale
            Me.CaseSensitive = ds.CaseSensitive
            Me.EnforceConstraints = ds.EnforceConstraints
            Me.Merge(ds, false, System.Data.MissingSchemaAction.Add)
            Me.InitVars
        End Sub
        
        Protected Overrides Function GetSchemaSerializable() As System.Xml.Schema.XmlSchema
            Dim stream As System.IO.MemoryStream = New System.IO.MemoryStream
            Me.WriteXmlSchema(New XmlTextWriter(stream, Nothing))
            stream.Position = 0
            Return System.Xml.Schema.XmlSchema.Read(New XmlTextReader(stream), Nothing)
        End Function
        
        Friend Sub InitVars()
            Me.tableADDRESSE = CType(Me.Tables("ADDRESSE"),ADDRESSEDataTable)
            If (Not (Me.tableADDRESSE) Is Nothing) Then
                Me.tableADDRESSE.InitVars
            End If
        End Sub
        
        Private Sub InitClass()
            Me.DataSetName = "AddressDataSet"
            Me.Prefix = ""
            Me.Namespace = "http://tempuri.org/AddressDataSet.xsd"
            Me.Locale = New System.Globalization.CultureInfo("en-US")
            Me.CaseSensitive = false
            Me.EnforceConstraints = true
            Me.tableADDRESSE = New ADDRESSEDataTable
            Me.Tables.Add(Me.tableADDRESSE)
        End Sub
        
        Private Function ShouldSerializeADDRESSE() As Boolean
            Return false
        End Function
        
        Private Sub SchemaChanged(ByVal sender As Object, ByVal e As System.ComponentModel.CollectionChangeEventArgs)
            If (e.Action = System.ComponentModel.CollectionChangeAction.Remove) Then
                Me.InitVars
            End If
        End Sub
        
        Public Delegate Sub ADDRESSERowChangeEventHandler(ByVal sender As Object, ByVal e As ADDRESSERowChangeEvent)
        
        <System.Diagnostics.DebuggerStepThrough()>  _
        Public Class ADDRESSEDataTable
            Inherits DataTable
            Implements System.Collections.IEnumerable
            
            Private columnNAME As DataColumn
            
            Private columnPLZ As DataColumn
            
            Private columnORT As DataColumn
            
            Private columnSTRASSE As DataColumn
            
            Private columnID As DataColumn
            
            Private columnHAUSNUMMER As DataColumn
            
            Private columnTELEFON1 As DataColumn
            
            Private columnTELEFON2 As DataColumn
            
            Private columnNAME2 As DataColumn
            
            Private columnTYP As DataColumn
            
            Private columnKUNDENNUMMER As DataColumn

            Private columnFAX As DataColumn
            
            Friend Sub New()
                MyBase.New("ADDRESSE")
                Me.InitClass
            End Sub
            
            Friend Sub New(ByVal table As DataTable)
                MyBase.New(table.TableName)
                If (table.CaseSensitive <> table.DataSet.CaseSensitive) Then
                    Me.CaseSensitive = table.CaseSensitive
                End If
                If (table.Locale.ToString <> table.DataSet.Locale.ToString) Then
                    Me.Locale = table.Locale
                End If
                If (table.Namespace <> table.DataSet.Namespace) Then
                    Me.Namespace = table.Namespace
                End If
                Me.Prefix = table.Prefix
                Me.MinimumCapacity = table.MinimumCapacity
                Me.DisplayExpression = table.DisplayExpression
            End Sub
            
            <System.ComponentModel.Browsable(false)>  _
            Public ReadOnly Property Count As Integer
                Get
                    Return Me.Rows.Count
                End Get
            End Property
            
            Friend ReadOnly Property NAMEColumn As DataColumn
                Get
                    Return Me.columnNAME
                End Get
            End Property
            
            Friend ReadOnly Property PLZColumn As DataColumn
                Get
                    Return Me.columnPLZ
                End Get
            End Property
            
            Friend ReadOnly Property ORTColumn As DataColumn
                Get
                    Return Me.columnORT
                End Get
            End Property
            
            Friend ReadOnly Property STRASSEColumn As DataColumn
                Get
                    Return Me.columnSTRASSE
                End Get
            End Property
            
            Friend ReadOnly Property IDColumn As DataColumn
                Get
                    Return Me.columnID
                End Get
            End Property
            
            Friend ReadOnly Property HAUSNUMMERColumn As DataColumn
                Get
                    Return Me.columnHAUSNUMMER
                End Get
            End Property
            
            Friend ReadOnly Property TELEFON1Column As DataColumn
                Get
                    Return Me.columnTELEFON1
                End Get
            End Property
            
            Friend ReadOnly Property TELEFON2Column As DataColumn
                Get
                    Return Me.columnTELEFON2
                End Get
            End Property
            
            Friend ReadOnly Property NAME2Column As DataColumn
                Get
                    Return Me.columnNAME2
                End Get
            End Property
            
            Friend ReadOnly Property TYPColumn As DataColumn
                Get
                    Return Me.columnTYP
                End Get
            End Property
            
            Friend ReadOnly Property KUNDENNUMMERColumn As DataColumn
                Get
                    Return Me.columnKUNDENNUMMER
                End Get
            End Property

            Friend ReadOnly Property FAXColumn() As DataColumn
                Get
                    Return Me.columnFAX
                End Get
            End Property

            Default Public ReadOnly Property Item(ByVal index As Integer) As ADDRESSERow
                Get
                    Return CType(Me.Rows(index), ADDRESSERow)
                End Get
            End Property

            Public Event ADDRESSERowChanged As ADDRESSERowChangeEventHandler

            Public Event ADDRESSERowChanging As ADDRESSERowChangeEventHandler

            Public Event ADDRESSERowDeleted As ADDRESSERowChangeEventHandler

            Public Event ADDRESSERowDeleting As ADDRESSERowChangeEventHandler

            Public Overloads Sub AddADDRESSERow(ByVal row As ADDRESSERow)
                Me.Rows.Add(row)
            End Sub

            Public Overloads Function AddADDRESSERow(ByVal NAME As String, ByVal PLZ As String, ByVal ORT As String, ByVal STRASSE As String, ByVal ID As String, ByVal HAUSNUMMER As String, ByVal TELEFON1 As String, ByVal TELEFON2 As String, ByVal NAME2 As String, ByVal TYP As String, ByVal KUNDENNUMMER As String, ByVal FAX As String) As ADDRESSERow
                Dim rowADDRESSERow As ADDRESSERow = CType(Me.NewRow, ADDRESSERow)
                rowADDRESSERow.ItemArray = New Object() {NAME, PLZ, ORT, STRASSE, ID, HAUSNUMMER, TELEFON1, TELEFON2, NAME2, TYP, KUNDENNUMMER, FAX}
                Me.Rows.Add(rowADDRESSERow)
                Return rowADDRESSERow
            End Function

            Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
                Return Me.Rows.GetEnumerator
            End Function

            Public Overrides Function Clone() As DataTable
                Dim cln As ADDRESSEDataTable = CType(MyBase.Clone, ADDRESSEDataTable)
                cln.InitVars()
                Return cln
            End Function

            Protected Overrides Function CreateInstance() As DataTable
                Return New ADDRESSEDataTable()
            End Function

            Friend Sub InitVars()
                Me.columnNAME = Me.Columns("NAME")
                Me.columnPLZ = Me.Columns("PLZ")
                Me.columnORT = Me.Columns("ORT")
                Me.columnSTRASSE = Me.Columns("STRASSE")
                Me.columnID = Me.Columns("ID")
                Me.columnHAUSNUMMER = Me.Columns("HAUSNUMMER")
                Me.columnTELEFON1 = Me.Columns("TELEFON1")
                Me.columnTELEFON2 = Me.Columns("TELEFON2")
                Me.columnNAME2 = Me.Columns("NAME2")
                Me.columnTYP = Me.Columns("TYP")
                Me.columnKUNDENNUMMER = Me.Columns("KUNDENNUMMER")
                Me.columnFAX = Me.Columns("TELFX")
            End Sub

            Private Sub InitClass()
                Me.columnNAME = New DataColumn("NAME", GetType(System.String), Nothing, System.Data.MappingType.Element)
                Me.Columns.Add(Me.columnNAME)
                Me.columnPLZ = New DataColumn("PLZ", GetType(System.String), Nothing, System.Data.MappingType.Element)
                Me.Columns.Add(Me.columnPLZ)
                Me.columnORT = New DataColumn("ORT", GetType(System.String), Nothing, System.Data.MappingType.Element)
                Me.Columns.Add(Me.columnORT)
                Me.columnSTRASSE = New DataColumn("STRASSE", GetType(System.String), Nothing, System.Data.MappingType.Element)
                Me.Columns.Add(Me.columnSTRASSE)
                Me.columnID = New DataColumn("ID", GetType(System.String), Nothing, System.Data.MappingType.Element)
                Me.Columns.Add(Me.columnID)
                Me.columnHAUSNUMMER = New DataColumn("HAUSNUMMER", GetType(System.String), Nothing, System.Data.MappingType.Element)
                Me.Columns.Add(Me.columnHAUSNUMMER)
                Me.columnTELEFON1 = New DataColumn("TELEFON1", GetType(System.String), Nothing, System.Data.MappingType.Element)
                Me.Columns.Add(Me.columnTELEFON1)
                Me.columnTELEFON2 = New DataColumn("TELEFON2", GetType(System.String), Nothing, System.Data.MappingType.Element)
                Me.Columns.Add(Me.columnTELEFON2)
                Me.columnNAME2 = New DataColumn("NAME2", GetType(System.String), Nothing, System.Data.MappingType.Element)
                Me.Columns.Add(Me.columnNAME2)
                Me.columnTYP = New DataColumn("TYP", GetType(System.String), Nothing, System.Data.MappingType.Element)
                Me.Columns.Add(Me.columnTYP)
                Me.columnKUNDENNUMMER = New DataColumn("KUNDENNUMMER", GetType(System.String), Nothing, System.Data.MappingType.Element)
                Me.Columns.Add(Me.columnKUNDENNUMMER)
                Me.columnFAX = New DataColumn("TELFX", GetType(System.String), Nothing, System.Data.MappingType.Element)
                Me.Columns.Add(Me.columnFAX)
            End Sub

            Public Function NewADDRESSERow() As ADDRESSERow
                Return CType(Me.NewRow, ADDRESSERow)
            End Function

            Protected Overrides Function NewRowFromBuilder(ByVal builder As DataRowBuilder) As DataRow
                Return New ADDRESSERow(builder)
            End Function

            Protected Overrides Function GetRowType() As System.Type
                Return GetType(ADDRESSERow)
            End Function

            Protected Overrides Sub OnRowChanged(ByVal e As DataRowChangeEventArgs)
                MyBase.OnRowChanged(e)
                If (Not (Me.ADDRESSERowChangedEvent) Is Nothing) Then
                    RaiseEvent ADDRESSERowChanged(Me, New ADDRESSERowChangeEvent(CType(e.Row, ADDRESSERow), e.Action))
                End If
            End Sub

            Protected Overrides Sub OnRowChanging(ByVal e As DataRowChangeEventArgs)
                MyBase.OnRowChanging(e)
                If (Not (Me.ADDRESSERowChangingEvent) Is Nothing) Then
                    RaiseEvent ADDRESSERowChanging(Me, New ADDRESSERowChangeEvent(CType(e.Row, ADDRESSERow), e.Action))
                End If
            End Sub

            Protected Overrides Sub OnRowDeleted(ByVal e As DataRowChangeEventArgs)
                MyBase.OnRowDeleted(e)
                If (Not (Me.ADDRESSERowDeletedEvent) Is Nothing) Then
                    RaiseEvent ADDRESSERowDeleted(Me, New ADDRESSERowChangeEvent(CType(e.Row, ADDRESSERow), e.Action))
                End If
            End Sub

            Protected Overrides Sub OnRowDeleting(ByVal e As DataRowChangeEventArgs)
                MyBase.OnRowDeleting(e)
                If (Not (Me.ADDRESSERowDeletingEvent) Is Nothing) Then
                    RaiseEvent ADDRESSERowDeleting(Me, New ADDRESSERowChangeEvent(CType(e.Row, ADDRESSERow), e.Action))
                End If
            End Sub

            Public Sub RemoveADDRESSERow(ByVal row As ADDRESSERow)
                Me.Rows.Remove(row)
            End Sub
        End Class
        
        <System.Diagnostics.DebuggerStepThrough()>  _
        Public Class ADDRESSERow
            Inherits DataRow
            
            Private tableADDRESSE As ADDRESSEDataTable
            
            Friend Sub New(ByVal rb As DataRowBuilder)
                MyBase.New(rb)
                Me.tableADDRESSE = CType(Me.Table,ADDRESSEDataTable)
            End Sub
            
            Public Property NAME As String
                Get
                    Try 
                        Return CType(Me(Me.tableADDRESSE.NAMEColumn),String)
                    Catch e As InvalidCastException
                        Throw New StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e)
                    End Try
                End Get
                Set
                    Me(Me.tableADDRESSE.NAMEColumn) = value
                End Set
            End Property
            
            Public Property PLZ As String
                Get
                    Try 
                        Return CType(Me(Me.tableADDRESSE.PLZColumn),String)
                    Catch e As InvalidCastException
                        Throw New StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e)
                    End Try
                End Get
                Set
                    Me(Me.tableADDRESSE.PLZColumn) = value
                End Set
            End Property
            
            Public Property ORT As String
                Get
                    Try 
                        Return CType(Me(Me.tableADDRESSE.ORTColumn),String)
                    Catch e As InvalidCastException
                        Throw New StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e)
                    End Try
                End Get
                Set
                    Me(Me.tableADDRESSE.ORTColumn) = value
                End Set
            End Property
            
            Public Property STRASSE As String
                Get
                    Try 
                        Return CType(Me(Me.tableADDRESSE.STRASSEColumn),String)
                    Catch e As InvalidCastException
                        Throw New StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e)
                    End Try
                End Get
                Set
                    Me(Me.tableADDRESSE.STRASSEColumn) = value
                End Set
            End Property
            
            Public Property ID As String
                Get
                    Try 
                        Return CType(Me(Me.tableADDRESSE.IDColumn),String)
                    Catch e As InvalidCastException
                        Throw New StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e)
                    End Try
                End Get
                Set
                    Me(Me.tableADDRESSE.IDColumn) = value
                End Set
            End Property
            
            Public Property HAUSNUMMER As String
                Get
                    Try 
                        Return CType(Me(Me.tableADDRESSE.HAUSNUMMERColumn),String)
                    Catch e As InvalidCastException
                        Throw New StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e)
                    End Try
                End Get
                Set
                    Me(Me.tableADDRESSE.HAUSNUMMERColumn) = value
                End Set
            End Property
            
            Public Property TELEFON1 As String
                Get
                    Try 
                        Return CType(Me(Me.tableADDRESSE.TELEFON1Column),String)
                    Catch e As InvalidCastException
                        Throw New StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e)
                    End Try
                End Get
                Set
                    Me(Me.tableADDRESSE.TELEFON1Column) = value
                End Set
            End Property
            
            Public Property TELEFON2 As String
                Get
                    Try 
                        Return CType(Me(Me.tableADDRESSE.TELEFON2Column),String)
                    Catch e As InvalidCastException
                        Throw New StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e)
                    End Try
                End Get
                Set
                    Me(Me.tableADDRESSE.TELEFON2Column) = value
                End Set
            End Property
            
            Public Property NAME2 As String
                Get
                    Try 
                        Return CType(Me(Me.tableADDRESSE.NAME2Column),String)
                    Catch e As InvalidCastException
                        Throw New StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e)
                    End Try
                End Get
                Set
                    Me(Me.tableADDRESSE.NAME2Column) = value
                End Set
            End Property
            
            Public Property TYP As String
                Get
                    Try 
                        Return CType(Me(Me.tableADDRESSE.TYPColumn),String)
                    Catch e As InvalidCastException
                        Throw New StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e)
                    End Try
                End Get
                Set
                    Me(Me.tableADDRESSE.TYPColumn) = value
                End Set
            End Property
            
            Public Property KUNDENNUMMER As String
                Get
                    Try 
                        Return CType(Me(Me.tableADDRESSE.KUNDENNUMMERColumn),String)
                    Catch e As InvalidCastException
                        Throw New StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e)
                    End Try
                End Get
                Set
                    Me(Me.tableADDRESSE.KUNDENNUMMERColumn) = value
                End Set
            End Property

            Public Property FAX() As String
                Get
                    Try
                        Return CType(Me(Me.tableADDRESSE.FAXColumn), String)
                    Catch e As InvalidCastException
                        Throw New StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e)
                    End Try
                End Get
                Set(ByVal Value As String)
                    Me(Me.tableADDRESSE.FAXColumn) = Value
                End Set
            End Property


            Public Function IsNAMENull() As Boolean
                Return Me.IsNull(Me.tableADDRESSE.NAMEColumn)
            End Function

            Public Sub SetNAMENull()
                Me(Me.tableADDRESSE.NAMEColumn) = System.Convert.DBNull
            End Sub

            Public Function IsPLZNull() As Boolean
                Return Me.IsNull(Me.tableADDRESSE.PLZColumn)
            End Function

            Public Sub SetPLZNull()
                Me(Me.tableADDRESSE.PLZColumn) = System.Convert.DBNull
            End Sub

            Public Function IsORTNull() As Boolean
                Return Me.IsNull(Me.tableADDRESSE.ORTColumn)
            End Function

            Public Sub SetORTNull()
                Me(Me.tableADDRESSE.ORTColumn) = System.Convert.DBNull
            End Sub

            Public Function IsSTRASSENull() As Boolean
                Return Me.IsNull(Me.tableADDRESSE.STRASSEColumn)
            End Function

            Public Sub SetSTRASSENull()
                Me(Me.tableADDRESSE.STRASSEColumn) = System.Convert.DBNull
            End Sub

            Public Function IsIDNull() As Boolean
                Return Me.IsNull(Me.tableADDRESSE.IDColumn)
            End Function

            Public Sub SetIDNull()
                Me(Me.tableADDRESSE.IDColumn) = System.Convert.DBNull
            End Sub

            Public Function IsHAUSNUMMERNull() As Boolean
                Return Me.IsNull(Me.tableADDRESSE.HAUSNUMMERColumn)
            End Function

            Public Sub SetHAUSNUMMERNull()
                Me(Me.tableADDRESSE.HAUSNUMMERColumn) = System.Convert.DBNull
            End Sub

            Public Function IsTELEFON1Null() As Boolean
                Return Me.IsNull(Me.tableADDRESSE.TELEFON1Column)
            End Function

            Public Sub SetTELEFON1Null()
                Me(Me.tableADDRESSE.TELEFON1Column) = System.Convert.DBNull
            End Sub

            Public Function IsTELEFON2Null() As Boolean
                Return Me.IsNull(Me.tableADDRESSE.TELEFON2Column)
            End Function

            Public Sub SetTELEFON2Null()
                Me(Me.tableADDRESSE.TELEFON2Column) = System.Convert.DBNull
            End Sub

            Public Function IsNAME2Null() As Boolean
                Return Me.IsNull(Me.tableADDRESSE.NAME2Column)
            End Function

            Public Sub SetNAME2Null()
                Me(Me.tableADDRESSE.NAME2Column) = System.Convert.DBNull
            End Sub

            Public Function IsTYPNull() As Boolean
                Return Me.IsNull(Me.tableADDRESSE.TYPColumn)
            End Function

            Public Sub SetTYPNull()
                Me(Me.tableADDRESSE.TYPColumn) = System.Convert.DBNull
            End Sub

            Public Function IsKUNDENNUMMERNull() As Boolean
                Return Me.IsNull(Me.tableADDRESSE.KUNDENNUMMERColumn)
            End Function

            Public Sub SetKUNDENNUMMERNull()
                Me(Me.tableADDRESSE.KUNDENNUMMERColumn) = System.Convert.DBNull
            End Sub

            Public Function IsFAXNull() As Boolean
                Return Me.IsNull(Me.tableADDRESSE.FAXColumn)
            End Function

            Public Sub SetFAXNull()
                Me(Me.tableADDRESSE.FAXColumn) = System.Convert.DBNull
            End Sub


        End Class
        
        <System.Diagnostics.DebuggerStepThrough()>  _
        Public Class ADDRESSERowChangeEvent
            Inherits EventArgs
            
            Private eventRow As ADDRESSERow
            
            Private eventAction As DataRowAction
            
            Public Sub New(ByVal row As ADDRESSERow, ByVal action As DataRowAction)
                MyBase.New
                Me.eventRow = row
                Me.eventAction = action
            End Sub
            
            Public ReadOnly Property Row As ADDRESSERow
                Get
                    Return Me.eventRow
                End Get
            End Property
            
            Public ReadOnly Property Action As DataRowAction
                Get
                    Return Me.eventAction
                End Get
            End Property
        End Class
    End Class
End Namespace