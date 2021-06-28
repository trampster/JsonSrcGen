using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonNamedPropertyClass
    {
        [JsonName("age")]
        public int Age {get;set;}
        [JsonName("tallness")]
        public int Height {get;set;} 
        [JsonName("Needs\tEscaping")]
        public int Escaping {get;set;}
    }

    public class NamedPropertyTests : NamedPropertyTestsBase
    {
        protected override string ToJson(JsonNamedPropertyClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonNamedPropertyClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8NamedPropertyTests : NamedPropertyTestsBase
    {
        protected override string ToJson(JsonNamedPropertyClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonNamedPropertyClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class NamedPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"age\":42,\"Needs\\tEscaping\":12,\"tallness\":176}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonNamedPropertyClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonNamedPropertyClass()
            {
                Age = 42,
                Height = 176,
                Escaping = 12,
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonNamedPropertyClass value, string json);

        [Test] 
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonNamedPropertyClass();

            //act 
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Escaping, Is.EqualTo(12)); 
        }
    }
}