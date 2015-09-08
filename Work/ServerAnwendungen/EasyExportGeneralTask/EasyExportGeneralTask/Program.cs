﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using GeneralTools.Models;
using SapORM.Models;
using UniversalFileBasedLogging;
using WMDQryCln;

namespace EasyExportGeneralTask
{
    class Program
    {
        private static object total_hits;
        private static EasyResult result = new EasyResult();
        private static TaskKonfiguration taskConfiguration;

        private static LoggingClass LC;
        private static LogDataset logDS;
        private static LogCustomer logCustomer;
        private static List<LogFile> logFiles = new List<LogFile>(); 

        static void Main(string[] args)
        {
            // ----- TEST -----
            //args = new[] { "Athlon" };
            //args = new[] { "EuropaService" };
            //args = new[] { "StarCar" };
            //args = new[] { "XLeasing" };
            //args = new[] { "Alphabet" };
            //args = new[] { "LeasePlan" };
            //args = new[] { "XLCheck" };
            //args = new[] { "CharterWay_All" };
            //args = new[] { "CharterWay_Single" };
            //args = new[] { "DCBank" };
            //args = new[] { "DaimlerFleet" };
            //args = new[] { "SixtMobility" };
            //args = new[] { "Europcar" };
            // ----- TEST -----

            if ((args.Length > 0) && (!String.IsNullOrEmpty(args[0])))
            {
                string configName = args[0];

                // XML-Konfiguration einlesen
                if (readXmlConfiguration(configName))
                {
                    // Verarbeitung starten (separater STA-Thread erforderlich wg. COM-Library)
                    Thread workThread = new Thread(doWork);
                    workThread.SetApartmentState(ApartmentState.STA);
                    workThread.Start();
                    workThread.Join();
                }
                else
                {
                    Console.WriteLine("EasyExportGeneralTask_" + taskConfiguration.Name + ": Die Konfigurationsdatei konnte nicht gelesen werden!");
                    EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Die Konfigurationsdatei konnte nicht gelesen werden!", EventLogEntryType.Warning);
                }
            }
            else
            {
                Console.WriteLine("EasyExportGeneralTask: Parameter Konfigurationsname fehlt!"); 
                EventLog.WriteEntry("EasyExportGeneralTask", "Parameter Konfigurationsname fehlt!", EventLogEntryType.Warning); 
            }

            if (Konfiguration.pauseAfterCompletion)
            {
                Console.WriteLine();
                Console.WriteLine("Zum Beenden beliebige Taste drücken...");
                Console.ReadKey();
            }      
        }

        private static void doWork()
        {
            DateTime startTime = DateTime.Now;

            if (taskConfiguration.AbfrageNachDatum)
            {
                if (taskConfiguration.Abfragedatum.Year == 1900)
                {
                    taskConfiguration.Abfragedatum = DateTime.Today;
                }
                Console.WriteLine("EasyExportGeneralTask_" + taskConfiguration.Name + ": Verarbeitung gestartet. Selektionsdatum=" + taskConfiguration.Abfragedatum.ToShortDateString());
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Verarbeitung gestartet. Selektionsdatum=" + taskConfiguration.Abfragedatum.ToShortDateString(), EventLogEntryType.Information);
            }
            else
            {
                Console.WriteLine("EasyExportGeneralTask_" + taskConfiguration.Name + ": Verarbeitung gestartet.");
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Verarbeitung gestartet.", EventLogEntryType.Information);
            }

            #region Logging

            try
            {
                LC = new LoggingClass(Konfiguration.easyLogPathXml, taskConfiguration.Name);
                logDS = LC.ReadLog(taskConfiguration.LogfilesMitTitelStattKennzeichen);
                logCustomer = logDS.FindCustomer(taskConfiguration.easyLocation, taskConfiguration.easyArchiveNameFirst);

                if (logCustomer == null)
                {
                    logCustomer = new LogCustomer(taskConfiguration.easyLocation, taskConfiguration.easyArchiveNameFirst, logDS);
                }
            }
            catch (Exception)
            {
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Logging konnte nicht gestartet werden", EventLogEntryType.Warning);
            }

            #endregion

            #region Unbrauchbare Dateien löschen

            try
            {
                ClearBrokenFiles();
            }
            catch (Exception)
            {
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Unbrauchbare Dateien konnten nicht gelöscht werden.", EventLogEntryType.Warning);
            }

            #endregion

            if (taskConfiguration.VerzeichnisseLeeren)
            {
                #region Arbeitsverzeichnisse leeren

                try
                {
                    clearFolders();
                    EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Alte Dateien im Verarbeitungsordner gelöscht", EventLogEntryType.Information);
                }
                catch (Exception)
                {
                    EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Alte Dateien im Verarbeitungsordner konnten nicht gelöscht werden.", EventLogEntryType.Warning);
                }

                #endregion
            }

            switch (taskConfiguration.Ablauf)
            {
                case AblaufTyp.Athlon:
                    #region Athlon

                    // Normaler Durchlauf
                    QueryAthlon();
                    EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "1. Durchlauf(normale Verarbeitung) beendet.", EventLogEntryType.Information);
                    Console.WriteLine("Vergangene Zeit: " + (DateTime.Now - startTime).TotalMinutes.ToString("F2") + " Minuten");

                    // Durchlauf der Fehlersätze
                    QueryAthlon(true);
                    EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "2. Durchlauf(Nachverarbeitung) beendet.", EventLogEntryType.Information);
                    Console.WriteLine("Vergangene Zeit: " + (DateTime.Now - startTime).TotalMinutes.ToString("F2") + " Minuten");

                    #endregion
                    break;

                case AblaufTyp.EuropaService:
                    #region EuropaService

                    // Normaler Durchlauf
                    QueryEuropaService();
                    EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "1. Durchlauf(normale Verarbeitung) beendet.", EventLogEntryType.Information);
                    Console.WriteLine("Vergangene Zeit: " + (DateTime.Now - startTime).TotalMinutes.ToString("F2") + " Minuten");

                    // Durchlauf der Fehlersätze
                    QueryEuropaService(true);
                    EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "2. Durchlauf(Nachverarbeitung) beendet.", EventLogEntryType.Information);
                    Console.WriteLine("Vergangene Zeit: " + (DateTime.Now - startTime).TotalMinutes.ToString("F2") + " Minuten");

                    #endregion
                    break;

                case AblaufTyp.StarCar:
                    #region StarCar

                    // Normaler Durchlauf
                    QueryStarCar();
                    EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "1. Durchlauf(normale Verarbeitung) beendet.", EventLogEntryType.Information);
                    Console.WriteLine("Vergangene Zeit: " + (DateTime.Now - startTime).TotalMinutes.ToString("F2") + " Minuten");

                    // Durchlauf der Fehlersätze
                    QueryStarCar(true);
                    EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "2. Durchlauf(Nachverarbeitung) beendet.", EventLogEntryType.Information);
                    Console.WriteLine("Vergangene Zeit: " + (DateTime.Now - startTime).TotalMinutes.ToString("F2") + " Minuten");

                    #endregion
                    break;

                case AblaufTyp.XLeasing:
                    #region XLeasing

                    QueryXLeasing();

                    #endregion
                    break;

                case AblaufTyp.Alphabet:
                    #region Alphabet

                    QueryAlphabet();

                    #endregion
                    break;

                case AblaufTyp.LeasePlan:
                    #region LeasePlan

                    QueryLeasePlan();

                    #endregion
                    break;

                case AblaufTyp.XLCheck:
                    #region XLCheck

                    QueryXLCheck();

                    #endregion
                    break;

                case AblaufTyp.CharterWay_All:
                    #region CharterWay_All

                    // Normaler Durchlauf
                    QueryCharterWay_All();
                    EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "1. Durchlauf(normale Verarbeitung) beendet.", EventLogEntryType.Information);
                    Console.WriteLine("Vergangene Zeit: " + (DateTime.Now - startTime).TotalMinutes.ToString("F2") + " Minuten");

                    // Durchlauf der Fehlersätze
                    QueryCharterWay_All(true);
                    EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "2. Durchlauf(Nachverarbeitung) beendet.", EventLogEntryType.Information);
                    Console.WriteLine("Vergangene Zeit: " + (DateTime.Now - startTime).TotalMinutes.ToString("F2") + " Minuten");

                    #endregion
                    break;

                case AblaufTyp.CharterWay_Single:
                    #region CharterWay_Single

                    QueryCharterWay_Single();

                    #endregion
                    break;

                case AblaufTyp.DCBank:
                    #region DCBank

                    QueryDCBank();

                    #endregion
                    break;

                case AblaufTyp.DaimlerFleet:
                    #region DaimlerFleet

                    QueryDaimlerFleet();

                    #endregion
                    break;

                case AblaufTyp.SixtMobility:
                    #region SixtMobility

                    QuerySixtMobility();

                    #endregion
                    break;

                case AblaufTyp.Autoinvest:
                    #region Autoinvest

                    QueryAutoinvest();

                    #endregion
                    break;

                case AblaufTyp.Europcar:
                    #region Europcar

                    QueryEuropcar();

                    #endregion
                    break;

                default:
                    Console.WriteLine("FEHLER: Config-Parameter 'Ablauf' hat keine gültigen Wert");
                    EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Config-Parameter 'Ablauf' hat keine gültigen Wert", EventLogEntryType.Information);
                    return;
            }

            EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Verarbeitung beendet.", EventLogEntryType.Information);

            if (logFiles.Count > 0)
            {
                Helper.SendErrorEMail("Übertragungsfehler durch EasyExportGeneralTask.exe (" + taskConfiguration.Name + ")", logFiles);
            }
        }

        ///// <summary>
        ///// (Neu-)erstellen der Config-Datei (Achtung: NUR initial erforderlich)
        ///// </summary>
        //private static void createConfigFile()
        //{
        //    // Beispielkonfiguration für Athlon
        //    TaskKonfiguration dummy = new TaskKonfiguration
        //    {
        //        Name = "Athlon",
        //        Kundennummer = "12345",
        //        Ablauf = AblaufTyp.Athlon,
        //        AbfrageNachDatum = true,
        //        Abfragedatum = DateTime.Today,
        //        easyLocation = "ATHLON",
        //        easyBlobPathLocal = @"C:\TEMP",
        //        exportPathZBII = @"C:\TEMP",
        //        exportPathSteuerB = @"C:\TEMP",
        //        exportPathZip = @"C:\TEMP"
        //    };

        //    ArchivDefinition arcStandard = new ArchivDefinition
        //    {
        //        Name = "",
        //        Typ = ArchivTyp.Standard,
        //        IstJahresarchiv = false,
        //        IstJahrVierstellig = false
        //    };
        //    ArchivDefinition arcDokumente = new ArchivDefinition
        //    {
        //        Name = "ATHLON",
        //        Typ = ArchivTyp.Dokumente,
        //        IstJahresarchiv = true,
        //        IstJahrVierstellig = false
        //    };
        //    ArchivDefinition arcSteuerbescheide = new ArchivDefinition
        //    {
        //        Name = "STEUER",
        //        Typ = ArchivTyp.Steuerbescheide,
        //        IstJahresarchiv = true,
        //        IstJahrVierstellig = false
        //    };

        //    dummy.Archive = new List<ArchivDefinition> { arcStandard, arcDokumente, arcSteuerbescheide };

        //    List<TaskKonfiguration> liste = new List<TaskKonfiguration> {dummy};

        //    using (StreamWriter sWriter = new StreamWriter(@"c:\temp\configfile.xml", false))
        //    {
        //        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<TaskKonfiguration>));
        //        xmlSerializer.Serialize(sWriter, liste);
        //    }

        //    Console.WriteLine("Config-Datei erstellt.");
        //}

        /// <summary>
        /// Task-Konfiguration aus Xml-Datei einlesen
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        private static bool readXmlConfiguration(string configName)
        {
            try
            {
                using (StreamReader strReader = new StreamReader(Konfiguration.xmlConfigFilePath))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<TaskKonfiguration>));
                    var configs = (List<TaskKonfiguration>)xmlSerializer.Deserialize(strReader);

                    foreach (TaskKonfiguration config in configs)
                    {
                        if (config.Name.ToUpper() == configName.ToUpper())
                        {
                            taskConfiguration = config;
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler beim Einlesen der Konfigurationsdatei: " + ex.Message);
            }     

            return false;
        }

        /// <summary>
        /// Löscht die Inhalte aller Arbeitsordner
        /// </summary>
        private static void clearFolders()
        {
            if (!String.IsNullOrEmpty(taskConfiguration.exportPathZBII))
            {
                foreach (string filePath in Directory.GetFiles(taskConfiguration.exportPathZBII))
                {
                    File.Delete(filePath);
                }
            }

            if (!String.IsNullOrEmpty(taskConfiguration.exportPathSteuerB))
            {
                foreach (string filePath in Directory.GetFiles(taskConfiguration.exportPathSteuerB))
                {
                    File.Delete(filePath);
                }
            }

            if (!String.IsNullOrEmpty(taskConfiguration.easyBlobPathLocal))
            {
                foreach (string filePath in Directory.GetFiles(taskConfiguration.easyBlobPathLocal))
                {
                    File.Delete(filePath);
                }
            }
        }

        /// <summary>
        /// Archivabfrage für Athlon
        /// </summary>
        /// <param name="blnFehlersaetze"></param>
        private static void QueryAthlon(bool blnFehlersaetze = false)
        {
            string queryexpression = "";
            Process sdp;

            try
            {
                result.clear();

                // EasyArchiv-Query initialisieren
                clsQueryClass Weblink = new clsQueryClass();
                Weblink.Configure(taskConfiguration);

                if ((taskConfiguration.AbfrageNachDatum) && (taskConfiguration.Abfragedatum.Year > 1900))
                {
                    queryexpression = ".103=#" + taskConfiguration.Abfragedatum.ToShortDateString();
                }

                // Fahrzeugpapiere aus Archiv holen
                string status = Weblink.QueryArchive(taskConfiguration.easyArchiveNameDokumente, queryexpression, ref total_hits, ref result, taskConfiguration);

                if (status == "Keine Daten gefunden.")
                {
                    return;
                }

                // Steuerbescheide aus Archiv holen
                Weblink.QueryArchive(taskConfiguration.easyArchiveNameSteuern, queryexpression, ref total_hits, ref result, taskConfiguration);

                if (blnFehlersaetze)
                {
                    #region Filtern auf alle auf Fehler gelaufenen Dateien

                    result.hitList.Columns.Add("found", typeof(Boolean));
                    result.hitList.AcceptChanges();

                    if (logCustomer != null)
                    {
                        foreach (LogFile file in logCustomer)
                        {
                            // Alle Sätze zu einer Fahrgestellnummer finden, da der gesamte Satz neu gezogen werden muss
                            foreach (DataRow row in result.hitList.Rows)
                            {
                                if (row["FAHRGESTELLNR"].ToString() == file.FIN)
                                {
                                    row["found"] = true;
                                }
                            }
                        }
                    }

                    for (int i = 0; i < result.hitList.Rows.Count; i++)
                    {
                        if (result.hitList.Rows[i]["found"] is DBNull)
                        {
                            result.hitList.Rows[i].Delete();
                        }
                    }
                    result.hitList.AcceptChanges();

                    // Fehlerhafte Dateien aus Arbeitsordnern löschen
                    ClearBrokenFiles();

                    #endregion
                }

                // Bilder holen
                for (int i = 0; i < result.hitList.Rows.Count; i++)
                {
                    status = Weblink.QueryPicture(ref result, ref LC, logDS, logCustomer, taskConfiguration, ref logFiles, i, blnFehlersaetze);

                    if (!String.IsNullOrEmpty(status))
                    {
                        Console.WriteLine(status);
                    }
                }
                
                #region ZBII und COC zusammenführen und verschieben

                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Verarbeitung zur Zusammenführung u extrahieren der coc zu den ZBII gestartet", EventLogEntryType.Information);

                DataRow[] selection;

                if (blnFehlersaetze)
                {
                    selection = result.hitList.Select("[.TITEL]='ZB2' AND found=true");
                }
                else
                {
                    selection = result.hitList.Select("[.TITEL]='ZB2'");
                }

                foreach (DataRow row in selection)
                {
                    var fin = row["FAHRGESTELLNR"].ToString();

                    try
                    {
                        if (row["Filepath"] != DBNull.Value)
                        {
                            generateJPLFile("ZB", fin, row["LVNR"].ToString(), row["NUMMERLN"].ToString(), row["KENNZEICHEN"].ToString(), DateTime.Parse(row[".ARCHIVDATUM"].ToString()));

                            string strFilePathZBII = row["Filepath"].ToString();

                            // COC finden
                            var rowFound = result.hitList.Select("[.TITEL]='COC' AND FAHRGESTELLNR='" + fin + "'");
                            DataRow rowCOC = null;

                            if (rowFound.Length > 0)
                            {
                                rowCOC = rowFound[0];
                            }

                            if (rowCOC == null)
                            {
                                File.Copy(strFilePathZBII, taskConfiguration.exportPathZBII + "\\ZB_" + fin + ".pdf", true);

                                Console.WriteLine("Für " + fin + " " + row[".TITEL"] + " existiert keine COC. Datei als ZB_" + fin + ".pdf übertragen.");
                            }
                            else
                            {
                                string strFilePathCOC = rowCOC["Filepath"].ToString();

                                string AdultPDFCommand = "-mer -i \"" + strFilePathZBII + "\" -i \"" + strFilePathCOC + "\" -o \"" + taskConfiguration.exportPathZBII + "\\ZB_" + fin + ".pdf\"";

                                int indexLastTrenn = strFilePathZBII.LastIndexOf('\\') + 1;
                                string strFilname = strFilePathZBII.Substring(indexLastTrenn, strFilePathZBII.Length - indexLastTrenn);

                                int indexLastTrennCOC = strFilePathCOC.LastIndexOf('\\') + 1;
                                string strFilnameCOC = strFilePathCOC.Substring(indexLastTrennCOC, strFilePathCOC.Length - indexLastTrennCOC);

                                Console.WriteLine("Führe die PDFs ");
                                Console.WriteLine(strFilname);
                                Console.WriteLine(strFilnameCOC);
                                Console.WriteLine(" zu ZB_" + fin + ".pdf zusammen.");

                                if (File.Exists(Konfiguration.pathPdfSplitAndMergeApplication))
                                {
                                    // Mergen der PDF's
                                    sdp = Process.Start(Konfiguration.pathPdfSplitAndMergeApplication, AdultPDFCommand);

                                    if (sdp != null)
                                    {
                                        int waitCount = 0;

                                        while (!sdp.HasExited)
                                        {
                                            if (waitCount == 10)
                                            {
                                                if (sdp.Responding)
                                                {
                                                    sdp.CloseMainWindow();
                                                }
                                                else
                                                {
                                                    sdp.Kill();
                                                }
                                                Console.WriteLine("Exit with Error!");
                                                break;
                                            }

                                            waitCount++;
                                            // Ressourcen vorübergehend freigeben während Merge-Prozess arbeitet
                                            Thread.Sleep(2000);
                                        }
                                    }
                                }
                                else
                                {
                                    EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "AdultPDF.exe nicht gefunden! ZBII- und COC-Mergen übersprungen.", EventLogEntryType.Warning);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                #endregion

                #region Steuerbescheide verarbeiten

                foreach (DataRow row in result.hitList.Select("DOC_Archive='" + taskConfiguration.easyArchiveNameSteuern + "'"))
                {
                    if (row["Filepath"] != DBNull.Value)
                    {
                        var fin = row["FAHRGESTELLNR"].ToString();

                        // jplDatei Schreiben
                        generateJPLFile("S", fin, row["LVNR"].ToString(), row["NUMMERLN"].ToString(), row["KENNZEICHEN"].ToString(), DateTime.Parse(row[".ARCHIVDATUM"].ToString()));

                        // Datei kopieren und umbenennen
                        File.Copy(row["Filepath"].ToString(), taskConfiguration.exportPathSteuerB + "\\S_" + fin + ".pdf", true);

                        Console.WriteLine(fin + " " + row[".TITEL"] + " als S_" + fin + ".pdf übertragen.");
                    }
                }

                #endregion

                #region Dokumente zippen

                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Komprimieren der Ordner gestartet", EventLogEntryType.Information);

                string zipcommand;
                DirectoryInfo di;
                FileInfo[] aryFi;

                #region Fahrzeugdokumente zippen

                di = new DirectoryInfo(taskConfiguration.exportPathZBII);
                aryFi = di.GetFiles("*");

                if (aryFi.Length > 0)
                {
                    zipcommand = " a -tzip " + taskConfiguration.exportPathZip + "\\Fahrzeugdokumente " + taskConfiguration.exportPathZBII + "\\*";
                    sdp = Process.Start(Konfiguration.pathZipApplication, zipcommand);

                    if (sdp != null)
                    {
                        int waitCount = 0;
                        while (!sdp.HasExited)
                        {
                            if (waitCount == 20)
                            {
                                if (sdp.Responding)
                                {
                                    sdp.CloseMainWindow();
                                }
                                else
                                {
                                    sdp.Kill();
                                }
                                Console.WriteLine("Exit with Error!");
                                break;
                            }

                            waitCount++;
                            Thread.Sleep(10000);
                        }
                    }
                }
                else
                {
                    EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Keine ZBII Dateien zum komprimieren gefunden.", EventLogEntryType.Information);
                }

                #endregion

                #region Steuerbescheide zippen

                di = new DirectoryInfo(taskConfiguration.exportPathSteuerB);
                aryFi = di.GetFiles("*");

                if (aryFi.Length > 0)
                {
                    zipcommand = " a -tzip " + taskConfiguration.exportPathZip + "\\Steuerbescheide " + taskConfiguration.exportPathSteuerB + "\\*";
                    sdp = Process.Start(Konfiguration.pathZipApplication, zipcommand);

                    if (sdp != null)
                    {
                        int waitCount = 0;
                        while (!sdp.HasExited)
                        {
                            if (waitCount == 20)
                            {
                                if (sdp.Responding)
                                {
                                    sdp.CloseMainWindow();
                                }
                                else
                                {
                                    sdp.Kill();
                                }
                                Console.WriteLine("Exit with Error!");
                                break;
                            }

                            waitCount++;
                            Thread.Sleep(10000);
                        }
                    }
                }
                else
                {
                    EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Keine Steuerbescheide zum komprimieren gefunden.", EventLogEntryType.Information);
                }

                #endregion

                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Komprimieren der Ordner beendet", EventLogEntryType.Information);

                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine("EasyExportGeneralTask_" + taskConfiguration.Name + ": Verarbeitung abgebrochen:  " + ex.Message);
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Verarbeitung abgebrochen:  " + ex.Message, EventLogEntryType.Warning);
            }
        }

        /// <summary>
        /// Archivabfrage für EuropaService
        /// </summary>
        /// <param name="blnFehlersaetze"></param>
        private static void QueryEuropaService(bool blnFehlersaetze = false)
        {
            string queryexpression = "";

            try
            {
                result.clear();

                // EasyArchiv-Query initialisieren
                clsQueryClass Weblink = new clsQueryClass();
                Weblink.Configure(taskConfiguration);

                if ((taskConfiguration.AbfrageNachDatum) && (taskConfiguration.Abfragedatum.Year > 1900))
                {
                    queryexpression = ".103=#" + taskConfiguration.Abfragedatum.ToShortDateString();
                }

                #region Dokumente aus Archiv(en) holen

                // Dokumente aus Archiv holen
                string status = Weblink.QueryArchive(taskConfiguration.easyArchiveNameStandard, queryexpression, ref total_hits, ref result, taskConfiguration);

                if (status == "Keine Daten gefunden.")
                {
                    return;
                }

                #endregion

                if (blnFehlersaetze)
                {
                    #region Filtern auf alle auf Fehler gelaufenen Dateien

                    result.hitList.Columns.Add("found", typeof(Boolean));
                    result.hitList.AcceptChanges();

                    if (logCustomer != null)
                    {
                        foreach (LogFile file in logCustomer)
                        {
                            // Alle Sätze zu einer Fahrgestellnummer finden, da der gesamte Satz neu gezogen werden muss
                            foreach (DataRow row in result.hitList.Rows)
                            {
                                if (row["FAHRGESTELLNR"].ToString() == file.FIN)
                                {
                                    row["found"] = true;
                                }
                            }
                        }
                    }

                    for (int i = 0; i < result.hitList.Rows.Count; i++)
                    {
                        if (result.hitList.Rows[i]["found"] is DBNull)
                        {
                            result.hitList.Rows[i].Delete();
                        }
                    }
                    result.hitList.AcceptChanges();

                    // Fehlerhafte Dateien aus Arbeitsordnern löschen
                    ClearBrokenFiles();

                    #endregion
                }

                // Bilder holen
                for (int i = 0; i < result.hitList.Rows.Count; i++)
                {
                    status = Weblink.QueryPicture(ref result, ref LC, logDS, logCustomer, taskConfiguration, ref logFiles, i, blnFehlersaetze);

                    if (!String.IsNullOrEmpty(status))
                    {
                        Console.WriteLine(status);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EasyExportGeneralTask_" + taskConfiguration.Name + ": Verarbeitung abgebrochen:  " + ex.Message);
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Verarbeitung abgebrochen:  " + ex.Message, EventLogEntryType.Warning);
            }
        }

        /// <summary>
        /// Archivabfrage für StarCar
        /// </summary>
        /// <param name="blnFehlersaetze"></param>
        private static void QueryStarCar(bool blnFehlersaetze = false)
        {
            string queryexpression = "";

            try
            {
                result.clear();

                // EasyArchiv-Query initialisieren
                clsQueryClass Weblink = new clsQueryClass();
                Weblink.Configure(taskConfiguration);

                if ((taskConfiguration.AbfrageNachDatum) && (taskConfiguration.Abfragedatum.Year > 1900))
                {
                    queryexpression = ".103=#" + taskConfiguration.Abfragedatum.ToShortDateString();
                }

                // Dokumente aus Archiv holen
                string status = Weblink.QueryArchive(taskConfiguration.easyArchiveNameStandard, queryexpression, ref total_hits, ref result, taskConfiguration);

                if (status == "Keine Daten gefunden.")
                {
                    return;
                }

                if (blnFehlersaetze)
                {
                    #region Filtern auf alle auf Fehler gelaufenen Dateien

                    result.hitList.Columns.Add("found", typeof(Boolean));
                    result.hitList.AcceptChanges();

                    if (logCustomer != null)
                    {
                        foreach (LogFile file in logCustomer)
                        {
                            // Alle Sätze zu einer Fahrgestellnummer finden, da der gesamte Satz neu gezogen werden muss
                            foreach (DataRow row in result.hitList.Rows)
                            {
                                if (row["FAHRGESTELLNUMMER"].ToString() == file.FIN)
                                {
                                    row["found"] = true;
                                }
                            }
                        }
                    }

                    for (int i = 0; i < result.hitList.Rows.Count; i++)
                    {
                        if (result.hitList.Rows[i]["found"] is DBNull)
                        {
                            result.hitList.Rows[i].Delete();
                        }
                    }
                    result.hitList.AcceptChanges();

                    // Fehlerhafte Dateien aus Arbeitsordnern löschen
                    ClearBrokenFiles();

                    #endregion
                }

                // Bilder holen
                for (int i = 0; i < result.hitList.Rows.Count; i++)
                {
                    string strKennzeichen = Helper.verfaelscheKennzeichen(result.hitList.Rows[i]["KENNZEICHEN"].ToString());
                    status = Weblink.QueryPicture(ref result, ref LC, logDS, logCustomer, taskConfiguration, ref logFiles, i, blnFehlersaetze, new[] { strKennzeichen });

                    if (!String.IsNullOrEmpty(status))
                    {
                        Console.WriteLine(status);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EasyExportGeneralTask_" + taskConfiguration.Name + ": Verarbeitung abgebrochen:  " + ex.Message);
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Verarbeitung abgebrochen:  " + ex.Message, EventLogEntryType.Warning);
            }
        }

        /// <summary>
        /// Archivabfrage für XLeasing
        /// </summary>
        private static void QueryXLeasing()
        {
            string queryexpression = "";

            try
            {
                result.clear();

                // EasyArchiv-Query initialisieren
                clsQueryClass Weblink = new clsQueryClass();
                Weblink.Configure(taskConfiguration);

                if ((taskConfiguration.AbfrageNachDatum) && (taskConfiguration.Abfragedatum.Year > 1900))
                {
                    queryexpression = ".103=#" + taskConfiguration.Abfragedatum.ToShortDateString();
                }

                // Dokumente aus Archiv holen
                string status = Weblink.QueryArchive(taskConfiguration.easyArchiveNameStandard, queryexpression, ref total_hits, ref result, taskConfiguration);

                if (status == "Keine Daten gefunden.")
                {
                    return;
                }

                // Bilder holen
                for (int i = 0; i < result.hitList.Rows.Count; i++)
                {
                    status = Weblink.QueryPicture(ref result, ref LC, logDS, logCustomer, taskConfiguration, ref logFiles, i);

                    if (!String.IsNullOrEmpty(status))
                    {
                        Console.WriteLine(status);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EasyExportGeneralTask_" + taskConfiguration.Name + ": Verarbeitung abgebrochen:  " + ex.Message);
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Verarbeitung abgebrochen:  " + ex.Message, EventLogEntryType.Warning);
            }
        }

        /// <summary>
        /// Archivabfrage für Alphabet
        /// </summary>
        private static void QueryAlphabet()
        {
            bool blnErrorOccured = false;

            try
            {
                S.AP.Init("Z_M_EXPORTAENDERUNG_LHS", "I_KUNNR", taskConfiguration.Kundennummer);
                DataTable tblSapResults = S.AP.GetExportTableWithExecute("GT_WEB");

                // EasyArchiv-Query initialisieren
                clsQueryClass Weblink = new clsQueryClass();
                Weblink.Configure(taskConfiguration);

                foreach (DataRow row in tblSapResults.Rows)
                {
                    if (blnErrorOccured)
                    {
                        break;
                    }

                    if (row["EMAIL"] == DBNull.Value)
                    {
                        throw new Exception("Es wurde keine Emailadresse in der Tabelle gefunden (EQUNR=" + row["EQUNR"] + ")");
                    }

                    result.clear();

                    string queryexpression = ".1001=" + row["ZZFAHRG"] + " & .110=" + row["MNCOD"].ToString().Substring(0, 3);

                    string status = Weblink.QueryArchive(taskConfiguration.easyArchiveNameStandard, queryexpression, ref total_hits, ref result, taskConfiguration);

                    // wenn keine Ergebnisse gefunden, nochmal mit anderem titel probieren
                    if (status == "Keine Daten gefunden.")
                    {
                        // abfrage abfeuern nur nochmal mit Brief statt ZBII
                        string titelQuery = "";

                        if (row["MNCOD"].ToString().Substring(0, 3) == "ZB2")
                        {
                            titelQuery = "Fahrzeugbrief";
                        }

                        queryexpression = ".1001=" + row["ZZFAHRG"] + " & .110=" + titelQuery;

                        status = Weblink.QueryArchive(taskConfiguration.easyArchiveNameStandard, queryexpression, ref total_hits, ref result, taskConfiguration);
                    }

                    if (status == "Keine Daten gefunden.")
                    {
                        EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name,
                            "Fehler beim EasyExport (Code 01): " + "Konnte Datei nicht finden. Querystring:" + queryexpression + " Status:" + status, EventLogEntryType.Warning);
                        Helper.SendErrorEMail("Fehler bei " + "EasyExportGeneralTask_" + taskConfiguration.Name,
                            "Fehler beim EasyExport (Code 01): " + "Konnte Datei nicht finden. Querystring:" + queryexpression + " / " + EventLogEntryType.Warning);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(status))
                        {
                            throw new Exception(status);
                        }

                        int iIndex = 0;

                        if (result.hitCounter > 1)
                        {
                            string strDate = "";

                            for (int i = 0; i < result.hitList.Rows.Count; i++)
                            {
                                string datum = result.hitList.Rows[i][2].ToString();

                                if ((String.IsNullOrEmpty(strDate)) || (String.Compare(datum, strDate) > 0))
                                {
                                    strDate = datum;
                                    iIndex = i;
                                }
                            }
                        }

                        status = Weblink.QueryPicture(ref result, ref LC, logDS, logCustomer, taskConfiguration, ref logFiles, iIndex);

                        if (!String.IsNullOrEmpty(status))
                        {
                            Console.WriteLine(status);
                        }

                        if (taskConfiguration.DatumInSapSetzen)
                        {
                            if (!SetActionDate(row["MANUM"].ToString(), row["QMNUM"].ToString()))
                            {
                                blnErrorOccured = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EasyExportGeneralTask_" + taskConfiguration.Name + ": Fehler beim EasyExport (Code 01): " + ex.ToString());
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Fehler beim EasyExport (Code 01): " + ex.ToString(), EventLogEntryType.Warning);
            } 
        }

        /// <summary>
        /// Archivabfrage für LeasePlan
        /// </summary>
        private static void QueryLeasePlan()
        {
            bool blnErrorOccured = false;

            try
            {
                S.AP.Init("Z_M_Exportaenderung_Lp", "I_KUNNR", taskConfiguration.Kundennummer);
                S.AP.SetImportParameter("I_DATUM_VON", DateTime.Today.AddDays(-7));
                S.AP.SetImportParameter("I_DATUM_BIS", DateTime.Today);
                DataTable tblSapResults = S.AP.GetExportTableWithExecute("GT_WEB");

                // EasyArchiv-Query initialisieren
                clsQueryClass Weblink = new clsQueryClass();
                Weblink.Configure(taskConfiguration);

                foreach (DataRow row in tblSapResults.Rows)
                {
                    if (blnErrorOccured)
                    {
                        break;
                    }

                    result.clear();

                    string queryexpression = ".1001=" + row["ZZFAHRG"] + " & .110=" + row["MNCOD"].ToString().Substring(0, 3);

                    string status = Weblink.QueryArchive(taskConfiguration.easyArchiveNameStandard, queryexpression, ref total_hits, ref result, taskConfiguration);

                    if (status == "Keine Daten gefunden.")
                    {
                        EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name,
                            "Fehler beim EasyExport (Code 01): " + "Konnte Datei nicht finden. Querystring:" + queryexpression + " Status:" + status, EventLogEntryType.Warning);
                        Helper.SendErrorEMail("Fehler bei " + "EasyExportGeneralTask_" + taskConfiguration.Name,
                            "Fehler beim EasyExport (Code 01): " + "Konnte Datei nicht finden. Querystring:" + queryexpression + " / " + EventLogEntryType.Warning);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(status))
                        {
                            throw new Exception(status);
                        }

                        int iIndex = 0;

                        if (result.hitCounter > 1)
                        {
                            string strDate = "";
                            
                            for (int i = 0; i < result.hitList.Rows.Count; i++)
                            {
                                string strTmpDat = result.hitList.Rows[i][".ARCHIVDATUM"].ToString();
                                string datum = strTmpDat.Substring(6, 4) + strTmpDat.Substring(3, 2) + strTmpDat.Substring(0, 2);

                                if ((String.IsNullOrEmpty(strDate)) || (String.Compare(datum, strDate) > 0))
                                {
                                    strDate = datum;
                                    iIndex = i;
                                }
                            }
                        }

                        string strLeasingvertragsnummer = row["LIZNR"].ToString();
                        string strSubj = "";

                        switch (row["MNCOD"].ToString())
                        {
                            case "ZB2N":
                                strSubj = "N - " + strLeasingvertragsnummer;
                                break;
                            case "COC":
                                strSubj = "C - " + strLeasingvertragsnummer;
                                break;
                            case "ZB2A":
                                strSubj = "A - " + strLeasingvertragsnummer;
                                break;
                            case "ZB1L":
                                strSubj = "L - " + strLeasingvertragsnummer;
                                break;
                        }

                        status = Weblink.QueryPicture(ref result, ref LC, logDS, logCustomer, taskConfiguration, ref logFiles, iIndex, false, new[] { strSubj });

                        if (!String.IsNullOrEmpty(status))
                        {
                            Console.WriteLine(status);
                        }

                        if (taskConfiguration.DatumInSapSetzen)
                        {
                            if (!SetActionDate(row["MANUM"].ToString(), row["QMNUM"].ToString()))
                            {
                                blnErrorOccured = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EasyExportGeneralTask_" + taskConfiguration.Name + ": Fehler beim EasyExport (Code 01): " + ex.ToString());
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Fehler beim EasyExport (Code 01): " + ex.ToString(), EventLogEntryType.Warning);
            }
        }

        /// <summary>
        /// Archivabfrage für XLCheck
        /// </summary>
        private static void QueryXLCheck()
        {
            try
            {
                S.AP.Init("Z_DPM_READ_AUFTR_BC_001", "I_KUNNR_AG", taskConfiguration.Kundennummer);
                DataTable tblSapResults = S.AP.GetExportTableWithExecute("GT_WEB");

                // EasyArchiv-Query initialisieren
                clsQueryClass Weblink = new clsQueryClass();
                Weblink.Configure(taskConfiguration);

                foreach (DataRow row in tblSapResults.Rows)
                {
                    result.clear();

                    string strTidnr = row["TIDNR"].ToString();

                    string queryexpression = ".1001=" + strTidnr;

                    string status = Weblink.QueryArchive(taskConfiguration.easyArchiveNameStandard, queryexpression, ref total_hits, ref result, taskConfiguration);

                    if (status == "Keine Daten gefunden.")
                    {
                        EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name,
                            "Fehler beim EasyExport (Code 01): " + "Konnte Datei nicht finden. Querystring:" + queryexpression + " Status:" + status, EventLogEntryType.Warning);
                        Helper.SendErrorEMail("Fehler bei " + "EasyExportGeneralTask_" + taskConfiguration.Name,
                            "Fehler beim EasyExport (Code 01): " + "Konnte Datei nicht finden. Querystring:" + queryexpression + " / " + EventLogEntryType.Warning);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(status))
                        {
                            throw new Exception(status);
                        }

                        status = Weblink.QueryPicture(ref result, ref LC, logDS, logCustomer, taskConfiguration, ref logFiles, 0, false, new[] { strTidnr });

                        if (!String.IsNullOrEmpty(status))
                        {
                            Console.WriteLine(status);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EasyExportGeneralTask_" + taskConfiguration.Name + ": Fehler beim EasyExport (Code 01): " + ex.ToString());
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Fehler beim EasyExport (Code 01): " + ex.ToString(), EventLogEntryType.Warning);
            }
        }

        /// <summary>
        /// Archivabfrage für CharterWay_All
        /// </summary>
        /// <param name="blnFehlersaetze"></param>
        private static void QueryCharterWay_All(bool blnFehlersaetze = false)
        {
            string queryexpression = "";

            try
            {
                result.clear();

                // EasyArchiv-Query initialisieren
                clsQueryClass Weblink = new clsQueryClass();
                Weblink.Configure(taskConfiguration);

                if ((taskConfiguration.AbfrageNachDatum) && (taskConfiguration.Abfragedatum.Year > 1900))
                {
                    queryexpression = ".103=#" + taskConfiguration.Abfragedatum.ToShortDateString();
                }

                // Dokumente aus Archiv holen
                string status = Weblink.QueryArchive(taskConfiguration.easyArchiveNameStandard, queryexpression, ref total_hits, ref result, taskConfiguration);

                if (status == "Keine Daten gefunden.")
                {
                    return;
                }

                if (blnFehlersaetze)
                {
                    #region Filtern auf alle auf Fehler gelaufenen Dateien

                    result.hitList.Columns.Add("found", typeof(Boolean));
                    result.hitList.AcceptChanges();

                    if (logCustomer != null)
                    {
                        foreach (LogFile file in logCustomer)
                        {
                            // Alle Sätze zu einer Fahrgestellnummer finden, da der gesamte Satz neu gezogen werden muss
                            foreach (DataRow row in result.hitList.Rows)
                            {
                                if (row["FAHRGESTELLNUMMER"].ToString() == file.FIN)
                                {
                                    row["found"] = true;
                                }
                            }
                        }
                    }

                    for (int i = 0; i < result.hitList.Rows.Count; i++)
                    {
                        if (result.hitList.Rows[i]["found"] is DBNull)
                        {
                            result.hitList.Rows[i].Delete();
                        }
                    }
                    result.hitList.AcceptChanges();

                    // Fehlerhafte Dateien aus Arbeitsordnern löschen
                    ClearBrokenFiles();

                    #endregion
                }

                // Daten zu den FIN der Trefferliste aus SAP holen
                S.AP.Init("Z_M_READ_GRUNDDAT_001", "I_KUNNR_AG", taskConfiguration.Kundennummer);
                S.AP.SetImportParameter("I_EING_DAT_VON", DateTime.Today.AddYears(-3));
                S.AP.SetImportParameter("I_EING_DAT_BIS", DateTime.Today);

                DataTable tblImport = S.AP.GetImportTable("GT_IN");
                foreach (DataRow dRow in result.hitList.Rows)
                {
                    DataRow newRow = tblImport.NewRow();
                    newRow["CHASSIS_NUM"] = dRow["FAHRGESTELLNUMMER"];
                    tblImport.Rows.Add(newRow);
                }

                DataTable tblSapData = S.AP.GetExportTableWithExecute("GT_WEB");

                // Bilder holen
                for (int i = 0; i < result.hitList.Rows.Count; i++)
                {
                    string strKennzeichen = Helper.verfaelscheKennzeichen(result.hitList.Rows[i]["KENNZEICHEN"].ToString());
                    string mvanummer = "";

                    DataRow[] dRows = tblSapData.Select("CHASSIS_NUM='" + result.hitList.Rows[i]["FAHRGESTELLNUMMER"].ToString() + "'");
                    if (dRows.Length > 0)
                    {
                        if (String.IsNullOrEmpty(strKennzeichen))
                        {
                            strKennzeichen = Helper.verfaelscheKennzeichen(dRows[0]["LICENSE_NUM"].ToString());
                        }

                        if ((result.hitList.Rows[i][".TITEL"].ToString() == "FZGBRIEF") && (dRows[0]["EXPIRY_DATE"].ToString() != "00000000"))
                        {
                            result.hitList.Rows[i][".TITEL"] = "FZGBRIEFAB";
                        }

                        mvanummer = dRows[0]["MVA_NUMMER"].ToString();
                    }

                    status = Weblink.QueryPicture(ref result, ref LC, logDS, logCustomer, taskConfiguration, ref logFiles, i, blnFehlersaetze, new[] { strKennzeichen, mvanummer });

                    if (!String.IsNullOrEmpty(status))
                    {
                        Console.WriteLine(status);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EasyExportGeneralTask_" + taskConfiguration.Name + ": Fehler beim EasyExport (Code 01): " + ex.ToString());
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Fehler beim EasyExport (Code 01): " + ex.ToString(), EventLogEntryType.Warning);
            }
        }

        /// <summary>
        /// Archivabfrage für CharterWay_Single
        /// </summary>
        private static void QueryCharterWay_Single()
        {
            bool blnErrorOccured = false;

            try
            {
                var logPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\cwlog_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                using (StreamWriter logWriter = new StreamWriter(logPath, true))
                {
                    S.AP.Init("Z_DPM_EXP_ERSTEINGANG_01", "I_KUNNR_AG", taskConfiguration.Kundennummer);
                    S.AP.SetImportParameter("I_NUR_UNZUGELASSENE_FZG", "X");

                    DataTable tblImport = S.AP.GetImportTable("GT_IN");

                    List<string> werte = new List<string> { "720", "721", "722" };

                    foreach (string wert in werte)
                    {
                        DataRow newRow = tblImport.NewRow();
                        newRow["BAUTL"] = wert.PadLeft(18, '0');
                        tblImport.Rows.Add(newRow);
                    }

                    DataTable tblSapResults = S.AP.GetExportTableWithExecute("GT_OUT");

                    // EasyArchiv-Query initialisieren
                    clsQueryClass Weblink = new clsQueryClass();
                    Weblink.Configure(taskConfiguration);

                    string EasyTyp = "";
                    string ExportTyp = "";

                    foreach (DataRow row in tblSapResults.Rows)
                    {
                        if (blnErrorOccured)
                        {
                            break;
                        }

                        // Datensätze ohne VIN überspringen
                        if (String.IsNullOrEmpty(row["ZZFAHRG"].ToString()))
                        {
                            continue;
                        }

                        string strBautl = row["BAUTL"].ToString().TrimStart('0');

                        logWriter.WriteLine(DateTime.Now.ToString() + " - Verarbeite " + row["ZZFAHRG"].ToString() + ", " + strBautl);

                        if (werte.Contains(strBautl))
                        {
                            switch (strBautl)
                            {
                                case "720":
                                    EasyTyp = "COC";
                                    ExportTyp = "720_COC";
                                    break;

                                case "721":
                                    EasyTyp = "DATENBLATT";
                                    ExportTyp = "721_DATENBLATT";
                                    break;

                                case "722":
                                    EasyTyp = "FZGBRIEF";
                                    ExportTyp = "722_ZBII";
                                    break;
                            }
                        }


                        result.clear();

                        string queryexpression = ".1001=" + row["ZZFAHRG"] + " & .110=" + EasyTyp;

                        string status = Weblink.QueryArchive(taskConfiguration.easyArchiveNameStandard, queryexpression, ref total_hits, ref result, taskConfiguration);

                        logWriter.WriteLine(DateTime.Now.ToString() + " - Abfragestatus für " + row["ZZFAHRG"].ToString() + ", " + strBautl + ": " + status);

                        if (status != "Keine Daten gefunden.")
                        {
                            if (!String.IsNullOrEmpty(status))
                            {
                                throw new Exception(status);
                            }

                            int iIndex = 0;

                            if (result.hitCounter > 1)
                            {
                                string strDate = "";

                                for (int i = 0; i < result.hitList.Rows.Count; i++)
                                {
                                    string datum = result.hitList.Rows[i][4].ToString();

                                    if ((String.IsNullOrEmpty(strDate)) || (String.Compare(datum, strDate) > 0))
                                    {
                                        strDate = datum;
                                        iIndex = i;
                                    }
                                }
                            }

                            status = Weblink.QueryPicture(ref result, ref LC, logDS, logCustomer, taskConfiguration, ref logFiles, iIndex, false, new[] { ExportTyp });

                            logWriter.WriteLine(DateTime.Now.ToString() + " - Dokumentabrufstatus für " + row["ZZFAHRG"].ToString() + ", " + strBautl + ": " + status);

                            if (!String.IsNullOrEmpty(status))
                            {
                                Console.WriteLine(status);
                            }

                            if (taskConfiguration.DatumInSapSetzen)
                            {
                                logWriter.WriteLine(DateTime.Now.ToString() + " - Setze Datum für " + row["ZZFAHRG"].ToString() + ", " + strBautl + " in SAP");

                                if (!SetActionDate(row["MANUM"].ToString(), row["QMNUM"].ToString()))
                                {
                                    blnErrorOccured = true;
                                    logWriter.WriteLine(DateTime.Now.ToString() + " - Datum setzen für " + row["ZZFAHRG"].ToString() + ", " + strBautl + " fehlgeschlagen");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EasyExportGeneralTask_" + taskConfiguration.Name + ": Fehler beim EasyExport (Code 01): " + ex.ToString());
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Fehler beim EasyExport (Code 01): " + ex.ToString(), EventLogEntryType.Warning);
            }
        }

        /// <summary>
        /// Archivabfrage für DCBank
        /// </summary>
        private static void QueryDCBank()
        {
            string queryexpression = "";

            try
            {
                result.clear();

                // EasyArchiv-Query initialisieren
                clsQueryClass Weblink = new clsQueryClass();
                Weblink.Configure(taskConfiguration);

                if ((taskConfiguration.AbfrageNachDatum) && (taskConfiguration.Abfragedatum.Year > 1900))
                {
                    queryexpression = ".103=#" + taskConfiguration.Abfragedatum.ToShortDateString();
                }

                #region Dokumente aus Archiv(en) holen

                // Dokumente aus Archiv holen
                string status = Weblink.QueryArchive(taskConfiguration.easyArchiveNameStandard, queryexpression, ref total_hits, ref result, taskConfiguration);

                if (status == "Keine Daten gefunden.")
                {
                    return;
                }

                #endregion

                // Bilder holen
                for (int i = 0; i < result.hitList.Rows.Count; i++)
                {
                    status = Weblink.QueryPicture(ref result, ref LC, logDS, logCustomer, taskConfiguration, ref logFiles, i);

                    if (!String.IsNullOrEmpty(status))
                    {
                        Console.WriteLine(status);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EasyExportGeneralTask_" + taskConfiguration.Name + ": Verarbeitung abgebrochen:  " + ex.Message);
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Verarbeitung abgebrochen:  " + ex.Message, EventLogEntryType.Warning);
            }
        }

        /// <summary>
        /// Archivabfrage für DaimlerFleet
        /// </summary>
        private static void QueryDaimlerFleet()
        {
            bool blnErrorOccured = false;

            try
            {
                S.AP.Init("Z_DPM_EXP_ABMELDUNGEN_DF_01", "I_KUNNRS", taskConfiguration.Kundennummer);

                if (taskConfiguration.AbfrageNachDatum && taskConfiguration.Abfragedatum.Year > 1900)
                {
                    S.AP.SetImportParameter("I_QMDAT", taskConfiguration.Abfragedatum.ToShortDateString());
                }

                DataTable tblSapResults = S.AP.GetExportTableWithExecute("GT_OUT_JOB");

                // EasyArchiv-Query initialisieren
                clsQueryClass Weblink = new clsQueryClass();
                Weblink.Configure(taskConfiguration);

                foreach (DataRow row in tblSapResults.Rows)
                {
                    if (blnErrorOccured)
                    {
                        break;
                    }

                    result.clear();

                    string queryexpression = ".1001=" + row["CHASSIS_NUM"] + " & .110=ZB1";

                    string status = Weblink.QueryArchive(taskConfiguration.easyArchiveNameStandard, queryexpression, ref total_hits, ref result, taskConfiguration);

                    if (status != "Keine Daten gefunden.")
                    {
                        if (!String.IsNullOrEmpty(status))
                        {
                            throw new Exception(status);
                        }

                        int iIndex = 0;

                        if (result.hitCounter > 1)
                        {
                            string strDate = "";

                            for (int i = 0; i < result.hitList.Rows.Count; i++)
                            {
                                string datum = result.hitList.Rows[i][4].ToString();

                                if ((String.IsNullOrEmpty(strDate)) || (String.Compare(datum, strDate) > 0))
                                {
                                    strDate = datum;
                                    iIndex = i;
                                }
                            }
                        }

                        status = Weblink.QueryPicture(ref result, ref LC, logDS, logCustomer, taskConfiguration, ref logFiles, iIndex);

                        if (!String.IsNullOrEmpty(status))
                        {
                            Console.WriteLine(status);
                        }

                        if (taskConfiguration.DatumInSapSetzen)
                        {
                            if (!SetActionDate("", row["QMNUM"].ToString(), true))
                            {
                                blnErrorOccured = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EasyExportGeneralTask_" + taskConfiguration.Name + ": Verarbeitung abgebrochen:  " + ex.Message);
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Verarbeitung abgebrochen:  " + ex.Message, EventLogEntryType.Warning);
            }
        }

        /// <summary>
        /// Archivabfrage für Sixt Mobility
        /// </summary>
        private static void QuerySixtMobility()
        {
            bool blnErrorOccured = false;

            try
            {
                Z_M_EXPORTAENDERUNG_01.Init(S.AP, "I_KUNNR", taskConfiguration.Kundennummer);

                S.AP.SetImportParameter("I_ZZREFERENZ1", "20");
                S.AP.SetImportParameter("I_DATUM_VON", DateTime.Today.AddDays(-7));
                S.AP.SetImportParameter("I_DATUM_BIS", DateTime.Today);

                var sapResults = Z_M_EXPORTAENDERUNG_01.GT_WEB.GetExportListWithExecute(S.AP);

                // EasyArchiv-Query initialisieren
                clsQueryClass Weblink = new clsQueryClass();
                Weblink.Configure(taskConfiguration);

                foreach (var item in sapResults)
                {
                    if (blnErrorOccured)
                    {
                        break;
                    }

                    result.clear();

                    string queryexpression = String.Format(".1001={0} & .110={1}", item.ZZFAHRG, item.MNCOD.SubstringTry(0, 3));

                    string status = Weblink.QueryArchive(taskConfiguration.easyArchiveNameStandard, queryexpression, ref total_hits, ref result, taskConfiguration);

                    if (status == "Keine Daten gefunden.")
                    {
                        EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name,
                            "Fehler beim EasyExport (Code 01): " + "Konnte Datei nicht finden. Querystring:" + queryexpression + " Status:" + status, EventLogEntryType.Warning);
                        Helper.SendErrorEMail("Fehler bei " + "EasyExportGeneralTask_" + taskConfiguration.Name,
                            "Fehler beim EasyExport (Code 01): " + "Konnte Datei nicht finden. Querystring:" + queryexpression + " / " + EventLogEntryType.Warning);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(status))
                        {
                            throw new Exception(status);
                        }

                        int iIndex = 0;

                        if (result.hitCounter > 1)
                        {
                            string strDate = "";

                            for (int i = 0; i < result.hitList.Rows.Count; i++)
                            {
                                string strTmpDat = result.hitList.Rows[i][".ARCHIVDATUM"].ToString();
                                string datum = strTmpDat.Substring(6, 4) + strTmpDat.Substring(3, 2) + strTmpDat.Substring(0, 2);

                                if ((String.IsNullOrEmpty(strDate)) || (String.Compare(datum, strDate) > 0))
                                {
                                    strDate = datum;
                                    iIndex = i;
                                }
                            }
                        }

                        status = Weblink.QueryPicture(ref result, ref LC, logDS, logCustomer, taskConfiguration, ref logFiles, iIndex, false, new[] { item });

                        if (!String.IsNullOrEmpty(status))
                        {
                            Console.WriteLine(status);
                        }

                        if (taskConfiguration.DatumInSapSetzen)
                        {
                            if (!SetActionDate(item.MANUM, item.QMNUM))
                            {
                                blnErrorOccured = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EasyExportGeneralTask_" + taskConfiguration.Name + ": Fehler beim EasyExport (Code 01): " + ex.ToString());
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Fehler beim EasyExport (Code 01): " + ex.ToString(), EventLogEntryType.Warning);
            }
        }

        /// <summary>
        /// Archivabfrage für Autoinvest
        /// </summary>
        private static void QueryAutoinvest()
        {
            string queryexpression = "";

            try
            {
                result.clear();

                // EasyArchiv-Query initialisieren
                clsQueryClass Weblink = new clsQueryClass();
                Weblink.Configure(taskConfiguration);

                if ((taskConfiguration.AbfrageNachDatum) && (taskConfiguration.Abfragedatum.Year > 1900))
                {
                    queryexpression = ".103=#" + taskConfiguration.Abfragedatum.ToShortDateString();
                }

                // Dokumente aus Archiv holen
                string status = Weblink.QueryArchive(taskConfiguration.easyArchiveNameStandard, queryexpression, ref total_hits, ref result, taskConfiguration);

                if (status == "Keine Daten gefunden.")
                {
                    return;
                }

                // Bilder holen
                for (int i = 0; i < result.hitList.Rows.Count; i++)
                {
                    status = Weblink.QueryPicture(ref result, ref LC, logDS, logCustomer, taskConfiguration, ref logFiles, i);

                    if (!String.IsNullOrEmpty(status))
                    {
                        Console.WriteLine(status);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EasyExportGeneralTask_" + taskConfiguration.Name + ": Verarbeitung abgebrochen:  " + ex.Message);
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Verarbeitung abgebrochen:  " + ex.Message, EventLogEntryType.Warning);
            }
        }

        /// <summary>
        /// Archivabfrage für Europcar
        /// </summary>
        private static void QueryEuropcar()
        {
            bool blnErrorOccured = false;

            try
            {
                S.AP.Init("Z_DPM_AVM_DOKUMENT_MAIL", "I_KUNNR_AG", taskConfiguration.Kundennummer);
                DataTable tblSapResults = S.AP.GetExportTableWithExecute("GT_WEB");

                // EasyArchiv-Query initialisieren
                clsQueryClass Weblink = new clsQueryClass();
                Weblink.Configure(taskConfiguration);

                foreach (DataRow row in tblSapResults.Rows)
                {
                    if (blnErrorOccured)
                    {
                        break;
                    }

                    if (row["EMAIL"] == DBNull.Value)
                    {
                        throw new Exception("Es wurde keine Emailadresse in der Tabelle gefunden (CHASSIS_NUM=" + row["CHASSIS_NUM"] + ")");
                    }

                    result.clear();

                    string queryexpression = ".1001=" + row["CHASSIS_NUM"] + " & .110=" + row["DOK_TYP"].ToString().Substring(0, 3);

                    string status = Weblink.QueryArchive(taskConfiguration.easyArchiveNameStandard, queryexpression, ref total_hits, ref result, taskConfiguration);

                    if (status == "Keine Daten gefunden.")
                    {
                        EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name,
                            "Fehler beim EasyExport (Code 01): " + "Konnte Datei nicht finden. Querystring:" + queryexpression + " Status:" + status, EventLogEntryType.Warning);
                        Helper.SendErrorEMail("Fehler bei " + "EasyExportGeneralTask_" + taskConfiguration.Name,
                            "Fehler beim EasyExport (Code 01): " + "Konnte Datei nicht finden. Querystring:" + queryexpression + " / " + EventLogEntryType.Warning);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(status))
                        {
                            throw new Exception(status);
                        }

                        int iIndex = 0;

                        if (result.hitCounter > 1)
                        {
                            string strDate = "";

                            for (int i = 0; i < result.hitList.Rows.Count; i++)
                            {
                                string datum = result.hitList.Rows[i][2].ToString();

                                if ((String.IsNullOrEmpty(strDate)) || (String.Compare(datum, strDate) > 0))
                                {
                                    strDate = datum;
                                    iIndex = i;
                                }
                            }
                        }

                        status = Weblink.QueryPicture(ref result, ref LC, logDS, logCustomer, taskConfiguration, ref logFiles, iIndex);

                        if (!String.IsNullOrEmpty(status))
                        {
                            Console.WriteLine(status);
                        }

                        if (taskConfiguration.DatumInSapSetzen)
                        {
                            S.AP.InitExecute("Z_DPM_AVM_DOKUMENT_MAIL", "I_KUNNR_AG, I_CHASSIS_NUM", taskConfiguration.Kundennummer, row["CHASSIS_NUM"].ToString());

                            if (S.AP.ResultCode != 0)
                            {
                                blnErrorOccured = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EasyExportGeneralTask_" + taskConfiguration.Name + ": Fehler beim EasyExport (Code 01): " + ex.ToString());
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Fehler beim EasyExport (Code 01): " + ex.ToString(), EventLogEntryType.Warning);
            }
        }

        /// <summary>
        /// Übermittlungsdatum setzen
        /// </summary>
        /// <param name="strManum"></param>
        /// <param name="strQmnum"></param>
        /// <param name="qmel"></param>
        /// <returns></returns>
        private static bool SetActionDate(string strManum, string strQmnum, bool qmel = false)
        {
            try
            {
                if (qmel)
                {
                    S.AP.Init("Z_M_Uebermittlungsdatum_Mel_Lp");
                }
                else
                {
                    S.AP.Init("Z_M_Uebermittlungsdatum_Lp");

                    S.AP.SetImportParameter("I_KUNNR", taskConfiguration.Kundennummer);
                    S.AP.SetImportParameter("I_MANUM", strManum);
                }

                S.AP.SetImportParameter("I_QMNUM", strQmnum);
                S.AP.SetImportParameter("I_ZZUEBER", DateTime.Today);

                S.AP.Execute();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Erzeugt eine JPL-Datei mit Hilfe der angegebenen Parameter
        /// </summary>
        /// <param name="FilePraefix"></param>
        /// <param name="FileIdent"></param>
        /// <param name="Vertragsnummer"></param>
        /// <param name="Kundennummer"></param>
        /// <param name="Kennzeichen"></param>
        /// <param name="dokumentDatum"></param>
        private static void generateJPLFile(string FilePraefix, string FileIdent, string Vertragsnummer, string Kundennummer, string Kennzeichen, DateTime dokumentDatum)
        {
            string FilePath = "";

            switch (FilePraefix)
            {
                case "ZB":
                    FilePath = taskConfiguration.exportPathZBII;
                    break;
                case "S":
                    FilePath = taskConfiguration.exportPathSteuerB;
                    break;
            }

            StreamWriter objWriter = new StreamWriter(FilePath + "\\" + FilePraefix + "_" + FileIdent + ".jpl");

            objWriter.WriteLine("dokuart=\"DKFZ\"");
            objWriter.WriteLine("logi_verzeichnis=\"Fr\"");
            objWriter.WriteLine("dok_dat_feld[50]=\"" + dokumentDatum.ToShortDateString() + "\"");
            objWriter.WriteLine("dok_dat_feld[80]=\"" + Vertragsnummer + "\"");
            objWriter.WriteLine("dok_dat_feld[5]=\"" + Helper.verfaelscheKennzeichen(Kennzeichen) + "\"");
            objWriter.WriteLine("dok_dat_feld[81]=\"" + Kundennummer + "\"");

            switch (FilePraefix)
            {
                case "ZB":
                    objWriter.WriteLine("text[1]=\"ZBII\"");
                    break;
                case "S":
                    objWriter.WriteLine("text[1]=\"Steuer\"");
                    break;
            }

            objWriter.Close();
        }

        /// <summary>
        /// Löscht alle beschädigten Dateien in den Arbeitsverzeichnissen
        /// </summary>
        private static void ClearBrokenFiles()
        {
            string[] Filelist;
            List<string> Deletelist = new List<string>();

            // Dateien zum clearen im allg. Exportverzeichnis finden
            if (!String.IsNullOrEmpty(taskConfiguration.easyBlobPathLocal))
            {
                Filelist = Directory.GetFiles(taskConfiguration.easyBlobPathLocal);

                for (int i = 0; i < Filelist.GetLength(0); i++)
                {
                    FileInfo file = new FileInfo(Filelist[i]);
                    if (file.Length < 500)
                    {
                        Deletelist.Add(Filelist[i]);
                    }
                }
            }

            // ZBII zum clearen finden
            if (!String.IsNullOrEmpty(taskConfiguration.exportPathZBII))
            {
                Filelist = Directory.GetFiles(taskConfiguration.exportPathZBII);

                for (int i = 0; i < Filelist.GetLength(0); i++)
                {
                    FileInfo file = new FileInfo(Filelist[i]);
                    if ((file.Extension.ToLower() == ".pdf") && (file.Length < 500))
                    {
                        Deletelist.Add(Filelist[i]);
                    }
                }
            }

            // Steuerbescheide zum clearen finden
            if (!String.IsNullOrEmpty(taskConfiguration.exportPathSteuerB))
            {
                Filelist = Directory.GetFiles(taskConfiguration.exportPathSteuerB);

                for (int i = 0; i < Filelist.GetLength(0); i++)
                {
                    FileInfo file = new FileInfo(Filelist[i]);
                    if (file.Length < 500)
                    {
                        Deletelist.Add(Filelist[i]);
                    }
                }
            }

            // gefundene Dateien löschen
            bool blDelError = false;

            foreach (string DelFile in Deletelist)
            {
                try
                {
                    File.Delete(DelFile);
                }
                catch
                {
                    blDelError = true;
                }
            }

            if (blDelError)
            {
                EventLog.WriteEntry("EasyExportGeneralTask_" + taskConfiguration.Name, "Es konnte nicht alle unbrauchbaren Dateien aus dem Verzeichnis " + taskConfiguration.easyBlobPathLocal + " gelöscht werden.", EventLogEntryType.Warning);
            }
        }
    }
}
