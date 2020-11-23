using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System;

[assembly: JsonArray(typeof(string))] 

namespace UnitTests.ArrayTests
{
    public class StringArrayTests
    { 
        string ExpectedJson = "[\"one\",null,\"two\"]";

        [SetUp]
        public void Setup()
        {
           
        }

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var array = new string[]{"one", null, "two"};

            //act
            var json = JsonConverter.ToJson(ref array);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson.ToString()));
        }

        [Test]
        public void ToJson_Null_CorrectString()
        {
            var array = (string[])null;
            //arrange
            //act
            var json = JsonConverter.ToJson(ref array);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("null"));
        }

        [Test]
        public void FromJson_EmptyArray_CorrectArray() 
        {
            //arrange
            var array = new string[]{};

            //act
            JsonConverter.FromJson(ref array, ExpectedJson.ToString());

            //assert
            Assert.That(array.Length, Is.EqualTo(3));
            Assert.That(array[0], Is.EqualTo("one"));
            Assert.That(array[1], Is.Null);
            Assert.That(array[2], Is.EqualTo("two"));
        }

        [Test] 
        public void FromJson_PopulatedArray_CorrectArray()
        {
            //arrange
            var array = new string[]{"asfd", "gggg"};

            //act
            JsonConverter.FromJson(ref array, ExpectedJson.ToString());

            //assert
            Assert.That(array.Length, Is.EqualTo(3));
            Assert.That(array[0], Is.EqualTo("one"));
            Assert.That(array[1], Is.Null);
            Assert.That(array[2], Is.EqualTo("two"));;
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var array = new string[]{"asfd", "gggg"};

            //act
            JsonConverter.FromJson(ref array, "null");

            //assert
            Assert.That(array, Is.Null);
        }

        [Test]
        public void FromJson_ArrayNull_MakesArray()
        {
            var array = (string[])null;
            //arrange
            //act
            JsonConverter.FromJson(ref array, ExpectedJson.ToString());

            //assert
            Assert.That(array.Length, Is.EqualTo(3));
            Assert.That(array[0], Is.EqualTo("one"));
            Assert.That(array[1], Is.Null);
            Assert.That(array[2], Is.EqualTo("two"));
        }
    }
}