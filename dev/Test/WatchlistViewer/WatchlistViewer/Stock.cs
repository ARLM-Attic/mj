﻿using System.Windows.Input;
using WpfTools4.Commands;
// ReSharper disable RedundantUsingDirective
using System.Windows.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using GeneralTools.Models;

namespace WatchlistViewer
{
    public class Stock : ModelBase 
    {
        private string _name;
        private string _wkn;
        private DateTime _dateTime;
        private double _value;
        private double _openValue;
        private double _topValue;
        private double _bottomValue;
        private double _change;

        public static double PixelAbsPercentChangeMaxForUI = 25;

        private readonly Dictionary<string, string> _nameTranslateDict = new Dictionary<string, string>
        {
            { "Goldpreis", "Gold~1326189~XAUUSD=X~0.0~2" },
            { "Euro / US", "€/US~1390634~EURUSD=X~0.0000~2" },
            { "DAX", "DAX~20735~%5EGDAXI~0~2" },
        };

        public string Name
        {
            get { return _name; }
            set { _name = value; SendPropertyChanged("Name"); SendPropertyChanged("ToolTip"); SendPropertyChanged("ShortName"); }
        }

        public string Wkn
        {
            get { return _wkn; }
            set { _wkn = value; SendPropertyChanged("Wkn"); SendPropertyChanged("ToolTip"); SendPropertyChanged("IdNotation"); }
        }

        public DateTime DateTime
        {
            get { return _dateTime; }
            set { _dateTime = value; SendPropertyChanged("DateTime"); SendPropertyChanged("ToolTip"); }
        }

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                SendPropertyChanged("Value");
                SendPropertyChanged("ValueFormatted");
                SendPropertyChanged("Change");
                SendPropertyChanged("ChangeFormatted");
                SendPropertyChanged("ForeColor");
                SendPropertyChanged("AbsPercentChangeForUI");
                SendPropertyChanged("PixelAbsPercentChangeForUI");
                SendPropertyChanged("ToolTipChangePercent");
            }
        }

        public double TopValue
        {
            get { return _topValue; }
            set
            {
                _topValue = value;
                SendPropertyChanged("TopValue");
                SendPropertyChanged("TopValueFormatted");
                SendPropertyChanged("Change");
                SendPropertyChanged("ChangeFormatted");
                SendPropertyChanged("ForeColor");
                SendPropertyChanged("AbsPercentChangeForUI");
                SendPropertyChanged("PixelAbsPercentChangeForUI");
                SendPropertyChanged("ToolTipChangePercent");
            }
        }

        public double BottomValue
        {
            get { return _bottomValue; }
            set
            {
                _bottomValue = value;
                SendPropertyChanged("BottomValue");
                SendPropertyChanged("BottomValueFormatted");
                SendPropertyChanged("Change");
                SendPropertyChanged("ChangeFormatted");
                SendPropertyChanged("ForeColor");
                SendPropertyChanged("AbsPercentChangeForUI");
                SendPropertyChanged("PixelAbsPercentChangeForUI");
                SendPropertyChanged("ToolTipChangePercent");
            }
        }

        public double OpenValue
        {
            get { return _openValue; }
            set 
            { 
                _openValue = value;
                SendPropertyChanged("OpenValue");
                SendPropertyChanged("OpenValueFormatted");
                SendPropertyChanged("Change");
                SendPropertyChanged("ChangeFormatted");
            }
        }

        public double PercentChange
        {
            get
            {
                if (Math.Abs(OpenValue) < 0.01)
                    return 0;

                return ((Value - OpenValue) * 100) / OpenValue;
            }
        }

        public double Change
        {
            get { return (Value - OpenValue); }
        }

        public Brush ForeColor
        {
            get { return (PercentChange < 0 ? Brushes.Red : Brushes.Green); }
        }

        public string ToolTip { get { return string.Format("Zeit {0:HH:mm:ss} vom {0:dd.MM.yyyy}", DateTime); } }

        public string ToolTipChangePercent
        {
            get
            {
                return $"% {PercentChange:0.00}   " +
                       $"^ {TopValueFormatted}   " +
                       $"¬ {BottomValueFormatted}   " +
                       $"       (Open: {OpenValueFormatted}  Change: {ChangeFormatted})";
            }
        }

        public string ShortName { get { return GetPartOfValue(0, "{self}"); } }

        public string IdNotation { get { return GetPartOfValue(1, ""); } }

        public string YahooSymbol { get { return GetPartOfValue(2, ""); } }

        public string ValueFormatted { get { return Value.ToString(GetPartOfValue(3, "#,##0.00")); } }
        public string OpenValueFormatted { get { return OpenValue.ToString(GetPartOfValue(3, "#,##0.00")); } }
        public string TopValueFormatted { get { return TopValue.ToString(GetPartOfValue(3, "#,##0.00")); } }
        public string BottomValueFormatted { get { return BottomValue.ToString(GetPartOfValue(3, "#,##0.00")); } }
        public string ChangeFormatted { get { return Change.ToString(GetPartOfValue(3, "#,##0.00")); } }

        public double AbsPercentChangeMaxForUI { get { return double.Parse(GetPartOfValue(4, "1.0")); } }

        public double AbsPercentChangeForUI
        {
            get
            {
                var absPercentChange = Math.Abs(PercentChange);
                if (absPercentChange > AbsPercentChangeMaxForUI)
                    return 1.0;

                return absPercentChange / AbsPercentChangeMaxForUI;
            }
        }

        public double PixelAbsPercentChangeForUI
        {
            get
            {
                if (Math.Abs(OpenValue) < 0.01)
                    return 0;

                return AbsPercentChangeForUI * PixelAbsPercentChangeMaxForUI;
            }
        }

        private string GetPartOfValue(int index, string defaultValue)
        {
            var key = _nameTranslateDict.Keys.FirstOrDefault(k => Name.Contains(k));
            if (key == null)
                return GetDefaultValue(Name, defaultValue);

            var val = _nameTranslateDict[key];
            if (!val.Contains("~"))
                return GetDefaultValue(val, defaultValue);

            var valArray = val.Split('~');
            if (index >= valArray.Length)
                return GetDefaultValue(val, defaultValue);

            return valArray[index];
        }

        static string GetDefaultValue(string val, string defaultValue)
        {
            if (defaultValue == "{self}")
                return val;
            
            return defaultValue;
        }

        public MainViewModel Parent { get; set; }

        public ICommand ShowWknAtComdirectCommand { get; private set; }

        public Stock()
        {
            ShowWknAtComdirectCommand = new DelegateCommand(e => Parent.ShowWknAtComdirect(this), e => true);
        }
    }
}