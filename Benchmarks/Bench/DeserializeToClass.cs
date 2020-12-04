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
            _json = JsonSerializer.Serialize(new JsonClass1() { Name = ") A S DA(S )(U @# E ASD 5616 1516 5 A S )(J D)) J@# )JM A S> ><", Value = byte.MaxValue, Age = int.MaxValue, On = true });
        }

        [Benchmark]
        public void SystemTextJsonString()
        {
            var result = JsonSerializer.Deserialize<JsonClass1>(_json);
        }


        [Benchmark]
        public void SystemTextJsonUTF8String()
        {
            var result = JsonSerializer.Deserialize<JsonClass1>(_json);
        }

        [Benchmark]
        public void NewtonsoftString()
        {
            var result = JsonConvert.DeserializeObject<JsonClass1>(_json);
        }

        [GlobalSetup(Target = nameof(RunJilString))]
        public void JilSetup()
        {
            Setup();
            RunJilString();
        }
        [Benchmark]
        public void RunJilString()
        {
            var result = Jil.JSON.Deserialize<JsonClass1>(_json);
        }

        [Benchmark]
        public void ServiceStackString()
        {
            var result = SST.JsonSerializer.DeserializeFromString<JsonClass1>(_json);
        }

        [Benchmark]
        public void Utf8JsonString()
        {
            var result = Utf8Json.JsonSerializer.Deserialize<JsonClass1>(_json);
        }

        [Benchmark]
        public void SourceGENClass()
        {
            var retClass = new JsonClass1();
            _jsonSrcGen.FromJson(retClass, _json);
        }

        //Next Version
        //[Benchmark]
        //public void SourceGENStruct()
        //{
        //    var structRef = new JsonStruct();
        //    _jsonSrcGen.FromJson(ref structRef, _json);
        //}

        //[Benchmark]
        //public void SourceGENStruct3()
        //{
        //    var structRef = new JsonDecimalStruct3();
        //    _genJSON.FromJson(ref structRef, _json);
        //}
    }
}
