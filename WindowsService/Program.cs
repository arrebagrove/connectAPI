using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using System.IO;
using System.Threading;

using System.IO;
using System.Threading;
using System.Configuration;
using System.Runtime.InteropServices;  

namespace WindowsService
{
    class WindowsService : ServiceBase
    {

        public WindowsService()
        {
            this.ServiceName = "My Windows Service";
            this.EventLog.Log = "Application";
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            WriteToFile("Service works OnTimer " + DateTime.UtcNow.ToString());
            // TODO: Insert monitoring activities here.  
        }

        static void Main(string[] args) { ServiceBase[] ServicesToRun; ServicesToRun = new ServiceBase[] { new WindowsService() }; ServiceBase.Run(ServicesToRun); }


        protected override void OnStart(string[] args)
        {

            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 5000; // 5 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            WriteToFile("Service works OnStart " + DateTime.UtcNow.ToString());

        }

        protected override void OnStop()
        {
            WriteToFile("Service works OnStop " + DateTime.UtcNow.ToString());
        }

        protected override void OnContinue()
        {
            WriteToFile("Service works OnContinue " + DateTime.UtcNow.ToString());
        }

        private void WriteToFile(string text)
        {
            string path = "C:\\ServiceLog.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                writer.Close();
            }
        }

    }
}
