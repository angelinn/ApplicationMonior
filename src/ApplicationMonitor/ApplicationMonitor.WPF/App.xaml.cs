using ApplicationMonitor.WPF.Services;
using GalaSoft.MvvmLight.Ioc;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ApplicationMonitor.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    public partial class App : Application
    {
        public App()
        {
            LogManager.GetCurrentClassLogger().Info($"Application started at {DateTime.Now}.");
            SimpleIoc.Default.Register<InteractionService>();
        }
    }
}
