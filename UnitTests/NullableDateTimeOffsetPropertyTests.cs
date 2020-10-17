using NUnit.Framework;
using JsonSrcGen;
using System;
using System.Collections;

namespace UnitTests
{
    public class NullableDateTimeOffsetTestCaseData
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
                
                //null                
                yield return new TestCaseData("null", null);
            }
        }
    }

    [Json]
    public class NullableDateTimeOffsetClass
    {
        public DateTimeOffset? Property
        {
            get;
            set;
        }
    }


    [TestFixture]
    public class NullableDateTimeOffsetPropertyTests
    {
        JsonConverter _convert = new JsonConverter();

        [Test, TestCaseSource(typeof(DateTimeOffsetTestCaseData), "TestCases")]
        public void DateTimeProperty_CorrectlyDeserialized(string value, DateTimeOffset expectedDateTime)
        {
            //arrange
            var jsonClass = new NullableDateTimeOffsetClass();

            //act
            _convert.FromJson(jsonClass, $"{{\"Property\":{value}}}");

            //assert
            Assert.That(jsonClass.Property, Is.EqualTo(expectedDateTime));
        }

        [Test]
        public void ToJson_DateOnly_CorrectJson()
        {
            //arrange
            var dateTimeObject = new NullableDateTimeOffsetClass();
            dateTimeObject.Property = new DateTimeOffset(new DateTime(2017,3,7), new TimeSpan(13,00,00));

            //act
            var json = _convert.ToJson(dateTimeObject);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"Property\":\"2017-03-07T00:00:00+13:00\"}"));
        }

        [Test]
        public void ToJson_DateAndTime_CorrectJson()
        {
            //arrange
            var dateTimeObject = new NullableDateTimeOffsetClass();
            dateTimeObject.Property = new DateTimeOffset(new DateTime(2016,1,2,23,59,58,555), new TimeSpan(-9,-15,00));

            //act
            var json = _convert.ToJson(dateTimeObject);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"Property\":\"2016-01-02T23:59:58.555-09:15\"}"));
        }

        [Test]
        public void ToJson_Utc_CorrectJson()
        {
            //arrange
            var dateTimeObject = new NullableDateTimeOffsetClass();
            dateTimeObject.Property = new DateTime(2016,1,2,23,59,58,555, DateTimeKind.Utc);

            //act
            var json = _convert.ToJson(dateTimeObject);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"Property\":\"2016-01-02T23:59:58.555+00:00\"}"));
        }

        [Test]
        public void ToJson_Local_CorrectJson()
        {
            //arrange
            var dateTimeObject = new NullableDateTimeOffsetClass();
            dateTimeObject.Property = new DateTime(2016,1,2,23,59,58,555, DateTimeKind.Local);

            //act
            var json = _convert.ToJson(dateTimeObject);

            //assert
            var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
            var sign = offset.Duration().TotalMinutes >= 0 ? "+" : "-";
            var hours = Math.Abs(offset.Hours).ToString("00");
            var minutes = offset.Minutes.ToString("00");
            Assert.That(json.ToString(), Is.EqualTo($"{{\"Property\":\"2016-01-02T23:59:58.555{sign}{hours}:{minutes}\"}}"));
        }

        [Test]
        public void ToJson_Null_CorrectJson()
        {
            //arrange
            var dateTimeObject = new NullableDateTimeOffsetClass();
            dateTimeObject.Property = null;

            //act
            var json = _convert.ToJson(dateTimeObject);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"Property\":null}"));
        }
    }
}