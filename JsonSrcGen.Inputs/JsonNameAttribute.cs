using System;

namespace JsonSrcGen
{
    [AttributeUsage(AttributeTargets.Property)]
    public class JsonNameAttribute : Attribute
    {
        public JsonNameAttribute(string name)
        {
            Name = name;
        }

        public string Name {get;}
    }
}