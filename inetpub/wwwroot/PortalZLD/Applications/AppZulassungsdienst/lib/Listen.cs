﻿using System;
using System.Data;
using CKG.Base.Business;
using SapORM.Contracts;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
    /// <summary>
    /// Klasse für verschiedene Reports (Tagesliste, Prägeliste, Lieferscheine, Auswertung)
    /// </summary>
    public class Listen: SapOrmBusinessBase
    {
        #region "Properties"

        public DataTable TagesListe { get; private set; }
        public DataTable KopfListe { get; private set; }
        public DataTable BemListe { get; private set; }
        public DataTable PraegListeKopf { get; private set; }
        public DataTable Lieferscheine { get; private set; }
        public DataTable Auswertung { get; private set; }
        public DataTable ZLDAdresseTagli { get; private set; }

        public String KennzeichenVon { get; set; }
        public String KennzeichenBis { get; set; }
        public String KundeVon { get; set; }
        public String KundeBis { get; set; }
        public String Zuldat { get; set; }
        public String Delta { get; set; }
        public String Gesamt { get; set; }
        public String SelMatnr { get; set; }
        public String SelDatum { get; set; }
        public String SelDatumBis { get; set; }
        public String SelID { get; set; }
        public String SelKunde { get; set; }
        public String SelKennz { get; set; }
        public String SelRef1 { get; set; }
        public String SelStvA { get; set; }
        public String SelZahlart { get; set; }
        public String SelGroupTourID { get; set; }
        public String alleDaten { get; set; }
        public String Abgerechnet { get; set; }
        public String NochNichtDurchgefuehrt { get; set; }
        public String nichtAbgerechnet { get; set; }
        public String Filialname { get; set; }
        public String ZuldatBis { get; set; }
        public String Sortierung { get; set; }
        public String Filename { get; set; }
        
        public byte[] pdfTagesliste { get; private set; }
        public byte[] pdfPraegeliste { get; private set; }

        #endregion

        public Listen(string userReferenz)
        {
            VKORG = ZLDCommon.GetVkOrgFromUserReference(userReferenz);
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz);
        }

        public void Fill()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_EXPORT_TAGLI.Init(SAP);

                    SAP.SetImportParameter("I_VKBUR", VKBUR);

                    if (!String.IsNullOrEmpty(KundeVon))
                        SAP.SetImportParameter("I_KUNNR_VON", KundeVon.ToSapKunnr());

                    if (!String.IsNullOrEmpty(KundeBis))
                        SAP.SetImportParameter("I_KUNNR_BIS", KundeBis.ToSapKunnr());

                    SAP.SetImportParameter("I_KREISKZ_VON", KennzeichenVon);

                    SAP.SetImportParameter("I_KREISKZ_BIS", (String.IsNullOrEmpty(KennzeichenBis) ? KennzeichenVon : KennzeichenBis));

                    SAP.SetImportParameter("I_ZZZLDAT", Zuldat);

                    if (!String.IsNullOrEmpty(Delta))
                        SAP.SetImportParameter("I_ZDELTA", Delta);

                    if (!String.IsNullOrEmpty(Gesamt))
                        SAP.SetImportParameter("I_ZGESAMT", Gesamt);

                    SAP.SetImportParameter("I_AUSGABE", "S"); //XSTRING-PDF-Ausgabe

                    CallBapi();

                    Filename = SAP.GetExportParameter("E_FILENAME");

                    Filialname = SAP.GetExportParameter("E_KTEXT");
                    KopfListe = SAP.GetExportTable("GT_TAGLI_K");
                    TagesListe = SAP.GetExportTable("GT_TAGLI_P");
                    BemListe = SAP.GetExportTable("GT_TAGLI_BEM");
                    ZLDAdresseTagli = SAP.GetExportTable("ES_FIL_ADRS");
                    pdfTagesliste = SAP.GetExportParameterByte("E_PDF");
                });
        }

        public void FillPraegeliste()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_EXPORT_PRALI.Init(SAP);

                    SAP.SetImportParameter("I_VKBUR", VKBUR);
                    SAP.SetImportParameter("I_KREISKZ_VON", KennzeichenVon);

                    SAP.SetImportParameter("I_KREISKZ_BIS", (String.IsNullOrEmpty(KennzeichenBis) ? KennzeichenVon : KennzeichenBis));

                    SAP.SetImportParameter("I_ZZZLDAT", Zuldat);

                    if (!String.IsNullOrEmpty(Delta))
                        SAP.SetImportParameter("I_ZDELTA", Delta);

                    if (!String.IsNullOrEmpty(Gesamt))
                        SAP.SetImportParameter("I_ZGESAMT", Gesamt);

                    SAP.SetImportParameter("I_SORT", Sortierung);
                    SAP.SetImportParameter("I_AUSGABE", "S"); //XSTRING-PDF-Ausgabe

                    CallBapi();

                    Filename = SAP.GetExportParameter("E_FILENAME");
                    PraegListeKopf = SAP.GetExportTable("GT_BELEG");
                    pdfPraegeliste = SAP.GetExportParameterByte("E_PDF");
                });
        }

        public void setPraliPrint()
        {
            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection();
            try
            {
                connection.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["Connectionstring"];
                connection.Open();
                System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "alter table dbo.ZLDKopfTabelle " +
                                      "disable trigger WriteTS";
                command.ExecuteNonQuery();

                DateTime tmpDate;

                DateTime.TryParse(Zuldat, out tmpDate);

                foreach (DataRow drow in PraegListeKopf.Rows) 
                {
                    String query = "id_sap = " + drow["ZULBELN"].ToString().TrimStart('0');

                    String str = "Update ZLDKopfTabelle " +
                     "Set Prali_Print= 1 " +
                     " Where " + query;

                    command = new System.Data.SqlClient.SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = str;
                    command.ExecuteNonQuery();
                }

                command = new System.Data.SqlClient.SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "alter table dbo.ZLDKopfTabelle " +
                                      "enable trigger WriteTS";
                command.ExecuteNonQuery();
            }
            finally { connection.Close(); }
        }

        public void FillLieferschein(string Liefart)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_EXPORT_LS.Init(SAP);

                    SAP.SetImportParameter("I_VKBUR", VKBUR);

                    SAP.SetImportParameter("I_KUNNR_VON", KundeVon.ToSapKunnr());

                    if (!String.IsNullOrEmpty(KundeBis))
                        SAP.SetImportParameter("I_KUNNR_BIS", KundeBis.ToSapKunnr());

                    if (!String.IsNullOrEmpty(KennzeichenVon))
                        SAP.SetImportParameter("I_KREISKZ_VON", KennzeichenVon);

                    if (!String.IsNullOrEmpty(KennzeichenBis))
                        SAP.SetImportParameter("I_KREISKZ_BIS", KennzeichenBis);

                    SAP.SetImportParameter("I_ZZZLDAT_VON", Zuldat);

                    if (!String.IsNullOrEmpty(ZuldatBis))
                        SAP.SetImportParameter("I_ZZZLDAT_BIS", ZuldatBis);

                    if (!String.IsNullOrEmpty(SelGroupTourID))
                        SAP.SetImportParameter("I_GRUPPE", SelGroupTourID.PadLeft0(10));

                    SAP.SetImportParameter("I_LS", Liefart);

                    CallBapi();

                    Lieferscheine = SAP.GetExportTable("GT_FILENAME");
                });
        }

        public void FillAuswertungNeu()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_EXPORT_AUSWERTUNG_1.Init(SAP);

                    if (!String.IsNullOrEmpty(SelKunde))
                        SAP.SetImportParameter("I_KUNNR", SelKunde.ToSapKunnr());

                    SAP.SetImportParameter("I_VKORG", VKORG);
                    SAP.SetImportParameter("I_VKBUR", VKBUR);
                    SAP.SetImportParameter("I_ZZZLDAT_VON", SelDatum);
                    SAP.SetImportParameter("I_ZZZLDAT_BIS", SelDatumBis);
                    SAP.SetImportParameter("I_KREISKZ", SelStvA);

                    if (!String.IsNullOrEmpty(SelID))
                        SAP.SetImportParameter("I_ZULBELN", SelID.PadLeft0(10));

                    SAP.SetImportParameter("I_ZZREFNR1", SelRef1);
                    SAP.SetImportParameter("I_ZZKENN", SelKennz);

                    if (!String.IsNullOrEmpty(SelMatnr))
                        SAP.SetImportParameter("I_MATNR", SelMatnr.PadLeft0(18));

                    SAP.SetImportParameter("I_ABRKZ", Abgerechnet);
                    SAP.SetImportParameter("I_NNDGF", NochNichtDurchgefuehrt);
                    SAP.SetImportParameter("I_LOEKZ", "");
                    SAP.SetImportParameter("I_ZAHLART", SelZahlart);

                    if (!String.IsNullOrEmpty(SelGroupTourID))
                        SAP.SetImportParameter("I_GRUPPE", SelGroupTourID.PadLeft0(10));

                    CallBapi();

                    Auswertung = SAP.GetExportTable("GT_LISTE1");

                    foreach (DataRow dRow in Auswertung.Rows)
                    {
                        // Führende Nullen für die Anzeige entfernen
                        if (dRow["ZULBELN"] != null)
                        {
                            dRow["ZULBELN"] = dRow["ZULBELN"].ToString().TrimStart('0');
                        }
                        if (dRow["KUNNR"] != null)
                        {
                            dRow["KUNNR"] = dRow["KUNNR"].ToString().TrimStart('0');
                        }
                    }
                });
        }

        /// <summary>
        /// Nachdruck der Aufträge. Aufträge werden von SAP in einem Verzeichnis abgelegt. 
        /// Barcode und EasyID kommen aus den Auswertungstabellen. Bapi: Z_ZLD_GET_BARQ_FROM_EASY
        /// </summary>
        /// <param name="BarqNr">Barcode</param>
        /// <param name="EasyID">EasyID</param>
        public void GetBarqFromEasy(string BarqNr, string EasyID)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_GET_BARQ_FROM_EASY.Init(SAP, "I_BARQ_NR, I_OBJECT_ID", BarqNr, EasyID);

                    CallBapi();

                    Filename = SAP.GetExportParameter("E_FILENAME");
                });
        }
    }
}
