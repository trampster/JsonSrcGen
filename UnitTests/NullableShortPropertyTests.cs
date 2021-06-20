using NUnit.Framework;
using JsonSrcGen;
using System.Text;

namespace UnitTests
{
    [Json]
    public class JsonNullableShortClass
    {
        public short? Age {get;set;}
        public short? Height {get;set;}
        public short? Min {get;set;}
        public short? Max {get;set;}
        public short? Zero {get;set;}
        public short? Null {get;set;}
    }

    public class NullableShortPropertyTests : NullableShortPropertyTestsBase
    {
        protected override string ToJson(JsonNullableShortClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }
    }

    public class Utf8NullableShortPropertyTests : NullableShortPropertyTestsBase
    {
        protected override string ToJson(JsonNullableShortClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }
    }

    public abstract class NullableShortPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42,\"Height\":176,\"Max\":32767,\"Min\":-32768,\"Null\":null,\"Zero\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonNullableShortClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonNullableShortClass()
            {
                Age = 42,
                Height = 176,
                Max = short.MaxValue,
                Min = short.MinValue,
                Zero = 0,
                Null = null
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson.ToString()));
        }

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonNullableShortClass();

            //act
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Min, Is.EqualTo(short.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(short.MaxValue));
            Assert.That(jsonClass.Zero, Is.EqualTo(0));
            Assert.That(jsonClass.Null, Is.Null);
        }
    }
}