
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
using System.Threading;
using System.Windows.Forms;
using NewApp;

namespace NewApp
{
    public class Program
    {
        private static ManualResetEvent _exitEvent = new ManualResetEvent(false);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
           /* Console.WriteLine("Listening for USB devices. Press any key to exit...");

            // Start the background process
            Thread backgroundThread = new Thread(BackgroundProcess);
            backgroundThread.Start();

            // Start listening for USB devices
            UsbDeviceListener usbListener = new UsbDeviceListener();
            usbListener.UsbDeviceConnected += async (driveLetter) =>
            {
                Docker.isInfected = false;
                Docker.path = "";
                Console.WriteLine($"New USB Device Connected - Drive: {driveLetter}");

                if (!Docker.isInfected)
                {*/
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                /*}
            };

            // Start listening for USB devices in a separate thread
            Thread usbListenerThread = new Thread(() =>
            {
                usbListener.StartListening();
            });

            usbListenerThread.Start();

            // Wait for key press to exit
            //Console.ReadKey();
            _exitEvent.Set();

            usbListener.StopListening();
            usbListenerThread.Join();
            backgroundThread.Join();*/
        }

        static void BackgroundProcess()
        {
            while (!_exitEvent.WaitOne(0))
            {
                // Your background process logic here
                Console.WriteLine("Running background process...");
                Thread.Sleep(1000); // Simulate work by sleeping
            }
        }
    }
}

