using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text;

namespace JsonSrcGen
{
    public class BuildersBenchmark
    {
        readonly JsonStringBuilder _jsonStringBuilder;
        readonly StringBuilder _stringBuilder;

        public BuildersBenchmark()
        {
            _jsonStringBuilder = new JsonStringBuilder();
            _stringBuilder = new StringBuilder();
        }


        [Benchmark]
        public string JsonStringBuilder()
        {
            _jsonStringBuilder.Clear();
            _jsonStringBuilder.Append("{\"First\":");
            _jsonStringBuilder.Append(125);
            _jsonStringBuilder.Append(",\"Min\":");
            _jsonStringBuilder.Append(int.MinValue);
            _jsonStringBuilder.Append(",\"Max\":");
            _jsonStringBuilder.Append(int.MaxValue);
            _jsonStringBuilder.Append(",\"Zero\":");
            _jsonStringBuilder.Append(0);
            _jsonStringBuilder.Append("}");

            return _jsonStringBuilder.ToString();
        }

        [Benchmark]
        public string StringBuilder()
        {
            _stringBuilder.Clear();
            _stringBuilder.Append("{\"First\":");
            _stringBuilder.Append(125);
            _stringBuilder.Append(",\"Min\":");
            _stringBuilder.Append(int.MinValue);
            _stringBuilder.Append(",\"Max\":");
            _stringBuilder.Append(int.MaxValue);
            _stringBuilder.Append(",\"Zero\":");
            _stringBuilder.Append(0);
            _stringBuilder.Append("}");

            return _stringBuilder.ToString();
        }
    }
}