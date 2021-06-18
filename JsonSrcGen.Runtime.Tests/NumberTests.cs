using NUnit.Framework;
using System.Text;
using System.Globalization;

namespace JsonSrcGen.Runtime.Tests
{
    public class NumberTests
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

        [Test]
        public void AppendInt_CorrectResult([Values(
            int.MinValue, (int)-1_000_000_000, (int)-999_999_999, -100_000_000, -99_999_999, -10_000_000, -9_999_999, 
            -1_000_000, -999_999, -100_000, -99_999, -10000, -9999, -1000, -999, -100, -99, -10, -9, -1, 
            0, 1, 9, 10, 99, 100, 999, 1_000, 9_999, 10_000, 99_999, 100_000, 999_999, 1_000_000, 9_999_999, 
            10_000_000, 99_999_999, 100_000_000, (int)999_999_999, (int)1_000_000_000, int.MaxValue)]int value)
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
        public void AppendUInt_CorrectResult([Values(
            uint.MinValue, 1u, 9u, 10u, 99u, 100u, 999u, 1_000u, 9_999u, 10_000u, 99_999u, 100_000u, 999_999u, 
            1_000_000u, 9_999_999u, 10_000_000u, 99_999_999u, 100_000_000u, 999_999_999u, 1_000_000_000u, uint.MaxValue)]uint value)
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
        public void AppendLong_CorrectResult([Values(
            long.MinValue, -1_000_000_000_000_000_000, -999_999_999_999_999_999, -100_000_000_000_000_000, 
            -99_999_999_999_999_999, -10_000_000_000_000_000, -9_999_999_999_999_999, -1_000_000_000_000_000, 
            -999_999_999_999_999, -100_000_000_000_000, -99_999_999_999_999,
             -1_000_000_000_000, -999_999_999_999, -100_000_000_000, -99_999_999_999, -10_000_000_000, 
            -9_999_999_999, -1_000_000_000, -999_999_999, -100_000_000, -99_999_999, -10_000_000, -9_999_999, 
            -1_000_000, -999_999, -100_000, -99_999, -10000, -9999, -1000, -999, -100, -99, -10, -9, -1, 
            0, 1, 9, 10, 99, 100, 999, 1_000, 9_999, 10_000, 99_999, 100_000, 999_999, 1_000_000, 9_999_999, 
            10_000_000, 99_999_999, 100_000_000, 999_999_999, 1_000_000_000, 9_999_999_999, 10_000_000_000,
            99_999_999_999, 100_000_000_000, 999_999_999_999, 1_000_000_000_000, 9_999_999_999_999, 10_000_000_000_000, 
            99_999_999_999_999, 100_000_000_000_000, 999_999_999_999_999, 1_000_000_000_000_000, 
            9_999_999_999_999_999, 10_000_000_000_000_000, 99_999_999_999_999_999, 100_000_000_000_000_000, 
            999_999_999_999_999_999, 1_000_000_000_000_000_000, long.MaxValue)]long value)
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
        public void AppendLong_CorrectResult([Values(
            ulong.MinValue, 1u, 9u, 10u, 99u, 100u, 999u, 1_000u, 9_999u, 10_000u, 99_999u, 100_000u, 999_999u, 1_000_000u, 9_999_999u, 
            10_000_000u, 99_999_999u, 100_000_000u, 999_999_999u, 1_000_000_000u, 9_999_999_999u, 10_000_000_000u,
            99_999_999_999u, 100_000_000_000u, 999_999_999_999u, 1_000_000_000_000u, 9_999_999_999_999u, 10_000_000_000_000u, 
            99_999_999_999_999u, 100_000_000_000_000u, 999_999_999_999_999u, 1_000_000_000_000_000u, 
            9_999_999_999_999_999u, 10_000_000_000_000_000u, 99_999_999_999_999_999u, 100_000_000_000_000_000u, 
            999_999_999_999_999_999u, 1_000_000_000_000_000_000u, ulong.MaxValue)]ulong value)
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
        public void AppendFloat_CorrectResult([Values(
            1, 12, 123, 1234, 12345, 123456, 1234567, 123456789, 1234567891,
            -1, -12, -123, -1234, -12345, -123456, -1234567, -123456789, -1234567891,
            0.1f, 0.5f, 0.00123f, 0.00009f, 0.00000123f, 54.905f, 42.21f,
            float.MaxValue, float.MinValue, 0)]float value)
        {
            // arrange
            _builder.Clear();

            // act
            _builder.Append(value);

            // assert
            var bytes = _builder.AsSpan();
            var resultString = Encoding.UTF8.GetString(bytes);
            var resultFloat = float.Parse(resultString, provider: CultureInfo.InvariantCulture);

            var expectedString = value.ToString(CultureInfo.InvariantCulture);
            var expectedFloat = float.Parse(expectedString, provider: CultureInfo.InvariantCulture);

            Assert.That(resultFloat, Is.EqualTo(expectedFloat));
        }

        [Test]
        public void AppendDouble_CorrectResult([Values(123456789, 0.1f, 0.00000123f, 0.00123f,
            123456789, 1, 12, 123, 1234, 12345, 123456, 1234567, 1234567891,
            -1, -12, -123, -1234, -12345, -123456, -1234567, -123456789, -1234567891,
            0.1f, 0.5f, 0.00123f, 0.00009f, 0.00000123f, 54.905f, 42.21f,
            float.MaxValue, float.MinValue, double.MaxValue, double.MinValue, 0)]double value)
        {
            // arrange
            _builder.Clear();

            // act
            _builder.Append(value);

            // assert
            var bytes = _builder.AsSpan();
            var resultString = Encoding.UTF8.GetString(bytes);
            var resultFloat = double.Parse(resultString, provider: CultureInfo.InvariantCulture);

            var expectedString = value.ToString(CultureInfo.InvariantCulture);
            var expectedFloat = double.Parse(expectedString, provider: CultureInfo.InvariantCulture);

            Assert.That(resultFloat, Is.EqualTo(expectedFloat));
        }

        [Test]
        public void AppendDecimal_CorrectResult([Values(
            123456789, 1, 12, 123, 1234, 12345, 123456, 1234567, 1234567891,
            -1, -12, -123, -1234, -12345, -123456, -1234567, -123456789, -1234567891,
            0.1f, 0.5f, 0.00123f, 0.00009f, 0.00000123f, 54.905f, 42.21f, 0)]double doubleValue)
        {
            // arrange
            var value = (decimal)doubleValue;
            _builder.Clear();

            // act
            _builder.Append(value);

            // assert
            var bytes = _builder.AsSpan();
            var resultString = Encoding.UTF8.GetString(bytes);
            var resultFloat = double.Parse(resultString, provider: CultureInfo.InvariantCulture);

            var expectedString = value.ToString(CultureInfo.InvariantCulture);
            var expectedFloat = double.Parse(expectedString, provider: CultureInfo.InvariantCulture);

            Assert.That(resultFloat, Is.EqualTo(expectedFloat));
        }

        [Test]
        public void AppendDecimal_MaxValue_CorrectResult()
        {
            // arrange
            var value = decimal.MaxValue;
            _builder.Clear();

            // act
            _builder.Append(value);

            // assert
            var bytes = _builder.AsSpan();
            var resultString = Encoding.UTF8.GetString(bytes);
            var resultFloat = double.Parse(resultString, provider: CultureInfo.InvariantCulture);

            var expectedString = value.ToString(CultureInfo.InvariantCulture);
            var expectedFloat = double.Parse(expectedString, provider: CultureInfo.InvariantCulture);

            Assert.That(resultFloat, Is.EqualTo(expectedFloat));
        }

        [Test]
        public void AppendDecimal_MinValue_CorrectResult()
        {
            // arrange
            var value = decimal.MinValue;
            _builder.Clear();

            // act
            _builder.Append(value);

            // assert
            var bytes = _builder.AsSpan();
            var resultString = Encoding.UTF8.GetString(bytes);
            var resultFloat = double.Parse(resultString, provider: CultureInfo.InvariantCulture);

            var expectedString = value.ToString(CultureInfo.InvariantCulture);
            var expectedFloat = double.Parse(expectedString, provider: CultureInfo.InvariantCulture);

            Assert.That(resultFloat, Is.EqualTo(expectedFloat));
        }
    }
}