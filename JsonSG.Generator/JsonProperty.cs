namespace JsonSG.Generator
{
    public class JsonProperty
    {
        public JsonProperty(string type, string name)
        {
            Type = type;
            Name = name;
        }

        public string Type {get;}

        public string Name {get;}
    }
}