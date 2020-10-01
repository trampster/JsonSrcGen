using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System;

[assembly: JsonList(typeof(string))] 

namespace UnitTests.ListTests
{
    public class StringListTests
    { 
        JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[\"one\",null,\"two\"]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var list = new List<string>(){"one", null, "two"};

            //act
            var json = _convert.ToJson(list);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            //act
            var json = _convert.ToJson((List<string>)null);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("null"));
        }

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var list = new List<string>();

            //act
            _convert.FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0], Is.EqualTo("one"));
            Assert.That(list[1], Is.Null);
            Assert.That(list[2], Is.EqualTo("two"));
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var list = new List<string>(){"asfd", "gggg"};

            //act
            list =_convert.FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0], Is.EqualTo("one"));
            Assert.That(list[1], Is.Null);
            Assert.That(list[2], Is.EqualTo("two"));;
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var list = new List<string>(){"asfd", "gggg"};

            //act
            list = _convert.FromJson(list, "null");

            //assert
            Assert.That(list, Is.Null);
        }

        [Test]
        public void FromJson_ListNull_MakesList()
        {
            //arrange
            //act
            var list = _convert.FromJson((List<string>)null, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0], Is.EqualTo("one"));
            Assert.That(list[1], Is.Null);
            Assert.That(list[2], Is.EqualTo("two"));
        }
    }
}