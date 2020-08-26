using NUnit.Framework;
using JsonSG;


namespace UnitTests
{
    [Json]
    public class JsonIntClass
    {
        public int Age {get;set;}
        public int Height {get;set;} 
    }

    public class IntPropertyTests
    {
        JsonSG.JsonSGConvert _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonSGConvert();
        }

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonIntClass()
            {
                Age = 42,
                Height = 176
            };

            //act
            var json = _convert.ToJson(jsonClass);

            //assert
            Assert.That(json, Is.EqualTo("{\"Age\":42,\"Height\":176}"));
        }

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = "{\"Age\":42,\"Height\":176}";
            var jsonClass = new JsonIntClass();

            //act
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
        }
    }
}