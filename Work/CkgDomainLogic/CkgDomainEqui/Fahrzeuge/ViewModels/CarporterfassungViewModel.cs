﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.UPSShip;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class CarporterfassungViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public ICarporterfassungDataService DataService { get { return CacheGet<ICarporterfassungDataService>(); } }

        public CarporterfassungModel AktuellesFahrzeug { get; set; }

        public CarporterfassungModel CarportSelectionModel { get; set; }

        [XmlIgnore]
        public string CarportSelectionModes
        {
            get
            {
                return string.Format("{0},{1};{2},{3}",
                                        "OnlyMine", Localize.OnlyMyEntries,
                                        "AllUsers", Localize.AllUsersEntries);
            }
        }

        [XmlIgnore]
        public List<CarporterfassungModel> FahrzeugeAlle { get; set; }

        [XmlIgnore]
        public List<CarporterfassungModel> Fahrzeuge { get; set; }

        [XmlIgnore]
        public List<CarporterfassungModel> FahrzeugeFiltered
        {
            get { return PropertyCacheGet(() => Fahrzeuge); }
            protected set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<CarporterfassungModel> FahrzeugeForConfirmation { get; set; }

        [XmlIgnore]
        public List<CarporterfassungModel> FahrzeugeForConfirmationFiltered
        {
            get { return PropertyCacheGet(() => FahrzeugeForConfirmation); }
            protected set { PropertyCacheSet(value); }
        }

        public string LastCarportId { get; set; }

        public string UserCarportId
        {
            get
            {
                if (LogonContext == null || LogonContext.User == null)
                    return "";

                return LogonContext.User.Reference;
            }
        }

        [XmlIgnore]
        public IDictionary<string, string> CarportPdis
        {
            get { return PropertyCacheGet(() => DataService.GetCarportPdis().InsertAtTop("", Localize.DropdownDefaultOptionPleaseChoose)); }
        }

        [XmlIgnore]
        public IDictionary<string, string> CarportPersistedPdis
        {
            get
            {
                if (FahrzeugeAlle == null)
                    return new Dictionary<string, string>();

                return CarportPdis
                            .Where(c => FahrzeugeAlle.Any(f => f.CarportId == c.Key) || c.Key == LastCarportId)
                                .ToDictionary(c => c.Key, c => c.Value);
            }
        }

        [XmlIgnore]
        public List<CarportInfo> CarportAdressen {
            get { return PropertyCacheGet(() => new List<CarportInfo>()); }
            protected set { PropertyCacheSet(value); }
        }

        public bool EditMode { get; set; }

        public int CurrentAppID { get; set; }

        public void Init(List<CarporterfassungModel> fahrzeugePersisted)
        {
            FahrzeugeAlle = fahrzeugePersisted;

            GetCurrentAppID();
            DataMarkForRefresh();
            LoadCarportAdressen();
            LoadFahrzeugModel();

            SetFahrzeugeForCurrentMode();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.FahrzeugeFiltered);
            PropertyCacheClear(this, m => m.CarportPdis);
        }

        void SetFahrzeugeForCurrentMode()
        {
            Func<CarporterfassungModel, bool> selector = 
                e => CarportSelectionModel.CarportSelectionMode == "AllUsers"
                    ? e.CarportId == CarportSelectionModel.CarportIdPersisted
                    : e.UserName == LogonContext.UserName;

            Fahrzeuge = FahrzeugeAlle.Where(selector).ToListOrEmptyList();

            PropertyCacheClear(this, m => m.FahrzeugeFiltered);
        }

        public void LoadFahrzeugModel(string kennzeichen = null)
        {
            EditMode = true;

            if (kennzeichen != null)
            {
                AktuellesFahrzeug = Fahrzeuge.FirstOrDefault(f => f.Kennzeichen == kennzeichen);
                if (AktuellesFahrzeug != null)
                    return;
            }            

            AktuellesFahrzeug = new CarporterfassungModel
                {
                    CarportId = LastCarportId,
                    KundenNr = LogonContext.KundenNr.ToSapKunnr(),
                    DemontageDatum = DateTime.Today
                };


            if (CarportSelectionModel != null)
                return;

            CarportSelectionModel = new CarporterfassungModel
            {
                KundenNr = LogonContext.KundenNr.ToSapKunnr(),
                CarportSelectionMode = "AllUsers"
            };

            SetCarportIdPersisted(LastCarportId);
        }

        public void SetCarportIdPersisted(string carportId)
        {
            CarportSelectionModel.CarportIdPersisted = carportId.NotNullOr(CarportPersistedPdis.Keys.FirstOrDefault());

            SetFahrzeugeForCurrentMode();
        }

        public void LastCarportIdInit(string lastCarportId)
        {
            LastCarportId = UserCarportId.NotNullOr(lastCarportId);
        }

        public string DeleteFahrzeugModel(string kennzeichen)
        {
            var fzg = FahrzeugeAlle.FirstOrDefault(f => f.Kennzeichen == kennzeichen);
            if (fzg == null)
                return "";

            FahrzeugeAlle.Remove(fzg);
            SetFahrzeugeForCurrentMode();

            return fzg.ObjectKey;
        }

        public void AddFahrzeug(CarporterfassungModel item)
        {
            FahrzeugeAlle.Add(item);
            SetFahrzeugeForCurrentMode();

            DataMarkForRefresh();
        }

        public void UpdateFahrzeug(CarporterfassungModel item)
        {
            var itemToUpdate = FahrzeugeAlle.FirstOrDefault(f => f.ObjectKey == item.ObjectKey);
            if (itemToUpdate != null)
            {
                var itemIndex = FahrzeugeAlle.IndexOf(itemToUpdate);
                FahrzeugeAlle[itemIndex] = item;

                SetFahrzeugeForCurrentMode();

                DataMarkForRefresh();
            }
        }

        public void PrepareCarportModel(ref CarporterfassungModel model)
        {
            model.Kennzeichen = model.Kennzeichen.NotNullOrEmpty().Trim().ToUpper();
            model.BestandsNr = model.BestandsNr.NotNullOrEmpty().Trim().ToUpper();
            model.FahrgestellNr = model.FahrgestellNr.NotNullOrEmpty().Trim().ToUpper().Replace("O", "0").Replace("I", "1");

            string carportName;
            if (CarportPdis.TryGetValue(model.CarportId, out carportName))
                model.CarportName = carportName;
        }

        public void CheckFahrgestellnummer(CarporterfassungModel model, ModelStateDictionary state)
        {
            var erg = DataService.CheckFahrgestellnummer(model.FahrgestellNr, model.FahrgestellNrPruefziffer);

            if (!String.IsNullOrEmpty(erg))
                state.AddModelError("", String.Format("{0}: {1}", Localize.VinInvalid, erg));
        }

        public void SaveCarportSelectionModel(CarporterfassungModel model)
        {
            CarportSelectionModel = model;
            SetCarportIdPersisted(model.CarportIdPersisted);
        }

        public string SaveFahrzeuge(Action<string, string> outerClearListFunction)
        {
            EditMode = false;

            Dictionary<string, string> objectKeyDict;
            try
            {
                objectKeyDict = Fahrzeuge.ToDictionary(t => t.Kennzeichen, t => t.ObjectKey);
            }
            catch
            {
                return Localize.Carporterfassung_SaveErrorMultipleLicenseNumbers;
            }

            var saveErg = "";
            Fahrzeuge = DataService.SaveFahrzeuge(Fahrzeuge, ref saveErg);

            if (!String.IsNullOrEmpty(saveErg))
                return String.Format("{0}: {1}", Localize.ErrorsOccuredOnSaving, saveErg);

            FahrzeugeForConfirmation = new List<CarporterfassungModel>();
            PropertyCacheClear(this, m => m.FahrzeugeForConfirmationFiltered);

            // restore shopping cart ID's  +  move items into buffer list "FahrzeugeForConfirmation" to enable clearing shopping cart 
            Fahrzeuge.ForEach(f =>
            {
                PrepareCarportModel(ref f);

                if (objectKeyDict.ContainsKey(f.Kennzeichen))
                    f.ObjectKey = objectKeyDict[f.Kennzeichen];

                FahrzeugeForConfirmation.Add(f);
            });

            ClearList(outerClearListFunction);

            DataMarkForRefresh();

            return "";
        }

        private void ClearList(Action<string, string> outerClearListFunction)
        {
            var ownerMultiKey = "ALL";
            var additionalFilter = string.Format(" and ObjectData like '%<CarportId>{0}</CarportId>%'", CarportSelectionModel.CarportIdPersisted);
            if (CarportSelectionModel.CarportSelectionMode.NotNullOrEmpty() != "AllUsers")
            {
                ownerMultiKey = null;
                additionalFilter = null;
            }

            outerClearListFunction(ownerMultiKey, additionalFilter);

            FahrzeugeAlle.RemoveAll(f => f.Status.IsNullOrEmpty());
            SetFahrzeugeForCurrentMode();

            DataMarkForRefresh();
        }

        public byte[] GetLieferschein()
        {
            var fahrzeugeOk = FahrzeugeForConfirmation.Where(f => f.Status.IsNullOrEmpty()).OrderBy(f => f.Kennzeichen).ToList();

            if (fahrzeugeOk.None())
                return new byte[]{};

            var tblLieferschein = new DataTable("Lieferschein");
            tblLieferschein.Columns.Add("Nr");
            tblLieferschein.Columns.Add("Kennzeichen");
            tblLieferschein.Columns.Add("Fahrgestellnummer");
            tblLieferschein.Columns.Add("Hersteller");
            tblLieferschein.Columns.Add("Demontagedatum");
            tblLieferschein.Columns.Add("Vorlage ZBI");
            tblLieferschein.Columns.Add("Anzahl Kennzeichen");
            tblLieferschein.Columns.Add("Web User");
            tblLieferschein.Columns.Add("Carport Nr");
            tblLieferschein.Columns.Add("Erfassungsdatum");
            tblLieferschein.Columns.Add("Bestandsnummer");
            tblLieferschein.Columns.Add("Auftragsnummer");
            tblLieferschein.AcceptChanges();

            var tblKopf = new DataTable("Kopf");
            tblKopf.Columns.Add("CarportID");
            tblKopf.Columns.Add("Name1");
            tblKopf.Columns.Add("Name2");
            tblKopf.Columns.Add("LieferscheinNummer");
            tblKopf.Columns.Add("Kundenname");
            tblKopf.AcceptChanges();

            var nr = 1;
            var lieferscheinNr = "";
            foreach (var fzg in fahrzeugeOk)
            {
                if (nr == 1)
                {
                    var newKopfRow = tblKopf.NewRow();
                    newKopfRow["CarportID"] = fzg.CarportId;
                    newKopfRow["Name1"] = LogonContext.User.LastName;
                    newKopfRow["Name2"] = LogonContext.User.FirstName;
                    newKopfRow["LieferscheinNummer"] = fzg.LieferscheinNr;
                    lieferscheinNr = fzg.LieferscheinNr;
                    newKopfRow["Kundenname"] = LogonContext.CustomerName;
                    tblKopf.Rows.Add(newKopfRow);
                }

                var newRow = tblLieferschein.NewRow();
                newRow["Nr"] = nr;
                newRow["Kennzeichen"] = fzg.Kennzeichen;
                newRow["Fahrgestellnummer"] = fzg.FahrgestellNr;
                newRow["Hersteller"] = "";
                newRow["Demontagedatum"] = fzg.DemontageDatum.ToString("dd.MM.yyyy");
                newRow["Vorlage ZBI"] = fzg.Zb1Vorhanden;
                newRow["Anzahl Kennzeichen"] = fzg.AnzahlKennzeichen;
                newRow["Web User"] = LogonContext.UserName;
                newRow["Carport Nr"] = fzg.CarportId;
                newRow["Erfassungsdatum"] = DateTime.Now.ToShortDateString();
                newRow["Bestandsnummer"] = fzg.BestandsNr;
                newRow["Auftragsnummer"] = fzg.AuftragsNr;
                tblLieferschein.Rows.Add(newRow);
                nr++;
            }

            var imageHt = new Hashtable();
            var ms = BarcodeService.CreateBarcode(lieferscheinNr);
            imageHt.Add("Logo3", ms);

            var docFactory = new WordDocumentFactory(tblLieferschein, imageHt);

            return docFactory.CreateDocumentAndReturnBytes(Localize.Fahrzeuge_Carporterfassung, Path.Combine(AppSettings.RootPath, @"Documents\Templates\Bestellung.doc"), tblKopf);
        }

        public string GenerateUpsShippingOrderHtml()
        {
            var adresseDad = CarportAdressen.FirstOrDefault(a => a.CarportId == "DAD");
            var adresseCarport = CarportAdressen.FirstOrDefault(a => a.CarportId == LastCarportId);

            if (adresseDad == null || adresseCarport == null)
                return Localize.NoAddressTypesAvailableForThisCustomer;

            var username = GeneralConfiguration.GetConfigValue("UpsShippingWebService", "Username");
            var password = GeneralConfiguration.GetConfigValue("UpsShippingWebService", "Password");
            var accessKey = GeneralConfiguration.GetConfigValue("UpsShippingWebService", "AccessKey");

            if (username.IsNullOrEmpty() || password.IsNullOrEmpty() || accessKey.IsNullOrEmpty())
                return Localize.NoAccessDataFoundInDatabase;

            try
            {
                var securityToken = new UPSSecurity
                    {
                        UsernameToken = new UPSSecurityUsernameToken {Username = username, Password = password},
                        ServiceAccessToken = new UPSSecurityServiceAccessToken {AccessLicenseNumber = accessKey}
                    };

                var shipmentCharge = new ShipmentChargeType
                    {
                        BillShipper = new BillShipperType {AccountNumber = adresseCarport.KundenNr},
                        Type = "01"
                    };

                var paymentInfo = new PaymentInfoType {ShipmentCharge = new[] {shipmentCharge}};

                var shipperAddress = new ShipAddressType
                    {
                        AddressLine = new[] { adresseCarport.StrasseHausnummer },
                        City = adresseCarport.Ort,
                        PostalCode = adresseCarport.Plz,
                        CountryCode = adresseCarport.Land
                    };

                var shipper = new ShipperType
                    {
                        ShipperNumber = adresseCarport.KundenNr,
                        Address = shipperAddress,
                        Name = adresseCarport.Name1,
                        AttentionName = adresseCarport.Name2,
                        Phone = new ShipPhoneType { Number = adresseCarport.Telefon }
                    };

                var shipToAddress = new ShipToAddressType
                    {
                        AddressLine = new[] { adresseDad.StrasseHausnummer },
                        City = adresseDad.Ort,
                        PostalCode = adresseDad.Plz,
                        CountryCode = adresseDad.Land
                    };

                var shipTo = new ShipToType
                    {
                        Address = shipToAddress,
                        Name = adresseDad.Name1,
                        AttentionName = adresseDad.Name2,
                        Phone = new ShipPhoneType { Number = adresseDad.Telefon }
                    };

                var firstFahrzeug = FahrzeugeForConfirmation.ToListOrEmptyList().FirstOrDefault();
                var refNumbers = new[]
                    {
                        new ReferenceNumberType {Code = "PO", Value = firstFahrzeug == null ? "4711" : firstFahrzeug.LieferscheinNr},
                        new ReferenceNumberType {Code = "DP", Value = adresseCarport.CarportId}
                    };

                var package = new PackageType
                    {
                        Packaging = new PackagingType {Code = "02", Description = "Package"},
                        PackageWeight = new PackageWeightType
                            {
                                UnitOfMeasurement = new ShipUnitOfMeasurementType {Code = "KGS", Description = "KG"},
                                Weight = "10"
                            }
                    };

                var shipment = new ShipmentType
                    {
                        Description = "ShipmentRequest",
                        PaymentInformation = paymentInfo,
                        Shipper = shipper,
                        ShipTo = shipTo,
                        ReferenceNumber = refNumbers,
                        Service = new ServiceType {Code = "11", Description = "UPS Standard"},
                        Package = new[] {package}
                    };

                var shipmentRequest = new ShipmentRequest
                    {
                        Request = new RequestType {RequestOption = new[] {"nonvalidate"}},
                        Shipment = shipment,
                        LabelSpecification =
                            new LabelSpecificationType {LabelImageFormat = new LabelImageFormatType {Code = "GIF"}}
                    };

                var shipService = new ShipService { Url = GeneralConfiguration.GetConfigValue("UpsShippingWebService", "Url"), UPSSecurityValue = securityToken };

#pragma warning disable 618
                System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();
#pragma warning restore 618

                var shipmentResponse = shipService.ProcessShipment(shipmentRequest);

                var result = shipmentResponse.ShipmentResults.PackageResults.First();

                var gifHexString = result.ShippingLabel.GraphicImage;
                var htmlBytes = Convert.FromBase64String(result.ShippingLabel.HTMLImage);

                var htmlString = Encoding.Default.GetString(htmlBytes);

                var strImgPattern = "<IMG SRC=\"[^\"]*?\"";
                var strImgReplace = string.Format("<IMG SRC=\"data:image/gif;base64,{0}\"", gifHexString);

                htmlString = Regex.Replace(htmlString, strImgPattern, strImgReplace);

                return htmlString;
            }
            catch (System.Web.Services.Protocols.SoapException soapEx)
            {
                return string.Format("{0}: {1} -> {2}", Localize.Error, soapEx.Message, soapEx.Detail.InnerText);
            }
            catch (Exception ex)
            {
                return string.Format("{0}: {1}", Localize.Error, ex.Message);
            }
        }

        public void FilterFahrzeuge(string filterValue, string filterProperties)
        {
            FahrzeugeFiltered = Fahrzeuge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void FilterFahrzeugeForConfirmation(string filterValue, string filterProperties)
        {
            FahrzeugeForConfirmationFiltered = FahrzeugeForConfirmation.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        private void GetCurrentAppID()
        {
            CurrentAppID = LogonContext.GetAppIdCurrent();
        }

        private void LoadCarportAdressen()
        {
            var adressKennung = ApplicationConfiguration.GetApplicationConfigValue("AdressKennung", CurrentAppID.ToString(), LogonContext.Customer.CustomerID, LogonContext.Group.GroupID);

            CarportAdressen = DataService.GetCarportAdressen(adressKennung);
        } 
    }
}
