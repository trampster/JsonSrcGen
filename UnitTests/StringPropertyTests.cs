using NUnit.Framework;
using JsonSrcGen;
using System.Text;

namespace UnitTests
{
    [Json]
    public class JsonClass
    {
        public string EscapingNeeded {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string NullProperty {get;set;}
    }

    public class StringPropertyTests : StringPropertyTestsBase
    {
        protected override string ToJson(JsonClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }
    }

    public class Utf8StringPropertyTests : StringPropertyTestsBase
    {
        protected override string ToJson(JsonClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }
    }

    public abstract class StringPropertyTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;

        const string EscapePropertyJson = "quote\\\"backslash\\\\forwardslash\\/backspace\\bformfeed\\fnewline\\ncarragereturn\\rtab\\t";
        const string NeedsEscaping = "quote\"backslash\\forwardslash/backspace\bformfeed\fnewline\ncarragereturn\rtab\t";

        string ExpectedJson = $"{{\"EscapingNeeded\":\"{EscapePropertyJson}\",\"FirstName\":\"Bob\",\"LastName\":\"Marley\",\"NullProperty\":null}}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(JsonClass jsonClass);

        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonClass()
            {
                EscapingNeeded = NeedsEscaping,
                FirstName = "Bob",
                LastName = "Marley",
                NullProperty = null
            };

            //act
            var json = ToJson(jsonClass);

            //assert
            Assert.That(json, Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = ExpectedJson;
            var jsonClass = new JsonClass();

            //act
            _convert.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.EscapingNeeded, Is.EqualTo(NeedsEscaping));
            Assert.That(jsonClass.FirstName, Is.EqualTo("Bob"));
            Assert.That(jsonClass.LastName, Is.EqualTo("Marley"));
            Assert.That(jsonClass.NullProperty, Is.EqualTo(null));
        }
    }
}