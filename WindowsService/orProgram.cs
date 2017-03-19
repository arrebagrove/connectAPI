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
            this.ServiceName = "xService";
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

            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus); 

            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 5000; // 5 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            WriteToFile("Service works OnStart " + DateTime.UtcNow.ToString());

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);  

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



        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public long dwServiceType;
            public ServiceState dwCurrentState;
            public long dwControlsAccepted;
            public long dwWin32ExitCode;
            public long dwServiceSpecificExitCode;
            public long dwCheckPoint;
            public long dwWaitHint;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);  


    }
}
