using NUnit.Framework;
using JsonSrcGen;
using System.Text;

namespace UnitTests
{
    [Json]
    public class JsonNullableBooleanClass
    {
        public bool? IsTrue {get;set;}
        public bool? IsFalse {get;set;}
        public bool? IsNull {get;set;}
    }

    public class NullableBooleanPropertyTests : NullableBooleanPropertyTestsBase
    {
        protected override string ToJson(JsonNullableBooleanClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }
    }

    public class Utf8NullableBooleanPropertyTests : NullableBooleanPropertyTestsBase
    {
        protected override string ToJson(JsonNullableBooleanClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }
    }

    public abstract class NullableBooleanPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;

        const string _json = "{\"IsFalse\":false,\"IsNull\":null,\"IsTrue\":true}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonNullableBooleanClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonNullableBooleanClass()
            {
                IsTrue = true,
                IsFalse = false,
                IsNull = null
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(_json));
        }


        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var jsonClass = new JsonNullableBooleanClass();

            //act
            _convert.FromJson(jsonClass, _json);

            //assert
            Assert.That(jsonClass.IsTrue, Is.True);
            Assert.That(jsonClass.IsFalse, Is.False);
            Assert.That(jsonClass.IsNull, Is.Null);
        }
    }
}