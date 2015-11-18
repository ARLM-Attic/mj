﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using WpfTools4.Services;

namespace MyBoss
{
    public partial class MainWindow
    {
        private LowLevelKeyboardListener _listener;
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private System.Windows.Forms.Timer _t;
        private double _lastTicks1, _lastTicks2, _lastTicks3, _lastTicks4;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _listener = new LowLevelKeyboardListener();
            _listener.OnKeyPressed += _listener_OnKeyPressed;
            _listener.HookKeyboard();

            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.Click += notifyIcon_Click;
            var icon = Application.GetResourceStream(new Uri("pack://application:,,,/MyBoss;component/boss.ico"));
            if (icon != null)
                _notifyIcon.Icon = new System.Drawing.Icon(icon.Stream);

            _notifyIcon.Visible = true;
            Hide();
        }

        const string OutlookProcessFullName = "outlook";

        static void KillOutlook()
        {
            var outlookProcess = Process.GetProcesses().FirstOrDefault(p => OutlookProcessFullName.Contains(p.ProcessName.ToLower()));
            if (outlookProcess != null)
                outlookProcess.Kill();
        }

        public static void StartOutlook()
        {
            Process.Start(OutlookProcessFullName + ".exe");
        }

        bool _listener_OnKeyPressed(KeyPressedArgs e)
        {
            if (LowLevelKeyboardListener.Disabled)
                return false;

            if (TryCheckCtrlAltKeyPressAction(e, Key.T, () =>
                    {
                        notifyIcon_Click(null, null);
                        Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "todo.txt"));
                    }))
                return true;

            if (TryCheckCtrlAltKeyPressAction(e, Key.G, () =>
                    {
                        var pi = new ProcessStartInfo
                        {
                            FileName = @"C:\Program Files (x86)\Git\bin\sh.exe",
                            Arguments = "--login -i",
                            WorkingDirectory = @"c:\dev"
                        };
                        Process.Start(pi);
                    }))
                return false;

            if (TryCheckCtrlAltKeyPressAction(e, Key.C, () =>
                    {
                        Clipboard.SetText("seE17igEl");
                    }))
                return false;

            

            TryCheckDoubleTimeKeyPressAction(e, ref _lastTicks1, Key.LeftAlt, () =>
            {
                LowLevelKeyboardListener.Disabled = true;
                Tools.ShowDesktop();
                Thread.Sleep(50);
                new FakeWindow("fake_wallpaper.png").Show();
            });

            TryCheckDoubleTimeKeyPressAction(e, ref _lastTicks2, Key.LeftShift, () =>
            {
                LowLevelKeyboardListener.Disabled = true;
                KillOutlook();
                Tools.ShowDesktop();
                Thread.Sleep(50);
                new FakeWindow("fake_lockscreen.png").Show();
            });

            TryCheckDoubleTimeKeyPressAction(e, ref _lastTicks3, Key.RightCtrl, () =>
            {
                notifyIcon_Click(null, null);
            });

            TryCheckDoubleTimeKeyPressAction(e, ref _lastTicks4, Key.RightShift, Close);

            return false;
        }

        static bool TryCheckCtrlAltKeyPressAction(KeyPressedArgs e, Key key, Action action)
        {
            if (!(Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftAlt) && e.KeyPressed == key))
                return false;

            action();

            Thread.Sleep(1000);
            return true;
        }

        static void TryCheckDoubleTimeKeyPressAction(KeyPressedArgs e, ref double lastTicks, Key key, Action action)
        {
            if (e.KeyPressed != key)
            {
                lastTicks = 0;
                return;
            }

            var ticksNow = (DateTime.Now - DateTime.MinValue).TotalMilliseconds;
            if (lastTicks > 0 && (ticksNow - lastTicks > 100) && (ticksNow - lastTicks < 200))
                action();

            lastTicks = ticksNow;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _listener.UnHookKeyboard();

            _notifyIcon.Visible = false;
            _notifyIcon = null;
        }

        void notifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;

            _t = new System.Windows.Forms.Timer
            {
                Enabled = true,
                Interval = 2000
            };
            _t.Tick += T_Tick;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            Hide();

            _t.Stop();
            _t.Dispose();
        }
    }
}
