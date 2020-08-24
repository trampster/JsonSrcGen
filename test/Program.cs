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

            Console.WriteLine(_convert.ToJson(testClass));

            Func<JsongTests3, bool> check = toCheck => toCheck.FirstName == "Daniel" && toCheck.LastName == "Hughes";

            Test("IdealTest", json => ReadJson(testClass, json), check, ExpectedJson);
            Test("System.Text.Json", json => System.Text.Json.JsonSerializer.Deserialize<JsongTests3>(json), check, ExpectedJson);
            Test("FromJsonSG", json => FromJsonSG(testClass, json), check, ExpectedJson);
            // Test("ToJsonAppend", ToJsonAppend, testClass, ExpectedJson);
            //ReadJson(testClass, ExpectedJson);

            Console.WriteLine($"FirstName: {testClass.FirstName} LastName {testClass.LastName}");
        }

        static string ExpectedJson = $"{{\"FirstName\":\"Daniel\",\"LastName\":\"Hughes\"}}";

        static string ToJsonSG(JsongTests3 testClass)
        {
            return _convert.ToJson(testClass);//Encoding.UTF8.GetString(bytes, 0, bytes.Length);
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
            builder.Append("}");

            return Builder.ToString();
        }

        static JsongTests3 FromJsonSG(JsongTests3 testClass, string jsonString)
        {
            _convert.FromJson(testClass, jsonString);
            return testClass;
        }

        static JsongTests3 ReadJson(JsongTests3 testClass, string jsonString)
        {
            var json = jsonString.AsSpan();

            //skip whitespace at start
            json = json.SkipWhitespaceTo('{');


            while(true)
            {
                json = json.SkipWhitespaceTo('\"');

                var propertyName = json.ReadTo('\"');

                json = json.Slice(propertyName.Length + 1);

                json = json.SkipWhitespaceTo(':');

                json = json.SkipWhitespaceTo('\"');
                var propertyValue = json.ReadTo('\"');
                
                json = json.Slice(propertyValue.Length + 1);

                switch(propertyName[0])
                {
                    case 'F':
                        if(!propertyName.EqualsString("FirstName"))
                        {
                            break;
                        }
                        testClass.FirstName = new String(propertyValue); //dam it you made me allocate memory
                        break;
                    case 'L':
                        if(!propertyName.EqualsString("LastName"))
                        {
                            break;
                        }
                        testClass.LastName = new String(propertyValue); //dam it you made me allocate memory again
                        break;
                }

                json = json.SkipWhitespaceTo(',', '}', out char found);
                if(found == '}')
                {
                    return testClass;
                }
            }
        }

        static void Test(string name, Func<JsongTests3,string> method, JsongTests3 test, string expectedJson)
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

        static void Test(string name, Func<string, JsongTests3> method, Func<JsongTests3, bool> test, string json)
        {
            var result = method(json);
            if(!test(result))
            {
                throw new Exception($"Method {name} didn't produce correct class info, expected {json} actual FirstName {result.FirstName} LastName {result.LastName}");
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
