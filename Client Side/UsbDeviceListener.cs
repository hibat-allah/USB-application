using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;

namespace ClientSideDocker
{

    public class UsbDeviceListener
    {
        public event Action<string> UsbDeviceConnected;

        private ManagementEventWatcher _watcher;
        private List<string> _previousDriveLetters;

        public UsbDeviceListener()
        {
            //Console.WriteLine("usb device listener ");
            _watcher = new ManagementEventWatcher();
            WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");
            _watcher.EventArrived += new EventArrivedEventHandler(DeviceInsertedEvent);
            _watcher.Query = query;
            _previousDriveLetters = GetUsbDriveLetters();
            //Console.WriteLine("previous drivers " + _previousDriveLetters);
        }

        public void StartListening()
        {
            _watcher.Start();
        }

        public void StopListening()
        {
            _watcher.Stop();
        }

        private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            var newDriveLetter = GetNewUsbDriveLetter();
            if (!string.IsNullOrEmpty(newDriveLetter))
            {
                UsbDeviceConnected?.Invoke(newDriveLetter);
            }
        }

        private string GetNewUsbDriveLetter()
        {
            var currentDriveLetters = GetDriveLetters();
            //Console.WriteLine("current Drive Letters before loop: " + string.Join(", ", currentDriveLetters));

            foreach (var drive in currentDriveLetters)
            {
                if (!_previousDriveLetters.Contains(drive))
                {
                    //Console.WriteLine("this letter is a new one " + drive);
                    _previousDriveLetters = currentDriveLetters; // Update previous drives list
                    //Console.WriteLine("Previous Drive Letters in the loop : " + string.Join(", ", _previousDriveLetters));

                    return drive;
                }
            }
            //Console.WriteLine("Previous Drive Letters: " + string.Join(", ", _previousDriveLetters));

            _previousDriveLetters = currentDriveLetters; // Update previous drives list
            return null;
        }


        private List<string> GetUsbDriveLetters()
        {
            List<string> driveLetters = new List<string>();

            foreach (var drive in Environment.GetLogicalDrives())
            {
                DriveInfo driveInfo = new DriveInfo(drive);
                //Console.WriteLine("new device " + driveInfo.Name+ " type : "+driveInfo.DriveType);
                if (driveInfo.DriveType == DriveType.Removable)
                {
                    driveLetters.Add(drive);
                }
            }

            return driveLetters;
        }
        private List<string> GetDriveLetters()
        {
            List<string> driveLetters = new List<string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT DeviceID, MediaType FROM Win32_DiskDrive");
            ManagementObjectCollection drives = searcher.Get();

            foreach (ManagementObject drive in drives)
            {
                string mediaType = drive["MediaType"]?.ToString() ?? string.Empty;

                // Check if the drive is a removable disk or an external fixed disk
                if (mediaType.Contains("Removable Media") || mediaType.Contains("External hard disk media"))
                {

                    foreach (ManagementObject partition in drive.GetRelated("Win32_DiskPartition"))
                    {
                        foreach (ManagementObject logicalDisk in partition.GetRelated("Win32_LogicalDisk"))
                        {
                            driveLetters.Add(logicalDisk["DeviceID"].ToString());
                        }
                    }
                }
            }
            //Console.WriteLine("driveLetters " +driveLetters);
            return driveLetters;
        }
    }

}
