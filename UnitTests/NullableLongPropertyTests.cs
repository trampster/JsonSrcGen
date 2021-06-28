using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonNullableLongClass
    {
        public long? Age {get;set;}
        public long? Height {get;set;}
        public long? Min {get;set;}
        public long? Max {get;set;}
        public long? Zero {get;set;}
        public long? Null {get;set;}
    }

    public class NullableLongPropertyTests : NullableLongPropertyTestsBase
    {
        protected override string ToJson(JsonNullableLongClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonNullableLongClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8NullableLongPropertyTests : NullableLongPropertyTestsBase
    {
        protected override string ToJson(JsonNullableLongClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonNullableLongClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class NullableLongPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42,\"Height\":176,\"Max\":9223372036854775807,\"Min\":-9223372036854775808,\"Null\":null,\"Zero\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonNullableLongClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonNullableLongClass()
            {
                Age = 42,
                Height = 176,
                Max = long.MaxValue,
                Min = long.MinValue,
                Zero = 0,
                Null = null
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonNullableLongClass value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonNullableLongClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Min, Is.EqualTo(long.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(long.MaxValue));
            Assert.That(jsonClass.Zero, Is.EqualTo(0));
            Assert.That(jsonClass.Null, Is.Null);
        }
    }
}