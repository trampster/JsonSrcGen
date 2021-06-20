using NUnit.Framework;
using JsonSrcGen;
using System.Text;

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

    public class NamedPropertyTests : NamedPropertyTestsBase
    {
        protected override string ToJson(JsonNamedPropertyClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }
    }

    public class Utf8NamedPropertyTests : NamedPropertyTestsBase
    {
        protected override string ToJson(JsonNamedPropertyClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }
    }

    public abstract class NamedPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"age\":42,\"Needs\\tEscaping\":12,\"tallness\":176}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonNamedPropertyClass jsonClass);

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
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
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