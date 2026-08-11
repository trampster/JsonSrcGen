using JsonSrcGen;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text;
using System;

[assembly: GenerationOutputFolder("/home/daniel/Work/JsonSrcGen/Generated")]


namespace Benchmarks
{
    
    [Json]
    public class JsonTestClass
    {
        public string FirstName{get;set;}
        public string LastName{get;set;}
        public int Age{get;set;}
        public bool Registered{get;set;}
    }

    

    public class SpanJsonComparision
    {
        readonly JsonConverter _jsonSrcGenConvert;
        readonly byte[] _json = Encoding.UTF8.GetBytes("{\"FirstName\":\"John\",\"LastName\":\"Smith\",\"Age\":12,\"Registered\":true}");
        JsonTestClass _simpleClass = new JsonTestClass();

        public SpanJsonComparision()
        {
            _jsonSrcGenConvert = new JsonConverter();
        }


        [Benchmark]
        public JsonTestClass JsonSrcGen_FromJson()
        {
            _jsonSrcGenConvert.FromJson(_simpleClass, _json);
            return _simpleClass;
        }

        [Benchmark]
        public JsonTestClass SpanJSON()
        {
            return SpanJson.JsonSerializer.Generic.Utf8.Deserialize<JsonTestClass>(_json);
        }
    }
}