using System;

namespace JsonSGen
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