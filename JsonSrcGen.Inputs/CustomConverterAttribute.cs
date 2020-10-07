using System;

namespace JsonSrcGen
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    internal class CustomConverterAttribute : Attribute
    {
        public CustomConverterAttribute(Type listType)
        {
        }

        public Type ListType {get;}
    }
}