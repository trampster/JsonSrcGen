using System;

namespace JsonSrcGen
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=true)]
    internal class CustomConverterAttribute : Attribute
    {
        public CustomConverterAttribute(Type listType)
        {
        }

        public Type ListType {get;}
    }
}