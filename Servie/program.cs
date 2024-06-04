using System;
using System.Diagnostics;
using System.Management;
using System.ServiceProcess;

public partial class UsbMonitorService : ServiceBase
{
    public UsbMonitorService()
    {
        InitializeComponent();
    }

    protected override void OnStart(string[] args)
    {
        EventLog.WriteEntry("UsbMonitorService started.");

        string status = CheckStatusInDatabase();

        if (status == "complete")
        {
            DisableUsbPorts();
        }
        else if (status == "selected")
        {
            StartUsbDeviceListener();
        }
    }

    protected override void OnStop()
    {
        EventLog.WriteEntry("UsbMonitorService stopped.");
    }

    private string CheckStatusInDatabase()
    {
        // Add your database logic here to check the status
        return "selected"; // Example status
    }

    private void DisableUsbPorts()
    {
        // Add your logic to disable USB ports here
    }

    private void StartUsbDeviceListener()
    {
        ManagementEventWatcher watcher = new ManagementEventWatcher();
        WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");
        watcher.EventArrived += new EventArrivedEventHandler(DeviceInsertedEvent);
        watcher.Query = query;
        watcher.Start();
    }

    private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
    {
        // Get the USB device ID and other details if needed
        string deviceId = GetUsbDeviceId();
        Process.Start("path_to_your_application.exe", deviceId);
    }

    private string GetUsbDeviceId()
    {
        // Add your logic to get the USB device ID here
        return "example_device_id";
    }
}
