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


namespace blocage
{
    /* without DB
    class Program
    {
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



        // -------------------------- Listes THAT SHOULD BE UPDATED --------------------------------------

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

        // liste des utilisateur dans l'entreprise
        static List<Users> users = new List<Users>
    {
        new Users ("admin" , "Assia2001"),
        new Users ("hibat" ,"Hibat2001"),
        new Users ("Moh" , "Moh2002"),
        new Users("user","Hello123!")
    };

        // liste de liaison user peripherique 
        static List<UserDevices> userDev = new List<UserDevices>
    {
        new UserDevices ("USB\\VID_1D57&PID_130F","admin" ),
        new UserDevices ("USB\\VID_058F&PID_6387","user" ),

        new UserDevices ("USB\\VID_0BDA&PID_8179","hibat" ),
        new UserDevices ("USB\\VID_0BDA&PID_8179","Moh" ),

        new UserDevices ("USB\\VID_0000&PID_3825","user" )
    };
        // liste devices autorisé 

        static List<Device> usbdevices = new List<Device>{
        new Device("USB\\VID_1D57&PID_130F"  , "souris" ,"microsoft" ,"{745A17A0-74D3-11D0-B6FE-00A0C90F57DA}" , "usb.inf"),
        new Device( "USB\\VID_0BDA&PID_8179" , "RTL8188EU Wireless LAN 802.11n USB 2.0" ,"microsoft" ,"{4d36e972-e325-11ce-bfc1-08002be10318}", "netrtwlanu.inf"),
        new Device( "USB\\VID_058F&PID_6387" , "qsd" ,"microsoft" ,"{4D36E967-E325-11CE-BFC1-08002BE10318}", "usbstor.inf"),
         new Device( "HID\\VID_0000&PID_3825" , "souris" ,"microsoft" ,"{745A17A0-74D3-11D0-B6FE-00A0C90F57DA}", "usb.inf"),

       };


        // liste des classe autoriser 
        static List<ClasseD> usbclasses = new List<ClasseD>
            {
                new ClasseD("HIDUSB", "{745A17A0-74D3-11D0-B6FE-00A0C90F57DA}", "SYSTEM\\CurrentControlSet\\Services\\HidUsb" , false),
                new ClasseD("USBStore", "{4D36E967-E325-11CE-BFC1-08002BE10318}", "SYSTEM\\CurrentControlSet\\Services\\USBSTOR" , true),
                new ClasseD("USBprint", "{28D78FAD-5A12-11D1-AE5B-0000F803A8C2}", "SYSTEM\\CurrentControlSet\\Services\\USBprint" , true),
                new ClasseD("WINUSB", "{88BAE032-5A81-49F0-BC3D-A4FF138216D6}", "SYSTEM\\CurrentControlSet\\Services\\WINUSB" , true),
                //new ClasseD("RtlWlanu", "{4d36e972-e325-11ce-bfc1-08002be10318}", "SYSTEM\\CurrentControlSet\\Services\\RtlWlanu" , true ),
                // Ajoutez ici d'autres types de périphériques USB avec leurs GUID et chemins
            };
        
        //------------------------------------------------------------------------------------------------
        
        
        static void Main(string[] args)
        {
            string userName = Environment.UserName;
            Console.WriteLine("Nom de l'utilisateur : " + userName);

            string machineName = Environment.MachineName;
            Console.WriteLine("Nom de la machine : " + machineName);

            string sessionName = WindowsIdentity.GetCurrent().Name;
            Console.WriteLine("Nom de la session : " + sessionName);
            
            
            // ----- Listener starting ------
            
            string query = "SELECT * FROM __InstanceOperationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'";
            ManagementEventWatcher watcher = new ManagementEventWatcher();
            watcher.Query = new WqlEventQuery(query);

            // Ajouter un gestionnaire d'événements pour gérer les événements de branchement et de débranchement
            watcher.EventArrived += new EventArrivedEventHandler(USBEventArrived);

            watcher.Start();
            
            
            foreach (var item in usbclasses)
            {
                //DefaultStartValue(item.Chemin);
                if (item.isAutorised == true){
                    ActivateStartValue(item.Chemin);
                    AddToGpo(item.GUID);
                }

            }
            
            Console.WriteLine("Surveillance des événements USB. Appuyez sur Enter pour quitter.");
            Console.ReadKey();
            
            // Arrêter la surveillance lorsque l'utilisateur appuie sur Enter
            watcher.Stop();


        }


        // ------------------------------- Functions -----------------------------------------------------


        // ---------------- Printing Device Info----------------------------
        static void DisplayDeviceInfo(ManagementBaseObject device)

        {
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("ID du périphérique: " + device["DeviceID"]);
            Console.WriteLine("Fabricant: " + device["Manufacturer"]);
            Console.WriteLine("classe" + device["PNPClass"]);

            Console.WriteLine("--------------------------------------------");
        }

        static string ExtractDeviceClass(ManagementBaseObject device)
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

        static void ListInstalledDriversForDevice(string deviceId)
        {
            Guid classGuid = Guid.Empty;
            IntPtr deviceInfoSet = SetupDiGetClassDevs(ref classGuid, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT | DIGCF_ALLCLASSES);
            if (deviceInfoSet != IntPtr.Zero)
            {
                SP_DEVINFO_DATA deviceInfoData = new SP_DEVINFO_DATA();
                deviceInfoData.cbSize = (uint)Marshal.SizeOf(typeof(SP_DEVINFO_DATA));

                uint memberIndex = 0;
                while (SetupDiEnumDeviceInfo(deviceInfoSet, memberIndex, ref deviceInfoData))
                {
                    StringBuilder deviceInstanceId = new StringBuilder(256);
                    int requiredSize = 0;
                    if (SetupDiGetDeviceInstanceId(deviceInfoSet, ref deviceInfoData, deviceInstanceId, deviceInstanceId.Capacity, out requiredSize))
                    {
                        if (deviceInstanceId.ToString().Equals(deviceId, StringComparison.OrdinalIgnoreCase))
                        {
                            ListInstalledDrivers(deviceInfoSet, deviceInfoData);
                            break;
                        }
                    }

                    memberIndex++;
                }

                SetupDiDestroyDeviceInfoList(deviceInfoSet);
            }
            else
            {
                Console.WriteLine("Erreur lors de l'obtention de la liste des périphériques : " + Marshal.GetLastWin32Error());
            }
        }


        // --------------------------Listner----------------------

        static void USBEventArrived(object sender, EventArrivedEventArgs e)
        {
            // Vérifier le type d'événement
            if (e.NewEvent.ClassPath.ClassName == "__InstanceCreationEvent")
            {
                // Périphérique branché
                ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                //UninstallUSBDevice(instance["DeviceID"].ToString());
                //RefreshDeviceManager();


                // Console.WriteLine("eefs"+instance["DisplayName"].ToString());
                string vidpid = ExtractUSBVidPid(instance["DeviceID"].ToString());
                Console.WriteLine(instance);
                Console.WriteLine(vidpid);

                string classS = ExtractDeviceClass(instance);

                if (string.IsNullOrEmpty(classS))
                {
                    //DisplayDeviceInfo(instance);
                    if (IsDeviceAllowed(vidpid, usbdevices))
                    {

                        //Restart restart = new Restart();
                        //restart.restartDev(instance["DeviceID"].ToString());

                        Console.WriteLine("Peripherique autorisé " + instance["DeviceID"].ToString());
                    }

                    // Autoriser l'installation des pilotes ou prendre d'autres actions nécessaires
                    else
                    {
                        Console.WriteLine("Périphérique non autorisé détecté: " + instance["DeviceID"].ToString());
                        // Interdire l'installation des pilotes ou prendre d'autres mesures nécessaires
                    }
                }

            }
            else if (e.NewEvent.ClassPath.ClassName == "__InstanceDeletionEvent")
            {

            }
        }


        // Unplug device
        static void UninstallUSBDevice(string hardwareId)
        {
            // Identifier le périphérique à désinstaller
            Guid classGuid = Guid.Empty; // Utiliser tous les périphériques
            IntPtr deviceInfoSet = SetupDiGetClassDevs(ref classGuid, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT | DIGCF_ALLCLASSES);

            if (deviceInfoSet == IntPtr.Zero)
            {
                Console.WriteLine("Erreur lors de la récupération des informations sur les périphériques.");
                return;
            }

            try
            {
                SP_DEVINFO_DATA deviceInfoData = new SP_DEVINFO_DATA();
                deviceInfoData.cbSize = (uint)Marshal.SizeOf(deviceInfoData);

                for (uint i = 0; ; i++)
                {
                    if (!SetupDiEnumDeviceInfo(deviceInfoSet, i, ref deviceInfoData))
                    {
                        break;
                    }

                    uint propertyRegDataType;
                    uint requiredSize;
                    byte[] propertyBuffer = new byte[1024];

                    if (SetupDiGetDeviceRegistryProperty(deviceInfoSet, ref deviceInfoData, SPDRP_HARDWAREID, out propertyRegDataType, propertyBuffer, (uint)propertyBuffer.Length, out requiredSize))
                    {
                        string[] hardwareIds = Encoding.Unicode.GetString(propertyBuffer).Trim('\0').Split('\0');
                        foreach (string id in hardwareIds)
                        {
                            if (id.Equals(hardwareId, StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine($"Désinstallation du périphérique : {id}");

                                // Désinstaller le pilote
                                if (SetupDiCallClassInstaller(DIF_REMOVE, deviceInfoSet, ref deviceInfoData))
                                {
                                    Console.WriteLine("Périphérique désinstallé avec succès.");
                                }
                                else
                                {
                                    Console.WriteLine("Erreur lors de la désinstallation du périphérique.");
                                }

                                return;
                            }
                        }
                    }
                }

                Console.WriteLine("Périphérique non trouvé.");
            }
            finally
            {
                SetupDiDestroyDeviceInfoList(deviceInfoSet);
            }
        }


        // -----
        static void RefreshDeviceManager()
        {
            try
            {
                // Utiliser pnputil pour forcer un rescan
                Process.Start(new ProcessStartInfo
                {
                    FileName = "pnputil",
                    Arguments = "/enum-devices /rescan",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
                Console.WriteLine("Gestionnaire de périphériques rafraîchi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du rafraîchissement du Gestionnaire de périphériques : {ex.Message}");
            }
        }




        // ------------------------ Registry Modification ----------------
        //----------Blocking---------

        static void DefaultStartValue(string registryPath)
        {
            try
            {
                Console.WriteLine("m in Default start value ");
                // Ouvrir la clé de registre
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, true))
                {
                    // Vérifier si la clé existe
                    if (key != null)
                    {
                        // Set the "Start" value to 4 to disable the registry key
                        key.SetValue("Start", 4, RegistryValueKind.DWord);
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

        const uint INSTALLFLAG_FORCE = 0x00000001;

        static bool IsDeviceAllowed(string vidPid, List<Device> allowedDeviceIds)
        {
            string registryPath = "SOFTWARE\\Policies\\Microsoft\\Windows\\DeviceInstall\\Restrictions\\AllowDeviceIDs";
            string path2 = "SOFTWARE\\Wow6432Node\\Policies\\Microsoft\\Windows\\DeviceInstall\\Restrictions\\AllowDeviceIDs";

            // Récupérer le nom de l'utilisateur actuel
            string currentUserName = Environment.UserName;
            Console.WriteLine("1");

            foreach (var allowedId in allowedDeviceIds)
            {
                string allowed = ExtractUSBVidPid(allowedId.IdDevice);

                if (vidPid.Equals(allowed))
                {
                    Console.WriteLine("id allowd");
                    // Vérifier si le périphérique appartient à une classe autorisée
                    bool isClassAllowed = false;
                    foreach (var usbClass in usbclasses)
                    {
                        Console.WriteLine(usbClass.GUID);

                        if (usbClass.GUID == allowedId.GuidD && usbClass.isAutorised)
                        {
                            Console.WriteLine("usb classAllowd");
                            Console.WriteLine(usbClass.GUID);
                            Console.WriteLine(usbClass.isAutorised);
                            isClassAllowed = true;
                            break;
                        }
                    }

                    if (!isClassAllowed)
                    {
                        Console.WriteLine("Périphérique non autorisé : classe non autorisée");
                        return false;
                    }

                    // Vérifier si le périphérique appartient à l'utilisateur actuel
                    bool isUserAllowed = false;
                    foreach (var userDevice in userDev)
                    {
                        Console.WriteLine(userDevice.idDev);
                        Console.WriteLine(userDevice.email);
                        Console.WriteLine(currentUserName);
                        Console.WriteLine(allowedId.IdDevice);
                        if (userDevice.idDev == allowedId.IdDevice && userDevice.email == currentUserName)
                        {
                            Console.WriteLine("user allowed ");
                            isUserAllowed = true;
                            break;
                        }
                    }

                    if (!isUserAllowed)
                    {
                        Console.WriteLine("Périphérique non autorisé : utilisateur non autorisé");
                        return false;
                    }

                    Console.Write("Veuillez entrer votre mot de passe : ");
                    string inputPassword = Console.ReadLine();

                    // Vérifier le mot de passe
                    foreach (var user in users)
                    {
                        if (user.Name == currentUserName && user.Password == inputPassword)
                        {
                            // Mot de passe correct, continuer avec la mise à jour du registre et l'installation du pilote
                            string fullInfPath = "C:\\Windows\\inf\\" + allowedId.infs;
                            ModifyRegistry(registryPath, vidPid, allowedId.IdDevice);
                            ModifyRegistry(path2, vidPid, allowedId.IdDevice);

                            bool rebootRequired;
                            bool success = UpdateDriverForPlugAndPlayDevices(IntPtr.Zero, allowedId.IdDevice, fullInfPath, INSTALLFLAG_FORCE, out rebootRequired);

                            if (!success)
                            {
                                int errorCode = Marshal.GetLastWin32Error();
                                throw new Exception($"UpdateDriverForPlugAndPlayDevices failed with error code: {errorCode}");
                            }
                            else
                            {
                                Console.WriteLine("Driver update completed successfully.");
                            }

                            return true;
                        }
                    }
                }
            }

            // Mot de passe incorrect

            return false;
        }


        //modification of the registry

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


        static void AddToGpo(string guid)
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

        static void ActivateStartValue(string registryPath)
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

    }
*/
    /*
    class Program
    {
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



        // ----------------- List -----------------------------------------

        static List<ClasseDrives> usbclassDr = new List<ClasseDrives>();
        static List<Users> users = new List<Users>();
        static List<UserDevices> userDev = new List<UserDevices>();
        static List<Device> usbdevices = new List<Device>();
        static List<ClasseD> usbclasses = new List<ClasseD>();

        static DatabaseHelper dbHelper;
        static string connectionString = "Server=192.168.1.133;Database=entreprisebd;User Id=xlient;Password=M4B1U#;";

        static void Main(string[] args)
        {
            dbHelper = new DatabaseHelper(connectionString);

            LoadDataFromDatabase();

            // Existing logic...
            string userName = Environment.UserName;
            Console.WriteLine("Nom de l'utilisateur : " + userName);

            string machineName = Environment.MachineName;
            Console.WriteLine("Nom de la machine : " + machineName);

            string sessionName = WindowsIdentity.GetCurrent().Name;
            Console.WriteLine("Nom de la session : " + sessionName);


            // ----- Listener starting ------

            string query = "SELECT * FROM __InstanceOperationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'";
            ManagementEventWatcher watcher = new ManagementEventWatcher();
            watcher.Query = new WqlEventQuery(query);

            // Ajouter un gestionnaire d'événements pour gérer les événements de branchement et de débranchement
            watcher.EventArrived += new EventArrivedEventHandler(USBEventArrived);

            watcher.Start();


            foreach (var item in usbclasses)
            {
                //DefaultStartValue(item.Chemin);
                if (item.isAutorised == true)
                {
                    ActivateStartValue(item.Chemin);
                    AddToGpo(item.GUID);
                }

            }

            Console.WriteLine("Surveillance des événements USB. Appuyez sur Enter pour quitter.");
            Console.ReadKey();

            // Arrêter la surveillance lorsque l'utilisateur appuie sur Enter
            watcher.Stop();


        }


        // ------------------------------- Functions -----------------------------------------------------


        // ---------------- Printing Device Info----------------------------
        static void DisplayDeviceInfo(ManagementBaseObject device)

        {
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("ID du périphérique: " + device["DeviceID"]);
            Console.WriteLine("Fabricant: " + device["Manufacturer"]);
            Console.WriteLine("classe" + device["PNPClass"]);

            Console.WriteLine("--------------------------------------------");
        }

        static string ExtractDeviceClass(ManagementBaseObject device)
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

        static void ListInstalledDriversForDevice(string deviceId)
        {
            Guid classGuid = Guid.Empty;
            IntPtr deviceInfoSet = SetupDiGetClassDevs(ref classGuid, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT | DIGCF_ALLCLASSES);
            if (deviceInfoSet != IntPtr.Zero)
            {
                SP_DEVINFO_DATA deviceInfoData = new SP_DEVINFO_DATA();
                deviceInfoData.cbSize = (uint)Marshal.SizeOf(typeof(SP_DEVINFO_DATA));

                uint memberIndex = 0;
                while (SetupDiEnumDeviceInfo(deviceInfoSet, memberIndex, ref deviceInfoData))
                {
                    StringBuilder deviceInstanceId = new StringBuilder(256);
                    int requiredSize = 0;
                    if (SetupDiGetDeviceInstanceId(deviceInfoSet, ref deviceInfoData, deviceInstanceId, deviceInstanceId.Capacity, out requiredSize))
                    {
                        if (deviceInstanceId.ToString().Equals(deviceId, StringComparison.OrdinalIgnoreCase))
                        {
                            ListInstalledDrivers(deviceInfoSet, deviceInfoData);
                            break;
                        }
                    }

                    memberIndex++;
                }

                SetupDiDestroyDeviceInfoList(deviceInfoSet);
            }
            else
            {
                Console.WriteLine("Erreur lors de l'obtention de la liste des périphériques : " + Marshal.GetLastWin32Error());
            }
        }


        // --------------------------Listner----------------------

        static void USBEventArrived(object sender, EventArrivedEventArgs e)
        {
            // Vérifier le type d'événement
            if (e.NewEvent.ClassPath.ClassName == "__InstanceCreationEvent")
            {
                // Périphérique branché
                ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                //UninstallUSBDevice(instance["DeviceID"].ToString());
                //RefreshDeviceManager();


                // Console.WriteLine("eefs"+instance["DisplayName"].ToString());
                string vidpid = ExtractUSBVidPid(instance["DeviceID"].ToString());
                Console.WriteLine(instance);
                Console.WriteLine(vidpid);

                string classS = ExtractDeviceClass(instance);

                if (string.IsNullOrEmpty(classS))
                {
                    //DisplayDeviceInfo(instance);
                    if (IsDeviceAllowed(vidpid, usbdevices))
                    {

                        //Restart restart = new Restart();
                        //restart.restartDev(instance["DeviceID"].ToString());

                        Console.WriteLine("Peripherique autorisé " + instance["DeviceID"].ToString());
                    }

                    // Autoriser l'installation des pilotes ou prendre d'autres actions nécessaires
                    else
                    {
                        Console.WriteLine("Périphérique non autorisé détecté: " + instance["DeviceID"].ToString());
                        // Interdire l'installation des pilotes ou prendre d'autres mesures nécessaires
                    }
                }

            }
            else if (e.NewEvent.ClassPath.ClassName == "__InstanceDeletionEvent")
            {

            }
        }


        // Unplug device
        static void UninstallUSBDevice(string hardwareId)
        {
            // Identifier le périphérique à désinstaller
            Guid classGuid = Guid.Empty; // Utiliser tous les périphériques
            IntPtr deviceInfoSet = SetupDiGetClassDevs(ref classGuid, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT | DIGCF_ALLCLASSES);

            if (deviceInfoSet == IntPtr.Zero)
            {
                Console.WriteLine("Erreur lors de la récupération des informations sur les périphériques.");
                return;
            }

            try
            {
                SP_DEVINFO_DATA deviceInfoData = new SP_DEVINFO_DATA();
                deviceInfoData.cbSize = (uint)Marshal.SizeOf(deviceInfoData);

                for (uint i = 0; ; i++)
                {
                    if (!SetupDiEnumDeviceInfo(deviceInfoSet, i, ref deviceInfoData))
                    {
                        break;
                    }

                    uint propertyRegDataType;
                    uint requiredSize;
                    byte[] propertyBuffer = new byte[1024];

                    if (SetupDiGetDeviceRegistryProperty(deviceInfoSet, ref deviceInfoData, SPDRP_HARDWAREID, out propertyRegDataType, propertyBuffer, (uint)propertyBuffer.Length, out requiredSize))
                    {
                        string[] hardwareIds = Encoding.Unicode.GetString(propertyBuffer).Trim('\0').Split('\0');
                        foreach (string id in hardwareIds)
                        {
                            if (id.Equals(hardwareId, StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine($"Désinstallation du périphérique : {id}");

                                // Désinstaller le pilote
                                if (SetupDiCallClassInstaller(DIF_REMOVE, deviceInfoSet, ref deviceInfoData))
                                {
                                    Console.WriteLine("Périphérique désinstallé avec succès.");
                                }
                                else
                                {
                                    Console.WriteLine("Erreur lors de la désinstallation du périphérique.");
                                }

                                return;
                            }
                        }
                    }
                }

                Console.WriteLine("Périphérique non trouvé.");
            }
            finally
            {
                SetupDiDestroyDeviceInfoList(deviceInfoSet);
            }
        }


        // -----
        static void RefreshDeviceManager()
        {
            try
            {
                // Utiliser pnputil pour forcer un rescan
                Process.Start(new ProcessStartInfo
                {
                    FileName = "pnputil",
                    Arguments = "/enum-devices /rescan",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
                Console.WriteLine("Gestionnaire de périphériques rafraîchi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du rafraîchissement du Gestionnaire de périphériques : {ex.Message}");
            }
        }




        // ------------------------ Registry Modification ----------------
        //----------Blocking---------

        static void DefaultStartValue(string registryPath)
        {
            try
            {
                Console.WriteLine("m in Default start value ");
                // Ouvrir la clé de registre
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, true))
                {
                    // Vérifier si la clé existe
                    if (key != null)
                    {
                        // Set the "Start" value to 4 to disable the registry key
                        key.SetValue("Start", 4, RegistryValueKind.DWord);
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

        const uint INSTALLFLAG_FORCE = 0x00000001;

        static bool IsDeviceAllowed(string vidPid, List<Device> allowedDeviceIds)
        {
            string registryPath = "SOFTWARE\\Policies\\Microsoft\\Windows\\DeviceInstall\\Restrictions\\AllowDeviceIDs";
            string path2 = "SOFTWARE\\Wow6432Node\\Policies\\Microsoft\\Windows\\DeviceInstall\\Restrictions\\AllowDeviceIDs";

            // Récupérer le nom de l'utilisateur actuel
            string currentUserName = Environment.UserName;
            Console.WriteLine("1");

            foreach (var allowedId in allowedDeviceIds)
            {
                string allowed = ExtractUSBVidPid(allowedId.IdDevice);

                if (vidPid.Equals(allowed))
                {
                    Console.WriteLine("id allowd");
                    // Vérifier si le périphérique appartient à une classe autorisée
                    bool isClassAllowed = false;
                    foreach (var usbClass in usbclasses)
                    {
                        Console.WriteLine(usbClass.GUID);

                        if (usbClass.GUID == allowedId.GuidD && usbClass.isAutorised)
                        {
                            Console.WriteLine("usb classAllowd");
                            Console.WriteLine(usbClass.GUID);
                            Console.WriteLine(usbClass.isAutorised);
                            isClassAllowed = true;
                            break;
                        }
                    }

                    if (!isClassAllowed)
                    {
                        Console.WriteLine("Périphérique non autorisé : classe non autorisée");
                        return false;
                    }

                    // Vérifier si le périphérique appartient à l'utilisateur actuel
                    bool isUserAllowed = false;
                    foreach (var userDevice in userDev)
                    {
                        Console.WriteLine(userDevice.idDev);
                        Console.WriteLine(userDevice.email);
                        Console.WriteLine(currentUserName);
                        Console.WriteLine(allowedId.IdDevice);
                        if (userDevice.idDev == allowedId.IdDevice && userDevice.email == currentUserName)
                        {
                            Console.WriteLine("user allowed ");
                            isUserAllowed = true;
                            break;
                        }
                    }

                    if (!isUserAllowed)
                    {
                        Console.WriteLine("Périphérique non autorisé : utilisateur non autorisé");
                        return false;
                    }

                    Console.Write("Veuillez entrer votre mot de passe : ");
                    string inputPassword = Console.ReadLine();

                    // Vérifier le mot de passe
                    foreach (var user in users)
                    {
                        if (user.Name == currentUserName && user.Password == inputPassword)
                        {
                            // Mot de passe correct, continuer avec la mise à jour du registre et l'installation du pilote
                            string fullInfPath = "C:\\Windows\\inf\\" + allowedId.infs;
                            ModifyRegistry(registryPath, vidPid, allowedId.IdDevice);
                            ModifyRegistry(path2, vidPid, allowedId.IdDevice);

                            bool rebootRequired;
                            bool success = UpdateDriverForPlugAndPlayDevices(IntPtr.Zero, allowedId.IdDevice, fullInfPath, INSTALLFLAG_FORCE, out rebootRequired);

                            if (!success)
                            {
                                int errorCode = Marshal.GetLastWin32Error();
                                throw new Exception($"UpdateDriverForPlugAndPlayDevices failed with error code: {errorCode}");
                            }
                            else
                            {
                                Console.WriteLine("Driver update completed successfully.");
                            }

                            return true;
                        }
                    }
                }
            }

            // Mot de passe incorrect

            return false;
        }


        //modification of the registry

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


        static void AddToGpo(string guid)
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

        static void ActivateStartValue(string registryPath)
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




        // ---------------------------- DB intreactions------------------------
        static void LoadDataFromDatabase()
        {
            LoadUsbClasses();
            LoadUsers();
            LoadUserDevices();
            LoadUsbDevices();
            LoadUsbClassDetails();
        }

        static void LoadUsbClasses()
        {
            DataTable dt = dbHelper.ExecuteQuery("SELECT * FROM DriverClass");
            foreach (DataRow row in dt.Rows)
            {
                usbclassDr.Add(new ClasseDrives(row["guid"].ToString(), row["nameDrives"].ToString(), row["description"].ToString()));
            }
        }

        static void LoadUsers()
        {
            DataTable dt = dbHelper.ExecuteQuery("SELECT * FROM Users");
            foreach (DataRow row in dt.Rows)
            {
                users.Add(new Users(row["userName"].ToString(), row["password"].ToString()));
            }
        }

        static void LoadUserDevices()
        {
            DataTable dt = dbHelper.ExecuteQuery("SELECT * FROM Device_Users");
            foreach (DataRow row in dt.Rows)
            {
                userDev.Add(new UserDevices(row["device_id"].ToString(), row["user_id"].ToString()));
            }
        }

        static void LoadUsbDevices()
        {
            DataTable dt = dbHelper.ExecuteQuery("SELECT * FROM Device");
            foreach (DataRow row in dt.Rows)
            {
                usbdevices.Add(new Device(row["idDevice"].ToString(), row["nameD"].ToString(), row["inf"].ToString(), row["classI_id"].ToString(), "usb.inf"));
            }
        }

        static void LoadUsbClassDetails()
        {
            DataTable dt = dbHelper.ExecuteQuery("SELECT * FROM Class");
            foreach (DataRow row in dt.Rows)
            {
                usbclasses.Add(new ClasseD(row["type"].ToString(), row["guid"].ToString(), row["chemain"].ToString(), Convert.ToBoolean(row["description"])));
            }
        }

        static void LogDeviceEvent(string userId, string actionType, string fileName, string filePath, string deviceId, string machineId)
        {
            string query = "INSERT INTO Logs (user_id, action_type, file_name, file_path, device_id, machine_id, timestamp) " +
                           "VALUES (@user_id, @action_type, @file_name, @file_path, @device_id, @machine_id, @timestamp)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@user_id", userId);
                command.Parameters.AddWithValue("@action_type", actionType);
                command.Parameters.AddWithValue("@file_name", fileName);
                command.Parameters.AddWithValue("@file_path", filePath);
                command.Parameters.AddWithValue("@device_id", deviceId);
                command.Parameters.AddWithValue("@machine_id", machineId);
                command.Parameters.AddWithValue("@timestamp", DateTime.Now);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }


    }
*/
}



/*
using Microsoft.Win32;
using System.Collections.Generic;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Text;
using System;
using blocage;

namespace Newblocage
{
    class Program
    {
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

        static List<ClasseDrives> usbclassDr = new List<ClasseDrives>
        {
            // Add your ClasseDrives items here
        };

        static List<Users> users = new List<Users>
        {
            new Users ("admin" , "Assia2001"),
            new Users ("hibat" ,"Hibat2001"),
            new Users ("Moh" , "Moh2002"),
            new Users("user","Hello123!")
        };

        static List<UserDevices> userDev = new List<UserDevices>
        {
            new UserDevices ("USB\\VID_1D57&PID_130F","admin" ),
            new UserDevices ("USB\\VID_058F&PID_6387","user" ),
            new UserDevices ("USB\\VID_0BDA&PID_8179","hibat" ),
            new UserDevices ("USB\\VID_0BDA&PID_8179","Moh" ),
            new UserDevices ("HID\\VID_0000&PID_3825","user" )
        };

        // liste devices autorisé 

        static List<Device> usbdevices = new List<Device>{
        new Device("USB\\VID_1D57&PID_130F"  ,"{745A17A0-74D3-11D0-B6FE-00A0C90F57DA}" ),
        new Device( "USB\\VID_0BDA&PID_8179" , "{4d36e972-e325-11ce-bfc1-08002be10318}"),
        new Device( "USB\\VID_058F&PID_6387" , "{4D36E967-E325-11CE-BFC1-08002BE10318}"),
         new Device( "HID\\VID_0000&PID_3825"  ,"{745A17A0-74D3-11D0-B6FE-00A0C90F57DA}"),

       };
        static List<ClasseD> usbclasses = new List<ClasseD>
        {
            new ClasseD("HIDUSB", "{745A17A0-74D3-11D0-B6FE-00A0C90F57DA}", "SYSTEM\\CurrentControlSet\\Services\\HidUsb" , true),
            new ClasseD("USBStore", "{4D36E967-E325-11CE-BFC1-08002BE10318}", "SYSTEM\\CurrentControlSet\\Services\\USBSTOR" , true),
            new ClasseD("USBprint", "{28D78FAD-5A12-11D1-AE5B-0000F803A8C2}", "SYSTEM\\CurrentControlSet\\Services\\USBprint" , true),
            new ClasseD("WINUSB", "{88BAE032-5A81-49F0-BC3D-A4FF138216D6}", "SYSTEM\\CurrentControlSet\\Services\\WINUSB" , true),
            new ClasseD("RtlWlanu", "{4d36e972-e325-11ce-bfc1-08002be10318}", "SYSTEM\\CurrentControlSet\\Services\\RtlWlanu" , true ),
        };

        static void Main(string[] args)
        {
            string userName = Environment.UserName;
            Console.WriteLine("Nom de l'utilisateur : " + userName);

            string machineName = Environment.MachineName;
            Console.WriteLine("Nom de la machine : " + machineName);

            string sessionName = WindowsIdentity.GetCurrent().Name;
            Console.WriteLine("Nom de la session : " + sessionName);

            string query = "SELECT * FROM __InstanceOperationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'";
            ManagementEventWatcher watcher = new ManagementEventWatcher(new WqlEventQuery(query));
            watcher.EventArrived += new EventArrivedEventHandler(USBEventArrived);
            watcher.Start();

            foreach (var item in usbclasses)
            {
                if (item.isAutorised)
                    ActivateStartValue(item.Chemin);
                AddToGpo(item.GUID);
            }

            Console.WriteLine("Surveillance des événements USB. Appuyez sur Enter pour quitter.");
            Console.ReadKey();
            watcher.Stop();
        }

        static void DisplayDeviceInfo(ManagementBaseObject device)
        {
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("ID du périphérique: " + device["DeviceID"]);
            Console.WriteLine("Fabricant: " + device["Manufacturer"]);
            Console.WriteLine("classe: " + device["PNPClass"]);
            Console.WriteLine("--------------------------------------------");
        }

        static void ActivateStartValue(string registryPath)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, true))
                {
                    if (key != null)
                    {
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

        static bool IsDeviceAllowed(string vidPid, List<Device> allowedDeviceIds)
        {
            string registryPath = "SOFTWARE\\Policies\\Microsoft\\Windows\\DeviceInstall\\Restrictions\\AllowDeviceIDs";
            string path2 = "SOFTWARE\\Wow6432Node\\Policies\\Microsoft\\Windows\\DeviceInstall\\Restrictions\\AllowDeviceIDs";

            string currentUserName = Environment.UserName;

            foreach (var allowedId in allowedDeviceIds)
            {
                string allowed = ExtractUSBVidPid(allowedId.IdDevice);

                if (vidPid.Equals(allowed))
                {
                    bool isClassAllowed = false;
                    foreach (var usbClass in usbclasses)
                    {
                        if (usbClass.GUID == allowedId.GuidD && usbClass.isAutorised)
                        {
                            isClassAllowed = true;
                            break;
                        }
                    }

                    if (!isClassAllowed)
                    {
                        Console.WriteLine("Périphérique non autorisé : classe non autorisée");
                        return false;
                    }

                    bool isUserAllowed = false;
                    foreach (var userDevice in userDev)
                    {
                        if (userDevice.idDev == allowedId.IdDevice && userDevice.email == currentUserName)
                        {
                            isUserAllowed = true;
                            break;
                        }
                    }

                    if (!isUserAllowed)
                    {
                        Console.WriteLine("Périphérique non autorisé : utilisateur non autorisé");
                        return false;
                    }

                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, true) ??
                                             Registry.LocalMachine.OpenSubKey(path2, true))
                    {
                        if (key != null)
                        {
                            key.SetValue("1", vidPid, RegistryValueKind.String);
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Impossible de trouver le chemin de registre spécifié.");
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        static void USBEventArrived(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            DisplayDeviceInfo(instance);

            string deviceID = (string)instance["DeviceID"];
            string vidPid = ExtractUSBVidPid(deviceID);

            if (!IsDeviceAllowed(vidPid, usbdevices))
            {
                DisableDeviceByInstanceId(deviceID);
            }
        }

        static string ExtractUSBVidPid(string deviceID)
        {

            // Utiliser une expression régulière pour extraire les chaînes USB\VID_XXXX&PID_XXXX de chaque périphérique
            string pattern = @"VID_[\da-fA-F]+&PID_[\da-fA-F]+";
            Match match = Regex.Match(deviceID, pattern);

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


            string pattern = @"VID_[0-9A-F]{4}&PID_[0-9A-F]{4}";
            Match match = Regex.Match(deviceID, pattern);

            return match.Success ? match.Value : null
        }

        static void DisableDeviceByInstanceId(string deviceInstanceId)
        {
            Guid classGuid = Guid.Empty;
            IntPtr deviceInfoSet = SetupDiGetClassDevs(ref classGuid, IntPtr.Zero, IntPtr.Zero, DIGCF_ALLCLASSES | DIGCF_PRESENT);

            if (deviceInfoSet == IntPtr.Zero)
            {
                Console.WriteLine("Erreur lors de l'obtention de la liste des périphériques.");
                return;
            }

            SP_DEVINFO_DATA deviceInfoData = new SP_DEVINFO_DATA
            {
                cbSize = (uint)Marshal.SizeOf(typeof(SP_DEVINFO_DATA))
            };

            uint index = 0;
            while (SetupDiEnumDeviceInfo(deviceInfoSet, index, ref deviceInfoData))
            {
                index++;

                StringBuilder deviceInstanceIdBuffer = new StringBuilder(256);
                int requiredSize;
                if (SetupDiGetDeviceInstanceId(deviceInfoSet, ref deviceInfoData, deviceInstanceIdBuffer, deviceInstanceIdBuffer.Capacity, out requiredSize))
                {
                    if (deviceInstanceIdBuffer.ToString() == deviceInstanceId)
                    {
                        if (SetupDiCallClassInstaller(DIF_REMOVE, deviceInfoSet, ref deviceInfoData))
                        {
                            Console.WriteLine("Périphérique désactivé : " + deviceInstanceId);
                        }
                        else
                        {
                            Console.WriteLine("Erreur lors de la désactivation du périphérique : " + deviceInstanceId);
                        }
                        break;
                    }
                }
            }

            SetupDiDestroyDeviceInfoList(deviceInfoSet);
        }

        static void AddToGpo(string guid)
        {
            string registryPath = @"SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions\AllowDeviceClasses";
            string path2 = @"SOFTWARE\Wow6432Node\Policies\Microsoft\Windows\DeviceInstall\Restrictions\AllowDeviceClasses";
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, true) ??
                                         Registry.LocalMachine.OpenSubKey(path2, true))
                {
                    if (key != null)
                    {
                        key.SetValue("1", guid, RegistryValueKind.String);
                    }
                    else
                    {
                        Console.WriteLine("Impossible de trouver le chemin de registre spécifié.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de l'ajout à la GPO : {ex.Message}");
            }
        }
    }

    public class ClasseDrives
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public ClasseDrives(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }

    public class Users
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public Users(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

    public class UserDevices
    {
        public string idDev { get; set; }
        public string email { get; set; }

        public UserDevices(string idDev, string email)
        {
            this.idDev = idDev;
            this.email = email;
        }
    }

    public class Device
    {
        public string IdDevice { get; set; }
        public string GuidD { get; set; }

        public Device(string idDevice, string guidD)
        {
            IdDevice = idDevice;
            GuidD = guidD;
        }
    }

    public class ClasseD
    {
        public string Name { get; set; }
        public string GUID { get; set; }
        public string Chemin { get; set; }
        public bool isAutorised { get; set; }

        public ClasseD(string name, string guid, string chemin, bool isAutorised)
        {
            Name = name;
            GUID = guid;
            Chemin = chemin;
            this.isAutorised = isAutorised;
        }
    }
}
*/