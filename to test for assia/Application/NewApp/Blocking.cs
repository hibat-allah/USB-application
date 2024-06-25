using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NewApp
{
    public class Blocking
    {
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
        public static extern bool GetDeviceInfo(string deviceId, out DeviceInfo deviceInfo);

        [DllImport("DeviceDriverManager.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool UninstallDriver(string deviceId, ref DeviceInfo uninstallData);


        public static void UninstallDevice(string deviceId)
        {

            GetDeviceInfo(deviceId, out DeviceInfo deviceinfo);
            DeviceInfo uninstallData = deviceinfo;
            if (UninstallDriver(deviceId, ref uninstallData))
            {
                 LogHelper.Log("Driver uninstalled successfully.");
            }
            else
            {
                LogHelper.Log("Driver uninstallation failed.");
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


    }
}
