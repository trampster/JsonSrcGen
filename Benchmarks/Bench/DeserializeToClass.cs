using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using SST = ServiceStack.Text;


namespace Benchmarks
{

    [MemoryDiagnoser]
    [RankColumn]
    public class DeserializeToClass
    {
        private string _json;
        private JsonSrcGen.JsonConverter _jsonSrcGen = new JsonSrcGen.JsonConverter();

        [GlobalSetup]
        public void Setup()
        {
            //Only serializing to a String. Then everyone used this string
            _json = JsonSerializer.Serialize(new JsonClass1() { Name = ") A S DA(S )(U @# E ASD 5616 1516 5 A S )(J D)) J@# )JM A S> ><", Value = byte.MaxValue, Age = int.MaxValue, On = true });
        }

        [Benchmark]
        public void SystemTextJson()
        {
            var result = JsonSerializer.Deserialize<JsonClass1>(_json);
        }


        [Benchmark]
        public void SystemTextJsonUTF8()
        {
            var result = JsonSerializer.Deserialize<JsonClass1>(_json);
        }

        [Benchmark]
        public void Newtonsoft()
        {
            var result = JsonConvert.DeserializeObject<JsonClass1>(_json);
        }

        [GlobalSetup(Target = nameof(RunJil))]
        public void JilSetup()
        {
            Setup();
            RunJil();
        }
        [Benchmark]
        public void RunJil()
        {
            var result = Jil.JSON.Deserialize<JsonClass1>(_json);
        }

        [Benchmark]
        public void ServiceStack()
        {
            var result = SST.JsonSerializer.DeserializeFromString<JsonClass1>(_json);
        }

        [Benchmark]
        public void Utf8_Json()
        {
            var result = Utf8Json.JsonSerializer.Deserialize<JsonClass1>(_json);
        }

        [Benchmark]
        public void SourceGEN()
        {
            var retClass = new JsonClass1();
            _jsonSrcGen.FromJson(retClass, _json);
        }

        [Benchmark]
        public void SourceGENStruct()
        {
            var structRef = new JsonStruct2();
            _jsonSrcGen.FromJson(ref structRef, _json);
        }
    }
}
