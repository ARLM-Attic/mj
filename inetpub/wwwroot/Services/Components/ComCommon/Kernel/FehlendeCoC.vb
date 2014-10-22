﻿Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Common

Public Class FehlendeCoCl
    Inherits Base.Business.DatenimportBase

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, _
                            ByVal strSessionID As String, _
                            ByVal page As Web.UI.Page)
        m_strClassAndMethod = "FehlendeCoCl.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_FEHLENDE_COC_ALLGEMEIN", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR", strKUNNR)

                myProxy.callBapi()

                Dim tblTemp2 As DataTable = myProxy.getExportTable("WEB_OUT")
                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: FehlendeCoC.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 12.08.10   Time: 10:09
' Updated in $/CKAG2/Services/Components/ComCommon/Kernel
' ITA 4035 Testfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 11.08.10   Time: 17:05
' Created in $/CKAG2/Services/Components/ComCommon/Kernel
' ITA 4035 Torso
' 
'
' ************************************************