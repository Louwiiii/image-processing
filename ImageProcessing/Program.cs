using System;
using System.IO;
using SixLabors.ImageSharp;

namespace ImageProcessing
{
    public class Program
    {
        static void Main(string[] args)
        {

            //MyImage qr = new QRCode("HELLO WORLD. HELLO WORLD. HELLO WORLD. HELLO WORLD.");
            //qr.Resized(10).FromImageToFile("Images/qrcode.bmp");

            MyImage test = new MyImage("Images/Test003.bmp");
            test.FromImageToFile("Images/test bugged.bmp");

            MyImage read_qr = new MyImage("Images/coco qrcode.bmp");
            Console.WriteLine(QRCode.Read(read_qr));

            Console.ReadKey();
        }
    }
}
