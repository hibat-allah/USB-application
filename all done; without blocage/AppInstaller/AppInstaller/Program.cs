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
            //AddRegistryEntry(appName, appPath);
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
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true)
                ?? Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");
            key?.SetValue("NoDriveTypeAutoRun", 255, RegistryValueKind.DWord);
            key?.Close();
        }

        static void DisableAutoplay()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", true)
                ?? Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers");
            key?.SetValue("DisableAutoplay", 1, RegistryValueKind.DWord);
            key?.Close();
        }
    }

}


