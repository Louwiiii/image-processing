using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace ImageProcessing
{
    public class MyImage
    {

        // File header data

        string fileType;

        /// <summary>
        /// The size of the file in bytes
        /// </summary>
        int fileSize
        {
            get
            {
                return imageSize + fileDataOffset;
            }
        }

        /// <summary>
        /// The offset in bytes where the image's data starts
        /// </summary>
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
        int numberOfBytesPerPixel
        {
            get
            {
                return numberOfBitsPerPixel / 8;
            }
        }

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
        public Pixel[,] image;

        /// <summary>
        /// Number of bytes added to each row of pixel in order to have a multiple of 32 bits
        /// </summary>
        int padding
        {
            get
            {
                return (4 - (numberOfBitsPerPixel * bitmapWidth / 8) % 4) % 4;
            }
        }


        /// <summary>
        /// Reads a .bmp file and creates a MyImage object from it
        /// </summary>
        public MyImage(string filepath)
        {
            byte[] bytes;

            bytes = File.ReadAllBytes(filepath);

            Console.WriteLine("\nReading image: " + filepath);
            Console.WriteLine("File header");

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
            int bitmapHeight = ConvertEndianToInt(bytes.Skip(22).Take(4).ToArray()); // Can be negative : if so, image's rows are stored from top to bottom

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

            for (int i = 14; i < 14 + 40; i++)
            {
                Console.Write(bytes[i] + " ");
            }
            Console.WriteLine();
            
            this.image = new Pixel[Math.Abs(bitmapHeight), bitmapWidth];
            byte[] imageData = bytes.Skip(this.fileDataOffset).Take(this.imageSize).ToArray();

            for (int i = 0; i < Math.Abs(bitmapHeight); i++)
            {
                for (int j = 0; j < bitmapWidth; j++)
                {
                    // Put the pixels in the matrix
                    // By default, rows are ordered from bottom to top, but if bitmapHeight is negative, image's rows are stored from top to bottom
                    // If numberOfBitsPerPixel is 24, a pixel has only BGR values. If numberOfBitsPerPixel is 32, a pixel has BGRA values
                    byte[] pixelData = imageData.Skip(numberOfBytesPerPixel * (bitmapWidth * i + j) + padding * i).Take(numberOfBytesPerPixel).ToArray();
                    this.image[(bitmapHeight >= 0 ? bitmapHeight - 1 - i: i), j] = numberOfBitsPerPixel == 24 ? new Pixel(pixelData[2], pixelData[1], pixelData[0]) : new Pixel(pixelData[2], pixelData[1], pixelData[0], pixelData[3]);
                }
            }
        }

        /// <summary>
        /// Creates an image from an existing image by copying it
        /// </summary>
        /// <param name="image">The image to copy</param>
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

        /// <summary>
        /// Exports the image to a file
        /// </summary>
        /// <param name="filepath">The filepath of the exported image</param>
        public void FromImageToFile(string filepath)
        {
            List<byte> fichier = new List<byte>();
            // File header

            //this.fileType

            fichier.Add((byte)this.fileType[0]);
            fichier.Add((byte)this.fileType[1]);

            //this.fileSize
            fichier.AddRange(ConvertIntToEndian(this.fileSize, 4));

            //ajout de 4 bytes de réserve
            fichier.AddRange(new byte[] { 0, 0, 0, 0 });

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

            for (int i = bitmapHeight - 1; i >= 0; i--)
            {
                for (int j = 0; j < bitmapWidth; j++)
                {
                    byte[] pixelData;
                    if (numberOfBitsPerPixel == 24)
                        pixelData = new byte[] { (byte)image[i, j].B, (byte)image[i, j].G, (byte)image[i, j].R };
                    else if (numberOfBitsPerPixel == 32)
                        pixelData = new byte[] { (byte)image[i, j].B, (byte)image[i, j].G, (byte)image[i, j].R, (byte)image[i, j].A };
                    else
                        throw new Exception("Number of bits per pixel for this image not supported");

                    fichier.AddRange(pixelData);
                }

                for (int k = 0; k < padding; k++)
                {
                    fichier.Add(0);
                }
            }


            File.WriteAllBytes(filepath, fichier.ToArray());
        }

        /// <summary>
        /// Converts a LittleEndian byte array to a signed integer
        /// </summary>
        /// <param name="tab">The byte array</param>
        /// <returns>The corresponding integer</returns>
        public static int ConvertEndianToInt(byte[] tab)
        {
            int result = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                result += (int)tab[i] * (int)Math.Pow(256, i);
            }
            return result;
        }

        /// <summary>
        /// Converts a signed integer to a LittleEndian byte array of a certain number of bytes
        /// </summary>
        /// <param name="value">The integer to convert</param>
        /// <param name="numberOfBytes">The number of bytes of the final byte array</param>
        /// <returns>The converted byte array</returns>
        public static byte[] ConvertIntToEndian(int value, uint numberOfBytes)
        {
            byte[] result = new byte[numberOfBytes];

            for (int i = (int)numberOfBytes - 1; i >= 0; i--)
            {
                result[i] = (byte)(value / Math.Pow(256, i));
                value = (int)(value % Math.Pow(256, i));

            }

            return result;
        }

        /// <summary>
        /// Makes a copy of this MyImage and returns it
        /// </summary>
        /// <returns>The clone of the MyImage</returns>
        public MyImage Clone()
        {
            return new MyImage(this);
        }

        /// <summary>
        /// Gets a copy of the image in shades of grey
        /// </summary>
        /// <returns>The modified image</returns>
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

        /// <summary>
        /// Gets a copy of the image in only black and white pixels
        /// </summary>
        /// <returns>The modified image</returns>
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

        /// <summary>
        /// Calculates the distance between 2 points x and y
        /// </summary>
        /// <param name="x1">Abscissa of point x</param>
        /// <param name="y1">Ordinate of point x</param>
        /// <param name="x2">Abscissa of point y</param>
        /// <param name="y2">Ordinate of point y</param>
        /// <returns>The distance between the points</returns>
        public static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        /// <summary>
        /// Rotates an image from a certain angle. The resulting image will have white corners if needed.
        /// </summary>
        /// <param name="degrees">The angle in degrees of the rotation</param>
        /// <returns>The modified image</returns>
        public MyImage Rotation(double degrees)
        {
            degrees = degrees % 360;
            double angle = degrees * (Math.PI) / 180f; // The rotation angle in radians
            MyImage result = this.Clone();

            // Coordinates of the corner that will be used to calculate the height of the new image
            int height_corner_x;
            int height_corner_y;

            // Coordinates of the corner that will be used to calculate the width of the new image
            int width_corner_x;
            int width_corner_y;

            // New height and width are calculated using coordinates of the corners of the image
            if (degrees <= 90 || (degrees >= 180 && degrees <= 270))
            {
                // Height corner is the top right / bottom left
                height_corner_y = bitmapHeight / 2;
                height_corner_x = bitmapWidth / 2;

                // Width corner is the bottom right / top left
                width_corner_y = -bitmapHeight / 2;
                width_corner_x = bitmapWidth / 2;

            }
            else
            {
                // Height corner is the bottom right / top left
                height_corner_y = -bitmapHeight / 2;
                height_corner_x = bitmapWidth / 2;

                // Width corner is the top right / bottom left
                width_corner_y = bitmapHeight / 2;
                width_corner_x = bitmapWidth / 2;
            }

            // Get angle of the corners before rotation, add angle of rotation and get sinus and cosinus
            int newBitmapHeight = (int)Math.Abs(2 * Distance(height_corner_x, height_corner_y, 0, 0) * Math.Sin(Math.Atan2(height_corner_y, height_corner_x) + angle));
            int newBitmapWidth = (int)Math.Abs(2 * Distance(width_corner_x, width_corner_y, 0, 0) * Math.Cos(Math.Atan2(width_corner_y, width_corner_x) + angle));

            result.image = new Pixel[newBitmapHeight, newBitmapWidth];


            // Cycle through the pixels of the result image and find the corresponding pixel in the old image
            for (int newI = 0; newI < result.bitmapHeight; newI++)
            {
                for (int newJ = 0; newJ < result.bitmapWidth; newJ++)
                {
                    // Center of the image is at x,y = (0, 0)

                    double newX = newJ - (newBitmapWidth / 2);
                    double newY = (newBitmapHeight / 2) - newI;

                    double oldX = (Distance(newX, newY, 0, 0) * Math.Cos(Math.Atan2(newY - 0, newX - 0) - angle));
                    double oldY = (Distance(newX, newY, 0, 0) * Math.Sin(Math.Atan2(newY - 0, newX - 0) - angle));

                    int oldI = (int)((bitmapHeight / 2) - oldY);
                    int oldJ = (int)(oldX + (bitmapWidth / 2));

                    if (oldJ < 0 || oldI < 0 || oldJ >= bitmapWidth || oldI >= bitmapHeight)
                    {
                        result.image[newI, newJ] = new Pixel(255, 255, 255); // White corners
                    }
                    else
                    {
                        result.image[newI, newJ] = image[oldI, oldJ];
                    }
                }

            }
            return result;
        }


        /// <summary>
        /// Inverts the image horizontally
        /// </summary>
        /// <returns>The modified image</returns>
        public MyImage EffetMiroir()
        {
            MyImage result = this.Clone();

            for (int i = 0; i < result.image.GetLength(0); i++)
            {
                for (int j = 0; j < result.image.GetLength(1); j++)
                {
                    result.image[i, j] = image[i, result.image.GetLength(1) - j - 1];
                }
            }

            return result;
        }

        /// <summary>
        /// Resizes the image
        /// </summary>
        /// <param name="factor">The multiplication factor of the resulting image</param>
        /// <returns>The modified image</returns>
        public MyImage Resized(float factor)
        {
            MyImage result = this.Clone();


            result.image = new Pixel[(int)(this.bitmapHeight * factor), (int)(this.bitmapWidth * factor)];

            for (int i = 0; i < result.bitmapHeight; i++)
            {
                for (int j = 0; j < result.bitmapWidth; j++)
                {
                    result.image[i, j] = this.image[(int)(i * (double)this.bitmapHeight / result.bitmapHeight), (int)(j * (double)this.bitmapWidth / result.bitmapWidth)];
                }
            }

            return result;
        }

        /// <summary>
        /// Makes the convolution of the image with a given kernel
        /// </summary>
        /// <param name="kernel"></param>
        /// <returns>The convoluted image</returns>
        public MyImage Convoluted(float[,] kernel)
        {
            MyImage result = this.Clone();

            int[][,] resultChannels = new int[3][,];

            int[][,] channels = GetChannels(); // Separate the color channels

            // Make the convolution of each channel
            for (int i = 0; i < 3; i++)
            {
                resultChannels[i] = Convolution(channels[i], kernel);
            }

            // Change the channels of the image to the new channels
            result.SetChannels(resultChannels);

            return result;
        }

        /// <summary>
        /// Separates the color channels of the image into 3 matrices
        /// </summary>
        /// <returns></returns>
        public int[][,] GetChannels()
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

        /// <summary>
        /// Put the colors of the image from 3 matrices containing the color channels of the image
        /// </summary>
        /// <param name="channels"></param>
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

        /// <summary>
        /// Makes the convolution of a matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="kernel"></param>
        /// <returns>The convoluted matrix</returns>
        public static int[,] Convolution(int[,] matrix, float[,] kernel)
        {
            int[,] result = new int[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    for (int k = 0; k < kernel.GetLength(1); k++)
                    {
                        for (int l = 0; l < kernel.GetLength(0); l++)
                        {
                            if ((i - (kernel.GetLength(0) / 2) + l) >= 0 && (i - (kernel.GetLength(0) / 2) + l) < matrix.GetLength(0) && (j - (kernel.GetLength(1) / 2) + k) >= 0 && (j - (kernel.GetLength(1) / 2) + k) < matrix.GetLength(1))
                            {
                                result[i, j] += (int)(kernel[l, k] * matrix[i - (kernel.GetLength(0) / 2) + l, j - (kernel.GetLength(1) / 2) + k]);
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Uses the convolution to blur the image
        /// </summary>
        /// <returns></returns>
        public MyImage Blur()
        {
            return Convoluted(new float[,] { { 1 / 9f, 1 / 9f, 1 / 9f }, { 1 / 9f, 1 / 9f, 1 / 9f }, { 1 / 9f, 1 / 9f, 1 / 9f } });
        }

        /// <summary>
        /// Uses the convolution to detect the borders of the image
        /// </summary>
        /// <returns></returns>
        public MyImage BorderDetection()
        {
            return Convoluted(new float[,] { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } });
        }

        /// <summary>
        /// Uses the convolution to reinforce the edges of the image
        /// </summary>
        /// <returns></returns>
        public MyImage EdgeReinforcement()
        {
            return Convoluted(new float[,] { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } });
        }

        /// <summary>
        /// Uses the convolution to give an horizontal motion blur effect to the image
        /// </summary>
        /// <returns></returns>
        public MyImage MotionBlur()
        {
            return Convoluted(new float[,] { { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 }, { 1 / 7f, 1 / 7f, 1 / 7f, 1 / 7f, 1 / 7f, 1 / 7f, 1 / 7f }, { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 } });

        }

        /// <summary>
        /// Uses the convolution to emboss the image
        /// </summary>
        /// <returns></returns>
        public MyImage Emboss()
        {
            return Convoluted(new float[,] { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } });
        }

        /// <summary>
        /// Uses the convolution to sharpen the image
        /// </summary>
        /// <returns></returns>
        public MyImage Sharpen()
        {
            return Convoluted(new float[,] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } });
        }

        /// <summary>
        /// Makes an histogram of the image
        /// </summary>
        /// <param name="width">The width of the histogram in pixels</param>
        /// <param name="height">The height of the histogram in pixels</param>
        /// <param name="margin">The size in pixels of the black margin</param>
        /// <returns>The histogram as a MyImage</returns>
        public MyImage Histogram(int width=500, int height=250, int margin = 50)
        {
            MyImage result = new MyImage(width, height);

            int[,] colorValuesCounts = new int[3, 256]; // The count of each value for each color

            // Count the number of pixels that have each color value in the image
            for (int i = 0; i < bitmapHeight; i++)
            {
                for (int j = 0; j < bitmapWidth; j++)
                {
                    colorValuesCounts[0, image[i, j].R]++;
                    colorValuesCounts[1, image[i, j].G]++;
                    colorValuesCounts[2, image[i, j].B]++;
                }
            }

            int maxCount = 0; // The highest peak of the histogram
            // Find maximum of the valueCounts
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    if (colorValuesCounts[i, j] > maxCount)
                        maxCount = colorValuesCounts[i, j];
                }
            }

            for (int j = margin; j < width - margin; j++)
            {
                // Iterate through the 3 color channels
                for (int k = 0; k < 3; k++)
                {
                    int valueCount = colorValuesCounts[k, ((j - margin) * 255) / (width - 2 * margin)]; // The count of this value for this color

                    // Create the peak corresponding to this value for each color
                    for (int i = height - margin - 1; i >= margin; i--)
                    {
                        int rowValue = (height - margin - i) * maxCount / (height - 2 * margin); // The value corresponding to the y of the pixel on the histogram (relative to the scale of the y axis)

                        if (rowValue > valueCount) // The peak is high enough
                        {
                            break;
                        }
                        else
                        {
                            switch (k)
                            {
                                case 0:
                                    result.image[i, j].R = 255;
                                    break;

                                case 1:
                                    result.image[i, j].G = 255;
                                    break;

                                case 2:
                                    result.image[i, j].B = 255;
                                    break;
                            }

                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Create a Mandelbrot fractal
        /// </summary>
        /// <param name="width">The width of the image in pixels</param>
        /// <param name="iterations">Number of iterations before considering that the point is in the set</param>
        /// <param name="centerX">The abscissa of the center of the view box</param>
        /// <param name="centerY">The ordinate of the center of the view box</param>
        /// <param name="rectWidth">The width of the view box</param>
        /// <param name="rectHeight">The height of the view box</param>
        /// <returns></returns>
        public static MyImage Mandelbrot(int width, int iterations = 100, double centerX = 0, double centerY = 0, double rectWidth = 3f, double rectHeight = 2f)
        {
            int height = (int) (rectHeight * width/rectWidth);
            MyImage fract = new MyImage(width, height);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    
                    double x = (j * rectWidth / width) - (rectWidth / 2) + centerX;
                    double y = (rectHeight / 2) - (i * rectHeight / height) + centerY;

                    Complex z = new Complex(0, 0);
                    Complex coordinates = new Complex(x, y);
                    for (int k = 0; k < iterations; k++)
                    {
                        z = (z * z) + coordinates;
                        double moduleZ = Math.Sqrt(z.Real * z.Real + z.Imaginary * z.Imaginary);
                        if (moduleZ > 2)
                        {
                            fract.image[i, j] = new Pixel(255 * (iterations - k)/iterations, 255 * (iterations - k) / iterations, 255);
                            break;
                        }
                    }
                }
            }
            return fract;
        }

        /// <summary>
        /// Hide the image into another
        /// </summary>
        /// <param name="containerImage">The image that will store the hidden image</param>
        /// <returns>The final image</returns>
        public MyImage HideIn(MyImage containerImage)
        {
            MyImage hiddenImage = this;
            MyImage result = containerImage.Clone();

            if (hiddenImage.bitmapWidth > containerImage.bitmapWidth || hiddenImage.bitmapHeight > containerImage.bitmapHeight)
                throw new Exception("The hidden image is too big to be stored in the hiding image");
            
            for (int i = 0; i < hiddenImage.image.GetLength(0); i++)
            {
                for (int j = 0; j < hiddenImage.image.GetLength(1); j++)
                {
                    result.image[i, j].R = Convert.ToInt32(Convert.ToString(((byte)containerImage.image[i, j].R), 2).PadLeft(8, '0').Substring(0, 4) + Convert.ToString(((byte)hiddenImage.image[i, j].R), 2).PadLeft(8, '0').Substring(0, 4), 2);
                    result.image[i, j].G = Convert.ToInt32(Convert.ToString(((byte)containerImage.image[i, j].G), 2).PadLeft(8, '0').Substring(0, 4) + Convert.ToString(((byte)hiddenImage.image[i, j].G), 2).PadLeft(8, '0').Substring(0, 4), 2);
                    result.image[i, j].B = Convert.ToInt32(Convert.ToString(((byte)containerImage.image[i, j].B), 2).PadLeft(8, '0').Substring(0, 4) + Convert.ToString(((byte)hiddenImage.image[i, j].B), 2).PadLeft(8, '0').Substring(0, 4), 2);
                }
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>Hidden image and container image</returns>
        public (MyImage, MyImage) DiscoverImage()
        {
            MyImage imagecachee = this.Clone();
            MyImage imagecachante = this.Clone();
            for (int i = 0; i < imagecachante.image.GetLength(0); i++)
            {
                for (int j = 0; j < imagecachante.image.GetLength(1); j++)
                {
                    imagecachante.image[i, j].R = Convert.ToInt32(Convert.ToString(((byte)this.image[i, j].R), 2).PadLeft(8, '0').Substring(0, 4) + "0000", 2);
                    imagecachante.image[i, j].G = Convert.ToInt32(Convert.ToString(((byte)this.image[i, j].G), 2).PadLeft(8, '0').Substring(0, 4) + "0000", 2);
                    imagecachante.image[i, j].B = Convert.ToInt32(Convert.ToString(((byte)this.image[i, j].B), 2).PadLeft(8, '0').Substring(0, 4) + "0000", 2);

                    imagecachee.image[i, j].R = Convert.ToInt32(Convert.ToString(((byte)this.image[i, j].R), 2).PadLeft(8, '0').Substring(4, 4) + "0000", 2);
                    imagecachee.image[i, j].G = Convert.ToInt32(Convert.ToString(((byte)this.image[i, j].G), 2).PadLeft(8, '0').Substring(4, 4) + "0000", 2);
                    imagecachee.image[i, j].B = Convert.ToInt32(Convert.ToString(((byte)this.image[i, j].B), 2).PadLeft(8, '0').Substring(4, 4) + "0000", 2);
                }
            }
            return (imagecachee, imagecachante);
        }
    }
}
