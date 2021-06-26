using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System.Text;

[assembly: JsonList(typeof(long))] 

namespace UnitTests.ListTests
{
    public class LongListTests : LongListTestsBase
    {
        protected override string ToJson(List<long> json)
        {
            return _convert.ToJson(json).ToString();
        }

        protected override List<long> FromJson(List<long> value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8LongListTests : LongListTestsBase
    {
        protected override string ToJson(List<long> json)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(json); 
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override List<long> FromJson(List<long> value, string json)
        {
            return _convert.FromJson(value, Encoding.UTF8.GetBytes(json));
        }
    }

    public abstract class LongListTestsBase
    { 
        protected JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[-9223372036854775808,-1,0,1,42,9223372036854775807]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(List<long> json);


        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var list = new List<long>(){long.MinValue,-1,0, 1, 42, long.MaxValue};

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
            var json = ToJson((List<long>)null);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("null"));
        }

        protected abstract List<long> FromJson(List<long> value, string json);

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var list = new List<long>();

            //act
            FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(6));
            Assert.That(list[0], Is.EqualTo(long.MinValue));
            Assert.That(list[1], Is.EqualTo(-1));
            Assert.That(list[2], Is.EqualTo(0));
            Assert.That(list[3], Is.EqualTo(1));
            Assert.That(list[4], Is.EqualTo(42));
            Assert.That(list[5], Is.EqualTo(long.MaxValue));
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var list = new List<long>(){1, 2, 3};

            //act
            list = FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(6));
            Assert.That(list[0], Is.EqualTo(long.MinValue));
            Assert.That(list[1], Is.EqualTo(-1));
            Assert.That(list[2], Is.EqualTo(0));
            Assert.That(list[3], Is.EqualTo(1));
            Assert.That(list[4], Is.EqualTo(42));
            Assert.That(list[5], Is.EqualTo(long.MaxValue));
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var list = new List<long>(){1, 2, 3};

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
            var list = FromJson((List<long>)null, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(6));
            Assert.That(list[0], Is.EqualTo(long.MinValue));
            Assert.That(list[1], Is.EqualTo(-1));
            Assert.That(list[2], Is.EqualTo(0));
            Assert.That(list[3], Is.EqualTo(1));
            Assert.That(list[4], Is.EqualTo(42));
            Assert.That(list[5], Is.EqualTo(long.MaxValue));
        }
    }
}