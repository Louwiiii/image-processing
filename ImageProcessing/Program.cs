using System;
using System.IO;
using SixLabors.ImageSharp;

namespace ImageProcessing
{
    public class Program
    {
        static void Main(string[] args)
        {

            MyImage qr = new QRCode("HELLO WORLD. HELLO WORLD. HELLO WORLD. HELLO WORLD.");
            qr.Resized(10).FromImageToFile("Images/qrcode.bmp");



            MyImage read_qr = new MyImage("Images/qrcode.bmp");

            Console.WriteLine(QRCode.Read(read_qr));

            Console.ReadKey();
        }
    }
}
