using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessing;

namespace ImageProcessing.Tests
{
    [TestClass()]
    public class MyImageTests
    {
        MyImage img;

        public MyImageTests()
        {
            img = new MyImage("../../../../ImageProcessingInterface/bin/Debug/netcoreapp3.1/images/coco.bmp");
        }

        [TestMethod()]
        public void ConstructorTest()
        {
            Assert.IsTrue(img.image.GetLength(0) == 200 && img.image.GetLength(1) == 320);
        }

        [TestMethod()]
        [DataRow(40, 50)]
        [DataRow(75, 24)]
        [DataRow(29, 157)]
        public void MirrorTest(int i, int j)
        {
            MyImage mirror = img.Mirror();
            Assert.IsTrue(mirror.image[i, j] == img.image[i, img.image.GetLength(1) - 1 - j]);
        }

        [TestMethod()]
        [DataRow(40, 50)]
        [DataRow(75, 24)]
        [DataRow(29, 157)]
        public void BlurTest(int i, int j)
        {
            MyImage blur = img.Blur();

            int redAverage = 0;
            for (int i2 = i - 1; i2 <= i + 1; i2++)
            {
                for (int j2 = j - 1; j2 <= j + 1; j2++)
                {
                    redAverage += img.image[i2, j2].R;
                }
            }

            redAverage /= 9;

            Assert.AreEqual(redAverage, blur.image[i, j].R);
        }

        [TestMethod()]
        [DataRow(0.5f)]
        [DataRow(3f)]
        [DataRow(1.75f)]
        public void ResizedTest(float factor)
        {
            MyImage resized = img.Resized(factor);
            Assert.AreEqual(img.image.GetLength(0)*factor, resized.image.GetLength(0));;
        }
    }
}
