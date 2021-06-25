using NUnit.Framework;
using JsonSrcGen;
using System.Text;

[assembly: JsonArray(typeof(bool))]

namespace UnitTests.ListTests
{
    public class BooleanArrayTests : BooleanArrayTestsBase
    {
        protected override string ToJson(bool[] json)
        {
            return _convert.ToJson(json).ToString();
        }

        protected override bool[] FromJson(bool[] value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8BooleanArrayTests : BooleanArrayTestsBase
    {
        protected override string ToJson(bool[] json)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(json); 
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override bool[] FromJson(bool[] value, string json)
        {
            return _convert.FromJson(value, Encoding.UTF8.GetBytes(json));
        }
    }

    public abstract class BooleanArrayTestsBase
    { 
        protected JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[true,false]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(bool[] json);

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var array = new bool[]{true, false};

            //act
            var json = ToJson(array);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        protected abstract bool[] FromJson(bool[] value, string json);

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var array = new bool[0];

            //act
            array = FromJson(array, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(2));
            Assert.That(array[0], Is.True);
            Assert.That(array[1], Is.False);
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var array = new bool[]{false, false, false};

            //act
            array = FromJson(array, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(2));
            Assert.That(array[0], Is.True);
            Assert.That(array[1], Is.False);
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull() 
        {
            //arrange
            var array = new bool[]{false, false, false};

            //act
            array = FromJson(array, "null");

            //assert
            Assert.That(array, Is.Null); 
        }

        [Test]
        public void FromJson_ListNull_MakesList()
        {
            //arrange
            //act
            var array = FromJson((bool[])null, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(2));
            Assert.That(array[0], Is.True);
            Assert.That(array[1], Is.False);
        }
    }
}