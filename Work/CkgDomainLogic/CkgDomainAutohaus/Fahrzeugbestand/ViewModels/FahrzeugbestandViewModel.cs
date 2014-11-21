﻿// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeugbestand.Contracts;
using CkgDomainLogic.Fahrzeugbestand.Models;
using CkgDomainLogic.Fahrzeugbestand.Services;
using GeneralTools.Models;
using System.IO;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Fahrzeugbestand.ViewModels
{
    public class FahrzeugbestandViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFahrzeugbestandDataService DataService
        {
            get { return CacheGet<IFahrzeugbestandDataService>(); }
        }

        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            //PropertyCacheClear(this, m => m.StepFriendlyNames);
            //RechnungsAdressen = DataService.GetRechnungsAdressen();
        }
    }
}
