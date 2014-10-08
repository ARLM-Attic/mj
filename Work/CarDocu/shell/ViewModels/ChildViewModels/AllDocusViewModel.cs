﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using CarDocu.Models;
using CarDocu.Services;
using GeneralTools.Models;
using WpfTools4.Commands;
using WpfTools4.Services;
using WpfTools4.ViewModels;
using Tools = CarDocu.Services.Tools;

namespace CarDocu.ViewModels
{
    public class AllDocusViewModel : ViewModelBase, IMainViewModelChild
    {
        #region Properties

        public MainViewModel Parent { get; set; }

        public string Header
        {
            get { return ModeTemplateItems ? "Verfügbare Vorlagen" : "Bisher erfasste Scan-Dokumente"; }
        }

        private ObservableCollection<ScanDocument> _scanItems;
        public ObservableCollection<ScanDocument> ScanItems
        {
            get
            {
                if (_scanItems != null)
                    return _scanItems;

                _scanItems = new ObservableCollection<ScanDocument>(DomainService.Repository.ScanDocumentRepository.ScanDocuments);

                return _scanItems;
            }
        }

        private ObservableCollection<ScanDocument> _templatetems;
        public ObservableCollection<ScanDocument> TemplateItems
        {
            get
            {
                if (_templatetems != null)
                    return _templatetems;

                _templatetems = new ObservableCollection<ScanDocument>(DomainService.Repository.ScanTemplateRepository.ScanDocuments);

                return _templatetems;
            }
        }

        public ObservableCollection<ScanDocument> Items
        {
            get
            {
                var items = ModeTemplateItems ? TemplateItems : ScanItems;
                
                var view = CollectionViewSource.GetDefaultView(items);
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("CreateDate", ListSortDirection.Descending));
                view.Refresh();

                return items;
            }
        }

        public bool ItemsAvailable { get { return Items.Any(); } }

        private bool _modeTemplate;
        public bool ModeTemplateItems
        {
            get { return _modeTemplate; }
            set
            {
                _modeTemplate = value;
                SendPropertyChanged("ModeTemplateItems");
                SendPropertyChanged("ModeScanItems");
                SendPropertyChanged("Header");
                SendPropertyChanged("Details");
                SendPropertyChanged("Items");
                SendPropertyChanged("ItemsAvailable");
            }
        }

        private bool _modeScanItems = true;
        public bool ModeScanItems
        {
            get { return _modeScanItems; }
            set
            {
                _modeScanItems = value;
                ModeTemplateItems = !_modeScanItems;
            }
        }

        private ScanDocument _selectedItem;
        public ScanDocument SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; SendPropertyChanged("SelectedItem"); }
        }

        public ICommand ScanDocuNewCommand { get; private set; }
        public ICommand ScanDocuLoadCommand { get; private set; }
        public ICommand ScanDocuDeleteCommand { get; private set; }
        public ICommand ScanPdfFolderOpenCommand { get; private set; }

        public ICommand ZipToArchiveCommand { get; private set; }

        #endregion


        public AllDocusViewModel()
        {
            ScanDocuNewCommand = new DelegateCommand(e => ScanDocuNew(), e => CanScanDocuNew());
            ScanDocuLoadCommand = new DelegateCommand(e => ScanDocuLoad(), e => CanScanDocuLoad());
            ScanDocuDeleteCommand = new DelegateCommand(e => ScanDocuDelete(), e => CanScanDocuDelete());
            ScanPdfFolderOpenCommand = new DelegateCommand(e => DomainService.Repository.ScanPdfFolderOpen());
            ZipToArchiveCommand = new DelegateCommand(e => ZipArchiveUserLogsAndScanDocuments());

            DomainService.Repository.ScanDocumentRepository.OnAddScanDocument += sd => { Items.Add(sd); SendPropertyChanged("ItemsAvailable"); };
            DomainService.Repository.ScanDocumentRepository.OnDeleteScanDocument += sd => { Items.Remove(sd); SendPropertyChanged("ItemsAvailable"); };
            
            DomainService.Repository.ScanTemplateRepository.OnAddScanDocument += sd => { Items.Add(sd); SendPropertyChanged("ItemsAvailable"); };
            DomainService.Repository.ScanTemplateRepository.OnDeleteScanDocument += sd => { Items.Remove(sd); SendPropertyChanged("ItemsAvailable"); };
        }

        static void ZipArchiveUserLogsAndScanDocuments()
        {
            if (DomainService.Threads.IsBusy)
            {
                Tools.Alert("Hinweis:\r\n\r\nAktive Hintergrundprozesse müssen erst noch abgeschlossen werden!\r\n\r\nBitte versuchen Sie es in ein paar Augenblicken noch einmal.");
                return;
            }

            if (DomainService.Repository.ScanDocumentRepository.ScanDocuments.None())
            {
                Tools.Alert("Hinweis:\r\n\r\nEine ZIP-Archivierung ist nicht erforderlich, da seit der letzten Archivierung keine Scan-Dokumente erfasst wurden.");
                return;
            }

            if (!Tools.Confirm("Alle Scan-Dokumente aus dem Ablage-Archiv werden nun in das ZIP-Archiv verschoben.\r\n\r\nWeiter?"))
                return;

            ProgressBarOperation.Start(DomainService.Repository.ZipArchiveUserLogsAndScanDocuments, ZipArchiveUserLogsAndScanDocumentsComplete);
        }

        static void ZipArchiveUserLogsAndScanDocumentsComplete(ProgressBarOperation progressBarOperation)
        {
            if (!progressBarOperation.TaskResult)
            {
                Tools.Alert("Die ZIP-Archivierung wurde abgebrochen!");
                return;
            }

            Tools.Alert("Die ZIP-Archivierung wurde erfolgreich durchgeführt!\r\n\r\nHinweis: Die Anwendung wird nun neu gestartet!");
            DomainService.Repository.ScanDocumentRepository.ScanDocuments.RemoveAll(d => true);
            DomainService.Repository.UserSettingsSave();
            DomainService.Repository.LogItemFilesDelete();
            App.Restart();
        }

        void ScanDocuNew()
        {
            Parent.RibbonSelectedTabChanged(true, ModeTemplateItems);
        }

        static bool CanScanDocuNew()
        {
            return true;
        }

        void ScanDocuLoad()
        {
            Parent.LoadStoredScanDocu(SelectedItem);
        }

        bool CanScanDocuLoad()
        {
            return (SelectedItem != null);
        }

        void ScanDocuDelete()
        {
            if (!Tools.Confirm("Wollen Sie das markierte Dokument inkl. aller hierfür gescannten Dokumente wirklich endgültig löschen?"))
                return;

            if (DomainService.Repository.ScanDocumentRepositoryTryDeleteScanDocument(SelectedItem))
            {
                DomainService.Repository.ScanDocumentRepositorySave();
            }
        }

        bool CanScanDocuDelete()
        {
            return (SelectedItem != null);
        }

        public void ReloadItems()
        {
            _scanItems = null;
            _templatetems = null;
        }

        public void RefreshSelectedItem(ScanDocument scanDocument)
        {
            var selItem = Items.ToList().FirstOrDefault(sd => sd.DocumentID == scanDocument.DocumentID);
            Items.Remove(selItem);
            Items.Add(scanDocument);
            SelectedItem = scanDocument;

            DomainService.Repository.ScanDocumentRepositorySave();
        }
    }
}