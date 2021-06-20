using NUnit.Framework;
using JsonSrcGen;
using System.Text;

namespace UnitTests
{
    [Json]
    public class JsonBooleanClass
    {
        public bool IsTrue {get;set;}
        public bool IsFalse {get;set;}
    }

    public class BooleanPropertyTests : BooleanPropertyTestsBase
    {
        protected override string ToJson(JsonBooleanClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }
    }

    public class Utf8BooleanPropertyTests : BooleanPropertyTestsBase
    {
        protected override string ToJson(JsonBooleanClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }
    }

    public abstract class BooleanPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonBooleanClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonBooleanClass()
            {
                IsTrue = true,
                IsFalse = false
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"IsFalse\":false,\"IsTrue\":true}"));
        }


        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = "{\"IsFalse\":false,\"IsTrue\":true}";
            var jsonClass = new JsonBooleanClass();

            //act
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.IsTrue, Is.True);
            Assert.That(jsonClass.IsFalse, Is.False);
        }
    }
}