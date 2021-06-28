using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonUShortClass
    {
        public ushort Age {get;set;}
        public ushort Height {get;set;}
        public ushort Min {get;set;}
        public ushort Max {get;set;}
    }

    public class UShortPropertyTests : UShortPropertyTestsBase
    {
        protected override string ToJson(JsonUShortClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonUShortClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8UShortPropertyTests : UShortPropertyTestsBase
    {
        protected override string ToJson(JsonUShortClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }
        
        protected override ReadOnlySpan<char> FromJson(JsonUShortClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class UShortPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42,\"Height\":176,\"Max\":65535,\"Min\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonUShortClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonUShortClass()
            {
                Age = 42,
                Height = 176,
                Max = ushort.MaxValue,
                Min = ushort.MinValue
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonUShortClass value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonUShortClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Min, Is.EqualTo(ushort.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(ushort.MaxValue));
        }
    }
}