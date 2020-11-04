using System;
using System.Text;

namespace JsonSrcGen.TypeGenerators
{
    public class CharGenerator : IJsonGenerator
    {
        public string TypeName => "Char";

        public void GenerateFromJson(CodeBuilder codeBuilder, int indentLevel, JsonType type, Func<string, string> valueSetter, string valueGetter)
        {
            string propertyValueName = $"property{UniqueNumberGenerator.UniqueNumber}Value";
            codeBuilder.AppendLine(indentLevel, $"json = json.Read(out char {propertyValueName});");
            codeBuilder.AppendLine(indentLevel, valueSetter(propertyValueName));
        }

        public void GenerateToJson(CodeBuilder codeBuilder, int indentLevel, StringBuilder appendBuilder, JsonType type, string valueGetter, bool canBeNull)
        {
            appendBuilder.Append($"\\\"");
            codeBuilder.MakeAppend(indentLevel, appendBuilder);
            codeBuilder.AppendLine(indentLevel, $"builder.AppendEscaped({valueGetter});");
            appendBuilder.Append($"\\\"");
            codeBuilder.MakeAppend(indentLevel, appendBuilder);
        }

        public CodeBuilder ClassLevelBuilder => null;

        public void OnNewObject(CodeBuilder codeBuilder, int indentLevel, Func<string, string> valueSetter)
        {

        }

        public void OnObjectFinished(CodeBuilder codeBuilder, int indentLevel, Func<string, string> valueSetter)
        {
            
        }
    }
}