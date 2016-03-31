using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using SKYPE4COMLib;

namespace ToP.Console
{
    class Program
    {
        private static Skype skype = null;
        public static Queue<string> OutgoingMeesageQueue = new Queue<string>(); 
        private static bool ClientConnected = false;
        private static DataWriter writer = null;
        static void Main(string[] args)
        {
            //System.Console.Write("Enter device name: ");
            //string deviceName = System.Console.ReadLine();
            Gui.WriteTo(Gui.TextLocation.Status, "Attaching to Skype...");
            skype = new Skype();
            skype.Attach();
            Gui.WriteTo(Gui.TextLocation.Status, "Attached!");
            StartBluetoothServer();
            System.Console.ReadLine();
        }

        private static async void Skype_UserStatus(TUserStatus Status)
        {
            if (writer == null) return;
            writer.WriteString(PredicaProtocol.GetCommand((int)Status));
            await writer.StoreAsync();
            await writer.FlushAsync();
        }

        static async void StartBluetoothServer(string deviceName = "HC-05")
        {
            IInputStream stream = null;
            IOutputStream ostream = null;
            try
            {
                var devicesInfoCollection = await DeviceInformation.FindAllAsync(
                    RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort));
                var myDevice = devicesInfoCollection.FirstOrDefault(d => d.Name.ToUpper().Contains(deviceName));

                Gui.WriteTo(Gui.TextLocation.Status, "Connecting to \"" + deviceName + "\"");
                var BTService = await RfcommDeviceService.FromIdAsync(myDevice.Id);
                var mySocket = new StreamSocket();
                stream = mySocket.InputStream;
                ostream = mySocket.OutputStream;

                await mySocket.ConnectAsync(BTService.ConnectionHostName, BTService.ConnectionServiceName);
                Gui.WriteTo(Gui.TextLocation.Status, "Connected to \"" + deviceName + "\"");
                ClientConnected = true;
            }
            catch (Exception e)
            {
                Gui.WriteTo(Gui.TextLocation.Status, "Connected to \"" + deviceName + "\". Warning - connection was already open");
                System.Console.WriteLine(e.Message);
            }
            Parallel.Invoke(() =>
            {
                ReaderLoop(stream);
                WriterLoop(ostream);
            });
        }

        private static async void WriterLoop(IOutputStream ostream)
        {
            var writer = new DataWriter(ostream);
            skype.UserStatus += Skype_UserStatus;
        }

        private static async void ReaderLoop(IInputStream stream)
        {
            var reader = new DataReader(stream);
            reader.InputStreamOptions = InputStreamOptions.ReadAhead;
            while (ClientConnected)
            {
                uint messageLength = await reader.LoadAsync(5);
                string message = reader.ReadString(messageLength);
                if (message.Contains(PredicaProtocol.CommandPrefix))
                {
                    message = message.Replace(PredicaProtocol.CommandPrefix, string.Empty);
                    if (Convert.ToInt32(message) == (int) PredicaProtocol.PredicaCommand.ToggleDnd)
                    {
                        if(skype.CurrentUserStatus != TUserStatus.cusDoNotDisturb)
                            skype.CurrentUserStatus = TUserStatus.cusDoNotDisturb;
                        else
                            skype.CurrentUserStatus = TUserStatus.cusOnline;
                    }
                }
            }
        }
      
    }
}
