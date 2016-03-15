﻿Imports System
Imports KBSBase
Imports GeneralTools.Models
Imports SapORM.Models

Public Class Zulassungen
    Inherits ErrorHandlingClass

#Region "Properties"

    Public Property Kostenstelle As String

    Private m_tblZulassungen As DataTable
    Public ReadOnly Property tblZulassungen As DataTable
        Get
            Return m_tblZulassungen
        End Get
    End Property

#End Region

    Public Sub New(ByVal kst As String)
        Kostenstelle = kst
    End Sub

#Region "Methods"

    Public Sub LoadZulassungen()
        ClearErrorState()

        Try
            Z_FIL_ZUL_EXPORT_ORDER.Init(S.AP, "I_VKBUR", Kostenstelle)

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                m_tblZulassungen = S.AP.GetExportTable("GT_ZUL_ORDER")
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub SaveZulassungen()
        ClearErrorState()

        If m_tblZulassungen.Rows.Count = 0 Then
            RaiseError("9999", "Keine Datensätze zum Speichern vorhanden.")
            Exit Sub
        End If

        Try
            Z_FIL_ZUL_IMPORT_STATUS.Init(S.AP)

            Dim tblSap As DataTable = S.AP.GetImportTable("GT_ZFIL_ZUL_K")

            For Each row As DataRow In m_tblZulassungen.Rows
                Dim newRow As DataRow = tblSap.NewRow()
                newRow("ID") = row("ID")
                newRow("VKBUR") = row("VKBUR")
                newRow("KUNNR") = row("KUNNR")
                newRow("NAME1") = row("NAME1")
                newRow("ZZZLDAT") = row("ZZZLDAT")
                newRow("REFERENZ") = row("REFERENZ")
                newRow("ZZKENN") = row("ZZKENN")
                newRow("ORDERID") = row("ORDERID")
                newRow("HPPOS") = row("HPPOS").ToString().PadLeft(4, "0"c)
                newRow("STATUS") = row("STATUS")
                newRow("POSNR") = row("POSNR").ToString().PadLeft(4, "0"c)
                newRow("MATNR") = row("MATNR")
                newRow("MAKTX") = row("MAKTX")
                newRow("GEBUEHR") = row("GEBUEHR").ToString()
                newRow("GEB_POS") = row("GEB_POS").ToString().PadLeft(4, "0"c)
                tblSap.Rows.Add(newRow)
            Next

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

#End Region

End Class