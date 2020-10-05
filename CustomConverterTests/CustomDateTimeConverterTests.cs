using NUnit.Framework;
using JsonSrcGen;
using System;

[assembly: GenerationOutputFolder("/home/daniel/Work/JsonSrcGen/Generated")]

namespace CustomConverterTests
{
    [CustomConverter(typeof(DateTime))]
    public class CustomDateTimeConverter : ICustomConverterValueType<DateTime>
    {
        public void ToJson(IJsonBuilder builder, DateTime target)
        {
            builder.Append("\"");
            builder.Append(target.ToString());
            builder.Append("\""); 
        }

        public ReadOnlySpan<char> FromJson(ReadOnlySpan<char> json, out DateTime value)
        {
            json = json.SkipWhitespace();
            if(json[0] != '\"')
            {
                throw new InvalidJsonException("DateTime should start with a quote");
            }
            json = json.Slice(1);

            var dateTimeSpan = json.ReadTo('\"');

            value = DateTime.Parse(dateTimeSpan);

            return json.Slice(dateTimeSpan.Length + 1); 
        }
    }

    [Json]
    public class CustomClass 
    {
        public DateTime DateTime{get;set;}
    }

    public class CustomDateTimeConverterTests
    {
        [Test]
        public void ToJson_CorrectJson() 
        {
            //arrange
            var dateTime = DateTime.MinValue;

            //act
            var json = new JsonConverter().ToJson(new CustomClass(){DateTime=dateTime}); 

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"DateTime\":\"0001-01-01T00:00:00\"}"));
        }

        [Test]
        public void FromJson_CorrectDateTime() 
        {
            //arrange
            var customClass = new CustomClass();

            //act
            new JsonConverter().FromJson(customClass, "{\"DateTime\":\"0001-01-01T00:00:00\"}"); 

            //assert
            Assert.That(customClass.DateTime, Is.EqualTo(DateTime.MinValue));
        }
    }
}