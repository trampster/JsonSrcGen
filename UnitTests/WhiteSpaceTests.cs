using NUnit.Framework;
using JsonSrcGen;
using System;
using System.Text;

namespace UnitTests
{
    [Json]
    public class WhiteSpaceJsonClass
    {
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string NullProperty {get;set;}
    }

    public class WhiteSpaceTests : WhiteSpaceTestsBase
    {
        protected override ReadOnlySpan<char> FromJson(WhiteSpaceJsonClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8WhiteSpaceTests : WhiteSpaceTestsBase
    {
        protected override ReadOnlySpan<char> FromJson(WhiteSpaceJsonClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class WhiteSpaceTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract ReadOnlySpan<char> FromJson(WhiteSpaceJsonClass value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = "{\r\n\"FirstName\": \"Bob\",\r\n\t\"LastName\":\r   \"Marley\",\n\"NullProperty\": null\n}";
            var jsonClass = new WhiteSpaceJsonClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.FirstName, Is.EqualTo("Bob"));
            Assert.That(jsonClass.LastName, Is.EqualTo("Marley"));
            Assert.That(jsonClass.NullProperty, Is.EqualTo(null));
        }
    }
}