using NUnit.Framework;
using JsonSGen;
using System.Collections.Generic;
using System;

[assembly: JsonList(typeof(short))] 

namespace UnitTests.ListTests
{
    public class ShortListTests
    { 
        JsonSGen.JsonSGenConvert _convert;

        string ExpectedJson = "[-32768,-1,0,1,42,32767]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonSGenConvert();
        }

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var list = new List<short>(){short.MinValue,-1,0, 1, 42, short.MaxValue};

            //act
            var json = _convert.ToJson(list);

            //assert
            Assert.That(json, Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            //act
            var json = _convert.ToJson((List<short>)null);

            //assert
            Assert.That(json, Is.EqualTo("null"));
        }

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var list = new List<short>();

            //act
            _convert.FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(6));
            Assert.That(list[0], Is.EqualTo(short.MinValue));
            Assert.That(list[1], Is.EqualTo(-1));
            Assert.That(list[2], Is.EqualTo(0));
            Assert.That(list[3], Is.EqualTo(1));
            Assert.That(list[4], Is.EqualTo(42));
            Assert.That(list[5], Is.EqualTo(short.MaxValue));
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var list = new List<short>(){1, 2, 3};

            //act
            list =_convert.FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(6));
            Assert.That(list[0], Is.EqualTo(short.MinValue));
            Assert.That(list[1], Is.EqualTo(-1));
            Assert.That(list[2], Is.EqualTo(0));
            Assert.That(list[3], Is.EqualTo(1));
            Assert.That(list[4], Is.EqualTo(42));
            Assert.That(list[5], Is.EqualTo(short.MaxValue));
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var list = new List<short>(){1, 2, 3};

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
            var list = _convert.FromJson((List<short>)null, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(6));
            Assert.That(list[0], Is.EqualTo(short.MinValue));
            Assert.That(list[1], Is.EqualTo(-1));
            Assert.That(list[2], Is.EqualTo(0));
            Assert.That(list[3], Is.EqualTo(1));
            Assert.That(list[4], Is.EqualTo(42));
            Assert.That(list[5], Is.EqualTo(short.MaxValue));
        }
    }
}