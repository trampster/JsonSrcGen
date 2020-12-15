using BenchmarkDotNet.Running;

[assembly: JsonSrcGen.JsonArray(typeof(int))]

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SerializeStructToString>();

            //Benchmark Examples
            //BenchmarkRunner.Run<DeserializeToStruct>();
            //BenchmarkRunner.Run<SerializeToArray>();

            //BenchmarkRunner.Run<DeserializeToClass>();
            //BenchmarkRunner.Run<DeserializeToArray>();
        }
    }

  
}

