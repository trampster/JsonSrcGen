using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using SST = ServiceStack.Text;

[assembly: JsonSrcGen.JsonArray(typeof(int))]

namespace Benchmarks
{
    [RankColumn]
    [MemoryDiagnoser]
    public class DeserializeToArray
    {
        private int[] _instance;

        [GlobalSetup]
        public void Setup()
        {
            _instance = new int[50];

            for (int i = 0; i < 50; i++)
            {
                _instance[i] = i;
            }
        }

        [Benchmark]
        public string SystemTextJsonString()
        {
            return JsonSerializer.Serialize(_instance);
        }

        [Benchmark]
        public string SystemTextJsonUTF8String()
        {
            return System.Text.UTF8Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes<int[]>(_instance));
        }

        [Benchmark]
        public string NewtonsoftString()
        {
            return JsonConvert.SerializeObject(_instance);
        }

        [GlobalSetup(Target = nameof(RunJilString))]
        public void JilSetup()
        {
            Setup();
            RunJilString();
        }
        [Benchmark]
        public string RunJilString()
        {
            return Jil.JSON.Serialize(_instance);
        }

        [Benchmark]
        public string ServiceStackString()
        {
            return SST.JsonSerializer.SerializeToString(_instance);
        }

        [Benchmark]
        public string Utf8JsonString()
        {
            return Utf8Json.JsonSerializer.ToJsonString(_instance);
        }

        [Benchmark]
        public string SourceGENClass()
        {
            return JsonSrcGen.JsonConverter.ToJson(ref _instance).ToString();
        }

    }

}
