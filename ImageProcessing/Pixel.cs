using System;
namespace ImageProcessing
{
    public struct Pixel
    {

        private int r;
        private int g;
        private int b;
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

        public Pixel(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public override string ToString()
        {
            return R + " " + G + " " + B;
        }

        public Pixel ToGrey()
        {
            return new Pixel((R+G+B)/3, (R + G + B) / 3, (R + G + B) / 3);
        }

        public Pixel ToBlackOrWhite()
        {
            int value = ((R + G + B) / 3) <= 127 ? 0 : 255;
            return new Pixel(value, value, value);
        }
    }
}
