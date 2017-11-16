using ApplicationMonitor.WPF.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ApplicationMonitor.WPF.Converters
{
    public class WindowStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ApplicationMonitorWindowState state = (ApplicationMonitorWindowState)value;
            switch (state)
            {
                case ApplicationMonitorWindowState.Minimized:
                    return WindowState.Minimized;
                case ApplicationMonitorWindowState.Maximized:
                    return WindowState.Maximized;
                case ApplicationMonitorWindowState.Normal:
                    return WindowState.Normal;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WindowState state = (WindowState)value;
            switch (state)
            {
                case WindowState.Minimized:
                    return ApplicationMonitorWindowState.Minimized;
                case WindowState.Maximized:
                    return ApplicationMonitorWindowState.Maximized;
                case WindowState.Normal:
                    return ApplicationMonitorWindowState.Normal;
                default:
                    return null;
            }
        }
    }
}
