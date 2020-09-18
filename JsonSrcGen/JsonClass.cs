using System.Collections.Generic;

namespace JsonSrcGen
{
    public class JsonClass
    {
        public JsonClass(string name, string classNamespace, List<JsonProperty> properties)
        {
            Name = name;
            Namespace = classNamespace;
            Properties = properties;
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
    }
}