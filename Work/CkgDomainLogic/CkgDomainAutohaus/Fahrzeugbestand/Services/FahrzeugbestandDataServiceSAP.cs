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
using CkgDomainLogic.Partner.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeugbestand.Models.AppModelMappings;

// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Fahrzeugbestand.Services
{
    public class FahrzeugAkteBestandDataServiceSAP : PartnerDataServiceSAP, IFahrzeugAkteBestandDataService
    {
        public FahrzeugAkteBestandDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        #region Load Fahrzeug-Akte-Bestand

        public FahrzeugAkteBestand GetTypDaten(string fin, string herstellerSchluessel, string typSchluessel, string vvsSchluessel)
        {
            Z_AHP_READ_TYPDAT_BESTAND.Init(SAP);

            SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_FIN", fin);
            SAP.SetImportParameter("I_ZZHERSTELLER_SCH", herstellerSchluessel);
            SAP.SetImportParameter("I_ZZTYP_SCHL", typSchluessel);
            SAP.SetImportParameter("I_ZZVVS_SCHLUESSEL", vvsSchluessel);

            SAP.Execute();

            var sapList = Z_AHP_READ_TYPDAT_BESTAND.GT_WEB_TYPDATEN.GetExportList(SAP);
            var list = AppModelMappings.Z_AHP_READ_TYPDAT_BESTAND_GT_TYPDATEN_To_FahrzeugAkteBestand.Copy(sapList);

            return list.FirstOrDefault();
        }

        public List<FahrzeugAkteBestand> GetFahrzeugeAkteBestand(FahrzeugAkteBestandSelektor model)
        {
            return
                AppModelMappings.Z_AHP_READ_FZGBESTAND_GT_WEBOUT_To_FahrzeugAkteBestand.Copy(
                    GetSapFahrzeugeAkteBestand(model)).ToList();
        }

        private IEnumerable<Z_AHP_READ_FZGBESTAND.GT_WEBOUT> GetSapFahrzeugeAkteBestand(FahrzeugAkteBestandSelektor model)
        {
            Z_AHP_READ_FZGBESTAND.Init(SAP);

            SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            
            SAP.SetImportParameter("I_FIN", model.FIN);

            SAP.SetImportParameter("I_HALTER", model.Halter);
            SAP.SetImportParameter("I_KAEUFER", model.Kaeufer);

            SAP.SetImportParameter("I_BRIEFBESTAND", model.BriefbestandsInfo);
            SAP.SetImportParameter("I_LGORT", model.BriefLagerort);
            SAP.SetImportParameter("I_STANDORT", model.FahrzeugStandort);
            SAP.SetImportParameter("I_ERSTZULDAT", model.ErstZulassungsgDatum);
            SAP.SetImportParameter("I_AKTZULDAT", model.ZulassungsgDatumAktuell);
            SAP.SetImportParameter("I_ABMDAT", model.AbmeldeDatum);
            SAP.SetImportParameter("I_KENNZ", model.Kennzeichen);
            SAP.SetImportParameter("I_BRIEFNR", model.Briefnummer);
            SAP.SetImportParameter("I_COCVORHANDEN", (model.CocVorhanden ? "X" : ""));

            SAP.Execute();

            return Z_AHP_READ_FZGBESTAND.GT_WEBOUT.GetExportList(SAP);
        }

        #endregion


        public string SaveFahrzeugAkteBestand(FahrzeugAkteBestand fahrzeugAkteBestand)
        {
            var error = SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                {
                    Z_AHP_CRE_CHG_FZG_AKT_BEST.Init(SAP);
                    SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
                    SAP.SetImportParameter("I_USER", LogonContext.UserName);

                    var fzgList = Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_WEB_IMP.GetImportList(SAP);

                    CreateRowForFahrzeug(fzgList, fahrzeugAkteBestand);

                    SAP.ApplyImport(fzgList);

                    SAP.Execute();
                },

                // SAP custom error handling:
                () =>
                {
                    var sapResultList = Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_OUT_ERR.GetExportList(SAP);

                    var sapResult = sapResultList.FirstOrDefault();
                    if (sapResult != null)
                        return string.Format("Fehler, folgendes Fahrzeug konnte nicht gespeichert werden: FIN {0}, Fin-ID {1}", sapResult.FIN, sapResult.FIN_ID);

                    return "";
                });

            return error;
        }

        private static void CreateRowForFahrzeug(List<Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_WEB_IMP> fzgList, FahrzeugAkteBestand f)
        {
            var sapFahrzeug = AppModelMappings.Z_AHP_CRE_CHG_FZG_AKT_BEST_GT_WEB_IMP_To_FahrzeugAkteBestand.CopyBack(f);

            fzgList.Add(sapFahrzeug);
        }
    }
}
