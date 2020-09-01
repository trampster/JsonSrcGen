using System.Text;
using System;
using System.Linq;
using JsonSG.Generator.PropertyHashing;
using System.Collections.Generic;

namespace JsonSG.Generator
{
    public class FromJsonGenerator
    {

        public void Generate(JsonClass jsonClass, CodeBuilder classBuilder)
        {
            classBuilder.AppendLine(2, $"public void FromJson({jsonClass.Namespace}.{jsonClass.Name} value, string jsonString)");
            classBuilder.AppendLine(2, "{");

            classBuilder.AppendLine(3, "var json = jsonString.AsSpan();");

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
            classBuilder.AppendLine(5, "return;");
            classBuilder.AppendLine(4, "}");

            classBuilder.AppendLine(3, "}");
            classBuilder.AppendLine(2, "}");
        }

        public void GenerateProperties(IReadOnlyCollection<JsonProperty> properties, int indentLevel, CodeBuilder classBuilder)
        {
            var propertyHashFactory = new PropertyHashFactory();
            var propertyHash = propertyHashFactory.FindBestHash(properties.Select(p => p.Name).ToArray());

            var hashesQuery =
                from property in properties
                let hash = propertyHash.Hash(property.Name)
                group property by hash into hashGroup
                orderby hashGroup.Key
                select hashGroup;

            var hashes = hashesQuery.ToArray();
            var switchGroups = FindSwitchGroups(hashes);

            foreach(var switchGroup in switchGroups)
            {
                // if(switchGroup.Count <= 2)
                // {
                //     GenerateIfGroup(switchGroup, propertyHandlers, unknownPropertyLabel, loopCheckLabel, hashLocal);
                //     continue;
                // }
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
                classBuilder.AppendLine(indentLevel+2, $"if(!propertyName.EqualsString(\"{property.Name}\"))");
                classBuilder.AppendLine(indentLevel+2, "{");
                classBuilder.AppendLine(indentLevel+3, "break;"); //todo: need to read to the next property (could be an object list so need to count '{' and '}')
                classBuilder.AppendLine(indentLevel+2, "}");
                switch(property.Type)
                {
                    case "Int32":
                        classBuilder.AppendLine(indentLevel+2, $"json = json.ReadInt(out int property{property.Name}Value);");
                        break;
                    case "UInt32":
                        classBuilder.AppendLine(indentLevel+2, $"json = json.ReadUInt(out uint property{property.Name}Value);");
                        break;
                    case "UInt64":
                        classBuilder.AppendLine(indentLevel+2, $"json = json.ReadULong(out ulong property{property.Name}Value);");
                        break;
                    case "Int16":
                        classBuilder.AppendLine(indentLevel+2, $"json = json.ReadShort(out short property{property.Name}Value);");
                        break;
                    case "UInt16":
                        classBuilder.AppendLine(indentLevel+2, $"json = json.ReadUShort(out ushort property{property.Name}Value);");
                        break;
                    case "Byte":
                        classBuilder.AppendLine(indentLevel+2, $"json = json.ReadByte(out byte property{property.Name}Value);");
                        break;
                    case "String":
                        classBuilder.AppendLine(indentLevel+2, $"json = json.ReadString(out string property{property.Name}Value);");
                        break;
                    case "Boolean":
                        classBuilder.AppendLine(indentLevel+2, $"json = json.ReadBool(out bool property{property.Name}Value);");
                        break;
                    default:
                        throw new Exception($"Unsupported type {property.Type} in From Json"); 
                }
                classBuilder.AppendLine(indentLevel+2, $"value.{property.Name} = property{property.Name}Value;");
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