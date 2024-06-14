using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            // Specify the name and path of your application's executable
            string appName = "USBApp";
            string appPath = @"C:\Users\user\source\repos\AppService\AppService\bin\Debug\AppService.exe";

            // Add shortcut to Startup folder
            //AddShortcutToStartup(appPath);

            // Add registry entry
            AddRegistryEntry(appName, appPath);
            LaunchApplication(appPath);
            Console.WriteLine("App lunched ...");
            DisableAutorun();
            DisableAutoplay();
        }

        static void AddShortcutToStartup(string targetPath)
        {
            // Get the path to the Startup folder for the current user
            string startupFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "MyApp.lnk");

            // Create a shortcut targetting your application's executable
            IWshRuntimeLibrary.WshShell wshShell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)wshShell.CreateShortcut(startupFolderPath);
            shortcut.TargetPath = targetPath;
            shortcut.Save();
        }

        static void AddRegistryEntry(string appName, string appPath)
        {
            // Create or open the registry key
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

            // Add your application to the registry
            key.SetValue(appName, appPath);
            key.Close();
        }
        private static void LaunchApplication(string applicationPath)
        {
            // Specify the path of the application you want to launch
            

            // Launch the application
            Process.Start(applicationPath);
        }
        static void DisableAutorun()
        {
            string key = @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";
            ModifyRegistry(key, "DisableAutoplay", "255");
        }

        static void DisableAutoplay()
        {
            string key = @"Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers";
            ModifyRegistry(key, "DisableAutoplay", "1");
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


    }

}


