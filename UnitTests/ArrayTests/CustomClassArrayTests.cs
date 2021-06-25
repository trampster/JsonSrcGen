using NUnit.Framework;
using JsonSrcGen;
using System;
using System.Text;

[assembly: JsonArray(typeof(UnitTests.ArrayTests.CustomClass))] 

namespace UnitTests.ArrayTests
{
    [Json]
    public class CustomClass
    {
        public string Name {get;set;} 
    }

    public class CustomClassArrayTests : CustomClassArrayTestsBase
    {
        protected override string ToJson(CustomClass[] json)
        {
            return _convert.ToJson(json).ToString();
        }

        protected override CustomClass[] FromJson(CustomClass[] value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8CustomClassArrayTests : CustomClassArrayTestsBase
    {
        protected override string ToJson(CustomClass[] json) 
        {
            var jsonUtf8 = _convert.ToJsonUtf8(json); 
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override CustomClass[] FromJson(CustomClass[] value, string json)
        {
            return _convert.FromJson(value, Encoding.UTF8.GetBytes(json));
        }
    }

    public abstract class CustomClassArrayTestsBase
    { 
        protected JsonSrcGen.JsonConverter _convert;

        string ExpectedJson = "[{\"Name\":\"William\"},null,{\"Name\":\"Susen\"}]";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }
        protected abstract string ToJson(CustomClass[] json);

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var array = new CustomClass[]{new CustomClass{Name = "William"}, null, new CustomClass(){Name="Susen"}};

            //act
            var json = ToJson(array);

            //assert
            Assert.That(new String(json), Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            //act
            var json = ToJson((CustomClass[])null);

            //assert
            Assert.That(new String(json), Is.EqualTo("null"));
        }

        protected abstract CustomClass[] FromJson(CustomClass[] value, string json);

        [Test]
        public void FromJson_EmptyArray_CorrectArray() 
        {
            //arrange
            var array = new CustomClass[]{};

            //act
            array = FromJson(array, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(3));
            Assert.That(array[0].Name, Is.EqualTo("William"));
            Assert.That(array[1], Is.Null);
            Assert.That(array[2].Name, Is.EqualTo("Susen"));
        }

        [Test] 
        public void FromJson_PopulatedArray_CorrectArray()
        {
            //arrange
            var array = new CustomClass[]{new CustomClass(), new CustomClass(), new CustomClass()};

            //act
            array = FromJson(array, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(3));
            Assert.That(array[0].Name, Is.EqualTo("William"));
            Assert.That(array[1], Is.Null);
            Assert.That(array[2].Name, Is.EqualTo("Susen"));
        }

        [Test] 
        public void FromJson_JsonNull_ReturnsNull()
        {
            //arrange
            var array = new CustomClass[]{new CustomClass(), new CustomClass(), new CustomClass()};

            //act
            array = FromJson(array, "null");

            //assert
            Assert.That(array, Is.Null);
        }

        [Test] 
        public void FromJson_ArrayNull_MakesArray()
        {
            //arrange
            //act
            var array = FromJson((CustomClass[])null, ExpectedJson);

            //assert
            Assert.That(array.Length, Is.EqualTo(3));
            Assert.That(array[0].Name, Is.EqualTo("William"));
            Assert.That(array[1], Is.Null);
            Assert.That(array[2].Name, Is.EqualTo("Susen"));
        }
    }
}