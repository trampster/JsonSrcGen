using NUnit.Framework;
using JsonSrcGen;


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

    public class LongPropertyTests
    {
        JsonSrcGen.JsonSrcGenConvert _convert;
        const string ExpectedJson = "{\"Age\":42,\"Height\":176,\"Max\":9223372036854775807,\"Min\":-9223372036854775808,\"Zero\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonSrcGenConvert();
        }

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
            var json = _convert.ToJson(jsonClass);

            //assert
            Assert.That(json, Is.EqualTo(ExpectedJson));
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