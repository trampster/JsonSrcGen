using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

[Json]
public class GlobalJsonClass
{
    public int Age {get;set;}
    public string Name {get;set;}
    public string NullProperty {get;set;}
    public bool IsTrue {get;set;}
}

namespace UnitTests
{
    public class GlobalNamespaceTests : GlobalNamespaceTestsBase
    {
        protected override string ToJson(GlobalJsonClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(GlobalJsonClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8GlobalNamespaceTestsTests : GlobalNamespaceTestsBase
    {
        protected override string ToJson(GlobalJsonClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(GlobalJsonClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class GlobalNamespaceTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(GlobalJsonClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new GlobalJsonClass()
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

        protected abstract ReadOnlySpan<char> FromJson(GlobalJsonClass value, string json);


        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = "{\"Age\":97,\"IsTrue\":true,\"Name\":\"Jack\",\"NullProperty\":null}";
            var jsonClass = new GlobalJsonClass();

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