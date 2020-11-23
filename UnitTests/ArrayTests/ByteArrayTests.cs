//using NUnit.Framework;
//using System.Collections.Generic;
//using System;

//[assembly: JsonSrcGen.JsonArray(typeof(byte))]

//namespace UnitTests.ArrayTests
//{
//    public class ByteArrayTests
//    {
//        // JsonSrcGen.JsonConverter _convert;

//        string ExpectedJson = "[0,1,255]";

//        [SetUp]
//        public void Setup()
//        {
//            // _convert = new JsonConverter();
//        }

//        [Test]
//        public void ToJson_CorrectString()
//        {
//            //arrange
//            var list = new byte[] { 0, 1, 255 };

//            //act
//            var json = JsonSrcGen.JsonConverter.ToJson(ref list);

//            //assert
//            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
//        }

//        [Test]
//        public void ToJson_Null_CorrectString()
//        {
//            var array = (byte[])null;
//            //arrange
//            //act
//            var json = JsonSrcGen.JsonConverter.ToJson(ref array);

//            //assert
//            Assert.That(json.ToString(), Is.EqualTo("null"));
//        }

//        [Test]
//        public void FromJson_EmptyArray_CorrectArray()
//        {
//            //arrange
//            var array = new byte[] { };

//            //act
//            JsonSrcGen.JsonConverter.FromJson(ref array, ExpectedJson);

//            //assert
//            Assert.That(array.Length, Is.EqualTo(3));
//            Assert.That(array[0], Is.EqualTo(0));
//            Assert.That(array[1], Is.EqualTo(1));
//            Assert.That(array[2], Is.EqualTo(255));
//        }

//        [Test]
//        public void FromJson_PopulatedArrayCorrectLength_ReusesArray()
//        {
//            //arrange
//            var array = new byte[] { 3, 2, 1 };

//            //act
//            JsonSrcGen.JsonConverter.FromJson(ref array, ExpectedJson);

//            //assert
//            Assert.That(array.Length, Is.EqualTo(3));
//            Assert.That(array[0], Is.EqualTo(0));
//            Assert.That(array[1], Is.EqualTo(1));
//            Assert.That(array[2], Is.EqualTo(255));
//        }

//        [Test]
//        public void FromJson_JsonNull_ReturnsNull()
//        {
//            //arrange
//            var array = new byte[] { 42, 42, 42 };

//            //act
//            JsonSrcGen.JsonConverter.FromJson(ref array, "null");

//            //assert
//            Assert.That(array, Is.Null);
//        }

//        [Test]
//        public void FromJson_ArrayNull_MakesArray()
//        {
//            var array = (byte[])null;
//            //arrange
//            //act
//            JsonSrcGen.JsonConverter.FromJson(ref array, ExpectedJson);

//            //assert
//            Assert.That(array.Length, Is.EqualTo(3));
//            Assert.That(array[0], Is.EqualTo(0));
//            Assert.That(array[1], Is.EqualTo(1));
//            Assert.That(array[2], Is.EqualTo(255));
//        }

//        [Test]
//        public void FromJson_EmptyListJson_EmptyArray()
//        {
//            var array = (byte[])null;
//            //arrange
//            //act
//            JsonSrcGen.JsonConverter.FromJson(ref array, "[]");

//            //assert
//            Assert.That(array.Length, Is.EqualTo(0));
//        }
//    }
//}