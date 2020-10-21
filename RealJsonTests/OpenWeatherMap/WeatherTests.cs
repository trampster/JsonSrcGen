using JsonSrcGen;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System;

namespace JsonSrcGen.RealJsonTests.OpenWeatherMap
{
    /// <summary>
    /// Test data retrieved from https://api.openweathermap.org/data/2.5/onecall?lat=33.441792&lon=-94.037689&appid={apiKey}
    /// </summary>
    public class WeatherTests
    {
        string _json;
        JsonConverter _converter;

        [SetUp]
        public void Setup()
        {
            _json = File.ReadAllText(Path.Combine("OpenWeatherMap","Weather.json"));
            _converter = new JsonConverter();
        }

        [Test]
        public void FromJson_CorrectTopLevel()
        {
            // arrange
            Weather weather = new Weather();
            
            // act
            _converter.FromJson(weather, _json);

            // assert
            Assert.That(weather.Latitude, Is.EqualTo(33.44f));
            Assert.That(weather.Longitude, Is.EqualTo(-94.04f));
            Assert.That(weather.Timezone, Is.EqualTo("America/Chicago"));
            Assert.That(weather.TimezoneOffset, Is.EqualTo(-18000));
        }

        [Test]
        public void FromJson_CorrectCurrent()
        {
            // arrange
            Weather weather = new Weather();
            
            // act
            _converter.FromJson(weather, _json);

            // assert
            var current = weather.Current;
            Assert.That(current.Dt, Is.EqualTo(1603185587));
            Assert.That(current.Sunrise, Is.EqualTo(1603196699));
            Assert.That(current.Temp, Is.EqualTo(294.59f));
            Assert.That(current.FeelsLike, Is.EqualTo(296.17f));
            Assert.That(current.Pressure, Is.EqualTo(1017));
            Assert.That(current.Humidity, Is.EqualTo(88));
            Assert.That(current.DewPoint, Is.EqualTo(292.52f));
            Assert.That(current.Uvi, Is.EqualTo(5.21f));
            Assert.That(current.Clouds, Is.EqualTo(20));
            Assert.That(current.Visibility, Is.EqualTo(10000));
            Assert.That(current.WindSpeed, Is.EqualTo(2.6f));
            Assert.That(current.WindDeg, Is.EqualTo(150));
            Assert.That(current.Weather.Id, Is.EqualTo(801));
            Assert.That(current.Weather.Main, Is.EqualTo("Clouds"));
            Assert.That(current.Weather.Description, Is.EqualTo("few clouds"));
            Assert.That(current.Weather.Icon, Is.EqualTo("02n"));
        }
    }
}