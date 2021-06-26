using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;
using System.Text;

[assembly: JsonList(typeof(UnitTests.ListTests.CustomClass))] 

namespace UnitTests.ListTests
{
    [Json]
    public class CustomClass
    {
        public string Name {get;set;}
    }

    public class CustomClassListTests : CustomClassListTestsBase
    {
        protected override string ToJson(List<CustomClass> json)
        {
            return _convert.ToJson(json).ToString();
        }

        protected override List<CustomClass> FromJson(List<CustomClass> value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class UtfCustomClassListTests : CustomClassListTestsBase
    {
        protected override string ToJson(List<CustomClass> json)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(json); 
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override List<CustomClass> FromJson(List<CustomClass> value, string json)
        {
            return _convert.FromJson(value, Encoding.UTF8.GetBytes(json));
        }
    }

    public abstract class CustomClassListTestsBase
    { 
        protected JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[{\"Name\":\"William\"},null,{\"Name\":\"Susen\"}]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(List<CustomClass> json);

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var list = new List<CustomClass>(){new CustomClass(){Name = "William"}, null, new CustomClass(){Name="Susen"}};

            //act
            var json = ToJson(list);

            //assert
            Assert.That(new string(json), Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            //act
            var json = ToJson((List<CustomClass>)null);

            //assert
            Assert.That(new string(json), Is.EqualTo("null"));
        }

        protected abstract List<CustomClass> FromJson(List<CustomClass> value, string json);


        [Test]
        public void FromJson_EmptyList_CorrectList() 
        {
            //arrange
            var list = new List<CustomClass>();

            //act
            FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0].Name, Is.EqualTo("William"));
            Assert.That(list[1], Is.Null);
            Assert.That(list[2].Name, Is.EqualTo("Susen"));
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var list = new List<CustomClass>(){new CustomClass(), new CustomClass(), new CustomClass()};

            //act
            list =FromJson(list, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0].Name, Is.EqualTo("William"));
            Assert.That(list[1], Is.Null);
            Assert.That(list[2].Name, Is.EqualTo("Susen"));
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var list = new List<CustomClass>(){new CustomClass(), new CustomClass(), new CustomClass()};

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
            var list = FromJson((List<CustomClass>)null, ExpectedJson);

            //assert
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0].Name, Is.EqualTo("William"));
            Assert.That(list[1], Is.Null);
            Assert.That(list[2].Name, Is.EqualTo("Susen"));
        }
    }
}