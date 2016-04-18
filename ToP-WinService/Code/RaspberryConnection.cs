using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SKYPE4COMLib;

namespace TriangleOfPower.Code
{
    public class RaspberryConnection : IDisposable
    {
        private readonly StreamReader StreamReader;
        private readonly StreamWriter StreamWriter;
        private readonly Stream _stream;
        private readonly Skype _skype = new Skype();
        private NetworkStream NetworkStream;

        public RaspberryConnection(Stream stream)
        {
            this.StreamReader = new StreamReader(stream);
            this.StreamWriter = new StreamWriter(stream);
        }

        public RaspberryConnection(Stream stream, Socket client) : this(stream)
        {
            NetworkStream = new NetworkStream(client);
        }

        public async Task ReaderLoop3()
        {
            while (NetworkStream != null && NetworkStream.CanRead)
            {
                byte[] myReadBuffer = new byte[1024];
                StringBuilder myCompleteMessage = new StringBuilder();
                int numberOfBytesRead = 0;

                // Incoming message may be larger than the buffer size.
                do
                {
                    numberOfBytesRead = NetworkStream.Read(myReadBuffer, 0, myReadBuffer.Length);

                    myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));

                }
                while (NetworkStream.DataAvailable);

                var command = myCompleteMessage.ToString();
                if (command.Contains(PredicaProtocol.CommandPrefix))
                {
                    command = command
                        .Substring(command.IndexOf(PredicaProtocol.CommandPrefix, StringComparison.Ordinal), 5)
                        .Replace(PredicaProtocol.CommandPrefix, string.Empty);
                    if (Convert.ToInt32(command) == (int)PredicaProtocol.PredicaCommand.ToggleDnd)
                    {
                        if (_skype.CurrentUserStatus != TUserStatus.cusDoNotDisturb)
                            _skype.CurrentUserStatus = TUserStatus.cusDoNotDisturb;
                        else
                            _skype.CurrentUserStatus = TUserStatus.cusOnline;
                    }
                }
            }
        }

        //public async Task ReaderLoop2(Stream reader)
        //{
        //    var networkstr = new NetworkStream();
        //    while (StreamReader != null && StreamReader.Peek() >= 0)
        //    {
        //        long streamLength = StreamReader.BaseStream.Length;
        //        var buffer = new char[streamLength];
        //        var read = StreamReader.Read(buffer, 0, buffer.Length);
        //        string command = new string(buffer)
        //            .Trim();
        //        if (command.Contains(PredicaProtocol.CommandPrefix))
        //        {
        //            command = command
        //                .Substring(command.LastIndexOf(PredicaProtocol.CommandPrefix, StringComparison.Ordinal), 5)
        //                .Replace(PredicaProtocol.CommandPrefix, string.Empty);
        //            if (Convert.ToInt32(command) == (int)PredicaProtocol.PredicaCommand.ToggleDnd)
        //            {
        //                if (_skype.CurrentUserStatus != TUserStatus.cusDoNotDisturb)
        //                    _skype.CurrentUserStatus = TUserStatus.cusDoNotDisturb;
        //                else
        //                    _skype.CurrentUserStatus = TUserStatus.cusOnline;
        //            }
        //        }
        //    }
        //}

        //public async Task ReaderLoop(Stream reader)
        //{
        //    while (reader != null)
        //    {
        //        reader.Read
        //        if (command.Contains(PredicaProtocol.CommandPrefix))
        //        {
        //            command = command.Replace(PredicaProtocol.CommandPrefix, string.Empty);
        //            if (Convert.ToInt32(command) == (int)PredicaProtocol.PredicaCommand.ToggleDnd)
        //            {
        //                if (_skype.CurrentUserStatus != TUserStatus.cusDoNotDisturb)
        //                    _skype.CurrentUserStatus = TUserStatus.cusDoNotDisturb;
        //                else
        //                    _skype.CurrentUserStatus = TUserStatus.cusOnline;
        //            }
        //        }
        //    }
        //}

        public void Start()
       {
            _skype.Attach();
            SendMessage((PredicaProtocol.GetCommand((int)_skype.CurrentUserStatus)));
            _skype.UserStatus += Skype_UserStatus;
            Task.Run(() => ReaderLoop3());
        }

        private void Skype_UserStatus(SKYPE4COMLib.TUserStatus Status)
        {
            SendMessage(PredicaProtocol.GetCommand((int)Status));
        }

        public void SendMessage(string message)
        {
            StreamWriter.WriteLine(message);
            StreamWriter.Flush();
        }


        public void Dispose()
        {
            _stream?.Dispose();
            StreamReader?.Dispose();
            StreamWriter?.Dispose();

        }
    }
}
