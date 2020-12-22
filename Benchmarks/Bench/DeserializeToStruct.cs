using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using SST = ServiceStack.Text;


namespace Benchmarks
{

    [MemoryDiagnoser]
    [RankColumn]
    public class DeserializeToStruct
    {
        private string _json;
        private JsonSrcGen.JsonConverter _jsonSrcGen = new JsonSrcGen.JsonConverter();

        [GlobalSetup]
        public void Setup()
        {
            //Only serializing to a String. Then everyone used this string
            _json = JsonSerializer.Serialize(new JsonStruct2() { Name = ") A S DA(S )(U @# E ASD 5616 1516 5 A S )(J D)) J@# )JM A S> ><", Value = byte.MaxValue, Age = int.MaxValue, On = true });
        }

        [Benchmark]
        public void SystemTextJson()
        {
            var result = JsonSerializer.Deserialize<JsonStruct2>(_json);
        }


        [Benchmark]
        public void SystemTextJsonUTF8()
        {
            var result = JsonSerializer.Deserialize<JsonStruct2>(_json);
        }

        [Benchmark]
        public void Newtonsoft()
        {
            var result = JsonConvert.DeserializeObject<JsonStruct2>(_json);
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
            var result = Jil.JSON.Deserialize<JsonStruct2>(_json);
        }

        /// <summary>
        /// NOT WORKING WITH STRUCT
        /// </summary>
        //[Benchmark]
        //public void ServiceStack()
        //{
        //    var result = SST.JsonSerializer.DeserializeFromString<JsonStruct2>(_json);
        //}

        [Benchmark]
        public void Utf8_Json()
        {
            var result = Utf8Json.JsonSerializer.Deserialize<JsonStruct2>(_json);
        }

        [Benchmark]
        public void SourceGEN()
        {
            var retClass = new JsonStruct2();
            _jsonSrcGen.FromJson(ref retClass, _json);
        }

        //[Benchmark]
        //public void SourceGENStruct()
        //{
        //    var structRef = new JsonStruct3();
        //    _jsonSrcGen.FromJson(ref structRef, _json);
        //}
    }
}
