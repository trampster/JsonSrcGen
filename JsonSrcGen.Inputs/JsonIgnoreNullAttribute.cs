using System;

namespace JsonSrcGen
{
    [AttributeUsage(AttributeTargets.Class)]
    public class JsonIgnoreNullAttribute : Attribute
    {
    }
}