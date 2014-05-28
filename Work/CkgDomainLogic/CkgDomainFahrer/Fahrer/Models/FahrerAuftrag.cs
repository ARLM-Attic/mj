﻿using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrer.Models
{
    public class FahrerAuftrag
    {
        [LocalizedDisplay(LocalizeConstants.Action)]
        [GridRawHtml]
        public string AuftragsCommand { get { return AuftragsCommandTemplate == null ? "-" : AuftragsCommandTemplate(this); } }

        [SelectListKey]
        [LocalizedDisplay(LocalizeConstants.OrderID)]
        [GridResponsiveVisible(GridResponsive.TabletOrWider)]
        public string AuftragsNr { get; set; }

        [SelectListText]
        [GridHidden]
        public string AuftragsNrFriendly { get { return AuftragsNr.NotNullOrEmpty().TrimStart('0'); } }

        [LocalizedDisplay(LocalizeConstants.DeliveryDate)]
        [GridResponsiveVisible(GridResponsive.TabletOrWider)]
        public DateTime? WunschLieferDatum { get; set; }

        [GridHidden]
        public string FahrerStatus { get; set; }

        [GridHidden]
        public bool AuftragIstNeu { get { return FahrerStatus.NotNullOrEmpty().Trim().IsNullOrEmpty(); } }

        [GridHidden]
        public bool AuftragIstAbgelehnt { get { return FahrerStatus.NotNullOrEmpty() == "NO"; } }

        [GridHidden]
        public bool AuftragIstAngenommnen { get { return FahrerStatus.NotNullOrEmpty() == "OK"; } }


        [LocalizedDisplay(LocalizeConstants.PostcodeStart)]
        [GridResponsiveVisible(GridResponsive.Workstation)]
        public string PlzStart { get; set; }

        [LocalizedDisplay(LocalizeConstants.CityStart)]
        [GridResponsiveVisible(GridResponsive.Workstation)]
        public string OrtStart { get; set; }

        [LocalizedDisplay(LocalizeConstants.Start)]
        [GridResponsiveVisible(GridResponsive.Tablet)]
        public string PlzOrtStart { get { return OrtStart.FormatIfNotNull("{0} {this}", PlzStart); } }


        [LocalizedDisplay(LocalizeConstants.PostcodeDestination)]
        [GridResponsiveVisible(GridResponsive.Workstation)]
        public string PlzZiel { get; set; }

        [LocalizedDisplay(LocalizeConstants.CityDestination)]
        [GridResponsiveVisible(GridResponsive.Workstation)]
        public string OrtZiel { get; set; }

        [LocalizedDisplay(LocalizeConstants.Destination)]
        [GridResponsiveVisible(GridResponsive.Tablet)]
        public string PlzOrtZiel { get { return OrtZiel.FormatIfNotNull("{0} {this}", PlzZiel); } }


        [LocalizedDisplay(LocalizeConstants.PostcodeReturn)]
        [GridResponsiveVisible(GridResponsive.Workstation)]
        public string PlzRueck { get; set; }

        [LocalizedDisplay(LocalizeConstants.CityReturn)]
        [GridResponsiveVisible(GridResponsive.Workstation)]
        public string OrtRueck { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReturnTour)]
        [GridResponsiveVisible(GridResponsive.Tablet)]
        public string PlzOrtRueck { get { return OrtRueck.FormatIfNotNull("{0} {this}", PlzRueck); } }

        [LocalizedDisplay(LocalizeConstants.Order)]
        [GridResponsiveVisible(GridResponsive.Smartphone)]
        [GridRawHtml]
        public string AuftragsDetails { get { return AuftragsDetailsTemplate == null ? "-" : AuftragsDetailsTemplate(this); } }


        public static Func<FahrerAuftrag, string> AuftragsCommandTemplate { get; set; }
        public static Func<FahrerAuftrag, string> AuftragsDetailsTemplate { get; set; }
    }
}
