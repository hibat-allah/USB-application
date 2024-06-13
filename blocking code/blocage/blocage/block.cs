using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blocage
{
    public class block
    {
        static string registryPath = "SOFTWARE\\Policies\\Microsoft\\Windows\\DeviceInstall\\Restrictions\\AllowDeviceIDs";
        static string registryPath2 = "SOFTWARE\\Wow6432Node\\Policies\\Microsoft\\Windows\\DeviceInstall\\Restrictions\\AllowDeviceIDs";

        public static void blockUSB(string hardwareId)
            {
            try
            {
                // block in the form: USB\PID VID
                //var parts = hardwareId.Split('\\');
                string vidpid=hardwareId;
                Blocking.DeleteRegistryValue(registryPath, vidpid);
                Blocking.DeleteRegistryValue(registryPath2, vidpid);

                string devconPath = @"devcon.exe";

                // Command to remove the device driver
                string command = $"remove \"{hardwareId}\"";

                ProcessStartInfo psi = new ProcessStartInfo(devconPath, command);
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.Verb = "runas"; // Run as administrator

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        throw new Exception($"Failed to remove device. Exit code: {process.ExitCode}");
                    }
                    else
                    {
                        Console.WriteLine("Device removed successfully. In the blocking function");
                    }
                }




            }
                catch (Exception ex) {
                Console.WriteLine("error: " + ex.ToString());
                Console.ReadKey();
                }
            }

           
        }
    }

