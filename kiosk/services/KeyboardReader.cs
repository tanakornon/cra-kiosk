using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk.services
{
    internal class KeyboardReader
    {
        public static string Readline()
        {
            string pressedKey = "";

            ConsoleKey exitKey = ConsoleKey.Enter;

            ConsoleKeyInfo cki;

            // Prevent example from ending if CTL+C is pressed.
            // Console.TreatControlCAsInput = true;

            // Console.WriteLine("Press any combination of CTL, ALT, and SHIFT, and a console key.");
            // Console.WriteLine("Press the Escape (Esc) key to quit: \n");

            while (true)
            {
                cki = Console.ReadKey();

                // Console.Write(" --- You pressed ");
                // if ((cki.Modifiers & ConsoleModifiers.Alt) != 0) Console.Write("ALT+");
                // if ((cki.Modifiers & ConsoleModifiers.Shift) != 0) Console.Write("SHIFT+");
                // if ((cki.Modifiers & ConsoleModifiers.Control) != 0) Console.Write("CTL+");

                // Console.WriteLine(cki.KeyChar.ToString());

                if (cki.Key == exitKey) break;

                pressedKey += cki.KeyChar;
            }

            return pressedKey;
        }
    }
}
