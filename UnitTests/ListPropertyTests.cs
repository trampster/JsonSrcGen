using NUnit.Framework;
using JsonSrcGen;
using System.Collections.Generic;


namespace UnitTests
{
    [Json]
    public class JsonListClass
    {
        public System.Collections.Generic.List<bool> BooleanList {get;set;} 
    }

    public class ListPropertyTests
    { 
        JsonSrcGen.JsonSrcGenConvert _convert;

        string ExpectedJson = $"{{\"BooleanList\":[true,false]}}";

        [SetUp]
        public void Setup()
        {
            _convert = new JsonSrcGenConvert();
        }

        [Test] 
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonClass = new JsonListClass()
            {
                BooleanList = new List<bool>(){true, false}
            };

            //act
            var json = _convert.ToJson(jsonClass);

            //assert
            Assert.That(json, Is.EqualTo(ExpectedJson));
        }

        [Test]
        public void ToJson_Null_CorrectString()
        {
            //arrange
            var jsonClass = new JsonListClass()
            {
                BooleanList = null
            };

            //act
            var json = _convert.ToJson(jsonClass);

            //assert
            Assert.That(json, Is.EqualTo("{\"BooleanList\":null}"));
        }

        [Test]
        public void FromJson_EmptyList_CorrectList()
        {
            //arrange
            var jsonClass = new JsonListClass()
            {
                BooleanList = new List<bool>()
            };

            //act
            _convert.FromJson(jsonClass, ExpectedJson);

            //assert
            Assert.That(jsonClass.BooleanList.Count, Is.EqualTo(2));
            Assert.That(jsonClass.BooleanList[0], Is.True);
            Assert.That(jsonClass.BooleanList[1], Is.False);
        }

        [Test]
        public void FromJson_NullList_CorrectList()
        {
            //arrange
            var jsonClass = new JsonListClass()
            {
                BooleanList = null
            };

            //act
            _convert.FromJson(jsonClass, ExpectedJson);

            //assert
            Assert.That(jsonClass.BooleanList.Count, Is.EqualTo(2));
            Assert.That(jsonClass.BooleanList[0], Is.True);
            Assert.That(jsonClass.BooleanList[1], Is.False);
        }

        [Test] 
        public void FromJson_PopulatedList_CorrectList()
        {
            //arrange
            var jsonClass = new JsonListClass()
            {
                BooleanList = new List<bool>(){false, false, false}
            };

            //act
            _convert.FromJson(jsonClass, ExpectedJson);

            //assert
            Assert.That(jsonClass.BooleanList.Count, Is.EqualTo(2));
            Assert.That(jsonClass.BooleanList[0], Is.True);
            Assert.That(jsonClass.BooleanList[1], Is.False);
        }
    }
}