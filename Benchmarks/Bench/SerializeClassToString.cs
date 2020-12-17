using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using System;
using JsonSerializer = System.Text.Json.JsonSerializer;
using SST = ServiceStack.Text;

namespace Benchmarks
{

    [MemoryDiagnoser]
    [RankColumn]
    public class SerializeClassToString
    {
        private JsonClass1 _instance;
        private JsonSrcGen.JsonConverter _jsonSrcGen = new JsonSrcGen.JsonConverter();

        [GlobalSetup]
        public void Setup()
        {
            _instance = _instance ?? new JsonClass1();
        }

        [Benchmark]
        public string SystemTextJson()
        {
            return JsonSerializer.Serialize(_instance);
        }

        [Benchmark]
        public string SystemTextJsonUTF8()
        {
            return System.Text.UTF8Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes<JsonClass1>(_instance));
        }

        [Benchmark]
        public string Newtonsoft()
        {
            return JsonConvert.SerializeObject(_instance);
        }

        [GlobalSetup(Target = nameof(RunJil))]
        public void JilSetup()
        {
            Setup();
            RunJil();
        }
        [Benchmark]
        public string RunJil()
        {
            return Jil.JSON.Serialize(_instance);
        }

        [Benchmark]
        public string Utf8_Json()
        {
            return Utf8Json.JsonSerializer.ToJsonString(_instance);
        }

        [Benchmark]
        public string ServiceStack()
        {
            return SST.JsonSerializer.SerializeToString(_instance);
        }

        [Benchmark]
        public string SourceGEN()
        {
            return _jsonSrcGen.ToJson(_instance).ToString();
        }
    }

}
