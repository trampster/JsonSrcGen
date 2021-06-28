using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonIgnoredPropertyClass
    {
        public int Age {get;set;}
        public int Height {get;set;} 
        public int Escaping {get;set;}
        [JsonIgnore]
        public int Ignored {get;set;}
    }

    public class IgnoredPropertyTests : IgnoredPropertyTestsBase
    {
        protected override string ToJson(JsonIgnoredPropertyClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonIgnoredPropertyClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8IgnoredPropertyTests : IgnoredPropertyTestsBase
    {
        protected override string ToJson(JsonIgnoredPropertyClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonIgnoredPropertyClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class IgnoredPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42,\"Escaping\":12,\"Height\":176}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter(); 
        }

        protected abstract string ToJson(JsonIgnoredPropertyClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonIgnoredPropertyClass()
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

        protected abstract ReadOnlySpan<char> FromJson(JsonIgnoredPropertyClass value, string json);

        [Test] 
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonIgnoredPropertyClass();

            //act 
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Escaping, Is.EqualTo(12));
        }
    }
}