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
    }

    public class Utf8Literals
    {
        readonly Dictionary<string, Utf8Literal> _literals = new Dictionary<string, Utf8Literal>();

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
    }
}