using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System;

[assembly: JsonArray(typeof(bool))]

namespace UnitTests.ListTests
{
    public class BooleanArrayTests
    { 
        JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[true,false]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var list = new bool[]{true, false};

            //act
            var json = JsonConverter.ToJson(list);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        } 

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            //act
            var json = JsonConverter.ToJson((List<bool>)null);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("null"));
        }

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var array = new bool[0];

            //act
            array = JsonConverter.FromJson(array, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(2));
            Assert.That(array[0], Is.True);
            Assert.That(array[1], Is.False);
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var array = new bool[]{false, false, false};

            //act
            array =JsonConverter.FromJson(array, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(2));
            Assert.That(array[0], Is.True);
            Assert.That(array[1], Is.False);
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull() 
        {
            //arrange
            var array = new bool[]{false, false, false};

            //act
            array = JsonConverter.FromJson(array, "null");

            //assert
            Assert.That(array, Is.Null); 
        }

        [Test]
        public void FromJson_ListNull_MakesList()
        {
            //arrange
            //act
            var array = JsonConverter.FromJson((bool[])null, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(2));
            Assert.That(array[0], Is.True);
            Assert.That(array[1], Is.False);
        }
    }
}