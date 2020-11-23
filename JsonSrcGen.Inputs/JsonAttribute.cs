using System;

namespace JsonSrcGen
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class JsonAttribute : Attribute
    {
    }
}