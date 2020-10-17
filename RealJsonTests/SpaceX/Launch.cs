using System;
using JsonSrcGen;

namespace JsonSrcGen.RealJsonTests.SpaceX
{
    [Json]
    public class Launch
    {
        [JsonName("fairings")]
        public Fairings Fairings {get;set;}

        [JsonName("links")]
        public Links Links {get;set;}
  
        [JsonName("static_fire_date_utc")]
        public DateTime? StaticFireDateUtc {get;set;}

        [JsonName("static_fire_date_unix")]
        public long? StaticFireDateUnix {get;set;}

        [JsonName("tbd")]
        public bool Tbd {get;set;}

        [JsonName("net")]
        public bool Net {get;set;}

        [JsonName("window")]
        public int? Window {get;set;}

        [JsonName("rocket")]
        public string Rocket {get;set;}

        [JsonName("success")]
        public bool? Success {get;set;}

        [JsonName("failures")]
        public Failures[] Failures {get;set;}

        [JsonName("details")]
        public string Details {get;set;}

        [JsonName("crew")]
        public string[] Crew {get;set;}

        [JsonName("ships")]
        public string[] Ships {get;set;}

        [JsonName("capsules")]
        public string[] Capsules {get;set;}

        [JsonName("payloads")]
        public string[] Payloads {get;set;}

        [JsonName("launchpad")]
        public string Launchpad {get;set;}

        [JsonName("auto_update")]
        public bool AutoUpdate {get;set;}

        [JsonName("flight_number")]
        public int FlightNumber {get;set;}

        [JsonName("name")]
        public string Name {get;set;}

        [JsonName("date_utc")]
        public DateTime DateUtc {get;set;}
        
        [JsonName("date_unix")]
        public long DateUnix {get;set;}

        [JsonName("date_local")]
        public DateTimeOffset DateLocal {get;set;}

        [JsonName("date_precision")]
        public string DatePrecision {get;set;}

        [JsonName("upcoming")]
        public bool Upcoming {get;set;}

        [JsonName("cores")]
        public Core[] Cores {get;set;}

        [JsonName("id")]
        public string Id {get;set;}
    }

    [Json]
    public class Fairings
    {
        [JsonName("reused")]
        public bool? Reused {get;set;}

        [JsonName("recovery_attempt")]
        public bool? RecoveryAttempt {get;set;}

        [JsonName("recovered")]
        public bool? Recovered {get;set;}

        [JsonName("ships")]
        public string[] Ships {get;set;}
    }

    [Json]
    public class Failures
    {
        [JsonName("time")]
        public int Time {get;set;}

        [JsonName("altitude")]
        public int? Altitude {get;set;}

        [JsonName("reason")]
        public string Reason {get;set;}
    }

    [Json]
    public class Links
    {
        [JsonName("patch")]
        public Patch Patch {get;set;}

        [JsonName("reddit")]
        public Reddit Reddit {get;set;}

        [JsonName("flickr")]
        public Flicker Flickr {get;set;}

        [JsonName("presskit")]
        public string Presskit {get;set;}

        [JsonName("webcast")]
        public string Webcast {get;set;}

        [JsonName("youtube_id")]
        public string YoutubeId {get;set;}

        [JsonName("article")]
        public string Article {get;set;}

        [JsonName("wikipedia")]
        public string Wikipedia {get;set;}
    }

    [Json]
    public class Patch
    {
        [JsonName("small")]
        public string Small {get;set;}

        [JsonName("large")]
        public string Large {get;set;}
    }

    [Json]
    public class Reddit
    {
        [JsonName("campaign")]
        public string Campaign {get;set;}

        [JsonName("launch")]
        public string Launch {get;set;}

        [JsonName("media")]
        public string Media {get;set;}

        [JsonName("recovery")]
        public string Recovery {get;set;}
    }

    [Json]
    public class Flicker
    {
        [JsonName("small")]
        public string[] Small {get;set;}

        [JsonName("original")]
        public string[] Original {get;set;}
    }

    [Json]
    public class Core
    {
        [JsonName("core")]
        public string CoreId {get;set;}

        [JsonName("flight")]
        public int? Flight {get;set;}

        [JsonName("gridfins")]
        public bool? Gridfins {get;set;}

        [JsonName("legs")]
        public bool? Legs {get;set;}

        [JsonName("reused")]
        public bool? Reused {get;set;}

        [JsonName("landing_attempt")]
        public bool? LandingAttempt {get;set;}

        [JsonName("landing_success")]
        public bool? LandingSuccess {get;set;}

        [JsonName("landing_type")]
        public string LandingType {get;set;}

        [JsonName("landpad")]
        public string Landpad {get;set;}
    }
}