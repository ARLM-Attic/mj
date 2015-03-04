﻿// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Web;
using CkgDomainLogic.General.Contracts;
using System.Linq;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Services
{
    public class LogonContextHelper
    {
        public static int GetAppIdCurrent(List<IApplicationUserMenuItem> userApps)
        {
            if (HttpContext.Current == null || userApps == null)
                return 0;

            var url = HttpContext.Current.Request.Url;
            if (HttpContext.Current.Request.HttpMethod.NotNullOrEmpty().ToUpper().Contains("POST") && HttpContext.Current.Request.UrlReferrer != null)
                url = HttpContext.Current.Request.UrlReferrer;

            var urlCurrent = url.ToString().ToLower().Replace("%2f", "/");
            var userAppCurrent = userApps.FirstOrDefault(ua => urlCurrent.Contains(ExtractUrlFromUserApp(ua.AppURL)));
            if (userAppCurrent == null)
                return 0;

            return userAppCurrent.AppID;
        }

        public static string ExtractUrlFromUserApp(string userAppUrl)
        {
            userAppUrl = userAppUrl.NotNullOrEmpty().ToLower().Replace("%2f", "/").Replace("=mvc/", "=");
            var index = userAppUrl.IndexOf("url=", StringComparison.CurrentCultureIgnoreCase);
            if (index == -1 || userAppUrl.Length <= index + 4)
                return userAppUrl;

            userAppUrl = userAppUrl.SubstringTry(index + 4);

            return userAppUrl;
        }
    }
}