using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;


namespace UnitTests
{
    [Json]
    public class JsonDictionaryClass
    {
        public Dictionary<string, string> Dictionary {get;set;} 
    }

    public class DictionaryPropertyTests
    { 
        JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "{\"Dictionary\":{\"FirstName\":\"Luke\",\"LastName\":\"Skywalker\"}}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

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
            var json =JsonConverter.ToJson(jsonClass);

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
            var json =JsonConverter.ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"Dictionary\":null}"));
        }

        [Test]
        public void FromJson_EmptyDictionary_CorrectDictionary()
        {
            //arrange
            var jsonClass = new JsonDictionaryClass()
            {
                Dictionary = new Dictionary<string, string>()
            };

            //act
           JsonConverter.FromJson(jsonClass, ExpectedJson);

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
           JsonConverter.FromJson(jsonClass, ExpectedJson);

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
           JsonConverter.FromJson(jsonClass, ExpectedJson);

            //assert
            Assert.That(jsonClass.Dictionary.Count, Is.EqualTo(2));
            Assert.That(jsonClass.Dictionary["FirstName"], Is.EqualTo("Luke"));
            Assert.That(jsonClass.Dictionary["LastName"], Is.EqualTo("Skywalker"));
        }
    }
}