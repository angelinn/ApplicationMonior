using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringService
{
    public class ApplicationMonitoring
    {
        private Process process;

        public void Begin(string path, int refresh = 30000)
        {
            Launch(path);

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(refresh);

                    if (process.HasExited)
                        Launch(path);
                }
            });
        }

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        public void ShowMonitored(bool show)
        {
            if (show)
                ShowWindow(process.MainWindowHandle, SW_SHOW);
            else
                ShowWindow(process.MainWindowHandle, SW_HIDE);
        }

        private void Launch(string path)
        {
            process = new Process();
            process.StartInfo.WorkingDirectory = Path.GetDirectoryName(path);
            process.StartInfo.FileName = path;
            process.Start();
        }
    }
}
