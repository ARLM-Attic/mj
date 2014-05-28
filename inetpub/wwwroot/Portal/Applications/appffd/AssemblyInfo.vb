Imports System.Reflection
Imports System.Runtime.InteropServices

' Allgemeine Informationen �ber eine Assembly werden �ber die folgende 
' Attributgruppe gesteuert. �ndern Sie diese Attributwerte, um Informationen,
' die mit einer Assembly verkn�pft sind, zu bearbeiten.

' Die Werte der Assemblyattribute �berpr�fen

<Assembly: AssemblyTitle("Anwendung FFD")> 
<Assembly: AssemblyDescription("ASPX Webanwendung f�r FFD")> 
<Assembly: AssemblyCompany("Christoph Kroschke Gruppe")> 
<Assembly: AssemblyProduct("CKG Web Portal")> 
<Assembly: AssemblyCopyright("(c) 2004-2007 Christoph Kroschke Gruppe")> 
<Assembly: AssemblyTrademark("Kroschke")> 
<Assembly: CLSCompliant(True)> 

'Die folgende GUID ist f�r die ID der Typbibliothek, wenn dieses Projekt in COM angezeigt wird
<Assembly: Guid("1F9D1EF3-6032-4421-9C0F-C45BA147C8F2")> 

' Versionsinformationen f�r eine Assembly bestehen aus den folgenden vier Werten:
'
'      Haupversion
'      Nebenversion 
'      Buildnummer
'      Revisionsnummer
'
' Sie k�nnen alle Werte angeben oder auf die standardm��igen Build- und Revisionsnummern 
' zur�ckgreifen, indem Sie '*' wie unten angezeigt verwenden:

<Assembly: AssemblyVersion("2007.11.23.0")> 

' ************************************************
' $History: AssemblyInfo.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd
' 
' *****************  Version 28  *****************
' User: Rudolpho     Date: 23.11.07   Time: 15:55
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' ITA: 1372 OR
' 
' *****************  Version 27  *****************
' User: Uha          Date: 29.08.07   Time: 10:05
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' ITA 1224: Neue Ergebnisspalte "Kunde_Unbekannt" hinzugef�gt
' 
' *****************  Version 26  *****************
' User: Uha          Date: 22.08.07   Time: 14:14
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' ITA 1208: Excel-Ergebnisausgabe hinzugef�gt
' 
' *****************  Version 25  *****************
' User: Uha          Date: 22.08.07   Time: 13:43
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Link auf Fahrzeughistorie aus Datagrid1 in Change81_2 entfernt
' 
' *****************  Version 24  *****************
' User: Uha          Date: 22.08.07   Time: 13:23
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' ITA 1208: Bugfixing 1
' 
' *****************  Version 23  *****************
' User: Uha          Date: 22.08.07   Time: 12:30
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' ITA 1208 Testversion
' 
' *****************  Version 22  *****************
' User: Uha          Date: 21.08.07   Time: 17:37
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' ITA 1208: Kompilierf�hige Vorversion mit Teilfunktionalit�t
' 
' *****************  Version 21  *****************
' User: Uha          Date: 20.08.07   Time: 16:17
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' 
' *****************  Version 20  *****************
' User: Uha          Date: 16.08.07   Time: 11:39
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' ITAs 1162, 1223 und 1161 werden jetzt �ber Report11.aspx abgewickelt.
' Report14 wieder komplett gel�scht.
' 
' *****************  Version 19  *****************
' User: Uha          Date: 15.08.07   Time: 17:30
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' ITA 1223: "Gesamtbestand Fahrzeugbriefe" (Report14) hinzugef�gt
' 
' *****************  Version 18  *****************
' User: Uha          Date: 15.08.07   Time: 16:18
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' ITA 1224: "Hinterlegung ALM-Daten" (Change80) hinzugef�gt
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 14.08.07   Time: 10:01
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Bugfix: Report31_2 - FillGrid" if objBank.AuftragsUebersicht Is
' Nothing" hinzugef�gt
' 
' *****************  Version 16  *****************
' User: Uha          Date: 13.08.07   Time: 10:55
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Schreibfehler in AssemblyVersion beseitigt
' 
' *****************  Version 15  *****************
' User: Uha          Date: 13.08.07   Time: 10:39
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' CSV-Ausgabe in MDR Report "Versendete Zulassungsdaten" inegriert
' 
' *****************  Version 14  *****************
' User: Uha          Date: 9.08.07    Time: 15:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Report "Versendete Zulassungsdaten" - 1. Version ohne Excel Download
' 
' *****************  Version 13  *****************
' User: Uha          Date: 9.08.07    Time: 11:12
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Report "Versendete Zulassungsdaten" vorbereitet
' 
' *****************  Version 12  *****************
' User: Uha          Date: 8.08.07    Time: 13:06
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' SAPProxy_MDR hinzugef�gt
' 
' *****************  Version 11  *****************
' User: Uha          Date: 2.07.07    Time: 17:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 10  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Logging der Laufzeiten der ASPX-Seiten eingef�hrt
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 8.06.07    Time: 15:36
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Abgleich Beyond Compare
' 
' *****************  Version 8  *****************
' User: Uha          Date: 23.05.07   Time: 9:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 7  *****************
' User: Uha          Date: 22.05.07   Time: 13:20
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 6  *****************
' User: Uha          Date: 21.05.07   Time: 14:22
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' �nderungen im Vergleich zur Startapplikation zum Stand 21.05.2007
' 
' *****************  Version 5  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' �nderungen aus StartApplication vom 02.05.2007 Mittags �bernommen
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 17:02
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' 
' ************************************************
