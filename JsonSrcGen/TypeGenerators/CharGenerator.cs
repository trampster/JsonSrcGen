using System;
using System.Text;

namespace JsonSrcGen.TypeGenerators
{
    public class CharGenerator : IJsonGenerator
    {
        public string GeneratorId => "Char";

        public void GenerateFromJson(CodeBuilder codeBuilder, int indentLevel, JsonType type, Func<string, string> valueSetter, string valueGetter, JsonFormat format)
        {
            string propertyValueName = $"property{UniqueNumberGenerator.UniqueNumber}Value";
            codeBuilder.AppendLine(indentLevel, $"json = json.Read(out char {propertyValueName});");
            codeBuilder.AppendLine(indentLevel, valueSetter(propertyValueName));
        }

        public void GenerateToJson(CodeBuilder codeBuilder, int indentLevel, StringBuilder appendBuilder, JsonType type, string valueGetter, bool canBeNull, JsonFormat format)
        {
            appendBuilder.Append($"\\\"");
            codeBuilder.MakeAppend(indentLevel, appendBuilder, format);
            codeBuilder.AppendLine(indentLevel, $"builder.AppendEscaped({valueGetter});");
            appendBuilder.Append($"\\\"");
            codeBuilder.MakeAppend(indentLevel, appendBuilder, format);
        }

        public CodeBuilder ClassLevelBuilder => null;

        public string OnNewObject(CodeBuilder codeBuilder, int indentLevel, Func<string, string> valueSetter)
        {
            codeBuilder.AppendLine(indentLevel, valueSetter("default(char)"));
            return null;
        }

        public void OnObjectFinished(CodeBuilder codeBuilder, int indentLevel, Func<string, string> valueSetter, string wasSetVariable)
        {
            
        }
    }
}