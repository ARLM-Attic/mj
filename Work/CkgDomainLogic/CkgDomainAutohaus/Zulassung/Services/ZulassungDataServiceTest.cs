﻿using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.Autohaus.Contracts;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.Autohaus.Services
{
    public class ZulassungDataServiceTest : Store, IZulassungDataService
    {
        #region Basics

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        public List<KundeAusHierarchie> KundenAusHierarchie { get; private set; }
        public List<Land> Laender { get; private set; }
        public List<SelectItem> Versicherungen { get; private set; }
        public List<VersandOption> VersandOptionen { get; private set; }
        public List<ZulassungsOption> ZulassungsOptionen { get; private set; }
        public List<ZulassungsDienstleistung> ZulassungsDienstleistungen { get; private set; }
        public List<FahrzeugStatus> FahrzeugStatusWerte { get; private set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        public string ToDataStoreKundenNr(string kundenNr)
        {
            return "";
        }

        public string GetZulassungskreisFromPostcodeAndCity(string postCode, string city)
        {
            return "";
        }

        public void Init(IAppSettings appSettings, ILogonContext logonContext)
        {
        }

        #endregion

        #region Dokumentencenter Formulare

        private static IEnumerable<Zulassungskreis> LoadZulassungskreiseFromSap()
        {
            return null;
        }

        public List<PdfFormular> GetFormulare(FormulareSelektor selector, Action<string, string> addModelError)
        {
            return null;
        }

        #endregion

        #region Zulassungen

        public List<Kunde> Kunden
        {
            get { return PropertyCacheGet(() => LoadKunden().ToList()); }
        }

        public List<Domaenenfestwert> Fahrzeugarten
        {
            get { return PropertyCacheGet(() => LoadFahrzeugartenFromSap().ToList()); }
        }

        public List<Material> Zulassungsarten
        {
            get { return PropertyCacheGet(() => LoadZulassungsAbmeldeArtenFromSap().Where(m => !m.IstAbmeldung).ToList()); }
        }

        public List<Material> Abmeldearten
        {
            get { return PropertyCacheGet(() => LoadZulassungsAbmeldeArtenFromSap().Where(m => m.IstAbmeldung).ToList()); }
        }

        public List<Zusatzdienstleistung> Zusatzdienstleistungen
        {
            get { return PropertyCacheGet(() => LoadZusatzdienstleistungenFromSap().ToList()); }
        }

        public List<Kennzeichengroesse> Kennzeichengroessen
        {
            get { return PropertyCacheGet(() => LoadKennzeichengroessenFromSql().ToList()); }
        }

        public List<Zulassungskreis> Zulassungskreise
        {
            get { return PropertyCacheGet(() => LoadZulassungskreiseFromSap().ToList()); }
        }

        private static ZulassungSqlDbContext CreateDbContext()
        {
            return new ZulassungSqlDbContext();
        }

        public void MarkForRefresh()
        {
            PropertyCacheClear(this, m => m.Kunden);
            PropertyCacheClear(this, m => m.Fahrzeugarten);
            PropertyCacheClear(this, m => m.Zulassungsarten);
            PropertyCacheClear(this, m => m.Abmeldearten);
            PropertyCacheClear(this, m => m.Zusatzdienstleistungen);
            PropertyCacheClear(this, m => m.Kennzeichengroessen);
            PropertyCacheClear(this, m => m.Zulassungskreise);
        }

        private static IEnumerable<Kunde> LoadKunden()
        {
            return new List<Kunde>
                {
                    new Kunde {KundenNr = "Avis", Name1 = "Avis Autovermietung GmbH"},
                    new Kunde {KundenNr = "CSI", Name1 = "CSI Catastrophe International Inc."},
                    new Kunde {KundenNr = "Tesla", Name1 = "Tesla Motors"},
                    new Kunde {KundenNr = "Sixt", Name1 = "Sixt Leasing GmbH"},
                };
        }

        public Bankdaten GetBankdaten(string iban)
        {
            return null;
        }

        public void GetZulassungskreisUndKennzeichen(Vorgang zulassung, out string kreis, out string kennzeichen)
        {
            throw new NotImplementedException();
        }

        public void GetZulassungsKennzeichen(string kreis, out string kennzeichen)
        {
            throw new NotImplementedException();
        }

        public string SaveZulassungen(List<Vorgang> zulassungen, bool saveDataToSap, bool saveFromShoppingCart, bool modusAbmeldung)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Domaenenfestwert> LoadFahrzeugartenFromSap()
        {
            return null;
        }

        private static IEnumerable<Material> LoadZulassungsAbmeldeArtenFromSap()
        {
            return null;
        }

        public string GetZulassungskreis(Vorgang zulassung)
        {
            return null;
        }

        private static IEnumerable<Zusatzdienstleistung> LoadZusatzdienstleistungenFromSap()
        {
            return null;
        }

        private static IEnumerable<Kennzeichengroesse> LoadKennzeichengroessenFromSql()
        {
            var ct = CreateDbContext();

            return ct.GetKennzeichengroessen();
        }

        public string SaveZulassungen(List<Vorgang> zulassungen, bool saveDataToSap, bool saveFromShoppingCart)
        {
            return "";
        }

        #endregion


        #region Zulassungs Report

        private List<ZulassungsReportModel> _zulassungsReportItems;

        public List<ZulassungsReportModel> GetZulassungsReportItems(ZulassungsReportSelektor selector, List<Kunde> kunden, Action<string, string> addModelError)
        {
            var list = (_zulassungsReportItems ?? (_zulassungsReportItems = CreateZulassungsReportItems()));

            if (selector.ZulassungsDatumRange.IsSelected)
                list = list.Where(item =>
                    item.ZulassungDatum.GetValueOrDefault() >= selector.ZulassungsDatumRange.StartDate &&
                    item.ZulassungDatum.GetValueOrDefault() <= selector.ZulassungsDatumRange.EndDate)
                        .ToListOrEmptyList();

            if (selector.AuftragsDatumRange.IsSelected)
                list = list.Where(item =>
                    item.ErfassungsDatum.GetValueOrDefault() >= selector.AuftragsDatumRange.StartDate &&
                    item.ErfassungsDatum.GetValueOrDefault() <= selector.AuftragsDatumRange.EndDate)
                        .ToListOrEmptyList();

            return list;
        }

        private List<ZulassungsReportModel> CreateZulassungsReportItems()
        {
            var list = new List<ZulassungsReportModel>();

            var random = new Random();
            for (var i = 0; i < 1500; i++)
            {
                var kundenIndex = random.Next(1, 10000) % Kunden.Count;
                
                if (Kunden.Count >= 4)
                {
                    kundenIndex = 0;
                    if (i % 15 == 0)
                        kundenIndex = 1;
                    else if (i % 5 == 0)
                        kundenIndex = 2;
                    else if (i % 3 == 0)
                        kundenIndex = 3;
                }

                var kunde = Kunden.GetRange(kundenIndex, 1).First();
                
                var erfDatum = DateTime.Today.AddDays(-1*random.Next(20, 365));
                var zulDatum = DateTime.Today.AddDays(-1*random.Next(20, 365));

                list.Add(new ZulassungsReportModel
                    {
                        KundenNr = kunde.KundenNr,
                        KundenNrAndName = kunde.KundenNameNr,
                        Kennzeichen = "OD-J " + i,
                        EvbNummmer = "" + (i*52).ToString("0000000"),
                        ErfassungsDatum = erfDatum,
                        ZulassungDatum = zulDatum,
                        Preis = (decimal)random.Next(0, 1000) / (zulDatum.Year == 2015 ? 5 : 25)
                    });
            }

            var groupedList = list.GroupBy(item => item.KundenNr).Select(g => new { Kunde = g.Key, Anzahl = g.Count() }).ToList();

            return list;
        }

        #endregion
    }
}
