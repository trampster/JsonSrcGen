using JsonSGen.Generator.TypeGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JsonSGen.Generator
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
                        "An exception was thrown by the JsonSGen generator",
                        "An exception was thrown by the JsonSGen generator: '{0}'",
                        "JsonSGen",
                        DiagnosticSeverity.Error,
                        isEnabledByDefault: true), 
                    Location.None,
                    e.ToString() + e.StackTrace));
            }
        }

        Dictionary<string, IJsonGenerator> _generators;

        IJsonGenerator GetGeneratorForType(JsonType type)
        {
            if(_generators.TryGetValue(type.GeneratorId, out var generator))
            {
                return generator;
            }
            throw new Exception($"Unsupported type {type.FullName} in from json generator");
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
using System.Collections.Generic;

namespace JsonSGen
{
    public class JsonSGenConvert
    {
        [ThreadStatic]
        StringBuilder Builder;
");


            Compilation compilation = context.Compilation;

            var classes = GetJsonClassInfo(receiver.CandidateClases, compilation);

            var generators = new IJsonGenerator[]
            {
                new DateTimeGenerator(),
                new NullableDateTimeGenerator(),
                new GuidGenerator(),
                new NullableGuidGenerator(),
                new AppendReadGenerator("Int32"),
                new AppendReadGenerator("UInt32"),
                new AppendReadGenerator("UInt64"),
                new AppendReadGenerator("Int64"),
                new AppendReadGenerator("Int16"),
                new AppendReadGenerator("UInt16"),
                new AppendReadGenerator("Byte"),
                new AppendReadGenerator("Double"),
                new AppendReadGenerator("Single") {ReadType="Double"},
                new NullableAppendReadGenerator("UInt64?"),
                new NullableAppendReadGenerator("UInt32?"),
                new NullableAppendReadGenerator("UInt16?") {ReadType="UInt32?"},
                new NullableAppendReadGenerator("Byte?") {ReadType="UInt32?"},
                new NullableAppendReadGenerator("Int32?"),
                new NullableAppendReadGenerator("Int16?") {ReadType="Int32?"},
                new NullableAppendReadGenerator("Int64?"),
                new NullableAppendReadGenerator("Double?"),
                new NullableAppendReadGenerator("Single?") {ReadType="Double?"},
                new BoolGenerator(),
                new NullableBoolGenerator(),
                new StringGenerator(),
                new ListGenerator(type => GetGeneratorForType(type)),
                new CustomTypeGenerator()
            };

            _generators = new Dictionary<string, IJsonGenerator>();
            foreach(var generator in generators)
            {
                _generators.Add(generator.TypeName, generator);
            }

            var toJsonGenerator = new ToJsonGenerator(GetGeneratorForType);
            var fromJsonGenerator = new FromJsonGenerator(GetGeneratorForType);

            foreach (var jsonClass in classes)
            {
                toJsonGenerator.Generate(jsonClass, classBuilder);
                fromJsonGenerator.Generate(jsonClass, classBuilder);
            }

            classBuilder.AppendLine(1, "}");
            classBuilder.AppendLine(0, "}");

            try
            {
               File.WriteAllText(Path.Combine("/home/daniel/Work/JsonSG", "Generated", "Generated.cs"), classBuilder.ToString());
            }
            catch(DirectoryNotFoundException)
            {
                //ignore because if it hasn't changed the current path is in the snap directory
            }

            context.AddSource("JsonSGenConvert", SourceText.From(classBuilder.ToString(), Encoding.UTF8));
        }

        IReadOnlyCollection<JsonClass> GetJsonClassInfo(List<ClassDeclarationSyntax> classDeclarations, Compilation compilation)
        {
            var jsonClasses = new List<JsonClass>();

            foreach (var candidateClass in classDeclarations)
            {

                SemanticModel model = compilation.GetSemanticModel(candidateClass.SyntaxTree);
                var classSymbol = model.GetDeclaredSymbol(candidateClass);

                if (HasJsonClassAttribute(classSymbol))
                {
                    string jsonClassName = classSymbol.Name;
                    string jsonClassNamespace = classSymbol.ContainingNamespace.ToString();

                    var jsonProperties = new List<JsonProperty>();

                    foreach(var member in classSymbol.GetMembers().Where(member => member.Kind == SymbolKind.Property))
                    { 
                        var property = member as IPropertySymbol;

                        string jsonPropertyName = null;
                        var attributes = property.GetAttributes();
                        foreach(var attribute in attributes)
                        {
                            if(attribute.AttributeClass.Name == "JsonNameAttribute" && attribute.AttributeClass.ContainingNamespace.Name == "JsonSGen")
                            {
                                jsonPropertyName = (string)attribute.ConstructorArguments.First().Value;
                            }
                        } 

                        string codePropertyName = member.Name;
                        var jsonPropertyType = GetType(member);
                        jsonProperties.Add(new JsonProperty(jsonPropertyType, jsonPropertyName ?? codePropertyName, codePropertyName));
                    }

                    jsonClasses.Add(new JsonClass(jsonClassName, jsonClassNamespace, jsonProperties));
                }
            }
            return jsonClasses; 
        }

        bool HasJsonClassAttribute(ISymbol symbol)
        {
            return symbol.GetAttributes().Any(ad => ad.AttributeClass.Name == "JsonAttribute" && ad.AttributeClass.ContainingNamespace.Name == "JsonSGen");
        }

        JsonType GetType(ISymbol symbol)
        {
            var property = symbol as IPropertySymbol;
            if(property != null)
            {
                return GetType(property.Type);
            }
            throw new Exception($"unsupported member type {symbol} {symbol.GetType()}");
        }

        JsonType GetType(ITypeSymbol typeSymbol)
        {
            if(typeSymbol.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
            {
                var namedType = typeSymbol as INamedTypeSymbol;
                if(namedType != null)
                {
                    string name = $"{namedType.TypeArguments.First().Name}?";
                    return new JsonType(name, name, null, false, GetGenericArguments(typeSymbol));
                }
            }
            bool isCustomType = HasJsonClassAttribute(typeSymbol);
            return new JsonType(isCustomType ? "Custom" : typeSymbol.Name, typeSymbol.Name, typeSymbol.ContainingNamespace.Name, isCustomType, GetGenericArguments(typeSymbol));
        }

        List<JsonType> GetGenericArguments(ITypeSymbol typeSymbol)
        {
            var list = new List<JsonType>();
            var namedType = typeSymbol as INamedTypeSymbol;
            if(namedType != null)
            {
                foreach(var typeArgument in namedType.TypeArguments)
                {
                    list.Add(GetType(typeArgument));
                }
            }
            return list;
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
                CandidateClases.Add(classDeclarationSyntax);
            }
        }
    }
}
