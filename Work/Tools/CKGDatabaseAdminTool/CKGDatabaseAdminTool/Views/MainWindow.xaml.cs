﻿using System.Reflection;
using System.Windows;
using CKGDatabaseAdminLib.ViewModels;
using CKGDatabaseAdminTool.UIServices;

namespace CKGDatabaseAdminTool
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public bool IsDbSelected { get; set; }

        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;

            if (vm.UseDefaultDbServer)
            {
                IsDbSelected = true;
                vm.SelectDbConnection();
            }
            else
                ShowDbSelectionDialog();

            Title = string.Format("CKGDatabaseAdminTool v{0}.{1}, Entwickler = {2}",
                                        Assembly.GetEntryAssembly().GetName().Version.Major,
                                        Assembly.GetEntryAssembly().GetName().Version.Minor.ToString("00"),
                                        vm.Developer);
        }

        private void ShowDbSelectionDialog()
        {
            var vm = (DataContext as MainViewModel);
            var dResult = DialogService.ShowDialog(ref vm, DialogTypes.DbSelection);
            if (dResult.HasValue && dResult.Value)
            {
                vm.SelectDbConnection();
                IsDbSelected = true;
            }
            else
            {
                IsDbSelected = false;
            }
        }

        private void btnChangeDbConnnection_OnClick(object sender, RoutedEventArgs e)
        {
            ShowDbSelectionDialog();
        }
    }
}