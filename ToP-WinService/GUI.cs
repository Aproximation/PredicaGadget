using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToP.Console
{
    public static class Gui
    {
        public enum TextLocation
        {
            Status,
            DeviceName,
            Sender,
            Reciever,
            Output
        }


        public static void WriteTo(TextLocation location, string value)
        {
            string baseMessage = "";
            switch (location)
            {
                case TextLocation.Status:
                    System.Console.SetCursorPosition(0, 0);
                    baseMessage = "Status: ";
                    break;
                case TextLocation.DeviceName:
                    System.Console.SetCursorPosition(0, System.Console.WindowHeight - 1);
                    baseMessage = "Connected device: ";
                    break;
                case TextLocation.Sender:
                    System.Console.SetCursorPosition(0, 1);
                    baseMessage = "Sender thread: ";
                    break;
                case TextLocation.Reciever:
                    System.Console.SetCursorPosition(0, 2);
                    baseMessage = "Reciever thread: ";
                    break;
                case TextLocation.Output:
                    System.Console.SetCursorPosition(0, 4);
                    baseMessage = "Received message: ";
                    break;
                default:
                    System.Console.SetCursorPosition(0, 0);
                    break;
            }

            ClearCurrentConsoleLine();
            System.Console.Write(baseMessage + value);
            //System.Console.SetCursorPosition(0, 0);
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = System.Console.CursorTop;
            System.Console.SetCursorPosition(0, System.Console.CursorTop);
            System.Console.Write(new string(' ', System.Console.WindowWidth));
            System.Console.SetCursorPosition(0, currentLineCursor);
        }


    }
}
