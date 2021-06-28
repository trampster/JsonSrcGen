using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonDictionaryClass
    {
        public Dictionary<string, string> Dictionary {get;set;} 
    }

    public class DictionaryPropertyTests : DictionaryPropertyTestsBase
    {
        protected override string ToJson(JsonDictionaryClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonDictionaryClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }
    
    public class Utf8DictionaryPropertyTests : DictionaryPropertyTestsBase
    {
        protected override string ToJson(JsonDictionaryClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonDictionaryClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class DictionaryPropertyTestsBase
    { 
        protected JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "{\"Dictionary\":{\"FirstName\":\"Luke\",\"LastName\":\"Skywalker\"}}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonDictionaryClass jsonClass);

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonDictionaryClass()
            {
                Dictionary = new Dictionary<string, string>()
                {
                    {"FirstName", "Luke"},
                    {"LastName", "Skywalker"}
                }
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            var jsonClass = new JsonDictionaryClass()
            {
                Dictionary = null
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"Dictionary\":null}"));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonDictionaryClass value, string json);

        [Test]
        public void FromJson_EmptyDictionary_CorrectDictionary()
        {
            //arrange
            var jsonClass = new JsonDictionaryClass()
            {
                Dictionary = new Dictionary<string, string>()
            };

            //act
            FromJson(jsonClass, ExpectedJson);

            //assert
            Assert.That(jsonClass.Dictionary.Count, Is.EqualTo(2));
            Assert.That(jsonClass.Dictionary["FirstName"], Is.EqualTo("Luke"));
            Assert.That(jsonClass.Dictionary["LastName"], Is.EqualTo("Skywalker"));
        }

        [Test]
        public void FromJson_NullDictionary_CorrectDictionary()
        {
            //arrange
            var jsonClass = new JsonDictionaryClass()
            {
                Dictionary = null
            };

            //act
            FromJson(jsonClass, ExpectedJson);

            //assert
            Assert.That(jsonClass.Dictionary.Count, Is.EqualTo(2));
            Assert.That(jsonClass.Dictionary["FirstName"], Is.EqualTo("Luke"));
            Assert.That(jsonClass.Dictionary["LastName"], Is.EqualTo("Skywalker"));
        }

        [Test] 
        public void FromJson_PopulatedDictionary_CorrectDictionary()
        {
            //arrange
            var jsonClass = new JsonDictionaryClass()
            {
                Dictionary = new Dictionary<string, string>()
                {
                    {"bar", "asdf"},
                    {"asdg", "asdffs"},
                    {"hghyy", "fdas"}
                }
            };

            //act
            FromJson(jsonClass, ExpectedJson);

            //assert
            Assert.That(jsonClass.Dictionary.Count, Is.EqualTo(2));
            Assert.That(jsonClass.Dictionary["FirstName"], Is.EqualTo("Luke"));
            Assert.That(jsonClass.Dictionary["LastName"], Is.EqualTo("Skywalker"));
        }
    }
}