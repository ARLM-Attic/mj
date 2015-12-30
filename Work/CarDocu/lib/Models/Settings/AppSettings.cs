﻿using System;
using System.Reflection;
using System.Xml.Serialization;
using GeneralTools.Models;

namespace CarDocu.Models
{
    public class AppSettings : ModelBase 
    {
        private string _domainName; 
        public string DomainName 
        { 
            get { return _domainName; }
            set { _domainName = value; SendPropertyChanged("DomainName"); }
        }

        private string _domainPath; 
        public string DomainPath 
        { 
            get { return _domainPath; }
            set
            {
                DomainPathIsDirty = (DomainPath != null && value.NotNullOrEmpty() != DomainPath.NotNullOrEmpty());

                _domainPath = value;
                SendPropertyChanged("DomainPath");
            }
        }

        private bool _onlineStatusAutoCheckDisabled;

        public bool OnlineStatusAutoCheckDisabled
        {
            get { return _onlineStatusAutoCheckDisabled; }
            set
            {
                _onlineStatusAutoCheckDisabled = value;
                SendPropertyChanged("OnlineStatusAutoCheckDisabled");
            }
        }

        private bool _domainPathIsDirty;

        [XmlIgnore]
        public bool DomainPathIsDirty
        {
            get { return _domainPathIsDirty; }
            set
            {
                _domainPathIsDirty = value;
                SendPropertyChanged("DomainPathIsDirty");
            }
        }

        [XmlIgnore]
        public bool DomainNameIsValid { get { return !string.IsNullOrEmpty(DomainName); } }

        public bool DomainPathIsValid { get { return !string.IsNullOrEmpty(DomainPath); } }

        [XmlIgnore]
        public bool IsValidAtFirstGlance { get { return !string.IsNullOrEmpty(DomainName) && !string.IsNullOrEmpty(DomainPath); } }

        [XmlIgnore]
        public static string AppCompanyName { get { return ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false)).Company; } }

        [XmlIgnore]
        public static string AppSettingsDirectoryName { get { return AppCompanyName + @"\CarDocu_Scan"; } }

        [XmlIgnore]
        public static string AppName { get { return "CKG Scan Client"; } }

        [XmlIgnore]
        public static string AppVersion { get { return string.Format("{0}.{1}", Assembly.GetEntryAssembly().GetName().Version.Major, Assembly.GetEntryAssembly().GetName().Version.Minor); } }
    }
}