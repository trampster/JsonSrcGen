using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonNullableIntClass
    {
        public int? Age {get;set;}
        public int? Height {get;set;}
        public int? Min {get;set;}
        public int? Max {get;set;}
        public int? Zero {get;set;}
        public int? Null {get;set;}
    }

    public class NullableIntPropertyTests : NullableIntPropertyTestsBase
    {
        protected override string ToJson(JsonNullableIntClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonNullableIntClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8NullableIntPropertyTests : NullableIntPropertyTestsBase
    {
        protected override string ToJson(JsonNullableIntClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonNullableIntClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class NullableIntPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42,\"Height\":176,\"Max\":2147483647,\"Min\":-2147483648,\"Null\":null,\"Zero\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }
        protected abstract string ToJson(JsonNullableIntClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonNullableIntClass()
            {
                Age = 42,
                Height = 176,
                Max = int.MaxValue,
                Min = int.MinValue,
                Zero = 0,
                Null = null
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonNullableIntClass value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonNullableIntClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Min, Is.EqualTo(int.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(int.MaxValue));
            Assert.That(jsonClass.Zero, Is.EqualTo(0));
            Assert.That(jsonClass.Null, Is.Null);
        }
    }
}