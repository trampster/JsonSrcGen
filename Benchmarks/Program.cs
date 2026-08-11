using System;
using BenchmarkDotNet.Running;
using JsonSrcGen;

[assembly: JsonSrcGen.JsonArray(typeof(int))]
[assembly: JsonSrcGen.JsonArray(typeof(string))]




namespace Benchmarks
{
    [Json]
    public class JsonArrayClass
    {
        public bool[] BooleanArray { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SimpleClassToJsonBenchmark>();
            // var comparision = new SpanJsonComparision();  
            // comparision.JsonSrcGen_Local();
        }
    }
}

