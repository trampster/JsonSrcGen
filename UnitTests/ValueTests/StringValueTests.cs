using NUnit.Framework;
using JsonSrcGen;

[assembly: JsonValue(typeof(string))] 

namespace UnitTests
{
    public class StringValueTests
    {
        JsonSrcGen.JsonConverter _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [TestCase("1", "\"1\"")]
        [TestCase("first", "\"first\"")]
        [TestCase("\"name\"", "\"\\\"name\\\"\"")]
        public void ToJson_CorrectString(string value, string expectedJson)
        {
            //arrange
            //act
            var json = JsonConverter.ToJson(value);

            //asserts
            Assert.That(json.ToString(), Is.EqualTo(expectedJson));
        } 

        [TestCase("1", "\"1\"")]
        [TestCase("first", "\"first\"")]
        [TestCase("\"name\"", "\"\\\"name\\\"\"")]
        public void FromJson_CorrectJsonClass(string expectedValue, string json)
        {
            string? value = (string)null;
            //arrange
            //act
            JsonConverter.FromJson(ref value, json); 

            //assert
            Assert.That(value, Is.EqualTo(expectedValue));
        }
    }
}