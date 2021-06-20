using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System;
using System.Text;

[assembly: JsonDictionary(typeof(string), typeof(int))] 

namespace UnitTests.ListTests
{
    public class IntDictionaryArrayTests : IntDictionaryArrayTestsBase
    {
        protected override string ToJson(Dictionary<string, int> json)
        {
            return _convert.ToJson(json).ToString();
        }
    }

    public class Utf8IntDictionaryArrayTests : IntDictionaryArrayTestsBase
    {
        protected override string ToJson(Dictionary<string, int> json)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(json); 
            return Encoding.UTF8.GetString(jsonUtf8);
        }
    }

    public abstract class IntDictionaryArrayTestsBase
    { 
        protected JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "{\"First\":1,\"Second\":2,\"Third\":3}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }
        protected abstract string ToJson(Dictionary<string, int> json);

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
            var json = ToJson(dictionary);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        } 

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            //act
            var json = ToJson((Dictionary<string, int>)null);

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