using kiosk.services;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace kiosk
{
    internal class Program
    {
        private const string MAC_ADDRESS = "<mac>";

        static void Main(string[] args)
        {
            Mqtt mqtt = new Mqtt();

            // Currently I use MAC_ADDRESS as MQTT ClientId
            mqtt.Connect(MAC_ADDRESS, "localhost", 1883);

            Task.Run(() => RFIDInputHandler(mqtt));
            Task.Run(() => QRScannerHandler(mqtt));
            Task.Run(() => QRPrintHandler(mqtt));

            while (true) ;
        }

        static async void RFIDInputHandler(Mqtt mqtt)
        {
            Console.WriteLine("Start RFID Handler.");

            // TODO: 1. Parse RFID data to MQTT payload

            string payload = "RFID";

            await mqtt.Publish($"/dev/RFID/{MAC_ADDRESS}", payload);
        }

        static async void QRScannerHandler(Mqtt mqtt)
        {
            Console.WriteLine("Start QR Scanner Handler.");

            while (true)
            {
                var keyPressed = KeyboardReader.Readline();

                await mqtt.Publish($"/dev/qrscan/{MAC_ADDRESS}", keyPressed);
            }
        }

        static async void QRPrintHandler(Mqtt mqtt)
        {
            Console.WriteLine("Start QR Print Handler.");

            async void SubscriptionCallback(string payload)
            {
                QRCode.ConvertTo1Bpp(payload, payload);

                // TODO: 3. Call printer to print the converted image

                await mqtt.Publish($"/dev/kiosk/{MAC_ADDRESS}/entry", payload);
            }

            await mqtt.Subscribe($"/dev/kiosk/{MAC_ADDRESS}/qrprint", SubscriptionCallback);
        }
    }
}
