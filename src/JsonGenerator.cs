using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JsonSG
{


    [Generator]
    public class JsonGenerator : ISourceGenerator
    {
        const string JsonAttributeText = @"using System;

namespace JsonSG
{
    public class JsonAttribute : Attribute
    {
        
    }
}
";

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
            context.AddSource("JsonAttribute", SourceText.From(JsonAttributeText, Encoding.UTF8));

            // retreive the populated receiver 
            if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
                return;
        
            // we're going to create a new compilation that contains the attribute.
            // TODO: we should allow source generators to provide source during initialize, so that this step isn't required.
            CSharpParseOptions options = (context.Compilation as CSharpCompilation).SyntaxTrees[0].Options as CSharpParseOptions;
            Compilation compilation = context.Compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(JsonAttributeText, Encoding.UTF8), options));

            // get the newly bound attribute, and INotifyPropertyChanged
            INamedTypeSymbol attributeSymbol = compilation.GetTypeByMetadataName("JsonSG.JsonAttribute");

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

            var printMethodBuilder = new StringBuilder();
            printMethodBuilder.Append(@"
                public void PrintClassInfo()
                {");

            foreach (var candidateClass in receiver.CandidateClases)
            {
                SemanticModel model = compilation.GetSemanticModel(candidateClass.SyntaxTree);
                var classSymbol = model.GetDeclaredSymbol(candidateClass);

                if (classSymbol.GetAttributes().Any(ad => ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default)))
                {
                    printMethodBuilder.Append($"System.Console.WriteLine(\" Found Class {classSymbol.Name}\");");

                    classBuilder.AppendLine($"public string ToJson({classSymbol.ContainingNamespace}.{classSymbol.Name} value)");
                    classBuilder.AppendLine( "{");
                    classBuilder.AppendLine(BuilderText);

                    var appendBuilder = new StringBuilder();
                    appendBuilder.Append("{");

                    bool isFirst = true;
                    foreach(var member in classSymbol.GetMembers().Where(member => member.Kind == SymbolKind.Property))
                    {
                        var property = member as IPropertySymbol;
                        printMethodBuilder.Append($"System.Console.WriteLine(\" Member {member.Name} Type {property.Type.Name}\");");
                        if(!isFirst)
                        {
                            appendBuilder.Append(",");
                        }
                        appendBuilder.Append($"\\\"{member.Name}\\\":");

                        if(GetType(member, printMethodBuilder) == "String")
                        {
                            appendBuilder.Append($"\\\"");
                        }
                        MakeAppend(classBuilder, appendBuilder);


                        classBuilder.AppendLine($"    builder.Append(value.{member.Name});");
                        if(GetType(member, printMethodBuilder) == "String")
                        {
                            appendBuilder.Append($"\\\"");
                        }

                        if(isFirst) isFirst = false;
                    }
                    appendBuilder.Append("}");
                    MakeAppend(classBuilder, appendBuilder);
                    classBuilder.AppendLine( "    return builder.ToString();");
                    classBuilder.AppendLine( "}");
                }
            }

            printMethodBuilder.Append(@"}");

            classBuilder.Append(printMethodBuilder.ToString());

            classBuilder.Append(@"}}");

            File.WriteAllText("Generated.cs", classBuilder.ToString());

            context.AddSource("JsonSGConvert", SourceText.From(classBuilder.ToString(), Encoding.UTF8));
        }

        string GetType(ISymbol symbol, StringBuilder logger)
        {
            var property = symbol as IPropertySymbol;
            if(property != null)
            {
                return property.Type.Name;
            }

            throw new Exception($"unsupported member type {symbol}");
        }

        void MakeAppend(StringBuilder classBuidler, StringBuilder appendContent)
        {
            classBuidler.Append($"    builder.Append(\"{appendContent.ToString()}\");");
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
