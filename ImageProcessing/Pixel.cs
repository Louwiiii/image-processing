using System;
namespace ImageProcessing
{
    public class Pixel
    {

        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public Pixel(int r, int g, int b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
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
