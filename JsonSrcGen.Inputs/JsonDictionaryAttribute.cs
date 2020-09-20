using System;

namespace JsonSrcGen
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=true)]
    internal class JsonDictionaryAttribute : Attribute
    {
        public JsonDictionaryAttribute(Type keyType, Type valueType)
        {
            KeyType = keyType;
            ValueType = valueType;
        }

        public Type KeyType {get;}
        public Type ValueType {get;}
    }
}