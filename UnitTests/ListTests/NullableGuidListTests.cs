using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System;
using System.Text;

[assembly: JsonList(typeof(Guid?))] 

namespace UnitTests.ListTests
{
    public class NullableGuidListTests : NullableGuidListTestsBase
    {
        protected override string ToJson(List<Guid?> json)
        {
            return _convert.ToJson(json).ToString();
        }

        protected override List<Guid?> FromJson(List<Guid?> value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8NullableGuidListTests : NullableGuidListTestsBase
    {
        protected override string ToJson(List<Guid?> json)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(json); 
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override List<Guid?> FromJson(List<Guid?> value, string json)
        {
            return _convert.FromJson(value, Encoding.UTF8.GetBytes(json));
        }
    }

    public abstract class NullableGuidListTestsBase
    { 
        protected JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[\"00000001-0002-0003-0405-060708090a0b\",null,\"00000002-0002-0003-0405-060708090a0b\"]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(List<Guid?> json);

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var list = new List<Guid?>(){new Guid(1,2,3,4,5,6,7,8,9,10,11), null, new Guid(2,2,3,4,5,6,7,8,9,10,11)};

            //act
            var json = ToJson(list);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            //act
            var json = ToJson((List<Guid?>)null);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("null"));
        }
        protected abstract List<Guid?> FromJson(List<Guid?> value, string json);

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var list = new List<Guid?>();

            //act
            FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0], Is.EqualTo(new Guid(1,2,3,4,5,6,7,8,9,10,11)));
            Assert.That(list[1], Is.Null);
            Assert.That(list[2], Is.EqualTo(new Guid(2,2,3,4,5,6,7,8,9,10,11)));
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var list = new List<Guid?>(){new Guid(), new Guid(), new Guid(), new Guid()};

            //act
            list = FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0], Is.EqualTo(new Guid(1,2,3,4,5,6,7,8,9,10,11)));
            Assert.That(list[1], Is.Null);
            Assert.That(list[2], Is.EqualTo(new Guid(2,2,3,4,5,6,7,8,9,10,11)));
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var list = new List<Guid?>(){new Guid()};

            //act
            list = FromJson(list, "null");

            //assert
            Assert.That(list, Is.Null);
        }

        [Test]
        public void FromJson_ListNull_MakesList()
        {
            //arrange
            //act
            var list = FromJson((List<Guid?>)null, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0], Is.EqualTo(new Guid(1,2,3,4,5,6,7,8,9,10,11)));
            Assert.That(list[1], Is.Null);
            Assert.That(list[2], Is.EqualTo(new Guid(2,2,3,4,5,6,7,8,9,10,11)));
        }
    }
}