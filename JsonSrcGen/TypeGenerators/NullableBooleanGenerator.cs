using System;
using System.Text;
using JsonSrcGen;

namespace JsonSrcGen.TypeGenerators
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

        public void GenerateToJson(CodeBuilder codeBuilder, int indentLevel, StringBuilder appendBuilder, JsonType type, string valueGetter, bool canBeNull)
        {
            codeBuilder.MakeAppend(indentLevel, appendBuilder);
            if(canBeNull)
            {
                codeBuilder.AppendLine(indentLevel, $"builder.Append({valueGetter} == null ? \"null\" : {valueGetter}.Value ? \"true\" : \"false\");");
            }
            else
            {
                codeBuilder.AppendLine(indentLevel, $"builder.Append({valueGetter}.Value ? \"true\" : \"false\");");
            }
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