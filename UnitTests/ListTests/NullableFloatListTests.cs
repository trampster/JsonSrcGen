using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System;

[assembly: JsonList(typeof(float?))] 

namespace UnitTests.ListTests
{
    public class NullableFloatListTests
    { 
        JsonSrcGen.JsonSrcGenConvert _convert;

        string ExpectedJson = "[42.21,176.568,3.4028235E+38,-3.4028235E+38,null,0]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonSrcGenConvert();
        }

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var list = new List<float?>(){42.21f, 176.568f, float.MaxValue, float.MinValue, null, 0};

            //act
            var json = _convert.ToJson(list);

            //assert
            Assert.That(json, Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            //act
            var json = _convert.ToJson((List<float?>)null);

            //assert
            Assert.That(json, Is.EqualTo("null"));
        }

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var list = new List<float?>();

            //act
            _convert.FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(6));
            Assert.That(list[0], Is.EqualTo(42.21f));
            Assert.That(list[1], Is.EqualTo(176.568f));
            Assert.That(list[2], Is.EqualTo(float.MaxValue));
            Assert.That(list[3], Is.EqualTo(float.MinValue));
            Assert.That(list[4], Is.Null);
            Assert.That(list[5], Is.EqualTo(0));
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var list = new List<float?>(){1, 2, 3};

            //act
            list =_convert.FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(6));
            Assert.That(list[0], Is.EqualTo(42.21f));
            Assert.That(list[1], Is.EqualTo(176.568f));
            Assert.That(list[2], Is.EqualTo(float.MaxValue));
            Assert.That(list[3], Is.EqualTo(float.MinValue));
            Assert.That(list[4], Is.Null);
            Assert.That(list[5], Is.EqualTo(0));
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var list = new List<float?>(){1, 2, 3};

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
            var list = _convert.FromJson((List<float?>)null, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(6));
            Assert.That(list[0], Is.EqualTo(42.21f));
            Assert.That(list[1], Is.EqualTo(176.568f));
            Assert.That(list[2], Is.EqualTo(float.MaxValue));
            Assert.That(list[3], Is.EqualTo(float.MinValue));
            Assert.That(list[4], Is.Null);
            Assert.That(list[5], Is.EqualTo(0));
        }
    }
}