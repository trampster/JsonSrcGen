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

    public class JsonPropertyInstance : JsonProperty
    {
        public JsonPropertyInstance(JsonType type, string jsonName, string codeName, bool optional, string wasSetVariable)
            : base(type, jsonName, codeName, optional)
        {
            WasSetVariable = wasSetVariable;
        }

        public string WasSetVariable {get;}
    }
}