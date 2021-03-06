﻿//#define __TEST

// ReSharper disable RedundantUsingDirective
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GeneralTools.Models;
using GeneralTools.Services;
using WpfTools4.Commands;
using WpfTools4.Services;

namespace PdfPrint
{
    public class MainViewModel : ModelBase
    {
        private bool _isPrinting;
        private int _page;
        private int _startPage;
        private int _endPage;
        private string _busyHint;
        private string _pdfFileName;
        private string _pdfPrintRange;


        public string PdfPrintRange
        {
            get { return _pdfPrintRange; }
            set
            {
                _pdfPrintRange = value;
                SendPropertyChanged("PdfPrintRange");
                SendPropertyChanged("PrintingEnabled");
            }
        }

        public string PdfFileName
        {
            get { return _pdfFileName; }
            set
            {
                _pdfFileName = value;
                SendPropertyChanged("PdfFileName");
                SendPropertyChanged("PdfFileNameIsValid");
                SendPropertyChanged("PrintingEnabled");
            }
        }

        public bool IsPrinting
        {
            get { return _isPrinting; }
            set
            {
                _isPrinting = value;
                SendPropertyChanged("IsPrinting");
                SendPropertyChanged("PrintingEnabled");
            }
        }

#if __TEST
        public string TestHint => "TEST";
#else
        public string TestHint => "";
#endif

        public bool PdfPrintRangeIsValid => PdfPrintRange.NotNullOrEmpty().Replace("-", "").Replace(" ", "").ToInt(0) > 0;

        public bool PdfFileNameIsValid => PdfFileName.IsNotNullOrEmpty();

        public bool PrintingEnabled => !IsPrinting && PdfFileNameIsValid && PdfPrintRangeIsValid;

        public int Page
        {
            get { return _page; }
            set { _page = value; SendPropertyChanged("Page"); SendPropertyChanged("PageHint"); SendPropertyChanged("PageHint2"); }
        }

        public string PageHint => Page > EndPage ? "Fertig! Alle Seiten gedruckt!" : $"Drucke Seite {Page}";

        public string PageHint2 => $"(von {StartPage}-{EndPage})";

        public int StartPage
        {
            get { return _startPage; }
            set { _startPage = value; SendPropertyChanged("StartPage"); SendPropertyChanged("ProgressStart"); }
        }

        public int EndPage
        {
            get { return _endPage; }
            set { _endPage = value; SendPropertyChanged("EndPage"); SendPropertyChanged("ProgressEnd"); }
        }

        public int ProgressStart => StartPage;
        public int ProgressEnd => EndPage + 1;

        public string BusyHint
        {
            get { return _busyHint; }
            set { _busyHint = value; SendPropertyChanged("BusyHint"); }
        }

        public static bool RequestBusyHint { get; set; }
        public ICommand PrintCommand { get; private set; }
        public ICommand SetPdfNameCommand { get; private set; }
        public ICommand ResetPdfNameCommand { get; private set; }


        public MainViewModel()
        {
            PrintCommand = new DelegateCommand(e => Print(), e => true);
            SetPdfNameCommand = new DelegateCommand(e => SetPdfName(), e => true);
            ResetPdfNameCommand = new DelegateCommand(e => ResetPdfName(), e => true);

            TaskService.InitUiSynchronizationContext();
            PdfFileName = Properties.Settings.Default.PdfFileName;
        }

        private void SetPdfName()
        {
            var fileName = Helper.GetPdfFilenameFromDialog(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PDF Datei zum Drucken wählen");
            if (fileName == null) return;

            Properties.Settings.Default.PdfFileName = PdfFileName = fileName;
            Properties.Settings.Default.Save();
        }

        void ResetPdfName()
        {
            Properties.Settings.Default.PdfFileName = PdfFileName = "";
            Properties.Settings.Default.Save();
        }

        private void Print()
        {
            IsPrinting = true;
            Page = 0;
            BusyHint = "";

            if (PdfPrintRange.IsNullOrEmpty()) return;
            if (PdfPrintRange.Contains("-"))
            {
                var range = PdfPrintRange.Replace(" ", "").Split('-');
                StartPage = range[0].ToInt();
                if (range.Length > 1)
                    EndPage = range[1].ToInt();
            }
            else
            {
                StartPage = PdfPrintRange.Replace(" ", "").ToInt();
                EndPage = StartPage;
            }

            if (StartPage < 1)
                StartPage = 1;
            if (EndPage < StartPage)
                EndPage = StartPage;

            var errorOccured = false;
            TaskService.StartLongRunningTask(() =>
            {
                for (var i = StartPage; i <= EndPage; i++)
                {
                    Page = i;
                    Thread.Sleep(10);

                    var requestBusyHint = false;
                    Application.Current.Dispatcher.Invoke((() => requestBusyHint = RequestBusyHint));
                    if (requestBusyHint)
                        BusyHint =
                            "Hinweis:\r\nAktive Druckprozesse müssen erst noch\r\nabgeschlossen werden!\r\nBitte versuchen Sie in ein paar Augenblicken\r\nnoch einmal die Anwendung zu schließen.";

#if __TEST
                    Thread.Sleep(700);
#else
                    var pdfOutFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"CkgPdfPrint-{Guid.NewGuid()}.pdf");
                    if (!Helper.PdfSplitDocument(PdfFileName, pdfOutFileName, i))
                    {
                        errorOccured = true;
                        Application.Current.Dispatcher.Invoke((() => requestBusyHint = RequestBusyHint));
                        BusyHint = "Fehler beim Drucken der gewünschten Seite!\r\nSind alle zu druckenden Seiten im Dokument\r\ntatsächlich vorhanden?";
                        break;
                    }

                    Helper.PdfPrint(pdfOutFileName);
                    FileService.TryFileDelete(pdfOutFileName);
#endif
                }
            })
            .ContinueWith(t =>
            {
                if (!errorOccured)
                    Page++;

                Thread.Sleep(3000);
            })
            .ContinueWith(t =>
            {
                BusyHint = "";
                RequestBusyHint = false;
                IsPrinting = false;
                Page = 0;
            }, TaskScheduler.FromCurrentSynchronizationContext()
            );
        }
    }
}
