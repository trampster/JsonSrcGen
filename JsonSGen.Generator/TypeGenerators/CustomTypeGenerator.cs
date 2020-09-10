using System;
using System.Text;
using JsonSGen.Generator;

namespace JsonSGen.Generator.TypeGenerators
{
    public class CustomTypeGenerator : IJsonGenerator
    {
        public string TypeName => "Custom"; 

        public void GenerateFromJson(CodeBuilder codeBuilder, int indentLevel, JsonType type, Func<string, string> valueSetter, string valueGetter)
        {
            string propertyValueName = $"property{UniqueNumberGenerator.UniqueNumber}Value";

            codeBuilder.AppendLine(indentLevel+2, "json = json.SkipWhitespace();");
            codeBuilder.AppendLine(indentLevel+2, "if(json[0] == 'n')");
            codeBuilder.AppendLine(indentLevel+2, "{");
            codeBuilder.AppendLine(indentLevel+3, valueSetter("null"));
            codeBuilder.AppendLine(indentLevel+3, $"json = json.Slice(4);");
            codeBuilder.AppendLine(indentLevel+3, $"break;");
            codeBuilder.AppendLine(indentLevel+2, "}");

            if(valueGetter != null)
            {
                codeBuilder.AppendLine(indentLevel+2, $"if({valueGetter} == null)"); 
                codeBuilder.AppendLine(indentLevel+2, "{");
                codeBuilder.AppendLine(indentLevel+3, valueSetter($"new {type.FullName}()"));
                codeBuilder.AppendLine(indentLevel+2, "}");
            }
            else
            {
                codeBuilder.AppendLine(indentLevel+2, valueSetter($"new {type.FullName}()"));
            }

            codeBuilder.AppendLine(indentLevel+2, $"json = FromJson({valueGetter}, json);");
        }

        public void GenerateToJson(CodeBuilder codeBuilder, int indentLevel, StringBuilder appendBuilder, JsonType type, string valueGetter)
        {
            codeBuilder.AppendLine(indentLevel, $"if({valueGetter} == null)");
            codeBuilder.AppendLine(indentLevel, "{");
            var nullAppendBuilder = new StringBuilder(appendBuilder.ToString());
            nullAppendBuilder.Append("null");
            codeBuilder.MakeAppend(indentLevel+1, nullAppendBuilder);
            codeBuilder.AppendLine(indentLevel, "}");
            codeBuilder.AppendLine(indentLevel, "else"); 
            codeBuilder.AppendLine(indentLevel, "{");
            codeBuilder.MakeAppend(indentLevel+1, appendBuilder);
            codeBuilder.AppendLine(indentLevel+1, $"ToJson({valueGetter}, builder);");
            codeBuilder.AppendLine(indentLevel, "}");
        }
    }
}