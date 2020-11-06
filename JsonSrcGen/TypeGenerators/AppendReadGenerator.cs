using System;
using System.Text;
using JsonSrcGen;

namespace JsonSrcGen.TypeGenerators
{
    public class AppendReadGenerator : IJsonGenerator
    {
        public string GeneratorId {get;}
        public string ReadType {get;set;}

        string TypeName => GeneratorId;

        public AppendReadGenerator(string generatorId)
        {
            GeneratorId = generatorId;
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

        public void GenerateToJson(CodeBuilder codeBuilder, int indentLevel, StringBuilder appendBuilder, JsonType type, string valueGetter, bool canBeNull)
        {
            codeBuilder.MakeAppend(indentLevel, appendBuilder);
            codeBuilder.AppendLine(indentLevel, $"builder.Append({valueGetter});");
        }

        public CodeBuilder ClassLevelBuilder => null;

        public string OnNewObject(CodeBuilder codeBuilder, int indentLevel, Func<string, string> valueSetter)
        {
            codeBuilder.AppendLine(indentLevel, valueSetter($"default({TypeName})"));
            return null;
        }

        public void OnObjectFinished(CodeBuilder codeBuilder, int indentLevel, Func<string, string> valueSetter, string wasSetVariable)
        {
            
        }
    }
}