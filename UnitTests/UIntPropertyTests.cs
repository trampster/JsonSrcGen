using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonUIntClass
    {
        public uint Age {get;set;}
        public uint Height {get;set;}
        public uint Min {get;set;}
        public uint Max {get;set;}
    }

    public class UIntPropertyTests : UIntPropertyTestsBase
    {
        protected override string ToJson(JsonUIntClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonUIntClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8UIntPropertyTests : UIntPropertyTestsBase
    {
        protected override string ToJson(JsonUIntClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonUIntClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class UIntPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert; 
        const string ExpectedJson = "{\"Age\":42,\"Height\":176,\"Max\":4294967295,\"Min\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonUIntClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonUIntClass()
            {
                Age = 42,
                Height = 176,
                Max = uint.MaxValue,
                Min = uint.MinValue
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonUIntClass value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonUIntClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Min, Is.EqualTo(uint.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(uint.MaxValue));
        }
    }
}