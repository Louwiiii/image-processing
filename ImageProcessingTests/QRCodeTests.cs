using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessing.Tests
{
    [TestClass()]
    public class QRCodeTests
    {
        [TestMethod()]
        [DataRow("Hello my name is Lucie and I love summer holidays")]
        [DataRow("You still have 3 apples.")]
        [DataRow("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./")]
        public void QRCodeTest(string text)
        {
            QRCode qr = new QRCode(text);
            Assert.AreEqual(text.ToUpper(), qr.Read());
        }

        [TestMethod()]
        [DataRow('H', 17)]
        [DataRow('2', 2)]
        [DataRow(':', 44)]
        [DataRow('A', 10)]
        [DataRow('-', 41)]
        public void EncodeCharTest(char input, int expected)
        {
            Assert.AreEqual(expected, QRCode.EncodeChar(input));
        }

        [TestMethod()]
        [DataRow(1, 25)]
        [DataRow(2, 47)]
        [DataRow(3, 77)]
        [DataRow(4, 114)]
        [DataRow(5, 154)]
        public void GetCharacterCapacityTest(int version, int expected)
        {
            Assert.AreEqual(QRCode.GetCharacterCapacity(version), expected);
        }
    }
}