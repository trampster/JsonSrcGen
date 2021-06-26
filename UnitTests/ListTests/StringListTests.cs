using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System.Text;

[assembly: JsonList(typeof(string))] 

namespace UnitTests.ListTests
{
    public class StringListTests : StringListTestsBase
    {
        protected override string ToJson(List<string> json)
        {
            return _convert.ToJson(json).ToString();
        }

        protected override List<string> FromJson(List<string> value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8StringListTests : StringListTestsBase
    {
        protected override string ToJson(List<string> json)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(json); 
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override List<string> FromJson(List<string> value, string json)
        {
            return _convert.FromJson(value, Encoding.UTF8.GetBytes(json));
        }
    }

    public abstract class StringListTestsBase
    { 
        protected JsonSrcGen.JsonConverter _convert;

        const string ExpectedJson = "[\"one\",null,\"two\"]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(List<string> json);

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

        protected abstract List<string> FromJson(List<string> value, string json);

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var list = new List<string>();

            //act
            FromJson(list, ExpectedJson);

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
            list = FromJson(list, ExpectedJson);

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
            list = FromJson(list, "null");

            //assert
            Assert.That(list, Is.Null);
        }

        [Test]
        public void FromJson_ListNull_MakesList()
        {
            //arrange
            //act
            var list = FromJson((List<string>)null, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0], Is.EqualTo("one"));
            Assert.That(list[1], Is.Null);
            Assert.That(list[2], Is.EqualTo("two"));
        }
    }
}