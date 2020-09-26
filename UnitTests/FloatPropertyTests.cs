using NUnit.Framework;
using JsonSrcGen;


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

    public class FloatPropertyTests 
    {
        JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42.21,\"Height\":176.568,\"Max\":3.4028235E+38,\"Min\":-3.4028235E+38,\"Zero\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

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
            var json = _convert.ToJson(jsonClass);

            //assert
            Assert.That(json, Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonFloatClass();

            //act
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42.21f));
            Assert.That(jsonClass.Height, Is.EqualTo(176.568f));
            Assert.That(jsonClass.Min, Is.EqualTo(float.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(float.MaxValue));
            Assert.That(jsonClass.Zero, Is.EqualTo(0));
        }
    }
}