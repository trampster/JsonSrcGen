using System;

namespace JsonSrcGen
{
    [AttributeUsage(AttributeTargets.Property)]
    public class JsonIgnoreAttribute : Attribute
    {
        public JsonIgnoreAttribute()
        {
        }
    }
}