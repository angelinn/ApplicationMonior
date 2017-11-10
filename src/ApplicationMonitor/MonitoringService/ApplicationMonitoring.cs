using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringService
{
    public class ApplicationMonitoring
    {
        private Process process;

        public void Begin(string path)
        {
            Launch(path);

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(30000);

                    if (process.HasExited)
                        Launch(path);
                }
            });
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
