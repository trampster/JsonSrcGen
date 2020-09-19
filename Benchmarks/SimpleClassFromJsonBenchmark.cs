using JsonSrcGen;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    public class SimpleClassFromJsonBenchmark
    {
        readonly JsonSrcGenConvert _jsonSrcGenConvert;
        const string Json = "{\"Age\":24,\"Height\":178.54,\"Name\":\"John Smith\"}";
        SimpleClass _simpleClass = new SimpleClass();

        public SimpleClassFromJsonBenchmark()
        {
            _jsonSrcGenConvert = new JsonSrcGenConvert();
        }


        [Benchmark]
        public SimpleClass JsonSrcGen_FromJson()
        {
            _jsonSrcGenConvert.FromJson(_simpleClass, Json);
            return _simpleClass;
        }

        [Benchmark]
        public SimpleClass SystemTextJson_FromJson()
        {
            return System.Text.Json.JsonSerializer.Deserialize<SimpleClass>(Json);
        }

        [Benchmark]
        public SimpleClass NewtonsoftJson_FromJson()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<SimpleClass>(Json);
        }

        [Benchmark]
        public SimpleClass Utf8Json_FromJson()
        {
            return Utf8Json.JsonSerializer.Deserialize<SimpleClass>(Json);
        }
    }
}