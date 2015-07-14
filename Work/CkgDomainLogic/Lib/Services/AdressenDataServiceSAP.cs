﻿// ReSharper disable ConvertClosureToMethodGroup
using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using Adresse = CkgDomainLogic.DomainCommon.Models.Adresse;
using AppModelMappings = CkgDomainLogic.DomainCommon.Models.AddressModelMappings;

namespace CkgDomainLogic.DomainCommon.Services
{
    public class AdressenDataServiceSAP : CkgGeneralDataServiceSAP, IAdressenDataService
    {
        public string AdressenKennung { get; set; }

        public List<Adresse> Adressen { get { return PropertyCacheGet(() => LoadFromSap()); } }

        public List<Adresse> RgAdressen { get { return PropertyCacheGet(() => GetCustomerAdressen("RG")); } }
        public List<Adresse> ReAdressen { get { return PropertyCacheGet(() => GetCustomerAdressen("RE")); } }
        public Adresse AgAdresse { get { return PropertyCacheGet(() => GetCustomerAdressen("AG").FirstOrDefault()); } }

        public List<Adresse> ZulassungsStellen { get { return PropertyCacheGet(() => GetZulassungsStellenFromSap()); } }

        public string KundennrOverride { get; set; }
        public string SubKundennr { get; set; }
        public string KundenNr { get { return KundennrOverride ?? LogonContext.KundenNr; } }


        public AdressenDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefreshAdressen()
        {
            PropertyCacheClear(this, m => m.Adressen);
            PropertyCacheClear(this, m => m.RgAdressen);
            PropertyCacheClear(this, m => m.ReAdressen);
            PropertyCacheClear(this, m => m.AgAdresse);
        }

        public Adresse SaveAdresse(Adresse adresse, Action<string, string> addModelError)
        {
            var storedItem = StoreToSap(adresse, addModelError, false);
            ModelMapping.Copy(storedItem, GetAdresseEquals(storedItem));

            return adresse;
        }

        public void DeleteAdresse(Adresse adresse)
        {
            StoreToSap(adresse, null, true);
            Adressen.RemoveAll(a => IsEqual(a, adresse));
        }

        public List<Adresse> GetZulassungsStellenFromSap()
        {
            Z_DPM_READ_ADRESSPOOL_01.Init(SAP);
            SAP.SetImportParameter("I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_EQTYP", "B");

            var sapList = Z_DPM_READ_ADRESSPOOL_01.GT_ZULAST.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_READ_ADRESSPOOL_01_GT_ZULAST__To__Adresse.Copy(sapList).Where(a => a.Typ.IsNotNullOrEmpty()).ToList();
        }

        public string TranslateFromFriendlyAdressenKennung(string friendlyKennung)
        {
            if (FriendlyAdressenKennungTranslationDict.ContainsValue(friendlyKennung))
                return FriendlyAdressenKennungTranslationDict.First(k => k.Value == friendlyKennung).Key;

            return friendlyKennung;
        }

        public string TranslateToFriendlyAdressenKennung(string kennung)
        {
            if (FriendlyAdressenKennungTranslationDict.ContainsKey(kennung))
                return FriendlyAdressenKennungTranslationDict[kennung];

            return kennung;
        }

        public List<Adresse> GetCustomerAdressen(string addressType)
        {
            SAP.Init("Z_M_PARTNER_AUS_KNVP_LESEN");
            SAP.SetImportParameter("KUNNR", KundenNr.ToSapKunnr());
            if (CkgDomainRules.IstKroschkeAutohaus(KundenNr))
                SAP.SetImportParameter("GRUPPE", GroupName);
            SAP.Execute();

            var sapItems = Z_M_PARTNER_AUS_KNVP_LESEN.AUSGABE.GetExportList(SAP);
            var webItems = AppModelMappings.Z_M_PARTNER_AUS_KNVP_LESEN_AUSGABE_To_Adresse.Copy(sapItems).Where(a => a.Kennung == addressType).OrderBy(w => w.Name1).ToList();

            var id = 0;
            webItems.ForEach(item =>
            {
                item.ID = ++id;
            });

            return webItems;
        }

        private Adresse GetAdresseEquals(Adresse item)
        {
            var foundAdresse = Adressen.FirstOrDefault(c => IsEqual(c, item));
            if (foundAdresse == null)
            {
                PropertyCacheClear(this, m => m.Adressen);
                foundAdresse = Adressen.FirstOrDefault(c => IsEqual(c, item));
            }

            return foundAdresse;
        }


        #region virtuals for polymorphism

        virtual protected Dictionary<string, string> FriendlyAdressenKennungTranslationDict
        {
            get { return new Dictionary<string, string>(); }
        } 

        virtual public bool IsEqual(Adresse a1, Adresse a2)
        {
            return (a1.ID == a2.ID);
        }

        virtual public List<Adresse> LoadFromSap(string internalKey = null, string kennungOverride = null, bool kundennrMitgeben = true)
        {
            Z_DPM_READ_ZDAD_AUFTR_006.Init(SAP);
            
            if (kundennrMitgeben)
                SAP.SetImportParameter("I_KUNNR", KundenNr.ToSapKunnr());

            if (kennungOverride.IsNotNullOrEmpty() || AdressenKennung.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_KENNUNG", TranslateFromFriendlyAdressenKennung(kennungOverride.IsNotNullOrEmpty() ? kennungOverride : AdressenKennung));
            
            if (internalKey.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_POS_KURZTEXT", internalKey);

            var sapList = Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB.GetExportListWithExecute(SAP);

            return AppModelMappings.MapAdressenFromSAP.Copy(sapList).ToList();
        }

        virtual protected Adresse StoreToSap(Adresse adresse, Action<string, string> addModelError, bool deleteOnly)
        {
            var insertMode = adresse.InternalKey.IsNullOrEmpty();
            if (insertMode)
            {
                // need to store a web private key here (InternalKey2), 
                // because we don't know the public key (InternalKey) our data store (SAP) will generate
                adresse.InternalKey2 = Guid.NewGuid().ToString();
            }

            var sapAdresse = AppModelMappings.MapAdressenToSAP.CopyBack(adresse);
            sapAdresse.KENNUNG = TranslateFromFriendlyAdressenKennung(sapAdresse.KENNUNG);

            Z_DPM_PFLEGE_ZDAD_AUFTR_006.Init(SAP);
            SAP.SetImportParameter("I_KUNNR", KundenNr.ToSapKunnr());
            var verarbeitungsKennzeichen = deleteOnly ? "D" : insertMode ? "N" : "U";
            SAP.SetImportParameter("I_VERKZ", verarbeitungsKennzeichen);

            try
            {
                var importList = Z_DPM_PFLEGE_ZDAD_AUFTR_006.GT_WEB.GetImportList(SAP);
                importList.Add(sapAdresse);
                SAP.ApplyImport(importList);
                SAP.Execute();
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
