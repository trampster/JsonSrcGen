using NUnit.Framework;
using JsonSrcGen;
using System.Text;
using System;

namespace UnitTests
{
    [Json]
    public struct MixedJsonStruct
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public string NullProperty { get; set; }
        public bool IsTrue { get; set; }
    }

    [Json]
    public readonly ref struct MixedJsonRefStruct
    {
        public int Age { get; }
        public string Name { get; }
        public string NullProperty { get; }
        public bool IsTrue { get; }

        public MixedJsonRefStruct(int age, string name, string nullProperty, bool isTrue) : this()
        {
            NullProperty = nullProperty;
            Age = age;
            Name = name;
            IsTrue = isTrue;
        }
    }

    public class StructPropertiesTests : StructPropertiesTestsBase
    {
        protected override string ToJson(ref MixedJsonRefStruct jsonClass)
        {
            return _convert.ToJson(ref jsonClass).ToString();
        }

        protected override string ToJson(ref MixedJsonStruct jsonClass)
        {
            return _convert.ToJson(ref jsonClass).ToString();
        }

        protected override ReadOnlySpan<char> FromJson(ref MixedJsonStruct value, string json)
        {
            return _convert.FromJson(ref value, json);
        }
    }

    public class Utf8StructPropertiesTests : StructPropertiesTestsBase
    {
        protected override string ToJson(ref MixedJsonRefStruct jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(ref jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override string ToJson(ref MixedJsonStruct jsonClass)
        {
            var jsonUtf8 = _convert.ToJsonUtf8(ref jsonClass);
            return Encoding.UTF8.GetString(jsonUtf8);
        }

        protected override ReadOnlySpan<char> FromJson(ref MixedJsonStruct value, string json)
        {
            return Encoding.UTF8.GetString(_convert.FromJson(ref value, Encoding.UTF8.GetBytes(json)));
        }
    }

    public abstract class StructPropertiesTestsBase
    {
        protected JsonSrcGen.JsonConverter _convert;

        [SetUp]
        public void Setup()
        {
            _convert = new JsonConverter();
        }

        protected abstract string ToJson(ref MixedJsonStruct jsonClass);

        protected abstract string ToJson(ref MixedJsonRefStruct jsonClass);


        [Test]
        public void ToJson_CorrectString()
        {
            //arrange
            var jsonStruct = new MixedJsonStruct()
            {
                Age = 97,
                Name = "Jack",
                NullProperty = null,
                IsTrue = true
            };

            //act
            var json = ToJson(ref jsonStruct);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"Age\":97,\"IsTrue\":true,\"Name\":\"Jack\",\"NullProperty\":null}"));
        }

        [Test]
        public void ToJson_CorrectString_StructREF()
        {
            //arrange
            var jsonStruct = new MixedJsonRefStruct(97, "Jack", null, true);

            //act
            var json = ToJson(ref jsonStruct);

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"Age\":97,\"IsTrue\":true,\"Name\":\"Jack\",\"NullProperty\":null}"));
        }

        protected abstract ReadOnlySpan<char> FromJson(ref MixedJsonStruct value, string json);

        [Test]
        public void FromJson_CorrectJsonClass()
        {
            //arrange
            var json = "{\"Age\":97,\"IsTrue\":true,\"Name\":\"Jack\",\"NullProperty\":null}";
            var jsonStruct = new MixedJsonStruct();

            //act
            FromJson(ref jsonStruct, json);

            //assert
            Assert.That(jsonStruct.Age, Is.EqualTo(97));
            Assert.That(jsonStruct.Name, Is.EqualTo("Jack"));
            Assert.That(jsonStruct.NullProperty, Is.Null);
            Assert.That(jsonStruct.IsTrue, Is.True);
        }
    }
}