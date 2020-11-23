using NUnit.Framework;
using JsonSrcGen;


namespace UnitTests
{
    [Json]
    public class CollisionJsonClass
    {
        public string Aaa {get;set;}
        
        public string Abb {get;set;}

        public string Aab {get;set;}
    }

    public class HashCollisionTest
    {
        JsonSrcGen.JsonConverter _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        /// <summary>
        /// The properties cannot be distingwished by length or by any indiviual column
        /// So a nested hash is required
        /// </summary>
        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = "{\"Aaa\":\"one\",\"Abb\":\"two\",\"Aab\":\"three\"}";
            var jsonClass = new CollisionJsonClass();

            //act
           JsonConverter.FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Aaa, Is.EqualTo("one"));
            Assert.That(jsonClass.Abb, Is.EqualTo("two"));
            Assert.That(jsonClass.Aab, Is.EqualTo("three"));
        }
    }
}