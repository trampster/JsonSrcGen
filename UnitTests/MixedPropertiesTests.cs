using NUnit.Framework;
using JsonSG;


namespace UnitTests
{
    [Json]
    public class MixedJsonClass
    {
        public int Age {get;set;}
        public string Name {get;set;}
        public string NullProperty {get;set;}
        public bool IsTrue {get;set;}
    }

    public class MixedPropertiesTests
    {
        JsonSG.JsonSGConvert _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonSGConvert();
        }

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new MixedJsonClass()
            {
                Age = 97,
                Name = "Jack",
                NullProperty = null,
                IsTrue = true
            };

            //act
            var json = _convert.ToJson(jsonClass); 

            //assert
            Assert.That(json, Is.EqualTo("{\"Age\":97,\"IsTrue\":true,\"Name\":\"Jack\",\"NullProperty\":null}"));
        }


        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = "{\"Age\":97,\"IsTrue\":true,\"Name\":\"Jack\",\"NullProperty\":null}";
            var jsonClass = new MixedJsonClass();

            //act
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(97));
            Assert.That(jsonClass.Name, Is.EqualTo("Jack"));
            Assert.That(jsonClass.NullProperty, Is.Null);
            Assert.That(jsonClass.IsTrue, Is.True);
        }
    }
}