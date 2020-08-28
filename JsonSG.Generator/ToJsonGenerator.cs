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
                        classBuilder.AppendLine(3, $"if(value.{property.Name} == null)");
                        classBuilder.AppendLine(3, "{");
                        var nullAppendBuilder = new StringBuilder(appendBuilder.ToString());
                        nullAppendBuilder.Append("null");
                        MakeAppend(4, classBuilder, nullAppendBuilder);
                        classBuilder.AppendLine(3, "}");

                        classBuilder.AppendLine(3, "else");
                        classBuilder.AppendLine(3, "{");
                        appendBuilder.Append($"\\\"");
                        MakeAppend(4, classBuilder, appendBuilder);
                        classBuilder.AppendLine(4, $"builder.Append(value.{property.Name});");
                        appendBuilder.Append($"\\\"");
                        MakeAppend(4, classBuilder, appendBuilder);
                        classBuilder.AppendLine(3, "}");
                        break;
                    case "Int32":
                        MakeAppend(3, classBuilder, appendBuilder);
                        classBuilder.AppendLine(3, $"builder.Append(value.{property.Name});");
                        break;
                    case "Boolean":
                        MakeAppend(3, classBuilder, appendBuilder);
                        classBuilder.AppendLine(3, $"builder.Append(value.{property.Name} ? \"true\" : \"false\");");
                        break;
                    default:
                        throw new Exception($"Unsupported type {property.Type}");

                }

                if(isFirst) isFirst = false;
            }
            appendBuilder.Append("}"); 
            MakeAppend(3, classBuilder, appendBuilder);
            classBuilder.AppendLine(3, "return builder.ToString();"); 
            classBuilder.AppendLine(2, "}");
        }

        void MakeAppend(int indentLevel, CodeBuilder classBuilder, StringBuilder appendContent)
        {
            classBuilder.AppendLine(indentLevel, $"builder.Append(\"{appendContent.ToString()}\");");
            appendContent.Clear();
        }
    }
}