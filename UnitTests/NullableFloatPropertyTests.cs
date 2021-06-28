using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class JsonNullableFloatClass
    {
        public float? Age {get;set;}
        public float? Height {get;set;}
        public float? Min {get;set;}
        public float? Max {get;set;}
        public float? Null {get;set;}
        public float? Zero {get;set;}
    }

    public class NullableFloatPropertyTests : NullableFloatPropertyTestsBase
    {
        protected override string ToJson(JsonNullableFloatClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(JsonNullableFloatClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8NullableFloatPropertyTests : NullableFloatPropertyTestsBase
    {
        protected override string ToJson(JsonNullableFloatClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(JsonNullableFloatClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class NullableFloatPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42.21,\"Height\":176.568,\"Max\":3.4028235E+38,\"Min\":-3.4028235E+38,\"Null\":null,\"Zero\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-us");
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
        }

        protected abstract string ToJson(JsonNullableFloatClass jsonClass);


        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonNullableFloatClass()
            {
                Age = 42.21f,
                Height = 176.568f,
                Max = float.MaxValue,
                Min = float.MinValue,
                Null = null,
                Zero = 0
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        protected abstract ReadOnlySpan<char> FromJson(JsonNullableFloatClass value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonNullableFloatClass();

            //act
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42.21f));
            Assert.That(jsonClass.Height, Is.EqualTo(176.568f));
            Assert.That(jsonClass.Min, Is.EqualTo(float.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(float.MaxValue));
            Assert.That(jsonClass.Null, Is.Null);
            Assert.That(jsonClass.Zero, Is.EqualTo(0));
        }
    }
}