using NUnit.Framework;
using System.Text;
using System;
using System.Linq;
using System.Globalization;

namespace JsonSrcGen.Runtime.Tests
{
    public class GuidTests
    {
        JsonUtf8Builder _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new JsonUtf8Builder();
        }


        [Test]
        public void AppendShort_CorrectResult()
        {
            // arrange
            _builder.Clear();
            var guid = Guid.Parse("01234567-89AB-CDEF-0123-456789ABCEF0");

            // act
            _builder.Append(guid);

            // assert
            var bytes = _builder.AsSpan();
            Assert.That(Encoding.UTF8.GetString(bytes), Is.EqualTo(guid.ToString()));
        }
    }
}