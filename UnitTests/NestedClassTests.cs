using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonParentClass
    {
        public JsonChildClass Child {get;set;}
        public JsonChildClass Null {get;set;}
    }

    [Json]
    public class JsonChildClass
    { 
        public string Name {get;set;}
        public int Age {get;set;} 
    }

    public class NestedClassTests : NestedClassTestsBase
    {
        protected override string ToJson(JsonParentClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonParentClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8NestedClassTests : NestedClassTestsBase
    {
        protected override string ToJson(JsonParentClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonParentClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class NestedClassTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "{\"Child\":{\"Age\":8,\"Name\":\"Samuel\"},\"Null\":null}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonParentClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonParentClass()
            {
                Child = new JsonChildClass()
                {
                    Name = "Samuel",
                    Age = 8
                },
                Null = null,
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonParentClass value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonParentClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Child.Name, Is.EqualTo("Samuel"));
            Assert.That(jsonClass.Child.Age, Is.EqualTo(8));
            Assert.That(jsonClass.Null, Is.Null);
        }

        [Test]
        public void FromJson_ChildAlreadyExists_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonParentClass()
            {
                Child = {}
            };

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Child.Name, Is.EqualTo("Samuel"));
            Assert.That(jsonClass.Child.Age, Is.EqualTo(8));
            Assert.That(jsonClass.Null, Is.Null);
        }
    }
}