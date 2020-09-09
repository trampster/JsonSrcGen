using System.Text;
using System.Linq;
using System;
using System.Collections.Generic;
using JsonSGen.TypeGenerators;
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

        readonly IJsonGenerator _customTypeGenerator = new CustomTypeGenerator();
        readonly Dictionary<string, IJsonGenerator> _generators;

        public ToJsonGenerator(IEnumerable<IJsonGenerator> generators)
        {
            _generators = new Dictionary<string, IJsonGenerator>();
            foreach(var generator in generators)
            {
                _generators.Add(generator.TypeName, generator);
            }
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
                generator.GenerateToJson(classBuilder, 3, appendBuilder, property);

                if(isFirst) isFirst = false;
            }
            appendBuilder.Append("}"); 
            classBuilder.MakeAppend(3, appendBuilder);
            classBuilder.AppendLine(2, "}");
        }

        IJsonGenerator GetGeneratorForType(JsonType type)
        {
            if(type.IsCustomType)
            {
                return _customTypeGenerator;
            } 
            if(_generators.TryGetValue(type.Name, out var generator))
            {
                return generator;
            }
            throw new Exception($"Unsupported type {type.FullName} in to json generator");
        }
    }
}