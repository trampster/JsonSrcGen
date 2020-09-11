using System.Text;
using System.Linq;
using System;
using System.Collections.Generic;
using JsonSGen.Generator.TypeGenerators;
using JsonSGen;

namespace JsonSGen.Generator
{
    public class ToJsonGenerator
    {
        const string BuilderText = @"
            var builder = Builder;
            if(builder == null)
            {
                builder = new StringBuilder();
                Builder = builder;
            }
            builder.Clear();";

        readonly Func<JsonType, IJsonGenerator> _getGeneratorForType;

        public ToJsonGenerator(Func<JsonType, IJsonGenerator> getGeneratorForType)
        {
            _getGeneratorForType = getGeneratorForType;
        }

        public void Generate(JsonClass jsonClass, CodeBuilder classBuilder)
        {
            classBuilder.AppendLine(2, $"public string ToJson({jsonClass.Namespace}.{jsonClass.Name} value)");
            classBuilder.AppendLine(2, "{");
            classBuilder.AppendLine(0, BuilderText);
            classBuilder.AppendLine(3, "ToJson(value, builder);");
            classBuilder.AppendLine(3, "return builder.ToString();");
            classBuilder.AppendLine(2, "}"); 


            classBuilder.AppendLine(2, $"public void ToJson({jsonClass.Namespace}.{jsonClass.Name} value, StringBuilder builder)");
            classBuilder.AppendLine(2, "{");

            var appendBuilder = new StringBuilder();
            appendBuilder.Append("{");

            bool isFirst = true;
            foreach(var property in jsonClass.Properties.OrderBy(p => p.JsonName))
            {
                if(!isFirst)
                {
                    appendBuilder.Append(",");
                }

                appendBuilder.Append($"\\\"");
                appendBuilder.AppendDoubleEscaped(property.JsonName);
                appendBuilder.Append($"\\\":");


                var generator = GetGeneratorForType(property.Type);
                generator.GenerateToJson(classBuilder, 3, appendBuilder, property.Type, $"value.{property.CodeName}");

                if(isFirst) isFirst = false;
            }
            appendBuilder.Append("}"); 
            classBuilder.MakeAppend(3, appendBuilder);
            classBuilder.AppendLine(2, "}");
        }

        public void GenerateList(JsonType type, CodeBuilder codeBuilder) 
        {
            codeBuilder.AppendLine(2, $"public string ToJson(List<{type.Namespace}.{type.Name}> value)");
            codeBuilder.AppendLine(2, "{");
            codeBuilder.AppendLine(0, BuilderText);

            var arrayJsonType = new JsonType("List", "List", "System.Coolection.Generic", false, new List<JsonType>(){type});
            var generator = _getGeneratorForType(arrayJsonType);
            generator.GenerateToJson(codeBuilder, 3, new StringBuilder(), arrayJsonType, "value" );

            codeBuilder.AppendLine(3, "return builder.ToString();");
            codeBuilder.AppendLine(2, "}"); 
        }

        IJsonGenerator GetGeneratorForType(JsonType type)
        {
            return _getGeneratorForType(type);
        }
    }
}