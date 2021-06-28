using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonFloatClass
    {
        public float Age {get;set;}
        public float Height {get;set;}
        public float Min {get;set;}
        public float Max {get;set;}
        public float Zero {get;set;}
    }

    public class FloatPropertyTests : FloatPropertyTestsBase
    {
        protected override string ToJson(JsonFloatClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonFloatClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8FloatPropertyTests : FloatPropertyTestsBase
    {
        protected override string ToJson(JsonFloatClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonFloatClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class FloatPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42.21,\"Height\":176.568,\"Max\":3.4028235E+38,\"Min\":-3.4028235E+38,\"Zero\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-us");
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;

        }

        protected abstract string ToJson(JsonFloatClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonFloatClass()
            {
                Age = 42.21f,
                Height = 176.568f,
                Max = float.MaxValue,
                Min = float.MinValue,
                Zero = 0
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            var resultClass = System.Text.Json.JsonSerializer.Deserialize<JsonFloatClass>(json);
            Assert.That(resultClass.Age, Is.EqualTo(jsonClass.Age));
            Assert.That(resultClass.Height, Is.EqualTo(jsonClass.Height));
            Assert.That(resultClass.Max, Is.EqualTo(jsonClass.Max));
            Assert.That(resultClass.Min, Is.EqualTo(jsonClass.Min));
            Assert.That(resultClass.Zero, Is.EqualTo(jsonClass.Zero));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonFloatClass value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonFloatClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42.21f));
            Assert.That(jsonClass.Height, Is.EqualTo(176.568f));
            Assert.That(jsonClass.Min, Is.EqualTo(float.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(float.MaxValue));
            Assert.That(jsonClass.Zero, Is.EqualTo(0));
        }
    }
}