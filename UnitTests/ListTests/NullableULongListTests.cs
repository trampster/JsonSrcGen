using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System.Text;

[assembly: JsonList(typeof(ulong?))] 

namespace UnitTests.ListTests
{
    public class NullableULongListTests : NullableULongListTestsBase
    {
        protected override string ToJson(List<ulong?> json)
        {
            return _convert.ToJson(json).ToString();
        }

        protected override List<ulong?> FromJson(List<ulong?> value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8NullableULongListTests : NullableULongListTestsBase
    {
        protected override string ToJson(List<ulong?> json)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(json); 
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override List<ulong?> FromJson(List<ulong?> value, string json)
        {
            return _convert.FromJson(value, Encoding.UTF8.GetBytes(json));
        }
    }

    public abstract class NullableULongListTestsBase
    { 
        protected JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[0,1,42,null,18446744073709551615]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(List<ulong?> json);

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var list = new List<ulong?>(){0, 1, 42, null, ulong.MaxValue};

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
            var json = ToJson((List<ulong?>)null);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("null"));
        }

        protected abstract List<ulong?> FromJson(List<ulong?> value, string json);

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var list = new List<ulong?>();

            //act
            FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(list[0], Is.EqualTo(0));
            Assert.That(list[1], Is.EqualTo(1));
            Assert.That(list[2], Is.EqualTo(42));
            Assert.That(list[3], Is.Null);
            Assert.That(list[4], Is.EqualTo(ulong.MaxValue));
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var list = new List<ulong?>(){1, 2, 3};

            //act
            list = FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(list[0], Is.EqualTo(0));
            Assert.That(list[1], Is.EqualTo(1));
            Assert.That(list[2], Is.EqualTo(42));
            Assert.That(list[3], Is.Null);
            Assert.That(list[4], Is.EqualTo(ulong.MaxValue));
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var list = new List<ulong?>(){1, 2, 3};

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
            var list = FromJson((List<ulong?>)null, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(list[0], Is.EqualTo(0));
            Assert.That(list[1], Is.EqualTo(1));
            Assert.That(list[2], Is.EqualTo(42));
            Assert.That(list[3], Is.Null);
            Assert.That(list[4], Is.EqualTo(ulong.MaxValue));
        }
    }
}