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
        public static readonly string CommandPrefix = "#p#";

        public enum SkypeStatus
        {
            Offline = 0,
            Online = 1,
            Away = 2,
            DoNotDisturb = 4,
            Invisible = 5,
        }

        public enum PredicaCommand
        {
            ToggleDnd = 0
        }

        public static string GetCommand(int status)
        {
            SkypeStatus convertedStatus = SkypeStatus.Offline;
            Enum.TryParse(status.ToString(), out convertedStatus);
            return CommandPrefix + $"{(int)convertedStatus:00}";
        }
    }
}
