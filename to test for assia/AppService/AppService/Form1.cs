using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Management.Instrumentation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppService
{
    public partial class Form1 : Form
    {
        static string logFilePath = @"C:\Users\user\Documents\LogService.txt"; // Update with an appropriate path
        static string registryPath = @"SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions\AllowDeviceIDs";
        static string registryPath2 = @"SOFTWARE\Wow6432Node\Policies\Microsoft\Windows\DeviceInstall\Restrictions\AllowDeviceIDs";
        private static List<string> allowedDeviceIdsInstances = new List<string>();
        private static string allowedID=null;
        public Form1()
        {
            InitializeComponent();
            Service();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private string apppath= @"C:\Users\user\Desktop\New folder\Application\NewApp\bin\x64\Debug\NewApp.exe";
        private UsbDeviceListener usbListener = new UsbDeviceListener();
        private Dictionary<string, Process> usbdeviceProcesses = new Dictionary<string, Process>();
        private ManagementEventWatcher _insertWatcher;
        private ManagementEventWatcher _removeWatcher;

        public void Service()
        {
            /*
            _insertWatcher = new ManagementEventWatcher();
            _removeWatcher = new ManagementEventWatcher();

            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'");
            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'");

            _insertWatcher.EventArrived += new EventArrivedEventHandler(OnUDBDeviceConnected);
            _removeWatcher.EventArrived += new EventArrivedEventHandler(OnUDBDeviceRemoved);

            _insertWatcher.Query = insertQuery;
            _removeWatcher.Query = removeQuery;*/
            
            Log("service class");
                //UsbDeviceListener usbListener = new UsbDeviceListener();
                //
                usbListener.UsbDeviceConnected += OnUDBDeviceConnected;
                usbListener.UsbDeviceRemoved += OnUDBDeviceRemoved;
           Log("after usb listner new");


                OnStart();
        }

        protected void OnStart()
        {
          Log("UsbMonitorService started.");

            string status = "selectif";//CheckStatusInDatabase();

                if (status == "complete")
                {
                    DisableUsbPorts();
                }
                else if (status == "selectif")
                {
                //StartUsbDeviceListener();
                //ActivateRegisters :
                    BD.ProcessAuthorizedUsbClasses();
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
            BD.BlockingClasses();

            // remove all values in the registry of AllowedIDs (gpo)
            Blocking.DeleteAllRegistryValues(registryPath);
            Blocking.DeleteAllRegistryValues(registryPath2);
        }
        public static string ExtractDeviceClass(ManagementBaseObject device)
        {
            // Vérifier si la propriété PNPClass est nulle ou vide
            if (device["PNPClass"] == null || string.IsNullOrEmpty(device["PNPClass"].ToString()))
            {
                return "";
            }
            else
            {
                return device["PNPClass"].ToString();
            }
        }

        private void OnUDBDeviceConnected(string deviceInfo)
        {
            // Device info is in the format "deviceId:driveLetter"
            //ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            var parts = deviceInfo.Split(':');
            if (parts.Length < 2)
            {
                Log("Invalid device info format.");
                return;
            }

            Log("device connected");
            string fullDeviceId = parts[0];//instance["DeviceID"].ToString();
            string vidpid = Blocking.ExtractUSBVidPid(fullDeviceId);// vid&pid
            string deviceId = Blocking.ExtractDeviceId(fullDeviceId);//for bd usb //
            string regid = Blocking.regId(fullDeviceId); // reg usb/
                                                         //string driveLetter = "F:\\";//parts[1];

            Log(vidpid + " bla " + deviceId + " bla " + regid);// +"driverLetter "+driveLetter);
            string classS = parts[1];
            Log("class is : " + classS);
            if (string.IsNullOrEmpty(classS)|| string.IsNullOrWhiteSpace(classS)|| classS=="null")
            {
                Log("yes i'm in the condition");
                // verify the deviceID first 
                bool verify = true;//Blocking.IsUSBAllowed(deviceId);
                Log("verify" + verify);
                if (verify)
                {
                    // getting the INF file from the DB
                    string inf = "usbstor.inf";//BD.GetDeviceInfFile(deviceId);
                    if (inf != null)
                    {
                        string fullInfPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "INF", inf);
                        Console.WriteLine($"INF Path: {fullInfPath}");

                        Blocking.ModifyRegistry(registryPath, regid, regid);
                        Blocking.ModifyRegistry(registryPath2, regid, regid);

                        /*var relatedDeviceIds = BD.GetRelatedDeviceIds(deviceId);
                        List<string> relatedDeviceIds2 = relatedDeviceIds;
                        Blocking.AllowAdditionalInstances(relatedDeviceIds2, registryPath, registryPath2);
                        */// you should add manually lel registre hadok thingiyat
                        Blocking.InstallDevice(regid, fullInfPath);
                       
                        //should decomnt allowedDeviceIdsInstances.AddRange(relatedDeviceIds2);
                        // Blocking.InstallDevice(regid, fullInfPath);

                        // verify if he is a storage device to lunch the application
                        allowedID = regid;

                        // Store the mapping of USB device ID to storage device ID
                        new UsbDeviceListener().AddDeviceMapping(fullDeviceId, regid);

                        Task.Delay(1000).Wait(); // Wait for 1 second
                       

                    }

                }
            }
           
            else
            {
                if ( classS== "DiskDrive")
                {//"STORAGE\\VOLUME"
                  
                        Log("before the call of the driver letter funcion");
                        var driveLetter = new UsbDeviceListener().GetDriveLetterByHardwareID(fullDeviceId);
                        Log(" driveLetter " + driveLetter);

                    //lunch the docker 

                     //bool Infected = await Docker.HandleUsbDevice(driverletter);

                    // start the application, jut to test i'll comment it
                        /*if (!string.IsNullOrEmpty(driveLetter))
                        {
                        //allowedID= new UsbDeviceListener().GetUsbDeviceId(driveLetter);
                        Log(" THE IDFOR THE DRIVER " + driveLetter + " IS " + allowedID);
                        Process applicationProcess = StartApplication(allowedID, driveLetter);
                            //Process.Start("E:\\Application\\NewApp\\bin\\Debug\\NewApp.exe", $"{deviceId} {driverletter}");
                            usbdeviceProcesses[driveLetter] = applicationProcess;
                        }*/
                    
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
                Verb = "runas", // This will start the process with elevated permissions
                UseShellExecute = true // This is required for the "runas" verb to work
            };

            try
            {
                Process applicationProcess = Process.Start(processStartInfo);
                return applicationProcess;
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                // Handle the case where the user cancels the UAC prompt
                Console.WriteLine($"The application failed to start as admin: {ex.Message}");
                return null;
            }
        }





        private void OnUDBDeviceRemoved(List<string> deviceInfoList)
        {

            //ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
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



                //string fullDeviceId = instance["DeviceID"].ToString(); ;
                //string driveLetter;// = parts[1];
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
