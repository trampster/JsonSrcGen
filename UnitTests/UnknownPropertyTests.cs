using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonUnknownPropertyClass
    {
        public int Age {get;set;}
        public int Height {get;set;}
        public int Size {get;set;}
    }

    public class UnknownPropertyTests : UnknownPropertyTestsBase
    {
        protected override ReadOnlySpan<char> FromJson(JsonUnknownPropertyClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8UnknownPropertyTests : UnknownPropertyTestsBase
    {
        protected override ReadOnlySpan<char> FromJson(JsonUnknownPropertyClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class UnknownPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert; 
        const string ExpectedJson = "{\"Age\":42,\"UnknownOne\":\"adf,adf\",\"Height\":176,\"UnknownList\":{1,2,3},\"Size\":12,\"UnknownClass\":{\"property\":13}}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonUnknownPropertyClass value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonUnknownPropertyClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Size, Is.EqualTo(12)); 
        }
    }
}