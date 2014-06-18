﻿using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Equi.Models.AppModelMappings;

namespace CkgDomainLogic.Equi.Services
{
    public class BriefbestandDataServiceSAP : CkgGeneralDataServiceSAP, IBriefbestandDataService
    {
        public FahrzeugbriefBestandFilter DatenFilter { get; set; }

        private List<FahrzeugbriefBestand> FahrzeugbriefeGesamt { get { return PropertyCacheGet(() => LoadFahrzeugbriefeFromSap().ToList()); } }

        public List<FahrzeugbriefBestand> Fahrzeugbriefe 
        { 
            get
            {
                if (DatenFilter.SelektionsfilterLagerbestand && DatenFilter.SelektionsfilterTempVersendete)
                {
                    return FahrzeugbriefeGesamt;
                }
                if (DatenFilter.SelektionsfilterLagerbestand)
                {
                    return FahrzeugbriefeGesamt.Where(b => b.AbcKennzeichen != "1").ToList();
                }
                if (DatenFilter.SelektionsfilterTempVersendete)
                {
                    return FahrzeugbriefeGesamt.Where(b => b.AbcKennzeichen == "1").ToList();
                }
                return new List<FahrzeugbriefBestand>();
            } 
        }

        public BriefbestandDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            DatenFilter = new FahrzeugbriefBestandFilter();
        }

        public void MarkForRefreshFahrzeugbriefe()
        {
            PropertyCacheClear(this, m => m.FahrzeugbriefeGesamt);
        }

        private IEnumerable<FahrzeugbriefBestand> LoadFahrzeugbriefeFromSap()
        {
            var sapList = Z_DPM_BRIEFBESTAND_001.GT_DATEN.GetExportListWithInitExecute(SAP, "I_KUNNR, I_BESTAND, I_TEMPVERS", LogonContext.KundenNr.ToSapKunnr(), "X", "X");

            return AppModelMappings.Z_DPM_BRIEFBESTAND_001_GT_DATEN_To_FahrzeugbriefBestand.Copy(sapList);
        }
    }
}