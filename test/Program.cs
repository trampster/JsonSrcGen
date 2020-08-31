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
            var testClass = new JsongTests3();
            testClass.First = byte.MaxValue;
            testClass.Second = byte.MaxValue;
            testClass.Third = byte.MaxValue;

            Console.WriteLine(_convert.ToJson(testClass));

            Func<JsongTests3, bool> check = toCheck => toCheck.First == byte.MaxValue && toCheck.Second == byte.MaxValue && toCheck.Third == byte.MaxValue;

            TestFromJson("System.Text.Json", json => System.Text.Json.JsonSerializer.Deserialize<JsongTests3>(json), check, ExpectedJson);
            TestFromJson("FromJsonSG", json => FromJsonSG(testClass, json), check, ExpectedJson);
            TestFromJson("FromJsonUtf8", json => FromJsonUtf8(json), check, ExpectedJson);

            TestToJson("To System.Text.Json", jsonClass => System.Text.Json.JsonSerializer.Serialize<JsongTests3>(jsonClass), testClass, ExpectedJson);
            TestToJson("ToJsonSG", jsonClass => ToJsonSG(jsonClass), testClass, ExpectedJson);
            TestToJson("ToJsonUtf8", jsonClass => ToJsonUtf8(jsonClass), testClass, ExpectedJson);
        }

        static string ExpectedJson = $"{{\"First\":255,\"Second\":255,\"Third\":255}}";
        static byte[] ExpectedJsonByte = Encoding.UTF8.GetBytes(ExpectedJson);


        static string ToJsonSG(JsongTests3 testClass)
        {
            return _convert.ToJson(testClass);//Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        static JsongTests3 FromJsonSG(JsongTests3 testClass, string jsonString)
        {
            _convert.FromJson(testClass, jsonString);
            return testClass;
        }

        static JsongTests3 FromJsonUtf8(string jsonString)
        {
            var testClass = Utf8Json.JsonSerializer.Deserialize<JsongTests3>(ExpectedJsonByte);
            return testClass;
        }

        static string ToJsonUtf8(JsongTests3 jsonString)
        {
            var testClass = Utf8Json.JsonSerializer.Serialize<JsongTests3>(jsonString);
            return ExpectedJson; //Uft8 json return a byte array, so I will just return the expectedJson so as not to penalize it.
        }

        static void TestToJson(string name, Func<JsongTests3,string> method, JsongTests3 test, string expectedJson)
        {
            var result = method(test);
            if(result != expectedJson)
            {
                throw new Exception($"Method {name} didn't produce correct json {expectedJson} actual {result}");
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

        static void TestFromJson(string name, Func<string, JsongTests3> method, Func<JsongTests3, bool> test, string json)
        {
            var result = method(json);
            if(!test(result))
            {
                throw new Exception($"Method {name} didn't produce correct class info, expected {json}, First={result.First}, Second={result.Second}, Third={result.Third}");
            }
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for(int index = 0; index < 10000000; index++)
            {
                method(json);
            }
            stopwatch.Stop();
            Console.WriteLine($"{name}: {stopwatch.ElapsedMilliseconds}");
        }
    }
}
