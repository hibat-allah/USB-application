using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using NewApp;

using System.Configuration.Install;

namespace Service
{

    public partial class Service1 : ServiceBase
    {
        private UsbDeviceListener usbListener = new UsbDeviceListener();
        private Dictionary<string, Process> usbdeviceProcesses = new Dictionary<string, Process>();
        
        public Service1()
        {
            InitializeComponent();
            usbListener.UsbDeviceConnected += OnUDBDeviceConnected;
            usbListener.UsbDeviceRemoved += OnUDBDeviceRemoved;


        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("UsbMonitorService started.");

            string status = CheckStatusInDatabase();

            if (status == "complete")
            {
                DisableUsbPorts();
            }
            else if (status == "selected")
            {
                //StartUsbDeviceListener();
                usbListener.UsbDeviceConnected += OnUDBDeviceConnected;
                usbListener.UsbDeviceRemoved += OnUDBDeviceRemoved;
                usbListener.StartListening();

            }
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("UsbMonitorService stopped.");
        }

        private string CheckStatusInDatabase()
        {
            // Add your database logic here to check the status
            return "selected"; // Example status
        }

        private void DisableUsbPorts()
        {
            // Add your logic to disable USB ports here
        }

        private string GetUsbDeviceId()
        {
            // Add your logic to get the USB device ID here
            return "USB\\VID_058F&PID_6387";
        }
        private void OnUDBDeviceConnected(string driverletter)
        {
            EventLog.WriteEntry($"USB Device Connected {driverletter} i'm in the connect handler");
            string deviceId = GetUsbDeviceId();

            Process applicationProcess = StartApplication(deviceId, driverletter);
            //Process.Start("E:\\Application\\NewApp\\bin\\Debug\\NewApp.exe", $"{deviceId} {driverletter}");
            usbdeviceProcesses[driverletter] = applicationProcess;

        }
        private Process StartApplication(string deviceId, string driverletter)
        {
            /*
            try
            {
                string appPath = @"E:\Application\NewApp\bin\Debug\NewApp.exe";
                string arguments = $"{deviceId} {driverletter}";
                EventLog.WriteEntry($"the entry of start app are: {deviceId}, {driverletter}");
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = appPath,
                    Arguments = arguments,
                };

                Process applicationProcess = Process.Start(processStartInfo);
                EventLog.WriteEntry($"app info after the process start: {processStartInfo}");
                if (applicationProcess == null)
                {
                    throw new Exception("Failed to start the application process.");
                }

                return applicationProcess;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry($"Failed to start the application: {ex.Message}", EventLogEntryType.Error);
                return null;
            }
            */

            try
            {
                string appPath = @"E:\Application\NewApp\bin\Debug\NewApp.exe";
                string arguments = $"{deviceId} {driverletter}";

                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = appPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                Process applicationProcess = new Process
                {
                    StartInfo = processStartInfo
                };

                applicationProcess.OutputDataReceived += (sender, e) => EventLog.WriteEntry($"Output: {e.Data}");
                applicationProcess.ErrorDataReceived += (sender, e) => EventLog.WriteEntry($"Error: {e.Data}", EventLogEntryType.Error);

                if (applicationProcess.Start())
                {
                    applicationProcess.BeginOutputReadLine();
                    applicationProcess.BeginErrorReadLine();
                    return applicationProcess;
                }
                else
                {
                    throw new Exception("Failed to start the application process.");
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry($"Failed to start the application: {ex.Message}", EventLogEntryType.Error);
                return null;
            }

        }

        private void OnUDBDeviceRemoved(List<string> driverletters)
        {
            EventLog.WriteEntry($"USB Device Removed {string.Join(",", driverletters)}");

//            Console.WriteLine($"USB Device Removed {driverletters}");
            foreach (string driverletter in driverletters)
            {

                StopApplication("E:\\Application\\NewApp\\bin\\Debug\\NewApp.exe", driverletter);
            }

        }
        private void StopApplication(string apppath, string drive)
        {


            if (usbdeviceProcesses.ContainsKey(drive))
            {
                CloseApplication(usbdeviceProcesses[drive]);

                usbdeviceProcesses.Remove(drive);
            }
        }
        private void CloseApplication(Process appP)
        {
            appP.Kill();
        }
        /*
        public static void InstallService()
        {
            try
            {
                using (AssemblyInstaller installer = new AssemblyInstaller(typeof(Service1).Assembly, null))
                {
                    installer.UseNewContext = true;
                    installer.Install(null);
                    installer.Commit(null);
                }

                using (ServiceController sc = new ServiceController("Service Hiba"))
                {
                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        sc.Start();
                    }
                }

                Console.WriteLine("Service installed and started successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static void UninstallService()
        {
            try
            {
                using (AssemblyInstaller installer = new AssemblyInstaller(typeof(Service1).Assembly, null))
                {
                    installer.UseNewContext = true;
                    installer.Uninstall(null);
                }

                Console.WriteLine("Service uninstalled successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        */




    }
}
