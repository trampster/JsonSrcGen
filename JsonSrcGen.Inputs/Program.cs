using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace JsonSrcGen.Inputs
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<BuildersBenchmark>();

            var benchmark = new BuildersBenchmark();
            Console.WriteLine(benchmark.JsonStringBuilder());
        }
    }
}
