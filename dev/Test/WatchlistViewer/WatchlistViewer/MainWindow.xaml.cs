﻿namespace WatchlistViewer
{
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
    }
}
