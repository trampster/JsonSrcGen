using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonExtraPropertyClass
    {
        public int Aaa {get;set;}
        public int Aaaa {get;set;} 
        public int Aaaaa {get;set;}
    }

    public class ExtraPropertyTests : ExtraPropertyTestsBase
    {
        protected override string ToJson(JsonExtraPropertyClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonExtraPropertyClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8ExtraPropertyTests : ExtraPropertyTestsBase
    {
        protected override string ToJson(JsonExtraPropertyClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonExtraPropertyClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class ExtraPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        // because the properties are only distinwhichable by length Bbb will match Aaa's
        // property and will have to be checked via a full comparison
        const string ExtraJson = "{\"Aaa\":42,\"Aaaa\":12,\"Aaaaa\":176,\"Bbb\":56}"; 
        const string ExpectedJson = "{\"Aaa\":42,\"Aaaa\":12,\"Aaaaa\":176}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter(); 
        }

        protected abstract string ToJson(JsonExtraPropertyClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonExtraPropertyClass()
            {
                Aaa = 42,
                Aaaa = 12,
                Aaaaa = 176,
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonExtraPropertyClass value, string json);

        [Test] 
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExtraJson;
            var jsonClass = new JsonExtraPropertyClass();

            //act 
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Aaa, Is.EqualTo(42));
            Assert.That(jsonClass.Aaaa, Is.EqualTo(12));
            Assert.That(jsonClass.Aaaaa, Is.EqualTo(176));
        }
    }
}