using System;
using System.Text;
using JsonSG;

namespace JsonSGTest
{
    class Program
    {
        static JsonSGConvert _convert = new JsonSGConvert();

        static void Main(string[] args)
        {
            var testClass = new JsongTests3()
            {
                FirstName = "Daniel",
                LastName = "Hughes",
                Age = 38
            };

            Console.WriteLine(_convert.ToJson(testClass));

            Test("ToJsonFormat", ToJsonFormat, testClass, ExpectedJson);
            Test("ToJsonStringConcat", ToJsonStringConcat, testClass, ExpectedJson);
            Test("ToJsonStringFormat", ToJsonStringFormat, testClass, ExpectedJson);
            Test("ToJsonAppend", ToJsonAppend, testClass, ExpectedJson);
            Test("Jsong", ToJsonSG, testClass, ExpectedJson);
            //Test("Utf8Json", ToUtf8Json, testClass, ExpectedJson);
        }

        const string ExpectedJson = "{\"FirstName\":\"Daniel\",\"LastName\":\"Hughes\",\"Age\":38}";

        // static string ToUtf8Json(JsongTests3 testClass)
        // {
        //     var bytes = Utf8Json.JsonSerializer.Serialize(testClass);
        //     return ExpectedJson;//Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        // }

        static string ToJsonSG(JsongTests3 testClass)
        {
            return _convert.ToJson(testClass);//Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

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

        [ThreadStatic]
        static StringBuilder Builder;

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
