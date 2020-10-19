using JsonSrcGen;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System;

[assembly: JsonArray(typeof(JsonSrcGen.RealJsonTests.SpaceX.Launch))]
[assembly: GenerationOutputFolder("/home/daniel/Work/JsonSrcGen/Generated")]

namespace JsonSrcGen.RealJsonTests.SpaceX
{
    /// <summary>
    /// Test data retrieved from https://api.spacexdata.com/v4/launches
    /// </summary>
    public class LaunchesTests
    {
        string _json;
        JsonConverter _converter;

        [SetUp]
        public void Setup()
        {
            _json = File.ReadAllText(Path.Combine("SpaceX","Launches.json"));
            _converter = new JsonConverter();
        }

        [Test]
        public void FromJson_CorrectCount()
        {
            // arrange
            // act
            var launches = _converter.FromJson((Launch[])null, _json);

            // assert
            Assert.That(launches.Length, Is.EqualTo(113));
        }

        [Test]
        public void FromJson_CorrectFirstItem()
        {
            // arrange
            // act
            var launches = _converter.FromJson((Launch[])null, _json);

            // assert
            Launch launch = launches[0];
            Assert.That(launch.Fairings.Reused, Is.EqualTo(false));
            Assert.That(launch.Fairings.RecoveryAttempt, Is.EqualTo(false));
            Assert.That(launch.Fairings.Recovered, Is.EqualTo(false));
            Assert.That(launch.Fairings.Ships.Length, Is.EqualTo(0));
            Assert.That(launch.Links.Patch.Small, Is.EqualTo("https://images2.imgbox.com/3c/0e/T8iJcSN3_o.png"));
            Assert.That(launch.Links.Patch.Large, Is.EqualTo("https://images2.imgbox.com/40/e3/GypSkayF_o.png"));
            Assert.That(launch.Links.Reddit.Campaign, Is.Null);
            Assert.That(launch.Links.Reddit.Launch, Is.Null);
            Assert.That(launch.Links.Reddit.Media, Is.Null);
            Assert.That(launch.Links.Reddit.Recovery, Is.Null);
            Assert.That(launch.Links.Flickr.Small.Length, Is.EqualTo(0));
            Assert.That(launch.Links.Flickr.Original.Length, Is.EqualTo(0));
            Assert.That(launch.Links.Presskit, Is.Null);
            Assert.That(launch.Links.Webcast, Is.EqualTo("https://www.youtube.com/watch?v=0a_00nJ_Y88"));
            Assert.That(launch.Links.YoutubeId, Is.EqualTo("0a_00nJ_Y88"));
            Assert.That(launch.Links.Article, Is.EqualTo("https://www.space.com/2196-spacex-inaugural-falcon-1-rocket-lost-launch.html"));
            Assert.That(launch.Links.Wikipedia, Is.EqualTo("https://en.wikipedia.org/wiki/DemoSat"));
            Assert.That(launch.StaticFireDateUtc.Value.ToString("o"), Is.EqualTo("2006-03-17T00:00:00.0000000Z"));
            Assert.That(launch.StaticFireDateUnix, Is.EqualTo(1142553600));
            Assert.That(launch.Tbd, Is.False);
            Assert.That(launch.Net, Is.False);
            Assert.That(launch.Window, Is.EqualTo(0));
            Assert.That(launch.Rocket, Is.EqualTo("5e9d0d95eda69955f709d1eb"));
            Assert.That(launch.Success, Is.False);
            Assert.That(launch.Details, Is.EqualTo("Engine failure at 33 seconds and loss of vehicle"));
            Assert.That(launch.Crew.Length, Is.EqualTo(0));
            Assert.That(launch.Ships.Length, Is.EqualTo(0));
            Assert.That(launch.Capsules.Length, Is.EqualTo(0));
            Assert.That(launch.Payloads.Length, Is.EqualTo(1));
            Assert.That(launch.Payloads[0], Is.EqualTo("5eb0e4b5b6c3bb0006eeb1e1"));
            Assert.That(launch.Launchpad, Is.EqualTo("5e9e4502f5090995de566f86"));
            Assert.That(launch.AutoUpdate, Is.True);
            Assert.That(launch.Failures.Length, Is.EqualTo(1));
            Assert.That(launch.Failures[0].Time, Is.EqualTo(33));
            Assert.That(launch.Failures[0].Altitude, Is.Null);
            Assert.That(launch.Failures[0].Reason, Is.EqualTo("merlin engine failure"));
            Assert.That(launch.FlightNumber, Is.EqualTo(1));
            Assert.That(launch.Name, Is.EqualTo("FalconSat"));
            Assert.That(launch.DateUtc.ToString("o"), Is.EqualTo("2006-03-24T22:30:00.0000000Z"));
            Assert.That(launch.DateUnix, Is.EqualTo(1143239400));
            Assert.That(launch.DateLocal.ToString("yyyy-MM-ddTHH\\:mm\\:sszzz"), Is.EqualTo("2006-03-25T10:30:00+12:00"));
            Assert.That(launch.DatePrecision, Is.EqualTo("hour"));
            Assert.That(launch.Upcoming, Is.False);
            Assert.That(launch.Cores.Length, Is.EqualTo(1));
            Assert.That(launch.Cores[0].CoreId, Is.EqualTo("5e9e289df35918033d3b2623"));
            Assert.That(launch.Cores[0].Flight, Is.EqualTo(1));
            Assert.That(launch.Cores[0].Gridfins, Is.False);
            Assert.That(launch.Cores[0].Legs, Is.False);
            Assert.That(launch.Cores[0].Reused, Is.False);
            Assert.That(launch.Cores[0].LandingAttempt, Is.False);
            Assert.That(launch.Cores[0].LandingSuccess, Is.Null);
            Assert.That(launch.Cores[0].LandingType, Is.Null);
            Assert.That(launch.Cores[0].Landpad, Is.Null);
            Assert.That(launch.Id, Is.EqualTo("5eb87cd9ffd86e000604b32a"));
        }

         [Test]
        public void FromJson_CorrectLastItem()
        {
            // arrange
            // act
            var launches = _converter.FromJson((Launch[])null, _json);

            // assert
            Launch launch = launches.Last();
            Assert.That(launch.Fairings.Reused, Is.Null);
            Assert.That(launch.Fairings.RecoveryAttempt, Is.Null);
            Assert.That(launch.Fairings.Recovered, Is.Null);
            Assert.That(launch.Fairings.Ships.Length, Is.EqualTo(0));
            Assert.That(launch.Links.Patch.Small, Is.Null);
            Assert.That(launch.Links.Patch.Large, Is.Null);
            Assert.That(launch.Links.Reddit.Campaign, Is.EqualTo("https://www.reddit.com/r/spacex/comments/j7qqbg/nrol108_launch_campaign_thread/"));
            Assert.That(launch.Links.Reddit.Launch, Is.Null);
            Assert.That(launch.Links.Reddit.Media, Is.Null);
            Assert.That(launch.Links.Reddit.Recovery, Is.Null);
            Assert.That(launch.Links.Flickr.Small.Length, Is.EqualTo(0));
            Assert.That(launch.Links.Flickr.Original.Length, Is.EqualTo(0));
            Assert.That(launch.Links.Presskit, Is.Null);
            Assert.That(launch.Links.Webcast, Is.Null);
            Assert.That(launch.Links.YoutubeId, Is.Null);
            Assert.That(launch.Links.Article, Is.Null);
            Assert.That(launch.Links.Wikipedia, Is.EqualTo("https://en.wikipedia.org/wiki/National_Reconnaissance_Office"));
            Assert.That(launch.StaticFireDateUtc.HasValue, Is.False);
            Assert.That(launch.StaticFireDateUnix.HasValue, Is.False);
            Assert.That(launch.Tbd, Is.False);
            Assert.That(launch.Net, Is.False);
            Assert.That(launch.Window, Is.Null);
            Assert.That(launch.Rocket, Is.EqualTo("5e9d0d95eda69973a809d1ec"));
            Assert.That(launch.Success, Is.Null);
            Assert.That(launch.Details, Is.EqualTo("SpaceX will launch NROL-108 for the National Reconnaissance Office aboard a Falcon 9 from SLC-40, Cape Canaveral Air Force Station. The booster for this mission is expected to land at LZ-1."));
            Assert.That(launch.Crew.Length, Is.EqualTo(0));
            Assert.That(launch.Ships.Length, Is.EqualTo(0));
            Assert.That(launch.Capsules.Length, Is.EqualTo(0));
            Assert.That(launch.Payloads.Length, Is.EqualTo(1));
            Assert.That(launch.Payloads[0], Is.EqualTo("5f839ac7818d8b59f5740d48"));
            Assert.That(launch.Launchpad, Is.EqualTo("5e9e4501f509094ba4566f84"));
            Assert.That(launch.AutoUpdate, Is.True);
            Assert.That(launch.Failures.Length, Is.EqualTo(0));
            Assert.That(launch.FlightNumber, Is.EqualTo(106));
            Assert.That(launch.Name, Is.EqualTo("NROL-108"));
            Assert.That(launch.DateUtc.ToString("o"), Is.EqualTo("2020-10-01T00:00:00.0000000Z"));
            Assert.That(launch.DateUnix, Is.EqualTo(1601510400));
            Assert.That(launch.DateLocal.ToString("o"), Is.EqualTo("2020-09-30T20:00:00.0000000-04:00"));
            Assert.That(launch.DatePrecision, Is.EqualTo("month"));
            Assert.That(launch.Upcoming, Is.True);
            Assert.That(launch.Cores.Length, Is.EqualTo(1));
            Assert.That(launch.Cores[0].CoreId, Is.Null);
            Assert.That(launch.Cores[0].Flight, Is.EqualTo(5));
            Assert.That(launch.Cores[0].Gridfins, Is.True);
            Assert.That(launch.Cores[0].Legs, Is.True);
            Assert.That(launch.Cores[0].Reused, Is.True);
            Assert.That(launch.Cores[0].LandingAttempt, Is.True);
            Assert.That(launch.Cores[0].LandingSuccess, Is.Null);
            Assert.That(launch.Cores[0].LandingType, Is.EqualTo("RTLS"));
            Assert.That(launch.Cores[0].Landpad, Is.EqualTo("5e9e3032383ecb267a34e7c7"));
            Assert.That(launch.Id, Is.EqualTo("5f8399fb818d8b59f5740d43"));
        }

        [Test]
        public void ToJson_CorrectJson()
        {
            // arrange
            Launch launch = new Launch()
            {
                Fairings = new Fairings()
                { 
                    Reused = false,
                    RecoveryAttempt = false,
                    Recovered = false,
                    Ships = new string[0]
                },
                Links = new Links()
                {
                    Patch = new Patch()
                    {
                        Small = "https://images2.imgbox.com/3c/0e/T8iJcSN3_o.png",
                        Large = "https://images2.imgbox.com/40/e3/GypSkayF_o.png"
                    },
                    Reddit = new Reddit()
                    {
                        Campaign = null,
                        Launch = null,
                        Media = null,
                        Recovery = null,
                    },
                    Flickr = new Flickr()
                    {
                        Small = new string[0],
                        Original = new string[0]
                    },
                    Presskit = null,
                    Webcast = "https://www.youtube.com/watch?v=0a_00nJ_Y88",
                    YoutubeId = "0a_00nJ_Y88",
                    Article = "https://www.space.com/2196-spacex-inaugural-falcon-1-rocket-lost-launch.html",
                    Wikipedia = "https://en.wikipedia.org/wiki/DemoSat",
                },
                StaticFireDateUtc = new DateTime(2006, 03, 17, 0, 0, 0, DateTimeKind.Utc),
                StaticFireDateUnix = 1142553600,
                Tbd = false,
                Net = false,
                Window = 0,
                Rocket = "5e9d0d95eda69955f709d1eb",
                Success = false,
                Details = "Engine failure at 33 seconds and loss of vehicle",
                Crew = new string[0],
                Ships = new string[0],
                Capsules = new string[0],
                Payloads = new string[]
                {
                    "5eb0e4b5b6c3bb0006eeb1e1"
                },
                Launchpad = "5e9e4502f5090995de566f86",
                AutoUpdate = true,
                Failures = new Failure[]
                {
                    new Failure()
                    {
                        Time = 33,
                        Altitude = null,
                        Reason = "merlin engine failure"
                    }
                },
                FlightNumber = 1,
                Name = "FalconSat",
                DateUtc = new DateTime(2006, 03, 24, 22, 30, 0, DateTimeKind.Utc),
                DateUnix = 1143239400,
                DateLocal = new DateTimeOffset(2006, 03, 25, 10, 30, 00, new TimeSpan(12, 00, 00)),
                DatePrecision = "hour",
                Upcoming = false,
                Cores = new Core[]
                {
                    new Core()
                    {
                        CoreId = "5e9e289df35918033d3b2623",
                        Flight = 1,
                        Gridfins = false,
                        Legs = false,
                        Reused = false,
                        LandingAttempt = false,
                        LandingSuccess = null,
                        LandingType = null,
                        Landpad = null,
                    }
                },
                Id = "5eb87cd9ffd86e000604b32a"
            };

            // act
            var json = _converter.ToJson(launch);

            // assert
            Assert.That(json.ToString(), Is.EqualTo(File.ReadAllText(Path.Combine("SpaceX","Launch.json"))));
        }
    }
}