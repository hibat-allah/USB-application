using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Management;
using System.Data.SqlClient;
using System.Data;

using System.IO;

namespace blocage
{
    /*
    public class Blocking
    {
        private static string connectionString = "Host=" + ServerConfiguration.IPDB() + ";Port=5432;Username=zflub;Password=M#N@d31N;Database=entreprisebd";


        static void Main(string[] args)
        {
            Console.WriteLine("main");

            string query = "SELECT * FROM __InstanceOperationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'";
            ManagementEventWatcher watcher = new ManagementEventWatcher();
            watcher.Query = new WqlEventQuery(query);
            Console.WriteLine("watcher");
            // Ajouter un gestionnaire d'événements pour gérer les événements de branchement et de débranchement
            watcher.EventArrived += new EventArrivedEventHandler(BlockingUSB);

            watcher.Start();
            BD.ProcessAuthorizedUsbClasses();
            Console.WriteLine("Surveillance des événements USB. Appuyez sur Enter pour quitter.");
            Console.ReadKey();

            // Arrêter la surveillance lorsque l'utilisateur appuie sur Enter
            watcher.Stop();
        }
        public static string ExtractDeviceId(string fullDeviceId)
        {
            // Split the device ID using the backslash as a delimiter
            var parts = fullDeviceId.Split('\\');

            // Join the first two parts with double backslashes
            if (parts.Length >= 2)
            {
                return parts[0] + "\\\\" + parts[1];
            }

            // If the device ID does not match the expected format, return the original string
            return fullDeviceId;
        }

        public static void BlockingUSB(object sender, EventArrivedEventArgs e)
        {
            // Vérifier le type d'événement
            if (e.NewEvent.ClassPath.ClassName == "__InstanceCreationEvent")
            {
                // recuperate the device id from the listener
                ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];

                string vidpid = ExtractUSBVidPid(instance["DeviceID"].ToString());
                Console.WriteLine($"{vidpid}");
                string deviceId =vidpid;// ----------------------
                Console.WriteLine($"{instance["DeviceID"].ToString()}");
                string id = ExtractDeviceId(instance["DeviceID"].ToString());
                Console.WriteLine("the id is " + id);
                bool allowed = IsUSBAllowed(id);
                Console.WriteLine($"{allowed}");
                string registryPath = "SOFTWARE\\Policies\\Microsoft\\Windows\\DeviceInstall\\Restrictions\\AllowDeviceIDs";
                string path2 = "SOFTWARE\\Wow6432Node\\Policies\\Microsoft\\Windows\\DeviceInstall\\Restrictions\\AllowDeviceIDs";
                const uint INSTALLFLAG_FORCE = 0x00000001;

                    if (allowed)
                    {
                        // install the drivers
                        string inf = BD.GetDeviceInfFile(id);
                        if (inf != null)
                        {
                            string fullInfPath = "C:\\Windows\\INF\\" + inf;
                            Console.WriteLine  ($"{fullInfPath}");
                            ModifyRegistry(registryPath, deviceId, id);
                            ModifyRegistry(path2, deviceId, id);

                            bool rebootRequired;
                            bool success = UpdateDriverForPlugAndPlayDevices(IntPtr.Zero, @"USB\VID_058F&PID_6387", fullInfPath, INSTALLFLAG_FORCE, out rebootRequired);

                            if (!success)
                            {
                                int errorCode = Marshal.GetLastWin32Error();
                                throw new Exception($"UpdateDriverForPlugAndPlayDevices failed with error code: {errorCode}");
                            }
                            else
                            {
                                Console.WriteLine("Driver update completed successfully.");
                            }
                        }
                    }
                    else
                    {
                        //blocking

                    }
            }
        }

        static void ModifyRegistry(string registryPath, string valueName, string valueData)
        {
            try
            {
                Console.WriteLine("m in modify registry");
                if (valueName != null && valueData != null)
                {
                    // Ouvrir la clé de registre en mode lecture-écriture
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, true))
                    {
                        Console.WriteLine($"the key is : {key}");
                        // Vérifier si la clé existe
                        if (key != null)
                        {
                            // Modifier la valeur de la clé de registre
                            key.SetValue(valueName, valueData, RegistryValueKind.String);
                        }
                        else
                        {
                            // La clé n'existe pas, donc la créer
                            Registry.LocalMachine.CreateSubKey(registryPath);

                            // Ouvrir à nouveau la clé nouvellement créée
                            using (RegistryKey newKey = Registry.LocalMachine.OpenSubKey(registryPath, true))
                            {
                                // Vérifier si la clé est créée avec succès
                                if (newKey != null)
                                {
                                    // Modifier la valeur de la clé de registre
                                    newKey.SetValue(valueName, valueData, RegistryValueKind.String);
                                }
                                else
                                {
                                    Console.WriteLine($"Impossible de créer la clé de registre '{registryPath}'.");
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Erreur : Les valeurs de clé de registre ne peuvent pas être nulles.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la modification de la clé de registre '{registryPath}': {ex.Message}");
            }

        }


        static string ExtractUSBVidPid(string deviceId)
        {


            // Utiliser une expression régulière pour extraire les chaînes USB\VID_XXXX&PID_XXXX de chaque périphérique
            string pattern = @"VID_[\da-fA-F]+&PID_[\da-fA-F]+";
            Match match = Regex.Match(deviceId, pattern);

            // Extraire la chaîne USB\VID_XXXX&PID_XXXX si une correspondance est trouvée
            if (match.Success)
            {
                return match.Value;
            }
            else
            {
                // Si aucune correspondance n'est trouvée, retourner une chaîne vide
                return "";
            }
        }

        // drivers

        static void ListInstalledDrivers(IntPtr deviceInfoSet, SP_DEVINFO_DATA deviceInfoData)
        {
            if (SetupDiBuildDriverInfoList(deviceInfoSet, ref deviceInfoData, SPDIT_COMPATDRIVER))
            {
                Console.WriteLine("Liste des pilotes compatibles pour le périphérique :");

                SP_DRVINFO_DATA driverInfoData = new SP_DRVINFO_DATA();
                driverInfoData.cbSize = (uint)Marshal.SizeOf(typeof(SP_DRVINFO_DATA));

                uint memberIndex = 0;
                while (SetupDiEnumDriverInfo(deviceInfoSet, ref deviceInfoData, SPDIT_COMPATDRIVER, memberIndex, ref driverInfoData))
                {
                    // Correctly decode and display driver information
                    Console.WriteLine($"Nom du pilote : {driverInfoData.Description}");
                    Console.WriteLine($"Fournisseur du pilote : {driverInfoData.MfgName}");
                    Console.WriteLine($"Version du pilote : {driverInfoData.DriverVersion}");
                    Console.WriteLine("--------------------------------------------");

                    memberIndex++;
                }

                SetupDiDestroyDriverInfoList(deviceInfoSet, ref deviceInfoData, SPDIT_COMPATDRIVER);
            }
            else
            {
                Console.WriteLine("Erreur lors de la construction de la liste des pilotes : " + Marshal.GetLastWin32Error());
            }
        }

        static List<ClasseDrives> usbclassDr = new List<ClasseDrives>
            {

               // new ClasseDrives( "{88BAE032-5A81-49F0-BC3D-A4FF138216D6}","USB\\Class_ff","1"),
                //new ClasseDrives( "{88BAE032-5A81-49F0-BC3D-A4FF138216D6}","USB\\Class_02","2"),
                new ClasseDrives("{745A17A0-74D3-11D0-B6FE-00A0C90F57DA}", "HID_DEVICE","3"),
                new ClasseDrives("{745A17A0-74D3-11D0-B6FE-00A0C90F57DA}", "USB\\Class_03","4"),
                //new ClasseDrives("{4d36e972-e325-11ce-bfc1-08002be10318}", "USB\\Class_FF","5"),
                new ClasseDrives("{4D36E967-E325-11CE-BFC1-08002BE10318}", "USBSTOR\\Disk","5"),

                new ClasseDrives("{4D36E967-E325-11CE-BFC1-08002BE10318}", "GenDisk","6")



            };

        public static void AddToGpo(string guid)
        {
            string registryPath = @"SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions\AllowDeviceClasses";
            string path2 = @"SOFTWARE\Wow6432Node\Policies\Microsoft\Windows\DeviceInstall\Restrictions\AllowDeviceClasses";

            Console.WriteLine("guid: " + guid);

            foreach (var item in usbclassDr)
            {


                // Vérifier si le GUID du périphérique existe dans usbclasses
                if (guid.Equals(item.guid, StringComparison.OrdinalIgnoreCase))
                {
                    ModifyRegistry(registryPath, item.drivesClass, item.drivesClass);
                    ModifyRegistry(path2, item.drivesClass, item.drivesClass);
                }
            }
        }

        public static void ActivateStartValue(string registryPath)
        {
            try
            {
                Console.WriteLine("m in activate start value ");
                // Ouvrir la clé de registre
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, true))
                {
                    // Vérifier si la clé existe
                    if (key != null)
                    {
                        // Modifier la valeur de Start à 3
                        key.SetValue("Start", 3, RegistryValueKind.DWord);
                    }
                    else
                    {
                        Console.WriteLine($"La clé de registre '{registryPath}' est introuvable.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la modification de la clé de registre '{registryPath}': {ex.Message}");
            }
        }



        //----------Deblocking--------- : Allow
        static void RemoveDeviceFromRegistry(string registryPath, string deviceId)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, true))
                {
                    if (key != null)
                    {
                        string[] valueNames = key.GetValueNames();
                        foreach (string valueName in valueNames)
                        {
                            string value = key.GetValue(valueName).ToString();
                            if (value.Contains(deviceId))
                            {
                                key.DeleteValue(valueName);
                                Console.WriteLine($"Removed {deviceId} from {registryPath}");
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error modifying registry: " + ex.Message);
            }
        }


        // checking if the usb peripheral is allowed
        public static bool IsUSBAllowed(string deviceId)
        {
            // verify ID
            bool validID= BD.CheckDeviceForUser(deviceId);
            if (validID)
            {
                // verify type
                bool validtype=BD.CheckDeviceType(deviceId);
                if (validtype){

                    //verify password
                    Console.Write("Veuillez entrer votre mot de passe : ");
                    string password = "Hello123!";//Console.ReadLine();

                    bool validpsw = BD.CheckPassword(password);
                        if (validpsw)
                            return true;
                }
            }

            return false;
        }






        // --------------------------------- importing classes----------------------------------
        [DllImport("newdev.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool UpdateDriverForPlugAndPlayDevices(IntPtr hwndParent, string HardwareId, string FullInfPath, uint InstallFlags, out bool bRebootRequired);
        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }

        public const int DIGCF_PRESENT = 0x00000002;
        public const int DIGCF_ALLCLASSES = 0x00000004;
        public const int DIF_REMOVE = 0x00000005;

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, uint Flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, uint MemberIndex, ref SP_DEVINFO_DATA DeviceInfoData);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetupDiGetDeviceInstanceId(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, StringBuilder DeviceInstanceId, int DeviceInstanceIdSize, out int RequiredSize);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiGetDeviceRegistryProperty(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, uint Property, out uint PropertyRegDataType, byte[] PropertyBuffer, uint PropertyBufferSize, out uint RequiredSize);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiCallClassInstaller(uint InstallFunction, IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        public const uint SPDRP_HARDWAREID = 0x00000001;
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiBuildDriverInfoList(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, uint DriverType);

        public const uint SPDIT_COMPATDRIVER = 0x00000002;

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiEnumDriverInfo(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, uint DriverType, uint MemberIndex, ref SP_DRVINFO_DATA DriverInfoData);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiDestroyDriverInfoList(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, uint DriverType);

        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DRVINFO_DATA
        {
            public uint cbSize;
            public uint DriverType;
            public uint Reserved;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Description;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string MfgName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string ProviderName;
            public ulong DriverDate;
            public ulong DriverVersion;
        }
    }
*/



    public class Blocking
    {
        //private static string connectionString = "Host=ServerIP;Port=5432;Username=zflub;Password=M#N@d31N;Database=entreprisebd";

        static void Main(string[] args)
        {
            Console.WriteLine("main");

            string query = "SELECT * FROM __InstanceOperationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'";
            ManagementEventWatcher watcher = new ManagementEventWatcher();
            watcher.Query = new WqlEventQuery(query);
            Console.WriteLine("watcher");

            // Add event handler for USB events
            watcher.EventArrived += new EventArrivedEventHandler(BlockingUSB);

            watcher.Start();
            BD.ProcessAuthorizedUsbClasses();
            Console.WriteLine("Surveillance des événements USB. Appuyez sur Enter pour quitter.");
            Console.ReadKey();

            Console.WriteLine("blocking the same device");

            block.blockUSB("USB\\VID_058F&PID_6387");
            Console.ReadKey();
            // Stop monitoring when the user presses Enter
            watcher.Stop();
        }

        public static string ExtractDeviceId(string fullDeviceId)
        {
            // Split the device ID using the backslash as a delimiter
            var parts = fullDeviceId.Split('\\');

            // Join the first two parts with double backslashes
            if (parts.Length >= 2)
            {
                return parts[0] + "\\\\" + parts[1];
            }

            // If the device ID does not match the expected format, return the original string
            return fullDeviceId;
        }

        public static string regId(string fullDeviceId)
        {
            // Split the device ID using the backslash as a delimiter
            var parts = fullDeviceId.Split('\\');
            return parts[0] + "\\" + parts[1];
        }
        public static void BlockingUSB(object sender, EventArrivedEventArgs e)
        {
            // Check the event type
            if (e.NewEvent.ClassPath.ClassName == "__InstanceCreationEvent")
            {
                // Retrieve the device ID from the listener
                ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                string fullDeviceId = instance["DeviceID"].ToString();
                string vidpid = ExtractUSBVidPid(fullDeviceId);
                string deviceId = ExtractDeviceId(fullDeviceId);
                string regid= regId(fullDeviceId);
                Console.WriteLine($"Device ID: {fullDeviceId}");
                Console.WriteLine($"Formatted Device ID: {deviceId}");
                Console.WriteLine($"VID_PID: {vidpid}");
                Console.WriteLine($"regid: {regid}");
                bool allowed = IsUSBAllowed(deviceId);
                Console.WriteLine($"Is USB Allowed: {allowed}");
                string registryPath= "SOFTWARE\\Policies\\Microsoft\\Windows\\DeviceInstall\\Restrictions\\AllowDeviceIDs";
                string registryPath2 = "SOFTWARE\\Wow6432Node\\Policies\\Microsoft\\Windows\\DeviceInstall\\Restrictions\\AllowDeviceIDs";

                if (allowed)
                {
                    // Install the drivers
                    string inf = BD.GetDeviceInfFile(deviceId);
                    if (inf != null)
                    {
                        string fullInfPath = "C:\\Windows\\inf\\" + inf;
                        Console.WriteLine($"INF Path: {fullInfPath}");
                        ModifyRegistry(registryPath, vidpid, regid);
                        ModifyRegistry(registryPath2, vidpid, regid);
                        CheckAndAddAdditionalValues(registryPath);
                        CheckAndAddAdditionalValues(registryPath2);
                        InstallDriver(deviceId, fullInfPath);

                        }
                }
                else
                {
                    // Blocking logic here
                    try
                    {
                        //DeleteRegistryValue(registryPath, vidpid);
                        //DeleteRegistryValue(registryPath2, vidpid);

                        block.blockUSB(regid);
                        Console.WriteLine("Device uninstalled successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }

        static void CheckAndAddAdditionalValues(string registryPath)
        {
            // Additional string values to check and add if necessary
            string[] additionalValues = { "STORAGE\\VOLUME", "USBSTOR\\DISK" };

            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, true))
                {
                    if (key != null)
                    {
                        foreach (string additionalValue in additionalValues)
                        {
                            if (key.GetValue(additionalValue) == null)
                            {
                                key.SetValue(additionalValue, additionalValue, RegistryValueKind.String);
                                Console.WriteLine($"Added additional value '{additionalValue}' to registry key '{registryPath}'.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Registry key '{registryPath}' not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking and adding additional values for registry '{registryPath}': {ex.Message}");
            }
        }


        static void ModifyRegistry(string registryPath, string valueName, string valueData)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, true))
                {
                    if (key != null)
                    {
                        key.SetValue(valueName, valueData, RegistryValueKind.String);
                    }
                    else
                    {
                        using (RegistryKey newKey = Registry.LocalMachine.CreateSubKey(registryPath))
                        {
                            if (newKey != null)
                            {
                                newKey.SetValue(valueName, valueData, RegistryValueKind.String);
                            }
                            else
                            {
                                Console.WriteLine($"Failed to create registry key '{registryPath}'.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error modifying registry '{registryPath}': {ex.Message}");
            }
        }

        public static void DeleteRegistryValue(string registryPath, string valueName)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, true))
                {
                    if (key != null)
                    {
                        // Check if the value exists before attempting to delete it
                        if (key.GetValue(valueName) != null)
                        {
                            // Delete the specified value
                            key.DeleteValue(valueName);
                            Console.WriteLine($"Deleted value '{valueName}' from registry path '{registryPath}'.");
                        }
                        else
                        {
                            Console.WriteLine($"Value '{valueName}' does not exist in registry path '{registryPath}'.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Registry key '{registryPath}' not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting registry value: {ex.Message}");
            }
        }

        static string ExtractUSBVidPid(string deviceId)
        {
            string pattern = @"VID_[\da-fA-F]+&PID_[\da-fA-F]+";
            Match match = Regex.Match(deviceId, pattern);

            return match.Success ? match.Value : "";
        }

        static void InstallDriver(string deviceId, string fullInfPath)
        {
            const uint INSTALLFLAG_FORCE = 0x00000001;
            try
            {
                bool rebootRequired;
                bool success = UpdateDriverForPlugAndPlayDevices(IntPtr.Zero, deviceId, fullInfPath, INSTALLFLAG_FORCE, out rebootRequired);

                if (!success)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Exception($"UpdateDriverForPlugAndPlayDevices failed with error code: {errorCode} ({errorCode:X})");
                }

                Console.WriteLine(rebootRequired ? "Reboot required after driver update." : "Driver updated successfully without requiring a reboot.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Driver installation error: {ex.Message}");
            }
        }

        public static bool IsUSBAllowed(string deviceId)
        {
            if (BD.CheckDeviceForUser(deviceId) && BD.CheckDeviceType(deviceId))
            {
                Console.Write("Veuillez entrer votre mot de passe : ");
                string password = "Hello123!"; // For testing purposes; replace with Console.ReadLine();

                return BD.CheckPassword(password);
            }
            return false;
        }

        // P/Invoke declarations
        [DllImport("newdev.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool UpdateDriverForPlugAndPlayDevices(IntPtr hwndParent, string HardwareId, string FullInfPath, uint InstallFlags, out bool bRebootRequired);
    }


}


