using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System;

[assembly: JsonDictionary(typeof(string), typeof(int))] 

namespace UnitTests.ListTests
{
    public class IntDictionaryArrayTests 
    { 
        JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "{\"First\":1,\"Second\":2,\"Third\":3}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var dictionary = new Dictionary<string, int>()
            {
                {"First", 1},
                {"Second", 2},
                {"Third", 3},
            };

            //act
            var json = _convert.ToJson(dictionary);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        } 

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            //act
            var json = _convert.ToJson((Dictionary<string, int>)null);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("null"));
        }

        [Test]
        public void FromJson_EmptyDictionary_CorrectList() 
        {
            //arrange
            var dictionary = new Dictionary<string, int>();

            //act
            dictionary = _convert.FromJson(dictionary, ExpectedJson);

            //assert
            Assert.That(dictionary.Count, Is.EqualTo(3));
            Assert.That(dictionary["First"], Is.EqualTo(1));
            Assert.That(dictionary["Second"], Is.EqualTo(2));
            Assert.That(dictionary["Third"], Is.EqualTo(3));
        }

        [Test] 
        public void FromJson_PopulatedDictionary_CorrectDictionary()
        {
            //arrange
            var dictionary = new Dictionary<string, int>()
            {
                {"Blar", 4},
                {"Tar", 5},
                {"Far", 3},
                {"Mar", 42},
                {"Par", 77}
            };

            //act
            dictionary =_convert.FromJson(dictionary, ExpectedJson);

            //assert
            Assert.That(dictionary.Count, Is.EqualTo(3));
            Assert.That(dictionary["First"], Is.EqualTo(1));
            Assert.That(dictionary["Second"], Is.EqualTo(2));
            Assert.That(dictionary["Third"], Is.EqualTo(3));
        }

        [Test] 
        public void FromJson_EmptyJson_EmptyDictionary()
        {
            //arrange
            var dictionary = new Dictionary<string, int>()
            {
                {"Blar", 4},
                {"Tar", 5},
                {"Far", 3},
                {"Mar", 42},
                {"Par", 77}
            };

            //act
            dictionary =_convert.FromJson(dictionary, "{}");

            //assert
            Assert.That(dictionary.Count, Is.EqualTo(0));
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull() 
        {
            //arrange
            var dictionary = new Dictionary<string, int>()
            {
                {"Blar", 4},
                {"Tar", 5},
                {"Far", 3},
                {"Mar", 42},
                {"Par", 77}
            };

            //act
            dictionary = _convert.FromJson(dictionary, "null");

            //assert
            Assert.That(dictionary, Is.Null); 
        }

        [Test]
        public void FromJson_DictionaryNull_MakesDictionary()
        {
            //arrange
            //act
            var dictionary = _convert.FromJson((Dictionary<string,int>)null, ExpectedJson);

            //assert
            Assert.That(dictionary.Count, Is.EqualTo(3));
            Assert.That(dictionary["First"], Is.EqualTo(1));
            Assert.That(dictionary["Second"], Is.EqualTo(2));
            Assert.That(dictionary["Third"], Is.EqualTo(3));
        }
    }
}