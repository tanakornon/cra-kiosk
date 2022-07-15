using KIOSKController.services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
        private readonly string deviceId = ConfigurationManager.AppSettings["deviceId"];

        public Home()
        {
            InitializeComponent();

            MqttClient mqtt = new MqttClient();
            mqtt.Connect();

            Task.Run(() => NFCCardReader(mqtt));
            Task.Run(() => Printer(mqtt));
            QRScanner(mqtt);
        }

        private void BTNStart_Click(object sender, EventArgs e)
        {

        }

        private void BTNPrint_Click(object sender, EventArgs e)
        {
            MsPrinter printer = new MsPrinter();
            printer.Print("test.bmp");
        }

        private void NFCCardReader(MqttClient mqtt)
        {
            Console.WriteLine("Start ACR122U Card Reader.");

            async void Publish(string data)
            {
                await mqtt.Publish($"/dev/RFID/{deviceId}", data);
            }

            CardReader cardReader = new CardReader(Publish);

            while (true) ;
        }

        private void QRScanner(MqttClient mqtt)
        {
            Console.WriteLine("Start QR Scanner.");

            async void Publish(string data)
            {
                await mqtt.Publish($"/dev/qrscan/{deviceId}", data);
            }

            InterceptKeys.Hook();
            InterceptKeys.Callback = Publish;
        }

        private async void Printer(MqttClient mqtt)
        {
            try
            {
                Console.WriteLine("Start Printer.");

                async void Subscribe(string payload)
                {
                    string sourceFile = payload;
                    string targetFile = payload;

                    ImageProcessor.ConvertTo1Bpp(sourceFile, targetFile);

                    MsPrinter printer = new MsPrinter();
                    printer.Print(targetFile);

                    await mqtt.Publish($"/dev/kiosk/{deviceId}/entry", payload);
                }

                await mqtt.Subscribe($"/dev/kiosk/{deviceId}/qrprint", Subscribe);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
