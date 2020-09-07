namespace JsonSGen.Generator
{
    public class JsonProperty
    {
        public JsonProperty(string type, string jsonName, string codeName)
        {
            Type = type;
            JsonName = jsonName;
            CodeName = codeName;
        }

        public string Type {get;}

        public string JsonName {get;}
        public string CodeName {get;}

    }
}