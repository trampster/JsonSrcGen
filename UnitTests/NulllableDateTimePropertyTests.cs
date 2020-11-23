using NUnit.Framework;
using JsonSrcGen;
using System;
using System.Collections;

namespace UnitTests
{
    public class NullableDateTimeTestCaseData
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData("\"2017-07-25\"", new DateTime(2017,7,25), DateTimeKind.Unspecified);
                yield return new TestCaseData("\"2017-07-25T23:59:58\"", new DateTime(2017,7,25,23,59,58), DateTimeKind.Unspecified);
                var dateTime = new DateTime(2017,7,25,23,59,58).AddMilliseconds(196.329);
                yield return new TestCaseData("\"2017-07-25T23:59:58.196329\"", dateTime, DateTimeKind.Unspecified);
                dateTime = new DateTime(2017,7,25,23,59,58).AddMilliseconds(123.45678);
                yield return new TestCaseData("\"2017-07-25T23:59:58.12345678\"", dateTime, DateTimeKind.Unspecified);
                dateTime = new DateTime(2017,7,25,23,59,58, DateTimeKind.Utc).AddMilliseconds(123.45678);

                //utc
                yield return new TestCaseData("\"2017-07-25T23:59:58.12345678Z\"", dateTime, DateTimeKind.Utc);
                
                //with offset
                var utc = new DateTime(2017,7,25,23,59,58, DateTimeKind.Utc).AddMilliseconds(123.45678).Subtract(new TimeSpan(3,15,0));
                var local = utc.ToLocalTime();
                yield return new TestCaseData("\"2017-07-25T23:59:58.12345678+03:15\"", local, DateTimeKind.Local);

                //whitespace at start
                yield return new TestCaseData(" \"2017-07-25\"", new DateTime(2017,7,25), DateTimeKind.Unspecified);

                //lazy doesn't start at beginning
                yield return new TestCaseData(" \"2017-07-25\"", new DateTime(2017,7,25), DateTimeKind.Unspecified);
                yield return new TestCaseData("null", null, DateTimeKind.Unspecified);
            }
        }
    }

    [Json]
    public class NullableDateTimeClass
    {
        public DateTime? Property
        {
            get;
            set;
        }
    }


    [TestFixture]
    public class NullableDateTimePropertyTests
    {
        JsonConverter _convert = new JsonConverter();

        [Test, TestCaseSource(typeof(NullableDateTimeTestCaseData), "TestCases")]
        public void DateTimeProperty_CorrectlyDeserialized(string value, DateTime? expectedDateTime, DateTimeKind expectedKind)
        {
            //arrange
            var jsonClass = new NullableDateTimeClass();

            //act
           JsonConverter.FromJson(jsonClass, $"{{\"Property\":{value}}}");

            //assert
            Assert.That(jsonClass.Property, Is.EqualTo(expectedDateTime));
            if(expectedDateTime != null)
            {
                Assert.That(jsonClass.Property.Value.Kind, Is.EqualTo(expectedKind)); 
            }
        }

        [Test]
        public void ToJson_DateOnly_CorrectJson()
        {
            //arrange
            var dateTimeObject = new NullableDateTimeClass();
            dateTimeObject.Property = new DateTime(2017,3,7);

            //act
            var json =JsonConverter.ToJson(dateTimeObject);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"Property\":\"2017-03-07T00:00:00\"}"));
        }

        [Test]
        public void ToJson_DateAndTimeNoOffset_CorrectJson()
        {
            //arrange
            var dateTimeObject = new NullableDateTimeClass();
            dateTimeObject.Property = new DateTime(2016,1,2,23,59,58,555);

            //act
            var json =JsonConverter.ToJson(dateTimeObject);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"Property\":\"2016-01-02T23:59:58.555\"}"));
        }

        [Test]
        public void ToJson_Utc_CorrectJson()
        {
            //arrange
            var dateTimeObject = new NullableDateTimeClass();
            dateTimeObject.Property = new DateTime(2016,1,2,23,59,58,555, DateTimeKind.Utc);

            //act
            var json =JsonConverter.ToJson(dateTimeObject);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"Property\":\"2016-01-02T23:59:58.555Z\"}"));
        }

        //[Test]
        //public void ToJson_Local_CorrectJson()
        //{
        //    //arrange
        //    var dateTimeObject = new NullableDateTimeClass();
        //    dateTimeObject.Property = new DateTime(2016,1,2,23,59,58,555, DateTimeKind.Local);

        //    //act
        //    var json =JsonConverter.ToJson(dateTimeObject);

        //    //assert
        //    var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
        //    var sign = offset.Duration().TotalMinutes > 0 ? "+" : "-";
        //    var hours = Math.Abs(offset.Hours).ToString("00");
        //    var minutes = offset.Minutes.ToString("00");
        //    Assert.That(json.ToString(), Is.EqualTo($"{{\"Property\":\"2016-01-02T23:59:58.555{sign}{hours}:{minutes}\"}}"));
        //}
    }
}