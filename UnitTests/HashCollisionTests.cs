using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public class CollisionJsonClass
    {
        public string Aaa {get;set;}
        
        public string Abb {get;set;}

        public string Aab {get;set;}
    }

    public class HashCollisionTest : HashCollisionTestBase
    {
        protected override ReadOnlySpan<char> FromJson(CollisionJsonClass value, string json)
        {
            return _convert.FromJson(value, json);
        }
    }

    public class Utf8HashCollisionTest : HashCollisionTestBase
    {
        protected override ReadOnlySpan<char> FromJson(CollisionJsonClass value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class HashCollisionTestBase
    {
        protected JsonSrcGen.JsonConverter _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract ReadOnlySpan<char> FromJson(CollisionJsonClass value, string json);

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
            FromJson(jsonClass, json);

            //assert
            Assert.That(jsonClass.Aaa, Is.EqualTo("one"));
            Assert.That(jsonClass.Abb, Is.EqualTo("two"));
            Assert.That(jsonClass.Aab, Is.EqualTo("three"));
        }
    }
}