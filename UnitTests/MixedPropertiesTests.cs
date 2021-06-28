using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

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

    public class MixedPropertiesTests : MixedPropertiesTestsBase
    {
        protected override string ToJson(MixedJsonClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(MixedJsonClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8MixedPropertiesTests : MixedPropertiesTestsBase
    {
        protected override string ToJson(MixedJsonClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(MixedJsonClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class MixedPropertiesTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(MixedJsonClass jsonClass);

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
            var json = ToJson(jsonClass); 

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"Age\":97,\"IsTrue\":true,\"Name\":\"Jack\",\"NullProperty\":null}"));
        }

        protected abstract ReadOnlySpan<char> FromJson(MixedJsonClass value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = "{\"Age\":97,\"IsTrue\":true,\"Name\":\"Jack\",\"NullProperty\":null}";
            var jsonClass = new MixedJsonClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(97));
            Assert.That(jsonClass.Name, Is.EqualTo("Jack"));
            Assert.That(jsonClass.NullProperty, Is.Null);
            Assert.That(jsonClass.IsTrue, Is.True);
        }
    }
}