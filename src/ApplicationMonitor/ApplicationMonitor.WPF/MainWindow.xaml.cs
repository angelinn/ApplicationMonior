using ApplicationMonitor.WPF.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using OpenFile = Microsoft.Win32.OpenFileDialog;

namespace ApplicationMonitor.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel MainViewModel { get; private set; } = new MainViewModel();

        private NotifyIcon notifyIcon;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MainViewModel;

            System.Windows.Application.Current.Exit += OnApplicationExit;

            notifyIcon = new NotifyIcon
            {
                Visible = true,
            };

            notifyIcon.DoubleClick += ResumeApp;

            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(
                new System.Windows.Forms.MenuItem[]
                {
                    new System.Windows.Forms.MenuItem("Application Monitor", ResumeApp),
                    new System.Windows.Forms.MenuItem("Monitored application", ResumeMonitored)
                });
        }

        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            MainViewModel.ShowMonitored(true);
        }

        private void ResumeApp(object sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        private void ResumeMonitored(object sender, EventArgs e)
        {
            MainViewModel.ShowMonitored(true);
        }

        private void OnMinimizeMonitoredToTrayClicked(object sender, RoutedEventArgs e)
        {
            MainViewModel.ShowMonitored(false);
        }

        private void OnStartClicked(object sender, RoutedEventArgs e)
        {
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(MainViewModel.FilePath);
            MainViewModel.Start();
        }

        private void OnBrowseClicked(object sender, RoutedEventArgs e)
        {
            OpenFile dialog = new OpenFile();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                MainViewModel.FilePath = dialog.FileName;
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                Hide();

            base.OnStateChanged(e);
        }
    }
}
