using JsonSrcGen;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text;
using System;

namespace Benchmarks
{
    public class SpanJsonWorkspace
    {
        byte[] _jsonOnce = Encoding.UTF8.GetBytes("12 ");
        byte[] _json;

        public SpanJsonWorkspace()
        {
            _json = new byte[_jsonOnce.Length * 1000];
            int jsonIndex = 0;
            for (int times = 0; times < 1000; times++)
            {
                for (int index = 0; index < _jsonOnce.Length; index++)
                {
                    _json[jsonIndex] = _jsonOnce[index];
                    jsonIndex++;
                }
            }
        }


        // [Benchmark]
        // public void JsonSrcGen()
        // {
        //     ReadOnlySpan<byte> json = _json.AsSpan();

        //     for(int index = 0; index < 1000; index++)
        //     {
        //         json = json.ReadOld(out int value);
        //     }
        // }

        [Benchmark]
        public void JsonSrcGenNew()
        {
            ReadOnlySpan<byte> json = _json.AsSpan();

            for (int index = 0; index < 1000; index++)
            {
                json = json.Read(out int value);
            }
        }

        [Benchmark]
        public void SpanJSON()
        {
            SpanJson.JsonReader<byte> jsonReader = new SpanJson.JsonReader<byte>(_json);

            for (int index = 0; index < 1000; index++)
            {
                jsonReader.ReadInt32();
            }
        }
    }
}