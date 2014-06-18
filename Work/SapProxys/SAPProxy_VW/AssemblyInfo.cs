using System.Reflection;
using System.Runtime.CompilerServices;

//
// Allgemeine Informationen �ber eine Assembly werden �ber folgende Attribute 
// gesteuert. �ndern Sie diese Attributswerte, um die Informationen zu modifizieren,
// die mit einer Assembly verkn�pft sind.
//
[assembly: AssemblyTitle("SAP Proxy VW")]
[assembly: AssemblyDescription("SAP Proxy for VW Library")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Christoph Kroschke Gruppe")]
[assembly: AssemblyProduct("CKG Web Portal")]
[assembly: AssemblyCopyright("(c) 2004-2007 Christoph Kroschke Gruppe")]
[assembly: AssemblyTrademark("Kroschke")]
[assembly: AssemblyCulture("")]		

//
// Versionsinformationen f�r eine Assembly bestehen aus folgenden vier Werten:
//
//      Hauptversion
//      Nebenversion 
//      Buildnummer
//      Revision
//
// Sie k�nnen alle Werte oder die standardm��ige Revision und Buildnummer 
// mit '*' angeben:

[assembly: AssemblyVersion("2008.5.14.0")]

//
// Um die Assembly zu signieren, m�ssen Sie einen Schl�ssel angeben. Weitere Informationen 
// �ber die Assemblysignierung finden Sie in der Microsoft .NET Framework-Dokumentation.
//
// Verwenden Sie folgende Attribute, um festzulegen welcher Schl�ssel verwendet wird. 
//
// Hinweis: 
//   (*) Wenn kein Schl�ssel angegeben ist, wird die Assembly nicht signiert.
//   (*) KeyName verweist auf einen Schl�ssel, der im CSP (Crypto Service
//       Provider) auf Ihrem Computer installiert wurde. KeyFile verweist auf eine Datei, die einen
//       Schl�ssel enth�lt.
//   (*) Wenn die Werte f�r KeyFile und KeyName angegeben werden, 
//       werden folgende Vorg�nge ausgef�hrt:
//       (1) Wenn KeyName im CSP gefunden wird, wird dieser Schl�ssel verwendet.
//       (2) Wenn KeyName nicht vorhanden ist und KeyFile vorhanden ist, 
//           wird der Schl�ssel in KeyFile im CSP installiert und verwendet.
//   (*) Um eine KeyFile zu erstellen, k�nnen Sie das Programm sn.exe (Strong Name) verwenden.
//       Wenn KeyFile angegeben wird, muss der Pfad von KeyFile
//       relativ zum Projektausgabeverzeichnis sein:
//       %Project Directory%\obj\<configuration>. Wenn sich KeyFile z.B.
//       im Projektverzeichnis befindet, geben Sie das AssemblyKeyFile-Attribut 
//       wie folgt an: [assembly: AssemblyKeyFile("..\\..\\mykey.snk")]
//   (*) Das verz�gern der Signierung ist eine erweiterte Option. Weitere Informationen finden Sie in der
//       Microsoft .NET Framework-Dokumentation.
//
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]

// ************************************************
// $History: AssemblyInfo.cs $
//
//*****************  Version 10  *****************
//User: Jungj        Date: 14.05.08   Time: 15:11
//Updated in $/CKG/Applications/AppVW/SAPProxy_VW
//ITA 1799
//
//*****************  Version 9  *****************
//User: Uha          Date: 14.08.07   Time: 9:17
//Updated in $/CKG/Applications/AppVW/SAPProxy_VW
//ITA 1177: Z_M_EXP_FIN_001 und Z_M_IMP_FIN_001 hinzugef�gt
//
//*****************  Version 8  *****************
//User: Uha          Date: 13.08.07   Time: 12:36
//Updated in $/CKG/Applications/AppVW/SAPProxy_VW
//Z_V_Zdad_V_Vwnutz_001 ge�ndert und Z_M_Tab_Imp_Zul_002 hinzugef�gt
//
//*****************  Version 7  *****************
//User: Uha          Date: 12.07.07   Time: 17:05
//Updated in $/CKG/Applications/AppVW/SAPProxy_VW
//ITA 1120: Zum Bapi Z_M_IMP_ZUL_HAEND_001 wurden an die �bergabetabelle
//GT_WEB Felder angehangen
//
//*****************  Version 6  *****************
//User: Uha          Date: 12.07.07   Time: 14:32
//Updated in $/CKG/Applications/AppVW/SAPProxy_VW
//ITA 1120: Z_M_IMP_ZUL_HAEND_001 und Z_M_IMP_ZUL_HAEND_002 hinzugef�gt
//
//*****************  Version 5  *****************
//User: Uha          Date: 19.06.07   Time: 13:00
//Updated in $/CKG/Applications/AppVW/SAPProxy_VW
//
// ************************************************