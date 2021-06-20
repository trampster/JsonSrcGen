using NUnit.Framework;
using JsonSrcGen;
using System.Text;

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
    }

    public class Utf8NestedClassTests : NestedClassTestsBase
    {
        protected override string ToJson(JsonParentClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
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


        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonParentClass();

            //act
            _convert.FromJson(jsonClass, json);

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
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Child.Name, Is.EqualTo("Samuel"));
            Assert.That(jsonClass.Child.Age, Is.EqualTo(8));
            Assert.That(jsonClass.Null, Is.Null);
        }
    }
}