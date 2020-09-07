using NUnit.Framework;
using JsonSGen;


namespace UnitTests
{
    [Json]
    public class JsonNamedPropertyClass
    {
        [JsonName("age")]
        public int Age {get;set;}
        [JsonName("tallness")]
        public int Height {get;set;}
        [JsonName("Needs\tEscaping")]
        public int Escaping {get;set;}
    }

    public class NamedPropertyTests
    {
        JsonSGen.JsonSGenConvert _convert;
        const string ExpectedJson = "{\"age\":42,\"Needs\\tEscaping\":12,\"tallness\":176}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonSGenConvert();
        }

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonNamedPropertyClass()
            {
                Age = 42,
                Height = 176,
                Escaping = 12,
            };

            //act
            var json = _convert.ToJson(jsonClass);

            //assert
            System.Console.WriteLine("JSON: " + new string(json));
            Assert.That(json, Is.EqualTo(ExpectedJson));
        }

        [Test] 
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonNamedPropertyClass();

            //act 
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Escaping, Is.EqualTo(12));
        }
    }
}