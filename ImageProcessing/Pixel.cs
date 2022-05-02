using System;
namespace ImageProcessing
{
    /// <summary>
    /// A struct containing the informations about a pixel (Red, Green and Blue colors and Alpha value)
    /// </summary>
    public struct Pixel
    {

        private int r;
        private int g;
        private int b;
        private int a;
        public int R
        {
            get
            {
                return r;
            }
            set
            {
                r = Math.Max(0,Math.Min(255,value));
            }
        }

        public int G
        {
            get
            {
                return g;
            }
            set
            {
                g = Math.Max(0, Math.Min(255, value));
            }
        }

        public int B
        {
            get
            {
                return b;
            }
            set
            {
                b = Math.Max(0, Math.Min(255, value));
            }
        }


        public int A
        {
            get
            {
                return a;
            }
            set
            {
                a = Math.Max(0, Math.Min(255, value));
            }
        }

        /// <summary>
        /// Get the value of the highest color
        /// </summary>
        public int Max
        {
            get
            {
                return Math.Max(Math.Max(R, G), B);
            }
        }

        /// <summary>
        /// Get the value of the lowest color
        /// </summary>
        public int Min
        {
            get
            {
                return Math.Min(Math.Min(R, G), B);
            }
        }

        /// <summary>
        /// Get the average of the color values of a pixel
        /// </summary>
        public double Average
        {
            get { return (R+G+B)/3f; }
        }

        public Pixel(int r, int g, int b, int a = 255)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        /// <summary>
        /// Creates a new pixel from a hex string of the form "#DDEEFF"
        /// </summary>
        /// <param name="hexColor"></param>
        public Pixel(string hexColor)
        {
            hexColor = hexColor.Replace("#", "");

            this.r = Convert.ToInt32(hexColor[0].ToString()+hexColor[1], 16);
            this.g = Convert.ToInt32(hexColor[2].ToString()+hexColor[3], 16);
            this.b = Convert.ToInt32(hexColor[4].ToString()+hexColor[5], 16);
            this.a = 255;
        }

        public static bool operator == (Pixel a, Pixel b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
        }

        public static bool operator != (Pixel a, Pixel b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A; ;
        }


        public override string ToString()
        {
            return R + " " + G + " " + B;
        }

        /// <summary>
        /// Converts a pixel to a gray pixel by averaging its color values
        /// </summary>
        /// <returns></returns>
        public Pixel ToGrey()
        {
            return new Pixel((R + G + B) / 3, (R + G + B) / 3, (R + G + B) / 3);
        }

        /// <summary>
        /// Converts a pixel to either black or white
        /// </summary>
        /// <returns></returns>
        public Pixel ToBlackOrWhite()
        {
            int value = ((R + G + B) / 3) <= 127 ? 0 : 255;
            return new Pixel(value, value, value);
        }
    }
}
