using System;
using System.Collections.Generic;
using System.Text;

namespace JsonSrcGen
{
    public class Utf8Literal
    {
        public Utf8Literal(string codeName, byte[] value)
        {
            CodeName = codeName;
            Value = value;
        }

        public string CodeName {get;}

        byte[] Value {get;}

        public void GenerateMatch(CodeBuilder codeBuilder)
        {
            codeBuilder.AppendLine(2, $"// Compares to {Encoding.UTF8.GetString(Value)}");
            codeBuilder.AppendLine(2, "[MethodImpl(MethodImplOptions.AggressiveInlining)]");
            codeBuilder.AppendLine(2, $"public static bool {CodeName}(this ReadOnlySpan<byte> json)");
            codeBuilder.AppendLine(2, "{");
            codeBuilder.AppendLine(3, $"if (json.Length != {Value.Length})");
            codeBuilder.AppendLine(3, "{");
            codeBuilder.AppendLine(4, "return false;");
            codeBuilder.AppendLine(3, "}");

            codeBuilder.Append("            return ");
            for(int index = 0; index < Value.Length - 1; index++)
            {
                codeBuilder.Append($"json[{index}] == {Value[index]} && ");
            }

            codeBuilder.AppendLine(0, $"json[{Value.Length -1}] == {Value[Value.Length -1]};");

            codeBuilder.AppendLine(2, "}");
        }

        public void GenerateCopy(CodeBuilder codeBuilder)
        {
            codeBuilder.AppendLine(2, $"// Copies UTF8 Literal for string {Encoding.UTF8.GetString(Value)}");
            codeBuilder.AppendLine(2, $"public IJsonBuilder {CodeName}()");
            codeBuilder.AppendLine(2, "{");

            codeBuilder.AppendLine(3, $"if (_index + {Value.Length} > _buffer.Length)");
            codeBuilder.AppendLine(3, "{");
            codeBuilder.AppendLine(4, $"ResizeBuffer({Value.Length});");
            codeBuilder.AppendLine(3, "}");

            codeBuilder.AppendLine(3, "var span = _buffer.AsSpan(_index);");

            codeBuilder.AppendLine(3, $"{{ _ = span[{Value.Length -1}]; }}");

            int index = 0;
            foreach(var part in Value)
            {
                codeBuilder.AppendLine(3, $"span[{index}] = {part};");
                index++;
            }

            codeBuilder.AppendLine(3, $"_index += {Value.Length};");
            codeBuilder.AppendLine(3, "return this;");
            codeBuilder.AppendLine(2, "}");
        }
    }

    public class Utf8Literals
    {
        readonly Dictionary<string, Utf8Literal> _literals = new Dictionary<string, Utf8Literal>();
        readonly Dictionary<string, Utf8Literal> _copyLiterals = new Dictionary<string, Utf8Literal>();
        public Utf8Literal GetMatchesLiteral(string value)
        {
            if(_literals.TryGetValue(value, out Utf8Literal literal))
            {
                return literal;
            }
            var newLiteral = new Utf8Literal(GenerateMatchesCodeName(), Encoding.UTF8.GetBytes(value));
            _literals.Add(value, newLiteral);
            return newLiteral;
        }

        public Utf8Literal GetCopyLiteral(string value)
        {
            if(_copyLiterals.TryGetValue(value, out Utf8Literal literal))
            {
                return literal;
            }
            var newLiteral = new Utf8Literal($"{GenerateCopyCodeName()}", Encoding.UTF8.GetBytes(value));
            _copyLiterals.Add(value, newLiteral);
            return newLiteral;
        }

        string GenerateCopyCodeName()
        {
            return $"AppendUtf8Literal{_copyLiterals.Count}";
        }

        string GenerateMatchesCodeName()
        {
            return $"MatchesUtf8Literal{_literals.Count}";
        }

        public void GenerateMatches(CodeBuilder codeBuilder)
        {
            codeBuilder.AppendLine(1, "internal static class JsonUtf8SpanMatchesExtensions");
            codeBuilder.AppendLine(1, "{");
            
            foreach(var literal in _literals.Values)
            {
                literal.GenerateMatch(codeBuilder);
            }

            codeBuilder.AppendLine(1, "}");
        }

        public void GenerateCopy(CodeBuilder codeBuilder)
        {
            foreach(var literal in _copyLiterals.Values)
            {
                literal.GenerateCopy(codeBuilder);
            }
        }
    }
}