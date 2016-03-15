﻿using System;
using System.Collections.Generic;

namespace GeneralTools.Models
{
    public enum DateRangeType { None, CurrentYear, LastYear, Last3Months, Last2Months, LastMonth, CurrentMonth, Last6Months, Last90Days, Last60Days, Last30Days, Last7Days, Today, Yesterday }

    public class DateRangeCore
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateRangeType RangeType { get; set; }
    }

    public class DateRangePresets 
    {
        public Dictionary<DateRangeType, DateRangeCore> Presets { get; private set; }

        public DateRangePresets()
        {
            Presets = new Dictionary<DateRangeType, DateRangeCore>
                {
                    {
                        DateRangeType.CurrentYear, new DateRangeCore
                        {
                            RangeType = DateRangeType.None,
                            StartDate = new DateTime(DateTime.Today.Year, 1, 1),
                            EndDate = new DateTime(DateTime.Today.Year, 12, 31),
                        }
                    },
                    {
                        DateRangeType.LastYear, new DateRangeCore
                        {
                            RangeType = DateRangeType.None,
                            StartDate = new DateTime(DateTime.Today.Year - 1, 1, 1),
                            EndDate = new DateTime(DateTime.Today.Year - 1, 12, 31),
                        }
                    },
                    {
                        DateRangeType.Last3Months, new DateRangeCore
                        {
                            RangeType = DateRangeType.None,
                            StartDate = DateTime.Today.AddMonths(-3).MoveToFirstDay(),
                            EndDate = DateTime.Today.AddMonths(-1).MoveToLastDay(),
                        }
                    },
                    {
                        DateRangeType.Last2Months, new DateRangeCore
                        {
                            RangeType = DateRangeType.None,
                            StartDate = DateTime.Today.AddMonths(-2).MoveToFirstDay(),
                            EndDate = DateTime.Today.AddMonths(-1).MoveToLastDay(),
                        }
                    },
                    {
                        DateRangeType.LastMonth, new DateRangeCore
                        {
                            RangeType = DateRangeType.None,
                            StartDate = DateTime.Today.AddMonths(-1).MoveToFirstDay(),
                            EndDate = DateTime.Today.AddMonths(-1).MoveToLastDay(),
                        }
                    },
                    {
                        DateRangeType.CurrentMonth, new DateRangeCore
                        {
                            RangeType = DateRangeType.None,
                            StartDate = DateTime.Today.AddMonths(0).MoveToFirstDay(),
                            EndDate = DateTime.Today.AddMonths(0).MoveToLastDay(),
                        }
                    },
                    {
                        DateRangeType.Last6Months, new DateRangeCore
                        {
                            RangeType = DateRangeType.None,
                            StartDate = DateTime.Today.AddMonths(-6),
                            EndDate = DateTime.Today,
                        }
                    },
                    {
                        DateRangeType.Last90Days, new DateRangeCore
                        {
                            RangeType = DateRangeType.None,
                            StartDate = DateTime.Today.AddDays(-90),
                            EndDate = DateTime.Today,
                        }
                    },
                    {
                        DateRangeType.Last60Days, new DateRangeCore
                        {
                            RangeType = DateRangeType.None,
                            StartDate = DateTime.Today.AddDays(-60),
                            EndDate = DateTime.Today,
                        }
                    },
                    {
                        DateRangeType.Last30Days, new DateRangeCore
                        {
                            RangeType = DateRangeType.None,
                            StartDate = DateTime.Today.AddDays(-30),
                            EndDate = DateTime.Today,
                        }
                    },
                    {
                        DateRangeType.Last7Days, new DateRangeCore
                        {
                            RangeType = DateRangeType.None,
                            StartDate = DateTime.Today.AddDays(-7),
                            EndDate = DateTime.Today,
                        }
                    },
                    {
                        DateRangeType.Today, new DateRangeCore
                        {
                            RangeType = DateRangeType.None,
                            StartDate = DateTime.Today,
                            EndDate = DateTime.Today,
                        }
                    },
                    {
                        DateRangeType.Yesterday, new DateRangeCore
                        {
                            RangeType = DateRangeType.None,
                            StartDate = DateTime.Today.AddDays(-1),
                            EndDate = DateTime.Today.AddDays(-1),
                        }
                    },
                };
        }
    }
}

