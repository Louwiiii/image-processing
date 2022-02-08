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

                for (int i = 14; i < 14 + 40; i++)
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
                        this.image[bitmapHeight - i - 1, j] = new Pixel(pixelData[2], pixelData[1], pixelData[0]);
                    }
                }

                // Pixel at [0, 0] is the top left pixel
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }

        }

        /// <summary>
        /// Creates an image from an existing image
        /// </summary>
        /// <param name="image"></param>
        public MyImage(MyImage img)
        {
            this.fileType = img.fileType;
            this.fileSize = img.fileSize;
            this.fileDataOffset = img.fileDataOffset;

            this.dibHeaderSize = img.dibHeaderSize;
            this.bitmapWidth = img.bitmapWidth;
            this.bitmapHeight = img.bitmapHeight;
            this.numberOfColorPlanes = img.numberOfColorPlanes;
            this.numberOfBitsPerPixel = img.numberOfBitsPerPixel;
            this.compressionMethod = img.compressionMethod;
            this.imageSize = img.imageSize;
            this.horizontalResolution = img.horizontalResolution;
            this.verticalResolution = img.verticalResolution;
            this.numberOfColors = img.numberOfColors;
            this.numberOfImportantColors = img.numberOfImportantColors;

            // Image data
            this.image = new Pixel[this.bitmapHeight, this.bitmapWidth];

            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < this.image.GetLength(1); j++)
                {
                    this.image[i, j] = img.image[i, j];
                }
            }
        }

        public void FromImageToFile(string file)
        {
            List<byte> fichier = new List<byte>();
            // File header

            //this.fileType

            fichier.Add((byte)this.fileType[0]);
            fichier.Add((byte)this.fileType[1]);

            //this.fileSize
            fichier.AddRange(ConvertIntToEndian(this.fileSize, 4));

            //ajout de 4 bytes de réserve
            fichier.AddRange(new byte[] { new byte(), new byte(), new byte(), new byte()});

            //this.fileDataOffset
            fichier.AddRange(ConvertIntToEndian(this.fileDataOffset, 4));


            // Image header

            //this.dibHeaderSize 
            fichier.AddRange(ConvertIntToEndian(this.dibHeaderSize, 4));

            //this.bitmapWidth 
            fichier.AddRange(ConvertIntToEndian(this.bitmapWidth, 4));
            //this.bitmapHeight
            fichier.AddRange(ConvertIntToEndian(this.bitmapHeight, 4));

            //this.numberOfColorPlanes 
            fichier.AddRange(ConvertIntToEndian(this.numberOfColorPlanes, 2));
            //this.numberOfBitsPerPixel 
            fichier.AddRange(ConvertIntToEndian(this.numberOfBitsPerPixel, 2));
            //this.compressionMethod
            fichier.AddRange(ConvertIntToEndian(this.compressionMethod, 4));
            //this.imageSize 
            fichier.AddRange(ConvertIntToEndian(this.imageSize, 4));

            //this.horizontalResolution
            fichier.AddRange(ConvertIntToEndian(this.horizontalResolution, 4));
            //this.verticalResolution 
            fichier.AddRange(ConvertIntToEndian(this.verticalResolution, 4));

            //this.numberOfColors
            fichier.AddRange(ConvertIntToEndian(this.numberOfColors, 4));
            //this.numberOfImportantColors 
            fichier.AddRange(ConvertIntToEndian(this.numberOfImportantColors, 4));

            // Image data

            for (int i = bitmapHeight-1; i >= 0; i--)
            {
                for (int j = 0; j < bitmapWidth; j++)
                {
                    byte[] pixelData = { (byte)image[i, j].B, (byte)image[i, j].G, (byte)image[i, j].R };
                    fichier.AddRange(pixelData);
                }
            }

            File.WriteAllBytes(file, fichier.ToArray());
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

        public static byte[] ConvertIntToEndian(int value, uint numberOfBytes)
        {
            List<byte> tab = BitConverter.GetBytes(value).ToList();

            while (tab.Count < numberOfBytes)
            {
                tab.Add(new byte());
            }

            while (tab.Count > numberOfBytes)
            {
                tab.RemoveAt(tab.Count - 1);
            }

            return tab.ToArray();
        }

        public MyImage Clone()
        {
            return new MyImage(this);
        }

        public MyImage ToShadesOfGrey()
        {
            MyImage result = this.Clone();

            for (int i = 0; i < result.image.GetLength(0); i++)
            {
                for (int j = 0; j < result.image.GetLength(1); j++)
                {
                    result.image[i, j] = result.image[i, j].ToGrey();
                }
            }

            return result;
        }


        public MyImage Rotation(int degre)
        {
            MyImage result = this.Clone();

            return result;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction">0: De droite à gauche. 1: De haut en bas.</param>
        /// <returns></returns>
        public MyImage EffetMiroir(int direction)
        {
            MyImage result = this.Clone();

            switch (direction)
            {
                case 0:
                    for (int i = 0; i < result.image.GetLength(0); i++)
                    {
                        for (int j = 0; j < result.image.GetLength(1); j++)
                        {
                            result.image[i, j] = image[i, result.image.GetLength(1) - j - 1];
                        }
                    }
                    break;
                case 1:
                    for (int i = 0; i < result.image.GetLength(0); i++)
                    {
                        for (int j = 0; j < result.image.GetLength(1); j++)
                        {
                            result.image[i, j] = image[result.image.GetLength(0) - i - 1, j];
                        }
                    }
                    break;
                default:
                    throw new Exception("cette direction n'existe pas");
            }
            
            return result; 
        }

        public MyImage Resized(float factor)
        {
            MyImage result = this.Clone();

            result.bitmapWidth = (int)(this.bitmapWidth * factor);
            result.bitmapHeight = (int)(this.bitmapHeight * factor);

            for (int i = 0; i < result.bitmapHeight; i++)
            {
                for (int j = 0; j < result.bitmapWidth; j++)
                {
                    result.image[i, j] = this.image[(int)(i / result.bitmapHeight * this.bitmapHeight), (int)(j / result.bitmapWidth * this.bitmapWidth)];
                }
            }

            result.imageSize = (int)(result.bitmapWidth * result.bitmapHeight * result.numberOfBitsPerPixel / 8);
            result.fileSize = result.fileDataOffset + result.imageSize;

            return result;
        }


    }
}
