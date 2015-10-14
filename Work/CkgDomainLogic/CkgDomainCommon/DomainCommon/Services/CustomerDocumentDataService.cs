﻿using System;
using System.Collections.Generic;
using System.Configuration;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Database.Services;
using CkgDomainLogic.General.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.DomainCommon.Services
{
    public class CustomerDocumentDataService : Store, ICustomerDocumentDataService
    {
        private DomainDbContext _domainDbContext;

        public string ApplicationKey { get; set; }
        public string ReferenceKey { get; set; }

        public List<CustomerDocument> AllDocuments { get { return _domainDbContext.CustomerDocumentsForCustomerApplication(ApplicationKey); } }

        public List<CustomerDocument> Documents { get { return _domainDbContext.CustomerDocumentsForCustomerApplicationForReferenceKey(ApplicationKey, ReferenceKey); } }

        public CustomerDocument SaveDocument(CustomerDocument doc)
        {
            return _domainDbContext.SaveCustomerDocument(doc);
        }

        public int DeleteDocument(int id)
        {
            return _domainDbContext.DeleteCustomerDocument(id);
        }

        public List<CustomerDocumentCategory> Categories { get { return _domainDbContext.CustomerDocumentCategoriesForCustomerApplication(ApplicationKey); } }

        public CustomerDocumentCategory SaveCategory(CustomerDocumentCategory cat)
        {
            return _domainDbContext.SaveCustomerDocumentCategory(cat);
        }

        public int DeleteCategory(int id)
        {
            return _domainDbContext.DeleteCustomerDocumentCategory(id);
        }

        #region unused Iterface-Members

        public List<KundeAusHierarchie> KundenAusHierarchie { get; private set; }
        public List<Land> Laender { get; private set; }
        public List<SelectItem> Versicherungen { get; private set; }
        public List<VersandOption> VersandOptionen { get; private set; }
        public List<ZulassungsOption> ZulassungsOptionen { get; private set; }
        public List<ZulassungsDienstleistung> ZulassungsDienstleistungen { get; private set; }
        public List<FahrzeugStatus> FahrzeugStatusWerte { get; private set; }
        public List<Hersteller> Hersteller { get { return new List<Hersteller>(); } }
        public string ToDataStoreKundenNr(string kundenNr)
        {
            throw new NotImplementedException();
        }

        public string GetZulassungskreisFromPostcodeAndCity(string postCode, string city)
        {
            throw new NotImplementedException();
        }

        public string CheckFahrgestellnummer(string fin, string pruefziffer)
        {
            throw new NotImplementedException();
        }

        #endregion

        public void Init(IAppSettings appSettings, ILogonContext logonContext)
        {
            _domainDbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], logonContext.UserName);    
        }

        public string CountryPlzValidate(string country, string plz)
        {
            if (country.NotNullOrEmpty().ToUpper() == "DE" && plz.IsNotNullOrEmpty() && plz.Length != 5)
                return "Deutsche Postleitzahlen müssen 5-stellig sein";

            return "";
        }
    }
}
