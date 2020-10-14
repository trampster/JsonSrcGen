using System;
using System.Text;
using JsonSrcGen;

namespace JsonSrcGen.TypeGenerators
{
    public class ListGenerator : IJsonGenerator
    {
        readonly Func<JsonType, IJsonGenerator> _getGeneratorForType;
        public string TypeName => "List"; 

        public ListGenerator(Func<JsonType, IJsonGenerator> getGeneratorForType)
        {
            _getGeneratorForType = getGeneratorForType;
        }

        public void GenerateFromJson(CodeBuilder codeBuilder, int indentLevel, JsonType type, Func<string, string> valueSetter, string valueGetter)
        {
            var listElementType = type.GenericArguments[0];
            var generator = _getGeneratorForType(listElementType);

            string foundVariable = $"found{UniqueNumberGenerator.UniqueNumber}";
            codeBuilder.AppendLine(indentLevel, $"json = json.SkipWhitespaceTo('[', 'n', out char {foundVariable});");

            codeBuilder.AppendLine(indentLevel, $"if({foundVariable} == 'n')");
            codeBuilder.AppendLine(indentLevel, "{");
            codeBuilder.AppendLine(indentLevel+1, "json = json.Slice(3);");
            codeBuilder.AppendLine(indentLevel+1, valueSetter("null"));
            codeBuilder.AppendLine(indentLevel, "}");
            codeBuilder.AppendLine(indentLevel, "else");
            codeBuilder.AppendLine(indentLevel, "{");

            indentLevel++;

            codeBuilder.AppendLine(indentLevel, $"if({valueGetter} == null)");
            codeBuilder.AppendLine(indentLevel, "{");
            codeBuilder.AppendLine(indentLevel+1, valueSetter($"new List<{listElementType.FullName}>()"));
            codeBuilder.AppendLine(indentLevel, "}");
            codeBuilder.AppendLine(indentLevel, "else");
            codeBuilder.AppendLine(indentLevel, "{");
            codeBuilder.AppendLine(indentLevel+1, $"{valueGetter}.Clear();");
            codeBuilder.AppendLine(indentLevel, "}");

            

            
            Func<string, string> listAdder = value => $"{valueGetter}.Add({value});";

            codeBuilder.AppendLine(indentLevel, "while(true)");
            codeBuilder.AppendLine(indentLevel, "{");

            codeBuilder.AppendLine(indentLevel+1, "if(json[0] == ']')");
            codeBuilder.AppendLine(indentLevel+1, "{");
            codeBuilder.AppendLine(indentLevel+2, "json = json.Slice(1);");
            codeBuilder.AppendLine(indentLevel+2, "break;");
            codeBuilder.AppendLine(indentLevel+1, "}");

            generator.GenerateFromJson(codeBuilder, indentLevel+1, listElementType, listAdder, null);
            codeBuilder.AppendLine(indentLevel+1, "json = json.SkipWhitespace();");
            codeBuilder.AppendLine(indentLevel+1, "switch (json[0])");
            codeBuilder.AppendLine(indentLevel+1, "{");
            codeBuilder.AppendLine(indentLevel+2, "case ',':");
            codeBuilder.AppendLine(indentLevel+3, "json = json.Slice(1);");
            codeBuilder.AppendLine(indentLevel+3, "continue;");
            codeBuilder.AppendLine(indentLevel+2, "case ']':");
            codeBuilder.AppendLine(indentLevel+3, "json = json.Slice(1);");
            codeBuilder.AppendLine(indentLevel+3, "break;");
            codeBuilder.AppendLine(indentLevel+2, "default:");
            codeBuilder.AppendLine(indentLevel+3, "throw new InvalidJsonException($\"Unexpected character while parsing list Expected ',' or ']' but got '{json[0]}'\", json);");
            codeBuilder.AppendLine(indentLevel+1, "}");
            codeBuilder.AppendLine(indentLevel+1, "break;");
            codeBuilder.AppendLine(indentLevel, "}");
            indentLevel--;
            codeBuilder.AppendLine(indentLevel, "}");

        }

        int _listNumber = 0;

        public void GenerateToJson(CodeBuilder codeBuilder, int indentLevel, StringBuilder appendBuilder, JsonType type, string valueGetter)
        {
            codeBuilder.MakeAppend(indentLevel, appendBuilder);

            string listName = $"list{_listNumber}"; 
            _listNumber++;

            codeBuilder.AppendLine(indentLevel, $"var {listName} = {valueGetter};");
            codeBuilder.AppendLine(indentLevel, $"if({listName} == null)");
            codeBuilder.AppendLine(indentLevel, "{");
            appendBuilder.Append("null");
            codeBuilder.MakeAppend(indentLevel+1, appendBuilder);
            codeBuilder.AppendLine(indentLevel, "}");
            codeBuilder.AppendLine(indentLevel, "else");
            codeBuilder.AppendLine(indentLevel, "{");

            var listElementType = type.GenericArguments[0];
            var generator = _getGeneratorForType(listElementType);
            appendBuilder.Append("[");
            codeBuilder.MakeAppend(indentLevel+1, appendBuilder);


            
            codeBuilder.AppendLine(indentLevel+1, $"for(int index = 0; index < {valueGetter}.Count-1; index++)");
            codeBuilder.AppendLine(indentLevel+1, "{");
            
            generator.GenerateToJson(codeBuilder, indentLevel+2, appendBuilder, listElementType, $"{listName}[index]");

            appendBuilder.Append(",");
            codeBuilder.MakeAppend(indentLevel+2, appendBuilder);


            codeBuilder.AppendLine(indentLevel+1, "}");

            codeBuilder.AppendLine(indentLevel+1, $"if({valueGetter}.Count > 1)");
            codeBuilder.AppendLine(indentLevel+1, "{");
            generator.GenerateToJson(codeBuilder, indentLevel+2, appendBuilder, listElementType, $"{listName}[{valueGetter}.Count-1]");
            codeBuilder.AppendLine(indentLevel+1, "}");

            appendBuilder.Append("]");
            codeBuilder.MakeAppend(indentLevel+1, appendBuilder);
            codeBuilder.AppendLine(indentLevel, "}");
        }

        public CodeBuilder ClassLevelBuilder => null;
    }
}