using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppService
{
    public class Blocking
    {
        static string logFilePath = @"C:\Users\user\Documents\blockingLogs.txt"; // Update with an appropriate path
        private static Dictionary<string, DeviceInfo> uninstallDataDictionary = new Dictionary<string, DeviceInfo>();
        // -------- importing DLLs ---------------------------------------------

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SP_DEVINFO_DATA
        {
            public int cbSize;
            public Guid ClassGuid;
            public int DevInst;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DeviceInfo
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string deviceId;
            public SP_DEVINFO_DATA deviceInfoData;
            public IntPtr deviceInfoSet;
        }

        [DllImport("DeviceDriverManager.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool InstallDriver(string deviceId, string infPath, out DeviceInfo uninstallData);

        [DllImport("DeviceDriverManager.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetDeviceInfo(string deviceId, out DeviceInfo deviceInfo);

        [DllImport("DeviceDriverManager.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool UninstallDriver(string deviceId, ref DeviceInfo uninstallData);



        // ------------------ Get IDs of the device in 3 forms -----------------
        // form : USB\\ VID&PID to use it with the DB
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
        // form : USB\VID&PID to use it as a value to registry
        public static string regId(string fullDeviceId)
        {
            // Split the device ID using the backslash as a delimiter
            var parts = fullDeviceId.Split('\\');
            return parts[0] + "\\" + parts[1];
        }
        //form ; VID&PID to name the registry
        public static string ExtractUSBVidPid(string deviceId)
        {
            string pattern = @"VID_[\da-fA-F]+&PID_[\da-fA-F]+";
            Match match = Regex.Match(deviceId, pattern);
            Console.WriteLine("In extract functino this is the vid value:" + match.Value);
            return match.Success ? match.Value : null;
        }



        //------------------- Registry Modifications ---------------------------
        public static void AllowAdditionalInstances(List<string> relatedDeviceIds, string registryPath, string registryPath2)
        {

            foreach (var relatedDeviceId in relatedDeviceIds)
            {
                string regId = relatedDeviceId;
                ModifyRegistry(registryPath, relatedDeviceId, regId);
                ModifyRegistry(registryPath2, relatedDeviceId, regId);
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
        public static void InstallDevice(string deviceId, string fullInfPath)
        {
            DeviceInfo uninstallData;
            if (InstallDriver(deviceId, fullInfPath, out uninstallData))
            {
                Log("Driver installed successfully.");
                Log("UninstallData:" + uninstallData);
                Log($"DeviceId: {uninstallData.deviceId}");
                uninstallDataDictionary[deviceId] = uninstallData;

            }
            else
            {
                Log("Driver installation failed.");
            }
        }



        //------------------- Uninstall drivers ----------------------------------
        public static void UninstallDevice(string deviceId)
        {
            /*if (uninstallDataDictionary.TryGetValue(deviceId,out DeviceInfo uninstallData))
            {
                if (UninstallDriver(deviceId, ref uninstallData))
                {
                    uninstallDataDictionary.Remove(deviceId);
                    Log("Driver uninstalled successfully.");
                }
                else
                {
                    Log("Driver uninstallation failed.");
                }
            }
            else
            {
                Log("No uninstall Data found");

            }*/

            if (!uninstallDataDictionary.ContainsKey(deviceId))
            {
                if (GetDeviceInfo(deviceId, out DeviceInfo deviceinfo))
                {
                    uninstallDataDictionary[deviceId] = deviceinfo;
                }
                else
                {
                    Log("there is no device info data to uinstall the device!");
                    return;
                }
            }
            DeviceInfo uninstallData = uninstallDataDictionary[deviceId];
            if (UninstallDriver(deviceId, ref uninstallData))
            {
                uninstallDataDictionary.Remove(deviceId);
                Log("Driver uninstalled successfully.");
            }
            else
            {
                Log("Driver uninstallation failed.");
            }


        }



        //TO DO:  link it with the password form
        //------------------- Verify Device if he is allowed -------------------
        public static bool IsUSBAllowed(string deviceId)
        {
            if (BD.CheckDeviceType(deviceId))
            {
                if (BD.CheckDeviceForUser(deviceId) && BD.CheckDeviceType(deviceId))
                {
                    Console.Write("Veuillez entrer votre mot de passe : ");
                    //string password = "Hello123!"; // For testing purposes; replace with Console.ReadLine();
                    using (var passwordForm = new password())
                    {
                        if (passwordForm.ShowDialog() == DialogResult.OK)
                        {
                            string password = passwordForm.Password;
                            return BD.CheckPassword(password);
                        }
                        else
                        {
                            return false;
                        }
                    }




                    //return BD.CheckPassword(password);
                }
                return false;

            }
            else
            {


                return true;
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
