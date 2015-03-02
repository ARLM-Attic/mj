﻿using System.Windows;
// ReSharper disable RedundantUsingDirective
using System.Diagnostics;
using GeneralTools.Models;
using WpfTools4.ViewModels;
using System.Windows.Input;
using WpfTools4.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchlistViewer
{
    public class MainViewModel : ViewModelBase
    {
        private List<Stock> _stockItems;

        public List<Stock> StockItems
        {
            get { return _stockItems; }
            set { _stockItems = value; SendPropertyChanged("StockItems"); }
        }

        public ICommand WatchlistShowCommand { get; private set; }
        public ICommand WatchlistHideCommand { get; private set; }
        public ICommand GetStockDataCommand { get; private set; }

        public ICommand QuitCommand { get; private set; }

        private readonly System.Windows.Forms.Timer _initialDelayTimer;
        private System.Windows.Forms.Timer _workTimer;

        public MainViewModel()
        {
            WatchlistShowCommand = new DelegateCommand(e => WatchlistShow(), e => true);
            WatchlistHideCommand = new DelegateCommand(e => WatchlistHide(), e => true);
            GetStockDataCommand = new DelegateCommand(e => GetStockData(), e => true);
            QuitCommand = new DelegateCommand(e => Quit(), e => true);

#if TEST
            _initialDelayTimer = new System.Windows.Forms.Timer { Enabled = true, Interval = 20 };
#else
            FirefoxWebDriver.InvokeWatchlist();
            _initialDelayTimer = new System.Windows.Forms.Timer { Enabled = true, Interval = 2000 };
#endif
            _initialDelayTimer.Tick += InitialDelayTimerTick;
        }

        void InitialDelayTimerTick(object sender, EventArgs e)
        {
            _initialDelayTimer.Stop();
            _initialDelayTimer.Dispose();

            WatchlistHide();
            _workTimer = new System.Windows.Forms.Timer { Enabled = true, Interval = 1000 };
            _workTimer.Tick += WorkTimerTick;
        }

        void WorkTimerTick(object sender, EventArgs e)
        {
            TaskService.StartLongRunningTask(GetStockData);
        }

        private static void WatchlistShow()
        {
            FirefoxWebDriver.ShowBrowser();
        }

        private static void WatchlistHide()
        {
            FirefoxWebDriver.HideBrowser();
        }

        private void GetStockData()
        {
            var items = FirefoxWebDriver.GetStockData();

            if (StockItems == null || StockItems.None())
            {
                items.ForEach(item => item.Parent = this);
                StockItems = items;
                return;
            }

            items.ForEach(item =>
            {
                var stockItem = StockItems.FirstOrDefault(si => si.Name == item.Name);
                if (stockItem == null)
                    return;
                
                ModelMapping.Copy(item, stockItem);
                stockItem.Parent = this;
            });
        }

        public void ShowWknAtComdirect(Stock stock)
        {
            MessageBox.Show(stock.TargetWkn);
        }

        private static void Quit()
        {
            ProcessHelper.KillAllProcessesOf("FireFox");
            Process.GetCurrentProcess().Kill();
        }
    }
}
