﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;

namespace CkgDomainLogic.Autohaus.ViewModels
{
    public class FormulareViewModel : CkgBaseViewModel
    {
        [XmlIgnore, ScriptIgnore]
        public IZulassungDataService DataService { get { return CacheGet<IZulassungDataService>(); } }

        public FormulareSelektor FormulareSelektor
        {
            get { return PropertyCacheGet(() => new FormulareSelektor()); }
            set { PropertyCacheSet(value); }
        }

        public ZiPoolSelektor ZiPoolSelektor
        {
            get { return PropertyCacheGet(() => new ZiPoolSelektor { Dienstleistung = "ZUL", FahrzeugTyp = "1" }); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore, ScriptIgnore]
        public List<Zulassungskreis> Zulassungskreise { get { return PropertyCacheGet(() => DataService.Zulassungskreise); } }

        [XmlIgnore]
        public string GewaehlterKreisText {
            get
            {
                if (FormulareSelektor == null || Zulassungskreise == null)
                    return "";

                var kreis = Zulassungskreise.FirstOrDefault(k => k.KreisKz == FormulareSelektor.Zulassungskreis);

                return (kreis != null ? kreis.KreisText : FormulareSelektor.Zulassungskreis);
            }
        }

        [XmlIgnore]
        public List<PdfFormular> Formulare
        {
            get { return PropertyCacheGet(() => new List<PdfFormular>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore, ScriptIgnore]
        public List<Domaenenfestwert> FahrzeugArten { get { return PropertyCacheGet(() => DataService.Fahrzeugarten); } }

        [XmlIgnore]
        public ZiPoolDaten ZiPoolDaten
        {
            get { return PropertyCacheGet(() => new ZiPoolDaten()); }
            private set { PropertyCacheSet(value); }
        }

        public ZiPoolDetaildaten ZiPoolDetails
        {
            get
            {
                if (ZiPoolDaten == null || ZiPoolSelektor == null)
                    return new ZiPoolDetaildaten();

                return ZiPoolDaten.Details.FirstOrDefault(d => d.Gewerblich == ZiPoolSelektor.Gewerblich && d.Dienstleistung == ZiPoolSelektor.Dienstleistung);
            }
        }

        public void DataInit()
        {
        }

        public string GetKreisByPlz(string plz)
        {
            if (plz != FormulareSelektor.Postleitzahl)
            {
                FormulareSelektor.Postleitzahl = plz;
                FormulareSelektor.Zulassungskreis = DataService.GetZulassungskreisFromPostcodeAndCity(plz, "");
            }

            return FormulareSelektor.Zulassungskreis;
        }

        public void ApplySelection(FormulareSelektor selektor)
        {
            // bei einer leeren Plz wurde der Kreis geändert, andernfalls wurde bereits vorher per "GetKreisByPlz" der Kreis automatisch ermittelt
            if (string.IsNullOrEmpty(selektor.Postleitzahl))
                FormulareSelektor = selektor;
            else
                selektor.Zulassungskreis = FormulareSelektor.Zulassungskreis;
        }

        public void LoadFormulareAndZiPoolDaten(Action<string, string> addModelError)
        {
            Formulare = DataService.GetFormulare(FormulareSelektor, addModelError);
            ZiPoolDaten = DataService.GetZiPoolDaten(FormulareSelektor.Zulassungskreis, addModelError);

            if ((Formulare == null || Formulare.None()) && ZiPoolDaten == null)
                addModelError("", Localize.NoDataFound);
        }

        public void FilterZiPool(bool gewerblich, string dienstleistung)
        {
            ZiPoolSelektor.Gewerblich = gewerblich;
            ZiPoolSelektor.Dienstleistung = dienstleistung;
        }
    }
}