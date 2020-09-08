using System.Collections.Generic;

namespace JsonSGen.Generator
{
    public class JsonType
    {
        public JsonType(string name, string typeNamespace, bool isCustomType)
        {
            Name = name;
            Namespace = typeNamespace;
            IsCustomType = isCustomType;
        }
        public string Name {get;}
        public string Namespace {get;}
        public string FullName => $"{Namespace}.{Name}";
        public bool IsCustomType {get;}
    }
}