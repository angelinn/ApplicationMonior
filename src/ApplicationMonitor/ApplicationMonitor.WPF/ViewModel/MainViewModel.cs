using ApplicationMonitor.WPF.Models;
using ApplicationMonitor.WPF.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MonitoringService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApplicationMonitor.WPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ApplicationMonitoring applicationMonitoring = new ApplicationMonitoring();
        private readonly InteractionService interactionService;

        public ICommand StartCommand { get; private set; }
        public ICommand HideCommand { get; private set; }
        public ICommand BrowseCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }

        public MainViewModel(InteractionService interaction)
        {
            interactionService = interaction;
            interactionService.CreateTrayIcon(ResumeWindow, new KeyValuePair<string, Action>[]
            {
                new KeyValuePair<string, Action>("Application Monitor", ResumeWindow),
                new KeyValuePair<string, Action>("Monitored application", () => ShowMonitored(true))
            });

            StartCommand = new RelayCommand(StartAsync);
            HideCommand = new RelayCommand(MinimizeToTray);
            CloseCommand = new RelayCommand(() => ShowMonitored(true));
            BrowseCommand = new RelayCommand(PromptForFileName);

            IsVisible = true;
            WindowState = ApplicationMonitorWindowState.Normal;

            applicationMonitoring.OnApplicationStarted += OnApplicationStarted;
            applicationMonitoring.OnApplicationExited += OnApplicationExited;
        }

        private void ResumeWindow()
        {
            IsVisible = true;
            WindowState = ApplicationMonitorWindowState.Normal;
        }

        private void HideWindow()
        {
            IsVisible = false;
        }

        private void PromptForFileName()
        {
            FilePath = interactionService.PromptForFileName();
        }

        private void MinimizeToTray()
        {
            try
            {
                ShowMonitored(false);
                IsVisible = false;
            }
            catch (Exception ex)
            {
                interactionService.MessageBoxOK(ex.Message);
            }
        }

        private void OnApplicationExited(object sender, Process e)
        {
            Log += $"[{DateTime.Now}] {e.ProcessName} exited.{Environment.NewLine}";
        }

        private void OnApplicationStarted(object sender, Process e)
        {
            Log += $"[{DateTime.Now}] {e.ProcessName} started.{Environment.NewLine}";
            interactionService.SetTrayIcon(e.MainModule.FileName);
        }

        private ApplicationMonitorWindowState windowState;
        public ApplicationMonitorWindowState WindowState
        {
            get
            {
                return windowState;
            }
            set
            {
                windowState = value;
                if (windowState == ApplicationMonitorWindowState.Minimized)
                    IsVisible = false;

                RaisePropertyChanged();
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
                RaisePropertyChanged();
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
                RaisePropertyChanged();
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
                RaisePropertyChanged();
            }
        }

        private bool isVisible;
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
                RaisePropertyChanged();
            }
        }



        public async void StartAsync()
        {
            try
            {
                await applicationMonitoring.BeginAsync(filePath, appName);
                ShowMonitored(false);
                IsVisible = false;
            }
            catch (Exception e)
            {
                interactionService.MessageBoxOK(e.Message);
            }
        }

        public void ShowMonitored(bool show)
        {
            applicationMonitoring.ShowMonitored(show);
        }
    }
}
