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
using InTheHand.Net.Sockets;
using SKYPE4COMLib;

namespace ToP.Console
{
    class Program
    {
        private static Skype skype = null;
        public static Queue<string> OutgoingMeesageQueue = new Queue<string>(); 
        private static bool ClientConnected = false;
        static void Main(string[] args)
        {
            System.Console.Write("Enter device name: ");
            string deviceName = System.Console.ReadLine();
            Gui.WriteTo(Gui.TextLocation.Status, "Attaching to Skype...");
            skype = new Skype();
            skype.Attach();
            skype.UserStatus += Skype_UserStatus;
            skype.CurrentUserStatus = TUserStatus.cusNotAvailable;
            Gui.WriteTo(Gui.TextLocation.Status, "Attached!");
            StartBluetoothServer(deviceName);
            System.Console.ReadLine();
        }

        private static void Skype_UserStatus(TUserStatus Status)
        {
            PredicaProtocol.StatusChanged(Status);
        }

        static async void StartBluetoothServer(string deviceName)
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
            }
            Parallel.Invoke(() =>
            {
                ReaderLoop(stream);
                WriterLoop(ostream);
            });
        }

        private static async void WriterLoop(IOutputStream ostream)
        {
            Gui.WriteTo(Gui.TextLocation.Sender, "Starting...");
            //int dotsToDisplay = 0;
            var writer = new DataWriter(ostream);
            int loopTurn = 0;
            while (ClientConnected)
            {
                await Task.Delay(1000);
                loopTurn++;
                if (loopTurn >= 3)
                {
                    PredicaProtocol.Ping();
                    loopTurn = 0;
                }               
                //if (OutgoingMeesageQueue.Count == 0)
                //{
                //    Gui.WriteTo(Gui.TextLocation.Sender, "Idle" + new string('.', dotsToDisplay));
                //    dotsToDisplay++;
                //    if (dotsToDisplay > 3)
                //        dotsToDisplay = 0;
                //    await Task.Delay(1000);
                //    continue;
                //}
                //Gui.WriteTo(Gui.TextLocation.Sender, "Sending message...");
                if(OutgoingMeesageQueue.Count == 0) continue;
                var message = OutgoingMeesageQueue.Dequeue();
                int count = message.Length;
                //uint ucount = Convert.ToInt32()
                writer.WriteString(Convert.ToString(count));
                writer.WriteString(message);
                await writer.StoreAsync();
                await writer.FlushAsync();
                Gui.WriteTo(Gui.TextLocation.Sender, "Sent: " + count + message);                
            }
            Gui.WriteTo(Gui.TextLocation.Sender, "Disconnected");
        }

        private static async void ReaderLoop(IInputStream stream)
        {
            Gui.WriteTo(Gui.TextLocation.Reciever, "Starting...");
            var reader = new DataReader(stream);
            reader.InputStreamOptions = InputStreamOptions.Partial;

            while (ClientConnected)
            {
                Gui.WriteTo(Gui.TextLocation.Reciever, "Awaiting message...");
                uint messageLength = await reader.LoadAsync(sizeof(uint));
                //if (messageLength == 0)
                //{
                //    Gui.WriteTo(Gui.TextLocation.Reciever, "Received disconnection request (message count = 0)");
                //    ClientConnected = false;
                //}
                //Gui.WriteTo(Gui.TextLocation.Reciever, "Reading message...");
                string message = reader.ReadString(messageLength);
                Gui.WriteTo(Gui.TextLocation.Output, message);
            }
            Gui.WriteTo(Gui.TextLocation.Sender, "Disconnecting...");
            Gui.WriteTo(Gui.TextLocation.Reciever, "Disconnected");
            Gui.WriteTo(Gui.TextLocation.Status, "Disconnected gracefully");
        }

        //private static async void OnConnectionReceived(
        //    StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        //{
        //    try
        //    {
        //        NotifyStatus("Client Connected");

        //        // Don't need the listener anymore
        //        //socketListener.Dispose();
        //        //socketListener = null;

        //        var socket = args.Socket;

        //        var writer = new DataWriter(socket.OutputStream);

        //        var reader = new DataReader(socket.InputStream);
        //        bool remoteDisconnection = false;
        //        while (true)
        //        {
        //            uint readLength = await reader.LoadAsync(sizeof(uint));
        //            if (readLength < sizeof(uint))
        //            {
        //                remoteDisconnection = true;
        //                break;
        //            }
        //            uint currentLength = reader.ReadUInt32();

        //            readLength = await reader.LoadAsync(currentLength);
        //            if (readLength < currentLength)
        //            {
        //                remoteDisconnection = true;
        //                break;
        //            }
        //            string message = reader.ReadString(currentLength);
        //            System.Console.WriteLine(message);

        //            //await ApplicationDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
        //            //{
        //            //    ConversationListBox.Items.Add("Received: " + message);
        //            //});
        //        }

        //        reader.DetachStream();
        //        if (remoteDisconnection)
        //        {
        //            //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
        //            //{
        //            //    Disconnect();
        //            //    NotifyStatus("Client disconnected.");
        //            //});
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        NotifyError(e);
        //    }
        //}
    }
}
