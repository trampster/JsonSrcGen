using NUnit.Framework;
using JsonSrcGen;


namespace UnitTests
{
    [Json]
    public class JsonNullableUShortClass
    {
        public ushort? Age {get;set;}
        public ushort? Height {get;set;}
        public ushort? Min {get;set;}
        public ushort? Max {get;set;}
        public ushort? Null {get;set;}
    }

    public class NullableUShortPropertyTests
    {
        JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42,\"Height\":176,\"Max\":65535,\"Min\":0,\"Null\":null}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonNullableUShortClass()
            {
                Age = 42,
                Height = 176,
                Max = ushort.MaxValue,
                Min = ushort.MinValue,
                Null = null
            };

            //act
            var json =JsonConverter.ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonNullableUShortClass();

            //act
           JsonConverter.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Min, Is.EqualTo(ushort.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(ushort.MaxValue));
            Assert.That(jsonClass.Null, Is.Null);
        } 
    }
}