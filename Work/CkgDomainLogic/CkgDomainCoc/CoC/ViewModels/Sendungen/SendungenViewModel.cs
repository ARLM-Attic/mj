﻿// ReSharper disable RedundantUsingDirective
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.CoC.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;

// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.CoC.ViewModels
{
    public class SendungenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IZulassungDataService DataService { get { return CacheGet<IZulassungDataService>(); } }


        public Func<IEnumerable> FilteredObjectsCurrent
        {
            get { return PropertyCacheGet(() => (Func<IEnumerable>)(() => SendungenFiltered)); }
            set { PropertyCacheSet(value); }
        }


        #region Sendungen

        public SendungsAuftragSelektor SendungsAuftragSelektor
        {
            get { return PropertyCacheGet(() => new SendungsAuftragSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<SendungsAuftrag> Sendungen
        {
            get { return PropertyCacheGet(() => new List<SendungsAuftrag>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<SendungsAuftrag> SendungenFiltered
        {
            get
            {
                FilteredObjectsCurrent = () => SendungenFiltered;
                return PropertyCacheGet(() => Sendungen);
            }
            private set { PropertyCacheSet(value); }
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.SendungenFiltered);
            PropertyCacheClear(this, m => m.SendungsAuftragSelektor);
        }

        public void LoadSendungen(SendungsAuftragSelektor model, Action<string, string> addModelError)
        {
            Sendungen = DataService.GetSendungsAuftraege(model);

            if (Sendungen.None())
                addModelError("", Localize.NoDataFound);

            DataMarkForRefresh();
        }

        public void FilterSendungen(string filterValue, string filterProperties)
        {
            SendungenFiltered = Sendungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion


        public void DataMarkForRefreshMulti()
        {
            PropertyCacheClear(this, m => m.SendungenIdFiltered);
            PropertyCacheClear(this, m => m.SendungsAuftragIdSelektor);
        }


        #region Sendungen, Suche nach ID

        public SendungsAuftragIdSelektor SendungsAuftragIdSelektor
        {
            get { return PropertyCacheGet(() => new SendungsAuftragIdSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<SendungsAuftrag> SendungenId
        {
            get { return PropertyCacheGet(() => new List<SendungsAuftrag>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<SendungsAuftrag> SendungenIdFiltered
        {
            get
            {
                FilteredObjectsCurrent = () => SendungenIdFiltered;
                return PropertyCacheGet(() => SendungenId);
            }
            private set { PropertyCacheSet(value); }
        }

        public void LoadSendungenId(SendungsAuftragIdSelektor model, Action<string, string> addModelError)
        {
            SendungenId = DataService.GetSendungsAuftraegeId(model);

            if (SendungenId.None())
                addModelError("", Localize.NoDataFound);

            DataMarkForRefresh();
        }

        public void FilterSendungenId(string filterValue, string filterProperties)
        {
            SendungenIdFiltered = SendungenId.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion


        #region Sendungen, Suche nach Docs

        public SendungsAuftragDocsSelektor SendungsAuftragDocsSelektor
        {
            get { return PropertyCacheGet(() => new SendungsAuftragDocsSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<SendungsAuftrag> SendungenDocs
        {
            get { return PropertyCacheGet(() => new List<SendungsAuftrag>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<SendungsAuftrag> SendungenDocsFiltered
        {
            get
            {
                FilteredObjectsCurrent = () => SendungenDocsFiltered;
                return PropertyCacheGet(() => SendungenDocs);
            }
            private set { PropertyCacheSet(value); }
        }

        public void LoadSendungenDocs(SendungsAuftragDocsSelektor model, Action<string, string> addModelError)
        {
            SendungenDocs = DataService.GetSendungsAuftraegeDocs(model);

            if (SendungenDocs.None())
                addModelError("", Localize.NoDataFound);

            DataMarkForRefresh();
        }

        public void FilterSendungenDocs(string filterValue, string filterProperties)
        {
            SendungenDocsFiltered = SendungenDocs.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion

    }
}
