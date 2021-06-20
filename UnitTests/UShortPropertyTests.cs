using NUnit.Framework;
using JsonSrcGen;
using System.Text;

namespace UnitTests
{
    [Json]
    public class JsonUShortClass
    {
        public ushort Age {get;set;}
        public ushort Height {get;set;}
        public ushort Min {get;set;}
        public ushort Max {get;set;}
    }

    public class UShortPropertyTests : ShortPropertyTestsBase
    {
        protected override string ToJson(JsonShortClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }
    }

    public class Utf8UShortPropertyTests : ShortPropertyTestsBase
    {
        protected override string ToJson(JsonShortClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }
    }

    public abstract class UShortPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42,\"Height\":176,\"Max\":65535,\"Min\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonUShortClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonUShortClass()
            {
                Age = 42,
                Height = 176,
                Max = ushort.MaxValue,
                Min = ushort.MinValue
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonUShortClass();

            //act
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Min, Is.EqualTo(ushort.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(ushort.MaxValue));
        }
    }
}