using System;
using System.Text;
using NUnit.Framework;

namespace JsonSrcGen.Input.Tests;

public class JsonUtf8BuilderTests
{
    [TestCase("1234é")] // last character takes two bytes
    [TestCase("123€")]  // last character takes three bytes
    [TestCase("123456")]  // last character takes three bytes
    /// <summary>
    /// The builder starts with a size of 5 bytes
    /// </summary>
    public void Append_StringWouldOverflowInitialSize_AppendedCorrectly(string twoBig)
    {
        // arrange
        var builder = new JsonUtf8Builder();

        // act
        builder.Append(twoBig);

        // assert
        var span = builder.AsSpan();

        Assert.That(Encoding.UTF8.GetString(span), Is.EqualTo(twoBig));
    }

    [TestCase("1234é")] // last character takes two bytes
    [TestCase("123€")]  // last character takes three bytes
    [TestCase("123456")]  // last character takes three bytes
    /// <summary>
    /// The builder starts with a size of 5 bytes
    /// </summary>
    public void Append_ReadOnlySpanCharWouldOverflowInitialSize_AppendedCorrectly(string twoBig)
    {
        // arrange
        var builder = new JsonUtf8Builder();

        // act
        builder.Append(twoBig.AsSpan());

        // assert
        var span = builder.AsSpan();

        Assert.That(Encoding.UTF8.GetString(span), Is.EqualTo(twoBig));
    }
}