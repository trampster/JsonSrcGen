namespace JsonSrcGen
{
    public class JsonProperty
    {
        public JsonProperty(JsonType type, string jsonName, string codeName, bool optional)
        {
            Type = type;
            JsonName = jsonName;
            CodeName = codeName;
            Optional = optional;
        }

        public JsonType Type {get;}

        public string JsonName {get;}
        public string CodeName {get;}
        public bool Optional {get;}
    }
}