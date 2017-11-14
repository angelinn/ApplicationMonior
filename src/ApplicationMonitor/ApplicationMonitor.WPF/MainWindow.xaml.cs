using ApplicationMonitor.WPF.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApplicationMonitor.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel MainViewModel { get; private set; }

        private System.Windows.Forms.NotifyIcon notifyIcon;
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel = DataContext as MainViewModel;
            MainViewModel.OnSecondaryProcessStart += OnSecondaryProcessStart;

            Application.Current.Exit += OnApplicationExit;

            notifyIcon = new System.Windows.Forms.NotifyIcon
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

        private void OnSecondaryProcessStart(object sender, Process e)
        {
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(e.MainModule.FileName);
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
            try
            {
                MainViewModel.ShowMonitored(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnMinimizeMonitoredToTrayClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                MainViewModel.ShowMonitored(false);
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void OnStartClicked(object sender, RoutedEventArgs e)
        {
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(MainViewModel.FilePath);

            try
            {
                await MainViewModel.StartAsync();
                MainViewModel.ShowMonitored(false);
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnBrowseClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
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
