using HidLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIOSKController.services
{
    internal class USBReader
    {
        private readonly int VendorId = 0x046D;
        private readonly int ProductId = 0xC33A;

        private HidDevice _device;

        public USBReader(int vendorId, int productId)
        {
            VendorId = vendorId;
            ProductId = productId;
        }

        public void Start()
        {
            _device = HidDevices.Enumerate(VendorId, ProductId).FirstOrDefault();

            if (_device != null)
            {
                _device.OpenDevice();

                _device.Inserted += DeviceAttachedHandler;
                _device.Removed += DeviceRemovedHandler;

                _device.MonitorDeviceEvents = true;

                _device.ReadReport(OnReport);

                Console.WriteLine("Reader found, press any key to exit.");

                while (true) ;

                _device.CloseDevice();
            }
            else
            {
                Console.WriteLine("Could not find reader.");
                // Console.ReadKey();
            }
        }

        private void OnReport(HidReport report)
        {
            if (!_device.IsConnected) { return; }

            var cardData = report.Data[0];

            if (cardData != 0)
            {
                Console.WriteLine(cardData);
            }
            _device.ReadReport(OnReport);
        }

        private void DeviceAttachedHandler()
        {
            Console.WriteLine("Device attached.");
            _device.ReadReport(OnReport);
        }

        private void DeviceRemovedHandler()
        {
            Console.WriteLine("Device removed.");
        }
    }
}
