using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System.Text;

[assembly: JsonList(typeof(int?))] 

namespace UnitTests.ListTests
{
    public class NullableIntListTests : NullableIntListTestsBase
    {
        protected override string ToJson(List<int?> json)
        {
            return _convert.ToJson(json).ToString();
        }

        protected override List<int?> FromJson(List<int?> value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8NullableIntListTests : NullableIntListTestsBase
    {
        protected override string ToJson(List<int?> json)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(json); 
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override List<int?> FromJson(List<int?> value, string json)
        {
            return _convert.FromJson(value, Encoding.UTF8.GetBytes(json));
        }
    }

    public abstract class NullableIntListTestsBase
    { 
        protected JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[-2147483648,-1,0,1,42,null,2147483647]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(List<int?> json);

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var list = new List<int?>(){int.MinValue,-1,0, 1, 42, null, int.MaxValue};

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
            var json = ToJson((List<int?>)null);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("null"));
        }

        protected abstract List<int?> FromJson(List<int?> value, string json);

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var list = new List<int?>();

            //act
            FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(7));
            Assert.That(list[0], Is.EqualTo(int.MinValue));
            Assert.That(list[1], Is.EqualTo(-1));
            Assert.That(list[2], Is.EqualTo(0));
            Assert.That(list[3], Is.EqualTo(1));
            Assert.That(list[4], Is.EqualTo(42));
            Assert.That(list[5], Is.Null);
            Assert.That(list[6], Is.EqualTo(int.MaxValue));
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var list = new List<int?>(){1, 2, 3};

            //act
            list = FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(7));
            Assert.That(list[0], Is.EqualTo(int.MinValue));
            Assert.That(list[1], Is.EqualTo(-1));
            Assert.That(list[2], Is.EqualTo(0));
            Assert.That(list[3], Is.EqualTo(1));
            Assert.That(list[4], Is.EqualTo(42));
            Assert.That(list[5], Is.Null);
            Assert.That(list[6], Is.EqualTo(int.MaxValue));
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var list = new List<int?>(){1, 2, 3};

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
            var list = FromJson((List<int?>)null, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(7));
            Assert.That(list[0], Is.EqualTo(int.MinValue));
            Assert.That(list[1], Is.EqualTo(-1));
            Assert.That(list[2], Is.EqualTo(0));
            Assert.That(list[3], Is.EqualTo(1));
            Assert.That(list[4], Is.EqualTo(42));
            Assert.That(list[5], Is.Null);
            Assert.That(list[6], Is.EqualTo(int.MaxValue));
        }
    }
}