using System;
using System.Linq;
namespace ImageProcessing
{
    public static class QRCode
    {
        public static MyImage Generate (string content)
        {
            content = content.ToUpper();
            if (!content.All(x => char.IsLetterOrDigit(x) || " $%*+-./:".Contains(x)))
                throw new Exception("The entered text is not alpha-numeric");

            byte mode = 0b0010;
            char correctionLevel = 'L';
            int version = 0;

            switch (correctionLevel)
            {
                case 'L':
                default:
                    if (content.Length <= 25)
                        version = 1;
                    else if (content.Length <= 47)
                        version = 2;
                    break;
            }

            string contentLength = Convert.ToString(content.Length, 2).PadLeft(9, '0');

            MyImage result = new MyImage(21, 21);


            return null;
        }
    }
}
