using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonIntClass
    {
        public int Age {get;set;}
        public int Height {get;set;}
        public int Min {get;set;}
        public int Max {get;set;}
        public int Zero {get;set;}
    }

    public class IntPropertyTests : IntPropertyTestsBase
    {
        protected override string ToJson(JsonIntClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonIntClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8IntPropertyTests : IntPropertyTestsBase
    {
        protected override string ToJson(JsonIntClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonIntClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class IntPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42,\"Height\":176,\"Max\":2147483647,\"Min\":-2147483648,\"Zero\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonIntClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonIntClass()
            {
                Age = 42,
                Height = 176,
                Max = int.MaxValue,
                Min = int.MinValue,
                Zero = 0
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonIntClass value, string json);

        [Test] 
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonIntClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Min, Is.EqualTo(int.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(int.MaxValue));
            Assert.That(jsonClass.Zero, Is.EqualTo(0));
        }
    }
}