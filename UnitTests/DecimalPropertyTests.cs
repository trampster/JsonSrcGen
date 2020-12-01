using NUnit.Framework;
using JsonSrcGen;
using System.Threading;

namespace UnitTests
{
    [Json]
    public class JsonDecimal
    {
        public decimal Value { get; set; }

        public decimal Value2 { get; set; }

        public decimal Min { get; set; }

        public decimal? DecNull { get; set; }
    }

    public class DecimalPropertyTests
    {
        const decimal MIN = decimal.MinValue;

        JsonSrcGen.JsonConverter _convert;

        const string ExpectedJson_PT_BR = "{\"DecNull\":null,\"Min\":-79228162514264337593543950335,\"Value\":-200000,01,\"Value2\":1,5}";

        const string ExpectedJson_EN_US = "{\"DecNull\":null,\"Min\":-79228162514264337593543950335,\"Value\":-200000.01,\"Value2\":1.5}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test]
        public void ToJson_CorrectString_PT_BR()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;

            //arrange
            var jsonDec = new JsonDecimal()
            {
                DecNull = null,
                Min = MIN,
                Value = -200000.01m,
                Value2 = 1.5m,

            };

            //act
            var json = _convert.ToJson(jsonDec);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson_PT_BR));
        }

        [Test]
        public void FromJson_CorrectJsonClass_PT_BR()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;

            //arrange
            var json = ExpectedJson_PT_BR;
            var jsonDec = new JsonDecimal();

            //act
            _convert.FromJson(jsonDec, json);

            //assert
            Assert.That(jsonDec.DecNull, Is.Null);
            Assert.That(jsonDec.Value, Is.EqualTo(-200000.01m));
            Assert.That(jsonDec.Value2, Is.EqualTo(1.5m));
            Assert.That(jsonDec.Min, Is.EqualTo(MIN));
        }

        [Test]
        public void ToJson_CorrectString_EN_US()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-us");
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;

            //arrange
            var jsonDec = new JsonDecimal()
            {
                DecNull = null,
                Min = MIN,
                Value = -200000.01m,
                Value2 = 1.5m,
            };

            //act
            var json = _convert.ToJson(jsonDec);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson_EN_US));
        }


        [Test]
        public void FromJson_CorrectJsonClass_EN_US()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-us");
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;

            //arrange
            var json = ExpectedJson_EN_US;
            var jsonDec = new JsonDecimal();

            //act
            _convert.FromJson(jsonDec, json);

            //assert
            Assert.That(jsonDec.DecNull, Is.Null);
            Assert.That(jsonDec.Value, Is.EqualTo(-200000.01m));
            Assert.That(jsonDec.Value2, Is.EqualTo(1.5m));
            Assert.That(jsonDec.Min, Is.EqualTo(MIN));
        }
    }

}