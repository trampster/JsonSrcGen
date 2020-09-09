using System;
using System.Text;
using JsonSGen.Generator;

namespace JsonSGen.TypeGenerators
{
    public class ListGenerator : IJsonGenerator
    {
        readonly Func<JsonType, IJsonGenerator> _getGeneratorForType;
        public string TypeName => "List"; 

        public ListGenerator(Func<JsonType, IJsonGenerator> getGeneratorForType)
        {
            _getGeneratorForType = getGeneratorForType;
        }

        public void GenerateFromJson(CodeBuilder codeBuilder, int indentLevel, JsonProperty property)
        {
        }

        int _listNumber = 0;

        public void GenerateToJson(CodeBuilder codeBuilder, int indentLevel, StringBuilder appendBuilder, JsonType type, string valueGetter)
        {
            var listElementType = type.GenericArguments[0];
            var generator = _getGeneratorForType(listElementType);
            appendBuilder.Append("[");
            codeBuilder.MakeAppend(indentLevel, appendBuilder);

            string listName = $"list{_listNumber}"; 
            _listNumber++;

            codeBuilder.AppendLine(indentLevel, $"var {listName} = {valueGetter};");
            codeBuilder.AppendLine(indentLevel, $"for(int index = 0; index < {valueGetter}.Count-1; index++)");
            codeBuilder.AppendLine(indentLevel, "{");
            
            generator.GenerateToJson(codeBuilder, indentLevel+1, appendBuilder, listElementType, $"{listName}[index]");

            appendBuilder.Append(",");
            codeBuilder.MakeAppend(indentLevel+1, appendBuilder);


            codeBuilder.AppendLine(indentLevel, "}");

            codeBuilder.AppendLine(indentLevel, $"if({valueGetter}.Count > 1)");
            codeBuilder.AppendLine(indentLevel, "{");
            generator.GenerateToJson(codeBuilder, indentLevel+1, appendBuilder, listElementType, $"{listName}[{valueGetter}.Count-1]");
            codeBuilder.AppendLine(indentLevel, "}");

            appendBuilder.Append("]");
        }
    }
}