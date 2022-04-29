using System;
using System.IO;
using System.Linq;

namespace ImageProcessing
{
    public class Program
    {
        static void Main(string[] args)
        {

            //MyImage qr = new QRCode("HELLO WORLD. HELLO WORLD. HELLO WORLD. HELLO WORLD.");
            //qr.Resized(10).FromImageToFile("Images/qrcode.bmp");

            //MyImage.Mandelbrot(1000, iterations: 100, centerX : -1f).FromImageToFile("Images/mandelbrot.bmp");

            MyImage coco = new MyImage("Images/coco.bmp");
            MyImage qrcode = new QRCode("CONTENU CACHE").Resized(3);

            qrcode.HideIn(coco).DiscoverImage().Item1.FromImageToFile("Images/discovered.bmp");

            MyImage test = new MyImage("Images/Test003.bmp");
            test.FromImageToFile("Images/test bugged.bmp");

            MyImage read_qr = new MyImage("Images/coco qrcode.bmp");

            Console.ReadKey();
        }
    }
}
