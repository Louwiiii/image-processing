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
            img.Rotation(90).FromImageToFile("Images/coco23.bmp");
            //MyImage img1 = new MyImage("Images/coco23.bmp");
            
            Console.ReadKey();



        }
    }
}
