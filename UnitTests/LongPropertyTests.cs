using NUnit.Framework;
using JsonSrcGen;
using System.Text;

namespace UnitTests
{
    [Json]
    public class JsonLongClass
    {
        public long Age {get;set;}
        public long Height {get;set;}
        public long Min {get;set;}
        public long Max {get;set;}
        public long Zero {get;set;}
    }

    public class LongPropertyTests : LongPropertyTestsBase
    {
        protected override string ToJson(JsonLongClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }
    }

    public class Utf8LongPropertyTests : LongPropertyTestsBase
    {
        protected override string ToJson(JsonLongClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }
    }

    public abstract class LongPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42,\"Height\":176,\"Max\":9223372036854775807,\"Min\":-9223372036854775808,\"Zero\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }
        protected abstract string ToJson(JsonLongClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonLongClass()
            {
                Age = 42,
                Height = 176,
                Max = long.MaxValue,
                Min = long.MinValue,
                Zero = 0
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
            var jsonClass = new JsonLongClass();

            //act
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Min, Is.EqualTo(long.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(long.MaxValue));
            Assert.That(jsonClass.Zero, Is.EqualTo(0));
        }
    }
}