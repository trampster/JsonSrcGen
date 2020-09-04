using System.Text;
using JsonSGen.Generator;

namespace JsonSGen.TypeGenerators
{
    public interface IJsonGenerator
    {
        string TypeName {get; }

        void GenerateFromJson(CodeBuilder codeBuilder, int inputLevel, JsonProperty jsonProperty);

        void GenerateToJson(CodeBuilder codeBuilder, int inputLevel, StringBuilder appendBuilder, JsonProperty jsonProperty);
    }
}