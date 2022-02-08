using System;
using System.Drawing;

namespace ImageProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(MyImage.ConvertIntToEndian(256, 4)[0]);

            MyImage img = new MyImage("Images/coco.bmp");
            img.EffetMiroir(0).FromImageToFile("Images/coco22.bmp");
            MyImage img1 = new MyImage("Images/coco22.bmp");
            
            Console.ReadKey();



        }
    }
}
