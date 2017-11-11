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
        public event EventHandler<Process> OnApplicationStarted;
        public event EventHandler<Process> OnApplicationExited;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private Process process;
        private string appName;

        public async Task BeginAsync(string path, string appName = "", int refresh = 30000)
        {
            this.appName = appName;
            await LaunchAsync(path);

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(refresh);

                    if (process.HasExited)
                        await LaunchAsync(path);
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

        private async Task LaunchAsync(string path)
        {
            if (!String.IsNullOrEmpty(appName))
            {
                Process batProcess = new Process();
                batProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(path);
                batProcess.StartInfo.FileName = path;
                batProcess.Start();

                Process[] processes = Process.GetProcessesByName(appName);
                while (processes.Length == 0)
                {
                    await Task.Delay(1000);
                    processes = Process.GetProcessesByName(appName);
                }

                process = processes.First();
                process.Exited += OnProcessExited;
                process.EnableRaisingEvents = true;
            }
            else
            {
                process = new Process();
                process.StartInfo.WorkingDirectory = Path.GetDirectoryName(path);
                process.StartInfo.FileName = path;
                process.EnableRaisingEvents = true;
                process.Exited += OnProcessExited;
                process.Start();
            }

            logger.Info($"{process.ProcessName} started at {DateTime.Now}");

            OnApplicationStarted?.Invoke(this, process);
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            logger.Info($"{process.ProcessName} exited at {DateTime.Now}");

            OnApplicationExited?.Invoke(sender, process);
            process.Exited -= OnProcessExited;
        }
    }
}
