using NUnit.Framework;
using JsonSrcGen;
using System.Threading;

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

    public class NullableFloatPropertyTests 
    {
        JsonSrcGen.JsonConverter _convert;
        const string ExpectedJson = "{\"Age\":42.21,\"Height\":176.568,\"Max\":3.4028235E+38,\"Min\":-3.4028235E+38,\"Null\":null,\"Zero\":0}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test]
        public void ToJson_CorrectString()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-us");
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;

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
            var json =JsonConverter.ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-us");
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;

            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonNullableFloatClass();

            //act
           JsonConverter.FromJson(jsonClass, json);

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