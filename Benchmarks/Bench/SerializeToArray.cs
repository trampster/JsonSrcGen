using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using System;
using JsonSerializer = System.Text.Json.JsonSerializer;
using SST = ServiceStack.Text;

namespace Benchmarks
{
    [RankColumn]
    [MemoryDiagnoser]
    public class SerializeToArray
    {
        private static string _json;

        [GlobalSetup]
        public void Setup()
        {
            int[] instance = new int[50];

            for (int i = 0; i < 50; i++)
            {
                instance[i] = i;
            }

            _json = JsonSerializer.Serialize<int[]>(instance);
        }

        [Benchmark]
        public void SystemTextJsonArrayInt()
        {
            JsonSerializer.Deserialize<int[]>(_json);
        }

        [Benchmark]
        public void SystemTextJsonUTF8ArrayInt()
        {
           JsonSerializer.Deserialize<int[]>(_json);
        }

        [Benchmark]
        public void NewtonsoftArrayInt()
        {
            JsonConvert.DeserializeObject<int[]>(_json);
        }

        [GlobalSetup(Target = nameof(RunJilArrayInt))]
        public void JilSetup()
        {
            Setup();
            RunJilArrayInt();
        }
        [Benchmark]
        public void RunJilArrayInt()
        {
            Jil.JSON.Serialize(_json);
        }

        [Benchmark]
        public void ServiceStackArrayInt()
        {
            SST.JsonSerializer.DeserializeFromString<int[]>(_json);
        }

        [Benchmark]
        public void Utf8JsonArrayInt()
        {
            Utf8Json.JsonSerializer.Deserialize<int[]>(_json);
        }

        [Benchmark]
        public void SourceGENArrayInt()
        {
            var instance = System.Array.Empty<int>();
            JsonSrcGen.JsonConverter.FromJson(ref instance, _json);
        }

    }

}
