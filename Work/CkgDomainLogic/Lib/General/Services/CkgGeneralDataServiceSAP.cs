﻿using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.General.Models.AppModelMappings;

namespace CkgDomainLogic.General.Services
{
    public class CkgGeneralDataServiceSAP : Store, ICkgGeneralDataService
    {
        #region Administration + Infrastructure

        public string GroupName { get { return LogonContext.GroupName; } }

        public string UserName { get { return LogonContext.UserName; } }

        public string UserID { get { return LogonContext.UserID; } }

        public ISapDataService SAP { get; set; }

        public IAppSettings AppSettings { get; protected set; }

        public ILogonContextDataService LogonContext { get; protected set; }

        #endregion


        #region General data + Business logic

        public List<KundeAusHierarchie> KundenAusHierarchie
        {
            get
            {
                return PropertyCacheGet(() =>
                    AppModelMappings.Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE_GT_DEB_To_KundeAusHierarchie.Copy(SapKundenAusHierarchie)
                        .OrderBy(k => k.KundenNameNr).ToList());
            }
        }

        public List<Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.GT_DEB> SapKundenAusHierarchie
        {
                                                // ReSharper disable ConvertClosureToMethodGroup
            get { return PropertyCacheGet(() => GetSapKundenAusHierarchie()); }
                                                // ReSharper restore ConvertClosureToMethodGroup
        }

        public List<Land> Laender
        {
            get
            {
                return PropertyCacheGet(() =>
                    AppModelMappings.Z_M_Land_Plz_001_GT_WEB_To_Land.Copy(SapLaender)
                        .Concat(new List<Land> { new Land { ID = "", Name = Localize.DropdownDefaultOptionPleaseChoose } })
                            .OrderBy(w => w.Name).ToList());
            }
        }

        public List<Z_M_Land_Plz_001.GT_WEB> SapLaender
        {
            get { return PropertyCacheGet(() => Z_M_Land_Plz_001.GT_WEB.GetExportListWithInitExecute(SAP, "")).ToList(); }
        }


        public List<SelectItem> Versicherungen
        {
            get
            {
                return PropertyCacheGet(() =>
                    Z_M_PARTNER_AUS_KNVP_LESEN.AUSGABE.GetExportListWithInitExecute(SAP, "KUNNR", LogonContext.KundenNr.ToSapKunnr())
                        .Where(p => p.PARVW == "ZV")
                            .Select(p => new SelectItem(p.KUNNR, p.NAME1))
                                .Concat(new List<SelectItem> { new SelectItem("", Localize.DropdownDefaultOptionPleaseChoose) })
                                    .OrderBy(w => w.Text).ToList());
            }
        }


        public List<FahrzeugStatus> FahrzeugStatusWerte
        {
            get
            {
                return PropertyCacheGet(() =>
                    AppModelMappings.Z_DPM_CD_ABM_STATUS_TXT__ET_STATUS_To_FahrzeugStatus.Copy(SapFahrzeugStatusWerte)
                        .Concat(new List<FahrzeugStatus> { new FahrzeugStatus { ID = "", Name = Localize.DropdownDefaultOptionAll } })
                            .OrderBy(w => w.FullName).ToList());
            }
        }

        public List<Z_DPM_CD_ABM_STATUS_TXT.ET_STATUS> SapFahrzeugStatusWerte
        {
            get { return PropertyCacheGet(() => Z_DPM_CD_ABM_STATUS_TXT.ET_STATUS.GetExportListWithInitExecute(SAP, "")).ToList(); }
        }
        
                                                                                                        // ReSharper disable ConvertClosureToMethodGroup
        public List<Z_DPM_READ_LV_001.GT_OUT_DL> SapVersandOptionen { get { return PropertyCacheGet(() => GetSapVersandOptionen()); } }
                                                                                                        // ReSharper restore ConvertClosureToMethodGroup
        public List<VersandOption> VersandOptionen
        {
            get
            {
                return PropertyCacheGet(() =>
                    AppModelMappings.Z_DPM_READ_LV_001__GT_OUT_DL_To_VersandOption.Copy(SapVersandOptionen)
                        .Concat(new List<VersandOption> { new VersandOption { ID = "", Name = Localize.DropdownDefaultOptionPleaseChoose } })
                            .OrderBy(w => w.Name).ToList());
            }
        }

                                                                                                        // ReSharper disable ConvertClosureToMethodGroup
        public List<Z_DPM_READ_LV_001.GT_OUT_DL> SapZulassungsOptionen { get { return PropertyCacheGet(() => GetSapZulassungsOptionen()); } }
                                                                                                        // ReSharper restore ConvertClosureToMethodGroup
        public List<ZulassungsOption> ZulassungsOptionen
        {
            get
            {
                return PropertyCacheGet(() =>
                    AppModelMappings.Z_DPM_READ_LV_001__GT_OUT_DL_To_ZulassungsOption.Copy(SapZulassungsOptionen)
                        .Concat(new List<ZulassungsOption> { new ZulassungsOption { ID = "", Name = Localize.DropdownDefaultOptionPleaseChoose } })
                            .OrderBy(w => w.Name).ToList());
            }
        }

        // ReSharper disable ConvertClosureToMethodGroup
        public List<Z_DPM_READ_LV_001.GT_OUT_DL> SapZulassungsDienstleistungen { get { return PropertyCacheGet(() => GetSapZulassungsDienstleistungen()); } }
        // ReSharper restore ConvertClosureToMethodGroup

        public List<ZulassungsDienstleistung> ZulassungsDienstleistungen
        {
            get
            {
                return PropertyCacheGet(() =>
                    AppModelMappings.Z_DPM_READ_LV_001__GT_OUT_DL_To_ZulassungsDienstleistung.Copy(SapZulassungsDienstleistungen)
                        .OrderBy(w => w.Name).ToList());
            }
        }

        public List<Hersteller> Hersteller
        {
            get
            {
                return PropertyCacheGet(() =>
                    AppModelMappings.Z_M_HERSTELLERGROUP_T_HERST_To_Hersteller.Copy(GetSapHersteller())
                        .Concat(new List<Hersteller> { new Hersteller { Code = "", Name = Localize.DropdownDefaultOptionNotSpecified } })
                            .OrderBy(w => w.Name).ToList());
            }
        }

        public string CountryPlzValidate(string country, string plz)
        {
            ISA_ADDR_POSTAL_CODE_CHECK.Init(SAP);

            SAP.SetImportParameter("COUNTRY", country);
            SAP.SetImportParameter("POSTAL_CODE_CITY", plz);

            var validationItem = ISA_ADDR_POSTAL_CODE_CHECK.RETURN.GetExportListWithExecute(SAP).ToListOrEmptyList().FirstOrDefault();
            return validationItem == null ? "" : validationItem.MESSAGE;
        }

        #endregion


        public CkgGeneralDataServiceSAP(ISapDataService sap)
        {
            SAP = sap;

            //
            // Connecting our User LogonContext to our SapDataService
            //
            SAP.GetLogonContext = () => LogonContext;
        }

        private List<Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.GT_DEB> GetSapKundenAusHierarchie()
        {
            Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.Init(SAP);

            var orgRef = ((ILogonContextDataService)LogonContext).Organization.OrganizationReference;

            SAP.SetImportParameter("I_KUNNR", (String.IsNullOrEmpty(orgRef) ? LogonContext.KundenNr.ToSapKunnr() : orgRef.ToSapKunnr()));
            SAP.SetImportParameter("I_VKORG", ((ILogonContextDataService)LogonContext).Customer.AccountingArea.ToString());
            SAP.SetImportParameter("I_VKBUR", ((ILogonContextDataService)LogonContext).Organization.OrganizationReference2);
            SAP.SetImportParameter("I_SPART", "01");

            return Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.GT_DEB.GetExportListWithExecute(SAP).OrderBy(k => k.NAME1).ToList();
        }

        public List<Z_DPM_READ_LV_001.GT_OUT_DL> GetSapVersandOptionen()
        {
            // Versandoptionen
            return GetCustomerDefaultOptionen("3", x => x.EAN11.IsNotNullOrEmpty());
        }

        public List<Z_DPM_READ_LV_001.GT_OUT_DL> GetSapZulassungsOptionen()
        {
            // Zulassungsoptionen
            return GetCustomerDefaultOptionen("1", x => x.ASNUM.IsNullOrEmpty() && x.KTEXT1_H2.IsNullOrEmpty());
        }

        public List<Z_DPM_READ_LV_001.GT_OUT_DL> GetSapZulassungsDienstleistungen()
        {
            // Zulassungs-Dienstleistungen
            var transportType = "1";
            return GetCustomerDefaultOptionen("1", x => x.ASNUM.IsNotNullOrEmpty() && x.EXTGROUP == transportType && x.KTEXT1_H2.IsNullOrEmpty());
        }

        public List<Z_DPM_READ_LV_001.GT_OUT_DL> GetCustomerDefaultOptionen(string sortKey, Func<Z_DPM_READ_LV_001.GT_OUT_DL, bool> filter)
        {
            Z_DPM_READ_LV_001.Init(SAP, "I_VWAG", "X");

            var importListAG = Z_DPM_READ_LV_001.GT_IN_AG.GetImportList(SAP);
            importListAG.Add(new Z_DPM_READ_LV_001.GT_IN_AG { AG = LogonContext.KundenNr.ToSapKunnr() });
            SAP.ApplyImport(importListAG);

            var importListProcess = Z_DPM_READ_LV_001.GT_IN_PROZESS.GetImportList(SAP);
            importListProcess.Add(new Z_DPM_READ_LV_001.GT_IN_PROZESS { SORT1 = sortKey });
            SAP.ApplyImport(importListProcess);

            SAP.Execute();

            return Z_DPM_READ_LV_001.GT_OUT_DL.GetExportList(SAP).Where(filter).ToList();
        }

        public List<Z_M_HERSTELLERGROUP.T_HERST> GetSapHersteller()
        {
            Z_M_HERSTELLERGROUP.Init(SAP);

            SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            return Z_M_HERSTELLERGROUP.T_HERST.GetExportListWithExecute(SAP).OrderBy(k => k.HERST_T).Where(k => k.HERST_GROUP.IsNotNullOrEmpty()).ToList();
        }

        public string ToDataStoreKundenNr(string kundenNr)
        {
            return kundenNr.ToSapKunnr();
        }

        public void Init(IAppSettings appSettings, ILogonContext logonContext)
        {
            AppSettings = appSettings;
            LogonContext = (ILogonContextDataService)logonContext;
        }

        public string GetZulassungskreisFromPostcodeAndCity(string postCode, string city)
        {
            var zulassungsKreis = "";

            SAP.ExecuteAndCatchErrors(() =>
                {
                    var zulassungsKreisList = Z_Get_Zulst_By_Plz.T_ZULST.GetExportListWithInitExecute(SAP, "I_PLZ,I_ORT", postCode, city);
                    var zulassungsKreisItem = zulassungsKreisList.FirstOrDefault();
                    if (zulassungsKreisItem != null)
                        zulassungsKreis = zulassungsKreisItem.ZKFZKZ;
                });

            return zulassungsKreis;
        }

        static protected string FormatSapErrorMessage(string sapError)
        {
            return string.Format("Es ist ein Fehler aufgetreten, SAP-Fehler Meldung: {0}", sapError);
        }

        public string GetUserReferenceValueByReferenceType(Referenzfeldtyp referenceType)
        {
            if (LogonContext.Customer.Userreferenzfeld1 == referenceType.ToString())
                return LogonContext.User.Reference;

            if (LogonContext.Customer.Userreferenzfeld2 == referenceType.ToString())
                return LogonContext.User.Reference2;

            if (LogonContext.Customer.Userreferenzfeld3 == referenceType.ToString())
                return LogonContext.User.Reference3;

            return "";
        }
    }
}

