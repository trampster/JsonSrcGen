using NUnit.Framework;
using JsonSrcGen;
using System;

namespace CustomConverterTests
{
    [CustomConverter(typeof(string))]
    public class CustomCaseStringConverter : ICustomConverter<string>
    {
        public void ToJson(IJsonBuilder builder, string target)
        {
            builder.Append("\"");
            builder.Append(target.ToUpper());
            builder.Append("\""); 
        }

        public ReadOnlySpan<char> FromJson(ReadOnlySpan<char> json, ref string value)
        {
            json = json.SkipWhitespace();
            if(json[0] != '\"')
            {
                throw new InvalidJsonException("String should start with a quote");
            }
            json = json.Slice(1);

            var upercase = json.ReadTo('\"');

            value = upercase.ToString().ToLower();

            return json.Slice(upercase.Length + 1); 
        }
    }

    [Json]
    public class CustomStringClass 
    {
        public string Property{get;set;}
    }

    public class CustomStringConverterTests
    {
        [Test]
        public void ToJson_CorrectJson() 
        {
            //arrange
            var dateTime = DateTime.MinValue;

            //act
            var json = new JsonConverter().ToJson(new CustomStringClass(){Property="upercase"}); 

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"Property\":\"UPERCASE\"}"));
        }

        [Test]
        public void FromJson_CorrectDateTime() 
        {
            //arrange
            var customClass = new CustomStringClass();

            //act
            new JsonConverter().FromJson(customClass, "{\"DateTime\":\"UPERCASE\"}"); 

            //assert
            Assert.That(customClass.Property, Is.EqualTo("upercase")); 
        }
    }
}