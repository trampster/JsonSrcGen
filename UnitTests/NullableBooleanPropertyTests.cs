using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonNullableBooleanClass
    {
        public bool? IsTrue {get;set;}
        public bool? IsFalse {get;set;}
        public bool? IsNull {get;set;}
    }

    public class NullableBooleanPropertyTests : NullableBooleanPropertyTestsBase
    {
        protected override string ToJson(JsonNullableBooleanClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonNullableBooleanClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8NullableBooleanPropertyTests : NullableBooleanPropertyTestsBase
    {
        protected override string ToJson(JsonNullableBooleanClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonNullableBooleanClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class NullableBooleanPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;

        const string _json = "{\"IsFalse\":false,\"IsNull\":null,\"IsTrue\":true}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonNullableBooleanClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonNullableBooleanClass()
            {
                IsTrue = true,
                IsFalse = false,
                IsNull = null
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(_json));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonNullableBooleanClass value, string json);


        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var jsonClass = new JsonNullableBooleanClass();

            //act
            FromJson(jsonClass, _json);

            //assert
            Assert.That(jsonClass.IsTrue, Is.True);
            Assert.That(jsonClass.IsFalse, Is.False);
            Assert.That(jsonClass.IsNull, Is.Null);
        }
    }
}