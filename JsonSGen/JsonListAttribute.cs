using System;

namespace JsonSGen
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=true)]
    public class JsonListAttribute : Attribute
    {
        public JsonListAttribute(Type listType)
        {
        }

        public Type ListType {get;}
    }
}