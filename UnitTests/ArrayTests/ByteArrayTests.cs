using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System;

[assembly: JsonArray(typeof(byte))] 

namespace UnitTests.ArrayTests
{
    public class ByteArrayTests
    { 
        JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[0,1,255]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var list = new byte[]{0, 1, 255};

            //act
            var json = _convert.ToJson(list);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        } 

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            //act
            var json = _convert.ToJson((byte[])null);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("null"));
        }

        [Test]
        public void FromJson_EmptyArray_CorrectArray()
        {
            //arrange
            var array = new byte[]{};

            //act
            array = _convert.FromJson(array, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(3));
            Assert.That(array[0], Is.EqualTo(0));
            Assert.That(array[1], Is.EqualTo(1));
            Assert.That(array[2], Is.EqualTo(255));
        }

        [Test] 
        public void FromJson_PopulatedArrayCorrectLength_ReusesArray()
        {
            //arrange
            var array = new byte[]{3, 2, 1};

            //act
            _convert.FromJson(array, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(3));
            Assert.That(array[0], Is.EqualTo(0));
            Assert.That(array[1], Is.EqualTo(1));
            Assert.That(array[2], Is.EqualTo(255));
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var array = new byte[]{42, 42, 42};

            //act
            array = _convert.FromJson(array, "null");

            //assert
            Assert.That(array, Is.Null);
        }

        [Test]
        public void FromJson_ArrayNull_MakesArray()
        {
            //arrange
            //act
            var array = _convert.FromJson((byte[])null, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(3));
            Assert.That(array[0], Is.EqualTo(0));
            Assert.That(array[1], Is.EqualTo(1));
            Assert.That(array[2], Is.EqualTo(255));
        }

        [Test]
        public void FromJson_EmptyListJson_EmptyArray()
        {
            //arrange
            //act
            var array = _convert.FromJson((byte[])null, "[]");

            //assert
            Assert.That(array.Length, Is.EqualTo(0));
        }
    }
}