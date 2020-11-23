using NUnit.Framework;
using JsonSrcGen;


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

    public class NestedClassTests
    {
        JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "{\"Child\":{\"Age\":8,\"Name\":\"Samuel\"},\"Null\":null}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

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
            var json =JsonConverter.ToJson(jsonClass);

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
           JsonConverter.FromJson(jsonClass, json);

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
           JsonConverter.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Child.Name, Is.EqualTo("Samuel"));
            Assert.That(jsonClass.Child.Age, Is.EqualTo(8));
            Assert.That(jsonClass.Null, Is.Null);
        }
    }
}