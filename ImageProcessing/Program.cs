using System;
using System.Drawing;

namespace ImageProcessing
{
    class Program
    {
        static void Main(string[] args)
        {

            MyImage img = new MyImage("Images/coco.bmp");

            int i = 30;
            img.Rotation(i).FromImageToFile("Images/cocoRotated" + i + ".bmp");

            MyImage img3 = new MyImage("Images/cocoRotated30.bmp");

            img3.FromImageToFile("Images/cocoRotatedTest.bmp");
            
            

            img.EffetMiroir(0).FromImageToFile("Images/coco22.bmp");

            img.ToBlackAndWhite().FromImageToFile("Images/cocoB&W.bmp");

            MyImage img2 = img.Resized(0.3f);
            img2.FromImageToFile("Images/cocoResized.bmp");
            
            //MyImage img1 = new MyImage("Images/coco23.bmp");
            
            Console.ReadKey();
        }
    }
}
