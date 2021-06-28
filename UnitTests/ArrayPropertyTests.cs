using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonArrayClass
    {
        public bool[] BooleanArray {get;set;} 
    }

    public class ArrayPropertyTests : ArrayPropertyTestsBase
    {
        protected override string ToJson(JsonArrayClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonArrayClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8ArrayPropertyTests : ArrayPropertyTestsBase
    {
        protected override string ToJson(JsonArrayClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonArrayClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class ArrayPropertyTestsBase
    { 
        protected JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = $"{{\"BooleanArray\":[true,false]}}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonArrayClass jsonClass);

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonArrayClass()
            {
                BooleanArray = new bool[]{true, false}
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson.ToString()));
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
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"BooleanArray\":null}"));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonArrayClass value, string json);

        [Test]
        public void FromJson_EmptyArray_CorrectArray()
        {
            //arrange
            var jsonClass = new JsonArrayClass()
            {
                BooleanArray = new bool[0]
            };

            //act
            FromJson(jsonClass, ExpectedJson);

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
            FromJson(jsonClass, ExpectedJson);

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
            FromJson(jsonClass, ExpectedJson);

            //assert
            Assert.That(jsonClass.BooleanArray.Length, Is.EqualTo(2));
            Assert.That(jsonClass.BooleanArray[0], Is.True);
            Assert.That(jsonClass.BooleanArray[1], Is.False);
        }
    }
}