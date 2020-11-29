using System;
using System.Collections.Generic;
using System.Text;
using JsonSrcGen;

namespace JsonSrcGen.TypeGenerators
{
    public class ArrayGenerator : IJsonGenerator
    {
        readonly Func<JsonType, IJsonGenerator> _getGeneratorForType;
        readonly CodeBuilder _classLevelBuilder;
        public string GeneratorId => "Array"; 

        readonly Dictionary<string, string> _listLookup = new Dictionary<string, string>();

        public ArrayGenerator(Func<JsonType, IJsonGenerator> getGeneratorForType, CodeBuilder classLevelBuilder)
        {
            _getGeneratorForType = getGeneratorForType;
            _classLevelBuilder = classLevelBuilder;
        }

        public CodeBuilder ClassLevelBuilder => _classLevelBuilder;

        string GenerateThreadStaticList(JsonType type)
        {
            if(_listLookup.TryGetValue(type.FullName, out string listFieldName))
            {
                return listFieldName;
            }

            listFieldName = $"_listBuilder{UniqueNumberGenerator.UniqueNumber}";

            _classLevelBuilder.AppendLine(2, "[ThreadStatic]");
            _classLevelBuilder.AppendLine(2, $"static List<{type.FullName}> {listFieldName};");
            
            _listLookup.Add(type.FullName, listFieldName);
            return listFieldName;
        }


        public void GenerateFromJson(CodeBuilder codeBuilder, int indentLevel, JsonType type, Func<string, string> valueSetter, string valueGetter)
        {
            var listElementType = type.GenericArguments[0];
            var generator = _getGeneratorForType(listElementType);

            string staticBuilderField = GenerateThreadStaticList(type.GenericArguments[0]);
            string builderFieldName = $"listBuilder{UniqueNumberGenerator.UniqueNumber}";

            codeBuilder.AppendLine(indentLevel, $"var {builderFieldName} = {staticBuilderField};");
            codeBuilder.AppendLine(indentLevel, $"if({builderFieldName} == null)");
            codeBuilder.AppendLine(indentLevel, "{");
            codeBuilder.AppendLine(indentLevel+1, $"{builderFieldName} = new List<{type.GenericArguments[0].FullName}>();");
            codeBuilder.AppendLine(indentLevel+1, $"{staticBuilderField} = {builderFieldName};");
            codeBuilder.AppendLine(indentLevel, "}");
            codeBuilder.AppendLine(indentLevel, $"{builderFieldName}.Clear();");

            string foundVariable = $"found{UniqueNumberGenerator.UniqueNumber}";
            codeBuilder.AppendLine(indentLevel, $"json = json.SkipWhitespaceTo('[', 'n', out char {foundVariable});");

            codeBuilder.AppendLine(indentLevel, $"if({foundVariable} == 'n')");
            codeBuilder.AppendLine(indentLevel, "{");
            codeBuilder.AppendLine(indentLevel+1, "json = json.Slice(3);");
            codeBuilder.AppendLine(indentLevel+1, $"{builderFieldName} = null;");
            codeBuilder.AppendLine(indentLevel, "}");
            codeBuilder.AppendLine(indentLevel, "else");
            codeBuilder.AppendLine(indentLevel, "{");

            indentLevel++;

            codeBuilder.AppendLine(indentLevel, $"{builderFieldName}.Clear();");

            Func<string, string> listAdder = value => $"{builderFieldName}.Add({value});";

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

            string arrayName = $"array{UniqueNumberGenerator.UniqueNumber}";

            
            codeBuilder.AppendLine(indentLevel, $"if({builderFieldName} == null)");
            codeBuilder.AppendLine(indentLevel, "{");
            codeBuilder.AppendLine(indentLevel+1, valueSetter("null"));
            codeBuilder.AppendLine(indentLevel, "}");
            codeBuilder.AppendLine(indentLevel, "else");
            codeBuilder.AppendLine(indentLevel, "{");
            

            codeBuilder.AppendLine(indentLevel+1, $"{type.GenericArguments[0].FullName}[] {arrayName};");

            codeBuilder.AppendLine(indentLevel+1, $"if({builderFieldName}.Count == {valueGetter}?.Length)");
            codeBuilder.AppendLine(indentLevel+1, "{");
            codeBuilder.AppendLine(indentLevel+2, $"{arrayName} = {valueGetter};");
            codeBuilder.AppendLine(indentLevel+1, "}");
            codeBuilder.AppendLine(indentLevel+1, "else");
            codeBuilder.AppendLine(indentLevel+1, "{");
            codeBuilder.AppendLine(indentLevel+2, $"{arrayName} = new {type.GenericArguments[0].FullName}[{builderFieldName}.Count];");
            codeBuilder.AppendLine(indentLevel+1, "}");

            codeBuilder.AppendLine(indentLevel+1, $"for(int index = 0; index < {arrayName}.Length; index++)");
            codeBuilder.AppendLine(indentLevel+1, "{");
            codeBuilder.AppendLine(indentLevel+2, $"{arrayName}[index] = {builderFieldName}[index];");
            codeBuilder.AppendLine(indentLevel+1, "}");
            codeBuilder.AppendLine(indentLevel+1, valueSetter(arrayName));

            codeBuilder.AppendLine(indentLevel, "}");
        }

       // int _listNumber = 0;

        public void GenerateToJson(CodeBuilder codeBuilder, int indentLevel, StringBuilder appendBuilder, JsonType type, string valueGetter, bool canBeNull)
        {
            codeBuilder.MakeAppend(indentLevel, appendBuilder);
   
            string listName = $"list{UniqueNumberGenerator.UniqueNumber}"; 

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


            
            codeBuilder.AppendLine(indentLevel, $"for(int index = 0; index < {valueGetter}.Length-1; index++)");
            codeBuilder.AppendLine(indentLevel, "{");
            
            generator.GenerateToJson(codeBuilder, indentLevel+2, appendBuilder, listElementType, $"{listName}[index]", listElementType.CanBeNull);

            appendBuilder.Append(",");
            codeBuilder.MakeAppend(indentLevel+1, appendBuilder);


            codeBuilder.AppendLine(indentLevel, "}");

            codeBuilder.AppendLine(indentLevel, $"if({valueGetter}.Length > 0)");
            codeBuilder.AppendLine(indentLevel, "{");
            generator.GenerateToJson(codeBuilder, indentLevel+1, appendBuilder, listElementType, $"{listName}[{valueGetter}.Length-1]", listElementType.CanBeNull);
            codeBuilder.AppendLine(indentLevel, "}");

            appendBuilder.Append("]");
            codeBuilder.MakeAppend(indentLevel, appendBuilder);
            if(canBeNull)
            {
                indentLevel--;
                codeBuilder.AppendLine(indentLevel, "}");
            }
        }

        public string OnNewObject(CodeBuilder codeBuilder, int indentLevel, Func<string, string> valueSetter)
        {
            codeBuilder.AppendLine(indentLevel, valueSetter("null"));
            return null;
        }

        public void OnObjectFinished(CodeBuilder codeBuilder, int indentLevel, Func<string, string> valueSetter, string wasSetVariable)
        {
            
        } 
    }
}