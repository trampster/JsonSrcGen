using System;

namespace JsonSrcGen
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=true)]
    internal class JsonListAttribute : Attribute
    {
        public JsonListAttribute(Type listType)
        {
        }

        public Type ListType {get;}
    }
}