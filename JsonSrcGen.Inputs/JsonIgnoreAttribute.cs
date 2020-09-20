using System;

namespace JsonSrcGen
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class JsonIgnoreAttribute : Attribute
    {
        public JsonIgnoreAttribute()
        {
        }
    }
}