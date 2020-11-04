using System;
using System.Text;
using JsonSrcGen;

namespace JsonSrcGen.TypeGenerators
{
    public interface IJsonGenerator
    {
        string TypeName {get; }

        void GenerateFromJson(CodeBuilder codeBuilder, int inputLevel, JsonType type, Func<string, string> valueSetter, string valueGetter);

        void GenerateToJson(CodeBuilder codeBuilder, int inputLevel, StringBuilder appendBuilder, JsonType type, string valueGetter, bool canBeNull);

        CodeBuilder ClassLevelBuilder {get;}

        void OnNewObject(CodeBuilder codeBuilder, int indentLevel, Func<string, string> valueSetter);

        void OnObjectFinished(CodeBuilder codeBuilder, int indentLevel, Func<string, string> valueSetter);
    }
}