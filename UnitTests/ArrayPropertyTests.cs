using NUnit.Framework;
using JsonSGen;
using System.Collections.Generic;


namespace UnitTests
{
    [Json]
    public class JsonArrayClass
    {
        public bool[] BooleanArray {get;set;} 
    }

    public class ArrayPropertyTests
    { 
        JsonSGen.JsonSGenConvert _convert;

        string ExpectedJson = $"{{\"BooleanArray\":[true,false]}}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonSGenConvert();
        }

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonArrayClass()
            {
                BooleanArray = new bool[]{true, false}
            };

            //act
            var json = _convert.ToJson(jsonClass);

            //assert
            Assert.That(json, Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            var jsonClass = new JsonArrayClass()
            {
                BooleanArray = null
            };

            //act
            var json = _convert.ToJson(jsonClass);

            //assert
            Assert.That(json, Is.EqualTo("{\"BooleanArray\":null}"));
        }

        [Test]
        public void FromJson_EmptyArray_CorrectArray()
        {
            //arrange
            var jsonClass = new JsonArrayClass()
            {
                BooleanArray = new bool[0]
            };

            //act
            _convert.FromJson(jsonClass, ExpectedJson);

            //assert
            Assert.That(jsonClass.BooleanArray.Length, Is.EqualTo(2));
            Assert.That(jsonClass.BooleanArray[0], Is.True);
            Assert.That(jsonClass.BooleanArray[1], Is.False);
        }

        [Test]
        public void FromJson_NullArray_CorrectArray()
        {
            //arrange
            var jsonClass = new JsonArrayClass()
            {
                BooleanArray = null
            };

            //act
            _convert.FromJson(jsonClass, ExpectedJson);

            //assert
            Assert.That(jsonClass.BooleanArray.Length, Is.EqualTo(2));
            Assert.That(jsonClass.BooleanArray[0], Is.True);
            Assert.That(jsonClass.BooleanArray[1], Is.False);
        }

        [Test] 
        public void FromJson_PopulatedArray_CorrectArray()
        {
            //arrange
            var jsonClass = new JsonArrayClass()
            {
                BooleanArray = new bool[]{false, false, false}
            };

            //act
            _convert.FromJson(jsonClass, ExpectedJson);

            //assert
            Assert.That(jsonClass.BooleanArray.Length, Is.EqualTo(2));
            Assert.That(jsonClass.BooleanArray[0], Is.True);
            Assert.That(jsonClass.BooleanArray[1], Is.False);
        }
    }
}