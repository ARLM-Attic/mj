Imports System.Reflection
Imports System.Runtime.InteropServices

' Allgemeine Informationen �ber eine Assembly werden �ber die folgende 
' Attributgruppe gesteuert. �ndern Sie diese Attributwerte, um Informationen,
' die mit einer Assembly verkn�pft sind, zu bearbeiten.

' Die Werte der Assemblyattribute �berpr�fen

<Assembly: AssemblyTitle("Anwendung ECAN")> 
<Assembly: AssemblyDescription("ASPX Webanwendung f�r ECAN")> 
<Assembly: AssemblyCompany("Christoph Kroschke Gruppe")> 
<Assembly: AssemblyProduct("CKG Web Portal")> 
<Assembly: AssemblyCopyright("(c) 2004-2007 Christoph Kroschke Gruppe")> 
<Assembly: AssemblyTrademark("Kroschke")> 
<Assembly: CLSCompliant(True)> 

'Die folgende GUID ist f�r die ID der Typbibliothek, wenn dieses Projekt in COM angezeigt wird
<Assembly: Guid("9BB8019B-E9B2-4D52-B0AF-F1F0364D9288")> 

' Versionsinformationen f�r eine Assembly bestehen aus den folgenden vier Werten:
'
'      Haupversion
'      Nebenversion 
'      Buildnummer
'      Revisionsnummer
'
' Sie k�nnen alle Werte angeben oder auf die standardm��igen Build- und Revisionsnummern 
' zur�ckgreifen, indem Sie '*' wie unten angezeigt verwenden:

<Assembly: AssemblyVersion("2007.8.20.1")> 

' ************************************************
' $History: AssemblyInfo.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:07
' Created in $/CKAG/Applications/appecan
' 
' *****************  Version 18  *****************
' User: Uha          Date: 20.08.07   Time: 17:11
' Updated in $/CKG/Applications/AppECAN/AppECANWeb
' Bugfix in Change01.aspx: Speicherbutton muss (ohne Autopostback im
' Datagrid) immer offen sein.
' 
' *****************  Version 17  *****************
' User: Uha          Date: 20.08.07   Time: 16:32
' Updated in $/CKG/Applications/AppECAN/AppECANWeb
' �berfl�ssiges Autopostback aus Change01.aspx (Datagrid) entfernt
' 
' *****************  Version 16  *****************
' User: Uha          Date: 12.07.07   Time: 14:45
' Updated in $/CKG/Applications/AppECAN/AppECANWeb
' In Report80 "Abmeldedatum" in Anzeige durch "Datum" ersetzt
' 
' *****************  Version 15  *****************
' User: Uha          Date: 11.07.07   Time: 12:37
' Updated in $/CKG/Applications/AppECAN/AppECANWeb
' Bug in Parameterliste von Z_M_Brief_Eingang gefixt
' 
' *****************  Version 14  *****************
' User: Uha          Date: 11.07.07   Time: 11:10
' Updated in $/CKG/Applications/AppECAN/AppECANWeb
' Report "T�glicher Eingang Fahrzeugbriefe" hinzugef�gt
' 
' *****************  Version 13  *****************
' User: Uha          Date: 9.07.07    Time: 17:12
' Updated in $/CKG/Applications/AppECAN/AppECANWeb
' Report "Auction Report ohne Vorlage ZBII" hinzugef�gt
' 
' *****************  Version 12  *****************
' User: Uha          Date: 9.07.07    Time: 13:26
' Updated in $/CKG/Applications/AppECAN/AppECANWeb
' Bugfixing - Change01 ("Freigabe Versand bei Zahlungseingang")
' 
' *****************  Version 11  *****************
' User: Uha          Date: 9.07.07    Time: 12:57
' Updated in $/CKG/Applications/AppECAN/AppECANWeb
' Change01 ("Freigabe Versand bei Zahlungseingang") zum Testen bereit
' 
' *****************  Version 10  *****************
' User: Uha          Date: 5.07.07    Time: 18:33
' Updated in $/CKG/Applications/AppECAN/AppECANWeb
' Erster Zwischenstand f�r "Freigabe Versand bei Zahlungseingang" -
' Wegschreiben fehlt noch
' 
' *****************  Version 9  *****************
' User: Uha          Date: 5.07.07    Time: 11:14
' Updated in $/CKG/Applications/AppECAN/AppECANWeb
' Proxy SAPProxy_ECAN2 hinzugef�gt
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 17:26
' Updated in $/CKG/Applications/AppECAN/AppECANWeb
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 12:32
' Updated in $/CKG/Applications/AppECAN/AppECANWeb
' Logging der Laufzeiten der ASPX-Seiten eingef�hrt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 8.03.07    Time: 17:01
' Updated in $/CKG/Applications/AppECAN/AppECANWeb
' 
' ************************************************
