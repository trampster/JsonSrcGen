using System;
using System.Text;
using JsonSGen.Generator;

namespace JsonSGen.Generator.TypeGenerators
{
    public class GuidGenerator : IJsonGenerator
    {
        public string TypeName => "Guid"; 

        public void GenerateFromJson(CodeBuilder codeBuilder, int indentLevel, JsonType type, Func<string, string> valueSetter, string valueGetter)
        {
            string propertyValueName = $"property{UniqueNumberGenerator.UniqueNumber}Value";
            codeBuilder.AppendLine(indentLevel, $"json = json.ReadGuid(out Guid {propertyValueName});");
            codeBuilder.AppendLine(indentLevel, valueSetter(propertyValueName));
        }

        public void GenerateToJson(CodeBuilder codeBuilder, int indentLevel, StringBuilder appendBuilder, JsonType type, string valueGetter)
        {
            appendBuilder.Append("\\\"");
            codeBuilder.MakeAppend(indentLevel, appendBuilder);
            codeBuilder.AppendLine(indentLevel, $"builder.Append({valueGetter});");
            appendBuilder.Append("\\\""); 
        }
        public CodeBuilder ClassLevelBuilder => null;
    }
}