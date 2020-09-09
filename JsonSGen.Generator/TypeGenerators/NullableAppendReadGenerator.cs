using System.Text;
using JsonSGen.Generator;

namespace JsonSGen.TypeGenerators
{
    public class NullableAppendReadGenerator : IJsonGenerator
    {
        public string TypeName {get;}
        public string ReadType {get;set;}

        public NullableAppendReadGenerator(string typeName)
        {
            TypeName = typeName;
        }

        public void GenerateFromJson(CodeBuilder codeBuilder, int indentLevel, JsonProperty property)
        {
            if(ReadType == null)
            {
                codeBuilder.AppendLine(indentLevel, $"json = json.Read(out {TypeName} property{property.CodeName}Value);");
                codeBuilder.AppendLine(indentLevel, $"value.{property.CodeName} = property{property.CodeName}Value;");
            }
            else
            {
                codeBuilder.AppendLine(indentLevel, $"json = json.Read(out {ReadType} property{property.CodeName}Value);");
                codeBuilder.AppendLine(indentLevel, $"value.{property.CodeName} = ({TypeName})property{property.CodeName}Value;");
            }
        }

        public void GenerateToJson(CodeBuilder codeBuilder, int indentLevel, StringBuilder appendBuilder, JsonType type, string valueGetter)
        {
            codeBuilder.MakeAppend(indentLevel, appendBuilder);
            codeBuilder.AppendLine(indentLevel, $"if({valueGetter} == null)");
            codeBuilder.AppendLine(indentLevel, "{");
            codeBuilder.AppendLine(indentLevel+1, $"builder.Append(\"null\");");
            codeBuilder.AppendLine(indentLevel, "}");
            codeBuilder.AppendLine(indentLevel, "else");
            codeBuilder.AppendLine(indentLevel, "{");
            codeBuilder.AppendLine(indentLevel+1, $"builder.Append({valueGetter});");
            codeBuilder.AppendLine(indentLevel, "}");
        }
    }
}