﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class UnfallmeldungenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFahrzeugeDataService DataService { get { return CacheGet<IFahrzeugeDataService>(); } }

        public UnfallmeldungenSelektor UnfallmeldungenSelektor
        {
            get { return PropertyCacheGet(() => new UnfallmeldungenSelektor()); }
            set { PropertyCacheSet(value); }
        }

        public Unfallmeldung MeldungForCreate { get; set; }

        public List<Adresse> StationsCodes
        {
            get
            {
                return PropertyCacheGet(() => DataService.GetStationCodes()
                            .Concat(new List<Adresse> { new Adresse { KundenNr = "", Name1 = Localize.DropdownDefaultOptionPleaseChoose }})
                                .OrderBy(s => s.Kennung)
                                    .ToListOrEmptyList());
            }
        }

        [XmlIgnore]
        public List<Unfallmeldung> Unfallmeldungen
        {
            get { return PropertyCacheGet(() => new List<Unfallmeldung>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Unfallmeldung> UnfallmeldungenFiltered
        {
            get { return PropertyCacheGet(() => Unfallmeldungen); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<FahrzeugStatus> FahrzeugStatusWerte
        {
            get { return PropertyCacheGet(() => DataService.FahrzeugStatusWerte); }
        }

        public void DataInit(bool forReport)
        {
            PropertyCacheClear(this, m => m.UnfallmeldungenSelektor);
            DataMarkForRefresh();
            UnfallmeldungenSelektor.NurMitAbmeldungen = forReport;
            UnfallmeldungenSelektor.MeldeDatumRange.IsSelected = forReport;
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.UnfallmeldungenFiltered);
        }

        public void Validate(Action<Expression<Func<UnfallmeldungenSelektor, object>>, string> addModelError)
        {            
        }

        public void LoadUnfallmeldungen()
        {
            Unfallmeldungen = DataService.GetUnfallmeldungen(UnfallmeldungenSelektor);

            DataMarkForRefresh();
        }

        public void SelectUnfallmeldung(string unfallNr, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var fzg = UnfallmeldungenFiltered.FirstOrDefault(f => f.UnfallNr == unfallNr);
            if (fzg == null)
                return;

            fzg.IsSelected = select;
            allSelectionCount = UnfallmeldungenFiltered.Count(c => c.IsSelected);
        }

        public void SelectUnfallmeldungen(bool select, Predicate<Unfallmeldung> filter, out int allSelectionCount, out int allCount, out int allFoundCount)
        {
            UnfallmeldungenFiltered.Where(f => filter(f)).ToListOrEmptyList().ForEach(f => f.IsSelected = select);

            allSelectionCount = UnfallmeldungenFiltered.Count(c => c.IsSelected);
            allCount = UnfallmeldungenFiltered.Count();
            allFoundCount = UnfallmeldungenFiltered.Count(c => c.IsValidForCancellation);
        }

        public void FilterUnfallmeldungen(string filterValue, string filterProperties)
        {
            UnfallmeldungenFiltered = Unfallmeldungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void UnfallmeldungenCancel(string cancelText, out int cancelCount, out string errorMessage)
        {
            var list = Unfallmeldungen.Where(c => c.IsSelected).ToListOrEmptyList();

            DataService.UnfallmeldungenCancel(list, cancelText, out cancelCount, out errorMessage);

            LoadUnfallmeldungen();
        }

        public void ValidateMeldungCreationSearch(Action<string, string> addModelError)
        {
            if (MeldungForCreate.Kennzeichen.IsNullOrEmpty() &&
                MeldungForCreate.Fahrgestellnummer.IsNullOrEmpty() &&
                MeldungForCreate.BriefNummer.IsNullOrEmpty() &&
                MeldungForCreate.UnitNummer.IsNullOrEmpty())
            {
                addModelError("", Localize.ProvideAtLeastOneOption);
            }
        }

        public void MeldungCreateTryLoadEqui(Action<string, string> addModelError)
        {
            string errorMessage;
            var model = MeldungForCreate;

            DataService.MeldungCreateTryLoadEqui(ref model, out errorMessage);
            
            if (errorMessage.IsNotNullOrEmpty())
                addModelError("", errorMessage);
            else
                MeldungForCreate = model;
        }

        public void ValidateMeldungCreationEdit(Action<string, string> addModelError, Unfallmeldung model)
        {
            if (model.StationsCode.IsNullOrEmpty())
                addModelError("StationsCode", Localize.Required);
        }

        public void MeldungCreate(Action<string, string> addModelError, Unfallmeldung model)
        {
            string errorMessage;

            MeldungForCreate.MeldungTyp = model.MeldungTyp;
            MeldungForCreate.StationsCode = model.StationsCode;
            MeldungForCreate.Standort = model.Standort;

            DataService.MeldungCreate(MeldungForCreate, out errorMessage);

            if (errorMessage.IsNotNullOrEmpty())
                addModelError("", errorMessage);
            else
                LoadUnfallmeldungen();
        }
    }
}