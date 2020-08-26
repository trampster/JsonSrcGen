using System.Text;
using System;

namespace JsonSG.Generator
{
    public class FromJsonGenerator
    {

        public void Generate(JsonClass jsonClass, CodeBuilder classBuilder)
        {
            classBuilder.AppendLine(2, $"public void FromJson({jsonClass.Namespace}.{jsonClass.Name} value, string jsonString)");
            classBuilder.AppendLine(2, "{");

            classBuilder.AppendLine(3, "var json = jsonString.AsSpan();");

            classBuilder.AppendLine(3, "json = json.SkipWhitespaceTo('{');");

            classBuilder.AppendLine(3, "while(true)");
            classBuilder.AppendLine(3, "{");

            classBuilder.AppendLine(4, "json = json.SkipWhitespaceTo('\\\"');");
            classBuilder.AppendLine(4, "var propertyName = json.ReadTo('\\\"');");
            classBuilder.AppendLine(4, "json = json.Slice(propertyName.Length + 1);");
            classBuilder.AppendLine(4, "json = json.SkipWhitespaceTo(':');");
            
            
            classBuilder.AppendLine(4, "switch(propertyName[0])");
            classBuilder.AppendLine(4, "{");

            foreach(var property in jsonClass.Properties)
            {
                classBuilder.AppendLine(5, $"case '{property.Name[0]}':");
                classBuilder.AppendLine(6, $"if(!propertyName.EqualsString(\"{property.Name}\"))");
                classBuilder.AppendLine(6, "{");
                classBuilder.AppendLine(7, "break;"); //todo: need to read to the next property (could be an object list so need to count '{' and '}')
                classBuilder.AppendLine(6, "}");
                if(property.Type == "Int32")
                {
                    classBuilder.AppendLine(6, $"json = json.ReadInt(out int property{property.Name}Value);");

                }
                else if(property.Type == "String")
                {
                    classBuilder.AppendLine(6, $"json = json.ReadString(out string property{property.Name}Value);");
                }
                classBuilder.AppendLine(6, $"value.{property.Name} = property{property.Name}Value;");
                classBuilder.AppendLine(6, "break;");
            }

            classBuilder.AppendLine(4, "}"); // end of switch

            classBuilder.AppendLine(4, "json = json.SkipWhitespaceTo(',', '}', out char found);");
            classBuilder.AppendLine(4, "if(found == '}')");
            classBuilder.AppendLine(4, "{");
            classBuilder.AppendLine(5, "return;");
            classBuilder.AppendLine(4, "}");
            



            classBuilder.AppendLine(3, "}");
            classBuilder.AppendLine(2, "}");
        }
    }
}