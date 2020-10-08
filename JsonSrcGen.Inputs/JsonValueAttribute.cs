using System;

namespace JsonSrcGen
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=true)]
    internal class JsonValueAttribute : Attribute
    {
        public JsonValueAttribute(Type listType)
        {
        }

        public Type ListType {get;}
    }
}