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
        public void Execute(SourceGeneratorContext context)
        {
            try
            {
                Generate(context);
            }
            catch (Exception e)
            {
                //This is temporary till https://github.com/dotnet/roslyn/issues/46084 is fixed
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor(
                        "SI0000",
                        "An exception was thrown by the JsonSG generator",
                        "An exception was thrown by the JsonSG generator: '{0}'",
                        "JsonSG",
                        DiagnosticSeverity.Error,
                        isEnabledByDefault: true), 
                    Location.None,
                    e.ToString() + e.StackTrace));
            }
        }

        public void Generate(SourceGeneratorContext context)
        {

            // retreive the populated receiver 
            if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
                return;
        
            var classBuilder = new CodeBuilder();

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

            var classes = GetJsonClassInfo(receiver.CandidateClases, compilation);

            var toJsonGenerator = new ToJsonGenerator();
            var fromJsonGenerator = new FromJsonGenerator();

            foreach (var jsonClass in classes)
            {
                toJsonGenerator.Generate(jsonClass, classBuilder);
                fromJsonGenerator.Generate(jsonClass, classBuilder);
            }

            classBuilder.AppendLine(1, "}");
            classBuilder.AppendLine(0, "}");

            File.WriteAllText(Path.Combine($"..", "Generated", "Generated.cs"), classBuilder.ToString());

            context.AddSource("JsonSGConvert", SourceText.From(classBuilder.ToString(), Encoding.UTF8));
        }

        IReadOnlyCollection<JsonClass> GetJsonClassInfo(List<ClassDeclarationSyntax> classDeclarations, Compilation compilation)
        {
            var jsonClasses = new List<JsonClass>();

            foreach (var candidateClass in classDeclarations)
            {

                SemanticModel model = compilation.GetSemanticModel(candidateClass.SyntaxTree);
                var classSymbol = model.GetDeclaredSymbol(candidateClass);

                if (classSymbol.GetAttributes().Any(ad => ad.AttributeClass.Name == "JsonAttribute" && ad.AttributeClass.ContainingNamespace.Name == "JsonSG"))
                {
                    string jsonClassName = classSymbol.Name;
                    string jsonClassNamespace = classSymbol.ContainingNamespace.ToString();

                    var jsonProperties = new List<JsonProperty>();

                    foreach(var member in classSymbol.GetMembers().Where(member => member.Kind == SymbolKind.Property))
                    {
                        var property = member as IPropertySymbol;
                        string jsonPropertyName = member.Name;
                        string jsonPropertyType = GetType(member);
                        jsonProperties.Add(new JsonProperty(jsonPropertyType, jsonPropertyName));
                    }

                    jsonClasses.Add(new JsonClass(jsonClassName, jsonClassNamespace, jsonProperties));
                }
            }
            return jsonClasses; 
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
            var classes = new List<JsonClass>();
            // any method with at least one attribute is a candidate for property generation
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
                && classDeclarationSyntax.AttributeLists.Count > 0)
            {
                var properties = new List<JsonProperty>();

                CandidateClases.Add(classDeclarationSyntax);
            }
        }
    }
}
