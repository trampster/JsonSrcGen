using System;
using System.Text;
using JsonSrcGen;

namespace JsonSrcGen.TypeGenerators
{
    public class NullableGuidGenerator : IJsonGenerator
    {
        public string TypeName => "Guid?"; 

        public void GenerateFromJson(CodeBuilder codeBuilder, int indentLevel, JsonType type, Func<string, string> valueSetter, string valueGetter)
        {
            string propertyValueName = $"property{UniqueNumberGenerator.UniqueNumber}Value";
            codeBuilder.AppendLine(indentLevel, $"json = json.ReadNullableGuid(out Guid? {propertyValueName});");
            codeBuilder.AppendLine(indentLevel, valueSetter(propertyValueName));
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
            codeBuilder.AppendLine(indentLevel+1, $"builder.Append({valueGetter}.Value);");
            appendBuilder.Append($"\\\"");
            codeBuilder.MakeAppend(indentLevel+1, appendBuilder);
            codeBuilder.AppendLine(indentLevel, "}");
        }

        public CodeBuilder ClassLevelBuilder => null;
    }
}