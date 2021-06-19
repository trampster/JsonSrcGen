using NUnit.Framework;
using JsonSrcGen;
using System.Text;

[assembly: JsonArray(typeof(string))] 

namespace UnitTests.ArrayTests
{
    public class StringArrayTests : StringArrayTestsBase
    {
        protected override string ToJson(string[] json)
        {
            return _convert.ToJson(json).ToString();
        }
    }

    public class Utf8StringArrayTests : StringArrayTestsBase
    {
        protected override string ToJson(string[] json)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(json); 
            return Encoding.UTF8.GetString(jsonUtf8);
        }
    }

    public abstract class StringArrayTestsBase
    { 
        protected JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[\"one\",null,\"two\"]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(string[] json);

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var array = new string[]{"one", null, "two"};

            //act
            var json = ToJson(array);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson.ToString()));
        }

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            //act
            var json = ToJson((string[])null);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("null"));
        }

        [Test]
        public void FromJson_EmptyArray_CorrectArray() 
        {
            //arrange
            var array = new string[]{};

            //act
            array = _convert.FromJson(array, ExpectedJson.ToString());

            //assert
            Assert.That(array.Length, Is.EqualTo(3));
            Assert.That(array[0], Is.EqualTo("one"));
            Assert.That(array[1], Is.Null);
            Assert.That(array[2], Is.EqualTo("two"));
        }

        [Test] 
        public void FromJson_PopulatedArray_CorrectArray()
        {
            //arrange
            var array = new string[]{"asfd", "gggg"};

            //act
            array =_convert.FromJson(array, ExpectedJson.ToString());

            //assert
            Assert.That(array.Length, Is.EqualTo(3));
            Assert.That(array[0], Is.EqualTo("one"));
            Assert.That(array[1], Is.Null);
            Assert.That(array[2], Is.EqualTo("two"));;
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var array = new string[]{"asfd", "gggg"};

            //act
            array = _convert.FromJson(array, "null");

            //assert
            Assert.That(array, Is.Null);
        }

        [Test]
        public void FromJson_ArrayNull_MakesArray()
        {
            //arrange
            //act
            var array = _convert.FromJson((string[])null, ExpectedJson.ToString());

            //assert
            Assert.That(array.Length, Is.EqualTo(3));
            Assert.That(array[0], Is.EqualTo("one"));
            Assert.That(array[1], Is.Null);
            Assert.That(array[2], Is.EqualTo("two"));
        }
    }
}