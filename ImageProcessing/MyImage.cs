using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;

namespace ImageProcessing
{
    public class MyImage
    {

        // File header data
        string fileType;
        int fileSize
        {
            get
            {
                return imageSize + fileDataOffset;
            }
        }
        
        int fileDataOffset;


        // DIB Header data
        int dibHeaderSize;
        int bitmapWidth
        {
            get
            {
                return this.image.GetLength(1);
            }
        }
        int bitmapHeight
        {
            get
            {
                return this.image.GetLength(0);
            }
        }
        int numberOfColorPlanes;
        int numberOfBitsPerPixel;
        int compressionMethod;
        int imageSize
        {
            get
            {
                return (bitmapWidth * bitmapHeight * (numberOfBitsPerPixel / 8)) + (padding * bitmapHeight);
            }
        }
        int horizontalResolution;
        int verticalResolution;
        int numberOfColors;
        int numberOfImportantColors;

        // Image data
        Pixel[,] image;

        // Number of bytes added to each row of pixel
        int padding
        {
            get
            {
                return (4 - (numberOfBitsPerPixel * bitmapWidth / 8) % 4) % 4;
            }
        }


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

                int fileSize = ConvertEndianToInt(bytes.Skip(2).Take(4).ToArray());

                this.fileDataOffset = ConvertEndianToInt(bytes.Skip(10).Take(4).ToArray());

                Console.WriteLine(fileType + " " + fileSize + " " + fileDataOffset);

                for (int i = 0; i < 14; i++)
                {
                    Console.Write(bytes[i] + " ");
                }

                Console.WriteLine("\n\nImage header");

                this.dibHeaderSize = ConvertEndianToInt(bytes.Skip(14).Take(4).ToArray());

                int bitmapWidth = ConvertEndianToInt(bytes.Skip(18).Take(4).ToArray());
                int bitmapHeight = ConvertEndianToInt(bytes.Skip(22).Take(4).ToArray());

                this.numberOfColorPlanes = ConvertEndianToInt(bytes.Skip(26).Take(2).ToArray());
                this.numberOfBitsPerPixel = ConvertEndianToInt(bytes.Skip(28).Take(2).ToArray());
                this.compressionMethod = ConvertEndianToInt(bytes.Skip(30).Take(4).ToArray());

                int imageSize = ConvertEndianToInt(bytes.Skip(34).Take(4).ToArray());

                this.horizontalResolution = ConvertEndianToInt(bytes.Skip(38).Take(4).ToArray());
                this.verticalResolution = ConvertEndianToInt(bytes.Skip(42).Take(4).ToArray());

                this.numberOfColors = ConvertEndianToInt(bytes.Skip(46).Take(4).ToArray());
                this.numberOfImportantColors = ConvertEndianToInt(bytes.Skip(50).Take(4).ToArray());

                Console.WriteLine(this.dibHeaderSize + " " + bitmapWidth + " " + bitmapHeight + " " + this.numberOfColorPlanes + " " + this.numberOfBitsPerPixel + " " + this.compressionMethod + " " + imageSize + " " + this.horizontalResolution
                    + " " + this.verticalResolution + " " + this.numberOfColors + " " + this.numberOfImportantColors);

                Console.WriteLine();

                for (int i = 14; i < 14 + 40; i++)
                {
                    Console.Write(bytes[i] + " ");
                }




                byte[] imageData = bytes.Skip(this.fileDataOffset).Take(imageSize).ToArray();
                this.image = new Pixel[bitmapHeight, bitmapWidth];

                for (int i = 0; i < bitmapHeight; i++)
                {
                    for (int j = 0; j < bitmapWidth; j++)
                    {
                        byte[] pixelData = imageData.Skip(3 * (bitmapWidth * i + j) + padding * i).Take(3).ToArray();
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
            int fileSize = img.fileSize;
            this.fileDataOffset = img.fileDataOffset;

            this.dibHeaderSize = img.dibHeaderSize;
            this.numberOfColorPlanes = img.numberOfColorPlanes;
            this.numberOfBitsPerPixel = img.numberOfBitsPerPixel;
            this.compressionMethod = img.compressionMethod;
            this.horizontalResolution = img.horizontalResolution;
            this.verticalResolution = img.verticalResolution;
            this.numberOfColors = img.numberOfColors;
            this.numberOfImportantColors = img.numberOfImportantColors;

            // Image data
            this.image = new Pixel[img.bitmapHeight, img.bitmapWidth];

            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < this.image.GetLength(1); j++)
                {
                    this.image[i, j] = img.image[i, j];
                }
            }
        }

        /// <summary>
        /// Create a new MyImage from scratch
        /// </summary>
        public MyImage(int width, int height, string fileType = "BM", int fileDataOffset = 54, int dibHeaderSize = 40, int numberOfColorPlanes = 1, int numberOfBitsPerPixel = 24, int compressionMethod = 0, int horizontalResolution = 11811, int verticalResolution = 11811, int numberOfColors = 0, int numberOfImportantColors = 0)
        {
            this.image = new Pixel[height, width];
            this.fileType = fileType;
            this.fileDataOffset = fileDataOffset;
            this.dibHeaderSize = dibHeaderSize;
            this.numberOfColorPlanes = numberOfColorPlanes;
            this.numberOfBitsPerPixel = numberOfBitsPerPixel;
            this.compressionMethod = compressionMethod;
            this.horizontalResolution = horizontalResolution;
            this.verticalResolution = verticalResolution;
            this.numberOfColors = numberOfColors;
            this.numberOfImportantColors = numberOfImportantColors;

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

                for (int k = 0; k < padding; k++)
                {
                    fichier.Add((byte) 0);
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
            byte[] result = new byte[numberOfBytes];

            for (int i = (int) numberOfBytes - 1; i >= 0; i--)
            {

                result[i] = (byte) (value / Math.Pow(256, i));
                value = (int) (value % Math.Pow(256, i));

            }

            //List<byte> tab = BitConverter.GetBytes(value).ToList();

            //while (tab.Count < numberOfBytes)
            //{
            //    tab.Add(new byte());
            //}

            //while (tab.Count > numberOfBytes)
            //{
            //    tab.RemoveAt(tab.Count - 1);
            //}

            return result;
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

        public MyImage ToBlackAndWhite()
        {
            MyImage result = this.Clone();

            for (int i = 0; i < result.image.GetLength(0); i++)
            {
                for (int j = 0; j < result.image.GetLength(1); j++)
                {
                    result.image[i, j] = result.image[i, j].ToBlackOrWhite();
                }
            }

            return result;
        }

        public static double Distance (double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        public MyImage Rotation(int degrees)
        {
            degrees = degrees % 360;
            double angle = degrees * (Math.PI) / 180f;
            MyImage result = this.Clone();

            int height_corner_x = 0;
            int height_corner_y = 0;
            int width_corner_x = 0;
            int width_corner_y = 0;

            // New height and width are calculated using coordinates of the corners of the image
            if (degrees <= 90 || (degrees >= 180 && degrees <= 270))
            {
                // Height corner is the top right / bottom left
                height_corner_y = bitmapHeight / 2;
                height_corner_x = bitmapWidth / 2;

                // Width corner is the bottom right / top left
                width_corner_y = - bitmapHeight / 2;
                width_corner_x = bitmapWidth / 2;

            } else
            {
                // Height corner is the bottom right / top left
                height_corner_y = - bitmapHeight / 2;
                height_corner_x = bitmapWidth / 2;

                // Width corner is the top right / bottom left
                width_corner_y = bitmapHeight / 2;
                width_corner_x = bitmapWidth / 2;
            }

            // Get angle of the corners before rotation, add angle of rotation and get sinus and cosinus
            int newBitmapHeight = (int) Math.Abs(2 * Distance(height_corner_x, height_corner_y, 0, 0) * Math.Sin(Math.Atan2(height_corner_y, height_corner_x) + angle));
            int newBitmapWidth = (int) Math.Abs(2 * Distance(width_corner_x, width_corner_y, 0, 0) * Math.Cos(Math.Atan2(width_corner_y, width_corner_x) + angle));

            result.image = new Pixel[newBitmapHeight, newBitmapWidth];

            double oldX;
            double oldY;

            for (int newI = 0; newI < result.bitmapHeight; newI++)
            {
                for (int newJ = 0; newJ < result.bitmapWidth; newJ++)
                {
                    //x = (int)(Math.Sin(angle) * i + Math.Cos(angle) * j);
                    //y = (int)(Math.Sin(angle) * (this.bitmapWidth - j) + Math.Cos(angle) * i);

                    // Center is in (0, 0)

                    double new_x = newJ - (newBitmapWidth / 2);
                    double new_y = (newBitmapHeight / 2) - newI;

                    oldX = (Distance(new_x, new_y, 0, 0) * Math.Cos(Math.Atan2(new_y - 0, new_x - 0) - angle));
                    oldY = (Distance(new_x, new_y, 0, 0) * Math.Sin(Math.Atan2(new_y - 0, new_x - 0) - angle));

                    int oldI = (int)((bitmapHeight / 2) - oldY);
                    int oldJ = (int)(oldX + (bitmapWidth / 2));

                    if (oldJ < 0 || oldI < 0 || oldJ >= bitmapWidth || oldI >= bitmapHeight)
                    {
                        result.image[newI, newJ] = new Pixel(255, 255, 255);
                    }
                    else
                    {
                        result.image[newI, newJ] = image[oldI, oldJ];
                    }

                    //result.image[(int)(Math.Sin(angle) * (this.bitmapWidth - newJ) + Math.Cos(angle) * newI), (int)(Math.Sin(angle) * newI + Math.Cos(angle) * newJ)] = image[newI, newJ];

                }
                
            }
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


            result.image = new Pixel[(int)(this.bitmapHeight * factor), (int)(this.bitmapWidth * factor)];

            for (int i = 0; i < result.bitmapHeight; i++)
            {
                for (int j = 0; j < result.bitmapWidth; j++)
                {
                    result.image[i, j] = this.image[(int)(i * 1.0f / result.bitmapHeight * this.bitmapHeight), (int)(j * 1.0f / result.bitmapWidth * this.bitmapWidth)];
                }
            }

            return result;
        }

        public MyImage Convoluted (float[,] kernel)
        {
            MyImage result = this.Clone();

            int[][,] resultChannels = new int[3][,];

            int[][,] channels = GetChannels();

            for (int i = 0; i < 3; i++)
            {
                resultChannels[i] = Convolution(channels[i], kernel) ;
            }

            result.SetChannels(resultChannels);

            return result;
        }

        public int[][,] GetChannels ()
        {
            int[][,] result = new int[3][,];

            for (int i = 0; i < 3; i++)
            {
                result[i] = new int[this.bitmapHeight, this.bitmapWidth];
            }

            for (int i = 0; i < this.bitmapHeight; i++)
            {
                for (int j = 0; j < this.bitmapWidth; j++)
                {
                    result[0][i, j] = this.image[i, j].R;
                    result[1][i, j] = this.image[i, j].G;
                    result[2][i, j] = this.image[i, j].B;
                }
            }

            return result;
        }

        public void SetChannels(int[][,] channels)
        {
            for (int i = 0; i < this.bitmapHeight; i++)
            {
                for (int j = 0; j < this.bitmapWidth; j++)
                {
                    this.image[i, j].R = channels[0][i, j];
                    this.image[i, j].G = channels[1][i, j];
                    this.image[i, j].B = channels[2][i, j];
                }
            }
        }

        public static int[,] Convolution (int[,] matrix, float[,] kernel)
        {
            int[,] result = new int[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i=0; i < matrix.GetLength(0); i++)
            {
                for (int j=0; j< matrix.GetLength(1); j++)
                {
                    for (int k=0; k < kernel.GetLength(1); k++)
                    {
                        for (int l=0; l< kernel.GetLength(0); l++)
                        {
                            if ((i - (kernel.GetLength(0)/2) + l) >= 0 && (i - (kernel.GetLength(0) / 2) + l) < matrix.GetLength(0) && (j - (kernel.GetLength(1) / 2) + k) >= 0 && (j - (kernel.GetLength(1) / 2) + k) < matrix.GetLength(1))
                            {
                                result[i, j] += (int)(kernel[l, k] * matrix[i - (kernel.GetLength(0) / 2) + l, j - (kernel.GetLength(1) / 2) + k]);
                            }
                        }
                    }
                }
            }
            return result;
        }

        public MyImage Blur ()
        {
            return Convoluted(new float[,] { { 1 / 9f, 1 / 9f, 1 / 9f }, { 1 / 9f, 1 / 9f, 1 / 9f  },{ 1 / 9f, 1 / 9f, 1 / 9f  } });
        }

        public MyImage BorderDetection()
        {
            return Convoluted(new float[,] { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } });
        }


        public MyImage EdgeReinforcement()
        {
            return Convoluted(new float[,] { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } });
        }

        public MyImage RandomKernel()
        {
            return Convoluted(new float[,] { { 1, 2, 3 }, { -8, -10, -12 }, { 7, 8, 9 } });
            //return Convoluted(new float[,] { { 68, -68, 68 }, { -68, 0, -68 }, { 68, -68, 68 } });
        }

        public MyImage MotionBlur()
        {
            //return Convoluted(new float[,] { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 1/5f, 1/5f, 1/5f, 1/5f, 1/5f }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } });
            return Convoluted(new float[,] { { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 }, { 1 / 7f, 1 / 7f, 1 / 7f, 1 / 7f, 1 / 7f, 1 / 7f, 1 / 7f }, { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 } });

        }

        public MyImage Repoussage()
        {
            return Convoluted(new float[,] { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } });
        }

        public MyImage Histogram()
        {
            MyImage result = new MyImage(256 + 100, 100);
            return null;
        }

        public MyImage Fractale()
        {
            MyImage fract = new MyImage();
            Complex coordonnees = new Complex(,);
            Complex z0 = new Complex(fract.image.GetLength(0)/2,fract.image.GetLength(1)/2);
            for (int i=0 ; i < fract.image.GetLength(0); i++)
            {
                for (int j=0; j < fract.image.GetLength(1); j++)
                {
        


                }
            }
        }

    }
}
