using NUnit.Framework;
using JsonSGen;
using System.Collections.Generic;


namespace UnitTests
{
    [Json]
    public class JsonListClass
    {
        public System.Collections.Generic.List<bool> BooleanList {get;set;} 
    }

    public class ListPropertyTests
    {
        JsonSGen.JsonSGenConvert _convert;

        string ExpectedJson = $"{{\"BooleanList\":[true,false]}}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonSGenConvert();
        }

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonListClass()
            {
                BooleanList = new List<bool>(){true, false}
            };

            //act
            var json = _convert.ToJson(jsonClass);

            //assert
            Assert.That(json, Is.EqualTo(ExpectedJson));
        } 
    }
}