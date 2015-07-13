﻿using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeuge.Models.AppModelMappings;

namespace CkgDomainLogic.Fahrzeuge.Services
{
    public class FahrzeugSperrenVerschiebenDataServiceSAP : CkgGeneralDataServiceSAP, IFahrzeugSperrenVerschiebenDataService
    {
        public FahrzeugSperrenVerschiebenDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public List<FahrzeuguebersichtPDI> GetPDIStandorte()
        {
            Z_DPM_LIST_PDI_001.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            SAP.Execute();

            return AppModelMappings.Z_DPM_LIST_PDI_001_GT_WEB_ToFahrzeuguebersichtPDI.Copy(Z_DPM_LIST_PDI_001.GT_WEB.GetExportList(SAP)).ToList();
        }

        public List<Fahrzeuguebersicht> GetFahrzeuge()
        {
            Z_DPM_LIST_POOLS_001.Init(SAP, "I_KUNNR_AG, I_SELECT", LogonContext.KundenNr.ToSapKunnr(), "X");

            SAP.Execute();

            return AppModelMappings.Z_DPM_LIST_POOLS_001_GT_WEB_ToFahrzeuguebersicht.Copy(Z_DPM_LIST_POOLS_001.GT_WEB.GetExportList(SAP)).ToList();            
        }

        public int FahrzeugeSperren(bool sperren, string sperrText, ref List<Fahrzeuguebersicht> fahrzeuge)
        {
            var anzOk = 0;

            Z_DPM_ZULASSUNGSSPERRE_01.Init(SAP, "I_KUNNR_AG, I_ACTION", LogonContext.KundenNr.ToSapKunnr(), (sperren ? "S" : "E"));

            var impList = new List<Z_DPM_ZULASSUNGSSPERRE_01.GT_IN>();

            fahrzeuge.ForEach(f => impList.Add(new Z_DPM_ZULASSUNGSSPERRE_01.GT_IN { CHASSIS_NUM = f.Fahrgestellnummer, SPERRVERMERK = sperrText }));

            SAP.ApplyImport(impList);

            SAP.Execute();

            var expList = Z_DPM_ZULASSUNGSSPERRE_01.GT_OUT.GetExportList(SAP);

            foreach (var fzg in fahrzeuge)
            {
                var expItem = expList.FirstOrDefault(e => e.CHASSIS_NUM == fzg.Fahrgestellnummer);

                if (expItem != null && expItem.BEM_RETURN.IsNotNullOrEmpty() && expItem.BEM_RETURN != "Sperre gesetzt" && expItem.BEM_RETURN != "Fahrzeug entsperrt")
                {
                    fzg.Bearbeitungsstatus = expItem.BEM_RETURN;
                }
                else
                {
                    fzg.Bearbeitungsstatus = Localize.OK;
                    anzOk++;
                }
            }

            return anzOk;
        }

        public int FahrzeugeVerschieben(string zielPdi, ref List<Fahrzeuguebersicht> fahrzeuge)
        {
            var anzOk = 0;

            Z_M_EC_AVM_PDIWECHSEL.Init(SAP, "ZZKUNNR", LogonContext.KundenNr.ToSapKunnr());

            var jetzt = DateTime.Now.ToShortDateString();

            foreach (var fzg in fahrzeuge)
            {
                try
                {
                    SAP.SetImportParameter("ZZQMNUM", fzg.MeldungsNr);
                    SAP.SetImportParameter("ZZCARPORT", zielPdi);
                    SAP.SetImportParameter("I_ZZCARPORT", fzg.DadPdi);
                    SAP.SetImportParameter("I_ZZDATBEM", jetzt);

                    // Bemerkungen müssen - obwohl vom User nicht änderbar - ans Bapi übergeben werden, gehen sonst verloren!
                    var bemerkungen = new List<Z_M_EC_AVM_PDIWECHSEL.ZZBEMERKUNG> { new Z_M_EC_AVM_PDIWECHSEL.ZZBEMERKUNG { TDLINE = fzg.BemerkungSperre } };
                    SAP.ApplyImport(bemerkungen);

                    SAP.Execute();

                    if (SAP.ResultCode == 0)
                    {
                        fzg.Bearbeitungsstatus = Localize.OK;
                        anzOk++;
                    }
                    else
                    {
                        fzg.Bearbeitungsstatus = String.Format("{0}: {1}", Localize.Error, SAP.ResultMessage);
                    }
                }
                catch (Exception ex)
                {
                    fzg.Bearbeitungsstatus = String.Format("{0}: {1}", Localize.Error, ex.Message);
                }
            }

            return anzOk;
        }

        public int FahrzeugeTexteErfassen(string bemerkungIntern, string bemerkungExtern, ref List<Fahrzeuguebersicht> fahrzeuge)
        {
            var anzOk = 0;

            Z_DPM_SET_BEM_FZGPOOL_01.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            var impList = new List<Z_DPM_SET_BEM_FZGPOOL_01.GT_IN>();

            fahrzeuge.ForEach(f => impList.Add(new Z_DPM_SET_BEM_FZGPOOL_01.GT_IN { CHASSIS_NUM = f.Fahrgestellnummer, BEMERKUNG_INTERN = bemerkungIntern, BEMERKUNG_EXTERN = bemerkungExtern }));

            SAP.ApplyImport(impList);

            SAP.Execute();

            var expList = Z_DPM_SET_BEM_FZGPOOL_01.GT_IN.GetExportList(SAP);

            foreach (var fzg in fahrzeuge)
            {
                var expItem = expList.FirstOrDefault(e => e.CHASSIS_NUM == fzg.Fahrgestellnummer);

                if (expItem != null && expItem.RET.IsNotNullOrEmpty())
                {
                    fzg.Bearbeitungsstatus = expItem.RET;
                }
                else
                {
                    fzg.Bearbeitungsstatus = Localize.OK;
                    anzOk++;
                }
            }

            return anzOk;
        }
    }
}
