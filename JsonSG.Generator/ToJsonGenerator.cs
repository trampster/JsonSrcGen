using System.Text;

namespace JsonSG.Generator
{
    public class ToJsonGenerator
    {
        const string BuilderText = @"
            var builder = Builder;
            if(builder == null)
            {
                builder = new StringBuilder();
                Builder = builder;
            }
            builder.Clear();";

        public void Generate(JsonClass jsonClass, CodeBuilder classBuilder)
        {
            classBuilder.AppendLine(2, $"public string ToJson({jsonClass.Namespace}.{jsonClass.Name} value)");
            classBuilder.AppendLine(2, "{");
            classBuilder.AppendLine(0, BuilderText);

            var appendBuilder = new StringBuilder();
            appendBuilder.Append("{");

            bool isFirst = true;
            foreach(var property in jsonClass.Properties)
            {
                if(!isFirst)
                {
                    appendBuilder.Append(",");
                }
                appendBuilder.Append($"\\\"{property.Name}\\\":"); 

                if(property.Type == "String")
                {
                    appendBuilder.Append($"\\\"");
                }
                MakeAppend(classBuilder, appendBuilder);

                classBuilder.AppendLine(3, $"builder.Append(value.{property.Name});");
                if(property.Type == "String")
                {
                    appendBuilder.Append($"\\\"");
                }

                if(isFirst) isFirst = false;
            }
            appendBuilder.Append("}"); 
            MakeAppend(classBuilder, appendBuilder);
            classBuilder.AppendLine(3, "return builder.ToString();"); 
            classBuilder.AppendLine(2, "}");
        }

        void MakeAppend(CodeBuilder classBuilder, StringBuilder appendContent)
        {
            classBuilder.AppendLine(3, $"builder.Append(\"{appendContent.ToString()}\");");
            appendContent.Clear();
        }
    }
}