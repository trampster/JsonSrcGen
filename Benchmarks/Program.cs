using BenchmarkDotNet.Running;
using System;
using System.Threading.Tasks;

[assembly: JsonSrcGen.JsonArray(typeof(int))]

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SerializeToString>();
            //BenchmarkRunner.Run<SerializeToArray>();

            //BenchmarkRunner.Run<DeserializeToClass>();
            //BenchmarkRunner.Run<DeserializeToArray>();
        }
    }

  
}

