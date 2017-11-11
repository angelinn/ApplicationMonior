using NLog;
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
        public event EventHandler OnApplicationStarted;
        public event EventHandler OnApplicationExited;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private Process process;
        private string appName;

        public void Begin(string path, string appName = "", int refresh = 30000)
        {
            this.appName = appName;
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
            try
            {
                if (show)
                    ShowWindow(process.MainWindowHandle, SW_SHOW);
                else
                    ShowWindow(process.MainWindowHandle, SW_HIDE);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw ex;
            }
        }

        private void Launch(string path)
        {
            process = new Process();
            process.StartInfo.WorkingDirectory = Path.GetDirectoryName(path);
            process.StartInfo.FileName = path;
            process.EnableRaisingEvents = true;
            process.Exited += OnProcessExited;
            process.Start();

            if (!String.IsNullOrEmpty(appName))
                process = Process.GetProcessesByName(appName).First();

            logger.Info($"{process.ProcessName} started at {DateTime.Now}");

            OnApplicationStarted?.Invoke(this, new EventArgs());
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            logger.Info($"{process.ProcessName} exited at {DateTime.Now}");

            OnApplicationExited?.Invoke(sender, e);
            process.Exited -= OnProcessExited;
        }
    }
}
