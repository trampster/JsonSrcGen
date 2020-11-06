using NUnit.Framework;
using JsonSrcGen;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    [Json]
    public class MyType
    {
        public string Name {get;set;}
    }

    [CustomConverter(typeof(MyType))]
    public class MyTypeConverter : ICustomConverter<MyType> 
    {
        public void ToJson(IJsonBuilder builder, MyType target) {} 

        public ReadOnlySpan<char> FromJson(ReadOnlySpan<char> span, ref MyType value) 
        {
            value.Name = "was set";
            return span;
        }
    }

    [Json]
    public class MyType2
    {
        public string Name {get;set;}
    }


    [Json]
    public class OptionalPropertyClass
    {
        [JsonOptional]
        public bool OptionalBool {get;set;}

        [JsonOptional]
        public int OptionalInt {get;set;}

        [JsonOptional]
        public int[] OptionalArray {get;set;}

        [JsonOptional]
        public char OptionalChar {get;set;}

        [JsonOptional]
        public MyType OptionalMyType {get;set;}

        [JsonOptional]
        public MyType2 OptionalMyType2 {get;set;}

        [JsonOptional]
        public DateTime OptionalDateTime {get;set;}

        [JsonOptional]
        public DateTimeOffset OptionalDateTimeOffset {get;set;}

        [JsonOptional]
        public Dictionary<string, int> OptionalDictionary {get;set;}

        [JsonOptional]
        public Guid OptionalGuid {get;set;}

        [JsonOptional]
        public List<int> OptionalList {get;set;}

        [JsonOptional]
        public int? OptionalNullableInt {get;set;}

        [JsonOptional]
        public bool? OptionalNullableBoolean {get;set;}

        [JsonOptional]
        public DateTime? OptionalNullableDateTime {get;set;}

        [JsonOptional]
        public DateTimeOffset? OptionalNullableDateTimeOffset {get;set;}

        [JsonOptional]
        public Guid? OptionalNullableGuid {get;set;}

        [JsonOptional]
        public String OptionalString {get;set;}
    }

    public class OptionalPropertyTests
    {
        JsonSrcGen.JsonConverter _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = "{}";
            var jsonClass = new OptionalPropertyClass() 
            {
                OptionalBool = true,
                OptionalInt = 42,
                OptionalArray = new int[]{42},
                OptionalChar = '4',
                OptionalMyType = new MyType{Name = "Bob"},
                OptionalMyType2 = new MyType2{Name = "Bob"},
                OptionalDateTime = DateTime.MaxValue,
                OptionalDateTimeOffset = DateTimeOffset.MaxValue,
                OptionalDictionary = new Dictionary<string, int>(), 
                OptionalGuid = new Guid(),
                OptionalList = new List<int>(),
                OptionalNullableInt = 42,
                OptionalNullableBoolean = true,
                OptionalNullableDateTime = DateTime.MaxValue,
                OptionalNullableDateTimeOffset = DateTimeOffset.MaxValue,
                OptionalNullableGuid = new Guid(),
                OptionalString = "string"
            };

            //act
            _convert.FromJson(jsonClass, json); 

            //assert
            Assert.That(jsonClass.OptionalBool, Is.EqualTo(default(bool)));
            Assert.That(jsonClass.OptionalInt, Is.EqualTo(default(int)));
            Assert.That(jsonClass.OptionalArray, Is.Null);
            Assert.That(jsonClass.OptionalChar, Is.EqualTo(default(char))); 
            Assert.That(jsonClass.OptionalMyType, Is.EqualTo(default(MyType))); 
            Assert.That(jsonClass.OptionalMyType2, Is.EqualTo(default(MyType2))); 
            Assert.That(jsonClass.OptionalDateTime, Is.EqualTo(default(DateTime)));
            Assert.That(jsonClass.OptionalDateTimeOffset, Is.EqualTo(default(DateTimeOffset)));
            Assert.That(jsonClass.OptionalDictionary, Is.EqualTo(default(Dictionary<string, int>)));
            Assert.That(jsonClass.OptionalGuid, Is.EqualTo(default(Guid)));
            Assert.That(jsonClass.OptionalList, Is.EqualTo(default(List<int>))); 
            Assert.That(jsonClass.OptionalNullableInt, Is.EqualTo(default(int?))); 
            Assert.That(jsonClass.OptionalNullableBoolean, Is.EqualTo(default(bool?)));  
            Assert.That(jsonClass.OptionalNullableDateTime, Is.EqualTo(default(DateTime?)));
            Assert.That(jsonClass.OptionalNullableDateTimeOffset, Is.EqualTo(default(DateTimeOffset?)));
            Assert.That(jsonClass.OptionalNullableGuid, Is.EqualTo(default(Guid?)));
            Assert.That(jsonClass.OptionalString, Is.EqualTo(default(string)));
        }
    }
}