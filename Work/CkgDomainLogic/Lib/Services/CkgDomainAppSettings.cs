﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web;
using CkgDomainLogic.General.Database.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace CkgDomainLogic.Services
{
    public class CkgDomainAppSettings : IAppSettings 
    {
        public string AppName { get { return ""; } }

        public string AppOwnerName { get { return GeneralTools.Services.GeneralConfiguration.GetConfigValue("Global", "AppOwnerShortName"); } }

        public string AppOwnerFullName { get { return GeneralTools.Services.GeneralConfiguration.GetConfigValue("Global", "AppOwnerFullName"); } }

        public string AppOwnerNameAndFullName { get { return string.Format("{0}{1}", AppOwnerName.AppendIfNotNull(" - "), AppOwnerFullName ); } }

        public string AppOwnerImpressumPartialViewName
        {
            get { return GeneralTools.Services.GeneralConfiguration.GetConfigValue("Global", "AppOwnerImpressumPartialViewName").NotNullOr("Partial/Impressum"); }
        }

        public string AppOwnerKontaktPartialViewName
        {
            get { return GeneralTools.Services.GeneralConfiguration.GetConfigValue("Global", "AppOwnerKontaktPartialViewName").NotNullOr("Partial/Kontakt"); }
        }

        public string AppCopyRight { get { return string.Format("© {0} {1}", DateTime.Now.Year, AppName); } }

        public bool IsClickDummyMode { get { return ConfigurationManager.AppSettings["IsClickDummyMode"].NotNullOrEmpty().ToLower() == "true"; } }

        public bool SapNoSqlCache { get { return ConfigurationManager.AppSettings["SapNoSqlCache"].NotNullOrEmpty().ToLower() == "true"; } }

        public string RootPath
        {
            get
            {
                if (HttpContext.Current != null)
                    return HttpContext.Current.Server.MapPath("~/");

                return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"..\..\");
            }
        }

        public string BinPath { get { return Path.Combine(RootPath, "bin"); } }

        public string DataPath { get { return Path.Combine(RootPath, @"App_Data\XmlData\"); } }

        public string UploadFilePath { get { return ConfigurationManager.AppSettings["UploadPathSambaArchive"]; } }

        public string UploadFilePathTemp { get { return ConfigurationManager.AppSettings["UploadPathTemp"]; } }

        public string TempPath { get { return !IsClickDummyMode ? ConfigurationManager.AppSettings["TempPDFPath"] : Path.Combine(RootPath, @"App_Data\FileUpload\Temp\"); } }


        public string WebPictureRelativePath { get { return ConfigurationManager.AppSettings["UploadPath"]; } }

        public string WebPictureContactsRelativePath { get { return string.Format("{0}Responsible/", WebPictureRelativePath); } }

        public string WebViewRelativePath { get { return ConfigurationManager.AppSettings["PathView"]; } }

        public string WebViewAbsolutePath { get { return ConfigurationManager.AppSettings["UploadPathSambaShow"]; } }

        
        public string TestKundenNr { get { return ConfigurationManager.AppSettings["LogonContextTestKundenNr"]; } }


        public string LogoPath { get { return ConfigurationManager.AppSettings["LogoPath"]; } }

        public int LogoPdfPosX { get { return ConfigurationManager.AppSettings["LogoPdfPosX"].ToInt(); } }

        public int LogoPdfPosY { get { return ConfigurationManager.AppSettings["LogoPdfPosY"].ToInt(); } }

        public string SmtpServer { get { return ConfigurationManager.AppSettings["SmtpMailServer"]; } }

        public string SmtpSender { get { return ConfigurationManager.AppSettings["SmtpMailSender"]; } }

        public ISecurityService SecurityService { get; set; }

        public IMailService MailService { get; set; }


        private static DomainDbContext CreateDbContext(string userName)
        {
            return new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], userName);
        }

        public IEnumerable<string> GetAddressPostcodeCityMappings(string plz)
        {
            return CreateDbContext("").GetAddressPostcodeCityMapping(plz);
        }

        public int TokenExpirationMinutes { get { return ConfigurationManager.AppSettings["TokenExpirationMinutes"].ToInt(120); } }
    }
}
