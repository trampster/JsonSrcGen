using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<SimpleClassFromJsonBenchmark>();
            summary = BenchmarkRunner.Run<SimpleClassToJsonBenchmark>();
        }
    }
}
