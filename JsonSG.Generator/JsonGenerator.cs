using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JsonSG.Generator
{


    [Generator]
    public class JsonGenerator : ISourceGenerator
    {
        const string BuilderText = @"
            var builder = Builder;
            if(builder == null)
            {
                builder = new StringBuilder();
                Builder = builder;
            }
            builder.Clear();";

        public void Execute(SourceGeneratorContext context)
        {
            File.AppendAllText("execute.log", "Execute Started");

            // retreive the populated receiver 
            if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
                return;
        
            StringBuilder classBuilder = new StringBuilder();

            classBuilder.Append(@"
using System;
using System.Text;

namespace JsonSG
{
    public class JsonSGConvert
    {
        [ThreadStatic]
        StringBuilder Builder;
");


            Compilation compilation = context.Compilation;

            foreach (var candidateClass in receiver.CandidateClases)
            {
                SemanticModel model = compilation.GetSemanticModel(candidateClass.SyntaxTree);
                var classSymbol = model.GetDeclaredSymbol(candidateClass);

                foreach(var attribute in classSymbol.GetAttributes())
                {
                    File.AppendAllText("execute.log", $"Attribute Name:{attribute.AttributeClass.Name} namespace:{attribute.AttributeClass.ContainingNamespace.Name}");
                }

                if (classSymbol.GetAttributes().Any(ad => ad.AttributeClass.Name == "JsonAttribute" && ad.AttributeClass.ContainingNamespace.Name == "JsonSG"))
                {
                    AppendLine(classBuilder, 2, $"public string ToJson({classSymbol.ContainingNamespace}.{classSymbol.Name} value)");
                    AppendLine(classBuilder, 2, "{");
                    classBuilder.AppendLine(BuilderText);

                    var appendBuilder = new StringBuilder();
                    appendBuilder.Append("{");

                    bool isFirst = true;
                    foreach(var member in classSymbol.GetMembers().Where(member => member.Kind == SymbolKind.Property))
                    {
                        var property = member as IPropertySymbol;
                        if(!isFirst)
                        {
                            appendBuilder.Append(",");
                        }
                        appendBuilder.Append($"\\\"{member.Name}\\\":");

                        if(GetType(member) == "String")
                        {
                            appendBuilder.Append($"\\\"");
                        }
                        MakeAppend(classBuilder, appendBuilder);

                        AppendLine(classBuilder, 3, $"builder.Append(value.{member.Name});");
                        if(GetType(member) == "String")
                        {
                            appendBuilder.Append($"\\\"");
                        }

                        if(isFirst) isFirst = false;
                    }
                    appendBuilder.Append("}"); 
                    MakeAppend(classBuilder, appendBuilder);
                    AppendLine(classBuilder, 3, "return builder.ToString();");
                    AppendLine(classBuilder, 2, "}");
                }
            }


            AppendLine(classBuilder, 1, "}");
            AppendLine(classBuilder, 0, "}");

            File.WriteAllText("Generated.cs", classBuilder.ToString());

            context.AddSource("JsonSGConvert", SourceText.From(classBuilder.ToString(), Encoding.UTF8));
        }

        void AppendLine(StringBuilder builder, int indentLevel, string text)
        {
            for(int index = 0; index < indentLevel; index++)
            {
                builder.Append("    ");
            }
            builder.AppendLine(text);
        }

        string GetType(ISymbol symbol)
        {
            var property = symbol as IPropertySymbol;
            if(property != null)
            {
                return property.Type.Name;
            }

            throw new Exception($"unsupported member type {symbol}");
        }

        void MakeAppend(StringBuilder classBuilder, StringBuilder appendContent)
        {
            AppendLine(classBuilder, 3, $"builder.Append(\"{appendContent.ToString()}\");");
            appendContent.Clear();
        }

        public void Initialize(InitializationContext context)
        {
            // Register a factory that can create our custom syntax receiver
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }
    }

    /// <summary>
    /// Created on demand before each generation pass
    /// </summary>
    class SyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> CandidateClases { get; } = new List<ClassDeclarationSyntax>();

        /// <summary>
        /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
        /// </summary>
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // any method with at least one attribute is a candidate for property generation
            if (syntaxNode is ClassDeclarationSyntax methodDeclarationSyntax
                && methodDeclarationSyntax.AttributeLists.Count > 0)
            {
                CandidateClases.Add(methodDeclarationSyntax);
            }
        }
    }
}
