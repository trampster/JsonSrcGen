using System.Collections.Generic;

namespace JsonSrcGen
{
    public class JsonType
    {
        public JsonType(string generatorId, string name, string typeNamespace, bool isCustomType, List<JsonType> genericArguments, bool canBeNull, bool isReferenceType)
        {
            GeneratorId = generatorId;
            Name = name;
            Namespace = typeNamespace;
            IsCustomType = isCustomType;
            GenericArguments = genericArguments;
            CanBeNull = canBeNull;
            IsReferenceType = isReferenceType;
        }

        public string GeneratorId {get;}
        public string Name {get;}
        public string Namespace {get;}
        public string FullName => $"{Namespace}.{Name}";
        public bool IsCustomType {get;}
        public bool CanBeNull {get;}
        public List<JsonType> GenericArguments {get;}
        public bool IsReferenceType {get;}
        public string NullibleReferenceTypeAnnotation => IsReferenceType ? "?" : "";
        public string FullNameWithNullableAnnotation => $"{FullName}{NullibleReferenceTypeAnnotation}";
    }
}