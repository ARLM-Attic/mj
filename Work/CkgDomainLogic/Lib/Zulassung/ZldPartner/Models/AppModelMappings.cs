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
                        d.BelegGebuehrenPosition = s.GEB_EBELP;
                        d.BelegNr = s.EBELN;
                        d.BelegPosition = s.EBELP;
                        d.FahrgestellNr = s.ZZFAHRG;
                        d.Gebuehr = s.GEBUEHR.ToString();
                        d.Gebuehrenrelevant = s.GEB_RELEVANT.XToBool();
                        d.Halter = s.ZH_NAME1;
                        d.Herkunft = s.HERK;
                        d.Kennzeichen = s.ZZKENN;
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
                        d.BelegPosition = s.EBELP;
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
                        d.DL_PREIS = s.Preis.ToNullableDecimal();
                        d.EBELN = s.BelegNr;
                        d.EBELP = s.BelegPosition;
                        d.EINDT = s.LieferDatum.ToNullableDateTime("dd.MM.yyyy");
                        d.GEBUEHR = s.Gebuehr.ToNullableDecimal();
                        d.GEB_EBELP = s.BelegGebuehrenPosition;
                        d.GEB_RELEVANT = s.Gebuehrenrelevant.BoolToX();
                        d.HERK = s.Herkunft;
                        d.KREISKZ = s.ZulassungsKreis;
                        d.MAKTX = s.MaterialText;
                        d.MATNR = s.MaterialNr;
                        d.PP_STATUS = s.Status;
                        d.VBELN = s.AuftragsNr;
                        d.VBELP = s.AuftragsPosition;
                        d.ZH_NAME1 = s.Halter;
                        d.ZZBRIEF = s.Zb2Nr;
                        d.ZZFAHRG = s.FahrgestellNr;
                        d.ZZKENN = s.Kennzeichen;
                        d.ZZZLDAT = s.ZulassungsDatum.ToNullableDateTime("dd.MM.yyyy");
                    }
                ));
            }
        }

        #endregion
    }
}
