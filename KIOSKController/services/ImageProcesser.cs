using IronBarCode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIOSKController.services
{
    internal class ImageProcesser
    {
        public static void Generate(string data, string fileName = "qrcode.bmp")
        {
            string tempFile = "temp.bmp";

            QRCodeWriter.CreateQrCode(data, 500, QRCodeWriter.QrErrorCorrectionLevel.Medium, QrVersion: 2).SaveAsWindowsBitmap(tempFile);

            Console.WriteLine($"Generate QR Code with data: {data} as {fileName}");
        }

        public static void ConvertTo1Bpp(string sourceFile, string outFile)
        {
            Bitmap original = (Bitmap)FromFile(sourceFile);

            var rectangle = new Rectangle(0, 0, original.Width, original.Height);
            var bmp1bpp = original.Clone(rectangle, PixelFormat.Format1bppIndexed);

            bmp1bpp.Save(outFile);

            Console.WriteLine($"Convert file to 1 bit depth: {sourceFile} to {outFile}");
        }

        public static Image FromFile(string path)
        {
            var bytes = File.ReadAllBytes(path);
            var ms = new MemoryStream(bytes);
            var img = Image.FromStream(ms);
            return img;
        }
    }
}
