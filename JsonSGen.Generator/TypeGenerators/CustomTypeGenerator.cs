using System.Text;
using JsonSGen.Generator;

namespace JsonSGen.TypeGenerators
{
    public class CustomTypeGenerator : IJsonGenerator
    {
        public string TypeName => "Custom"; 

        public void GenerateFromJson(CodeBuilder codeBuilder, int indentLevel, JsonProperty property)
        {
            codeBuilder.AppendLine(indentLevel+2, "json = json.SkipWhitespace();");
            codeBuilder.AppendLine(indentLevel+2, "if(json[0] == 'n')");
            codeBuilder.AppendLine(indentLevel+2, "{");
            codeBuilder.AppendLine(indentLevel+3, $"value.{property.CodeName} = null;");
            codeBuilder.AppendLine(indentLevel+3, $"json = json.Slice(4);");
            codeBuilder.AppendLine(indentLevel+3, $"break;");
            codeBuilder.AppendLine(indentLevel+2, "}"); 

            codeBuilder.AppendLine(indentLevel+2, $"if(value.{property.CodeName} == null)"); 
            codeBuilder.AppendLine(indentLevel+2, "{");
            codeBuilder.AppendLine(indentLevel+3, $"value.{property.CodeName} = new {property.Type.FullName}();");
            codeBuilder.AppendLine(indentLevel+2, "}");

            codeBuilder.AppendLine(indentLevel+2, $"json = FromJson(value.{property.CodeName}, json);");
        }

        public void GenerateToJson(CodeBuilder codeBuilder, int indentLevel, StringBuilder appendBuilder, JsonProperty property)
        {
            codeBuilder.AppendLine(indentLevel, $"if(value.{property.CodeName} == null)");
            codeBuilder.AppendLine(indentLevel, "{");
            var nullAppendBuilder = new StringBuilder(appendBuilder.ToString());
            nullAppendBuilder.Append("null");
            codeBuilder.MakeAppend(indentLevel+1, nullAppendBuilder);
            codeBuilder.AppendLine(indentLevel, "}");
            codeBuilder.AppendLine(indentLevel, "else"); 
            codeBuilder.AppendLine(indentLevel, "{");
            codeBuilder.MakeAppend(indentLevel+1, appendBuilder);
            codeBuilder.AppendLine(indentLevel+1, $"ToJson(value.{property.CodeName}, builder);");
            codeBuilder.AppendLine(indentLevel, "}");
        }
    }
}