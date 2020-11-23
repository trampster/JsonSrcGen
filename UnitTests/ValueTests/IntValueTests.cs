using NUnit.Framework;
using JsonSrcGen;

[assembly: JsonValue(typeof(int))] 

namespace UnitTests
{
    public class IntValueTests
    {
        JsonSrcGen.JsonConverter _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [TestCase(1, "1")]
        [TestCase(int.MaxValue, "2147483647")]
        [TestCase(int.MinValue, "-2147483648")]
        public void ToJson_CorrectString(int value, string expectedJson)
        {
            //arrange
            //act
            var json = JsonConverter.ToJson(value);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(expectedJson));
        } 

        [TestCase(1, "1")]
        [TestCase(int.MaxValue, "2147483647")]
        [TestCase(int.MinValue, "-2147483648")]
        public void FromJson_CorrectJsonClass(int expectedValue, string json)
        {
            //arrange
            int value = 0;
            //act
            value = JsonConverter.FromJson(value, json); 

            //assert
            Assert.That(value, Is.EqualTo(expectedValue));
        }
    }
}