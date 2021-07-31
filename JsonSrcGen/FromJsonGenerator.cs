using System.Text;
using System;
using System.Linq;
using JsonSrcGen.PropertyHashing;
using System.Collections.Generic;
using JsonSrcGen.TypeGenerators;

namespace JsonSrcGen
{
    public class FromJsonGenerator
    {
        readonly Func<JsonType, IJsonGenerator> _getGeneratorForType;
        readonly Utf8Literals _literals;

        public FromJsonGenerator(Func<JsonType, IJsonGenerator> getGeneratorForType, Utf8Literals utf8Literals)
        {
            _getGeneratorForType = getGeneratorForType;
            _literals = utf8Literals;

        }

        public void GenerateList(JsonType type, CodeBuilder codeBuilder)
        {
            codeBuilder.AppendLine(2, $"public List<{type.FullName}{type.NullibleReferenceTypeAnnotation}>? FromJson(List<{type.FullName}{type.NullibleReferenceTypeAnnotation}>? value, ReadOnlySpan<char> json)");
            codeBuilder.AppendLine(2, "{"); 
            
            var arrayJsonType = new JsonType("List", "List", "System.Collection.Generic", false, new List<JsonType>(){type}, true, true);
            var generator = _getGeneratorForType(arrayJsonType);

            generator.GenerateFromJson(codeBuilder, 3, arrayJsonType, value => $"value = {value};", "value", JsonFormat.String);

            codeBuilder.AppendLine(3, "return value;"); 
            codeBuilder.AppendLine(2, "}"); 
        }

        public void GenerateListUtf8(JsonType type, CodeBuilder codeBuilder)
        {
            codeBuilder.AppendLine(2, $"public List<{type.FullName}{type.NullibleReferenceTypeAnnotation}>? FromJson(List<{type.FullName}{type.NullibleReferenceTypeAnnotation}>? value, ReadOnlySpan<byte> json)");
            codeBuilder.AppendLine(2, "{"); 
            
            var arrayJsonType = new JsonType("List", "List", "System.Collection.Generic", false, new List<JsonType>(){type}, true, true);
            var generator = _getGeneratorForType(arrayJsonType);

            generator.GenerateFromJson(codeBuilder, 3, arrayJsonType, value => $"value = {value};", "value", JsonFormat.UTF8);

            codeBuilder.AppendLine(3, "return value;"); 
            codeBuilder.AppendLine(2, "}"); 
        }

        public void GenerateDictionary(JsonType keyType, JsonType valueType, CodeBuilder codeBuilder) 
        {
            codeBuilder.AppendLine(2, $"public Dictionary<{keyType.FullName}, {valueType.FullName}>? FromJson(Dictionary<{keyType.FullName}, {valueType.FullName}>? value, ReadOnlySpan<char> json)");
            codeBuilder.AppendLine(2, "{");
            
            var arrayJsonType = new JsonType("Dictionary", "Dictionary", "System.Coolection.Generic", false, new List<JsonType>(){keyType, valueType}, true, true);
            var generator = _getGeneratorForType(arrayJsonType);

            generator.GenerateFromJson(codeBuilder, 3, arrayJsonType, value => $"value = {value};", "value", JsonFormat.String);

            codeBuilder.AppendLine(3, "return value;"); 
            codeBuilder.AppendLine(2, "}"); 
        }

        public void GenerateDictionaryUtf8(JsonType keyType, JsonType valueType, CodeBuilder codeBuilder) 
        {
            codeBuilder.AppendLine(2, $"public Dictionary<{keyType.FullName}, {valueType.FullName}>? FromJson(Dictionary<{keyType.FullName}, {valueType.FullName}>? value, ReadOnlySpan<byte> json)");
            codeBuilder.AppendLine(2, "{");
            
            var arrayJsonType = new JsonType("Dictionary", "Dictionary", "System.Coolection.Generic", false, new List<JsonType>(){keyType, valueType}, true, true);
            var generator = _getGeneratorForType(arrayJsonType);

            generator.GenerateFromJson(codeBuilder, 3, arrayJsonType, value => $"value = {value};", "value", JsonFormat.UTF8);

            codeBuilder.AppendLine(3, "return value;"); 
            codeBuilder.AppendLine(2, "}"); 
        }

        public void GenerateArray(JsonType type, CodeBuilder codeBuilder) 
        {
            codeBuilder.AppendLine(2, $"public {type.FullName}{type.NullibleReferenceTypeAnnotation}[]? FromJson({type.FullName}{type.NullibleReferenceTypeAnnotation}[]? value, ReadOnlySpan<char> json)");
            codeBuilder.AppendLine(2, "{");

            var arrayJsonType = new JsonType("Array", "Array", "NA", false, new List<JsonType>(){type}, true, true);
            var generator = _getGeneratorForType(arrayJsonType);

            generator.GenerateFromJson(codeBuilder, 3, arrayJsonType, value => $"value = {value};", "value", JsonFormat.String);

            codeBuilder.AppendLine(3, "return value;"); 
            codeBuilder.AppendLine(2, "}"); 
        }

        public void GenerateArrayUtf8(JsonType type, CodeBuilder codeBuilder) 
        {
            codeBuilder.AppendLine(2, $"public {type.FullName}{type.NullibleReferenceTypeAnnotation}[]? FromJson({type.FullName}{type.NullibleReferenceTypeAnnotation}[]? value, ReadOnlySpan<byte> json)");
            codeBuilder.AppendLine(2, "{");

            var arrayJsonType = new JsonType("Array", "Array", "NA", false, new List<JsonType>(){type}, true, true);
            var generator = _getGeneratorForType(arrayJsonType);

            generator.GenerateFromJson(codeBuilder, 3, arrayJsonType, value => $"value = {value};", "value", JsonFormat.UTF8);

            codeBuilder.AppendLine(3, "return value;"); 
            codeBuilder.AppendLine(2, "}"); 
        }

        public void GenerateValue(JsonType type, CodeBuilder codeBuilder) 
        {
            codeBuilder.AppendLine(2, $"public {type.FullName}{type.NullibleReferenceTypeAnnotation} FromJson({type.FullName}{type.NullibleReferenceTypeAnnotation} value, ReadOnlySpan<char> json)");
            codeBuilder.AppendLine(2, "{");

            var generator = _getGeneratorForType(type);

            generator.GenerateFromJson(codeBuilder, 3, type, value => $"value = {value};", "value", JsonFormat.String);

            codeBuilder.AppendLine(3, "return value;"); 
            codeBuilder.AppendLine(2, "}"); 
        }

        public void GenerateValueUtf8(JsonType type, CodeBuilder codeBuilder) 
        {
            codeBuilder.AppendLine(2, $"public {type.FullName}{type.NullibleReferenceTypeAnnotation} FromJson({type.FullName}{type.NullibleReferenceTypeAnnotation} value, ReadOnlySpan<byte> json)");
            codeBuilder.AppendLine(2, "{");

            var generator = _getGeneratorForType(type);

            generator.GenerateFromJson(codeBuilder, 3, type, value => $"value = {value};", "value", JsonFormat.UTF8);

            codeBuilder.AppendLine(3, "return value;"); 
            codeBuilder.AppendLine(2, "}"); 
        }

        public void Generate(JsonClass jsonClass, CodeBuilder codeBuilder)
        {
            if (jsonClass.ReadOnly)
            {
                return;
            }

            if (jsonClass.StructRef)
            {
                codeBuilder.AppendLine(2, $"public ReadOnlySpan<char> FromJson(ref {jsonClass.FullName} value, ReadOnlySpan<char> json)");
            }
            else
            {
                codeBuilder.AppendLine(2, $"public ReadOnlySpan<char> FromJson({jsonClass.FullName} value, ReadOnlySpan<char> json)");
            }

            codeBuilder.AppendLine(2, "{");

            codeBuilder.AppendLine(3, "json = json.SkipToOpenCurlyBracket();");

            var properties = new List<JsonPropertyInstance>();
            foreach(var property in jsonClass.Properties)
            {
                string wasSetVariable = null;
                if(property.Optional)
                {
                    var generator = _getGeneratorForType(property.Type);
                    wasSetVariable = generator.OnNewObject(codeBuilder, 3, propertyValue => $"value.{property.CodeName} = {propertyValue};");
                }
                properties.Add(new JsonPropertyInstance(property.Type, property.JsonName, property.CodeName, property.Optional, wasSetVariable));
            }

            codeBuilder.AppendLine(3, "while(true)");
            codeBuilder.AppendLine(3, "{");
            
            codeBuilder.AppendLine(4, "int jsonIndex = 0;");
            codeBuilder.AppendLine(4, "for (; jsonIndex < json.Length; jsonIndex++)");
            codeBuilder.AppendLine(4, "{");
            codeBuilder.AppendLine(4, "    switch (json[jsonIndex])");
            codeBuilder.AppendLine(4, "    {");
            codeBuilder.AppendLine(4, "        case '\"':");
            codeBuilder.AppendLine(4, "            jsonIndex++;");
            codeBuilder.AppendLine(4, "            goto after;");
            codeBuilder.AppendLine(4, "        case '}':");
            foreach(var property in properties)
            {
                if(property.Optional)
                {
                    var generator = _getGeneratorForType(property.Type);
                    generator.OnObjectFinished(codeBuilder, 7, propertyValue => $"value.{property.CodeName} = {propertyValue};", property.WasSetVariable);
                }
            }
            codeBuilder.AppendLine(4, "            return json.Slice(jsonIndex + 1);");
            codeBuilder.AppendLine(4, "        default:");
            codeBuilder.AppendLine(4, "            continue;");
            codeBuilder.AppendLine(4, "    }");
            codeBuilder.AppendLine(4, "}");
            codeBuilder.AppendLine(4, "throw new InvalidJsonException($\"Unexpected character! expected '}}' or '\\\"' but got '{json[jsonIndex]}'\", json);");
            codeBuilder.AppendLine(4, "after:");

            codeBuilder.AppendLine(4, "json = json.Slice(jsonIndex);");
            codeBuilder.AppendLine(4, "var propertyName = json.ReadToQuote();");
            codeBuilder.AppendLine(4, "json = json.Slice(propertyName.Length + 1);");
            codeBuilder.AppendLine(4, "json = json.SkipToColon();");
            
            GenerateProperties(properties, 4, codeBuilder, JsonFormat.String);

            codeBuilder.AppendLine(3, "}");

            codeBuilder.AppendLine(2, "}");
        } 

        public void GenerateUtf8(JsonClass jsonClass, CodeBuilder codeBuilder)
        {
            if (jsonClass.ReadOnly)
            {
                return;
            }

            if (jsonClass.StructRef)
            {
                codeBuilder.AppendLine(2, $"public ReadOnlySpan<byte> FromJson(ref {jsonClass.FullName} value, ReadOnlySpan<byte> json)");
            }
            else
            {
                codeBuilder.AppendLine(2, $"public ReadOnlySpan<byte> FromJson({jsonClass.FullName} value, ReadOnlySpan<byte> json)");
            }

            codeBuilder.AppendLine(2, "{");

            codeBuilder.AppendLine(3, "json = json.SkipToOpenCurlyBracket();");

            var properties = new List<JsonPropertyInstance>();
            foreach(var property in jsonClass.Properties)
            {
                string wasSetVariable = null;
                if(property.Optional)
                {
                    var generator = _getGeneratorForType(property.Type);
                    wasSetVariable = generator.OnNewObject(codeBuilder, 3, propertyValue => $"value.{property.CodeName} = {propertyValue};");
                }
                properties.Add(new JsonPropertyInstance(property.Type, property.JsonName, property.CodeName, property.Optional, wasSetVariable));
            }

            codeBuilder.AppendLine(3, "while(true)");
            codeBuilder.AppendLine(3, "{");
            
            codeBuilder.AppendLine(4, "int jsonIndex = 0;");
            codeBuilder.AppendLine(4, "for (; jsonIndex < json.Length; jsonIndex++)");
            codeBuilder.AppendLine(4, "{");
            codeBuilder.AppendLine(4, "    switch (json[jsonIndex])");
            codeBuilder.AppendLine(4, "    {");
            codeBuilder.AppendLine(4, "        case (byte)'\"':");
            codeBuilder.AppendLine(4, "            jsonIndex++;");
            codeBuilder.AppendLine(4, "            goto after;");
            codeBuilder.AppendLine(4, "        case (byte)'}':");
            foreach(var property in properties)
            {
                if(property.Optional)
                {
                    var generator = _getGeneratorForType(property.Type);
                    generator.OnObjectFinished(codeBuilder, 7, propertyValue => $"value.{property.CodeName} = {propertyValue};", property.WasSetVariable);
                }
            }
            codeBuilder.AppendLine(4, "            return json.Slice(jsonIndex + 1);");
            codeBuilder.AppendLine(4, "        default:");
            codeBuilder.AppendLine(4, "            continue;");
            codeBuilder.AppendLine(4, "    }");
            codeBuilder.AppendLine(4, "}");
            codeBuilder.AppendLine(4, "throw new InvalidJsonException($\"Unexpected character! expected '}}' or '\\\"' but got '{(char)json[jsonIndex]}'\", Encoding.UTF8.GetString(json));");
            codeBuilder.AppendLine(4, "after:");

            codeBuilder.AppendLine(4, "json = json.Slice(jsonIndex);");
            codeBuilder.AppendLine(4, "var propertyName = json.ReadToQuote();");
            codeBuilder.AppendLine(4, "json = json.Slice(propertyName.Length + 1);");
            codeBuilder.AppendLine(4, "json = json.SkipToColon();");
            
            GenerateProperties(properties, 4, codeBuilder, JsonFormat.UTF8);

            codeBuilder.AppendLine(3, "}");

            codeBuilder.AppendLine(2, "}");
        }

        public void GenerateProperties(IReadOnlyCollection<JsonPropertyInstance> properties, int indentLevel, CodeBuilder classBuilder, JsonFormat jsonFormat)
        {
            var propertyHashFactory = new PropertyHashFactory();
            var propertyHash = propertyHashFactory.FindBestHash(properties.Select(p => p.JsonName).ToArray(), jsonFormat == JsonFormat.UTF8);

            var hashesQuery =
                from property in properties
                let hash = propertyHash.Hash(property.JsonName)
                group property by hash into hashGroup
                orderby hashGroup.Key
                select hashGroup;

            var hashes = hashesQuery.ToArray();
            var switchGroup = GetSwitchGroups(hashes);

            GenerateSwitchGroup(switchGroup, classBuilder, indentLevel, propertyHash, jsonFormat);
        }

        void GenerateSwitchGroup(SwitchGroup switchGroup, CodeBuilder classBuilder, int indentLevel, PropertyHash propertyHash, JsonFormat format)
        {
            classBuilder.AppendLine(indentLevel, $"switch({propertyHash.GenerateHashCode()})");
            classBuilder.AppendLine(indentLevel, "{");
            
            foreach(var hashGroup in switchGroup)
            {
                classBuilder.AppendLine(indentLevel+1, $"case {hashGroup.Key}:");
                var subProperties = hashGroup.ToArray();
                if(subProperties.Length != 1)
                {
                    GenerateProperties(subProperties, indentLevel+2, classBuilder, format);
                    classBuilder.AppendLine(indentLevel+2, "break;");
                    continue;
                }
                var property = subProperties[0];
                
                if(format == JsonFormat.UTF8)
                {
                    var propertyNameBuilder = new StringBuilder();
                    propertyNameBuilder.AppendEscaped(property.JsonName);
                    string jsonName = propertyNameBuilder.ToString();
                    var literal = _literals.GetMatchesLiteral(jsonName);
                    classBuilder.AppendLine(indentLevel+2, $"if(!propertyName.{literal.CodeName}())");
                }
                else if(format == JsonFormat.String)
                {
                    var propertyNameBuilder = new StringBuilder();
                    propertyNameBuilder.AppendDoubleEscaped(property.JsonName);
                    string jsonName = propertyNameBuilder.ToString();
                    classBuilder.AppendLine(indentLevel+2, $"if(!propertyName.EqualsString(\"{jsonName}\"))");
                }
                classBuilder.AppendLine(indentLevel+2, "{");
                classBuilder.AppendLine(indentLevel+3, "json = json.SkipProperty();");
                classBuilder.AppendLine(indentLevel+3, "break;"); 
                classBuilder.AppendLine(indentLevel+2, "}");

                var generator = _getGeneratorForType(property.Type);
                generator.GenerateFromJson(
                    classBuilder, 
                    indentLevel+2, 
                    property.Type, 
                    propertyValue => $"value.{property.CodeName} = {propertyValue};", 
                    $"value.{property.CodeName}",
                    format);
                if(property.WasSetVariable != null)
                {
                    classBuilder.AppendLine(indentLevel+2, $"{property.WasSetVariable} = true;");
                }

                classBuilder.AppendLine(indentLevel+2, "break;"); 
            }

            // default to handle unmapped properties
            classBuilder.AppendLine(indentLevel + 1, "default:");
            classBuilder.AppendLine(indentLevel + 2, "json = json.SkipProperty();");
            classBuilder.AppendLine(indentLevel + 2, "break;");
            
            classBuilder.AppendLine(indentLevel, "}"); // end of switch
        }

        class SwitchGroup : List<IGrouping<int, JsonPropertyInstance>>{}

        SwitchGroup GetSwitchGroups(IGrouping<int, JsonPropertyInstance>[] hashes) 
        {
            var switchGroup = new SwitchGroup();

            foreach(var grouping in hashes)
            {
                switchGroup.Add(grouping);
            }

            return switchGroup;
        }
    }
}