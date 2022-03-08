using System;
using System.Drawing;

namespace ImageProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            /*MyImage img = new MyImage("Images/coco.bmp");
            img.EffetMiroir(0).FromImageToFile("Images/coco22.bmp");

            img.ToBlackAndWhite().FromImageToFile("Images/cocoB&W.bmp");

            MyImage img2 = img.Resized(0.3f);
            img2.FromImageToFile("Images/cocoResized.bmp");
            img.Rotation(90).FromImageToFile("Images/coco24.bmp");
            img.Rotation(90).FromImageToFile("Images/coco23.bmp");
            //MyImage img1 = new MyImage("Images/coco23.bmp");*/
            int[,] mat = ([[2,1,3,0],[1,1,0,5],[3,3,1,0],[2,0,0,2]]);
            int[,] ker = ([[1,0,2],[2,1,0],[1,0,3]]);
            int[,] matconv = Convolution(mat, ker);
            AfficherMatrice(matconv);

            Console.ReadKey();



        }
    }
}
