using NUnit.Framework;
using JsonSG;


namespace UnitTests
{
    [Json]
    public class JsonNullableLongClass
    {
        public long? Age {get;set;}
        public long? Height {get;set;}
        public long? Min {get;set;}
        public long? Max {get;set;}
        public long? Zero {get;set;}
        public long? Null {get;set;}
    }

    public class NullableLongPropertyTests
    {
        JsonSG.JsonSGConvert _convert;
        const string ExpectedJson = "{\"Age\":42,\"Height\":176,\"Max\":9223372036854775807,\"Min\":-9223372036854775808,\"Null\":null,\"Zero\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonSGConvert();
        }

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonNullableLongClass()
            {
                Age = 42,
                Height = 176,
                Max = long.MaxValue,
                Min = long.MinValue,
                Zero = 0,
                Null = null
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
            var jsonClass = new JsonNullableLongClass();

            //act
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Min, Is.EqualTo(long.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(long.MaxValue));
            Assert.That(jsonClass.Zero, Is.EqualTo(0));
            Assert.That(jsonClass.Null, Is.Null);
        }
    }
}