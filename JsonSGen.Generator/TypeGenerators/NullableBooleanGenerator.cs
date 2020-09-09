using System.Text;
using JsonSGen.Generator;

namespace JsonSGen.TypeGenerators
{
    public class NullableBoolGenerator : IJsonGenerator
    {
        public string TypeName => "Boolean?";

        public void GenerateFromJson(CodeBuilder codeBuilder, int indentLevel, JsonProperty property)
        {
            codeBuilder.AppendLine(indentLevel, $"json = json.Read(out bool? property{property.CodeName}Value);");
            codeBuilder.AppendLine(indentLevel, $"value.{property.CodeName} = property{property.CodeName}Value;");
        }

        public void GenerateToJson(CodeBuilder codeBuilder, int indentLevel, StringBuilder appendBuilder, JsonProperty property)
        {
            codeBuilder.MakeAppend(indentLevel, appendBuilder);
            codeBuilder.AppendLine(indentLevel, $"builder.Append(value.{property.CodeName} == null ? \"null\" : value.{property.CodeName}.Value ? \"true\" : \"false\");");
        }
    }
}