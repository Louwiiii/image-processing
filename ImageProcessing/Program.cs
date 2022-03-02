using System;
using System.Drawing;

namespace ImageProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            MyImage img = new MyImage("Images/coco.bmp");

            for (int i = 0; i < 360; i++)
            {
                img.Rotation(i).FromImageToFile("Images/Rotation/cocoRotated" + i + ".bmp");
            }
            

            img.EffetMiroir(0).FromImageToFile("Images/coco22.bmp");

            img.ToBlackAndWhite().FromImageToFile("Images/cocoB&W.bmp");

            MyImage img2 = img.Resized(0.3f);
            img2.FromImageToFile("Images/cocoResized.bmp");
            
            //MyImage img1 = new MyImage("Images/coco23.bmp");
            
            Console.ReadKey();



        }
    }
}
