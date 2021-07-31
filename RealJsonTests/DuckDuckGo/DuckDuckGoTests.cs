using JsonSrcGen;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System;
using System.Text;

namespace JsonSrcGen.RealJsonTests.DuckDuckGo
{
    /// <summary>
    /// Test data retrieved from https://api.duckduckgo.com/?q=NewZealand&format=json&pretty=1
    /// </summary>
    public class DuckDuckGoTests
    {
        JsonConverter _converter; 

        const string _expectedText = "New Zealand is an island country in the southwestern Pacific Ocean. It consists of two main landmasses\u2014the North Island and the South Island\u2014and more than 700 smaller islands, covering a total area of 268,021 square kilometres. New Zealand is about 2,000 kilometres east of Australia across the Tasman Sea and 1,000 kilometres south of the islands of New Caledonia, Fiji, and Tonga. The country's varied topography and sharp mountain peaks, including the Southern Alps, owe much to tectonic uplift and volcanic eruptions. New Zealand's capital city is Wellington, and its most populous city is Auckland. Owing to their remoteness, the islands of New Zealand were the last large habitable lands to be settled by humans. Between about 1280 and 1350, Polynesians began to settle in the islands and then developed a distinctive M\u0101ori culture. In 1642, the Dutch explorer Abel Tasman became the first European to sight New Zealand.";

        [SetUp]
        public void Setup()
        {
            _converter = new JsonConverter();
        }

        ReadOnlySpan<char> FromJson(InstantAnswer instantAnswer, string json, bool utf8)
        {
            if(utf8)
            {
                return Encoding.UTF8.GetString(_converter.FromJson(instantAnswer, Encoding.UTF8.GetBytes(json)));
            }
            return _converter.FromJson(instantAnswer, json);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void FromJson_CorectProperties(bool utf8)
        {
            // arrange
            InstantAnswer instantAnswer = new InstantAnswer();
            var json = File.ReadAllText(Path.Combine("DuckDuckGo", "DuckDuckGo.json"));
            
            // act
            FromJson(instantAnswer, json, utf8);

            // assert
            Assert.That(instantAnswer.Abstract, Is.EqualTo(_expectedText));
            Assert.That(instantAnswer.AbstractText, Is.EqualTo(_expectedText));
            Assert.That(instantAnswer.Image, Is.EqualTo("/i/2523dee7.png"));
            Assert.That(instantAnswer.ImageHeight, Is.EqualTo(200));
            Assert.That(instantAnswer.ImageWidth, Is.EqualTo(400));
        }
    }
}