using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace ImageProcessing
{
    public class MyImage
    {

        // File header data
        string fileType;
        int fileSize;
        int fileDataOffset;


        // DIB Header data
        int dibHeaderSize;
        int bitmapWidth;
        int bitmapHeight;
        int numberOfColorPlanes;
        int numberOfBitsPerPixel;
        int compressionMethod;
        int imageSize;
        int horizontalResolution;
        int verticalResolution;
        int numberOfColors;
        int numberOfImportantColors;

        // Image data
        Pixel[,] image;


        /// <summary>
        /// Reads .bmp file and creates a MyImage object from it
        /// </summary>
        public MyImage(string filepath)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(filepath);

                Console.WriteLine("\nFile header");

                this.fileType = ((char)bytes[0]).ToString() + ((char)bytes[1]).ToString();

                this.fileSize = ConvertEndianToInt(bytes.Skip(2).Take(4).ToArray());

                this.fileDataOffset = ConvertEndianToInt(bytes.Skip(10).Take(4).ToArray());

                Console.WriteLine(fileType + " " + fileSize + " " + fileDataOffset);

                for (int i = 0; i < 14; i++)
                {
                    Console.Write(bytes[i] + " ");
                }

                Console.WriteLine("\n\nImage header");

                this.dibHeaderSize = ConvertEndianToInt(bytes.Skip(14).Take(4).ToArray());

                this.bitmapWidth = ConvertEndianToInt(bytes.Skip(18).Take(4).ToArray());
                this.bitmapHeight = ConvertEndianToInt(bytes.Skip(22).Take(4).ToArray());

                this.numberOfColorPlanes = ConvertEndianToInt(bytes.Skip(26).Take(2).ToArray());
                this.numberOfBitsPerPixel = ConvertEndianToInt(bytes.Skip(28).Take(2).ToArray());
                this.compressionMethod = ConvertEndianToInt(bytes.Skip(30).Take(4).ToArray());
                this.imageSize = ConvertEndianToInt(bytes.Skip(34).Take(4).ToArray());

                this.horizontalResolution = ConvertEndianToInt(bytes.Skip(38).Take(4).ToArray());
                this.verticalResolution = ConvertEndianToInt(bytes.Skip(42).Take(4).ToArray());

                this.numberOfColors = ConvertEndianToInt(bytes.Skip(46).Take(4).ToArray());
                this.numberOfImportantColors = ConvertEndianToInt(bytes.Skip(50).Take(4).ToArray());

                Console.WriteLine(this.bitmapWidth + " " + this.bitmapHeight + " " + this.numberOfColorPlanes + " " + this.numberOfBitsPerPixel + " " + this.compressionMethod + " " + this.imageSize + " " + this.horizontalResolution
                    + " " + this.verticalResolution + " " + this.numberOfColors + " " + this.numberOfImportantColors);

                for (int i = 14; i < 14+40; i++)
                {
                    Console.Write(bytes[i] + " ");
                }

                Console.WriteLine("\n\nImage data");

                byte[] imageData = bytes.Skip(this.fileDataOffset).Take(this.imageSize).ToArray();
                this.image = new Pixel[bitmapHeight, bitmapWidth];

                for (int i = 0; i < bitmapHeight; i++)
                {
                    for (int j = 0; j < bitmapWidth; j++)
                    {
                        byte[] pixelData = imageData.Skip(3 * (bitmapWidth * i + j)).Take(3).ToArray();
                        this.image[bitmapHeight-i-1, j] = new Pixel(pixelData[2], pixelData[1], pixelData[0]);
                    }
                }

                // Pixel at [0, 0] is the top left pixel
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }

        }

        public void FromImageToFile (string file)
        {
                List<byte> fichier = new List<byte>;
                Console.WriteLine("\nFile header");

                fichier.Add();
                this.fileType = ((char)bytes[0]).ToString() + ((char)bytes[1]).ToString();

                
                this.fileSize = ConvertEndianToInt(bytes.Skip(2).Take(4).ToArray());
            fichier.Add(ConvertIntToIndian())
                this.fileDataOffset = ConvertEndianToInt(bytes.Skip(10).Take(4).ToArray());

                Console.WriteLine(fileType + " " + fileSize + " " + fileDataOffset);

                for (int i = 0; i < 14; i++)
                {
                    Console.Write(bytes[i] + " ");
                }

                Console.WriteLine("\n\nImage header");

                this.dibHeaderSize = ConvertEndianToInt(bytes.Skip(14).Take(4).ToArray());

                this.bitmapWidth = ConvertEndianToInt(bytes.Skip(18).Take(4).ToArray());
                this.bitmapHeight = ConvertEndianToInt(bytes.Skip(22).Take(4).ToArray());

                this.numberOfColorPlanes = ConvertEndianToInt(bytes.Skip(26).Take(2).ToArray());
                this.numberOfBitsPerPixel = ConvertEndianToInt(bytes.Skip(28).Take(2).ToArray());
                this.compressionMethod = ConvertEndianToInt(bytes.Skip(30).Take(4).ToArray());
                this.imageSize = ConvertEndianToInt(bytes.Skip(34).Take(4).ToArray());

                this.horizontalResolution = ConvertEndianToInt(bytes.Skip(38).Take(4).ToArray());
                this.verticalResolution = ConvertEndianToInt(bytes.Skip(42).Take(4).ToArray());

                this.numberOfColors = ConvertEndianToInt(bytes.Skip(46).Take(4).ToArray());
                this.numberOfImportantColors = ConvertEndianToInt(bytes.Skip(50).Take(4).ToArray());

                Console.WriteLine(this.bitmapWidth + " " + this.bitmapHeight + " " + this.numberOfColorPlanes + " " + this.numberOfBitsPerPixel + " " + this.compressionMethod + " " + this.imageSize + " " + this.horizontalResolution
                    + " " + this.verticalResolution + " " + this.numberOfColors + " " + this.numberOfImportantColors);

        }

        public static int ConvertEndianToInt(byte[] tab)
        {
            int result = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                result += (int)tab[i] * (int)Math.Pow(256, i);
            }
            return result;
        }

        public static byte[] ConvertIntToEndian (int value)
        {
            // Find the biggest power of 256 lower or equal than value
            int i = 0;
            while (Math.Pow(256, i+1) < value)
            {
                i += 1;
            }

            byte[] tab = BitConverter.GetBytes(value);

            return tab;
        }

    }
}
