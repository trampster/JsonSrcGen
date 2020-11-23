using NUnit.Framework;
using JsonSrcGen;


namespace UnitTests
{
    [Json]
    public class WhiteSpaceJsonClass
    {
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string NullProperty {get;set;}
    }

    public class WhiteSpaceTests
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
            var json = "{\r\n\"FirstName\": \"Bob\",\r\n\t\"LastName\":\r   \"Marley\",\n\"NullProperty\": null\n}";
            var jsonClass = new JsonClass();

            //act
           JsonConverter.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.FirstName, Is.EqualTo("Bob"));
            Assert.That(jsonClass.LastName, Is.EqualTo("Marley"));
            Assert.That(jsonClass.NullProperty, Is.EqualTo(null));
        }
    }
}