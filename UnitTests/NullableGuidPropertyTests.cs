using NUnit.Framework;
using JsonSrcGen;
using System;


namespace UnitTests
{
    [Json]
    public class JsonNullableGuidClass
    {
        public Guid? GuidProperty {get;set;}
        public Guid? Null {get;set;}
    }

    public class NullableGuidPropertyTests
    {
        JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"GuidProperty\":\"00000001-0002-0003-0405-060708090a0b\",\"Null\":null}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

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
            var json = _convert.ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson)); 
        }

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange 
            var json = ExpectedJson;
            var jsonClass = new JsonNullableGuidClass();

            //act
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.GuidProperty, Is.EqualTo(new Guid(1,2,3,4,5,6,7,8,9,10,11)));
            Assert.That(jsonClass.Null, Is.Null);
        }
    }
}