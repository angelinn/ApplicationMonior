using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ApplicationMonitor.WPF.Services
{
    public class InteractionService
    {
        private System.Windows.Forms.NotifyIcon notifyIcon;

        public void CreateTrayIcon(Action doubleClick, KeyValuePair<string, Action>[] menuItems)
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Visible = true,
                ContextMenu = new System.Windows.Forms.ContextMenu(menuItems.Select(p =>
                    new System.Windows.Forms.MenuItem(p.Key, (s, e) => p.Value())).ToArray())
            };

            notifyIcon.DoubleClick += (s, e) => doubleClick();
        }

        public void SetTrayIcon(string fileName)
        {
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(fileName);
        }

        public void MessageBoxOK(string message, string title = "")
        {
            MessageBox.Show(message, title);
        }

        public string PromptForFileName()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
                return dialog.FileName;

            return null;
        }
    }
}
