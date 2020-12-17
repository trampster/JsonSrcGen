using NUnit.Framework;
using System.Text;
using System;
using System.Linq;
using System.Threading;

namespace JsonSrcGen.Runtime.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
   
        }


        [Test]
        public void WriteAscii_CorrectResult()
        {
            // arrange
            byte[] data = new byte[256];
            string input = new string(Enumerable.Range(0, 128).Select(u => (char)u).ToArray());

            // act
            data.WriteAscii(12, input);

            // assert
            var extractedString = Encoding.UTF8.GetString(data.AsSpan(12, input.Length));
            Assert.That(extractedString, Is.EqualTo(input));
        }

        //[Test]
        //public void WriteUTF8_OneByteOnly_CorrectResult()
        //{
        //    // arrange
        //    byte[] data = new byte[256];
        //    string input = new string(Enumerable.Range(0, 128).Select(u => (char)u).ToArray());

        //    // NOTE: Method not exists
        //    // act
        //    //data.WriteUtf8(12, input);

        //    // assert
        //    var extractedString = Encoding.UTF8.GetString(data.AsSpan(12, input.Length));
        //    Assert.That(extractedString, Is.EqualTo(input));
        //}

        //[Test]
        //public void WriteUTF8_TwoBytes_CorrectResult()
        //{
        //    // arrange
        //    byte[] data = new byte[6000];
        //    string input = new string(Enumerable.Range(0x0080, 1920).Select(u => (char)u).ToArray());

        //    // NOTE: Method not exists
        //    // act
        //    //data.WriteUtf8(12, input);

        //    // assert
        //    byte[] expectedData = new byte[6000];
        //    Encoding.UTF8.GetBytes(input, expectedData.AsSpan(12));

        //    Console.WriteLine($"Index 11 {expectedData[11]}, actual {data[11]}");
        //    Console.WriteLine($"Index 12 {expectedData[12]}, actual {data[12]}");

        //    Assert.That(data, Is.EqualTo(expectedData));
        //}

        //[Test]
        //public void WriteUTF8_ThreeBytesFirstRange_CorrectResult()
        //{
        //    // arrange
        //    int bufferSize = ((0xD7FF - 0x0800)*3) + 12;
        //    byte[] data = new byte[bufferSize];
        //    string input = new string(Enumerable.Range(0x0800, 0xD7FF - 0x0800).Select(u => (char)u).ToArray());

        //    // NOTE: Method not exists
        //    // act
        //    //data.WriteUtf8(12, input);

        //    // assert
        //    byte[] expectedData = new byte[bufferSize];
        //    Encoding.UTF8.GetBytes(input, expectedData.AsSpan(12));


        //    Assert.That(data, Is.EqualTo(expectedData));
        //}

        //[Test]
        //public void WriteUTF8_ThreeBytesSecondRange_CorrectResult()
        //{
        //    // arrange
        //    int bufferSize = ((0xFFFF - 0xE000)*3) + 12;
        //    byte[] data = new byte[bufferSize];
        //    string input = new string(Enumerable.Range(0xE000, 0xFFFF - 0xE000).Select(u => (char)u).ToArray());

        //    // NOTE: Method not exists
        //    // act
        //    //data.WriteUtf8(12, input);

        //    // assert
        //    byte[] expectedData = new byte[bufferSize];
        //    Encoding.UTF8.GetBytes(input, expectedData.AsSpan(12));


        //    Assert.That(data, Is.EqualTo(expectedData));
        //}

        //[Test]
        //public void WriteUTF8_FourBytes_CorrectResult()
        //{
        //    // arrange
        //    int numberCharacters = 4;
        //    int bufferSize = (numberCharacters * 4) + 12;
        //    byte[] data = new byte[bufferSize];
        //    string input = "\U00010000\U00012435\U00101234\U0010FFFF";

        //    // NOTE: Method not exists
        //    // act
        //    //data.WriteUtf8(12, input);

        //    // assert
        //    byte[] expectedData = new byte[bufferSize];
        //    Encoding.UTF8.GetBytes(input, expectedData.AsSpan(12));
        //    Assert.That(data, Is.EqualTo(expectedData));
        //}
    }
}