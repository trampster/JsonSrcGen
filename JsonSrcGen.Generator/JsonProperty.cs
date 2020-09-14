namespace JsonSrcGen.Generator
{
    public class JsonProperty
    {
        public JsonProperty(JsonType type, string jsonName, string codeName)
        {
            Type = type;
            JsonName = jsonName;
            CodeName = codeName;
        }

        public JsonType Type {get;}

        public string JsonName {get;}
        public string CodeName {get;}
    }
}