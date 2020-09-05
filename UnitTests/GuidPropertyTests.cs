using NUnit.Framework;
using JsonSGen;
using System;


namespace UnitTests
{
    [Json]
    public class JsonGuidClass
    {
        public Guid GuidProperty {get;set;}
    }

    public class GuidPropertyTests
    {
        JsonSGen.JsonSGenConvert _convert;
        const string ExpectedJson = "{\"GuidProperty\":\"00000001-0002-0003-0405-060708090a0b\"}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonSGenConvert();
        }

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
            Assert.That(json, Is.EqualTo(ExpectedJson)); 
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