Imports System.Reflection
Imports System.Runtime.InteropServices

' Allgemeine Informationen �ber eine Assembly werden �ber die folgende 
' Attributgruppe gesteuert. �ndern Sie diese Attributwerte, um Informationen,
' die mit einer Assembly verkn�pft sind, zu bearbeiten.

' Die Werte der Assemblyattribute �berpr�fen

<Assembly: AssemblyTitle("Anwendung VW")> 
<Assembly: AssemblyDescription("ASPX Webanwendung f�r VW")> 
<Assembly: AssemblyCompany("Christoph Kroschke Gruppe")> 
<Assembly: AssemblyProduct("CKG Web Portal")> 
<Assembly: AssemblyCopyright("(c) 2004-2007 Christoph Kroschke Gruppe")> 
<Assembly: AssemblyTrademark("Kroschke")> 
<Assembly: CLSCompliant(True)> 

'Die folgende GUID ist f�r die ID der Typbibliothek, wenn dieses Projekt in COM angezeigt wird
<Assembly: Guid("E72D8F13-7E2A-4F67-973B-9D2F04131EB8")> 

' Versionsinformationen f�r eine Assembly bestehen aus den folgenden vier Werten:
'
'      Haupversion
'      Nebenversion 
'      Buildnummer
'      Revisionsnummer
'
' Sie k�nnen alle Werte angeben oder auf die standardm��igen Build- und Revisionsnummern 
' zur�ckgreifen, indem Sie '*' wie unten angezeigt verwenden:

<Assembly: AssemblyVersion("2007.9.12.0")> 

' ************************************************
' $History: AssemblyInfo.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:48
' Created in $/CKAG/Applications/appvw
' 
' *****************  Version 23  *****************
' User: Uha          Date: 12.09.07   Time: 11:25
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' Kleine �nderung in Report82 (andere Texte) - dabei Formular zur
' �bersetzung vorbereitet
' 
' *****************  Version 22  *****************
' User: Uha          Date: 27.08.07   Time: 9:34
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' �nderungen in Workflow Werkstattzuordnungsliste: Darstellung in
' Detail-Datagrid und Vorauswahl Radio-Buttons
' 
' *****************  Version 21  *****************
' User: Uha          Date: 27.08.07   Time: 9:17
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' Bugfix in Werkstattzuordnungsliste II: Leerer SAP-R�ckgabewert f�r
' CHASSIS_NUM2 verursachte Fehler
' 
' *****************  Version 20  *****************
' User: Uha          Date: 22.08.07   Time: 17:52
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' Bugfixing ITA 1120 und 1177
' 
' *****************  Version 19  *****************
' User: Uha          Date: 15.08.07   Time: 11:23
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' Werkstattzuordnungsliste_Template.xls hinzugef�gt
' 
' *****************  Version 18  *****************
' User: Uha          Date: 15.08.07   Time: 11:16
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' ITA 1177 "Werkstattzuordnungsliste II" - testf�hige Version
' 
' *****************  Version 17  *****************
' User: Uha          Date: 14.08.07   Time: 13:42
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' ITA 1177 "Werkstattzuordnungsliste II" - kompilierf�hige Rohversion
' hinzugef�gt
' 
' *****************  Version 16  *****************
' User: Uha          Date: 13.08.07   Time: 17:03
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' Bugfixing in "Lieferschein-Handling" lt. ITA 1125
' 
' *****************  Version 15  *****************
' User: Uha          Date: 13.08.07   Time: 16:11
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' ITA 1125 "Werkstattzuordnungsliste" hinzugef�gt
' 
' *****************  Version 14  *****************
' User: Uha          Date: 13.08.07   Time: 14:04
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' Lieferschein-Handling lt. ITA 1125 ge�ndert
' 
' *****************  Version 13  *****************
' User: Uha          Date: 18.07.07   Time: 13:38
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' Report "Workflow Werkstattzuordnungsliste" f�r VW und DP zum Testen
' fertig
' 
' *****************  Version 12  *****************
' User: Uha          Date: 17.07.07   Time: 18:51
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' 3. teilfunktionaler Zwischenstand von Change01
' 
' *****************  Version 11  *****************
' User: Uha          Date: 16.07.07   Time: 17:22
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' 2. teilfunktionaler Zwischenstand von Change01
' 
' *****************  Version 10  *****************
' User: Uha          Date: 16.07.07   Time: 14:21
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' Teilfunktionaler Zwischenstand von Change01
' 
' *****************  Version 9  *****************
' User: Uha          Date: 12.07.07   Time: 16:58
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' Report "Workflow Werkstattzuordnungsliste 1" roh hinzugef�gt = Keine
' Komplierfehler aber nicht lauff�hig
' 
' *****************  Version 8  *****************
' User: Uha          Date: 20.06.07   Time: 14:32
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' Logging der Laufzeiten der ASPX-Seiten eingef�hrt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 23.05.07   Time: 8:46
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 6  *****************
' User: Uha          Date: 22.05.07   Time: 10:27
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 21.05.07   Time: 15:52
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' �nderungen im Vergleich zur Startapplikation zum Stand 21.05.2007
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 17:12
' Updated in $/CKG/Applications/AppVW/AppVWWeb
' 
' ************************************************
