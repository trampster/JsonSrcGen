using NUnit.Framework;
using JsonSrcGen;


namespace UnitTests
{
    [Json]
    public class JsonBooleanClass
    {
        public bool IsTrue {get;set;}
        public bool IsFalse {get;set;}
    }

    public class BooleanPropertyTests
    {
        JsonSrcGen.JsonConverter _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonBooleanClass()
            {
                IsTrue = true,
                IsFalse = false
            };

            //act
            var json = _convert.ToJson(jsonClass);

            //assert
            Assert.That(json, Is.EqualTo("{\"IsFalse\":false,\"IsTrue\":true}"));
        }


        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = "{\"IsFalse\":false,\"IsTrue\":true}";
            var jsonClass = new JsonBooleanClass();

            //act
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.IsTrue, Is.True);
            Assert.That(jsonClass.IsFalse, Is.False);
        }
    }
}