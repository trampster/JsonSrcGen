using System.Collections.Generic;

namespace JsonSrcGen
{
    public class JsonClass
    {
        public JsonClass(string name, string classNamespace, List<JsonProperty> properties, bool ignoreNull, bool structRef = false, bool readOnly = false)
        {
            Name = name;
            Namespace = classNamespace;
            Properties = properties;
            IgnoreNull = ignoreNull;
            StructRef = structRef;
            ReadOnly = readOnly;
        }

        public IReadOnlyCollection<JsonProperty> Properties
        {
            get;
        }

        public string Name 
        {
            get;
        }

        public string Namespace
        {
            get;
        }

        public bool IgnoreNull
        {
            get;
        }

        public bool StructRef
        {
            get;
        }

        public bool ReadOnly
        {
            get;
        }


        public string FullName => $"{Namespace}.{Name}";
    }
}