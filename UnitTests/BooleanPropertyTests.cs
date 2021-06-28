using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonBooleanClass
    {
        public bool IsTrue {get;set;}
        public bool IsFalse {get;set;}
    }

    public class BooleanPropertyTests : BooleanPropertyTestsBase
    {
        protected override string ToJson(JsonBooleanClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonBooleanClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8BooleanPropertyTests : BooleanPropertyTestsBase
    {
        protected override string ToJson(JsonBooleanClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonBooleanClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class BooleanPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonBooleanClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonBooleanClass()
            {
                IsTrue = true,
                IsFalse = false
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"IsFalse\":false,\"IsTrue\":true}"));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonBooleanClass value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = "{\"IsFalse\":false,\"IsTrue\":true}";
            var jsonClass = new JsonBooleanClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.IsTrue, Is.True);
            Assert.That(jsonClass.IsFalse, Is.False);
        }
    }
}