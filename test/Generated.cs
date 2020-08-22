
using System;
using System.Text;

namespace JsonSG
{
    public class JsonSGConvert
    {
        [ThreadStatic]
        StringBuilder Builder;
public string ToJson(JsonSGTest.JsongTests3 value)
{

            var builder = Builder;
            if(builder == null)
            {
                builder = new StringBuilder();
                Builder = builder;
            }
            builder.Clear();
    builder.Append("{\"FirstName\":\"");    builder.Append(value.FirstName);
    builder.Append("\",\"LastName\":\"");    builder.Append(value.LastName);
    builder.Append("\",\"Age\":");    builder.Append(value.Age);
    builder.Append("}");    return builder.ToString();
}

                public void PrintClassInfo()
                {System.Console.WriteLine(" Found Class JsongTests3");System.Console.WriteLine(" Member FirstName Type String");System.Console.WriteLine(" Member LastName Type String");System.Console.WriteLine(" Member Age Type Int32");}}}