using JsonSrcGen;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Text.Json.Serialization;

namespace Benchmarks
{
    [JsonSerializable(typeof(SimpleClass))]
    internal partial class MyJsonContext : JsonSerializerContext
    {
    }

    public class SimpleClassToJsonBenchmark
    {
        readonly JsonSrcGen.JsonConverter _jsonSrcGenConvert;
        SimpleClass _simpleClass = new SimpleClass()
        {
            Age = 42,
            Height = 178.54f,
            Name = "John Smith"
        };

        public SimpleClassToJsonBenchmark()
        {
            _jsonSrcGenConvert = new JsonSrcGen.JsonConverter();
        }


        [Benchmark]
        public ReadOnlySpan<char> JsonSrcGen_ToJson()
        {
            return _jsonSrcGenConvert.ToJson(_simpleClass);
        }

        [Benchmark]
        public string SystemTextJson_ToJson()
        {
            return System.Text.Json.JsonSerializer.Serialize(_simpleClass);
        }

        [Benchmark]
        public string SystemTextJsonSourceGen_ToJson()
        {
            return System.Text.Json.JsonSerializer.Serialize(_simpleClass, MyJsonContext.Default.SimpleClass);
        }

        [Benchmark]
        public string NewtonsoftJson_ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(_simpleClass);
        }

        [Benchmark]
        public string Utf8Json_ToJson()
        {
            return Utf8Json.JsonSerializer.ToJsonString(_simpleClass);
        }
    }
}