﻿// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.Services;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeugbestand.Contracts;
using CkgDomainLogic.Fahrzeugbestand.Models;
using CkgDomainLogic.Partner.Contracts;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeugbestand.Models.AppModelMappings;

// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Partner.Services
{
    public class PartnerDataServiceSAP : AdressenDataServiceSAP, IPartnerDataService 
    {
        public string AuftragGeber { get; set; }

        public string AuftragGeberOderKundenNr { get { return AuftragGeber.IsNotNullOrEmpty() ? AuftragGeber : KundenNr; } }


        public PartnerDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        #region Partner-Adressen, overrides

        override protected Dictionary<string, string> FriendlyAdressenKennungTranslationDict
        {
            get
            {
                return new Dictionary<string, string> 
                {
                    { "ZO01", "HALTER" },
                    { "ZO02", "KAEUFER" },
                };
            }
        }

        override public bool IsEqual(Adresse a1, Adresse a2)
        {
            return (a1.KundenNr.ToSapKunnr() == a2.KundenNr.ToSapKunnr());
        }

        override public List<Adresse> LoadFromSap(string internalKey = null, string kennungOverride = null, bool kundennrMitgeben = true)
        {
            Z_AHP_READ_PARTNER.Init(SAP);
            
            SAP.SetImportParameter("I_KUNNR", KundenNr.ToSapKunnr());

            if (kennungOverride.IsNotNullOrEmpty() || AdressenKennung.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_PARTART", TranslateFromFriendlyAdressenKennung(kennungOverride.IsNotNullOrEmpty() ? kennungOverride : AdressenKennung));

            var sapList = Z_AHP_READ_PARTNER.GT_OUT.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_AHP_READ_PARTNER_GT_OUT_To_Adresse.Copy(sapList, (s, d) =>
                {
                    d.Kennung = AdressenKennung;
                }).ToList();
        }

        override protected Adresse StoreToSap(Adresse adresse, Action<string, string> addModelError, bool deleteOnly)
        {
            var insertMode = adresse.KundenNr.IsNullOrEmpty();

            var sapAdresse = AppModelMappings.Z_AHP_CRE_CHG_PARTNER_GT_WEB_IMP_To_Adresse.CopyBack(adresse);
            sapAdresse.PARTART = TranslateFromFriendlyAdressenKennung(sapAdresse.PARTART);

            Z_AHP_CRE_CHG_PARTNER.Init(SAP);
            SAP.SetImportParameter("I_KUNNR", KundenNr.ToSapKunnr());

            try
            {
                var importList = Z_AHP_CRE_CHG_PARTNER.GT_WEB_IMP.GetImportList(SAP);
                importList.Add(sapAdresse);
                SAP.ApplyImport(importList);
                SAP.Execute();

                if (insertMode)
                {
                    var savedSapItem = Z_AHP_CRE_CHG_PARTNER.GT_OUT.GetExportList(SAP).FirstOrDefault();
                    if (savedSapItem != null)
                        adresse.KundenNr = savedSapItem.KUNNR;
                }
            }
            catch (Exception e)
            {
                if (addModelError != null)
                {
                    var errorPropertyName = e.Message.GetPartEnclosedBy('\'');
                    if (errorPropertyName.IsNullOrEmpty())
                        errorPropertyName = "SapError";

                    addModelError(errorPropertyName, e.Message);
                }
            }

            return adresse;
        }

        #endregion
    }
}