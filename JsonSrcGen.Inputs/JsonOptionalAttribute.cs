using System;

namespace JsonSrcGen
{
    [AttributeUsage(AttributeTargets.Property)]
    public class JsonOptionalAttribute : Attribute
    {
        public JsonOptionalAttribute()
        {
        }
    }
}