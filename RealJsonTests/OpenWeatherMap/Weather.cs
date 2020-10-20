using System;
using JsonSrcGen;

namespace JsonSrcGen.RealJsonTests.OpenWeatherMap
{
    [Json]
    public class Weather
    {
        [JsonName("lat")]
        public float Latitude {get;set;}

        [JsonName("long")]
        public float Longitude {get;set;}

        [JsonName("timezone")]
        public string Timezone {get;set;}

        [JsonName("timezone_offset")]
        public int TimezoneOffset {get;set;}
    }
}