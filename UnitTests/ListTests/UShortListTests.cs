using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System;

[assembly: JsonList(typeof(ushort))] 

namespace UnitTests.ListTests
{
    public class UShortListTests
    { 
        JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[0,1,42,65535]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var list = new List<ushort>(){0, 1, 42, ushort.MaxValue};

            //act
            var json = _convert.ToJson(list);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson.ToString()));
        }

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            //act
            var json = _convert.ToJson((List<ushort>)null);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("null"));
        }

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var list = new List<ushort>();

            //act
            _convert.FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(4));
            Assert.That(list[0], Is.EqualTo(0));
            Assert.That(list[1], Is.EqualTo(1));
            Assert.That(list[2], Is.EqualTo(42));
            Assert.That(list[3], Is.EqualTo(ushort.MaxValue));
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var list = new List<ushort>(){1, 2, 3};

            //act
            list =_convert.FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(4));
            Assert.That(list[0], Is.EqualTo(0));
            Assert.That(list[1], Is.EqualTo(1));
            Assert.That(list[2], Is.EqualTo(42));
            Assert.That(list[3], Is.EqualTo(ushort.MaxValue));
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var list = new List<ushort>(){1, 2, 3};

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
            var list = _convert.FromJson((List<ushort>)null, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(4));
            Assert.That(list[0], Is.EqualTo(0));
            Assert.That(list[1], Is.EqualTo(1));
            Assert.That(list[2], Is.EqualTo(42));
            Assert.That(list[3], Is.EqualTo(ushort.MaxValue));
        }
    }
}