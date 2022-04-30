using System;
using System.IO;
using System.Linq;

namespace ImageProcessing
{
    public class Program
    {
        static void Main(string[] args)
        {
            MyImage temp = new MyImage("images/portrait.bmp");
            temp.FromImageToFile("images/portrait2.bmp");

            Console.ReadKey();
        }
    }
}
