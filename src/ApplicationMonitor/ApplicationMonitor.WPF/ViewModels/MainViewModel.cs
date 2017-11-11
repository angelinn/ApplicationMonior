using MonitoringService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApplicationMonitor.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ApplicationMonitoring applicationMonitoring = new ApplicationMonitoring();

        public ICommand BrowseCommand { get; set; }
        public ICommand StartCommand { get; set; }

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
            applicationMonitoring.Begin(filePath);
        }

        public void ShowMonitored(bool show)
        {
            applicationMonitoring.ShowMonitored(show);
        }
    }
}
