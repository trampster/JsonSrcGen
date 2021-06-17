using NUnit.Framework;
using JsonSrcGen;
using System.Threading;
using System.Text;

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

    public class DecimalPropertyTests : DecimalPropertyTestsBase
    {
        protected override string ToJson(JsonDecimal jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }
    }

    public class Utf8DecimalPropertyTests : DecimalPropertyTestsBase
    {
        protected override string ToJson(JsonDecimal jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }
    }

    public abstract class DecimalPropertyTestsBase
    {
        const decimal MIN = decimal.MinValue;

        protected JsonSrcGen.JsonConverter _convert;

        const string ExpectedJson = "{\"DecNull\":null,\"Min\":-79228162514264337593543950335,\"Value\":-200000.01,\"Value2\":1.5}";

        [SetUp]
        public void Setup() 
        {
            _convert = new JsonConverter();
            //pt-BR using comma so this checks that we are not using the current culture (json spec requires dot)
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
        }

        protected abstract string ToJson(JsonDecimal jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonDec = new JsonDecimal()
            {
                DecNull = null,
                Min = MIN,
                Value = -200000.01m,
                Value2 = 1.5m,
            };

            //act
            var json = ToJson(jsonDec);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }


        [Test]
        public void FromJson_CorrectJsonClass()
        {

            //arrange
            var json = ExpectedJson;
            var jsonDec = new JsonDecimal();

            //act
            _convert.FromJson(jsonDec, json);

            //assert
            Assert.That(jsonDec.DecNull, Is.Null);
            Assert.That(jsonDec.Value2, Is.EqualTo(1.5m));
            Assert.That(jsonDec.Value, Is.EqualTo(-200000.01m));
            Assert.That(jsonDec.Min, Is.EqualTo(MIN));
        }
    }

}