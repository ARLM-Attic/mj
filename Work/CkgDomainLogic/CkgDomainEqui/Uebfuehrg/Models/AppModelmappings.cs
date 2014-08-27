﻿using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region Load from Repository

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_M_IMP_AUFTRDAT_007.GT_WEB, Adresse> Z_M_IMP_AUFTRDAT_007_GT_WEB_To_Adresse
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_IMP_AUFTRDAT_007.GT_WEB, Adresse>(
                    new Dictionary<string, string> {
                        { "MANDT", "Mandant" },
                        { "KUNNR", "KundenNr" },
                        { "NAME1", "Name1" },
                        { "NAME2", "Ansprechpartner" },
                        { "STRAS", "Strasse" },
                        { "PSTLZ", "PLZ" },
                        { "ORT01", "Ort" },
                        { "TELNR", "Telefon" },
                        { "EMAIL", "Email" },
                        { "FAXNR", "Fax" },
                        { "KENNUNG", "SubTyp" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_M_PARTNER_AUS_KNVP_LESEN.AUSGABE, Adresse> Z_M_PARTNER_AUS_KNVP_LESEN_AUSGABE_To_Adresse
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_PARTNER_AUS_KNVP_LESEN.AUSGABE, Adresse>(
                    new Dictionary<string, string> {
                        { "KUNNR", "KundenNr" },
                        { "NAME1", "Name1" },
                        { "NAME2", "Ansprechpartner" },
                        //{ "NICK_NAME", "NickName" },
                        { "STREET", "Strasse" },
                        { "HOUSE_NUM1", "HausNr" },
                        { "POST_CODE1", "PLZ" },
                        { "CITY1", "Ort" },
                        { "TEL_NUMBER", "Telefon" },
                        { "TELFX", "Fax" },
                        { "PARVW", "SubTyp" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_READ_LV_001.GT_OUT_DL, TransportTyp> Z_DPM_READ_LV_001_GT_OUT_DL_To_TransportTyp
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_LV_001.GT_OUT_DL, TransportTyp>(
                    new Dictionary<string, string> {
                        { "EXTGROUP", "ID" },
                        { "KTEXT1_H1", "Name" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_READ_LV_001.GT_OUT_DL, Dienstleistung> Z_DPM_READ_LV_001_GT_OUT_DL_To_Dienstleistung
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_LV_001.GT_OUT_DL, Dienstleistung>(
                    new Dictionary<string, string> {
                        { "EXTGROUP", "TransportTyp" },
                        { "ASNUM", "DienstleistungsID" },
                        { "ASKTX", "Name" },
                        { "TBTWR", "Preis" },
                        { "EAN11", "MaterialNummer" },
                        { "VW_AG", "SapFlagVwAG" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Adresse, Adresse> Adresse_To_Adresse
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Adresse, Adresse>(
                    new Dictionary<string, string> {
                        { "Name1", "Name1" },
                        { "Name2", "Name2" },
                        { "Strasse", "Strasse" },
                        { "PLZ", "PLZ" },
                        { "Ort", "Ort" },
                        { "Land", "Land" },
                        { "Ansprechpartner", "Ansprechpartner" },
                        { "Telefon", "Telefon" },
                        { "Fax", "Fax" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        public static ModelMapping<Z_UEB_CREATE_ORDER_01.GT_RET, UeberfuehrungsAuftragsPosition> Z_UEB_CREATE_ORDER_01_GT_RET_To_UeberfuehrungsAuftrag
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_UEB_CREATE_ORDER_01.GT_RET, UeberfuehrungsAuftragsPosition>(
                    new Dictionary<string, string> {
                        { "VBELN", "AuftragsNr" },
                        { "FAHRT", "FahrtIndex" },
                        { "BEMERKUNG", "Bemerkung" },
                    }));
            }
        }

        #endregion


        #region Save to Repository

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_UEB_CREATE_ORDER_01.GT_FAHRTEN, Fahrt> Z_UEB_CREATE_ORDER_01_GT_FAHRTEN_To_Fahrt
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_UEB_CREATE_ORDER_01.GT_FAHRTEN, Fahrt>(
                    new Dictionary<string, string> {
                        { "REIHENFOLGE", "ReihenfolgeTmp" },
                        { "FAHRT", "FahrtIndex" },
                        { "FAHRZEUG", "FahrzeugIndex" },
                        { "VORGANG", "VorgangsNummer" },
                        { "TRANSPORTTYP", "TypName" },
                        { "TRANSPORTTYPNR", "TypNr" },
                        { "VDATU", "Datum" },
                        { "AT_TIM_VON", "Uhrzeit" },
                        { "AT_TIM_BIS", "EmptyString" },
                        { "KENNZ_ZUS_FAHT", "EmptyString" },
                    },
                    // Copy from SAP
                    (sap, business) => { },
                    // Copy to SAP
                    (business, sap) =>
                    {
                        if (sap.AT_TIM_VON.IsNotNullOrEmpty())
                            sap.AT_TIM_VON = sap.AT_TIM_VON.Replace(":", "") + "00";
                        if (sap.AT_TIM_BIS.IsNotNullOrEmpty())
                            sap.AT_TIM_BIS = sap.AT_TIM_BIS.Replace(":", "") + "00";
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_UEB_CREATE_ORDER_01.GT_ADRESSEN, Adresse> Z_UEB_CREATE_ORDER_01_GT_ADRESSEN_To_Adresse
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_UEB_CREATE_ORDER_01.GT_ADRESSEN, Adresse>(
                    new Dictionary<string, string> {
                        { "FAHRT", "FahrtIndexAktuellTmp" },
                        { "PARTN_NUMB", "KundenNr" },
                        { "NAME", "Name1" },
                        { "NAME_2", "Ansprechpartner" },
                        { "STREET", "Strasse" },
                        { "POSTL_CODE", "PLZ" },
                        { "CITY", "Ort" },
                        { "TELEPHONE", "Telefon" },
                        { "COUNTRY", "Land" },
                        { "SMTP_ADDR", "Email" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        public static ModelMapping<Z_UEB_CREATE_ORDER_01.GT_BEM, KurzBemerkung> Z_UEB_CREATE_ORDER_01_GT_BEM_To_KurzBemerkung
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_UEB_CREATE_ORDER_01.GT_BEM, KurzBemerkung>(
                    new Dictionary<string, string> {
                        { "FAHRT", "GroupName" },
                        { "TEXT_ID", "ID" },
                        { "BEMERKUNG", "Bemerkung" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_UEB_CREATE_ORDER_01.GT_DIENSTLSTGN, Dienstleistung> Z_UEB_CREATE_ORDER_01_GT_DIENSTLSTGN_To_Dienstleistung
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_UEB_CREATE_ORDER_01.GT_DIENSTLSTGN, Dienstleistung>(
                    new Dictionary<string, string> {
                        { "FAHRT", "FahrtIndex" },
                        { "DIENSTL_NR", "DienstleistungsID" },
                        { "DIENSTL_TEXT", "Name" },
                        { "MATNR", "MaterialNummer" },
                        { "FLAG_TEXT", "MaterialNummerConverted" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_UEB_CREATE_ORDER_01.GT_FZG, Fahrzeug> Z_UEB_CREATE_ORDER_01_GT_FZG_To_Fahrzeug
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_UEB_CREATE_ORDER_01.GT_FZG, Fahrzeug>(
                    new Dictionary<string, string> {
                        { "FAHRZEUG", "FahrzeugIndex" },
                        { "ZZFAHRZGTYP", "Typ" },
                        { "ZZKENN", "KennzeichenConverted" },
                        { "FZGART", "FahrzeugklasseConverted" },
                        { "ZULGE", "FahrzeugZugelassenConverted" },
                        { "ZUL_BEI_CK_DAD", "FahrzeugZugelassenDAD" },
                        { "SOWI", "Bereifung" },
                        { "ROTKENN", "EmptyString" },
                        { "AUGRU", "Fahrzeugwert" },
                        { "ZZREFNR", "Referenznummer" },
                        { "ZZFAHRG", "FIN" },
                        //{ "ERSTZULDAT", "EmptyString" },
                        { "ZFZGKAT", "EmptyString" },
                        { "EXKUNNR_ZL", "EmptyString" },
                    }));
            }
        }

        #endregion
    }
}