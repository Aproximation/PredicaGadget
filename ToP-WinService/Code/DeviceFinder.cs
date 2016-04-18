using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand.Net.Sockets;
using System.Management;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Factory;

namespace TriangleOfPower.Code
{
    public static class DeviceFinder
    {
        public static BluetoothClient GetClientForDevice(string name)
        {
            return Discover(name)
                .ForcePair()
                .Connect();
        }

        private static BluetoothClient Connect(this BluetoothDeviceInfo device)
        {
            BluetoothClient client = new BluetoothClient();
            BluetoothEndPoint ep = new BluetoothEndPoint(device.DeviceAddress, BluetoothService.SerialPort);
            client.Connect(ep);
            return client;
        }

        private static BluetoothDeviceInfo ForcePair(this BluetoothDeviceInfo device)
        {
            if (device.Authenticated)
                BluetoothSecurity.RemoveDevice(device.DeviceAddress);
            if (BluetoothSecurity.PairRequest(device.DeviceAddress, "1234"))
                return device;
            App.MainIcon.ShowBalloonTip(30000, Properties.Resources.AppName, "Unable to pair with device", ToolTipIcon.Error);
            return null;

        }

        public static BluetoothDeviceInfo Discover(this string deviceName)
        {
            BluetoothClient client = new BluetoothClient();
            var device = client.DiscoverDevicesInRange().FirstOrDefault(x => x.DeviceName.Contains(deviceName));           
            if (device == null)
            {
                App.MainIcon.ShowBalloonTip(30000, Properties.Resources.AppName, "HC-05 not found", ToolTipIcon.Warning);
                return null;
            }
            return device;
        }

        //private static string GetBluetoothPort(string deviceAddress)
        //{
        //    const string Win32_SerialPort = "Win32_SerialPort";
        //    SelectQuery q = new SelectQuery(Win32_SerialPort);
        //    ManagementObjectSearcher s = new ManagementObjectSearcher(q);
        //    foreach (object cur in s.Get())
        //    {
        //        ManagementObject mo = (ManagementObject)cur;
        //        string pnpId = mo.GetPropertyValue("PNPDeviceID").ToString();

        //        if (pnpId.Contains(deviceAddress))
        //        {
        //            object captionObject = mo.GetPropertyValue("Caption");
        //            string caption = captionObject.ToString();
        //            int index = caption.LastIndexOf("(COM", StringComparison.Ordinal);
        //            if (index > 0)
        //            {
        //                string portString = caption.Substring(index);
        //                string comPort = portString.
        //                              Replace("(", string.Empty).Replace(")", string.Empty);
        //                return comPort;
        //            }
        //        }
        //    }
        //    System.Windows.MessageBox.Show(
        //        "Unable to find any serial port for device communication. Please remove and pair the Triangle of Power");
        //    return null;
        //}
    }
}
