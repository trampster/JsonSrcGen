using System;
using System.Text;
using JsonSrcGen;

namespace JsonSrcGen.TypeGenerators
{
    public interface IJsonGenerator
    {
        string GeneratorId {get; }

        void GenerateFromJson(CodeBuilder codeBuilder, int inputLevel, JsonType type, Func<string, string> valueSetter, string valueGetter);

        void GenerateToJson(CodeBuilder codeBuilder, int inputLevel, StringBuilder appendBuilder, JsonType type, string valueGetter, bool canBeNull);

        CodeBuilder ClassLevelBuilder {get;}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeBuilder"></param>
        /// <param name="indentLevel"></param>
        /// <param name="valueSetter"></param>
        /// <returns>wasSetVariableName which is used to track if the property was set, or null if it isn't tracked</returns>
        string OnNewObject(CodeBuilder codeBuilder, int indentLevel, Func<string, string> valueSetter);

        void OnObjectFinished(CodeBuilder codeBuilder, int indentLevel, Func<string, string> valueSetter, string wasSetVariableName);
    }
}