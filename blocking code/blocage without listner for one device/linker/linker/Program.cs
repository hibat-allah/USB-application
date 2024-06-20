using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace linker
{
    /*
    class Program
    {
        [DllImport("YourCppDll.dll", CharSet = CharSet.Unicode)]
        private static extern int ListDeviceProperties(string deviceId, out IntPtr deviceIds);

        [DllImport("YourCppDll.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool InstallDriver(string infPath, IntPtr deviceIds, int deviceIdCount);

        [DllImport("YourCppDll.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetDeviceInfo(string deviceId, out SP_DEVINFO_DATA deviceInfoData, out IntPtr deviceInfoSet);

        [DllImport("YourCppDll.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UninstallDriver(ref SP_DEVINFO_DATA deviceInfoData, IntPtr deviceInfoSet);

        [DllImport("YourCppDll.dll", CharSet = CharSet.Unicode)]
        private static extern void FreeDeviceIds(IntPtr deviceIds, int count);

        [StructLayout(LayoutKind.Sequential)]
        private struct SP_DEVINFO_DATA
        {
            public int cbSize;
            public Guid ClassGuid;
            public int DevInst;
            public IntPtr Reserved;
        }

        static void Main(string[] args)
        {
            string deviceId = "USB\\VID_058F&PID_6387";
            string infPath = "C:\\Windows\\inf\\usbstor.inf";

            // Call the ListDeviceProperties function
            int count = ListDeviceProperties(deviceId, out IntPtr deviceIdsPtr);
            // Convert the returned pointer to a list of strings
            List<string> deviceIds = PtrToStringList(deviceIdsPtr, count);

            if (deviceIds.Count == 0)
            {
                Console.WriteLine("No device IDs found for the specified device.");
                return;
            }

            Console.WriteLine("Press any key to install the device...");
            Console.ReadKey();
            Console.WriteLine("Installing driver...");
            bool installSuccess = InstallDriver(infPath, deviceIdsPtr, count);

            if (installSuccess)
            {
                Console.WriteLine("Driver installed successfully.");
            }
            else
            {
                Console.WriteLine("Driver installation failed.");
            }

            // Get device info and store it
            if (GetDeviceInfo(deviceId, out SP_DEVINFO_DATA deviceInfoData, out IntPtr deviceInfoSet))
            {
                Console.WriteLine("Device info stored. Press any key to uninstall the device...");
                Console.ReadKey();

                // Uninstall device using the stored device info
                bool uninstallSuccess = UninstallDriver(ref deviceInfoData, deviceInfoSet);

                if (uninstallSuccess)
                {
                    Console.WriteLine("Driver uninstalled successfully.");
                }
                else
                {
                    Console.WriteLine("Driver uninstallation failed.");
                }
            }
            else
            {
                Console.WriteLine("Failed to get device info.");
            }

            FreeDeviceIds(deviceIdsPtr, count);
        }

        private static List<string> PtrToStringList(IntPtr ptr, int count)
        {
            var list = new List<string>();
            IntPtr[] ptrArray = new IntPtr[count];
            Marshal.Copy(ptr, ptrArray, 0, count);

            for (int i = 0; i < count; i++)
            {
                list.Add(Marshal.PtrToStringUni(ptrArray[i]));
            }
            return list;
        }
    }
    */

    /*
    class Program
    {
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
            public SP_DEVINFO_DATA DeviceInfoData;
            public IntPtr deviceInfoSet;
        }

        [DllImport("DeviceDriverManager.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool InstallDriver(string deviceId, string infPath, StringBuilder uninstallData, int uninstallDataSize);

        [DllImport("DeviceDriverManager.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool UninstallDriver(string deviceId, string uninstallData);

        [DllImport("DeviceDriverManager.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetDeviceInfo(string deviceId, out DeviceInfo deviceInfo);

        static void Main(string[] args)
        {
            string deviceId = "USB\\VID_058F&PID_6387";
            string infPath = "C:\\Windows\\inf\\usbstor.inf";
            StringBuilder uninstallData = new StringBuilder(1024);

            if (InstallDriver(deviceId, infPath, uninstallData, uninstallData.Capacity))
            {
                Console.WriteLine("Driver installed successfully.");
                Console.WriteLine("Uninstall Data: " + uninstallData.ToString());

                if (GetDeviceInfo(deviceId, out DeviceInfo deviceInfo))
                {
                    Console.WriteLine("Device Info obtained successfully.");

                    // Uninstall the driver using the stored uninstall data
                    if (UninstallDriver(deviceId, uninstallData.ToString()))
                    {
                        Console.WriteLine("Driver uninstalled successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Driver uninstallation failed.");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to get device info.");
                }
            }
            else
            {
                Console.WriteLine("Driver installation failed.");
            }
            Console.ReadKey();
        }
    }
    */
    /*
    public class Program
    {
        [StructLayout(LayoutKind.Sequential)]
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
            public SP_DEVINFO_DATA DeviceInfoData;
            public IntPtr deviceInfoSet;
        }

        [DllImport("DeviceDriverManager.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool InstallDriver(string deviceId, string infPath, [Out] StringBuilder uninstallData, int uninstallDataCapacity);

        [DllImport("DeviceDriverManager.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetDeviceInfo(string deviceId, out DeviceInfo deviceInfo);

        [DllImport("DeviceDriverManager.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool UninstallDriver(string deviceId, string uninstallData);

        public static void Main()
        {
            string deviceId = "USB\\VID_058F&PID_6387";
            string infPath = "C:\\Windows\\inf\\usbstor.inf";
            StringBuilder uninstallData = new StringBuilder(1024);

            if (InstallDriver(deviceId, infPath, uninstallData, uninstallData.Capacity))
            {
                Console.WriteLine("Driver installed successfully.");
            }
            else
            {
                Console.WriteLine("Driver installation failed.");
            }

            DeviceInfo deviceInfo;
            if (GetDeviceInfo(deviceId, out deviceInfo))
            {
                Console.WriteLine("Device info retrieved successfully."+ deviceInfo);
            }
            else
            {
                Console.WriteLine("Failed to retrieve device info.");
            }

            if (UninstallDriver(deviceId, uninstallData.ToString()))
            {
                Console.WriteLine("Driver uninstalled successfully.");
            }
            else
            {
                Console.WriteLine("Driver uninstallation failed.");
            }
            Console.ReadKey();
        }
    }
    */

   
    class Program
    {
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

        static void Main()
        {
            string deviceId = "USB\\VID_058F&PID_6387";
            string infPath = "C:\\Windows\\inf\\usbstor.inf";

            // Installing the driver
            DeviceInfo uninstallData;
            if (InstallDriver(deviceId, infPath, out uninstallData))
            {
                Console.WriteLine("Driver installed successfully.");
                Console.WriteLine("UninstallData:"+ uninstallData);
                Console.WriteLine($"DeviceId: {uninstallData.deviceId}");
            }
            else
            {
                Console.WriteLine("Driver installation failed.");
            }
            Console.WriteLine("want to uninstall it?");
            Console.ReadKey();
            // Uninstalling the driver
            if (UninstallDriver(deviceId, ref uninstallData))
            {
                Console.WriteLine("Driver uninstalled successfully.");
            }
            else
            {
                Console.WriteLine("Driver uninstallation failed.");
            }
            Console.ReadKey();
        }
    }


}
