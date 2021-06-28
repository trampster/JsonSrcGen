using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonListClass
    {
        public System.Collections.Generic.List<bool> BooleanList {get;set;} 
    }

    public class ListPropertyTests : ListPropertyTestsBase
    {
        protected override string ToJson(JsonListClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonListClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8ListPropertyTests: ListPropertyTestsBase
    {
        protected override string ToJson(JsonListClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonListClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class ListPropertyTestsBase
    { 
        protected JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = $"{{\"BooleanList\":[true,false]}}";

        protected abstract string ToJson(JsonListClass jsonClass);

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonListClass()
            {
                BooleanList = new List<bool>(){true, false}
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            var jsonClass = new JsonListClass()
            {
                BooleanList = null
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"BooleanList\":null}"));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonListClass value, string json);

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var jsonClass = new JsonListClass()
            {
                BooleanList = new List<bool>()
            };

            //act
            FromJson(jsonClass, ExpectedJson);

            //assert
            Assert.That(jsonClass.BooleanList.Count, Is.EqualTo(2));
            Assert.That(jsonClass.BooleanList[0], Is.True);
            Assert.That(jsonClass.BooleanList[1], Is.False);
        }

        [Test]
        public void FromJson_NullList_CorrectList()
        {
            //arrange
            var jsonClass = new JsonListClass()
            {
                BooleanList = null
            };

            //act
            FromJson(jsonClass, ExpectedJson);

            //assert
            Assert.That(jsonClass.BooleanList.Count, Is.EqualTo(2));
            Assert.That(jsonClass.BooleanList[0], Is.True);
            Assert.That(jsonClass.BooleanList[1], Is.False);
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var jsonClass = new JsonListClass()
            {
                BooleanList = new List<bool>(){false, false, false}
            };

            //act
            FromJson(jsonClass, ExpectedJson);

            //assert
            Assert.That(jsonClass.BooleanList.Count, Is.EqualTo(2));
            Assert.That(jsonClass.BooleanList[0], Is.True);
            Assert.That(jsonClass.BooleanList[1], Is.False);
        }
    }
}