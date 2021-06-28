using NUnit.Framework;
using JsonSrcGen;
using System;
using System.Text;

namespace UnitTests
{
    [Json]
    public class JsonGuidClass
    {
        public Guid GuidProperty {get;set;}
    }

    public class GuidPropertyTests : GuidPropertyTestsBase
    {
        protected override string ToJson(JsonGuidClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonGuidClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8GuidPropertyTests : GuidPropertyTestsBase
    {
        protected override string ToJson(JsonGuidClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonGuidClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class GuidPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"GuidProperty\":\"00000001-0002-0003-0405-060708090a0b\"}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonGuidClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonGuidClass()
            {
                GuidProperty = new Guid(1,2,3,4,5,6,7,8,9,10,11),
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson)); 
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonGuidClass value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange 
            var json = ExpectedJson;
            var jsonClass = new JsonGuidClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.GuidProperty, Is.EqualTo(new Guid(1,2,3,4,5,6,7,8,9,10,11)));
        }
    }
}