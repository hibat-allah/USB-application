
/*
namespace NewApp
{
    internal static class Program
    {
        private static ManualResetEvent _exitEvent = new ManualResetEvent(false);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Console.WriteLine("Listening for USB devices. Press any key to exit...");
            //while (true)
            //{
                UsbDeviceListener usbListener = new UsbDeviceListener();
                usbListener.UsbDeviceConnected += async (driveLetter) =>
                {
                    Docker.isInfected = false; Docker.path = "";
                    Console.WriteLine($"New USB Device Connected - Drive: {driveLetter}");
                    //await Docker.HandleUsbDevice(driveLetter);
                    
                    if (!Docker.isInfected)
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form1());
                    }
                };
            /*
                usbListener.StartListening();
            Thread.Sleep(10000);
            //Console.ReadKey();
            usbListener.StopListening();
            //}*/
/*   // Start listening for USB devices in a separate thread
   Thread usbListenerThread = new Thread(() =>
   {
       usbListener.StartListening();
   });

   usbListenerThread.Start();

   // Wait for the exit signal
   _exitEvent.WaitOne();

   usbListener.StopListening();
   usbListenerThread.Join();

}
}
}
*/


using System;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewApp;

namespace NewApp
{
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        [STAThread]
        static async Task Main(String[] args)
        {
            string logFilePath = @"C:\Users\ASUS\Documents\LogApp.txt"; // Update with an appropriate path
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
            try
            {

                    writer.WriteLine("Application started at: " + DateTime.Now);

                    //if (args.Length >= 2)
                    //{
                    string usbDeviceId = "USB\\VID_058F&PID_6387";//args[0];



                    string driverletter = "F:\\";//args[1];
                        bool Infected = await Docker.HandleUsbDevice(driverletter);
                        writer.WriteLine("after docker " + DateTime.Now);

                        //writer.WriteLine($"Received USB Device ID: {usbDeviceId}");
                        writer.WriteLine($"Received Drive Letter: {driverletter}");

                        writer.WriteLine($"is it infected? : {Infected}");

                        if (!Infected)
                        {
                            await FileTraverser.TraverseFilesAsync(driverletter);
                            Application.EnableVisualStyles();
                            Application.SetCompatibleTextRenderingDefault(false);
                            Application.Run(new Form1(driverletter));
                            writer.WriteLine($"interface lunched ");
                            
                            writer.WriteLine("after traversing files");
                        }
                        else
                        {
                             string registryPath = @"SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions\AllowDeviceIDs";
                             string registryPath2 = @"SOFTWARE\Wow6432Node\Policies\Microsoft\Windows\DeviceInstall\Restrictions\AllowDeviceIDs";

                            //Block(usbDeviceId);
                            Blocking.DeleteRegistryValue(registryPath, usbDeviceId);
                            Blocking.DeleteRegistryValue(registryPath2, usbDeviceId);

                            // here blocking
                            Blocking.UninstallDevice(usbDeviceId);
                        }
                       

                    //}

            }
            catch (Exception ex)
            {
                writer.WriteLine($"Unexpected error: {ex.Message}");
                // Log the exception (consider using a logging framework)
               
            }
                }
        }
    }
}

