using MonitoringService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationMonitor.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ApplicationMonitoring applicationMonitoring = new ApplicationMonitoring();

        public MainViewModel()
        {
            applicationMonitoring.OnApplicationStarted += OnApplicationStarted;
            applicationMonitoring.OnApplicationExited += OnApplicationExited;
        }

        private void OnApplicationExited(object sender, EventArgs e)
        {
            Log += $"[{DateTime.Now}] Application exited.{Environment.NewLine}";
        }

        private void OnApplicationStarted(object sender, EventArgs e)
        {
            Log += $"[{DateTime.Now}] Application started.{Environment.NewLine}";
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

        public void Start()
        {
            applicationMonitoring.Begin(filePath, appName);
        }

        public void ShowMonitored(bool show)
        {
            applicationMonitoring.ShowMonitored(show);
        }
    }
}
