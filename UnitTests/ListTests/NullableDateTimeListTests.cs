using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System;

[assembly: JsonList(typeof(DateTime?))] 

namespace UnitTests.ListTests
{
    public class NullableDateTimeListTests
    { 
        JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[\"2017-07-25T00:00:00\",\"2017-07-25T23:59:58\",null,\"2017-07-25T23:59:58.196\"]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var list = new List<DateTime?>(){new DateTime(2017,7,25), new DateTime(2017,7,25,23,59,58), null, new DateTime(2017,7,25,23,59,58).AddMilliseconds(196)};

            //act
            var json = JsonConverter.ToJson(list);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            //act
            var json = JsonConverter.ToJson((List<DateTime?>)null);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("null"));
        }

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var list = new List<DateTime?>();

            //act
            JsonConverter.FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(4));
            Assert.That(list[0], Is.EqualTo(new DateTime(2017,7,25)));
            Assert.That(list[1], Is.EqualTo(new DateTime(2017,7,25,23,59,58)));
            Assert.That(list[2], Is.Null);
            Assert.That(list[3], Is.EqualTo(new DateTime(2017,7,25,23,59,58).AddMilliseconds(196)));
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var list = new List<DateTime?>(){DateTime.Now, DateTime.Now, DateTime.Now};

            //act
            list =JsonConverter.FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(4));
            Assert.That(list[0], Is.EqualTo(new DateTime(2017,7,25)));
            Assert.That(list[1], Is.EqualTo(new DateTime(2017,7,25,23,59,58)));
            Assert.That(list[2], Is.Null);
            Assert.That(list[3], Is.EqualTo(new DateTime(2017,7,25,23,59,58).AddMilliseconds(196)));
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var list = new List<DateTime?>(){DateTime.Now, DateTime.Now, DateTime.Now};

            //act
            list = JsonConverter.FromJson(list, "null");

            //assert
            Assert.That(list, Is.Null);
        }

        [Test]
        public void FromJson_ListNull_MakesList()
        {
            //arrange
            //act
            var list = JsonConverter.FromJson((List<DateTime?>)null, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(4));
            Assert.That(list[0], Is.EqualTo(new DateTime(2017,7,25)));
            Assert.That(list[1], Is.EqualTo(new DateTime(2017,7,25,23,59,58)));
            Assert.That(list[2], Is.Null);
            Assert.That(list[3], Is.EqualTo(new DateTime(2017,7,25,23,59,58).AddMilliseconds(196)));
        }
    }
}