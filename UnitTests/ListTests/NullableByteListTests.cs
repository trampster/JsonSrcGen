using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System.Text;

[assembly: JsonList(typeof(byte?))] 

namespace UnitTests.ListTests
{
    public class NullableByteListTests : NullableByteListTestsBase
    {
        protected override string ToJson(List<byte?> json)
        {
            return _convert.ToJson(json).ToString();
        }
    }

    public class Utf8NullableByteListTests : NullableByteListTestsBase
    {
        protected override string ToJson(List<byte?> json)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(json); 
            return Encoding.UTF8.GetString(jsonUtf8);
        }
    }

    public abstract class NullableByteListTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[0,1,null,255]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(List<byte?> json);

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var list = new List<byte?>(){0, 1, null, 255};

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
            var json = ToJson((List<byte?>)null);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("null"));
        }

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var list = new List<byte?>();

            //act
            _convert.FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(4));
            Assert.That(list[0], Is.EqualTo(0));
            Assert.That(list[1], Is.EqualTo(1));
            Assert.That(list[2], Is.Null);
            Assert.That(list[3], Is.EqualTo(255));
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var list = new List<byte?>(){3, 5, 9};

            //act
            list =_convert.FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(4));
            Assert.That(list[0], Is.EqualTo(0));
            Assert.That(list[1], Is.EqualTo(1));
            Assert.That(list[2], Is.Null);
            Assert.That(list[3], Is.EqualTo(255));
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var list = new List<byte?>(){42, 42, 42};

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
            var list = _convert.FromJson((List<byte?>)null, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(4));
            Assert.That(list[0], Is.EqualTo(0));
            Assert.That(list[1], Is.EqualTo(1));
            Assert.That(list[2], Is.Null);
            Assert.That(list[3], Is.EqualTo(255));
        }
    }
}