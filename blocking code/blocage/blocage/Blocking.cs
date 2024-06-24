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
        private static List<string> allowedDeviceIdsInstances = new List<string>();

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
            /*
            Console.WriteLine("blocking the same device");

            block.blockUSB("USB\\VID_058F&PID_6387");
            Console.ReadKey();*/
            // Stop monitoring when the user presses Enter
            watcher.Stop();
        }
        // ------------------ Event Functions ----------------------------------
        public static void BlockingUSB(object sender, EventArrivedEventArgs e)
        {
            // Check the event type
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            string fullDeviceId = instance["DeviceID"].ToString();
            string vidpid = ExtractUSBVidPid(fullDeviceId);// vid&pid
            string deviceId = ExtractDeviceId(fullDeviceId);//bd usb //
            string regid = regId(fullDeviceId); // reg usb/
            Console.WriteLine($"\nDevice ID: {fullDeviceId}");
            Console.WriteLine($"Formatted Device ID: {deviceId}");
            Console.WriteLine($"VID_PID: {vidpid}");
            Console.WriteLine($"regid: {regid}\n");
            string registryPath = @"SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions\AllowDeviceIDs";
            string registryPath2 = @"SOFTWARE\Wow6432Node\Policies\Microsoft\Windows\DeviceInstall\Restrictions\AllowDeviceIDs";
            if (e.NewEvent.ClassPath.ClassName == "__InstanceCreationEvent")
            {
                // Retrieve the device ID from the listener

                //if (allowedDeviceIdsInstances.Contains(deviceId) || allowedDeviceIdsInstances.Contains(regid))
                if(vidpid== null || vidpid=="" || vidpid == " ")
                {
                    Console.WriteLine("Device is already allowed, ignoring...");
                    //ModifyRegistry(registryPath, regid, regid);
                    //ModifyRegistry(registryPath2, regid, regid);
                    return;
                }


                bool allowed = IsUSBAllowed(deviceId);
                Console.WriteLine($"Is USB Allowed: {allowed}");

                if (allowed)
                {
                    // Install the drivers
                    string inf = BD.GetDeviceInfFile(deviceId);
                    if (inf != null)
                    {
                        //string fullInfPath = "C:\\Windows\\inf\\" + inf;
                        //Console.WriteLine($"INF Path: {fullInfPath}");
                        string fullInfPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "INF", inf);
                        Console.WriteLine($"INF Path: {fullInfPath}");

                        ModifyRegistry(registryPath, regid, regid);
                        ModifyRegistry(registryPath2, regid, regid);
                        //CheckAndAddAdditionalValues(registryPath);
                        //CheckAndAddAdditionalValues(registryPath2);
                        //InstallDriver(deviceId, fullInfPath);

                        var relatedDeviceIds = BD.GetRelatedDeviceIds(deviceId);
                        List<string> relatedDeviceIds2 = relatedDeviceIds;
                        AllowAdditionalInstances(relatedDeviceIds2, registryPath, registryPath2);

                        // Install drivers for the main device and its related instances
                        InstallDriver(deviceId, fullInfPath);
                        //AllowRelatedInstances(relatedDeviceIds2, fullInfPath);
                        // i want to add the device id to the list
                        allowedDeviceIdsInstances.AddRange(relatedDeviceIds2);

                        // lunch the application
                        // function call (another .cs file which the lunching process is there)

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
            else if (e.NewEvent.ClassPath.ClassName == "__InstanceDeletionEvent")
            {
                Console.WriteLine($"trying to remove: {fullDeviceId}");
                block.blockUSB(regid);
                Console.WriteLine($"Uninstalled related device: {deviceId}");
                if (vidpid != null || vidpid !="" || vidpid != " ")
                {
                    List<string> relatedDeviceIds = BD.GetRelatedDeviceIds(deviceId);

                    if (relatedDeviceIds != null)
                    {
                        //allowedDeviceIdsInstances.Remove(deviceId);
                        //allowedDeviceIdsInstances.Remove(regid);

                        // Uninstall or block related instances
                        foreach (string relatedDeviceId in relatedDeviceIds)
                        {
                            try
                            {
                                allowedDeviceIdsInstances.Remove(relatedDeviceId);
                                block.blockUSB(relatedDeviceId);
                                Console.WriteLine($"Uninstalled related device: {relatedDeviceId}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error uninstalling related device {relatedDeviceId}: {ex.Message}");
                            }
                        }
                    }
                }
            }
        }



        // ------------------ Get IDs of the device in 3 forms -----------------
        // form : USB\\ VID&PID
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
        // form : USB\VID&PID
        public static string regId(string fullDeviceId)
        {
            // Split the device ID using the backslash as a delimiter
            var parts = fullDeviceId.Split('\\');
            return parts[0] + "\\" + parts[1];
        }
        //form ; VID&PID
        public static string ExtractUSBVidPid(string deviceId)
        {
            string pattern = @"VID_[\da-fA-F]+&PID_[\da-fA-F]+";
            Match match = Regex.Match(deviceId, pattern);
            Console.WriteLine("In extract functino this is the vid value:" + match.Value);
            return match.Success ? match.Value : null;
        }


        
        //------------------- Registry Modifications ---------------------------
        static void AllowAdditionalInstances(List<string> relatedDeviceIds, string registryPath, string registryPath2)
        {
           
            foreach (var relatedDeviceId in relatedDeviceIds)
            {
                string regId= relatedDeviceId;
                ModifyRegistry(registryPath, relatedDeviceId, regId);
                ModifyRegistry(registryPath2, relatedDeviceId, regId);
            }
        }

        static void AllowRelatedInstances(List<string> relatedDeviceIds, string fullInfPath)
        {;
            foreach (var relatedDeviceId in relatedDeviceIds)
            {
                InstallDriver(relatedDeviceId, fullInfPath);
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

        /*
        public static void ModifyRegistry(string registryPath, string valueName, string valueData)
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
        */
        public static void ModifyRegistry(string registryPath, string valueName, string valueData)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, true))
                {
                    if (key != null)
                    {
                        // Check if any existing value has the same data
                        foreach (var existingValueName in key.GetValueNames())
                        {
                            if (key.GetValue(existingValueName)?.ToString() == valueData)
                            {
                                Console.WriteLine($"Registry value with data '{valueData}' already exists.");
                                return; // Exit function if duplicate data found
                            }
                        }

                        // Check if the value already exists with a different data
                        if (key.GetValue(valueName)?.ToString() != null && key.GetValue(valueName)?.ToString() != valueData)
                        {
                            Console.WriteLine($"Registry value '{valueName}' already exists with different data.");
                            return; // Exit function if different data found
                        }

                        // If no duplicate data or different value with same name, set the value
                        key.SetValue(valueName, valueData, RegistryValueKind.String);
                        Console.WriteLine($"Registry value '{valueName}' set with data '{valueData}'.");
                    }
                    else
                    {
                        // Create the registry key and set the value
                        using (RegistryKey newKey = Registry.LocalMachine.CreateSubKey(registryPath))
                        {
                            if (newKey != null)
                            {
                                newKey.SetValue(valueName, valueData, RegistryValueKind.String);
                                Console.WriteLine($"Registry key '{registryPath}' created and value '{valueName}' set with data '{valueData}'.");
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

        //------------------- Activating the Registries ------------------------
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

        //------------------- Install drivers ----------------------------------
        static void InstallDriver(string deviceId, string fullInfPath)
        {
            //const uint INSTALLFLAG_FORCE = 0x00000001;
            try
            {
                bool rebootRequired;
                bool success = UpdateDriverForPlugAndPlayDevices(IntPtr.Zero, deviceId, fullInfPath, 0, out rebootRequired);

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


        //------------------- Verify Device if he is allowed -------------------
        public static bool IsUSBAllowed(string deviceId)
        {
            if (BD.CheckDeviceType(deviceId))
            {
                if (BD.CheckDeviceForUser(deviceId) && BD.CheckDeviceType(deviceId))
                {
                    Console.Write("Veuillez entrer votre mot de passe : ");
                    string password = "Hello123!"; // For testing purposes; replace with Console.ReadLine();

                    return BD.CheckPassword(password);
                }
                return false;

            }
            else { 
                
                
                return true; 
            }
        }

        // P/Invoke declarations
        [DllImport("newdev.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool UpdateDriverForPlugAndPlayDevices(IntPtr hwndParent, string HardwareId, string FullInfPath, uint InstallFlags, out bool bRebootRequired);
    }







/*
namespace AppService
    {
        public class UnifiedUsbDeviceListener
        {
            public event Action<string> UsbDeviceConnected;
            public event Action<List<string>> UsbDeviceRemoved;

            private ManagementEventWatcher _insertWatcher;
            private ManagementEventWatcher _removeWatcher;
            private ManagementEventWatcher _deviceWatcher;
            private List<string> _previousDriveLetters;
            private static List<string> allowedDeviceIdsInstances = new List<string>();

            public UnifiedUsbDeviceListener()
            {
                _insertWatcher = new ManagementEventWatcher();
                _removeWatcher = new ManagementEventWatcher();
                _deviceWatcher = new ManagementEventWatcher();

                WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");
                WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 3");
                WqlEventQuery deviceQuery = new WqlEventQuery("SELECT * FROM __InstanceOperationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'");

                _insertWatcher.EventArrived += new EventArrivedEventHandler(DeviceInsertedEvent);
                _removeWatcher.EventArrived += new EventArrivedEventHandler(DeviceRemovedEvent);
                _deviceWatcher.EventArrived += new EventArrivedEventHandler(BlockingUSB);

                _insertWatcher.Query = insertQuery;
                _removeWatcher.Query = removeQuery;
                _deviceWatcher.Query = deviceQuery;

                _previousDriveLetters = GetUsbDriveLetters();
            }

            public void StartListening()
            {
                _insertWatcher.Start();
                _removeWatcher.Start();
                _deviceWatcher.Start();
            }

            public void StopListening()
            {
                _insertWatcher.Stop();
                _removeWatcher.Stop();
                _deviceWatcher.Stop();
            }

            private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
            {
                var newDriveLetter = GetNewUsbDriveLetter();
                if (!string.IsNullOrEmpty(newDriveLetter))
                {
                    UsbDeviceConnected?.Invoke(newDriveLetter);
                }
            }

            private void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
            {
                var removedDriveLetters = GetRemovedUsbDriveLetters();
                foreach (var drive in removedDriveLetters)
                {
                    _previousDriveLetters.Remove(drive);
                }
                UsbDeviceRemoved?.Invoke(removedDriveLetters);
            }

            private string GetNewUsbDriveLetter()
            {
                var currentDriveLetters = GetDriveLetters();
                foreach (var drive in currentDriveLetters)
                {
                    if (!_previousDriveLetters.Contains(drive))
                    {
                        _previousDriveLetters.Add(drive);
                        return drive;
                    }
                }
                return null;
            }

            public List<string> GetRemovedUsbDriveLetters()
            {
                var currentDriveLetters = GetDriveLetters();
                return _previousDriveLetters.Except(currentDriveLetters).ToList();
            }

            private List<string> GetUsbDriveLetters()
            {
                List<string> driveLetters = new List<string>();
                foreach (var drive in Environment.GetLogicalDrives())
                {
                    DriveInfo driveInfo = new DriveInfo(drive);
                    if (driveInfo.DriveType == DriveType.Removable)
                    {
                        driveLetters.Add(drive);
                    }
                }
                return driveLetters;
            }

            private List<string> GetDriveLetters()
            {
                List<string> driveLetters = new List<string>();
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT DeviceID, MediaType FROM Win32_DiskDrive");
                ManagementObjectCollection drives = searcher.Get();

                foreach (ManagementObject drive in drives)
                {
                    string mediaType = drive["MediaType"]?.ToString() ?? string.Empty;

                    if (mediaType.Contains("Removable Media") || mediaType.Contains("External hard disk media"))
                    {
                        foreach (ManagementObject partition in drive.GetRelated("Win32_DiskPartition"))
                        {
                            foreach (ManagementObject logicalDisk in partition.GetRelated("Win32_LogicalDisk"))
                            {
                                driveLetters.Add(logicalDisk["DeviceID"].ToString());
                            }
                        }
                    }
                }
                return driveLetters;
            }

            public static void BlockingUSB(object sender, EventArrivedEventArgs e)
            {
                ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                string fullDeviceId = instance["DeviceID"].ToString();
                string vidpid = ExtractUSBVidPid(fullDeviceId);
                string deviceId = ExtractDeviceId(fullDeviceId);
                string regid = regId(fullDeviceId);

                Console.WriteLine($"Device ID: {fullDeviceId}");
                Console.WriteLine($"Formatted Device ID: {deviceId}");
                Console.WriteLine($"VID_PID: {vidpid}");
                Console.WriteLine($"regid: {regid}");

                string registryPath = @"SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions\AllowDeviceIDs";
                string registryPath2 = @"SOFTWARE\Wow6432Node\Policies\Microsoft\Windows\DeviceInstall\Restrictions\AllowDeviceIDs";

                if (e.NewEvent.ClassPath.ClassName == "__InstanceCreationEvent")
                {
                    if (allowedDeviceIdsInstances.Contains(deviceId) || allowedDeviceIdsInstances.Contains(regid))
                    {
                        Console.WriteLine("Device is already allowed, ignoring...");
                        ModifyRegistry(registryPath, vidpid, regid);
                        ModifyRegistry(registryPath2, vidpid, regid);
                        return;
                    }

                    bool allowed = IsUSBAllowed(deviceId);
                    Console.WriteLine($"Is USB Allowed: {allowed}");

                    if (allowed)
                    {
                        string inf = BD.GetDeviceInfFile(deviceId);
                        if (inf != null)
                        {
                            string fullInfPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "INF", inf);
                            Console.WriteLine($"INF Path: {fullInfPath}");

                            ModifyRegistry(registryPath, vidpid, regid);
                            ModifyRegistry(registryPath2, vidpid, regid);

                            var relatedDeviceIds = BD.GetRelatedDeviceIds(deviceId);
                            AllowAdditionalInstances(relatedDeviceIds, registryPath, registryPath2);
                            InstallDriver(deviceId, fullInfPath);
                            AllowRelatedInstances(relatedDeviceIds, fullInfPath);
                            allowedDeviceIdsInstances.AddRange(relatedDeviceIds);
                        }
                    }
                    else
                    {
                        try
                        {
                            block.blockUSB(regid);
                            Console.WriteLine("Device uninstalled successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
                else if (e.NewEvent.ClassPath.ClassName == "__InstanceDeletionEvent")
                {
                    Console.WriteLine($"Device removed: {fullDeviceId}");

                    if (allowedDeviceIdsInstances.Contains(deviceId))
                    {
                        allowedDeviceIdsInstances.Remove(deviceId);
                        allowedDeviceIdsInstances.Remove(regid);
                        List<string> relatedDeviceIds = BD.GetRelatedDeviceIds(deviceId);
                        foreach (string relatedDeviceId in relatedDeviceIds)
                        {
                            allowedDeviceIdsInstances.Remove(relatedDeviceId);
                        }

                        foreach (string relatedDeviceId in relatedDeviceIds)
                        {
                            try
                            {
                                block.blockUSB(relatedDeviceId);
                                Console.WriteLine($"Uninstalled related device: {relatedDeviceId}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error uninstalling related device {relatedDeviceId}: {ex.Message}");
                            }
                        }
                    }
                }
            }

            public static string ExtractDeviceId(string fullDeviceId)
            {
                var parts = fullDeviceId.Split('\\');
                return parts.Length >= 2 ? parts[0] + "\\\\" + parts[1] : fullDeviceId;
            }

            public static string regId(string fullDeviceId)
            {
                var parts = fullDeviceId.Split('\\');
                return parts[0] + "\\" + parts[1];
            }

            public static string ExtractUSBVidPid(string deviceId)
            {
                string pattern = @"VID_[\da-fA-F]+&PID_[\da-fA-F]+";
                Match match = Regex.Match(deviceId, pattern);
                return match.Success ? match.Value : "";
            }

            static void AllowAdditionalInstances(List<string> relatedDeviceIds, string registryPath, string registryPath2)
            {
                foreach (var relatedDeviceId in relatedDeviceIds)
                {
                    string regId = relatedDeviceId;
                    ModifyRegistry(registryPath, relatedDeviceId, regId);
                    ModifyRegistry(registryPath2, relatedDeviceId, regId);
                }
            }

            static void AllowRelatedInstances(List<string> relatedDeviceIds, string fullInfPath)
            {
                foreach (var relatedDeviceId in relatedDeviceIds)
                {
                    InstallDriver(relatedDeviceId, fullInfPath);
                }
            }

            public static void ModifyRegistry(string registryPath, string valueName, string valueData)
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
                            Console.WriteLine();    }
                    }
                }} } }
*/



}


