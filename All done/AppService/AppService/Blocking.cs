using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppService
{
    public class Blocking
    {






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

        public static void AllowRelatedInstances(List<string> relatedDeviceIds, string fullInfPath)
        {
            ;
            foreach (var relatedDeviceId in relatedDeviceIds)
            {
                InstallDriver(relatedDeviceId, fullInfPath);
            }
        }
        public static void CheckAndAddAdditionalValues(string registryPath)
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
        public static void InstallDriver(string deviceId, string fullInfPath)
        {
            //const uint INSTALLFLAG_FORCE = 0x00000001;
            /* try
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
             }*/
            //DriverInstaller.InstallDriver(deviceId,fullInfPath);
           
        }



        //------------------- Uninstall drivers ----------------------------------
        public static void UninstallDriver(string deviceId, string fullInfPath)
        { }



        //TO DO:  link it with the password form
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
            else
            {


                return true;
            }
        }
    
    
    
    
    }
}
