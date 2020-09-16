using System.Text;
using System;
using System.Linq;
using JsonSrcGen.Generator.PropertyHashing;
using System.Collections.Generic;
using JsonSrcGen.Generator.TypeGenerators;

namespace JsonSrcGen.Generator
{
    public class FromJsonGenerator
    {
        readonly Func<JsonType, IJsonGenerator> _getGeneratorForType;

        public FromJsonGenerator(Func<JsonType, IJsonGenerator> getGeneratorForType)
        {
            _getGeneratorForType = getGeneratorForType;
        }

        public void GenerateList(JsonType type, CodeBuilder codeBuilder) 
        {
            codeBuilder.AppendLine(2, $"public List<{type.Namespace}.{type.Name}> FromJson(List<{type.Namespace}.{type.Name}> value, string jsonString)");
            codeBuilder.AppendLine(2, "{");

            codeBuilder.AppendLine(3, "var json = jsonString.AsSpan();");
            
            var arrayJsonType = new JsonType("List", "List", "System.Coolection.Generic", false, new List<JsonType>(){type});
            var generator = _getGeneratorForType(arrayJsonType);

            generator.GenerateFromJson(codeBuilder, 3, arrayJsonType, value => $"value = {value};", "value");

            codeBuilder.AppendLine(3, "return value;"); 
            codeBuilder.AppendLine(2, "}"); 
        }

        public void GenerateDictionary(JsonType keyType, JsonType valueType, CodeBuilder codeBuilder) 
        {
            codeBuilder.AppendLine(2, $"public Dictionary<{keyType.FullName}, {valueType.FullName}> FromJson(Dictionary<{keyType.FullName}, {valueType.FullName}> value, string jsonString)");
            codeBuilder.AppendLine(2, "{");

            codeBuilder.AppendLine(3, "var json = jsonString.AsSpan();");
            
            var arrayJsonType = new JsonType("Dictionary", "Dictionary", "System.Coolection.Generic", false, new List<JsonType>(){keyType, valueType});
            var generator = _getGeneratorForType(arrayJsonType);

            generator.GenerateFromJson(codeBuilder, 3, arrayJsonType, value => $"value = {value};", "value");

            codeBuilder.AppendLine(3, "return value;"); 
            codeBuilder.AppendLine(2, "}"); 
        }

        public void GenerateArray(JsonType type, CodeBuilder codeBuilder) 
        {
            codeBuilder.AppendLine(2, $"public {type.Namespace}.{type.Name}[] FromJson({type.Namespace}.{type.Name}[] value, string jsonString)");
            codeBuilder.AppendLine(2, "{");

            codeBuilder.AppendLine(3, "var json = jsonString.AsSpan();");
            
            var arrayJsonType = new JsonType("Array", "Array", "NA", false, new List<JsonType>(){type});
            var generator = _getGeneratorForType(arrayJsonType);

            generator.GenerateFromJson(codeBuilder, 3, arrayJsonType, value => $"value = {value};", "value");

            codeBuilder.AppendLine(3, "return value;"); 
            codeBuilder.AppendLine(2, "}"); 
        }

        public void Generate(JsonClass jsonClass, CodeBuilder classBuilder)
        {
            classBuilder.AppendLine(2, $"public void FromJson({jsonClass.Namespace}.{jsonClass.Name} value, string jsonString)");
            classBuilder.AppendLine(2, "{");
            classBuilder.AppendLine(3, "FromJson(value, jsonString.AsSpan());");
            classBuilder.AppendLine(2, "}"); 

            classBuilder.AppendLine(2, $"public ReadOnlySpan<char> FromJson({jsonClass.Namespace}.{jsonClass.Name} value, ReadOnlySpan<char> json)");
            classBuilder.AppendLine(2, "{");

            classBuilder.AppendLine(3, "json = json.SkipWhitespaceTo('{');");

            classBuilder.AppendLine(3, "while(true)");
            classBuilder.AppendLine(3, "{");

            classBuilder.AppendLine(4, "json = json.SkipWhitespaceTo('\\\"');");
            classBuilder.AppendLine(4, "var propertyName = json.ReadTo('\\\"');");
            classBuilder.AppendLine(4, "json = json.Slice(propertyName.Length + 1);");
            classBuilder.AppendLine(4, "json = json.SkipWhitespaceTo(':');");
            
            GenerateProperties(jsonClass.Properties, 4, classBuilder);

            classBuilder.AppendLine(4, "json = json.SkipWhitespaceTo(',', '}', out char found);"); 
            classBuilder.AppendLine(4, "if(found == '}')");
            classBuilder.AppendLine(4, "{");
            classBuilder.AppendLine(5, "return json;");
            classBuilder.AppendLine(4, "}");

            classBuilder.AppendLine(3, "}");
            classBuilder.AppendLine(2, "}");
        }

        public void GenerateProperties(IReadOnlyCollection<JsonProperty> properties, int indentLevel, CodeBuilder classBuilder)
        {
            var propertyHashFactory = new PropertyHashFactory();
            var propertyHash = propertyHashFactory.FindBestHash(properties.Select(p => p.JsonName).ToArray());

            var hashesQuery =
                from property in properties
                let hash = propertyHash.Hash(property.JsonName)
                group property by hash into hashGroup
                orderby hashGroup.Key
                select hashGroup;

            var hashes = hashesQuery.ToArray();
            var switchGroups = FindSwitchGroups(hashes);

            foreach(var switchGroup in switchGroups)
            {
                GenerateSwitchGroup(switchGroup, classBuilder, indentLevel, propertyHash);
            }
        }

        void GenerateSwitchGroup(SwitchGroup switchGroup, CodeBuilder classBuilder, int indentLevel, PropertyHash propertyHash)
        {
            classBuilder.AppendLine(indentLevel, $"switch({propertyHash.GenerateHashCode()})");
            classBuilder.AppendLine(indentLevel, "{");
            
            foreach(var hashGroup in switchGroup)
            {
                classBuilder.AppendLine(indentLevel+1, $"case {hashGroup.Key}:");
                var subProperties = hashGroup.ToArray();
                if(subProperties.Length != 1)
                {
                    GenerateProperties(subProperties, indentLevel+2, classBuilder);
                    classBuilder.AppendLine(indentLevel+2, "break;");
                    continue;
                }
                var property = subProperties[0];
                var propertyNameBuilder = new StringBuilder();
                propertyNameBuilder.AppendDoubleEscaped(property.JsonName);
                string jsonName = propertyNameBuilder.ToString();
                classBuilder.AppendLine(indentLevel+2, $"if(!propertyName.EqualsString(\"{jsonName}\"))");
                classBuilder.AppendLine(indentLevel+2, "{");
                classBuilder.AppendLine(indentLevel+3, "json = json.SkipProperty();");
                classBuilder.AppendLine(indentLevel+3, "break;"); 
                classBuilder.AppendLine(indentLevel+2, "}");

                var generator = _getGeneratorForType(property.Type);
                generator.GenerateFromJson(classBuilder, indentLevel+2, property.Type, propertyValue => $"value.{property.CodeName} = {propertyValue};", $"value.{property.CodeName}");

                classBuilder.AppendLine(indentLevel+2, "break;"); 
            }

            classBuilder.AppendLine(indentLevel, "}"); // end of switch
        }

        class SwitchGroup : List<IGrouping<int, JsonProperty>>{}

        IEnumerable<SwitchGroup> FindSwitchGroups(IGrouping<int, JsonProperty>[] hashes) 
        {
            int last = 0;
            int gaps = 0;
            var switchGroup = new SwitchGroup();
            foreach(var grouping in hashes)
            {
                int hash = grouping.Key;
                gaps += hash - last -1;
                if(gaps > 8)
                {
                    //to many gaps this switch group is finished
                    yield return switchGroup;
                    switchGroup = new SwitchGroup();
                    gaps = 0;
                }
                switchGroup.Add(grouping);
            }
            yield return switchGroup;
        }
    }
}