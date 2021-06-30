using System;
using System.Text;
using JsonSrcGen;

namespace JsonSrcGen.TypeGenerators
{
    public class CustomConverterGenerator : IJsonGenerator
    {
        public string GeneratorId { get; }
        string TypeName { get; }
        readonly CodeBuilder _classLevelBuilder;


        public readonly string _converterName;

        public CustomConverterGenerator(string generatorId, string typeName, string converterClassName, CodeBuilder classLevelBuilder)
        {
            GeneratorId = generatorId;
            TypeName = typeName;
            _classLevelBuilder = classLevelBuilder;
            _converterName =  $"CustomConverter{UniqueNumberGenerator.UniqueNumber}";
            classLevelBuilder.AppendLine(2, $"static readonly ICustomConverter<{typeName}> {_converterName} = new {converterClassName}();");
        }

        public CodeBuilder ClassLevelBuilder => _classLevelBuilder;


        public void GenerateFromJson(CodeBuilder codeBuilder, int indentLevel, JsonType type, Func<string, string> valueSetter, string valueGetter, JsonFormat format)
        {
            string propertyValueName = $"property{UniqueNumberGenerator.UniqueNumber}Value";

            codeBuilder.AppendLine(indentLevel, $"var {propertyValueName} = {valueGetter};");
            codeBuilder.AppendLine(indentLevel, $"json = {_converterName}.FromJson(json, ref {propertyValueName});");
            codeBuilder.AppendLine(indentLevel, valueSetter(propertyValueName));
        }

        public void GenerateToJson(CodeBuilder codeBuilder, int indentLevel, StringBuilder appendBuilder, JsonType type, string valueGetter, bool canBeNull, JsonFormat format)
        {
            codeBuilder.MakeAppend(indentLevel, appendBuilder, format);
            codeBuilder.AppendLine(indentLevel, $"{_converterName}.ToJson(builder, {valueGetter});");
        }

        public string OnNewObject(CodeBuilder codeBuilder, int indentLevel, Func<string, string> valueSetter)
        {
            string wasSetVariable = $"wasSet{UniqueNumberGenerator.UniqueNumber}";
            codeBuilder.AppendLine(indentLevel, $"bool {wasSetVariable} = false;");
            return wasSetVariable;
        }

        public void OnObjectFinished(CodeBuilder codeBuilder, int indentLevel, Func<string, string> valueSetter, string wasSetVariableName)
        {
            codeBuilder.AppendLine(indentLevel, $"if(!{wasSetVariableName})");
            codeBuilder.AppendLine(indentLevel, "{");
            codeBuilder.AppendLine(indentLevel+1, valueSetter($"default({TypeName})"));
            codeBuilder.AppendLine(indentLevel, "}");
        }
    }
}