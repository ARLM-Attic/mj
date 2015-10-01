﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Transactions;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Models.DataModels;
using CkgDomainLogic.Zulassung.MobileErfassung.Models;
using GeneralTools.Services;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Database.Services
{
    public class DomainDbContext : DbContext
    {
        protected string ConnectionString { get; private set; }

        public DomainDbContext(string connectionString, string userName)
            : base(connectionString)
        {
            ConnectionString = connectionString;
            UserName = userName;
        }

        public string UserName { get; private set; }

        public string UserID
        {
            get { return User == null ? "" : User.UserID.ToString(); }
        }

        public DbSet<ApplicationType> ApplicationTypes { get; set; }

        public DbSet<LoginUserMessage> LoginMessages { get; set; }

        public List<LoginUserMessage> ActiveLoginMessages { get { return LoginMessages.ToListOrEmptyList(); } }

        public List<LoginUserMessageConfirmations> GetLoginUserMessageConfirmations()
        {
            if (UserID.IsNullOrEmpty())
                return new List<LoginUserMessageConfirmations>();

            return Database.SqlQuery<LoginUserMessageConfirmations>("SELECT * FROM LoginUserMessageConfirmations WHERE UserID = {0}", UserID).ToList();
        }

        public void SetLoginUserMessageConfirmation(int messageID, DateTime showMessageFromDate)
        {
            if (UserID.IsNullOrEmpty())
                return;
            
            try
            {
                Database.ExecuteSqlCommand(
                    " insert into LoginUserMessageConfirmations (UserID, MessageID, ShowMessageFrom, ConfirmDate)" + 
                    " select {0}, {1}, {2}, getdate()", 
                    UserID, messageID, showMessageFromDate);
            }
            catch (Exception) { }
        }

        private User _user;
        public User User
        {
            get { return _user ?? (_user = GetUser(UserName)); }
        }


        public Customer GetCustomer(int customerID)
        {
            return Database.SqlQuery<Customer>("SELECT * FROM Customer WHERE CustomerID = {0}", customerID).FirstOrDefault();
        }

        public List<Customer> GetAllCustomer()
        {
            return Database.SqlQuery<Customer>("SELECT * FROM Customer").ToListOrEmptyList();
        }

        public string GetEmailAddressFromUserName(string userName)
        {
            return Database.SqlQuery<string>("select mail from vwWebUser inner join WebUserInfo on vwWebUser.UserID = WebUserInfo.id_user where Username = {0}", userName).FirstOrDefault();
        }

        public string GetUserAccountLastLockedBy(string userName)
        {
            return Database.SqlQuery<string>("SELECT LastChangedBy FROM AdminHistory_User WHERE ID = (SELECT MAX(ID) FROM AdminHistory_User WHERE Username = {0} AND Action = {1})", userName, "Benutzer gesperrt").FirstOrDefault();
        }

        public User GetUserFromPasswordToken(string passwordRequestKey)
        {
            return Database.SqlQuery<User>("select * from vwWebUser where PasswordChangeRequestKey = {0}", passwordRequestKey).FirstOrDefault();
        }

        public List<User> GetUserForCustomer(Customer customer)
        {
            return Database.SqlQuery<User>("select * from vwWebUser where CustomerID = {0}", customer.CustomerID).ToListOrEmptyList();
        }

        public User GetUser(string userName)
        {
            return Database.SqlQuery<User>("SELECT * FROM vwWebUser WHERE Username = {0}", userName).FirstOrDefault();
        }

        public User GetUserFromUserNameAndPassword(string userName, string password)
        {
            return Database.SqlQuery<User>("SELECT * FROM vwWebUser WHERE Username = {0} and  Password = {1}", userName, password).FirstOrDefault();
        }

        public User GetUserFromUrlRemoteLoginKey(string urlRemoteLoginKey)
        {
            return Database.SqlQuery<User>("SELECT * FROM vwWebUser WHERE UrlRemoteLoginKey = {0}", urlRemoteLoginKey).FirstOrDefault();
        }

        public User GetAndSetUser(string userName)
        {
            UserName = userName;
            return _user = GetUser(userName);
        }

        public User GetAndSetUserFromUserNameAndPassword(string userName, string password)
        {
            UserName = userName;
            return _user = GetUserFromUserNameAndPassword(userName, password);
        }

        public User GetAndSetUserFromUrlRemoteLoginKey(string urlRemoteLoginKey)
        {
            _user = GetUserFromUrlRemoteLoginKey(urlRemoteLoginKey);
            if (_user != null)
                UserName = User.Username;
            return _user;
        }

        public Customer GetCustomerFromUserName(string userName)
        {
            var user = GetUser(userName);
            if (user == null)
                return null;

            return GetCustomer(user.CustomerID);
        }

        public WebUserInfo GetUserInfo()
        {
            return Database.SqlQuery<WebUserInfo>("SELECT * FROM WebUserInfo WHERE ID_User = {0}", UserID).FirstOrDefault();
        }

        public WebUserPwdHistory GetUserPwdHistory()
        {
            return Database.SqlQuery<WebUserPwdHistory>("SELECT * FROM WebUserPwdHistory WHERE UserID = {0} ORDER BY DateOfChange DESC", UserID).FirstOrDefault();
        }

        public List<string> GetAddressPostcodeCityMapping(string plz)
        {
            int plzInt;

            if (!Int32.TryParse(plz, out plzInt))
                return new List<string>();

            List<string> cityList;
            try
            {
                var query = Database.SqlQuery<AddressPostcodeCityMapping>("SELECT * FROM AddressPostcodeCityMapping WHERE PLZ = {0}", plzInt).ToList();
                cityList = query.OrderBy(c => c.Ort).GroupBy(c => c.Ort).Select(c => c.Key).ToList();
            }
            catch
            {
                cityList = new List<string>();
            }

            return cityList;
        }

        public DbSet<UserGroup> UserGroups { get; set; }

        private List<UserGroup> _userGroupsOfCurrentCustomer;
        public List<UserGroup> UserGroupsOfCurrentCustomer
        {
            get
            {
                return _userGroupsOfCurrentCustomer ?? (_userGroupsOfCurrentCustomer = UserGroups.Where(x => x.CustomerID == User.CustomerID).ToList());
            }
        }

        private UserGroup _userGroup;
        public UserGroup UserGroup
        {
            get
            {
                return _userGroup ?? (_userGroup = Database.SqlQuery<UserGroup>(" SELECT " +
                                                                                  " wg.* " +
                                                                                  " FROM WebUser wu " +
                                                                                  " INNER JOIN WebMember wm ON wm.UserID = wu.UserID " +
                                                                                  " INNER JOIN WebGroup wg ON wm.GroupID = wg.GroupID " +
                                                                                  " WHERE wu.UserID = {0} "
                                                                                  , UserID).FirstOrDefault());
            }
        }

        public DbSet<Organization> Organizations { get; set; }

        private Organization _organization;
        public Organization Organization
        {
            get
            {
                return _organization ?? (_organization = Database.SqlQuery<Organization>(" SELECT " +
                                                                                  " og.* " +
                                                                                  " FROM WebUser wu " +
                                                                                  " INNER JOIN OrganizationMember om ON om.UserID = wu.UserID " +
                                                                                  " INNER JOIN Organization og ON om.OrganizationID = og.OrganizationID " +
                                                                                  " WHERE wu.UserID = {0} "
                                                                                  , UserID).FirstOrDefault());
            }
        }

        public List<Contact> GetGroupContacts(int customerID, string groupName)
        {
            return Database.SqlQuery<Contact>(string.Format(" SELECT " +
                                                            " Contact.* " +
                                                            " FROM Contact  " +
                                                            " INNER JOIN ContactGroups ON " +
                                                            "	Contact.id = ContactGroups.ContactID " +
                                                            " INNER JOIN WebGroup ON " +
                                                            "	ContactGroups.GroupID = WebGroup.GroupID " +
                                                            " WHERE     (ContactGroups.CustomerID = {0}) AND (WebGroup.GroupName = '{1}')"
                                                            , customerID, groupName )).ToListOrEmptyList();
        }

        private IEnumerable<ApplicationUser> _userApps;

        public IEnumerable<ApplicationUser> UserApps
        {
            get
            {
                if (_userApps != null)
                    return _userApps;

                Database.ExecuteSqlCommand("exec ApplicationUserSettingsPrepareCustomerDefaults {0}", UserID);
                _userApps = Database.SqlQuery<ApplicationUser>("SELECT * FROM vwApplicationWebUser WHERE UserID = {0}", UserID);

                return _userApps;
            }
        }

        public void UserAppsRefresh()
        {
            _userApps = null;
        }

        public bool TryLogin(string password)
        {
            if (String.IsNullOrEmpty(password))
                return false;

            var tempUser = Database.SqlQuery<User>("SELECT * FROM vwWebUser WHERE Username = {0} AND password = {1}",
                    UserName,
                    System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1")).FirstOrDefault();

            if (tempUser == null)
                return false;

            return true;
        }

        public void FailedLoginsIncrementAndSave(string userName)
        {
            Database.ExecuteSqlCommand("UPDATE WebUser SET FailedLogins = FailedLogins + 1 WHERE Username = {0}", userName);
        }

        public void LockUserAndSave(string userName)
        {
            Database.ExecuteSqlCommand("UPDATE WebUser SET AccountIsLockedOut = 1, LastChangedBy = {0} WHERE Username = {1}", UserName, userName);
        }

        public void UnlockUserAndSave(string userName)
        {
            Database.ExecuteSqlCommand("UPDATE WebUser SET AccountIsLockedOut = 0, LastChangedBy = {0} WHERE Username = {1}", UserName, userName);
        }

        public void FailedLoginsResetAndSave(string userName)
        {
            Database.ExecuteSqlCommand("UPDATE WebUser SET FailedLogins = 0 WHERE Username = {0}", userName);
        }

        public void StorePasswordToUser(string userName, string password)
        {
            if (User != null)
                User.Password = password;

            Database.ExecuteSqlCommand("UPDATE WebUser SET Password = {0} WHERE Username = {1}", password, userName);
            Database.ExecuteSqlCommand("UPDATE WebUser SET LastPwdChange = getdate() WHERE Username = {0}", userName);
            Database.ExecuteSqlCommand("insert into WebUserPwdHistory (UserID, Password, DateOfChange, InitialPwd) select wu.UserID, {0}, getdate(), 0 from WebUser wu where wu.Username = {1}", password, userName);
            FailedLoginsResetAndSave(userName);
            UnlockUserAndSave(userName);
        }

        public void StorePasswordRequestKeyToUser(string userName, string passwordRequestKey)
        {
            if (User != null)
                User.Password = passwordRequestKey;

            Database.ExecuteSqlCommand("UPDATE WebUser SET PasswordChangeRequestKey = {0} WHERE Username = {1}", passwordRequestKey, userName);
        }

        public bool TryChangePassword(string oldPassword, string newPassword)
        {
            if ((String.IsNullOrEmpty(oldPassword)) || (String.IsNullOrEmpty(newPassword)))
                return false;

            var erg = Database.ExecuteSqlCommand("UPDATE WebUser SET Password = {0} WHERE Username = {1} AND Password = {2}",
                    System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, "sha1"),
                    UserName,
                    System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(oldPassword, "sha1"));

            return (erg > 0);
        }

        public void SetLastLogin(DateTime zeitpunkt)
        {
            Database.ExecuteSqlCommand("UPDATE WebUser SET LastLogin = {0} WHERE Username = {1}",
                    zeitpunkt,
                    UserName);
        }

        public void DataContextPersist(object dataContext)
        {
            var type = dataContext.GetType();

            var contextKey = type.GetFullTypeName();
            var data = XmlService.XmlSerializeToString(dataContext);

            Database.ExecuteSqlCommand("EXEC WebUserSessionContextSave {0}, {1}, {2}", UserName, contextKey, data);
        }

        public object DataContextRestore(string typeName)
        {
            var sessionContext = Database.SqlQuery<WebUserSessionContext>("SELECT * FROM WebUserSessionContext WHERE UserName = {0} and ContextKey = {1}", UserName, typeName).FirstOrDefault();
            if (sessionContext == null)
                return null;

            var type = Type.GetType(typeName);
            if (type == null)
                return null;

            return XmlService.XmlDeserializeFromString(sessionContext.ContextData, type);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            System.Data.Entity.Database.SetInitializer<DomainDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


        #region Dokument

        public DbSet<Dokument> Dokuments { get; set; }

        public List<Dokument> DokumentsForAdmin
        {
            get
            {
                var doks = Dokuments.Where(x => x.CustomerID == User.CustomerID).ToList();

                doks.ForEach(x => x.DocTypeName = DocumentTypesForCustomer.Find(t => t.DocumentTypeID == x.DocTypeID).DocTypeName);

                return doks;
            }
        }

        public List<Dokument> DokumentsForGroup
        {
            get
            {
                var dokumentIds = from dokumentRight in DokumentRights
                                 where dokumentRight.GroupID == UserGroup.GroupID
                                 select dokumentRight.DocumentID;

                var doks = Dokuments.Where(x => dokumentIds.Contains(x.DocumentID)).ToList();

                doks.ForEach(x => x.DocTypeName = DocumentTypesForCustomer.Find(t => t.DocumentTypeID == x.DocTypeID).DocTypeName);

                return doks;
            }
        }

        public Dokument SaveDokument(Dokument dokument)
        {
            // hier können weitere Validierungen vorgenommen werden

            // wenn es sich um ein neues Dokument handelt dann trage es in die Liste ein, sonst gehe ich davon aus dass das Dokument aus der Auflistung eingelesen wurde
            if (dokument.DocumentID == 0)
            {
                Dokuments.Add(dokument);                
            }

            SaveChanges();

            return dokument;
        }

        /// <summary>
        /// Dokument DocTypeId anpassen und die Zuordnung zu den Gruppen neu erstellen
        /// </summary>
        /// <param name="documentBearbeiten"></param>
        /// <returns></returns>
        public bool SaveDokument(DocumentErstellenBearbeiten documentBearbeiten)
        {
            // DokumentRights für das Dokument ausfüllen
            var dokumentRights = DokumentRights.Where(x => x.DocumentID == documentBearbeiten.ID);

            // Dokument ermitteln
            var dokument = Dokuments.Single(x => x.DocumentID == documentBearbeiten.ID);

            // Alte DokumentRights Elemente als gelöscht kennzeichnen, keine Prüfung hinsichtlich wiederverwendbarkeit
            dokumentRights.ToList().ForEach(x => DokumentRights.Remove(x));

            // Neue DokumentRights eintragen
            foreach (var webGroup in documentBearbeiten.SelectedWebGroups)
            {
                var documentID = documentBearbeiten.ID;
                var groupID = UserGroupsOfCurrentCustomer.Single(x => x.GroupID == int.Parse(webGroup)).GroupID;

                var newDokumentRight = new DokumentRight
                    {
                        DocumentID = documentID,
                        GroupID = groupID
                    };

                DokumentRights.Add(newDokumentRight);
            }

            // Dokument Wert DocTypeID überschreiben
            dokument.DocTypeID = documentBearbeiten.DocTypeID;
            dokument.LastEdited = DateTime.Now;

            // Jetzt alles auf ein mal schreiben! EF kümmert sich um die Transaction
            var itemsSaved = SaveChanges();
            return itemsSaved > 0;
        }

        public bool DeleteDokument(Dokument dokument)
        {
            var dokumentRights = DokumentRights.Where(x => x.DocumentID == dokument.DocumentID);
            var dokumentToRemove = Dokuments.Single(x => x.DocumentID == dokument.DocumentID);

            using (var scope = new TransactionScope())
            {
                // Alte DokumentRights Elemente als gelöscht kennzeichnen, keine Prüfung hinsichtlich wiederverwendbarkeit
                dokumentRights.ToList().ForEach(x => DokumentRights.Remove(x));
                SaveChanges();

                Dokuments.Remove(dokumentToRemove);
                SaveChanges();

                scope.Complete();

                return true;
            }
        }

        #endregion

        #region DocumentType

        public DbSet<DocumentType> DocumentTypes { get; set; }

        private List<DocumentType> _documentTypesForCustomer;
        public List<DocumentType> DocumentTypesForCustomer
        {
            get
            {
                // DocType mit ID 1 ist der default-Wert für alle Kunden
                return _documentTypesForCustomer ?? (_documentTypesForCustomer = DocumentTypes.Where(x => x.CustomerID == User.CustomerID || x.DocumentTypeID == 1).ToList());
            }
        }

        public DocumentType CreateDocumentType(DocumentType documentType)
        {
            documentType.CustomerID = User.CustomerID;
            DocumentTypes.Add(documentType);
            SaveChanges();
            return documentType;
        }

        public DocumentType UpdateDocumentType(DocumentType documentType)
        {
            var documentTypeToUpdate = DocumentTypes.Single(x => x.DocumentTypeID == documentType.DocumentTypeID);
            documentTypeToUpdate.DocTypeName = documentType.DocTypeName;
            SaveChanges();
            return documentType;
        }

        public int DeleteDokumentType(DocumentType documentType)
        {
            DocumentTypes.Remove(documentType);
            var itemsDeleted = SaveChanges();
            return itemsDeleted;
        }

        #endregion

        #region DokumentRight

        public DbSet<DokumentRight> DokumentRights { get; set; }

        #endregion

        #region TranslatedResource

        public DbSet<TranslatedResource> TranslatedResources { get; set; }
        
        public DbSet<TranslatedResourceCustom> TranslatedResourceCustoms { get; set; }

        public List<TranslatedResource> Resources
        {
            get { return TranslatedResources.ToList(); }
        }

        public List<TranslatedResourceCustom> ResourcesForCustomers
        {
            get { return TranslatedResourceCustoms.ToList(); }
        }

        public TranslatedResource TranslatedResourceLoad(string resourceKey)
        {
            var t = Database.SqlQuery<TranslatedResource>("SELECT * FROM TranslatedResource WHERE Resource = {0}", resourceKey).FirstOrDefault();
            return (t ?? new TranslatedResource { Resource = resourceKey });
        }

        public TranslatedResourceCustom TranslatedResourceCustomerLoad(string resourceKey, int customerID)
        {
            var t = Database.SqlQuery<TranslatedResourceCustom>("SELECT * FROM TranslatedResourceCustom WHERE Resource = {0} and CustomerID = {1}", resourceKey, customerID).FirstOrDefault();
            return (t ?? new TranslatedResourceCustom { Resource = resourceKey, CustomerID = customerID });
        }

        public void TranslatedResourceUpdate(TranslatedResource r)
        {
            Database.ExecuteSqlCommand(
                " if not exists(select Resource from TranslatedResource where Resource = {0}) " +
                "   insert into TranslatedResource (Resource, en, de) select {0}, {1}, {2}", r.Resource, r.en, r.de);

            Database.ExecuteSqlCommand(
                " update TranslatedResource set " +
                "  en = {0}, " +
                "  en_kurz = {1}, " +
                "  de = {2}, " +
                "  de_kurz = {3}, " +
                "  fr = {4}, " +
                "  fr_kurz = {5} " +
                " where Resource = {6}",
                    r.en, r.en_kurz,
                    r.de, r.de_kurz,
                    r.fr, r.fr_kurz,
                    r.Resource);
        }

        public void TranslatedResourceCustomerUpdate(TranslatedResourceCustom r)
        {
            if (r.de.IsNullOrEmpty() || r.en.IsNullOrEmpty())
                return;

            Database.ExecuteSqlCommand(
                " if not exists (select Resource from TranslatedResourceCustom where Resource = {0} and CustomerID = {1}) " +
                "   insert into TranslatedResourceCustom (Resource, CustomerID, en, de) select {0}, {1}, {2}, {3}",
                r.Resource, r.CustomerID, r.en, r.de);

            Database.ExecuteSqlCommand(
                " update TranslatedResourceCustom set " +
                "  en = {0}, " +
                "  en_kurz = {1}, " +
                "  de = {2}, " +
                "  de_kurz = {3}, " +
                "  fr = {4}, " +
                "  fr_kurz = {5} " +
                " where Resource = {6} and CustomerID = {7}",
                    r.en, r.en_kurz,
                    r.de, r.de_kurz,
                    r.fr, r.fr_kurz,
                    r.Resource, r.CustomerID);
        }

        public void TranslatedResourceCustomerDelete(TranslatedResourceCustom r)
        {
            Database.ExecuteSqlCommand(
                " delete from TranslatedResourceCustom where Resource = {0} and CustomerID = {1}",
                    r.Resource, r.CustomerID);
        }
        
        #endregion

        #region CustomerDocument

        /// <summary>
        /// Alle Kundendokumente aus der DB
        /// </summary>
        public DbSet<CustomerDocument> CustomerDocuments { get; set; }

        /// <summary>
        /// Alle Kundendokumente des jeweiligen Kunden
        /// </summary>
        public List<CustomerDocument> CustomerDocumentsForCustomer
        {
            get
            {
                var doks = CustomerDocuments.Where(x => x.CustomerID == User.CustomerID).ToList();

                doks.ForEach(x => x.Category = CustomerDocumentCategoriesForCustomer.Find(c => c.ID == x.CategoryID).CategoryName);

                return doks;
            }
        }

        /// <summary>
        /// Kundendokumente des jeweiligen Kunden für den angegebenen Anwendungsfall
        /// </summary>
        public List<CustomerDocument> CustomerDocumentsForCustomerApplication(string appKey)
        {
            return CustomerDocumentsForCustomer.Where(x => x.ApplicationKey == appKey).ToList();
        }

        /// <summary>
        /// Kundendokumente des jeweiligen Kunden für den angegebenen Anwendungsfall und Referenzschlüssel (z.B. VIN, VorgangID, ...)
        /// </summary>
        public List<CustomerDocument> CustomerDocumentsForCustomerApplicationForReferenceKey(string appKey, string referenceKey)
        {
            return CustomerDocumentsForCustomerApplication(appKey).Where(x => x.ReferenceField.ToString().PadLeft(10,'0') == referenceKey).ToList();
        }

        public CustomerDocument SaveCustomerDocument(CustomerDocument doc)
        {
            if (doc.ID == 0)
            {
                CustomerDocuments.Add(doc);
            }
            else
            {
                var docToUpdate = CustomerDocuments.Single(d => d.ID == doc.ID);
                docToUpdate.ReferenceField = doc.ReferenceField;
                docToUpdate.FileType = doc.FileType;
                docToUpdate.FileName = doc.FileName;
                docToUpdate.CategoryID = doc.CategoryID;
                docToUpdate.Uploaded = doc.Uploaded;
                docToUpdate.AdditionalData = doc.AdditionalData;
                docToUpdate.CustomerID = doc.CustomerID;
                docToUpdate.ApplicationKey = doc.ApplicationKey;
            }

            SaveChanges();

            return doc;
        }

        public int DeleteCustomerDocument(CustomerDocument doc)
        {
            var docToRemove = CustomerDocuments.Single(x => x.ID == doc.ID);
            CustomerDocuments.Remove(docToRemove);
            return SaveChanges();
        }

        public int DeleteCustomerDocument(int id)
        {
            var docToRemove = CustomerDocuments.Single(x => x.ID == id);
            CustomerDocuments.Remove(docToRemove);
            return SaveChanges();
        }

        #endregion

        #region CustomerDocumentCategory

        /// <summary>
        /// Alle Kundendokument-Kategorien aus der DB
        /// </summary>
        public DbSet<CustomerDocumentCategory> CustomerDocumentCategories { get; set; }

        /// <summary>
        /// Alle Kundendokument-Kategorien des jeweiligen Kunden
        /// </summary>
        public List<CustomerDocumentCategory> CustomerDocumentCategoriesForCustomer
        {
            get
            {
                return CustomerDocumentCategories.Where(x => x.CustomerID == User.CustomerID).ToList();
            }
        }

        /// <summary>
        /// Kundendokumente des jeweiligen Kunden für den angegebenen Anwendungsfall
        /// </summary>
        public List<CustomerDocumentCategory> CustomerDocumentCategoriesForCustomerApplication(string appKey)
        {
            return CustomerDocumentCategoriesForCustomer.Where(x => x.ApplicationKey == appKey).ToList();
        }

        public CustomerDocumentCategory SaveCustomerDocumentCategory(CustomerDocumentCategory cat)
        {
            if (cat.ID == 0)
            {
                CustomerDocumentCategories.Add(cat);
            }
            else
            {
                var catToUpdate = CustomerDocumentCategories.Single(c => c.ID == cat.ID);
                catToUpdate.CategoryName = cat.CategoryName;
                catToUpdate.CustomerID = cat.CustomerID;
                catToUpdate.DisplayOrder = cat.DisplayOrder;
                catToUpdate.ApplicationKey = cat.ApplicationKey;
            }

            SaveChanges();

            return cat;
        }

        public int DeleteCustomerDocumentCategory(CustomerDocumentCategory cat)
        {
            var catToRemove = CustomerDocumentCategories.Single(x => x.ID == cat.ID);
            CustomerDocumentCategories.Remove(catToRemove);
            return SaveChanges();
        }

        public int DeleteCustomerDocumentCategory(int id)
        {
            if (CustomerDocuments.Any(x => x.CategoryID == id))
            {
                return 0;
            }

            var catToRemove = CustomerDocumentCategories.Single(x => x.ID == id);
            CustomerDocumentCategories.Remove(catToRemove);
            return SaveChanges();
        }

        #endregion

        #region Abrufgruende

        public string GetAbrufgrundBezeichnung(string grundId)
        {
            var ergs =
                Database.SqlQuery<string>(
                    "SELECT WebBezeichnung FROM CustomerAbrufgruende WHERE CustomerID = {0} AND SAPWert = {1}",
                    User.CustomerID, grundId).ToList();

            return (ergs.Count > 0 ? ergs[0] : "");
        }

        #endregion

        #region ZLD (Mobile)

        public CkgUserInfo GetCkgUserInfo(string username)
        {
            return Database.SqlQuery<CkgUserInfo>("SELECT * FROM vwCkgUser WHERE Username = {0}", username).FirstOrDefault();
        }

        #endregion
    }
}
