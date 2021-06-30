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

        public void Generate(CodeBuilder codeBuilder)
        {
            if(Value.Length == 0) return;

            codeBuilder.AppendLine(2, $"// UTF8 Literal for string {Encoding.UTF8.GetString(Value)}");
            codeBuilder.Append($"        static readonly byte[] {CodeName} = new byte[]{{");
            for(int index = 0; index < Value.Length -1; index++)
            {
                codeBuilder.Append($"{Value[index]},");
            }

            codeBuilder.AppendLine(0, $"{Value[Value.Length-1]}}};");
        }

        public void GenerateCopy(CodeBuilder codeBuilder)
        {
            codeBuilder.AppendLine(3, $"// Copies UTF8 Literal for string {Encoding.UTF8.GetString(Value)}");
            codeBuilder.AppendLine(3, $"public IJsonBuilder {CodeName}()");
            codeBuilder.AppendLine(3, "{");

            codeBuilder.AppendLine(4, $"if (_index + {Value.Length} > _buffer.Length)");
            codeBuilder.AppendLine(4, "{");
            codeBuilder.AppendLine(5, $"ResizeBuffer({Value.Length});");
            codeBuilder.AppendLine(4, "}");

            codeBuilder.AppendLine(4, "var span = _buffer.AsSpan(_index);");

            codeBuilder.AppendLine(4, $"{{ _ = span[{Value.Length -1}]; }}");

            int index = 0;
            foreach(var part in Value)
            {
                codeBuilder.AppendLine(4, $"span[{index}] = {part};");
                index++;
            }

            codeBuilder.AppendLine(4, $"_index += {Value.Length};");
            codeBuilder.AppendLine(4, "return this;");
            codeBuilder.AppendLine(3, "}");
        }
    }

    public class Utf8Literals
    {
        readonly Dictionary<string, Utf8Literal> _literals = new Dictionary<string, Utf8Literal>();
        readonly Dictionary<string, Utf8Literal> _copyLiterals = new Dictionary<string, Utf8Literal>();
        public Utf8Literal GetStringLiteral(string value)
        {
            if(_literals.TryGetValue(value, out Utf8Literal literal))
            {
                return literal;
            }
            var newLiteral = new Utf8Literal(GenerateCodeName(), Encoding.UTF8.GetBytes(value));
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

        string GenerateCodeName()
        {
            return $"Utf8Literal{_literals.Count}";
        }

        public void Generate(CodeBuilder codeBuilder)
        {
            foreach(var literal in _literals.Values)
            {
                literal.Generate(codeBuilder);
            }
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