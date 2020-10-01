using NUnit.Framework;
using JsonSrcGen;


namespace UnitTests
{
    [Json]
    public class JsonDoubleClass
    {
        public double Age {get;set;}
        public double Height {get;set;}
        public double Min {get;set;}
        public double Max {get;set;}
        public double Zero {get;set;}
    }

    public class DoublePropertyTests 
    {
        JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42.21,\"Height\":176.568,\"Max\":1.7976931348623157E+308,\"Min\":-1.7976931348623157E+308,\"Zero\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonDoubleClass()
            {
                Age = 42.21d,
                Height = 176.568d,
                Max = double.MaxValue,
                Min = double.MinValue,
                Zero = 0
            };

            //act
            var json = _convert.ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonDoubleClass();

            //act
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Age, Is.EqualTo(42.21d));
            Assert.That(jsonClass.Height, Is.EqualTo(176.568d));
            Assert.That(jsonClass.Min, Is.EqualTo(double.MinValue));
            Assert.That(jsonClass.Max, Is.EqualTo(double.MaxValue));
            Assert.That(jsonClass.Zero, Is.EqualTo(0));
        }
    }
}