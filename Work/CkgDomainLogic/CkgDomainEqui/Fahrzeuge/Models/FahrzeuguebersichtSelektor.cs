﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class FahrzeuguebersichtSelektor : Store
    {
        [LocalizedDisplay(LocalizeConstants.Action)]
        public string Akion
        {
            get { return PropertyCacheGet(() => "manuell"); }
            set { PropertyCacheSet(value); }
        }

        public string Akionen { get { return string.Format("manuell,{0};upload,{1}", Localize.ManuelInput, Localize.FileUpload); } }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        [FormPersistable]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [FormPersistable]
        [Kennzeichen]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2No)]
        [FormPersistable]
        public string Zb2Nummer { get; set; }
     
        [LocalizedDisplay(LocalizeConstants.ModelID)]
        [FormPersistable]
        public string ModelID { get; set; }

        [LocalizedDisplay(LocalizeConstants.UnitNumber)]
        [FormPersistable]
        public string Unitnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        [FormPersistable]
        public string Auftragsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.BatchID)]
        [FormPersistable]
        public string BatchId { get; set; }

        [LocalizedDisplay(LocalizeConstants.SippCode)]
        [FormPersistable]
        // ReSharper disable once InconsistentNaming
        public string SIPPCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfZb2Receipt)]
        [FormPersistable]
        public DateRange EingangZb2DatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.DateOfVehicleArrival)]
        [FormPersistable]
        public DateRange EingangFahrzeugDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.DateOfReadyIndication)]
        [FormPersistable]
        public DateRange BereitmeldungDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        [FormPersistable]
        public DateRange ZulassungDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }



        [LocalizedDisplay(LocalizeConstants.CarManufacturer)]
        [FormPersistable]
        public string Herstellerkennung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        [FormPersistable]
        public string Statuskennung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Pdi)]
        [FormPersistable]
        // ReSharper disable once InconsistentNaming
        public string Pdi { get; set; }


        public static List<SelectItem> FahrzeugHersteller
        {
            get
            {
                var hersteller = GetViewModel().FahrzeugHersteller;
                return hersteller.ConvertAll(WrapManufacturer);              
            }
        }

        public static List<SelectItem> FahrzeugStatus
        {
            get
            {
                var status = GetViewModel().FahrzeugStatus;
                return status.ConvertAll(WrapStatus);
            }
        }

        public static List<SelectItem> PdiStandorte
        {
            get
            {
                var pdi = GetViewModel().PdiStandorte;
                return pdi.ConvertAll(WrapPdi);
            }
        }

        static SelectItem WrapManufacturer(Fahrzeughersteller hersteller)
        {
            if (hersteller.HerstellerName.StartsWith("(")) // wg. empty keys aus sap
                return new SelectItem(String.Empty, hersteller.HerstellerName);
            
            return new SelectItem(hersteller.HerstellerName, hersteller.HerstellerName);
        }

        static SelectItem WrapStatus(FahrzeuguebersichtStatus status)
        {            
            return new SelectItem(status.StatusKey, status.StatusText);
        }

        static SelectItem WrapPdi(FahrzeuguebersichtPDI pdi)
        {            
            return new SelectItem(pdi.PDIKey, pdi.PDIText);
        }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<FahrzeuguebersichtViewModel> GetViewModel { get; set; }

    }
}