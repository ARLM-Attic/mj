﻿using System;

namespace CkgDomainLogic.General.Contracts
{
    public interface IDashboardItemOptions
    {
        int RowSpan { get; set; }

        bool IsAuthorized { get; set; }

        string AuthorizedIfAppUrlsAuth { get; set; }

        string ItemType { get; set; }

        int JsonDataCacheExpirationMinutes { get; set; }

        bool IsChart { get; }

        bool IsPartialView { get; }

        string SettingsScriptFunction { get; set; }

        bool JsonDataCacheExpired(DateTime? dt);
    }
}