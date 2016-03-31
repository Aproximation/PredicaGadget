using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKYPE4COMLib;

namespace ToP.Console
{
    public static class PredicaProtocol
    {
        private static readonly string commandPrefix = "#p#";

        private static Dictionary<Command, string> CommandToStringDict = new Dictionary<Command, string>()
        {
            [Command.ChangeRemoteDeviceName] = "ChangeName",
            [Command.StatusChanged] = "StatusChanged",
            [Command.Ping] = "Ping",
            [Command.RotationChanged] = "RotationChanged"
        };

        public enum Command
        {
            ChangeRemoteDeviceName,
            StatusChanged,
            Ping,
            RotationChanged
        }

        public enum SkypeStatus
        {
            Offline = 0,
            Online = 1,
            Away = 2,
            DoNotDisturb = 4,
            Invisible = 5,
        }

        public static void QueueCommand(Command command, string value)
        {
            //string commandId = CommandToStringDict[command];
            Program.OutgoingMeesageQueue.Enqueue(commandPrefix + "04");
        }
        public static void Ping()
        {
            QueueCommand(Command.Ping, DateTime.Now.ToString("T"));
        }

        public static void StatusChanged(TUserStatus userStatus)
        {
            SkypeStatus predicaProtStatus;
            Enum.TryParse(((int)userStatus).ToString(), out predicaProtStatus);
            QueueCommand(Command.StatusChanged, ((int)predicaProtStatus).ToString());
        }
    }
}
