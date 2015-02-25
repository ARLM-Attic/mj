﻿// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using GeneralTools.Models;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.General.Services;

namespace CkgDomainLogic.DomainCommon.ViewModels
{
    public class DashboardViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IDashboardDataService DataService { get { return CacheGet<IDashboardDataService>(); } }


        public List<IDashboardItem> DashboardItems
        {
            get { return PropertyCacheGet(() => FilterItemsAvailableForUser(DataService.GetDashboardItems(LogonContext.UserName))); }
        }

        public List<IDashboardItem> VisibleSortedDashboardItems
        {
            get { return DashboardItems.Where(item => item.IsUserVisible).OrderBy(item => item.UserSort).ToList(); }
        }

        public List<IDashboardItem> HiddenDashboardItems
        {
            get { return DashboardItems.Where(item => !item.IsUserVisible).ToList(); }
        }

        public void DataInit()
        {
            DataMarkForRefresh();

            DashboardSessionSaveAllItems(DashboardItems);
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.DashboardItems);
        }

        List<IDashboardItem> FilterItemsAvailableForUser(IList<IDashboardItem> items)
        {
            if (LogonContext.UserApps == null)
                return items.ToList();

            return items.Where(item => LogonContext.UserApps.Any(userApp => userApp.AppURL.ToLower().Contains(item.RelatedAppUrl.ToLower()))).ToList();
        }

        public void DashboardItemsSave(string commaSeparatedIds)
        {
            DataService.SaveDashboardItems(DashboardItems, LogonContext.UserName, commaSeparatedIds);
        }

        public object GetBarChartData(string id)
        {
            var dbId = id.Replace("id_", "").Replace("#", "");
            var dashboardItem = DashboardItems.FirstOrDefault(item => item.ID == dbId.ToInt());

            if (dashboardItem == null)
                return new { };

            var data = DashboardAppUrlService.InvokeViewModelForAppUrl(dashboardItem.RelatedAppUrl, dashboardItem.Title);

            var chartOptionsFileName = Path.Combine(AppSettings.DataPath, "DashBoard", "ChartTemplates",
                                        string.Format("{0}.txt", dashboardItem.ChartJsonOptions));
            if (!File.Exists(chartOptionsFileName))
                return new { };

            var options = File.ReadAllText(chartOptionsFileName);

            if (options.NotNullOrEmpty().Contains("@ticks") && data.labels != null)
            {
                // label array json format, as string: "[[0,\"label 1\"], [1,\"label 2\"], [2,\"label 3\"]]"
                var labelArray = data.labels;
                options = options.Replace("@ticks",
                    string.Format("[{0}]",
                        string.Join(",", labelArray.Select(s => string.Format("[{0},\"{1}\"]", labelArray.ToList().IndexOf(s), s)))));
            }

            return new { data, options };
        }
    }
}
