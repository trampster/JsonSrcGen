using System;
using JsonSrcGen;

namespace JsonSrcGen.RealJsonTests.OpenWeatherMap
{
    [Json]
    public class Weather
    {
        [JsonName("lat")]
        public float Latitude {get;set;}

        [JsonName("lon")]
        public float Longitude {get;set;}

        [JsonName("timezone")]
        public string Timezone {get;set;}

        [JsonName("timezone_offset")]
        public int TimezoneOffset {get;set;}

        [JsonName("current")]
        public WeatherEntry Current {get;set;}

        [JsonName("minutely")]
        public WeatherEntry[] Minutely {get;set;}

        [JsonName("hourly")]
        public WeatherEntry[] Hourly {get;set;}

        [JsonName("daily")]
        public DailyWeatherEntry[] Daily {get;set;}
    }

    [Json]
    public class WeatherEntry
    {
        [JsonName("dt")]
        public ulong Dt {get;set;}

        [JsonName("precipitation")]
        public ulong? Precipitation {get;set;}

        [JsonName("sunrise")]
        public ulong? Sunrise {get;set;}

        [JsonName("sunset")]
        public ulong? Sunset {get;set;}

        [JsonName("temp")]
        public float? Temp {get;set;}

        [JsonName("feels_like")]
        public float? FeelsLike {get;set;}

        [JsonName("pressure")]
        public int? Pressure {get;set;}

        [JsonName("humidity")]
        public int? Humidity {get;set;}

        [JsonName("dew_point")]
        public float? DewPoint {get;set;}

        [JsonName("uvi")]
        public float? Uvi {get;set;}

        [JsonName("clouds")]
        public int? Clouds {get;set;}

        [JsonName("visibility")]
        public int? Visibility {get;set;}

        [JsonName("wind_speed")]
        public float? WindSpeed {get;set;}

        [JsonName("wind_deg")]
        public float? WindDeg {get;set;}

        [JsonName("weather")]
        public WeatherDescription[] Weather {get;set;}

        [JsonName("pop")]
        public float? Pop {get;set;}

        [JsonName("rain")]
        public Rain? Rain {get;set;}
    }

    [Json]
    public class DailyTemp
    {
        [JsonName("day")]
        public float Day {get;set;}

        [JsonName("min")]
        public float? Min {get;set;}

        [JsonName("Max")]
        public float? Max {get;set;}

        [JsonName("night")]
        public float Night {get;set;}

        [JsonName("eve")]
        public float Eve {get;set;}

        [JsonName("morn")]
        public float Morn {get;set;}
    }

    [Json]
    public class DailyWeatherEntry
    {
        [JsonName("dt")]
        public ulong Dt {get;set;}

        [JsonName("precipitation")]
        public ulong? Precipitation {get;set;}

        [JsonName("sunrise")]
        public ulong? Sunrise {get;set;}

        [JsonName("sunset")]
        public ulong? Sunset {get;set;}

        [JsonName("temp")]
        public DailyTemp Temp {get;set;}

        [JsonName("feels_like")]
        public DailyTemp FeelsLike {get;set;}

        [JsonName("pressure")]
        public int? Pressure {get;set;}

        [JsonName("humidity")]
        public int? Humidity {get;set;}

        [JsonName("dew_point")]
        public float? DewPoint {get;set;}

        [JsonName("uvi")]
        public float? Uvi {get;set;}

        [JsonName("clouds")]
        public int? Clouds {get;set;}

        [JsonName("visibility")]
        public int? Visibility {get;set;}

        [JsonName("wind_speed")]
        public float? WindSpeed {get;set;}

        [JsonName("wind_deg")]
        public float? WindDeg {get;set;}

        [JsonName("weather")]
        public WeatherDescription[] Weather {get;set;}

        [JsonName("pop")]
        public float? Pop {get;set;}

        [JsonName("rain")]
        public float? Rain {get;set;}
    }

    [Json]
    public class Rain
    {
        [JsonName("1h")]
        public float OneHour {get;set;}
    }

    [Json]
    public class WeatherDescription
    {
        [JsonName("id")]
        public int Id {get;set;}

        [JsonName("main")]
        public string Main {get;set;}

        [JsonName("description")]
        public string Description {get;set;}

        [JsonName("icon")]
        public string Icon {get;set;}
    }
}