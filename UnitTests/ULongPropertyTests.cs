using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonULongClass
    {
        public ulong Age {get;set;}
        public ulong Height {get;set;}
        public ulong Min {get;set;}
        public ulong Max {get;set;}
    }

    public class ULongPropertyTests : ULongPropertyTestsBase
    {
        protected override string ToJson(JsonULongClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonULongClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8ULongPropertyTests : ULongPropertyTestsBase
    {
        protected override string ToJson(JsonULongClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonULongClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class ULongPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42,\"Height\":176,\"Max\":18446744073709551615,\"Min\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }
        protected abstract string ToJson(JsonULongClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonULongClass()
            {
                Age = 42,
                Height = 176,
                Max = ulong.MaxValue,
                Min = ulong.MinValue
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonULongClass value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonULongClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Min, Is.EqualTo(ulong.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(ulong.MaxValue));
        }
    }
}