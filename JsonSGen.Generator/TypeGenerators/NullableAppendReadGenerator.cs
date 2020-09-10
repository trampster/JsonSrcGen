using System;
using System.Text;
using JsonSGen.Generator;

namespace JsonSGen.Generator.TypeGenerators
{
    public class NullableAppendReadGenerator : IJsonGenerator
    {
        public string TypeName {get;}
        public string ReadType {get;set;}

        public NullableAppendReadGenerator(string typeName)
        {
            TypeName = typeName;
        }

        public void GenerateFromJson(CodeBuilder codeBuilder, int indentLevel, JsonType type, Func<string, string> valueSetter, string valueGetter)
        {
            string propertyValueName = $"property{UniqueNumberGenerator.UniqueNumber}Value";
            if(ReadType == null)
            {
                codeBuilder.AppendLine(indentLevel, $"json = json.Read(out {TypeName} {propertyValueName});");
                codeBuilder.AppendLine(indentLevel, valueSetter(propertyValueName));
            }
            else
            {
                codeBuilder.AppendLine(indentLevel, $"json = json.Read(out {ReadType} {propertyValueName});");
                codeBuilder.AppendLine(indentLevel, valueSetter($"({TypeName}){propertyValueName}"));
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