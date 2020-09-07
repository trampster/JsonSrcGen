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


                switch(property.Type)
                {
                    case "String":
                        classBuilder.AppendLine(3, $"if(value.{property.CodeName} == null)");
                        classBuilder.AppendLine(3, "{");
                        var nullAppendBuilder = new StringBuilder(appendBuilder.ToString());
                        nullAppendBuilder.Append("null");
                        classBuilder.MakeAppend(4, nullAppendBuilder);
                        classBuilder.AppendLine(3, "}");

                        classBuilder.AppendLine(3, "else");
                        classBuilder.AppendLine(3, "{");
                        appendBuilder.Append($"\\\"");
                        classBuilder.MakeAppend(4, appendBuilder);
                        classBuilder.AppendLine(4, $"builder.AppendEscaped(value.{property.CodeName});");
                        appendBuilder.Append($"\\\"");
                        classBuilder.MakeAppend(4, appendBuilder);
                        classBuilder.AppendLine(3, "}");
                        break;
                    case "Int32":
                    case "UInt32":
                    case "Int64":
                    case "UInt64":
                    case "Int16":
                    case "UInt16":
                    case "Byte":
                    case "Single":
                    case "Double":
                        classBuilder.MakeAppend(3, appendBuilder);
                        classBuilder.AppendLine(3, $"builder.Append(value.{property.CodeName});");
                        break;
                    case "Single?":
                    case "Double?":
                        classBuilder.MakeAppend(3, appendBuilder);
                        classBuilder.AppendLine(3, $"if(value.{property.CodeName} == null)");
                        classBuilder.AppendLine(3, "{");
                        classBuilder.AppendLine(4, $"builder.Append(\"null\");");
                        classBuilder.AppendLine(3, "}");
                        classBuilder.AppendLine(3, "else");
                        classBuilder.AppendLine(3, "{");
                        classBuilder.AppendLine(4, $"builder.Append(value.{property.CodeName});");
                        classBuilder.AppendLine(3, "}");
                        break;
                    case "UInt32?":
                    case "UInt16?":
                    case "Byte?":
                    case "Int32?":
                    case "Int16?":
                    case "UInt64?":
                    case "Int64?":
                        classBuilder.MakeAppend(3, appendBuilder);
                        classBuilder.AppendLine(3, $"if(value.{property.CodeName} == null)");
                        classBuilder.AppendLine(3, "{");
                        classBuilder.AppendLine(4, $"builder.Append(\"null\");");
                        classBuilder.AppendLine(3, "}");
                        classBuilder.AppendLine(3, "else");
                        classBuilder.AppendLine(3, "{");
                        classBuilder.AppendLine(4, $"builder.Append(value.{property.CodeName});");
                        classBuilder.AppendLine(3, "}");
                        break;
                    case "Boolean":
                        classBuilder.MakeAppend(3, appendBuilder);
                        classBuilder.AppendLine(3, $"builder.Append(value.{property.CodeName} ? \"true\" : \"false\");");
                        break;
                    case "Boolean?":
                        classBuilder.MakeAppend(3, appendBuilder);
                        classBuilder.AppendLine(3, $"builder.Append(value.{property.CodeName} == null ? \"null\" : value.{property.CodeName}.Value ? \"true\" : \"false\");");
                        break;
                    default:
                        if(_generators.TryGetValue(property.Type, out var generator))
                        {
                            generator.GenerateToJson(classBuilder, 3, appendBuilder, property);
                            break;
                        }
                        throw new Exception($"Unsupported type {property.Type} in to json generator");

                }

                if(isFirst) isFirst = false;
            }
            appendBuilder.Append("}"); 
            classBuilder.MakeAppend(3, appendBuilder);
            classBuilder.AppendLine(3, "return builder.ToString();"); 
            classBuilder.AppendLine(2, "}");
        }
    }
}