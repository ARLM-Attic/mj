Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class Sixt_B08
    REM � Status-Report, Kunde: Sixt, BAPI: Z_M_Abm_Fehlende_Unterlagen,
    REM � Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

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


    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "Sixt_B08.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1

            Try
                Dim strAnzahl As String = ""

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ABM_FEHLENDE_UNTERLAGEN", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_ABM_FEHLENDE_UNTERLAGEN", "KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'Dim tblTemp2 As DataTable = myProxy.getExportTable("AUSGABE")
                Dim tblTemp2 As DataTable = S.AP.GetExportTable("AUSGABE")

                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: Sixt_B08.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 6  *****************
' User: Uha          Date: 3.07.07    Time: 9:25
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' 
' ************************************************
