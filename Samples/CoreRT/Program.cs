using System;
using JsonSrcGen;

namespace JsonSrcGen.Samples.CoreRT
{
    class Program
    {
        static void Main(string[] args)
        {
            var convert = new JsonSrcGenConvert();
            var json = convert.ToJson(new SimpleClass()
            {
                Age = 24,
                Height = 65.5f,
                Name = "Bilbo Baggins"
            });
            Console.WriteLine(json);
        }
    }
}
