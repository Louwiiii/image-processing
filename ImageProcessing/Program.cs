using System;
using System.Drawing;

namespace ImageProcessing
{
    class Program
    {
        static void Main(string[] args)
        {

            MyImage img = new MyImage("Images/coco.bmp");

            img.Blur().FromImageToFile("Images/cocoflou.bmp");
            img.BorderDetection().FromImageToFile("Images/cocoborder.bmp");

            img.RandomKernel().FromImageToFile("Images/randomkernel.bmp");

            img.Repoussage().FromImageToFile("Images/cocorepoussage.bmp");


            int i = 30;
            img.Rotation(i).FromImageToFile("Images/cocoRotated" + i + ".bmp");

            MyImage img3 = new MyImage("Images/cocoRotated30.bmp");

            img3.FromImageToFile("Images/cocoRotatedTest.bmp");
            
            

            img.EffetMiroir(0).FromImageToFile("Images/coco22.bmp");

            img.ToBlackAndWhite().FromImageToFile("Images/cocoB&W.bmp");

            MyImage img2 = img.Resized(0.3f);
            img2.FromImageToFile("Images/cocoResized.bmp");
            img.Rotation(90).FromImageToFile("Images/coco24.bmp");
            img.Rotation(90).FromImageToFile("Images/coco23.bmp");
            //MyImage img1 = new MyImage("Images/coco23.bmp");*/
            int[,] mat = new int[,] {{2,1,3,0},{1,1,0,5},{3,3,1,0},{2,0,0,2}};
            float[,] ker = new float[,] {{1f,0f,2f},{2f,1f,0f},{1f,0f,3f}};
            int[,] matconv = MyImage.Convolution(mat, ker);
            

            Console.ReadKey();
        }
    }
}
