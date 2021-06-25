using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System.Text;

[assembly: JsonArray(typeof(byte))] 

namespace UnitTests.ArrayTests
{
    public class ByteArrayTests : ByteArrayTestsBase
    {
        protected override string ToJson(byte[] json)
        {
            return _convert.ToJson(json).ToString();
        }

        protected override byte[] FromJson(byte[] value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class UtfByteArrayTests : ByteArrayTestsBase
    {
        protected override string ToJson(byte[] json)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(json); 
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override byte[] FromJson(byte[] value, string json)
        {
            return _convert.FromJson(value, Encoding.UTF8.GetBytes(json));
        }
    }

    public abstract class ByteArrayTestsBase
    { 
        protected JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[0,1,255]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(byte[] json);

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var list = new byte[]{0, 1, 255};

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
            var json = ToJson((byte[])null);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("null"));
        }

        protected abstract byte[] FromJson(byte[] value, string json);

        [Test]
        public void FromJson_EmptyArray_CorrectArray()
        {
            //arrange
            var array = new byte[]{};

            //act
            array = FromJson(array, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(3));
            Assert.That(array[0], Is.EqualTo(0));
            Assert.That(array[1], Is.EqualTo(1));
            Assert.That(array[2], Is.EqualTo(255));
        }

        [Test] 
        public void FromJson_PopulatedArrayCorrectLength_ReusesArray()
        {
            //arrange
            var array = new byte[]{3, 2, 1};

            //act
            FromJson(array, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(3));
            Assert.That(array[0], Is.EqualTo(0));
            Assert.That(array[1], Is.EqualTo(1));
            Assert.That(array[2], Is.EqualTo(255));
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var array = new byte[]{42, 42, 42};

            //act
            array = FromJson(array, "null");

            //assert
            Assert.That(array, Is.Null);
        }

        [Test]
        public void FromJson_ArrayNull_MakesArray()
        {
            //arrange
            //act
            var array = FromJson((byte[])null, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(3));
            Assert.That(array[0], Is.EqualTo(0));
            Assert.That(array[1], Is.EqualTo(1));
            Assert.That(array[2], Is.EqualTo(255));
        }

        [Test]
        public void FromJson_EmptyListJson_EmptyArray()
        {
            //arrange
            //act
            var array = FromJson((byte[])null, "[]");

            //assert
            Assert.That(array.Length, Is.EqualTo(0));
        }
    }
}