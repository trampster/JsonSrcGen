using System;

namespace JsonSrcGen
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class JsonOptionalAttribute : Attribute
    {
        public JsonOptionalAttribute()
        {
        }
    }
}