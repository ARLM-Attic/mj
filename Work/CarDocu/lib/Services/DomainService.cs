﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CarDocu.Models;
using GeneralTools.Services;
using System.Collections.ObjectModel;
using WebTools.Services;

namespace CarDocu.Services
{
    public static class DomainService
    {
        public static bool DebugIsAdminEnvironment
        {
            get
            {
                return Environment.UserName.ToLower().Contains("jenzenm") &&
                       Environment.MachineName.ToUpper().Contains("AHW460") ||
                       Environment.UserName.ToLower().Contains("dittbernerc") &&
                       Environment.MachineName.ToUpper().Contains("AHW329");
            }
        }

        public static string AppName { get { return AppSettings.AppName; } }

        public static string AppVersion { get { return string.Format("{0}.{1}", Assembly.GetEntryAssembly().GetName().Version.Major, Assembly.GetEntryAssembly().GetName().Version.Minor); } }

        public static DateTime JobCancelDate { get { return DateTime.Parse("01.01.2000"); } }

        public static string DomainPath { get { return Repository.AppSettings.DomainPath; } }

        public static string DomainName { get { return Repository.AppSettings.DomainName; } }

        private static DomainRepository _repository;
        public static DomainRepository Repository { get { return (_repository ?? (_repository = new DomainRepository())); } }

        private static DomainThreads _threads;
        public static DomainThreads Threads { get { return (_threads ?? (_threads = new DomainThreads())); } }

        private static SimpleLogger _logger;
        public static SimpleLogger Logger { get { return (_logger ?? (_logger = new SimpleLogger(Repository.UserErrorLogDirectoryName))); } }

        private static ObservableCollection<StatusMessage>  _statusMessages = new ObservableCollection<StatusMessage>();
        public static ObservableCollection<StatusMessage> StatusMessages { get { return (_statusMessages ?? (_statusMessages = new ObservableCollection<StatusMessage>())); } }
        
        static public bool UserLogon(Func<string> getUserLoginDataFromDialog, bool forceAdminLogon = false)
        {
            if (forceAdminLogon)
            {
                Repository.LogonUser = Repository.AdminUser;
                Repository.InitRemainingSettings();
                return true;
            }

            var loginData = getUserLoginDataFromDialog(); // Tools.Input("Login:", string.Format("{0}, Login", AppName));
            if (string.IsNullOrEmpty(loginData) || !loginData.Contains("~"))
                return false;

            var loginDataArray = loginData.Split('~');
            var loginName = loginDataArray[0];

            var logonUser = Repository.GlobalSettings.DomainUsers.FirstOrDefault(user => user.LoginName == loginName);
            if (logonUser == null)
            {
                if (!string.IsNullOrEmpty(loginName))
                    Tools.AlertError(string.Format("{0}:\r\n\r\nLogin fehlgeschlagen, Benutzer '{1}' ist unbekannt!", AppName, loginName));

                return false;
            }
            logonUser.DomainLocation = Repository.GlobalSettings.DomainLocations.First(loc => loc.SapCode == loginDataArray[1]);
            Repository.GlobalSettingsSave();

            Repository.LogonUser = logonUser;
            Repository.InitRemainingSettings();

            return true;
        }

        public static void LoadGlobalSettings()
        {
            Repository.InitGlobalSettings();
        }

        /// <summary>
        /// Stellt sicher, dass ein Pfad zu den Domänen bezogenen Einstellungen (Konfig Dateien, etc) verfügbar ist
        /// </summary>
        public static bool ValidDomainSettingsAvailable()
        {
            if (!string.IsNullOrEmpty(DomainPath) && !Directory.Exists(DomainPath))
                FileService.TryDirectoryCreate(DomainPath);

            if (string.IsNullOrEmpty(DomainPath) || !Directory.Exists(DomainPath))
                return false;

            if (string.IsNullOrEmpty(DomainName))
                return false;

            return true;
        }

        public static bool ValidArchivesAvailable()
        {
            if (Repository.GlobalSettings == null)
                return false;

            if (Repository.GlobalSettings.Archives.Any(archive => string.IsNullOrEmpty(archive.Path) && !archive.IsOptional))
                return false;

            return true;
        }

        public static bool SendMail(string to, string subject, string body, IEnumerable<string> filesToAttach = null)
        {
            if (Repository.GlobalSettings == null || Repository.GlobalSettings.SmtpSettings == null)
                return false;

            return new SmtpMailService(Repository.GlobalSettings.SmtpSettings).SendMail(to, subject, body, filesToAttach); 
        }

        public static bool SendTestMail()
        {
            return true;

            //if (Repository.GlobalSettings == null || Repository.GlobalSettings.SmtpSettings == null)
            //    return false; 

            //return SendMail(Repository.GlobalSettings.SmtpSettings.From, "CarDocu - Test", "");
        }
    }
}
