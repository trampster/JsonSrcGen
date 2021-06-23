using NUnit.Framework;
using JsonSrcGen;
using System.Text;

[assembly: JsonValue(typeof(string))] 

namespace UnitTests
{
    public class StringValueTests : StringValueTestsBase
    {
        protected override string ToJson(string json)
        {
            return _convert.ToJson(json).ToString();
        }

        protected override string FromJson(string value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8StringValueTests : StringValueTestsBase
    {
        protected override string ToJson(string json)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(json); 
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override string FromJson(string value, string json)
        {
            return _convert.FromJson(value, Encoding.UTF8.GetBytes(json));
        }
    }

    public abstract class StringValueTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(string json);

        [TestCase("1", "\"1\"")]
        [TestCase("first", "\"first\"")]
        [TestCase("\"name\"", "\"\\\"name\\\"\"")]
        public void ToJson_CorrectString(string value, string expectedJson)
        {
            //arrange
            //act
            var json = ToJson(value);

            //asserts
            Assert.That(json.ToString(), Is.EqualTo(expectedJson));
        }

        protected abstract string FromJson(string value, string json);


        [TestCase("1", "\"1\"")]
        [TestCase("first", "\"first\"")]
        [TestCase("\"name\"", "\"\\\"name\\\"\"")]
        public void FromJson_CorrectJsonClass(string expectedValue, string json)
        {
            //arrange
            //act
            string value = FromJson((string)null, json); 

            //assert
            Assert.That(value, Is.EqualTo(expectedValue));
        }
    }
}