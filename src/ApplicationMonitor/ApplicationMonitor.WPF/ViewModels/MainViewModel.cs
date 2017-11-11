using MonitoringService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationMonitor.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ApplicationMonitoring applicationMonitoring = new ApplicationMonitoring();

        public event EventHandler<Process> OnSecondaryProcessStart;

        public MainViewModel()
        {
            applicationMonitoring.OnApplicationStarted += OnApplicationStarted;
            applicationMonitoring.OnApplicationExited += OnApplicationExited;
        }

        private void OnApplicationExited(object sender, Process e)
        {
            Log += $"[{DateTime.Now}] {e.ProcessName} exited.{Environment.NewLine}";
        }

        private void OnApplicationStarted(object sender, Process e)
        {
            Log += $"[{DateTime.Now}] {e.ProcessName} started.{Environment.NewLine}";
            OnSecondaryProcessStart?.Invoke(this, e);
        }

        private string log;
        public string Log
        {
            get
            {
                return log;
            }
            set
            {
                log = value;
                OnPropertyChanged();
            }
        }

        private string filePath;
        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
                OnPropertyChanged();
            }
        }

        private string appName;
        public string AppName
        {
            get
            {
                return appName;
            }
            set
            {
                appName = value;
                OnPropertyChanged();
            }
        }

        public async Task StartAsync()
        {
            await applicationMonitoring.BeginAsync(filePath, appName);
        }

        public void ShowMonitored(bool show)
        {
            applicationMonitoring.ShowMonitored(show);
        }
    }
}
