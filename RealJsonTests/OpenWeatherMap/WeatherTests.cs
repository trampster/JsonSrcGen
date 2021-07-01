using JsonSrcGen;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System;
using System.Text;

namespace JsonSrcGen.RealJsonTests.OpenWeatherMap
{
    /// <summary>
    /// Test data retrieved from https://api.openweathermap.org/data/2.5/onecall?lat=33.441792&lon=-94.037689&appid={apiKey}
    /// </summary>
    public class WeatherTests
    {
        JsonConverter _converter;

        [SetUp]
        public void Setup()
        {
            _converter = new JsonConverter();
        }

        ReadOnlySpan<char> FromJson(Weather weather, string json, bool utf8)
        {
            if(utf8)
            {
                return Encoding.UTF8.GetString(_converter.FromJson(weather, Encoding.UTF8.GetBytes(json)));
            }
            return _converter.FromJson(weather, json);
        }

        [Test]
        public void FromJson_CorrectTopLevel([Values("Weather.json", "WeatherFormatted.json")] string jsonFile, [Values(true, false)]bool utf8)
        {
            // arrange
            Weather weather = new Weather();
            var json = File.ReadAllText(Path.Combine("OpenWeatherMap", jsonFile));
            
            // act
            FromJson(weather, json, utf8);

            // assert
            Assert.That(weather.Latitude, Is.EqualTo(33.44f));
            Assert.That(weather.Longitude, Is.EqualTo(-94.04f));
            Assert.That(weather.Timezone, Is.EqualTo("America/Chicago"));
            Assert.That(weather.TimezoneOffset, Is.EqualTo(-18000));
        }

        [Test]
        public void FromJson_CorrectCurrent([Values("Weather.json", "WeatherFormatted.json")]string jsonFile, [Values(true, false)]bool utf8)
        {
            // arrange
            Weather weather = new Weather();
            var json = File.ReadAllText(Path.Combine("OpenWeatherMap", jsonFile));
            
            // act
            FromJson(weather, json, utf8);

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
            Assert.That(current.Weather.Length, Is.EqualTo(1));
            Assert.That(current.Weather[0].Id, Is.EqualTo(801));
            Assert.That(current.Weather[0].Main, Is.EqualTo("Clouds"));
            Assert.That(current.Weather[0].Description, Is.EqualTo("few clouds"));
            Assert.That(current.Weather[0].Icon, Is.EqualTo("02n"));
        }

        [Test]
        public void FromJson_MinutelyCorrect([Values("Weather.json", "WeatherFormatted.json")]string jsonFile, [Values(true, false)]bool utf8)
        {
            // arrange
            Weather weather = new Weather();
            var json = File.ReadAllText(Path.Combine("OpenWeatherMap", jsonFile));
            
            // act
            FromJson(weather, json, utf8);

            // assert
            var minutely = weather.Minutely;

            Assert.That(minutely.Length, Is.EqualTo(61));

            // first
            Assert.That(minutely[0].Dt, Is.EqualTo(1603185600));
            Assert.That(minutely[0].Precipitation, Is.EqualTo(0));

            Assert.That(minutely[2].Dt, Is.EqualTo(1603185720));
            Assert.That(minutely[2].Precipitation, Is.EqualTo(0));

            // last
            Assert.That(minutely[60].Dt, Is.EqualTo(1603189200));
            Assert.That(minutely[60].Precipitation, Is.EqualTo(0));
        }

        [Test]
        public void FromJson_HourlyCorrect([Values("Weather.json", "WeatherFormatted.json")]string jsonFile, [Values(true, false)]bool utf8)
        {
            // arrange
            Weather weather = new Weather();
            var json = File.ReadAllText(Path.Combine("OpenWeatherMap", jsonFile));
            
            // act
            FromJson(weather, json, utf8);

            // assert
            var hourly = weather.Hourly;

            Assert.That(hourly.Length, Is.EqualTo(48));

            // first
            Assert.That(hourly[0].Dt, Is.EqualTo(1603184400));
            Assert.That(hourly[0].Temp, Is.EqualTo(294.59f));
            Assert.That(hourly[0].FeelsLike, Is.EqualTo(296.32f));
            Assert.That(hourly[0].Pressure, Is.EqualTo(1017));
            Assert.That(hourly[0].Humidity, Is.EqualTo(88));
            Assert.That(hourly[0].DewPoint, Is.EqualTo(292.52f));
            Assert.That(hourly[0].Clouds, Is.EqualTo(20));
            Assert.That(hourly[0].Visibility, Is.EqualTo(10000));
            Assert.That(hourly[0].WindSpeed, Is.EqualTo(2.39f));
            Assert.That(hourly[0].WindDeg, Is.EqualTo(184));
            Assert.That(hourly[0].Weather.Length, Is.EqualTo(1));
            Assert.That(hourly[0].Weather[0].Id, Is.EqualTo(501));
            Assert.That(hourly[0].Weather[0].Main, Is.EqualTo("Rain"));
            Assert.That(hourly[0].Weather[0].Description, Is.EqualTo("moderate rain"));
            Assert.That(hourly[0].Weather[0].Icon, Is.EqualTo("10n"));
            Assert.That(hourly[0].Pop, Is.EqualTo(0.88f));
            Assert.That(hourly[0].Rain.OneHour, Is.EqualTo(1.53f));

            // // last
            Assert.That(hourly[47].Dt, Is.EqualTo(1603353600));
            Assert.That(hourly[47].Temp, Is.EqualTo(292.79f));
            Assert.That(hourly[47].FeelsLike, Is.EqualTo(293.72f));
            Assert.That(hourly[47].Pressure, Is.EqualTo(1017));
            Assert.That(hourly[47].Humidity, Is.EqualTo(90));
            Assert.That(hourly[47].DewPoint, Is.EqualTo(291.13f));
            Assert.That(hourly[47].Clouds, Is.EqualTo(0));
            Assert.That(hourly[47].Visibility, Is.EqualTo(10000));
            Assert.That(hourly[47].WindSpeed, Is.EqualTo(2.64f));
            Assert.That(hourly[47].WindDeg, Is.EqualTo(153));
            Assert.That(hourly[47].Weather.Length, Is.EqualTo(1));
            Assert.That(hourly[47].Weather[0].Id, Is.EqualTo(800));
            Assert.That(hourly[47].Weather[0].Main, Is.EqualTo("Clear"));
            Assert.That(hourly[47].Weather[0].Description, Is.EqualTo("clear sky"));
            Assert.That(hourly[47].Weather[0].Icon, Is.EqualTo("01n"));
            Assert.That(hourly[47].Pop, Is.EqualTo(0f));
            Assert.That(hourly[47].Rain, Is.Null);
        }

        [Test]
        public void FromJson_DailyCorrect([Values("Weather.json", "WeatherFormatted.json")]string jsonFile, [Values(true, false)]bool utf8)
        {
            // arrange
            Weather weather = new Weather();
            var json = File.ReadAllText(Path.Combine("OpenWeatherMap", jsonFile));
            
            // act
            FromJson(weather, json, utf8);

            // assert
            var daily = weather.Daily;

            Assert.That(daily.Length, Is.EqualTo(8));

            // first
            Assert.That(daily[1].Dt, Is.EqualTo(1603303200));
            Assert.That(daily[1].Sunrise, Is.EqualTo(1603283148));
            Assert.That(daily[1].Sunset, Is.EqualTo(1603323315));
            Assert.That(daily[1].Temp.Day, Is.EqualTo(301.18f));
            Assert.That(daily[1].Temp.Min, Is.EqualTo(292f));
            Assert.That(daily[1].Temp.Max, Is.EqualTo(301.83f));
            Assert.That(daily[1].Temp.Night, Is.EqualTo(294.68f));
            Assert.That(daily[1].Temp.Eve, Is.EqualTo(297.13f));
            Assert.That(daily[1].Temp.Morn, Is.EqualTo(292f));
            Assert.That(daily[1].FeelsLike.Day, Is.EqualTo(302.18f));
            Assert.That(daily[1].FeelsLike.Night, Is.EqualTo(295.72f));
            Assert.That(daily[1].FeelsLike.Eve, Is.EqualTo(298.7f));
            Assert.That(daily[1].FeelsLike.Morn, Is.EqualTo(293.79f));
            Assert.That(daily[1].Pressure, Is.EqualTo(1018));
            Assert.That(daily[1].Humidity, Is.EqualTo(58));
            Assert.That(daily[1].DewPoint, Is.EqualTo(292.4f));
            Assert.That(daily[1].WindSpeed, Is.EqualTo(3.18f));
            Assert.That(daily[1].WindDeg, Is.EqualTo(155f));
            Assert.That(daily[1].Weather.Length, Is.EqualTo(1));
            Assert.That(daily[1].Weather[0].Id, Is.EqualTo(800));
            Assert.That(daily[1].Weather[0].Main, Is.EqualTo("Clear"));
            Assert.That(daily[1].Weather[0].Description, Is.EqualTo("clear sky"));
            Assert.That(daily[1].Weather[0].Icon, Is.EqualTo("01d"));
            Assert.That(daily[1].Clouds, Is.EqualTo(0));
            Assert.That(daily[1].Pop, Is.EqualTo(0));
            Assert.That(daily[1].Uvi, Is.EqualTo(4.92f));
        }

        ReadOnlySpan<char> ToJson(Weather weather, bool utf8)
        {
            if(utf8)
            {
                return Encoding.UTF8.GetString(_converter.ToJsonUtf8(weather));
            }
            return _converter.ToJson(weather);
        }

        /// <summary>
        /// This has been reduce for the orignal by removing most of the entries
        /// in the arrays, also reordered because JsonSrcGen produces alphabetically
        /// sorted json properties and null entries added because JsonSrcGen outputs
        /// nulls
        /// </summary>
        [Test]
        public void ToJsonReduced_CorrectJson([Values(true, false)]bool utf8)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-us");
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;

            // arrange
            Weather weather = new Weather()
            {
                Latitude = 33.44f,
                Longitude = -94.04f,
                Timezone = "America/Chicago",
                TimezoneOffset = -18000,
                Current = new WeatherEntry
                {
                    Dt = 1603185587,
                    Sunrise = 1603196699,
                    Sunset = 1603236983,
                    Temp = 294.59f,
                    FeelsLike = 296.17f,
                    Pressure = 1017,
                    Humidity = 88,
                    DewPoint = 292.52f,
                    Uvi = 5.21f,
                    Clouds = 20,
                    Visibility = 10000,
                    WindSpeed = 2.6f,
                    WindDeg = 150,
                    Weather = new WeatherDescription[]
                    {
                        new WeatherDescription()
                        {
                            Id = 801,
                            Main = "Clouds",
                            Description = "few clouds",
                            Icon = "02n"
                        }
                    }
                },
                Minutely = new MinutelyWeather[]
                {
                    new MinutelyWeather()
                    {
                        Dt = 1603185600,
                        Precipitation = 0
                    },
                    new MinutelyWeather()
                    {
                        Dt = 1603185660,
                        Precipitation = 0
                    }
                },
                Hourly = new WeatherEntry[]
                {
                    new WeatherEntry()
                    {
                        Dt = 1603184400,
                        Temp = 294.59f,
                        FeelsLike = 296.32f,
                        Pressure = 1017,
                        Humidity = 88,
                        DewPoint = 292.52f,
                        Clouds = 20,
                        Visibility = 10000,
                        WindSpeed = 2.39f,
                        WindDeg = 184,
                        Weather = new WeatherDescription[]
                        {
                            new WeatherDescription()
                            {
                                Id = 501,
                                Main = "Rain",
                                Description = "moderate rain",
                                Icon = "10n"
                            }
                        },
                        Pop = 0.88f,
                        Rain = new Rain()
                        {
                            OneHour = 1.53f
                        }
                    }
                },
                Daily = new DailyWeatherEntry[]
                {
                    new DailyWeatherEntry()
                    {
                        Dt = 1603216800,
                        Sunrise = 1603196699,
                        Sunset = 1603236983,
                        Temp = new DailyTemp()
                        {
                            Day = 299.61f,
                            Min = 293.56f,
                            Max = 301.32f,
                            Night = 294.58f,
                            Eve = 296.75f,
                            Morn = 293.79f
                        },
                        FeelsLike = new DailyTemp()
                        {
                            Day = 300.68f,
                            Night = 295.04f,
                            Eve = 298.05f,
                            Morn = 295.7f
                        },
                        Pressure = 1018,
                        Humidity = 63,
                        DewPoint = 292,
                        WindSpeed = 2.99f,
                        WindDeg = 195,
                        Weather = new WeatherDescription[]
                        {
                            new WeatherDescription()
                            {
                                Id = 501,
                                Main = "Rain",
                                Description = "moderate rain",
                                Icon = "10d"
                            }
                        },
                        Clouds = 90,
                        Pop = 0.88f,
                        Rain = 8.15f,
                        Uvi = 5.21f
                    }
                }
            };

            // act
            var json = ToJson(weather, utf8);

            // assert
            var jsonExpected = File.ReadAllText(Path.Combine("OpenWeatherMap", "WeatherToJson.json"));

            Assert.That(json.ToString(), Is.EqualTo(jsonExpected));
        }
    }
}