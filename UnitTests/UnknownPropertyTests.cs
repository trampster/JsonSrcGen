using NUnit.Framework;
using JsonSrcGen;


namespace UnitTests
{
    [Json]
    public class JsonUnknownPropertyClass
    {
        public int Age {get;set;}
        public int Height {get;set;}
        public int Size {get;set;}
    }

    public class UnknownPropertyTests
    {
        JsonSrcGen.JsonConverter _convert; 
        const string ExpectedJson = "{\"Age\":42,\"UnknownOne\":\"adf,adf\",\"Height\":176,\"UnknownList\":{1,2,3},\"Size\":12,\"UnknownClass\":{\"property\":13}}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonUnknownPropertyClass();

            //act
           JsonConverter.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Size, Is.EqualTo(12)); 
        }
    }
}