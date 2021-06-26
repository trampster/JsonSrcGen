using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System.Text;

[assembly: JsonList(typeof(bool))] 

namespace UnitTests.ListTests
{
    public class BooleanListTests : BooleanListTestsBase
    {
        protected override string ToJson(List<bool> json)
        {
            return _convert.ToJson(json).ToString();
        }

        protected override List<bool> FromJson(List<bool> value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8BooleanListTests : BooleanListTestsBase
    {
        protected override string ToJson(List<bool> json)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(json); 
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override List<bool> FromJson(List<bool> value, string json)
        {
            return _convert.FromJson(value, Encoding.UTF8.GetBytes(json));
        }
    }

    public abstract class BooleanListTestsBase
    { 
        protected JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[true,false]"; 

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(List<bool> json);

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var list = new List<bool>(){true, false};

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
            var json = ToJson((List<bool>)null);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("null"));
        }

        protected abstract List<bool> FromJson(List<bool> value, string json);

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var list = new List<bool>();

            //act
            FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.True);
            Assert.That(list[1], Is.False);
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var list = new List<bool>(){false, false, false};

            //act
            list = FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.True);
            Assert.That(list[1], Is.False);
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var list = new List<bool>(){false, false, false};

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
            var list = FromJson((List<bool>)null, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.True);
            Assert.That(list[1], Is.False);
        }

        [Test]
        public void FromJson_EmptyListJson_EmptyList()
        {
            //arrange
            //act
            var list = FromJson((List<bool>)null, "[]");

            //assert
            Assert.That(list.Count, Is.EqualTo(0));
        }
    }
}