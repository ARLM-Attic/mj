Imports System.Reflection
Imports System.Runtime.InteropServices

' Allgemeine Informationen �ber eine Assembly werden �ber die folgende 
' Attributgruppe gesteuert. �ndern Sie diese Attributwerte, um Informationen,
' die mit einer Assembly verkn�pft sind, zu bearbeiten.

' Die Werte der Assemblyattribute �berpr�fen

<Assembly: AssemblyTitle("Anwendung Kroschke")> 
<Assembly: AssemblyDescription("ASPX Webanwendung f�r Kroschke")> 
<Assembly: AssemblyCompany("Christoph Kroschke Gruppe")> 
<Assembly: AssemblyProduct("CKG Web Portal")> 
<Assembly: AssemblyCopyright("(c) 2004-2007 Christoph Kroschke Gruppe")> 
<Assembly: AssemblyTrademark("Kroschke")> 
<Assembly: CLSCompliant(True)> 

'Die folgende GUID ist f�r die ID der Typbibliothek, wenn dieses Projekt in COM angezeigt wird
<Assembly: Guid("6F2B35A0-4E14-4B1A-9B4E-CFE09BF58900")> 

' Versionsinformationen f�r eine Assembly bestehen aus den folgenden vier Werten:
'
'      Haupversion
'      Nebenversion 
'      Buildnummer
'      Revisionsnummer
'
' Sie k�nnen alle Werte angeben oder auf die standardm��igen Build- und Revisionsnummern 
' zur�ckgreifen, indem Sie '*' wie unten angezeigt verwenden:

<Assembly: AssemblyVersion("2007.11.15.0")> 

' ************************************************
' $History: AssemblyInfo.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:30
' Created in $/CKAG/Applications/AppKroschke
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 15.11.07   Time: 8:23
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb
' ITA:1433
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 13.11.07   Time: 16:38
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb
' ITA: 1374, 1404, 1433
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 11.09.07   Time: 9:06
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb
' ITA: 1218
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 20.08.07   Time: 16:48
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb
' ITA: 1192 ,1242
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 7.08.07    Time: 11:31
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb
' Bugfix Nacherfassung Abfangen von Falscheingaben in
' Input_004_011(Preise/Geb�hren) 
' 
' *****************  Version 11  *****************
' User: Uha          Date: 5.07.07    Time: 9:52
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb
' Leerauswahl f�r ddlSTVA hinzugef�gt (nach SAP-Lesen)
' 
' *****************  Version 10  *****************
' User: Uha          Date: 2.07.07    Time: 18:01
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 9  *****************
' User: Uha          Date: 2.07.07    Time: 13:07
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb
' Logging der Laufzeiten der ASPX-Seiten eingef�hrt
' 
' *****************  Version 8  *****************
' User: Uha          Date: 21.06.07   Time: 19:35
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb
' Bugfixing 2
' 
' *****************  Version 7  *****************
' User: Uha          Date: 21.06.07   Time: 11:02
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb
' In Input_004_01.aspx / function SetKunnr() den Kommentar vor
' document.Form1.txtDummy.value = document.Form1.ddlKunnr.selectedIndex;
' entfernt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 23.05.07   Time: 9:31
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.03.07    Time: 17:05
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb
' 
' ************************************************
