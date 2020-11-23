using NUnit.Framework;
using JsonSrcGen;


namespace UnitTests
{
    [Json]
    public class JsonNullableBooleanClass
    {
        public bool? IsTrue {get;set;}
        public bool? IsFalse {get;set;}
        public bool? IsNull {get;set;}
    }

    public class NullableBooleanPropertyTests
    {
        JsonSrcGen.JsonConverter _convert;

        const string _json = "{\"IsFalse\":false,\"IsNull\":null,\"IsTrue\":true}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonNullableBooleanClass()
            {
                IsTrue = true,
                IsFalse = false,
                IsNull = null
            };

            //act
            var json =JsonConverter.ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(_json));
        }


        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var jsonClass = new JsonNullableBooleanClass();

            //act
           JsonConverter.FromJson(jsonClass, _json);

            //assert
            Assert.That(jsonClass.IsTrue, Is.True);
            Assert.That(jsonClass.IsFalse, Is.False);
            Assert.That(jsonClass.IsNull, Is.Null);
        }
    }
}