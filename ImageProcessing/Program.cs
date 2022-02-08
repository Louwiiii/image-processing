using System;
using System.Drawing;

namespace ImageProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(MyImage.ConvertIntToEndian(256)[2]);

            MyImage img = new MyImage("Images/coco.bmp");
            Console.ReadKey();



        }
    }
}
