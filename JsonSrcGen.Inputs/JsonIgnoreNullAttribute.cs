using System;

namespace JsonSrcGen
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class JsonIgnoreNullAttribute : Attribute
    {
    }
}