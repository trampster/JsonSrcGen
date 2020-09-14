using System;
using System.Text;
using JsonSrcGen.Generator;

namespace JsonSrcGen.Generator.TypeGenerators
{
    public interface IJsonGenerator
    {
        string TypeName {get; }

        void GenerateFromJson(CodeBuilder codeBuilder, int inputLevel, JsonType type, Func<string, string> valueSetter, string valueGetter);

        void GenerateToJson(CodeBuilder codeBuilder, int inputLevel, StringBuilder appendBuilder, JsonType type, string valueGetter);

        CodeBuilder ClassLevelBuilder {get;}
    }
}