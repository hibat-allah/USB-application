using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Windows.Forms;

namespace AppService
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Service();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private string apppath= @"C:\Users\user\Desktop\without blockoing\Application\NewApp\bin\Debug\NewApp.exe";
            private UsbDeviceListener usbListener = new UsbDeviceListener();
            private Dictionary<string, Process> usbdeviceProcesses = new Dictionary<string, Process>();
            public void Service()
            {
                Console.WriteLine("service class");
                //UsbDeviceListener usbListener = new UsbDeviceListener();
                usbListener.UsbDeviceConnected += OnUDBDeviceConnected;
                usbListener.UsbDeviceRemoved += OnUDBDeviceRemoved;
                Console.WriteLine("after usb listner new");
                OnStart();
            }

            protected void OnStart()
            {
                Console.WriteLine("UsbMonitorService started.");

                string status = CheckStatusInDatabase();

                if (status == "complete")
                {
                    DisableUsbPorts();
                }
                else if (status == "selectif")
                {
                    //StartUsbDeviceListener();
                    Console.WriteLine("statring listining...");
                    usbListener.StartListening();


                }
            }

            protected void OnStop()
            {
                Console.WriteLine("UsbMonitorService stopped.");
            }

            private string CheckStatusInDatabase()
            {
            // Add your database logic here to check the status
            //return "selected"; // Example status
            Console.WriteLine(BD.CheckMachineTypeAndPerformAction());
            return BD.CheckMachineTypeAndPerformAction();
            }

            private void DisableUsbPorts()
            {
                // Add your logic to disable USB ports here
            }
        
            private void StartUsbDeviceListener()
            {
                ManagementEventWatcher watcher = new ManagementEventWatcher();
                WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");
                watcher.EventArrived += new EventArrivedEventHandler(DeviceInsertedEvent);
                watcher.Query = query;
                watcher.Start();
            }
            private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
            {
                // Get the USB device ID and other details if needed
                string deviceId = GetUsbDeviceId();
                
                Process.Start(apppath, deviceId);
            }
        
            private string GetUsbDeviceId()
            {
                // Add your logic to get the USB device ID here
                return "USB\\VID_058F&PID_6387";
            }
            private void OnUDBDeviceConnected(string driverletter)
            {

                Console.WriteLine($"USB Device Connected before call{driverletter}");
                string deviceId = GetUsbDeviceId();
                
                Process applicationProcess = StartApplication(deviceId, driverletter);
                //Process.Start("E:\\Application\\NewApp\\bin\\Debug\\NewApp.exe", $"{deviceId} {driverletter}");
                usbdeviceProcesses[driverletter] = applicationProcess;

                // Insert log
                LogHelper.InsertLog(Environment.UserName, "connexion", null, null, deviceId, "machineId",DateTime.Now);
            }
        
        private Process StartApplication(string deviceId, string driverletter)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = apppath,
                    Arguments = $"{deviceId} {driverletter}",
                };
                Process applicationProcess = Process.Start(processStartInfo);
                return applicationProcess;
            }
        

       


        private void OnUDBDeviceRemoved(List<string> driverletters)
            {

                Console.WriteLine($"USB Device Removed {driverletters}");
                foreach (string driverletter in driverletters)
                {

                    StopApplication(apppath, driverletter);
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
                try
                {
                    if (!appP.HasExited)
                    {
                        appP.Kill();
                        appP.WaitForExit(); // Optional: Wait for the process to exit to ensure it's terminated
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., access denied, process already exited)
                    Console.WriteLine($"Error closing application: {ex.Message}");
                }
        
           }
        

    }
}
