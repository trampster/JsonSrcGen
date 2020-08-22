using System;
using System.Text;
using JsonSG;

namespace JsonSGTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var convert = new JsonSGConvert();

            convert.PrintClassInfo();

            var testClass = new JsongTests3()
            {
                FirstName = "Daniel",
                LastName = "Hughes",
                Age = 38
            };

            string expectedJson = "{\"FirstName\":\"Daniel\",\"LastName\":\"Hughes\",\"Age\":38}";

            Console.WriteLine(convert.ToJson(testClass));

            Test("ToJsonFormat", ToJsonFormat, testClass, expectedJson);
            Test("ToJsonStringConcat", ToJsonStringConcat, testClass, expectedJson);
            Test("ToJsonStringFormat", ToJsonStringFormat, testClass, expectedJson);
            Test("ToJsonAppend", ToJsonAppend, testClass, expectedJson);
            Test("Jsong", test => convert.ToJson(test), testClass, expectedJson);
        }

        [ThreadStatic]
        static StringBuilder Builder;

        static string ToJsonFormat(JsongTests3 testClass)
        {
            return $"{{\"FirstName\":\"{testClass.FirstName}\",\"LastName\":\"{testClass.LastName}\",\"Age\":{testClass.Age}}}";
        }

        static string ToJsonStringFormat(JsongTests3 testClass)
        {
            return String.Format("{{\"FirstName\":\"{0}\",\"LastName\":\"{1}\",\"Age\":{2}}}", testClass.FirstName, testClass.LastName, testClass.Age);
        }

        static string ToJsonStringConcat(JsongTests3 testClass)
        {
            return String.Concat("{\"FirstName\":\"", testClass.FirstName, "\",\"LastName\":\"", testClass.LastName, "\",\"Age\":", testClass.Age, "}");
        }

        static string ToJsonAppend(JsongTests3 testClass)
        {
            var builder = Builder;
            if(builder == null)
            {
                builder = new StringBuilder();
                Builder = builder;
            }
            builder.Clear();
            builder.Append("{\"FirstName\":\"");
            builder.Append(testClass.FirstName);
            builder.Append("\",\"LastName\":\"");
            builder.Append(testClass.LastName);
            builder.Append("\",\"Age\":");
            builder.Append(testClass.Age);
            builder.Append("}");

            return Builder.ToString();
        }

        static void Test(string name, Func<JsongTests3,string> method, JsongTests3 test, string expectedJson)
        {
            var result = method(test);
            if(result != expectedJson)
            {
                throw new Exception($"Method {name} didn't produce correct json, expected {expectedJson} actual {result}");
            }
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for(int index = 0; index < 10000000; index++)
            {
                method(test);
            }
            stopwatch.Stop();
            Console.WriteLine($"{name}: {stopwatch.ElapsedMilliseconds}");
        }
    }
}
