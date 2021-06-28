using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonNullableULongClass
    {
        public ulong? Age {get;set;}
        public ulong? Height {get;set;}
        public ulong? Min {get;set;}
        public ulong? Max {get;set;}
        public ulong? Null {get;set;}
    }

    public class NullableULongPropertyTests : NullableULongPropertyTestsBase
    {
        protected override string ToJson(JsonNullableULongClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonNullableULongClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8NullableULongPropertyTests : NullableULongPropertyTestsBase
    {
        protected override string ToJson(JsonNullableULongClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonNullableULongClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class NullableULongPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42,\"Height\":176,\"Max\":18446744073709551615,\"Min\":0,\"Null\":null}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonNullableULongClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonNullableULongClass()
            {
                Age = 42,
                Height = 176,
                Max = ulong.MaxValue,
                Min = ulong.MinValue,
                Null = null
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonNullableULongClass value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonNullableULongClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Min, Is.EqualTo(ulong.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(ulong.MaxValue));
            Assert.That(jsonClass.Null, Is.Null);
        }
    }
}