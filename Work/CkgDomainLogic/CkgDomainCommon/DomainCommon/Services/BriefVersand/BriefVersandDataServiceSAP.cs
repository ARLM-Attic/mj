﻿// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappingsDomainCommon = CkgDomainLogic.DomainCommon.Models.AppModelMappings;
using AppModelMappingsGeneral = CkgDomainLogic.General.Models.AppModelMappings;

// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.DomainCommon.Services
{
    public class BriefVersandDataServiceSAP : CkgGeneralDataServiceSAP, IBriefVersandDataService
    {
        private bool? _endgVersand = null;

        private List<VersandGrund> Versandgruende { get { return PropertyCacheGet(() => LoadVersandgruendeFromSap().ToList()); } }

        public BriefVersandDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public List<VersandGrund> GetVersandgruende(bool endgVersand)
        {
            if (!_endgVersand.HasValue || endgVersand != _endgVersand.Value)
            {
                _endgVersand = endgVersand;
                PropertyCacheClear(this, m => m.Versandgruende);
            }

            return Versandgruende;
        }

        private IEnumerable<VersandGrund> LoadVersandgruendeFromSap()
        {
            Z_DPM_READ_VERS_GRUND_KUN_01.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            if (_endgVersand.Value)
            {
                SAP.SetImportParameter("I_ABCKZ", "2");
            }
            else
            {
                SAP.SetImportParameter("I_ABCKZ", "1");
            }

            var sapList = Z_DPM_READ_VERS_GRUND_KUN_01.GT_OUT.GetExportListWithExecute(SAP);

            return AppModelMappingsGeneral.Z_DPM_READ_VERS_GRUND_KUN_01_GT_OUT_To_VersandGrund.Copy(sapList);
        }

        public string SaveVersandBeauftragung(IEnumerable<VersandAuftragsAnlage> versandAuftraege)
        {
            return SAP.ExecuteAndCatchErrors(
                
                // exception safe SAP action:
                () => StoreVersandBeauftragungToSap(versandAuftraege), 
                
                // SAP custom error handling:
                () => {
                    var errorList = Z_DPM_FILL_VERSAUFTR.GT_ERR.GetExportList(SAP);
                    if (errorList.Any())
                        return string.Join("; ", errorList.Select(e => e.BEMERKUNG).Where(FilterSapErrorMessageVersandBeauftragung));
                    return "";
                }, 
                ignoreResultCode: true);
        }

        static bool FilterSapErrorMessageVersandBeauftragung(string sapErrorMessage)
        {
            // gesprochen mit Madeleine am 14.10.:
            // Folgende SAP Fehlermeldung ignorieren: "Equipment < ??? > nicht vorhanden!" 
            // (ist hier kein Fehler, da Equi (VIN) in Z_DPM_COC_01 Tabelle zu dieser Zeit noch gar nicht vorhanden sein kann)
            var strRegex = @"equipment\s\<\s(.)+\s\>\snicht vorhanden!";
            var myRegexOptions = RegexOptions.IgnoreCase;
            var myRegex = new Regex(strRegex, myRegexOptions);

            return !myRegex.Matches(sapErrorMessage).Cast<Match>().Any(m => m.Success);
        }

        private void StoreVersandBeauftragungToSap(IEnumerable<VersandAuftragsAnlage> versandAuftraege)
        {
            Z_DPM_FILL_VERSAUFTR.Init(SAP, "KUNNR_AG", ToDataStoreKundenNr(LogonContext.KundenNr));

            var importList = Z_DPM_FILL_VERSAUFTR.GT_IN.GetImportList(SAP);
            var sapVersandImportList = AppModelMappingsDomainCommon
                .Z_DPM_FILL_VERSAUFTR__GT_IN_From_VersandAuftragsAnlage
                    .CopyBack(versandAuftraege);
            importList.AddRange(sapVersandImportList);
            SAP.ApplyImport(importList);

            SAP.Execute();
        }

        #region not used yet

        public Fahrzeug GetFahrzeugBriefForVin(string vin)
        {
            return GetFahrzeugBriefe(new Fahrzeug {FIN = vin}).FirstOrDefault();
        }

        public IEnumerable<Fahrzeug> GetFahrzeugBriefe(Fahrzeug fahrzeugBriefParameter)
        {
            Z_DPM_UNANGEF_ALLG_01.Init(SAP, "I_KUNNR_AG, I_EQTYP", LogonContext.KundenNr.ToSapKunnr(), "B");

            var importListAG = Z_DPM_UNANGEF_ALLG_01.GT_IN.GetImportList(SAP);
            importListAG.Add(AppModelMappingsDomainCommon.MapFahrzeugeImportToSAP.CopyBack(fahrzeugBriefParameter));
            SAP.ApplyImport(importListAG);

            SAP.Execute();

            var listAbrufbar = AppModelMappingsDomainCommon.MapFahrzeugeAbrufbarFromSAP.Copy(Z_DPM_UNANGEF_ALLG_01.GT_ABRUFBAR.GetExportList(SAP));
            var listFehlerhaft = AppModelMappingsDomainCommon.MapFahrzeugeFehlerhaftFromSAP.Copy(Z_DPM_UNANGEF_ALLG_01.GT_FEHLER.GetExportList(SAP));
            var list = listAbrufbar.Concat(listFehlerhaft);

            return list;
        }

        #endregion
    }
}