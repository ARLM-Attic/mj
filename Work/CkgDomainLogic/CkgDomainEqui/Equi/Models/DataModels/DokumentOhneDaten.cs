﻿using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    /// <summary>
    /// Datensatz für den Report "Dokumente ohne Daten"
    /// </summary>
    public class DokumentOhneDaten
    {
        [LocalizedDisplay(LocalizeConstants.DateOfReceipt)]
        public DateTime? Eingangsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2)]
        public string ZB2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarOwner)]
        public string Name1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string Name2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string Strasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.HouseNo)]
        public string Hausnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string PLZ { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string Ort { get; set; }
    }
}