﻿// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Equi.Models;
using System.Linq;
using GeneralTools.Models;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Equi.ViewModels
{
    public class EinAusgaengeViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IBriefbestandDataService DataService { get { return CacheGet<IBriefbestandDataService>(); } }

        public EinAusgangSelektor EinAusgangSelektor
        {
            get { return PropertyCacheGet(() => new EinAusgangSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Fahrzeugbrief> EinAusgaenge
        {
            get { return PropertyCacheGet(() => new List<Fahrzeugbrief>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Fahrzeugbrief> EinAusgaengeFiltered
        {
            get { return PropertyCacheGet(() => EinAusgaenge); }
            private set { PropertyCacheSet(value); }
        }

        public void DataMarkForRefresh(bool refreshSelektor = true)
        {
            PropertyCacheClear(this, m => m.EinAusgaengeFiltered);

            if (refreshSelektor)
                PropertyCacheClear(this, m => m.EinAusgangSelektor);
        }

        public void LoadEinAusgaenge(EinAusgangSelektor model, Action<string, string> addModelError)
        {
            EinAusgaenge = DataService.GetEinAusgaenge(model);

            if (EinAusgaenge.None())
                addModelError("", Localize.NoDataFound);

            DataMarkForRefresh(false);
        }

        public void FilterEinAusgaenge(string filterValue, string filterProperties)
        {
            EinAusgaengeFiltered = EinAusgaenge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}