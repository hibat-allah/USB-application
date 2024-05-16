using System;
using System.Management;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WhiteList;
using System.Security.Claims;
using RestartDevice;
using System.Diagnostics;
using System.Runtime.InteropServices;
class Program




{
     [DllImport("newdev.dll", CharSet = CharSet.Ansi, SetLastError = true)]
    public static extern bool UpdateDriverForPlugAndPlayDevices(IntPtr hwndParent, string HardwareId, string FullInfPath, uint InstallFlags, out bool bRebootRequired);


    

    static List<ClasseDrives> usbclassDr = new List<ClasseDrives>
            {

                new ClasseDrives( "{88BAE032-5A81-49F0-BC3D-A4FF138216D6}","USB\\Class_ff","1"),
                new ClasseDrives( "{88BAE032-5A81-49F0-BC3D-A4FF138216D6}","USB\\Class_02","2"),
                new ClasseDrives("{745A17A0-74D3-11D0-B6FE-00A0C90F57DA}", "HID_DEVICE","3"),
                new ClasseDrives("{745A17A0-74D3-11D0-B6FE-00A0C90F57DA}", "USB\\Class_03","4"),
                //new ClasseDrives("{4d36e972-e325-11ce-bfc1-08002be10318}", "USB\\Class_FF","5")
               
            };
    static void Main(string[] args)
    {
        foreach (var item in usbclassDr)
{
    Console.WriteLine($"GUID: {item.guid}, DrivesClass: {item.drivesClass}");
}

       // liste devices autorisé 
       
        
        List<Device> usbdevices = new List<Device>{
        new Device("USB\\VID_1D57&PID_130F"  , "souris" ,"microsoft" ,"{745A17A0-74D3-11D0-B6FE-00A0C90F57DA}" , "usb.inf"),
        new Device("USB\\VID_0BDA&PID_8179"  , "RTL8188EU Wireless LAN 802.11n USB 2.0" ,"microsoft" ,"{4d36e972-e325-11ce-bfc1-08002be10318} ", "netrtwlanu.inf"),
        };
        // liste des classe autoriser 
         List<ClasseD> usbclasses = new List<ClasseD>
            {
                new ClasseD("HIDUSB", "{745A17A0-74D3-11D0-B6FE-00A0C90F57DA}", "SYSTEM\\CurrentControlSet\\Services\\HidUsb"),
                //new ClasseD("USBStore", "{4D36E967-E325-11CE-BFC1-08002BE10318}", "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\USBSTOR"),
                //new ClasseD("USBprint", "{28D78FAD-5A12-11D1-AE5B-0000F803A8C2}", "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\USBprint"),
                //new ClasseD("WINUSB", "{88BAE032-5A81-49F0-BC3D-A4FF138216D6}", "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\WINUSB"),
                new ClasseD("RtlWlanu", "{4d36e972-e325-11ce-bfc1-08002be10318}", "SYSTEM\\CurrentControlSet\\Services\\RtlWlanu"),
                // Ajoutez ici d'autres types de périphériques USB avec leurs GUID et chemins
            };
            Instance instance1 = new Instance ("USB\\VID_0BDA&PID_8179\\00E04C0001");
            Instance instance2 = new Instance ("USB\\VID_1D57&PID_130F\\5&15C64844&0&3");
            Instance instance3 = new Instance ("USB\\VID_1D57&PID_130F&MI_00\\6&23EF43A7&12&0000");
            Instance instance4 = new Instance ("USB\\VID_1D57&PID_130F&MI_01\\6&23EF43A7&12&0001");
            Instance instance5 = new Instance ("HID\\VID_1D57&PID_130F&MI_00&COL01\\7&1304E89B&0&0000");

           

             
            

           

        // Écoute des événements de branchement de périphériques
        ManagementEventWatcher watcher = new ManagementEventWatcher();
        watcher.EventArrived += (sender, e) =>
        {
            string eventType = e.NewEvent.ClassPath.ClassName;
            
            if (eventType == "__InstanceCreationEvent")

            {
                // Périphérique branché
                ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
               // Console.WriteLine("eefs"+instance["DisplayName"].ToString());
                string vidpid = ExtractUSBVidPid(instance["DeviceID"].ToString());
                Console.WriteLine(instance);
                Console.WriteLine(vidpid);
                Console.WriteLine("kk");
                string classS = ExtractDeviceClass(instance);
                Console.WriteLine("kk5");
                

                

                if (string.IsNullOrEmpty(classS)) {
                Console.WriteLine("kk4");
                
                 //DisplayDeviceInfo(instance);
            if (IsDeviceAllowed(vidpid, usbdevices))
            {
               
                //Restart restart = new Restart();
                //restart.restartDev(instance["DeviceID"].ToString());
                Console.WriteLine("kk2");
                Console.WriteLine("Peripherique autorisé "+ instance["DeviceID"].ToString());
                }    
                             
                // Autoriser l'installation des pilotes ou prendre d'autres actions nécessaires
            else
            {
                
                Console.WriteLine("Périphérique non autorisé détecté: " + instance["DeviceID"].ToString());
                // Interdire l'installation des pilotes ou prendre d'autres mesures nécessaires
            }
              } 
                
            }
            else if (eventType == "__InstanceDeletionEvent")
            {
                // Périphérique débranché
                ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                DisplayDeviceInfo(instance);

            }
        };
       
        //WqlEventQuery query = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity' ");
        //WqlEventQuery query = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity' AND TargetInstance.ConfigManagerErrorCode <> 0");
        // WqlEventQuery query = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity' AND TargetInstance.DeviceID LIKE '%VEN_Unknown%'");
        //WqlEventQuery query = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity' AND TargetInstance.PNPClass = 'Unknown'");
        //WqlEventQuery query = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity' AND TargetInstance.Service = 'null'");
       WqlEventQuery query = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'  ");


        watcher.Query = query;
        watcher.Start();

         foreach (var item in usbclasses)
            {
                ActivateStartValue(item.Chemin);
                AddToGpo(item.GUID );
                
            }


        Console.WriteLine("Écoute des périphériques branchés en cours. Appuyez sur une touche pour arrêter...");
        Console.ReadKey();

        watcher.Stop();
    }

    // Affiche les informations sur le périphérique
    static void DisplayDeviceInfo(ManagementBaseObject device)

    {
        Console.WriteLine("--------------------------------------------");
        Console.WriteLine("ID du périphérique: " + device["DeviceID"]);
        Console.WriteLine("Fabricant: " + device["Manufacturer"]);
        Console.WriteLine("classe"+device["PNPClass"]);
        
        Console.WriteLine("--------------------------------------------");
    }

    static void ActivateStartValue(string registryPath)
        {
            try
            {
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
    
    
    static bool IsDeviceAllowed(string vidPid, List<Device> allowedDeviceIds)
    {
        string registryPath = "SOFTWARE\\Policies\\Microsoft\\Windows\\DeviceInstall\\Restrictions\\AllowDeviceIDs";
        string path2=  "SOFTWARE\\Wow6432Node\\Policies\\Microsoft\\Windows\\DeviceInstall\\Restrictions\\AllowDeviceIDs";
                Console.WriteLine("je suis la ");

        foreach (var allowedId in allowedDeviceIds)
        {
                Console.WriteLine("hzl ");
               
                

                string allowed = ExtractUSBVidPid(allowedId.IdDevice);
                
            if (vidPid.Equals(allowed))
            {
                string fullInfPath = "C:\\Windows\\inf\\"+allowedId.infs;
                Console.WriteLine("je suis lass ");

                ModifyRegistry(registryPath, vidPid  ,allowedId.IdDevice );
                ModifyRegistry(path2, vidPid  ,allowedId.IdDevice );
                 
               
              bool rebootRequired;
            bool success = UpdateDriverForPlugAndPlayDevices(IntPtr.Zero, allowedId.IdDevice, fullInfPath, 0, out rebootRequired);

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
        return false;
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

    static void AddToGpo (string guid ){
        string registryPath = "SOFTWARE\\Policies\\Microsoft\\Windows\\DeviceInstall\\Restrictions\\AllowDeviceIDs";
        string path2=  "SOFTWARE\\Wow6432Node\\Policies\\Microsoft\\Windows\\DeviceInstall\\Restrictions\\AllowDeviceIDs";

        Console.WriteLine("e"+ guid );
        
        foreach (var item in usbclassDr)
            {
                Console.WriteLine("cc"+item.drivesClass);

                     // Vérifier si le GUID du périphérique existe dans usbclasses
                if (guid.Equals(item.guid, StringComparison.OrdinalIgnoreCase))
                    {
                Console.WriteLine("ss"+ item.drivesClass );
                        
                        ModifyRegistry(registryPath,item.drivesClass  ,item.drivesClass );
                        ModifyRegistry(path2,item.drivesClass  ,item.drivesClass );
                        
                    }
            }
    }


    static void ModifyRegistry(string registryPath, string valueName, string valueData)
{
    try
    {
        if (valueName != null && valueData != null)
        {
            // Ouvrir la clé de registre en mode lecture-écriture
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, true))
            {
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

   
   



}
