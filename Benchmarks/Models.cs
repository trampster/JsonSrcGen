using System.Collections.Generic;
using System.Linq;
using JsonSrcGen;

//using StackOnlyJsonParser;

namespace Benchmarks
{
    public class Models
    {
        private const int NUM = 50;

        public class PerfListTestClass
        {
            public List<JsonClass1> List { get; set; }

            public PerfListTestClass()
            {
                List = new List<JsonClass1>(Enumerable.Range(0, NUM).Select(x => new JsonClass1()));
            }
        }
    }


    [JsonSrcGen.Json]
    public class JsonClass1
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public bool On { get; set; }

        public byte Value { get; set; }
    }


    ///// <summary>
    ///// Equals JsonClass1
    ///// Next version
    ///// </summary>
    //[JsonSrcGen.Json]
    //public ref struct JsonStruct
    //{
    //    public string Name { get; set; }

    //    public int Age { get; set; }

    //    public bool On { get; set; }

    //    public byte Value { get; set; }
    //}

    ///// <summary>
    ///// Equals JsonClass1
    ///// Next version
    ///// </summary>
    //[JsonSrcGen.Json]
    //public struct JsonStruct2
    //{
    //    public string Name { get; set; }

    //    public int Age { get; set; }

    //    public bool On { get; set; }

    //    public byte Value { get; set; }
    //}

    ///// <summary>
    ///// Equals JsonClass1
    ///// Next version
    ///// </summary>
    //[JsonSrcGen.Json]
    //public readonly ref struct JsonStruct3
    //{
    //    public readonly string Name { get;  }

    //    public readonly int Age { get;  }

    //    public readonly bool On { get;  }

    //    public readonly byte Value { get; }

    //    public JsonStruct3(string name, int age, bool on, byte value) : this()
    //    {
    //        Value = value;
    //        Age = age;
    //        Name = name;
    //        On = on;
    //    }
    //}
}

