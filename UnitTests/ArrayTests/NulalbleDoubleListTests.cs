using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System;

[assembly: JsonArray(typeof(double?))] 

namespace UnitTests.ArrayTests
{
    public class NullableDoubleListTests
    { 
        JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[42.21,176.568,1.7976931348623157E+308,-1.7976931348623157E+308,null,0]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var array = new double?[]{42.21d, 176.568d, double.MaxValue, double.MinValue, null, 0};

            //act
            var json = _convert.ToJson(array);

            //assert
            Assert.That(json, Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            //act
            var json = _convert.ToJson((double?[])null);

            //assert
            Assert.That(json, Is.EqualTo("null"));
        }

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var array = new double?[]{};

            //act
            array = _convert.FromJson(array, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(6));
            Assert.That(array[0], Is.EqualTo(42.21d));
            Assert.That(array[1], Is.EqualTo(176.568d));
            Assert.That(array[2], Is.EqualTo(double.MaxValue));
            Assert.That(array[3], Is.EqualTo(double.MinValue));
            Assert.That(array[4], Is.Null);
            Assert.That(array[5], Is.EqualTo(0));
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var array = new double?[]{1, 2, 3};

            //act
            array =_convert.FromJson(array, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(6));
            Assert.That(array[0], Is.EqualTo(42.21d));
            Assert.That(array[1], Is.EqualTo(176.568d));
            Assert.That(array[2], Is.EqualTo(double.MaxValue));
            Assert.That(array[3], Is.EqualTo(double.MinValue));
            Assert.That(array[4], Is.Null);
            Assert.That(array[5], Is.EqualTo(0));
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var array = new double?[]{1, 2, 3};

            //act
            array = _convert.FromJson(array, "null");

            //assert
            Assert.That(array, Is.Null);
        }

        [Test]
        public void FromJson_ListNull_MakesList()
        {
            //arrange
            //act
            var array = _convert.FromJson((double?[])null, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(6));
            Assert.That(array[0], Is.EqualTo(42.21d));
            Assert.That(array[1], Is.EqualTo(176.568d));
            Assert.That(array[2], Is.EqualTo(double.MaxValue));
            Assert.That(array[3], Is.EqualTo(double.MinValue));
            Assert.That(array[4], Is.Null);
            Assert.That(array[5], Is.EqualTo(0));
        }
    }
}