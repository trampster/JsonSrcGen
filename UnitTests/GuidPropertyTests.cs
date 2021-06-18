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
    }

    public class Utf8GuidPropertyTests : GuidPropertyTestsBase
    {
        protected override string ToJson(JsonGuidClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
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
            var json = _convert.ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson)); 
        }

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange 
            var json = ExpectedJson;
            var jsonClass = new JsonGuidClass();

            //act
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.GuidProperty, Is.EqualTo(new Guid(1,2,3,4,5,6,7,8,9,10,11)));
        }
    }
}