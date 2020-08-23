using System.Text;

namespace JsonSG.Generator
{
    public class CodeBuilder
    {
        readonly StringBuilder _stringBuilder = new StringBuilder();

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
    }
}