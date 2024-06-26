using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;

namespace AppService
{
    public class UsbDeviceListener
    {
        public event Action<string> UsbDeviceConnected;
        public event Action<List<string>> UsbDeviceRemoved;

        private ManagementEventWatcher _insertWatcher;
        private ManagementEventWatcher _removeWatcher;
        private List<string> _previousDriveLetters;
        private Dictionary<string, string> deviceMapping; // New dictionary to store the mapping

        public UsbDeviceListener()
        {
            _insertWatcher = new ManagementEventWatcher();
            _removeWatcher = new ManagementEventWatcher();

            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'");
            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'");

            _insertWatcher.EventArrived += new EventArrivedEventHandler(DeviceInsertedEvent);
            _removeWatcher.EventArrived += new EventArrivedEventHandler(DeviceRemovedEvent);

            _insertWatcher.Query = insertQuery;
            _removeWatcher.Query = removeQuery;

            _previousDriveLetters = GetUsbDriveLetters();
            deviceMapping = new Dictionary<string, string>(); // Initialize the dictionary
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
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            string fullDeviceId = instance["DeviceID"].ToString();
            LogHelper.Log("device inserted 0"+fullDeviceId);

            var newDriveLetter = GetNewUsbDriveLetter();
            string classS = Form1.ExtractDeviceClass(instance);

            if (!string.IsNullOrEmpty(classS))
                { UsbDeviceConnected?.Invoke($"{fullDeviceId}:{classS}"); }
            else
               { UsbDeviceConnected?.Invoke($"{fullDeviceId}:null"); }
            /*if (!string.IsNullOrEmpty(newDriveLetter))
            {
                UsbDeviceConnected?.Invoke($"{newDriveLetter}");
            }
            else
            {
                UsbDeviceConnected?.Invoke($"null");
            }*/
        }

        private void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
        {
            /*var removedDriveLetters = GetRemovedUsbDriveLetters();
            foreach (var drive in removedDriveLetters)
            {
                _previousDriveLetters.Remove(drive);
            }
            if (removedDriveLetters.Any())
            {
                List<string> deviceInfoList = removedDriveLetters.Select(driveLetter => $"{driveLetter}").ToList();
                UsbDeviceRemoved?.Invoke(deviceInfoList);
            }
            else
            {
                UsbDeviceRemoved?.Invoke(new List<string> { $"null" });
            }*/
            var removedDriveLetters = GetRemovedUsbDriveLetters();
            foreach (var drive in removedDriveLetters)
            {
                _previousDriveLetters.Remove(drive);
            }

            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            var removedDeviceId = instance["DeviceID"].ToString();

            if (removedDriveLetters.Any())
            {
                List<string> deviceInfoList = removedDriveLetters.Select(driveLetter => $"{removedDeviceId}:{driveLetter}").ToList();
                UsbDeviceRemoved?.Invoke(deviceInfoList);
            }
            else
            {
                UsbDeviceRemoved?.Invoke(new List<string> { $"{removedDeviceId}:null" });
            }
        }

        public string GetNewUsbDriveLetter()
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

        public List<string> GetRemovedUsbDriveLetters()
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

        // New method to get the drive letter based on hardware ID
        public string GetDriveLetterByHardwareID(string hardwareID)
        {
            LogHelper.Log("i'm in the getting letter function in the listner");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT DeviceID, PNPDeviceID FROM Win32_DiskDrive");
            ManagementObjectCollection drives = searcher.Get();

            foreach (ManagementObject drive in drives)
            {
                string pnpDeviceID = drive["PNPDeviceID"]?.ToString() ?? string.Empty;
                LogHelper.Log("getting functio, thiis are the pnpdeviceids "+pnpDeviceID + "\n hardwarID "+hardwareID); 
                if (pnpDeviceID.IndexOf(hardwareID, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    foreach (ManagementObject partition in drive.GetRelated("Win32_DiskPartition"))
                    {
                        foreach (ManagementObject logicalDisk in partition.GetRelated("Win32_LogicalDisk"))
                        {
                            string driveLetter = logicalDisk["DeviceID"].ToString();
                            if (!_previousDriveLetters.Contains(driveLetter))
                            {
                                _previousDriveLetters.Add(driveLetter);
                                LogHelper.Log("driverletter " + driveLetter);
                                return driveLetter;
                            }
                        }
                    }
                }
            }
            return null;
        }

        // New method to map USB device ID to storage device ID
        public void AddDeviceMapping(string usbDeviceId, string storageDeviceId)
        {
            if (!deviceMapping.ContainsKey(storageDeviceId))
            {
                deviceMapping[storageDeviceId] = usbDeviceId;
            }
        }

        public string GetUsbDeviceId(string storageDeviceId)
        {
            return deviceMapping.ContainsKey(storageDeviceId) ? deviceMapping[storageDeviceId] : null;
        }


        public void RemoveDeviceMapping(string usbDeviceId, string storageDeviceId)
        {
            if (!deviceMapping.ContainsKey(storageDeviceId))
            {
                deviceMapping[storageDeviceId] = usbDeviceId;
            }
        }
    }
}
