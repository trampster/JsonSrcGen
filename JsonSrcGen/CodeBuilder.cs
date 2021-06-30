using System.Text;
using JsonSrcGen.TypeGenerators;

namespace JsonSrcGen
{
    public class CodeBuilder
    {
        readonly StringBuilder _stringBuilder = new StringBuilder();
        readonly Utf8Literals _utf8Literals;

        public CodeBuilder(Utf8Literals utf8Literals)
        {
            _utf8Literals = utf8Literals;
        }

        public void AppendLine(int indentLevel, string text)
        {
            for(int index = 0; index < indentLevel; index++)
            {
                _stringBuilder.Append("    ");
            }
            _stringBuilder.AppendLine(text);
        }

        public void Append(string text)
        {
            _stringBuilder.Append(text);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }

        public string Unescape(string escaped)
        {
            var builder = new StringBuilder();
            for(int index = 0; index < escaped.Length; index++)
            {
                char character = escaped[index];
                if(character != '\\')
                {
                    builder.Append(character);
                    continue;
                }
                index++;
                builder.Append(escaped[index]);
            }

            return builder.ToString();
        }

        public void MakeAppend(int indentLevel, StringBuilder appendContent, JsonFormat format)
        {
            if(appendContent.Length == 0)
            {
                return;
            }
            if(format == JsonFormat.String)
            {
                AppendLine(indentLevel, $"builder.Append(\"{appendContent.ToString()}\");");
            }
            else if (format == JsonFormat.UTF8)
            {
                var literal =_utf8Literals.GetCopyLiteral(Unescape(appendContent.ToString()));
                AppendLine(indentLevel, $"builder.{literal.CodeName}();");
            }
            appendContent.Clear();
        }
    }
}