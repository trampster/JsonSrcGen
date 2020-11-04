using NUnit.Framework;
using JsonSrcGen;


namespace UnitTests
{
    [Json]
    public class OptionalPropertyClass
    {
        [JsonOptional]
        public bool OptionalBool {get;set;}
    }

    public class OptionalPropertyTests
    {
        JsonSrcGen.JsonConverter _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = "{}";
            var jsonClass = new OptionalPropertyClass() 
            {
                OptionalBool = true
            };

            //act
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.OptionalBool, Is.EqualTo(default(bool)));
        }
    }
}