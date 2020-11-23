using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System;

namespace UnitTests
{
    [Json]
    [JsonIgnoreNull]
    public class IgnoreNullClass
    {
        public int? Age {get;set;}
        public int? AgeNull {get;set;}
        public int[] Array {get;set;}
        public int[] ArrayNull {get;set;}
        public bool? Boolean {get;set;}
        public bool? BooleanNull {get;set;}
        public CustomClass CustomClass {get;set;}
        public CustomClass CustomClassNull {get;set;}
        public DateTime? DateTime {get;set;}
        public DateTime? DateTimeNull {get;set;}
        public DateTimeOffset? DateTimeOffset {get;set;}
        public DateTimeOffset? DateTimeOffsetNull {get;set;}
        public Dictionary<string, int> Dictionary {get;set;}
        public Dictionary<string, int> DictionaryNull {get;set;}
        public Guid? Guid {get;set;}
        public Guid? GuidNull {get;set;}
        public List<int> List {get;set;}
        public List<int> ListNull {get;set;}
        public string Name {get;set;}
        public string NameNull {get;set;}
    }

    [Json]
    public class CustomClass
    {
        public string Name {get;set;}
    }

    public class IgnoreNullAttributeTests
    {
        JsonSrcGen.JsonConverter _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        [Test]
        public void ToJson_DoesntIncludeNulls()
        {
            //arrange
            var jsonClass = new IgnoreNullClass()
            {
                Age = 97,
                AgeNull = null,
                Boolean = true,
                CustomClass = new CustomClass(){Name="Name"},
                CustomClassNull = null,
                DateTime = DateTime.MinValue,
                DateTimeNull = null,
                DateTimeOffset = DateTimeOffset.MinValue,
                DateTimeOffsetNull = null,
                Dictionary = new Dictionary<string, int>(){{"one", 1}},
                DictionaryNull = null,
                Guid = Guid.Empty,
                GuidNull = null,
                Name = "Jack",
                NameNull = null,
                Array = new int[]{1},
                List = new List<int>(){1},
                ListNull = null
            };

            //act
            var json =JsonConverter.ToJson(jsonClass);

            //assert
            Assert.That(json.ToString(), Is.EqualTo(
                "{\"Age\":97,\"Array\":[1],\"Boolean\":true,\"CustomClass\":{\"Name\":\"Name\"},\"DateTime\":\"0001-01-01T00:00:00\"," +
                "\"DateTimeOffset\":\"0001-01-01T00:00:00+00:00\",\"Dictionary\":{\"one\":1},\"Guid\":\"00000000-0000-0000-0000-000000000000\"," +
                "\"List\":[1],\"Name\":\"Jack\"}")); 
        }
    }
}