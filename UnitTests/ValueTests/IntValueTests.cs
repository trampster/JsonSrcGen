using NUnit.Framework;
using JsonSrcGen;
using System.Text;

[assembly: JsonValue(typeof(int))] 

namespace UnitTests
{
    public class IntValueTests : IntValueTestsBase
    {
        protected override string ToJson(int json)
        {
            return _convert.ToJson(json).ToString();
        }

        protected override int FromJson(int value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8IntValueTests : IntValueTestsBase
    {
        protected override string ToJson(int json)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(json); 
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override int FromJson(int value, string json)
        {
            return _convert.FromJson(value, Encoding.UTF8.GetBytes(json));
        }
    }

    public abstract class IntValueTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(int json);

        [TestCase(1, "1")]
        [TestCase(int.MaxValue, "2147483647")]
        [TestCase(int.MinValue, "-2147483648")]
        public void ToJson_CorrectString(int value, string expectedJson)
        {
            //arrange
            //act
            var json = ToJson(value);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(expectedJson));
        }

        protected abstract int FromJson(int value, string json);

        [TestCase(1, "1")]
        [TestCase(int.MaxValue, "2147483647")]
        [TestCase(int.MinValue, "-2147483648")]
        public void FromJson_CorrectJsonClass(int expectedValue, string json)
        {
            //arrange
            int value = 0;
            
            //act
            value = FromJson(value, json); 

            //assert
            Assert.That(value, Is.EqualTo(expectedValue));
        }
    }
}