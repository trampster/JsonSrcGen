using System;

namespace JsonSrcGen
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=true)]
    public class JsonArrayAttribute : Attribute
    {
        public JsonArrayAttribute(Type listType)
        {
        }

        public Type ListType {get;}
    }
}