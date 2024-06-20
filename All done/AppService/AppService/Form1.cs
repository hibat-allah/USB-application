using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Windows.Forms;

namespace AppService
{
    public partial class Form1 : Form
    {
        static string logFilePath = @"C:\Users\user\Documents\LogService.txt"; // Update with an appropriate path
        static string registryPath = @"SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions\AllowDeviceIDs";
        static string registryPath2 = @"SOFTWARE\Wow6432Node\Policies\Microsoft\Windows\DeviceInstall\Restrictions\AllowDeviceIDs";
        private static List<string> allowedDeviceIdsInstances = new List<string>();

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
                Log("service class");
                //UsbDeviceListener usbListener = new UsbDeviceListener();
                usbListener.UsbDeviceConnected += OnUDBDeviceConnected;
                usbListener.UsbDeviceRemoved += OnUDBDeviceRemoved;
                Log("after usb listner new");
                OnStart();
        }

        protected void OnStart()
        {
          Log("UsbMonitorService started.");

                string status = CheckStatusInDatabase();

                if (status == "complete")
                {
                    DisableUsbPorts();
                }
                else if (status == "selectif")
                {
                    //StartUsbDeviceListener();
                    Log("statring listining...");
                    usbListener.StartListening();


                }
            }

        protected void OnStop()
            {
                Log("UsbMonitorService stopped.");
        }

        private string CheckStatusInDatabase()
        {
            // Add your database logic here to check the status
            //return "selected"; // Example status
            Log(BD.CheckMachineTypeAndPerformAction());
            return BD.CheckMachineTypeAndPerformAction();
        }

        private void DisableUsbPorts()
        {
                
                // add the registries to block the classes
                // remove all values in the registry of AllowedIDs (gpo)
        }
        
        
        private void OnUDBDeviceConnected(string deviceInfo)
        {
            // Device info is in the format "deviceId:driveLetter"
            var parts = deviceInfo.Split(':');
            if (parts.Length < 2)
            {
                Log("Invalid device info format.");
                return;
            }

            string fullDeviceId = parts[0];
            string vidpid = Blocking.ExtractUSBVidPid(fullDeviceId);// vid&pid
            string deviceId = Blocking.ExtractDeviceId(fullDeviceId);//bd usb //
            string regid = Blocking.regId(fullDeviceId); // reg usb/
            string driveLetter = parts[1];
            Log(vidpid + " bla " + deviceId + " bla "+regid+"driverLetter "+driveLetter);
            if (vidpid == null || vidpid == "" || vidpid == " ")
            {
                Log("Device is already allowed, ignoring...");
                //ModifyRegistry(registryPath, regid, regid);
                //ModifyRegistry(registryPath2, regid, regid);
                return;
            }

            // verify the deviceID first 
            bool verify = Blocking.IsUSBAllowed(deviceId);
            if (verify)
            {
                // getting the INF file from the DB
                string inf = BD.GetDeviceInfFile(deviceId);
                if (inf != null)
                {
                    string fullInfPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "INF", inf);
                    Console.WriteLine($"INF Path: {fullInfPath}");

                    Blocking.ModifyRegistry(registryPath, regid, regid);
                    Blocking.ModifyRegistry(registryPath2, regid, regid);

                    var relatedDeviceIds = BD.GetRelatedDeviceIds(deviceId);
                    List<string> relatedDeviceIds2 = relatedDeviceIds;
                    Blocking.AllowAdditionalInstances(relatedDeviceIds2, registryPath, registryPath2);

                    Blocking.InstallDevice(regid, fullInfPath);
                    allowedDeviceIdsInstances.AddRange(relatedDeviceIds2);

                    // verify if he is a storage device to lunch the application

                    if (driveLetter != null)
                    {
                        Log($"USB Device Connected before app call: {driveLetter}");

                        Process applicationProcess = StartApplication(regid, driveLetter);
                        //Process.Start("E:\\Application\\NewApp\\bin\\Debug\\NewApp.exe", $"{deviceId} {driverletter}");
                        usbdeviceProcesses[driveLetter] = applicationProcess;
                    }

                }

            }
            else
            {
                // Blocking logic here
                try
                {
                    Blocking.DeleteRegistryValue(registryPath, regid);
                    Blocking.DeleteRegistryValue(registryPath2, regid);

                    // here blocking
                    Blocking.UninstallDevice(regid);

                    Console.WriteLine("Device uninstalled successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

           

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
        

       


        private void OnUDBDeviceRemoved(List<string> deviceInfoList)
        {

            foreach (string deviceInfo in deviceInfoList)
            {
                // Device info is in the format "deviceId:driveLetter"
                var parts = deviceInfo.Split(':');
                if (parts.Length < 2)
                {
                    Log("Invalid device info format.");
                    continue;
                }

                string fullDeviceId = parts[0];
                string driveLetter = parts[1];
                Log($"USB Device Removed {driveLetter}");

                if (driveLetter != null)
                {
                    StopApplication(apppath, driveLetter);
                }

                Log($"USB Device Removed: {driveLetter}");

                // uninstall the drivers
                string vidpid = Blocking.ExtractUSBVidPid(fullDeviceId);// vid&pid
                string deviceId = Blocking.ExtractDeviceId(fullDeviceId);//bd usb //
                string regid = Blocking.regId(fullDeviceId); // reg usb/
                if (vidpid != null || vidpid != "" || vidpid != " ")
                {
                    List<string> relatedDeviceIds = BD.GetRelatedDeviceIds(deviceId);

                    if (relatedDeviceIds != null)
                    {
                        foreach (string relatedDeviceId in relatedDeviceIds)
                        {
                            try
                            {
                                allowedDeviceIdsInstances.Remove(relatedDeviceId);
                                Blocking.DeleteRegistryValue(registryPath, regid);
                                Blocking.DeleteRegistryValue(registryPath2, regid);
                                // uninstalling the device
                                Blocking.UninstallDevice(regid);

                            }
                            catch (Exception ex)
                            {
                                Log($"Error uninstalling related device {relatedDeviceId}: {ex.Message}");
                            }
                        }


                    }
                }
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
                        Log($"Error closing application: {ex.Message}");
                    }

                }
            




        public static void Log(string message)
        {
            //lock (lockObj)
            //{
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(message);
            }
            //}
        }

    }



}
