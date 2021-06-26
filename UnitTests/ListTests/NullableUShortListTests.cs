using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System.Text;

[assembly: JsonList(typeof(ushort?))] 

namespace UnitTests.ListTests
{
    public class NullableUShortListTests : NullableUShortListTestsBase
    {
        protected override string ToJson(List<ushort?> json)
        {
            return _convert.ToJson(json).ToString();
        }

        protected override List<ushort?> FromJson(List<ushort?> value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8NullableUShortListTests : NullableUShortListTestsBase
    {
        protected override string ToJson(List<ushort?> json)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(json); 
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override List<ushort?> FromJson(List<ushort?> value, string json)
        {
            return _convert.FromJson(value, Encoding.UTF8.GetBytes(json));
        }
    }

    public abstract class NullableUShortListTestsBase
    { 
        protected JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[0,1,42,null,65535]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(List<ushort?> json);

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var list = new List<ushort?>(){0, 1, 42, null, ushort.MaxValue};

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
            var json = ToJson((List<ushort?>)null);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("null"));
        }

        protected abstract List<ushort?> FromJson(List<ushort?> value, string json);

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var list = new List<ushort?>();

            //act
            FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(list[0], Is.EqualTo(0));
            Assert.That(list[1], Is.EqualTo(1));
            Assert.That(list[2], Is.EqualTo(42));
            Assert.That(list[3], Is.Null);
            Assert.That(list[4], Is.EqualTo(ushort.MaxValue));
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var list = new List<ushort?>(){1, 2, 3};

            //act
            list = FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(list[0], Is.EqualTo(0));
            Assert.That(list[1], Is.EqualTo(1));
            Assert.That(list[2], Is.EqualTo(42));
            Assert.That(list[3], Is.Null);
            Assert.That(list[4], Is.EqualTo(ushort.MaxValue));
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var list = new List<ushort?>(){1, 2, 3};

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
            var list = FromJson((List<ushort?>)null, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(list[0], Is.EqualTo(0));
            Assert.That(list[1], Is.EqualTo(1));
            Assert.That(list[2], Is.EqualTo(42));
            Assert.That(list[3], Is.Null);
            Assert.That(list[4], Is.EqualTo(ushort.MaxValue));
        }
    }
}