using NUnit.Framework;
using System.Text;
using System;
using System.Linq;

namespace JsonSrcGen.Runtime.Tests
{
    public class Tests
    {
        JsonUtf8Builder _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new JsonUtf8Builder();
        }


        [Test]
        public void AppendShort_CorrectResult([Values(
            short.MinValue, -10000, -9999, -1000, -999, -100, -99, -10, -9, -1, 
            0, 1, 9, 10, 99, 100, 999, 1000, 9999, 10000 ,short.MaxValue)]short value)
        {
            // arrange
            _builder.Clear();

            // act
            _builder.Append(value);

            // assert
            var bytes = _builder.AsSpan();
            Assert.That(Encoding.UTF8.GetString(bytes), Is.EqualTo(value.ToString()));
        }

        [Test]
        public void AppendUShort_CorrectResult([Values(
            ushort.MinValue, (ushort)1, (ushort)9, (ushort)10, (ushort)99, (ushort)100, (ushort)999, (ushort)1000, (ushort)9999, (ushort)10000, ushort.MaxValue)]ushort value)
        {
            // arrange
            _builder.Clear();

            // act
            _builder.Append(value);

            // assert
            var bytes = _builder.AsSpan();
            Assert.That(Encoding.UTF8.GetString(bytes), Is.EqualTo(value.ToString()));
        }

        [Test]
        public void AppendByte_CorrectResult([Values(
            0, 1, 9, 10, 99, 100, byte.MaxValue)]byte value)
        {
            // arrange
            _builder.Clear();

            // act
            _builder.Append(value);

            // assert
            var bytes = _builder.AsSpan();
            Assert.That(Encoding.UTF8.GetString(bytes), Is.EqualTo(value.ToString()));
        }
    }
}