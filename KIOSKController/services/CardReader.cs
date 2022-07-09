using Sydesoft.NfcDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIOSKController.services
{
    internal class CardReader
    {
        private readonly ACR122U acr122u;
        private readonly Action<string> Callback;

        public CardReader(Action<string> callback)
        {
            Callback = callback;

            acr122u = new ACR122U();
            acr122u.Init(false, 50, 4, 4, 200); // NTAG213
            acr122u.CardInserted += CardInserted;
            acr122u.CardRemoved += CardRemoved;
        }

        private void CardInserted(PCSC.ICardReader reader)
        {
            Console.WriteLine("NFC tag placed on reader.");
            Console.WriteLine("Unique ID: " + BitConverter
                .ToString(acr122u.GetUID(reader))
                .Replace("-", "")
            );

            byte[] rawData = acr122u.ReadData(reader);
            string strData = Encoding.UTF8.GetString(rawData);

            Callback(strData);
        }

        private void CardRemoved()
        {
            Console.WriteLine("NFC tag removed.");
        }
    }
}
