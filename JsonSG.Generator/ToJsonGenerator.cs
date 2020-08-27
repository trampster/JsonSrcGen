using System.Text;
using System.Linq;
using System;

namespace JsonSG.Generator
{
    public class ToJsonGenerator
    {
        const string BuilderText = @"
            var builder = Builder;
            if(builder == null)
            {
                builder = new StringBuilder();
                Builder = builder;
            }
            builder.Clear();";

        public void Generate(JsonClass jsonClass, CodeBuilder classBuilder)
        {
            classBuilder.AppendLine(2, $"public string ToJson({jsonClass.Namespace}.{jsonClass.Name} value)");
            classBuilder.AppendLine(2, "{");
            classBuilder.AppendLine(0, BuilderText);

            var appendBuilder = new StringBuilder();
            appendBuilder.Append("{");

            bool isFirst = true;
            foreach(var property in jsonClass.Properties.OrderBy(p => p.Name))
            {
                if(!isFirst)
                {
                    appendBuilder.Append(",");
                }

                appendBuilder.Append($"\\\"{property.Name}\\\":"); 

                switch(property.Type)
                {
                    case "String":
                        appendBuilder.Append($"\\\"");
                        MakeAppend(classBuilder, appendBuilder);
                        classBuilder.AppendLine(3, $"builder.Append(value.{property.Name});");
                        appendBuilder.Append($"\\\"");
                        break;
                    case "Int32":
                        MakeAppend(classBuilder, appendBuilder);
                        classBuilder.AppendLine(3, $"builder.Append(value.{property.Name});");
                        break;
                    case "Boolean":
                        MakeAppend(classBuilder, appendBuilder);
                        classBuilder.AppendLine(3, $"builder.Append(value.{property.Name} ? \"true\" : \"false\");");
                        break;
                    default:
                        throw new Exception($"Unsupported type {property.Type}");

                }

                if(isFirst) isFirst = false;
            }
            appendBuilder.Append("}"); 
            MakeAppend(classBuilder, appendBuilder);
            classBuilder.AppendLine(3, "return builder.ToString();"); 
            classBuilder.AppendLine(2, "}");
        }

        void MakeAppend(CodeBuilder classBuilder, StringBuilder appendContent)
        {
            classBuilder.AppendLine(3, $"builder.Append(\"{appendContent.ToString()}\");");
            appendContent.Clear();
        }
    }
}