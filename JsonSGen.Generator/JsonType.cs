using System.Collections.Generic;

namespace JsonSGen.Generator
{
    public class JsonType
    {
        public JsonType(string generatorId, string name, string typeNamespace, bool isCustomType, List<JsonType> genericArguments)
        {
            GeneratorId = generatorId;
            Name = name;
            Namespace = typeNamespace;
            IsCustomType = isCustomType;
            GenericArguments = genericArguments;
        }
        public string GeneratorId {get;}
        public string Name {get;}
        public string Namespace {get;}
        public string FullName => $"{Namespace}.{Name}";
        public bool IsCustomType {get;}

        public List<JsonType> GenericArguments {get;}
    }
}