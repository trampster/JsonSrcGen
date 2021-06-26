using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System.Text;

[assembly: JsonList(typeof(double))] 

namespace UnitTests.ListTests
{
    public class DoubleListTests : DoubleListTestsBase
    {
        protected override string ToJson(List<double> json)
        {
            return _convert.ToJson(json).ToString();
        }

        protected override List<double> FromJson(List<double> value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class UtfDoubleListTests : DoubleListTestsBase
    {
        protected override string ToJson(List<double> json)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(json); 
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override List<double> FromJson(List<double> value, string json)
        {
            return _convert.FromJson(value, Encoding.UTF8.GetBytes(json));
        }
    }

    public abstract class DoubleListTestsBase
    { 
        protected JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[42.21,176.568,1.7976931348623157E+308,-1.7976931348623157E+308,0]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-us");
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;

        }

        protected abstract string ToJson(List<double> json);

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var list = new List<double>(){42.21d, 176.568d, double.MaxValue, double.MinValue, 0};

            //act
            var json = ToJson(list);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            //act
            var json = ToJson((List<double>)null);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("null"));
        }

        protected abstract List<double> FromJson(List<double> value, string json);

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var list = new List<double>();

            //act
            FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(list[0], Is.EqualTo(42.21d));
            Assert.That(list[1], Is.EqualTo(176.568d));
            Assert.That(list[2], Is.EqualTo(double.MaxValue));
            Assert.That(list[3], Is.EqualTo(double.MinValue));
            Assert.That(list[4], Is.EqualTo(0));
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var list = new List<double>(){1, 2, 3};

            //act
            list =FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(list[0], Is.EqualTo(42.21d));
            Assert.That(list[1], Is.EqualTo(176.568d));
            Assert.That(list[2], Is.EqualTo(double.MaxValue));
            Assert.That(list[3], Is.EqualTo(double.MinValue));
            Assert.That(list[4], Is.EqualTo(0));
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var list = new List<double>(){1, 2, 3};

            //act
            list = FromJson(list, "null");

            //assert
            Assert.That(list, Is.Null);
        }

        [Test]
        public void FromJson_ListNull_MakesList()
        {
            //arrange
            //act
            var list = FromJson((List<double>)null, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(list[0], Is.EqualTo(42.21d));
            Assert.That(list[1], Is.EqualTo(176.568d));
            Assert.That(list[2], Is.EqualTo(double.MaxValue));
            Assert.That(list[3], Is.EqualTo(double.MinValue));
            Assert.That(list[4], Is.EqualTo(0));
        }
    }
}