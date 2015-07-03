﻿using System;
using System.Collections.Generic;
using CkgDomainLogic.Fahrer.Contracts;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrer.Models
{
    public class FahrerAuftragsProtokoll : IFahrerAuftragsFahrt
    {
        [LocalizedDisplay(LocalizeConstants.OrderID)]
        public string AuftragsNr { get; set; }

        [GridHidden]
        public string AuftragsNrFriendly { get { return AuftragsNr.NotNullOrEmpty().TrimStart('0'); } }

        [GridHidden]
        public string UniqueKey { get { return string.Format("{0}-{1}{2}", AuftragsNr.NotNullOrEmpty(), Fahrt.NotNullOrEmpty(), IstSonstigerAuftrag ? "s" : ""); } }

        [LocalizedDisplay(LocalizeConstants.MiscellaneousOrder)]
        public bool IstSonstigerAuftrag { get; set; }

        [GridHidden]
        public string AuftragsDetails
        {
            get
            {
                if (IstSonstigerAuftrag)
                    return Localize.MiscellaneousOrder;

                if (AuftragsNr.IsNullOrEmpty())
                    return Localize.DropdownDefaultOptionPleaseChoose;

                return new List<string>
                {
                    AuftragsNrFriendly.PrependIfNotNull("#"), Fahrt.FormatIfNotNull("Fahrt {this}"), OrtStart, OrtZiel, Kennzeichen, ProtokollArt
                }
                .JoinIfNotNull(", ");
            }
        }

        [LocalizedDisplay(LocalizeConstants.Tour)]
        public string Fahrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeliveryDate)]
        public DateTime? WunschLieferDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [Kennzeichen]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        [VIN]
        public string VIN { get; set; }

        [LocalizedDisplay(LocalizeConstants._Protokollart)]
        public string ProtokollArt { get; set; }

        [LocalizedDisplay(LocalizeConstants._Protokollart)]
        public string ProtokollArt2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.CityStart)]
        public string OrtStart { get; set; }

        [LocalizedDisplay(LocalizeConstants.CityDestination)]
        public string OrtZiel { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference)]
        public string Referenz { get; set; }
    }
}
