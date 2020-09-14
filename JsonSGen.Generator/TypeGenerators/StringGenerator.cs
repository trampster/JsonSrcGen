using System;
using System.Text;
using JsonSGen.Generator;

namespace JsonSGen.Generator.TypeGenerators
{
    public class StringGenerator : IJsonGenerator
    {
        public string TypeName => "String";

        public void GenerateFromJson(CodeBuilder codeBuilder, int indentLevel, JsonType type, Func<string, string> valueSetter, string valueGetter)
        {
            string propertyValueName = $"property{UniqueNumberGenerator.UniqueNumber}Value";
            codeBuilder.AppendLine(indentLevel+2, $"json = json.Read(out string {propertyValueName});");
            codeBuilder.AppendLine(indentLevel+2, valueSetter(propertyValueName));
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
            appendBuilder.Append($"\\\"");
            codeBuilder.MakeAppend(indentLevel+1, appendBuilder);
            codeBuilder.AppendLine(indentLevel+1, $"builder.AppendEscaped({valueGetter});");
            appendBuilder.Append($"\\\"");
            codeBuilder.MakeAppend(indentLevel+1, appendBuilder);
            codeBuilder.AppendLine(indentLevel, "}");
        }

        public CodeBuilder ClassLevelBuilder => null;
    }
}