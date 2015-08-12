﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Services
{
    public class ChartService
    {
        public static ChartItemsPackage GetPieChartGroupedItemsWithLabels<T>
            (
                IList<T> items,
                Func<T, string> xAxisKey,
                Func<IGrouping<int, T>, int> aggregate = null
            ) where T : class
        {
            var xAxisList = items./*OrderBy(xAxisKey).*/GroupBy(xAxisKey).Select(k => k.Key).ToListOrEmptyList();
            var xAxisLabels = xAxisList.ToArray();

            var groupArray = items
                /*.OrderBy(xAxisKey)*/
                .GroupBy(group => xAxisList.IndexOf(xAxisKey(group)))
                .Select(g => new[] { g.Key, (aggregate == null ? g.Count() : aggregate(g)) })
                .ToArray();

            var data = new object[groupArray.Count()];
            for (var k = 0; k < groupArray.Count(); k++)
            {
                data[k] = new
                    {
                        data = new [] { groupArray[k] },
                        label = xAxisLabels[k]
                    };
            }

            return new ChartItemsPackage
            {
                data = data
            };
        }

        public static ChartItemsPackage GetBarChartGroupedStackedItemsWithLabels<T>
            (
            IList<T> items,
            Func<T, string> xAxisKey,
            Action<IList<string>> addAdditionalXaxisKeys = null,
            Func<T, string> stackedKey = null,
            Func<IGrouping<int, T>, int> aggregate = null
            ) where T : class
        {
            var xAxisList = items./*OrderBy(xAxisKey).*/GroupBy(xAxisKey).Select(k => k.Key).ToListOrEmptyList();
            if (xAxisList.Any() && addAdditionalXaxisKeys != null)
                addAdditionalXaxisKeys(xAxisList);
            var xAxisLabels = xAxisList.ToArray();

            if (stackedKey == null)
                stackedKey = e => "";

            var stackedGroupValues = items.GroupBy(stackedKey)./*OrderBy(k => k.Key).*/Select(k => k.Key);
            var stackedGroupValuesArray = stackedGroupValues.ToArray();

            var data = new object[stackedGroupValues.Count()];
            for (var k = 0; k < stackedGroupValues.Count(); k++)
            {
                var subArray = items
                    .Where(item => stackedKey(item) == stackedGroupValuesArray[k])
                    .GroupBy(group => xAxisList.IndexOf(xAxisKey(group)))
                    .Select(g => new[] { g.Key, (aggregate == null ? g.Count() : aggregate(g)) })
                    .ToArray();

                data[k] = new
                {
                    data = subArray,
                    label = stackedGroupValuesArray[k]
                };
            }

            return new ChartItemsPackage
            {
                data = data,
                labels = xAxisLabels
            };
        }


        public static object PrepareChartDataAndOptions(ChartItemsPackage data, string dataPath, string chartTemplate)
        {
            var chartOptionsFileName = Path.Combine(dataPath, "DashBoard", "ChartTemplates", string.Format("{0}.txt", chartTemplate));
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
