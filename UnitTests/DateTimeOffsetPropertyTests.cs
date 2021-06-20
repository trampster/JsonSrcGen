using NUnit.Framework;
using JsonSrcGen;
using System;
using System.Collections;
using System.Text;

namespace UnitTests
{
    public class DateTimeOffsetTestCaseData
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData("\"2017-07-25\"", new DateTimeOffset(new DateTime(2017,7,25,0,0,0,DateTimeKind.Utc)));
                yield return new TestCaseData("\"2017-07-25T23:59:58\"", new DateTimeOffset(new DateTime(2017,7,25,23,59,58,DateTimeKind.Utc)));
                var dateTime = new DateTimeOffset(new DateTime(2017,7,25,23,59,58, DateTimeKind.Utc).AddMilliseconds(196.329));
                yield return new TestCaseData("\"2017-07-25T23:59:58.196329\"", dateTime);
                dateTime = new DateTimeOffset(new DateTime(2017,7,25,23,59,58, DateTimeKind.Utc).AddMilliseconds(123.45678));
                yield return new TestCaseData("\"2017-07-25T23:59:58.12345678\"", dateTime);
                dateTime = new DateTimeOffset(new DateTime(2017,7,25,23,59,58, DateTimeKind.Utc).AddMilliseconds(123.45678));

                //utc
                yield return new TestCaseData("\"2017-07-25T23:59:58.12345678Z\"", dateTime);
                
                //with offset
                dateTime = new DateTimeOffset(new DateTime(2017,7,25,23,59,58,DateTimeKind.Unspecified).AddMilliseconds(123.45678), new TimeSpan(3,15,0));
                yield return new TestCaseData("\"2017-07-25T23:59:58.12345678+03:15\"", dateTime);

                //whitespace at start
                yield return new TestCaseData(" \"2017-07-25\"", new DateTimeOffset(new DateTime(2017,7,25,0,0,0,DateTimeKind.Utc)));

                //lazy doesn't start at beginning
                yield return new TestCaseData(" \"2017-07-25\"", new DateTimeOffset(new DateTime(2017,7,25,0,0,0,DateTimeKind.Utc)));

                //nz standard time
                dateTime = new DateTimeOffset(new DateTime(2017,7,25,23,59,58,DateTimeKind.Unspecified).AddMilliseconds(123.45678), new TimeSpan(12,0,0));
                yield return new TestCaseData("\"2017-07-25T23:59:58.12345678+12:00\"", dateTime);

                //nz daylight savings time
                dateTime = new DateTimeOffset(new DateTime(2017,7,25,23,59,58,DateTimeKind.Unspecified).AddMilliseconds(123.45678), new TimeSpan(13,0,0));
                yield return new TestCaseData("\"2017-07-25T23:59:58.12345678+13:00\"", dateTime);

                //Chatham Island Standard Time
                dateTime = new DateTimeOffset(new DateTime(2017,7,25,23,59,58,DateTimeKind.Unspecified).AddMilliseconds(123.45678), new TimeSpan(12,45,0));
                yield return new TestCaseData("\"2017-07-25T23:59:58.12345678+12:45\"", dateTime);

                //Chile Summer Time
                dateTime = new DateTimeOffset(new DateTime(2017,7,25,23,59,58,DateTimeKind.Unspecified).AddMilliseconds(123.45678), new TimeSpan(-3,0,0));
                yield return new TestCaseData("\"2017-07-25T23:59:58.12345678-03:00\"", dateTime);
            }
        }
    }

    [Json]
    public class DateTimeOffsetClass
    {
        public DateTimeOffset Property
        {
            get;
            set;
        }
    }

    public class DateTimeOffsetPropertyTests : DateTimeOffsetPropertyTestsBase
    {
        protected override string ToJson(DateTimeOffsetClass jsonClass)
        {
            return _convert.ToJson(jsonClass).ToString();
        }
    }

    public class Utf8DateTimeOffsetPropertyTests : DateTimeOffsetPropertyTestsBase
    {
        protected override string ToJson(DateTimeOffsetClass jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }
    }

    [TestFixture]
    public abstract class DateTimeOffsetPropertyTestsBase
    {
        protected JsonConverter _convert = new JsonConverter();

        protected abstract string ToJson(DateTimeOffsetClass jsonClass);

        [Test, TestCaseSource(typeof(DateTimeOffsetTestCaseData), "TestCases")]
        public void DateTimeProperty_CorrectlyDeserialized(string value, DateTimeOffset expectedDateTime)
        {
            //arrange
            var jsonClass = new DateTimeOffsetClass();

            //act
            _convert.FromJson(jsonClass, $"{{\"Property\":{value}}}");

            //assert
            Assert.That(jsonClass.Property, Is.EqualTo(expectedDateTime));
        }

        [Test]
        public void ToJson_DateOnly_CorrectJson()
        {
            //arrange
            var dateTimeObject = new DateTimeOffsetClass();
            dateTimeObject.Property = new DateTimeOffset(new DateTime(2017,3,7), new TimeSpan(13,00,00));

            //act
            var json = ToJson(dateTimeObject);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"Property\":\"2017-03-07T00:00:00+13:00\"}"));
        }

        [Test]
        public void ToJson_DateAndTime_CorrectJson()
        {
            //arrange
            var dateTimeObject = new DateTimeOffsetClass();
            dateTimeObject.Property = new DateTimeOffset(new DateTime(2016,1,2,23,59,58,555), new TimeSpan(-9,-15,00));

            //act
            var json = ToJson(dateTimeObject);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"Property\":\"2016-01-02T23:59:58.555-09:15\"}"));
        }

        [Test]
        public void ToJson_Utc_CorrectJson()
        {
            //arrange
            var dateTimeObject = new DateTimeOffsetClass();
            dateTimeObject.Property = new DateTime(2016,1,2,23,59,58,555, DateTimeKind.Utc);

            //act
            var json = ToJson(dateTimeObject);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"Property\":\"2016-01-02T23:59:58.555+00:00\"}"));
        }
    }
}