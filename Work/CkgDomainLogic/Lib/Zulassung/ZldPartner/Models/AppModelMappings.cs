﻿// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.ZldPartner.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region Load from Repository

        static public ModelMapping<Z_ZLD_PP_GET_PO_01.GT_BESTELLUNGEN, OffeneZulassung> Z_ZLD_PP_GET_PO_01_GT_BESTELLUNGEN_To_OffeneZulassung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_PP_GET_PO_01.GT_BESTELLUNGEN, OffeneZulassung>(
                    new Dictionary<string, string> ()
                    ,(s, d) =>
                    {
                        d.AuftragsNr = s.VBELN;
                        d.AuftragsPosition = s.VBELP;
                        d.BeauftragtVon = s.BEZ_WERK_LGORT;
                        d.BelegGebuehrenPosition = s.GEB_EBELP;
                        d.BelegNr = s.EBELN;
                        d.BelegNrSort = s.EBELN_SORT;
                        d.BelegPosition = s.EBELP;
                        d.BelegPositionSort = s.EBELP_SORT;
                        d.BuchungsKreis = s.BUKRS;
                        d.EinkaufsOrganisation = s.EKORG;
                        d.Email = s.SMTP_ADDR;
                        d.Erfasser = s.ERNAM;
                        d.Express = s.EXPRESS.XToBool();
                        d.FahrgestellNr = s.ZZFAHRG;
                        d.Gebuehr = s.GEBUEHR.ToString();
                        d.Gebuehrenrelevant = s.GEB_RELEVANT.XToBool();
                        d.Halter = s.ZH_NAME1;
                        d.Hauptposition = s.HAUPT_POSITION.XToBool();
                        d.Herkunft = s.HERK;
                        d.Kennzeichen = s.ZZKENN;
                        d.KundenNr = s.KUNNR;
                        d.LagerOrt = s.LGORT;
                        d.Lieferant = s.LIFNR;
                        d.LieferDatum = s.EINDT.ToString("dd.MM.yyyy");
                        d.MaterialNr = s.MATNR;
                        d.MaterialText = s.MAKTX;
                        d.NeuePosition = false;
                        d.Preis = s.DL_PREIS.ToString();
                        d.Status = s.PP_STATUS.NotNullOrEmpty();
                        d.StornoBemerkungLangtextNr = s.LTEXT_NR;
                        d.StornoGrundId = s.GRUND_KEY;
                        d.Telefon = s.TELF1;
                        d.Werk = s.WERKS;
                        d.Zb2Nr = s.ZZBRIEF;
                        d.ZulassungsDatum = s.ZZZLDAT.ToString("dd.MM.yyyy");
                        d.ZulassungsKreis = s.KREISKZ;
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_PP_GET_ZULASSUNGEN_01.GT_BESTELL_LISTE, DurchgefuehrteZulassung> Z_ZLD_PP_GET_ZULASSUNGEN_01_GT_BESTELL_LISTE_To_DurchgefuehrteZulassung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_PP_GET_ZULASSUNGEN_01.GT_BESTELL_LISTE, DurchgefuehrteZulassung>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.AbrechnungErstellt = s.ABRECHNUNG_ERSTELLT.XToBool();
                        d.BelegNr = s.EBELN;
                        d.BelegNrSort = s.EBELN_SORT;
                        d.BelegPosition = s.EBELP;
                        d.BelegPositionSort = s.EBELP_SORT;
                        d.FahrgestellNr = s.ZZFAHRG;
                        d.Gebuehr = s.GEBUEHR.ToString();
                        d.Gebuehrenrelevant = s.GEB_RELEVANT.XToBool();
                        d.Halter = s.ZH_NAME1;
                        d.Herkunft = s.HERK;
                        d.Kennzeichen = s.ZZKENN;
                        d.Kunde = s.KUNDE;
                        d.LieferDatum = s.EINDT.ToString("dd.MM.yyyy");
                        d.MaterialNr = s.MATNR;
                        d.MaterialText = s.MAKTX;
                        d.Preis = s.DL_PREIS.ToString();
                        d.Status = s.PP_STATUS.NotNullOrEmpty();
                        d.Zb2Nr = s.ZZBRIEF;
                        d.ZulassungsDatum = s.ZZZLDAT.ToString("dd.MM.yyyy");
                        d.ZulassungsKreis = s.KREISKZ;
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_PP_STAMMDATEN.EXP_GRUENDE, StornoGrund> Z_ZLD_PP_STAMMDATEN_EXP_GRUENDE_To_StornoGrund
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_PP_STAMMDATEN.EXP_GRUENDE, StornoGrund>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.GrundId = s.GRUND_KEY;
                        d.GrundText = s.GRUND;
                        d.Status = s.PP_STATUS;
                        d.MitBemerkung = s.GR_LANGTEXT.XToBool();
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_PP_STAMMDATEN.EXP_MATERIAL, Material> Z_ZLD_PP_STAMMDATEN_EXP_MATERIAL_To_Material
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_PP_STAMMDATEN.EXP_MATERIAL, Material>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.In1010Hinzufuegbar = s.ZZMAT_1010.XToBool();
                        d.In1510Hinzufuegbar = s.ZZMAT_1510.XToBool();
                        d.MaterialNr = s.MATNR;
                        d.MaterialText = s.MAKTX;
                        d.PreisEingebbar = s.ZZMAT_PREIS.XToBool();
                    }));
            }
        }

        #endregion

        

        #region Save to Repository

        static public ModelMapping<Z_ZLD_PP_SAVE_PO_01.GT_BESTELLUNGEN, OffeneZulassung> Z_ZLD_PP_SAVE_PO_01_GT_BESTELLUNGEN_From_OffeneZulassung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_PP_SAVE_PO_01.GT_BESTELLUNGEN, OffeneZulassung>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.BEZ_WERK_LGORT = s.BeauftragtVon;
                        d.BUKRS = s.BuchungsKreis;
                        d.DL_PREIS = s.Preis.ToNullableDecimal();
                        d.EBELN = s.BelegNr;
                        d.EBELN_SORT = s.BelegNrSort;
                        d.EBELP = s.BelegPosition;
                        d.EBELP_SORT = s.BelegPositionSort;
                        d.EINDT = s.LieferDatum.ToNullableDateTime("dd.MM.yyyy");
                        d.EKORG = s.EinkaufsOrganisation;
                        d.ERNAM = s.Erfasser;
                        d.EXPRESS = s.Express.BoolToX();
                        d.GEBUEHR = s.Gebuehr.ToNullableDecimal();
                        d.GEB_EBELP = s.BelegGebuehrenPosition;
                        d.GEB_RELEVANT = s.Gebuehrenrelevant.BoolToX();
                        d.GRUND_KEY = s.StornoGrundId;
                        d.HAUPT_POSITION = s.Hauptposition.BoolToX();
                        d.HERK = s.Herkunft;
                        d.KREISKZ = s.ZulassungsKreis;
                        d.KUNNR = s.KundenNr;
                        d.LGORT = s.LagerOrt;
                        d.LIFNR = s.Lieferant;
                        d.LTEXT_NR = s.StornoBemerkungLangtextNr;
                        d.MAKTX = s.MaterialText;
                        d.MATNR = s.MaterialNr;
                        d.PP_STATUS = s.Status;
                        d.SMTP_ADDR = s.Email;
                        d.TELF1 = s.Telefon;
                        d.VBELN = s.AuftragsNr;
                        d.VBELP = s.AuftragsPosition;
                        d.WERKS = s.Werk;
                        d.ZH_NAME1 = s.Halter;
                        d.ZZBRIEF = s.Zb2Nr;
                        d.ZZFAHRG = s.FahrgestellNr;
                        d.ZZKENN = s.Kennzeichen;
                        d.ZZZLDAT = s.ZulassungsDatum.ToNullableDateTime("dd.MM.yyyy");
                    }
                ));
            }
        }

        static public ModelMapping<Z_ZLD_PP_SAVE_PO_01.GT_MATERIALIEN, OffeneZulassung> Z_ZLD_PP_SAVE_PO_01_GT_MATERIALIEN_From_OffeneZulassung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_PP_SAVE_PO_01.GT_MATERIALIEN, OffeneZulassung>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.EBELN = s.BelegNr;
                        d.MATNR = s.MaterialNr;
                        d.PREIS = s.Preis.ToNullableDecimal();
                    }
                ));
            }
        }

        #endregion
    }
}
