using System;
using System.Text;
using JsonSGen.Generator;

namespace JsonSGen.Generator.TypeGenerators
{
    public class NullableBoolGenerator : IJsonGenerator
    {
        public string TypeName => "Boolean?";

        public void GenerateFromJson(CodeBuilder codeBuilder, int indentLevel, JsonType type, Func<string, string> valueSetter, string valueGetter)
        {
            string propertyValueName = $"property{UniqueNumberGenerator.UniqueNumber}Value";
            codeBuilder.AppendLine(indentLevel, $"json = json.Read(out bool? {propertyValueName});");
            codeBuilder.AppendLine(indentLevel, valueSetter(propertyValueName));
        }

        public void GenerateToJson(CodeBuilder codeBuilder, int indentLevel, StringBuilder appendBuilder, JsonType type, string valueGetter)
        {
            codeBuilder.MakeAppend(indentLevel, appendBuilder);
            codeBuilder.AppendLine(indentLevel, $"builder.Append({valueGetter} == null ? \"null\" : {valueGetter}.Value ? \"true\" : \"false\");");
        }
    }
}