using NUnit.Framework;
using JsonSGen;
using System.Collections.Generic;
using System;

[assembly: JsonArray(typeof(UnitTests.ArrayTests.CustomClass))] 


namespace UnitTests.ArrayTests
{
    [Json]
    public class CustomClass
    {
        public string Name {get;set;}
    }

    public class CustomClassArrayTests
    { 
        JsonSGen.JsonSGenConvert _convert;

        string ExpectedJson = "[{\"Name\":\"William\"},null,{\"Name\":\"Susen\"}]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonSGenConvert();
        }

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var array = new CustomClass[]{new CustomClass{Name = "William"}, null, new CustomClass(){Name="Susen"}};

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
            var json = _convert.ToJson((CustomClass[])null);

            //assert
            Assert.That(json, Is.EqualTo("null"));
        }

        [Test]
        public void FromJson_EmptyArray_CorrectArray() 
        {
            //arrange
            var array = new CustomClass[]{};

            //act
            array = _convert.FromJson(array, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(3));
            Assert.That(array[0].Name, Is.EqualTo("William"));
            Assert.That(array[1], Is.Null);
            Assert.That(array[2].Name, Is.EqualTo("Susen"));
        }

        [Test] 
        public void FromJson_PopulatedArray_CorrectArray()
        {
            //arrange
            var array = new CustomClass[]{new CustomClass(), new CustomClass(), new CustomClass()};

            //act
            array = _convert.FromJson(array, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(3));
            Assert.That(array[0].Name, Is.EqualTo("William"));
            Assert.That(array[1], Is.Null);
            Assert.That(array[2].Name, Is.EqualTo("Susen"));
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var array = new CustomClass[]{new CustomClass(), new CustomClass(), new CustomClass()};

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
            var array = _convert.FromJson((CustomClass[])null, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(3));
            Assert.That(array[0].Name, Is.EqualTo("William"));
            Assert.That(array[1], Is.Null);
            Assert.That(array[2].Name, Is.EqualTo("Susen"));
        }
    }
}