using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonNullableByteClass
    {
        public byte? Age {get;set;}
        public byte? Height {get;set;}
        public byte? Min {get;set;}
        public byte? Max {get;set;}
        public byte? Null {get;set;}
    }

    public class NullableBytePropertyTests : NullableBytePropertyTestsBase
    {
        protected override string ToJson(JsonNullableByteClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonNullableByteClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8NullableBytePropertyTests : NullableBytePropertyTestsBase
    {
        protected override string ToJson(JsonNullableByteClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonNullableByteClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class NullableBytePropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42,\"Height\":176,\"Max\":255,\"Min\":0,\"Null\":null}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonNullableByteClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonNullableByteClass()
            {
                Age = 42,
                Height = 176,
                Max = byte.MaxValue,
                Min = byte.MinValue,
                Null = null
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonNullableByteClass value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonNullableByteClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Min, Is.EqualTo(byte.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(byte.MaxValue));
            Assert.That(jsonClass.Null, Is.Null);
        }
    }
}