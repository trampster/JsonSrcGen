using System.Collections.Generic;

namespace JsonSrcGen
{
    public class JsonClass
    {
        public JsonClass(string name, string classNamespace, List<JsonProperty> properties, bool ignoreNull)
        {
            Name = name;
            Namespace = classNamespace;
            Properties = properties;
            IgnoreNull = ignoreNull;
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
    }
}