using ApplicationMonitor.WPF.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        public MainViewModel MainViewModel { get; private set; } = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = MainViewModel;
        }

        private void OnStartClicked(object sender, RoutedEventArgs e)
        {
            MainViewModel.Start();
        }

        private void OnBrowseClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                MainViewModel.FilePath = dialog.FileName;
            }
        }
    }
}
