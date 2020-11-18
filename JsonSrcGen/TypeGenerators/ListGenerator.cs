using System;
using System.Text;
using JsonSrcGen;

namespace JsonSrcGen.TypeGenerators
{
    public class ListGenerator : IJsonGenerator
    {
        readonly Func<JsonType, IJsonGenerator> _getGeneratorForType;
        public string GeneratorId => "List"; 

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
            codeBuilder.AppendLine(indentLevel+1, valueSetter($"new List<{listElementType.FullName}{listElementType.NullibleReferenceTypeAnnotation}>()"));
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

        public void GenerateToJson(CodeBuilder codeBuilder, int indentLevel, StringBuilder appendBuilder, JsonType type, string valueGetter, bool canBeNull)
        {
            codeBuilder.MakeAppend(indentLevel, appendBuilder);

            string listName = $"list{_listNumber}"; 
            _listNumber++;

            codeBuilder.AppendLine(indentLevel, $"var {listName} = {valueGetter};");

            if(canBeNull)
            {
                codeBuilder.AppendLine(indentLevel, $"if({listName} == null)");
                codeBuilder.AppendLine(indentLevel, "{");
                appendBuilder.Append("null");
                codeBuilder.MakeAppend(indentLevel+1, appendBuilder);
                codeBuilder.AppendLine(indentLevel, "}");
                codeBuilder.AppendLine(indentLevel, "else");
                codeBuilder.AppendLine(indentLevel, "{");
                indentLevel++;
            }
            var listElementType = type.GenericArguments[0];
            var generator = _getGeneratorForType(listElementType);
            appendBuilder.Append("[");
            codeBuilder.MakeAppend(indentLevel, appendBuilder);


            
            codeBuilder.AppendLine(indentLevel, $"for(int index = 0; index < {valueGetter}.Count-1; index++)");
            codeBuilder.AppendLine(indentLevel, "{");
            
            generator.GenerateToJson(codeBuilder, indentLevel+1, appendBuilder, listElementType, $"{listName}[index]", listElementType.CanBeNull);

            appendBuilder.Append(",");
            codeBuilder.MakeAppend(indentLevel+1, appendBuilder);


            codeBuilder.AppendLine(indentLevel, "}");

            codeBuilder.AppendLine(indentLevel, $"if({valueGetter}.Count > 0)");
            codeBuilder.AppendLine(indentLevel, "{");
            generator.GenerateToJson(codeBuilder, indentLevel+1, appendBuilder, listElementType, $"{listName}[{valueGetter}.Count-1]", listElementType.CanBeNull);
            codeBuilder.AppendLine(indentLevel, "}");

            appendBuilder.Append("]");
            codeBuilder.MakeAppend(indentLevel, appendBuilder);
            
            if(canBeNull)
            {
                indentLevel--;
                codeBuilder.AppendLine(indentLevel, "}");
            }
        }

        public CodeBuilder ClassLevelBuilder => null;
        
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
            codeBuilder.AppendLine(indentLevel+1, valueSetter($"null"));
            codeBuilder.AppendLine(indentLevel, "}");
        }
    }
}