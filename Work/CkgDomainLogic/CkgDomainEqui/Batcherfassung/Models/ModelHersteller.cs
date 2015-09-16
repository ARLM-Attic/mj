﻿using System;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class ModelHersteller : Store
    {
        [LocalizedDisplay(LocalizeConstants.ModelID)]
        public string ModelID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Model)]        
        public string Modellbezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Manufacturer)]       
        public string HerstellerCode { get; set; }
         
        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string HerstellerName { get; set; }

        [LocalizedDisplay(LocalizeConstants.SippCode)]        
        public string SippCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.PeriodOfValidityDays)]
        public string Laufzeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.PeriodOfValidityBinding)]
        public bool Laufzeitbindung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Bluetooth)]
        public bool Bluetooth { get; set; }

        [LocalizedDisplay(LocalizeConstants.EngineType)]
        public string Antrieb { get; set; }

        [LocalizedDisplay(LocalizeConstants.WinterTires)]
        public bool Winterreifen { get; set; }

        [LocalizedDisplay(LocalizeConstants.NaviAvailable)]
        public bool NaviVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.SecurityFleet)]
        public bool SecurityFleet { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicensePlateLeaseCar)]
        public bool KennzeichenLeasingFahrzeug { get; set; }

        [LocalizedDisplay(LocalizeConstants.Towbar)]
        public bool AnhaengerKupplung { get; set; }

        public string Fahrzeuggruppe { get; set; }

        public string IdNameBezeichnung { get { return String.Format("{0} {1} {2}", ModelID, HerstellerName, Modellbezeichnung); } }
    }
}