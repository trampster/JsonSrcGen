using NUnit.Framework;
using JsonSrcGen;
using System.Collections;

namespace UnitTests
{
    [Json]
    public class JsonCharClass
    {
        public char Property {get;set;}
    }

    public class CharTestCaseData
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData('a', "\"a\"");
                yield return new TestCaseData('\b', "\"\\b\"");
                yield return new TestCaseData('\t', "\"\\t\"");
                yield return new TestCaseData('\n', "\"\\n\"");
                yield return new TestCaseData('\r', "\"\\r\"");
                yield return new TestCaseData('\f', "\"\\f\"");
                yield return new TestCaseData('\\', "\"\\\\\"");
                yield return new TestCaseData('/', "\"\\/\"");
                yield return new TestCaseData('/', "\"\\/\"");
            }
        }
    }

    public class CharPropertyTests
    {
        JsonSrcGen.JsonConverter _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test, TestCaseSource(typeof(CharTestCaseData), "TestCases")]
        public void ToJson_CorrectString(char character, string expectedPropertyValue)
        {
            //arrange
            var jsonClass = new JsonCharClass()
            {
                Property = character
            };

            //act
            var json = _convert.ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo($"{{\"Property\":{expectedPropertyValue}}}"));
        } 

        [Test, TestCaseSource(typeof(CharTestCaseData), "TestCases")]
        public void FromJson_CorrectJsonClass(char expectedCharacter, string property)
        {
            //arrange
            var json = $"{{\"Property\":{property}}}";
            var jsonClass = new JsonCharClass();

            //act
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Property, Is.EqualTo(expectedCharacter));
        }

        [Test]
        public void FromJson_EscapedUnicode_CorrectJsonClass()
        {
            //arrange
            var json = "{\"Property\":\"\\u1F4A\"}";
            var jsonClass = new JsonCharClass();

            //act
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Property, Is.EqualTo('\u1F4A'));
        }
    }
}