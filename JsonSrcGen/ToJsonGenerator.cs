using JsonSrcGen.TypeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonSrcGen
{
    public class ToJsonGenerator
    {
        const string BuilderText = @"
            var builder = Builder;
            builder.Clear();";

        readonly Func<JsonType, IJsonGenerator> _getGeneratorForType;

        public ToJsonGenerator(Func<JsonType, IJsonGenerator> getGeneratorForType)
        {
            _getGeneratorForType = getGeneratorForType;
        }

        public void Generate(JsonClass jsonClass, CodeBuilder classBuilder)
        {
            if (jsonClass.StructRef)
            {
                classBuilder.AppendLine(2, $"public static ReadOnlySpan<char> ToJson(ref {jsonClass.Namespace}.{jsonClass.Name} value)");
            }
            else
            {
                classBuilder.AppendLine(2, $"public static ReadOnlySpan<char> ToJson({jsonClass.Namespace}.{jsonClass.Name} value)");
            }

            classBuilder.AppendLine(2, "{");
            classBuilder.AppendLine(0, BuilderText);
            classBuilder.AppendLine(3, "ToJson(value, builder);");
            classBuilder.AppendLine(3, "return builder.AsSpan();");
            classBuilder.AppendLine(2, "}");

            classBuilder.AppendLine(2, $"[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]");
            classBuilder.AppendLine(2, $"public static void ToJson({jsonClass.Namespace}.{jsonClass.Name} value, JsonStringBuilder builder)");
            classBuilder.AppendLine(2, "{");

            var appendBuilder = new StringBuilder();
            appendBuilder.Append("{");

            bool isFirst = true;
            foreach(var property in jsonClass.Properties.OrderBy(p => p.JsonName))
            {
                int indent = 3;

                if(jsonClass.IgnoreNull && property.Type.CanBeNull)
                {
                    classBuilder.MakeAppend(indent, appendBuilder);
                    classBuilder.AppendLine(indent, $"if(value.{property.CodeName} != null)");
                    classBuilder.AppendLine(indent, "{");
                    indent++;

                }
                if(!isFirst)
                {
                    appendBuilder.Append(",");
                }

                appendBuilder.Append($"\\\"");
                appendBuilder.AppendDoubleEscaped(property.JsonName);
                appendBuilder.Append($"\\\":");

                var generator = GetGeneratorForType(property.Type);
                bool canBeNull = jsonClass.IgnoreNull ? false : property.Type.CanBeNull;
                generator.GenerateToJson(classBuilder, indent, appendBuilder, property.Type, $"value.{property.CodeName}", canBeNull);

                if(jsonClass.IgnoreNull && property.Type.CanBeNull)
                {
                    indent--;
                    classBuilder.AppendLine(indent, "}");
                }

                if(isFirst) isFirst = false;
            }
            appendBuilder.Append("}"); 
            classBuilder.MakeAppend(3, appendBuilder);
            classBuilder.AppendLine(2, "}");
        }

        public void GenerateList(JsonType type, CodeBuilder codeBuilder) 
        {
            codeBuilder.AppendLine(2, $"public static ReadOnlySpan<char> ToJson(List<{type.Namespace}.{type.Name}> value)");
            codeBuilder.AppendLine(2, "{");
            codeBuilder.AppendLine(0, BuilderText);

            var listJsonType = new JsonType("List", "List", "System.Collection.Generic", false, new List<JsonType>(){type}, true, true);
            var generator = _getGeneratorForType(listJsonType);
            generator.GenerateToJson(codeBuilder, 3, new StringBuilder(), listJsonType, "value", listJsonType.CanBeNull);

            codeBuilder.AppendLine(3, "return builder.AsSpan();");
            codeBuilder.AppendLine(2, "}"); 
        }

        public void GenerateArray(JsonType type, CodeBuilder codeBuilder) 
        {

            codeBuilder.AppendLine(2, $"public static ReadOnlySpan<char> ToJson(ref {type.Namespace}.{type.Name}[] value)");
            codeBuilder.AppendLine(2, "{");
            codeBuilder.AppendLine(0, BuilderText);

            var arrayJsonType = new JsonType("Array", "Array", "NA", false, new List<JsonType>(){type}, true, true);
            var generator = _getGeneratorForType(arrayJsonType);
            generator.GenerateToJson(codeBuilder, 3, new StringBuilder(), arrayJsonType, "value", arrayJsonType.CanBeNull);

            codeBuilder.AppendLine(3, "return builder.AsSpan();");
            codeBuilder.AppendLine(2, "}"); 
        }

        public void GenerateValue(JsonType type, CodeBuilder codeBuilder) 
        {
            codeBuilder.AppendLine(2, $"public static ReadOnlySpan<char> ToJson({type.Namespace}.{type.Name} value)");
            codeBuilder.AppendLine(2, "{");
            codeBuilder.AppendLine(0, BuilderText);

            var generator = _getGeneratorForType(type);
            generator.GenerateToJson(codeBuilder, 3, new StringBuilder(), type, "value", type.CanBeNull);

            codeBuilder.AppendLine(3, "return builder.AsSpan();");
            codeBuilder.AppendLine(2, "}"); 
        }

        public void GenerateDictionary(JsonType keyType, JsonType valueType, CodeBuilder codeBuilder) 
        {
            codeBuilder.AppendLine(2, $"public static ReadOnlySpan<char> ToJson(Dictionary<{keyType.FullName},{valueType.FullName}> value)");
            codeBuilder.AppendLine(2, "{");
            codeBuilder.AppendLine(0, BuilderText);

            var arrayJsonType = new JsonType("Dictionary", "Dictionary", "NA", false, new List<JsonType>(){keyType, valueType}, true, true);
            var generator = _getGeneratorForType(arrayJsonType);
            generator.GenerateToJson(codeBuilder, 3, new StringBuilder(), arrayJsonType, "value", arrayJsonType.CanBeNull);

            codeBuilder.AppendLine(3, "return builder.AsSpan();");
            codeBuilder.AppendLine(2, "}"); 
        }

        IJsonGenerator GetGeneratorForType(JsonType type)
        {
            return _getGeneratorForType(type);
        }
    }
}