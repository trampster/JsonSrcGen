using NUnit.Framework;
using JsonSrcGen;
using System.Collections;
using System.Text;
using System;

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

    public class CharPropertyTests : CharPropertyTestsBase
    {
        protected override string ToJson(JsonCharClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonCharClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8CharPropertyTests : CharPropertyTestsBase
    {
        protected override string ToJson(JsonCharClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonCharClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class CharPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonCharClass jsonClass);


        [Test, TestCaseSource(typeof(CharTestCaseData), "TestCases")]
        public void ToJson_CorrectString(char character, string expectedPropertyValue)
        {
            //arrange
            var jsonClass = new JsonCharClass()
            {
                Property = character
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo($"{{\"Property\":{expectedPropertyValue}}}"));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonCharClass value, string json);


        [Test, TestCaseSource(typeof(CharTestCaseData), "TestCases")]
        public void FromJson_CorrectJsonClass(char expectedCharacter, string property)
        {
            //arrange
            var json = $"{{\"Property\":{property}}}";
            var jsonClass = new JsonCharClass();

            //act
            FromJson(jsonClass, json);

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
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Property, Is.EqualTo('\u1F4A'));
        }
    }
}