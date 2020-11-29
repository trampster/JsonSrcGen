using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using SST = ServiceStack.Text;

namespace Benchmarks
{

    [MemoryDiagnoser]
    [RankColumn]
    public class SerializeToString
    {
        private JsonClass1 _instance;

        [GlobalSetup]
        public void Setup()
        {
            _instance = _instance ?? new JsonClass1();
        }

        [Benchmark]
        public string SystemTextJsonString()
        {
            return JsonSerializer.Serialize(_instance);
        }

        [Benchmark]
        public string SystemTextJsonUTF8String()
        {
            return System.Text.UTF8Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes<JsonClass1>(_instance));
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
        public string Utf8JsonString()
        {
            return Utf8Json.JsonSerializer.ToJsonString(_instance);
        }

        [Benchmark]
        public string ServiceStackString()
        {
            return SST.JsonSerializer.SerializeToString(_instance);
        }

        [Benchmark]
        public string SystemTextJsonStruct()
        {
            JsonStruct2 instance2 = new();
            return JsonSerializer.Serialize(instance2);
        }

        [Benchmark]
        public string Utf8JsonStruct()
        {
            JsonStruct2 instance2 = new();
            return Utf8Json.JsonSerializer.ToJsonString(instance2);
        }

        [Benchmark]
        public string SourceGENClass()
        {
            return JsonSrcGen.JsonConverter.ToJson(_instance).ToString();
        }

        [Benchmark]
        public string SourceGENStruct2()
        {
            JsonStruct2 instance2 = new();
            return JsonSrcGen.JsonConverter.ToJson(instance2).ToString();
        }

        [Benchmark]
        public string SourceGENStructRef()
        {
            JsonStruct instance2 = new();
            return JsonSrcGen.JsonConverter.ToJson(ref instance2).ToString();
        }

        [Benchmark]
        public string SourceGENStructRefReadOnly()
        {
            JsonStruct3 instance3 = new();
            return JsonSrcGen.JsonConverter.ToJson(ref instance3).ToString();
        }
    }

}
