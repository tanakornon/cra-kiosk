using KIOSKController.services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KIOSKController
{
    public partial class Home : Form
    {
        private const string MAC_ADDRESS = "<mac>";

        private MqttClient mqtt = new MqttClient();

        public Home()
        {
            InitializeComponent();

            // MqttClient mqtt = new MqttClient();

            
            /*
            mqtt.Connect(MAC_ADDRESS, "localhost", 1883);

            Task.Run(() => NFCCardReader(mqtt));
            Task.Run(() => QRScanner(mqtt));
            Task.Run(() => Printer(mqtt));
            */
        }

        private void BTNStart_Click(object sender, EventArgs e)
        {
            mqtt.Publish($"/dev/qrscan/{MAC_ADDRESS}", "test");
        }

        private async void NFCCardReader(MqttClient mqtt)
        {
            Console.WriteLine("Start ACR122U Card Reader.");

            async void Publish(string data)
            {
                string payload = "RFID";

                await mqtt.Publish($"/dev/RFID/{MAC_ADDRESS}", payload);
            }

            CardReader cardReader = new CardReader(Publish);

            while (true) ;
        }

        private async void QRScanner(MqttClient mqtt)
        {
            Console.WriteLine("Start QR Scanner.");



            USBReader usbReader = new USBReader(0x046D, 0x024F);
            usbReader.Start();

            while (true)
            {
                // var keyPressed = KeyboardReader.Readline();


                // await mqtt.Publish($"/dev/qrscan/{MAC_ADDRESS}", keyPressed);
            }
        }

        private async void Printer(MqttClient mqtt)
        {
            Console.WriteLine("Start Printer.");

            async void Subscribe(string payload)
            {
                string sourceFile = payload;
                string targetFile = payload;

                ImageProcesser.ConvertTo1Bpp(sourceFile, targetFile);

                MsPrinter printer = new MsPrinter();
                printer.Print(targetFile);

                await mqtt.Publish($"/dev/kiosk/{MAC_ADDRESS}/entry", payload);
            }

            await mqtt.Subscribe($"/dev/kiosk/{MAC_ADDRESS}/qrprint", Subscribe);

            Console.WriteLine("Stop Printer.");
        }
    }
}
