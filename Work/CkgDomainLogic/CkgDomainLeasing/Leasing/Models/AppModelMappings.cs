﻿using System;
using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Leasing.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region FromSap

        static public ModelMapping<Z_DPM_READ_SCHEINKOPIEN.GT_OUT, ZB1Kopie> Z_DPM_READ_SCHEINKOPIEN_GT_OUT_To_ZB1Kopie
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_SCHEINKOPIEN.GT_OUT, ZB1Kopie>(
                    new Dictionary<string, string> {
                        { "KUNNR", "Kundennummer" },
                        { "ERDAT", "Erstelldatum" },
                        { "LIZNR", "Vertragsnummer" },
                        { "LICENSE_NUM", "Kennzeichen" },
                        { "CHASSIS_NUM", "Fahrgestellnummer" },
                        { "REPLA_DATE", "Zulassungsdatum" },
                        { "HALTERNAME", "Haltername" },
                        { "KOPIE_FLG", "ZB1KopieVorhanden" },
                    }));
            }
        }

        static public ModelMapping<Z_M_Unzugelassene_Fzge_Sixt_L.T_DATA, UnzugelFzg> Z_M_Unzugelassene_Fzge_Sixt_L_T_DATA_To_UnzugelFzg
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_Unzugelassene_Fzge_Sixt_L.T_DATA, UnzugelFzg>(
                    new Dictionary<string, string> {
                        { "EQUNR", "Equipmentnummer" },
                        { "ZBRIEFEINGANG", "Briefeingang" },
                        { "CHASSIS_NUM", "Fahrgestellnummer" },
                        { "NAME1_ZP", "Haendlername" },
                        { "ORT01_ZP", "Haendlerort" },
                        { "ZZLVNR", "Leasingvertragsnummer" },
                        { "TIDNR", "Briefnummer" },
                    }));
            }
        }

        static public ModelMapping<Z_M_Abm_Fehl_Unterl_Sixt_Leas.AUSGABE, Abmeldedaten> Z_M_Abm_Fehl_Unterl_Sixt_Leas_AUSGABE_To_Abmeldedaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_Abm_Fehl_Unterl_Sixt_Leas.AUSGABE, Abmeldedaten>(
                    new Dictionary<string, string> {
                        { "ZZKENN", "Kennzeichen" },
                        { "LIZNR", "Leasingvertragsnummer" },
                        { "TIDNR", "Briefnummer" },
                        { "CHASSIS_NUM", "Fahrgestellnummer" },
                        { "KZINDAT", "Abmeldeeingang" },
                    }));
            }
        }

        static public ModelMapping<Z_M_ABMELDUNG_SIXT_LEASING.I_AUF, Abmeldedaten> Z_M_ABMELDUNG_SIXT_LEASING_I_AUF_To_Abmeldedaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_ABMELDUNG_SIXT_LEASING.I_AUF, Abmeldedaten>(
                    new Dictionary<string, string> {
                        { "ZZKENN", "Kennzeichen" },
                        { "LIZNR", "Leasingvertragsnummer" },
                        { "TIDNR", "Briefnummer" },
                        { "CHASSIS_NUM", "Fahrgestellnummer" },
                        { "KZINDAT", "Abmeldeeingang" },
                        { "FREIGABE", "Freigabe" },
                        { "FEHL_AUF_ANL", "Fehler" },
                    }));
            }
        }

        static public ModelMapping<Z_DPM_SIXT_PG_KLAERFALL.GT_WEB, Klaerfall> Z_DPM_SIXT_PG_KLAERFALL_GT_WEB_To_Klaerfall
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_SIXT_PG_KLAERFALL.GT_WEB, Klaerfall>(
                    new Dictionary<string, string> {
                        { "KUNUM", "Kundennummer" },
                        { "KUNDE", "Kundenname" },
                        { "LVTNR", "Leasingvertragsnummer" },
                        { "ANFDA", "Erstanschreiben" },
                        { "MAHNDAT1", "Mahnung1" },
                        { "MAHNDAT2", "Mahnung2" },
                        { "MAHNDAT3", "Mahnung3" },
                        { "EINGANG_VM", "EingangVollmacht" },
                        { "EINGANG_HR", "EingangHandelsregister" },
                        { "EINGANG_GA", "EingangGewerbeanmeldung" },
                        { "EINGANG_PA", "EingangPersonalausweis" },
                        { "EINGANG_EE", "EingangEinzugsermaechtigung" },
                        { "EVBNR", "EVB" },
                        { "EINGANG_SS", "EingangSicherungsschein" },
                        { "BEMERKUNG", "Bemerkung" },
                        { "NAME1", "Name1" },
                        { "NAME2", "Name2" },
                        { "NAME3", "Name3" },
                        { "STRASSE", "Strasse" },
                        { "PLZ", "Plz" },
                        { "ORT", "Ort" },
                        { "EMAIL", "Email" },
                        { "EMAIL2", "Email2" },
                        { "LIEFERTERMIN", "Liefertermin" },
                        { "KUNDENBETREUER", "Kundenbetreuer" },
                        { "HERSTELLER", "Hersteller" },
                        { "FZG_TYP", "Fahrzeugtyp" },
                        { "WUNSCHDATUM", "Wunschdatum" },
                        { "DAT_ANNAHMEDOK", "DruckdatumAnschreiben" },
                        { "STATUS", "Status" },
                        { "STEUER_ABR_KZ", "SteuerAbrechnen" },
                        { "ZUL_AUF", "ZulassungAuf" },
                        { "ZUL_DURCH", "ZulassungDurch" },
                        { "BESTELLART", "Bestellart" },
                        { "ZULDAT", "Zulassungsdatum" },
                        { "KOMPLETT", "UnterlagenKomplett" },
                    }));
            }
        }

        static public ModelMapping<Z_DPM_SIS_BESTAND.GT_WEB, Sicherungsschein> Z_DPM_SIS_BESTAND_GT_WEB_To_Sicherungsschein
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_SIS_BESTAND.GT_WEB, Sicherungsschein>(
                    new Dictionary<string, string> {
                        { "KUNNR", "Kundennummer" },
                        { "LIZNR", "Vertragsnummer" },
                        { "CHASSIS_NUM", "Fahrgestellnummer" },
                        { "LICENSE_NUM", "Kennzeichen" },
                        { "STORT", "Standortcode" },
                        { "STANDORT", "Standort" },
                        { "NAME1", "LN_Name" },
                        { "STREET", "LN_Strasse" },
                        { "POST_CODE1", "LN_Plz" },
                        { "CITY1", "LN_Ort" },
                        { "ZZMAHNS", "Mahnstufe" },
                        { "ZZLABEL", "Klaerfallcode" },
                        { "ZKLAERTEXT", "Klaerfalltext" },
                        { "REPLA_DATE", "Zulassungsdatum" },
                        { "KONZS", "Kundenreferenz" },
                        { "ZZREFERENZ1", "Konzernnummer" },
                    }));
            }
        }

        static public ModelMapping<Z_M_KLAERFAELLEVW.GT_WEB, NichtDurchfuehrbareZulassung> Z_M_KLAERFAELLEVW_GT_WEB_To_NichtDurchfuehrbareZulassung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_KLAERFAELLEVW.GT_WEB, NichtDurchfuehrbareZulassung>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.AuftragsNr = s.VBELN;
                        d.Belegdatum = s.AUDAT;
                        d.FahrgestellNr = s.ZZFAHRG;
                        d.Kennzeichen = s.ZZKENN;
                        d.KlaerfallTextZeile1 = s.TDLINE1;
                        d.KlaerfallTextZeile2 = s.TDLINE2;
                        d.KlaerfallTextZeile3 = s.TDLINE3;
                        d.KlaerfallTextZeile4 = s.TDLINE4;
                        d.KlaerfallTextZeile5 = s.TDLINE5;
                        d.KundenNr = s.KUNNR;
                        d.ReferenzNr = s.ZZREFNR;
                    }
                ));
            }
        }

        static public ModelMapping<Z_M_FAELLIGE_EQUI_LP.GT_WEB, UeberfaelligeRuecksendung> Z_M_FAELLIGE_EQUI_LP_GT_WEB_To_UeberfaelligeRuecksendung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_FAELLIGE_EQUI_LP.GT_WEB, UeberfaelligeRuecksendung>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Anforderer = s.IHREZ;
                        d.EquiNr = s.EQUNR;
                        d.FaelligkeitsDatum = s.ZZFAEDT;
                        d.FahrgestellNr = s.CHASSIS_NUM;
                        d.Kennzeichen = s.LICENSE_NUM;
                        d.LeasingvertragsNr = s.LIZNR;
                        d.Mahnstufe = s.ZZMAHNS;
                        d.Memo = s.TEXT200;
                        d.SuchnameKunde = s.SORTL;
                        d.VersandDatum = s.ZZTMPDT;
                        d.Versandgrund = s.ZZVGRUND_TEXT;
                        d.Vertriebseinheit = s.ZZLABEL;
                        d.Zb2Nr = s.TIDNR;
                    }
                ));
            }
        }

        #endregion


        #region ToSap

        static public ModelMapping<Z_M_ABMELDUNG_SIXT_LEASING.I_AUF, Abmeldedaten> Z_M_ABMELDUNG_SIXT_LEASING_I_AUF_From_Abmeldedaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_ABMELDUNG_SIXT_LEASING.I_AUF, Abmeldedaten>(
                    new Dictionary<string, string> {
                        { "ZZKENN", "Kennzeichen" },
                        { "LIZNR", "Leasingvertragsnummer" },
                        { "TIDNR", "Briefnummer" },
                        { "CHASSIS_NUM", "Fahrgestellnummer" },
                        { "KZINDAT", "Abmeldeeingang" },
                        { "FREIGABE", "Freigabe" },
                    }));
            }
        }

        static public void MapCarGateLeasingCsvUploadEntityToSAP(LeasingCargateCsvUploadModel source, Z_DPM_SAVE_DAT_IN_RUECKL_01.GT_IN destination)
        {
            string nodatetime = string.Empty;
            destination.CHASSIS_NUM = source.Fin;

            destination.STANDORT = source.Standort;


            if (source.EingangFahrzeugBlg != nodatetime)
            {
                var valueToParse = source.EingangFahrzeugBlg.Replace(" 00:00:00", string.Empty);
                destination.EING_FZG_BLG = DateTime.Parse(valueToParse);
            }

            if (source.BereitstellungFahrzeugBlg != nodatetime)
            {
                var valueToParse = source.BereitstellungFahrzeugBlg.Replace(" 00:00:00", string.Empty);
                destination.FZG_BEREIT_BLG_SGS = DateTime.Parse(valueToParse);    
            }

            if (source.FertigmeldungAufbereitungFahrzeugBlg != nodatetime)
            {
                var valueToParse = source.FertigmeldungAufbereitungFahrzeugBlg.Replace(" 00:00:00", string.Empty);
                destination.AUFBER_FERTIG = DateTime.Parse(valueToParse);    
            }
        }

        #endregion
    }
}