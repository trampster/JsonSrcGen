using NUnit.Framework;
using JsonSrcGen;
using System;
using System.Text;

namespace UnitTests
{
    [Json]
    public class JsonNullableGuidClass
    {
        public Guid? GuidProperty {get;set;}
        public Guid? Null {get;set;}
    }

    public class NullableGuidPropertyTests : NullableGuidPropertyTestsBase
    {
        protected override string ToJson(JsonNullableGuidClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonNullableGuidClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8NullableGuidPropertyTests : NullableGuidPropertyTestsBase
    {
        protected override string ToJson(JsonNullableGuidClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonNullableGuidClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class NullableGuidPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"GuidProperty\":\"00000001-0002-0003-0405-060708090a0b\",\"Null\":null}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonNullableGuidClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonNullableGuidClass()
            {
                GuidProperty = new Guid(1,2,3,4,5,6,7,8,9,10,11),
                Null = null
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson)); 
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonNullableGuidClass value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange 
            var json = ExpectedJson;
            var jsonClass = new JsonNullableGuidClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.GuidProperty, Is.EqualTo(new Guid(1,2,3,4,5,6,7,8,9,10,11)));
            Assert.That(jsonClass.Null, Is.Null);
        }
    }
}