using System;
using System.Drawing;

namespace ImageProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            MyImage img = new MyImage("Images/coco.bmp");
            img.EffetMiroir(0).FromImageToFile("Images/coco22.bmp");

            MyImage img2 = img.Resized(0.3f);
            img2.FromImageToFile("Images/cocoResized.bmp");
            img.Rotation(90).FromImageToFile("Images/coco24.bmp");
            //MyImage img1 = new MyImage("Images/coco23.bmp");
            
            Console.ReadKey();



        }
    }
}
