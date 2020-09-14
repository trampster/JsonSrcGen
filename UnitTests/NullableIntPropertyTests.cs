using NUnit.Framework;
using JsonSrcGen;


namespace UnitTests
{
    [Json]
    public class JsonNullableIntClass
    {
        public int? Age {get;set;}
        public int? Height {get;set;}
        public int? Min {get;set;}
        public int? Max {get;set;}
        public int? Zero {get;set;}
        public int? Null {get;set;}
    }

    public class NullableIntPropertyTests
    {
        JsonSrcGen.JsonSrcGenConvert _convert;
        const string ExpectedJson = "{\"Age\":42,\"Height\":176,\"Max\":2147483647,\"Min\":-2147483648,\"Null\":null,\"Zero\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonSrcGenConvert();
        }

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonNullableIntClass()
            {
                Age = 42,
                Height = 176,
                Max = int.MaxValue,
                Min = int.MinValue,
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
            var jsonClass = new JsonNullableIntClass();

            //act
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42));
            Assert.That(jsonClass.Height, Is.EqualTo(176));
            Assert.That(jsonClass.Min, Is.EqualTo(int.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(int.MaxValue));
            Assert.That(jsonClass.Zero, Is.EqualTo(0));
            Assert.That(jsonClass.Null, Is.Null);
        }
    }
}