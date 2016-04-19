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
            int retryCount = 0;
            BluetoothDeviceInfo discoveredDevice = Discover(name);
            while (discoveredDevice == null)
            {
                retryCount++;
                App.Status = $"Looking for {name}. Retry count: {retryCount}";
                Task.Delay(5000);
                discoveredDevice = Discover(name);
            }
            return discoveredDevice
                .ForcePair()
                .Connect();
        }

        private static BluetoothClient Connect(this BluetoothDeviceInfo device)
        {
            App.Status = "Connecting...";
            BluetoothClient client = new BluetoothClient();
            BluetoothEndPoint ep = new BluetoothEndPoint(device.DeviceAddress, BluetoothService.SerialPort);
            client.Connect(ep);
            return client;
        }

        private static BluetoothDeviceInfo ForcePair(this BluetoothDeviceInfo device)
        {
            App.Status = "Pairing...";
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
            return client.DiscoverDevicesInRange().FirstOrDefault(x => x.DeviceName.Contains(deviceName));           
            //if (device == null)
            //{
            //    App.MainIcon.ShowBalloonTip(30000, Properties.Resources.AppName, "HC-05 not found", ToolTipIcon.Warning);
            //    return null;
            //}
            //return device;
        }
    }
}
