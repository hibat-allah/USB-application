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

        private ManagementEventWatcher _insertWatcher;
        private ManagementEventWatcher _removeWatcher;
        private List<string> _previousDriveLetters;

        public UsbDeviceListener()
        {
            _insertWatcher = new ManagementEventWatcher();
            _removeWatcher = new ManagementEventWatcher();

            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");
            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 3");

            _insertWatcher.EventArrived += new EventArrivedEventHandler(DeviceInsertedEvent);
            _removeWatcher.EventArrived += new EventArrivedEventHandler(DeviceRemovedEvent);
            //Console.WriteLine("Previous Drive Letters in the loop : " + string.Join(", ", _previousDriveLetters));
 //Console.WriteLine("Previous Drive Letters in the loop : " + string.Join(", ", _previousDriveLetters));


            _insertWatcher.Query = insertQuery;
            _removeWatcher.Query = removeQuery;

            _previousDriveLetters = GetUsbDriveLetters();
        }

        public void StartListening()
        {
            _insertWatcher.Start();
            _removeWatcher.Start();
        }

        public void StopListening()
        {
            _insertWatcher.Stop();
            _removeWatcher.Stop();
        }

        private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            var newDriveLetter = GetNewUsbDriveLetter();
            if (!string.IsNullOrEmpty(newDriveLetter))
            {
                UsbDeviceConnected?.Invoke(newDriveLetter);
            }
        }

        private void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
        {
            var removedDriveLetters = GetRemovedUsbDriveLetters();
            foreach (var drive in removedDriveLetters)
            {
                _previousDriveLetters.Remove(drive);
            }
        }

        private string GetNewUsbDriveLetter()
        {
            var currentDriveLetters = GetDriveLetters();
            foreach (var drive in currentDriveLetters)
            {
                if (!_previousDriveLetters.Contains(drive))
                {
                    _previousDriveLetters.Add(drive);
                    return drive;
                }
            }
            return null;
        }

        private List<string> GetRemovedUsbDriveLetters()
        {
            var currentDriveLetters = GetDriveLetters();
            return _previousDriveLetters.Except(currentDriveLetters).ToList();
        }

        private List<string> GetUsbDriveLetters()
        {
            List<string> driveLetters = new List<string>();

            foreach (var drive in Environment.GetLogicalDrives())
            {
                DriveInfo driveInfo = new DriveInfo(drive);
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
            return driveLetters;
        }
    }
}


