﻿using System.Windows;
using System.Windows.Input;

namespace WatchlistViewer
{
    public partial class MainWindow 
    {
        public static ICommand ContextMenuItemCommand { get; private set; } 
         
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            const int marginRight = 0;

            var desktopWorkingArea = SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Width / 2 - e.NewSize.Width / 2 - marginRight;
        }
    }
}
