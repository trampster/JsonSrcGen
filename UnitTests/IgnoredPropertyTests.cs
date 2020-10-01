using NUnit.Framework;
using JsonSrcGen;


namespace UnitTests
{
    [Json]
    public class JsonIgnoredPropertyClass
    {
        public int Age {get;set;}
        public int Height {get;set;} 
        public int Escaping {get;set;}
        [JsonIgnore]
        public int Ignored {get;set;}
    }

    public class IgnoredPropertyTests
    {
        JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42,\"Escaping\":12,\"Height\":176}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter(); 
        }

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonIgnoredPropertyClass()
            {
                Age = 42,
                Height = 176,
                Escaping = 12,
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
            var jsonClass = new JsonIgnoredPropertyClass();

            //act 
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Escaping, Is.EqualTo(12));
        }
    }
}