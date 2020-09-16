using JsonSrcGen.Generator.TypeGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JsonSrcGen.Generator
{
    [Generator]
    public class JsonGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
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
                        "An exception was thrown by the JsonSrcGen generator",
                        "An exception was thrown by the JsonSrcGen generator: '{0}'",
                        "JsonSrcGen",
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

        public void Generate(GeneratorExecutionContext context)
        {

            // retreive the populated receiver 
            if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
                return;
        
            var classBuilder = new CodeBuilder();

            classBuilder.Append(@"
using System;
using System.Text;
using System.Collections.Generic;

namespace JsonSrcGen
{
    public class JsonSrcGenConvert
    {
        [ThreadStatic]
        StringBuilder Builder;
");


            Compilation compilation = context.Compilation;

            var classes = GetJsonClassInfo(receiver.CandidateClasses, compilation);

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
                new ArrayGenerator(type => GetGeneratorForType(type), new CodeBuilder()),
                new CustomTypeGenerator()
            };

            _generators = new Dictionary<string, IJsonGenerator>();
            foreach(var generator in generators)
            {
                _generators.Add(generator.TypeName, generator);
            }

            var toJsonGenerator = new ToJsonGenerator(GetGeneratorForType);
            var fromJsonGenerator = new FromJsonGenerator(GetGeneratorForType);

            var listTypes = GetListAttributesInfo(receiver.CandidateAttributes, compilation);
            foreach(var listType in listTypes)
            {
                toJsonGenerator.GenerateList(listType, classBuilder);
                fromJsonGenerator.GenerateList(listType, classBuilder);
            }

            var arrayTypes = GetArrayAttributesInfo(receiver.CandidateAttributes, compilation);
            foreach(var arrayType in arrayTypes)
            {
                toJsonGenerator.GenerateArray(arrayType, classBuilder);
                fromJsonGenerator.GenerateArray(arrayType, classBuilder);
            }

            foreach (var jsonClass in classes)
            {
                toJsonGenerator.Generate(jsonClass, classBuilder);
                fromJsonGenerator.Generate(jsonClass, classBuilder);
            }

            foreach(var generator in _generators)
            {
                var codeBuilder = generator.Value.ClassLevelBuilder;
                if(codeBuilder != null)
                {
                    classBuilder.Append(codeBuilder.ToString());
                }
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

            context.AddSource("JsonSrcGenConvert", SourceText.From(classBuilder.ToString(), Encoding.UTF8));
        }

        IReadOnlyCollection<JsonType> GetListAttributesInfo(List<AttributeSyntax> attributeDeclarations, Compilation compilation)
        {
            var listTypes = new List<JsonType>();
            foreach(var attribute in attributeDeclarations)
            {
                if(attribute.Name.ToString() == "JsonList") 
                {
                    SemanticModel model = compilation.GetSemanticModel(attribute.SyntaxTree);

                    foreach (AttributeArgumentSyntax arg in attribute.ArgumentList.Arguments)
                    {
                        ExpressionSyntax expr = arg.Expression;
                        if(expr is TypeOfExpressionSyntax typeofExpr)
                        {
                            TypeSyntax typeSyntax = typeofExpr.Type;
                            var typeInfo = model.GetTypeInfo(typeSyntax);
                            var jsonType = GetType(typeInfo.Type);
                            listTypes.Add(jsonType);
                        }
                    }
                }
            }
            return listTypes;
        }

        IReadOnlyCollection<JsonType> GetArrayAttributesInfo(List<AttributeSyntax> attributeDeclarations, Compilation compilation)
        {
            var arrayTypes = new List<JsonType>();
            foreach(var attribute in attributeDeclarations)
            {
                if(attribute.Name.ToString() == "JsonArray") 
                {
                    SemanticModel model = compilation.GetSemanticModel(attribute.SyntaxTree);

                    foreach (AttributeArgumentSyntax arg in attribute.ArgumentList.Arguments)
                    {
                        ExpressionSyntax expr = arg.Expression;
                        if(expr is TypeOfExpressionSyntax typeofExpr)
                        {
                            TypeSyntax typeSyntax = typeofExpr.Type;
                            var typeInfo = model.GetTypeInfo(typeSyntax);
                            var jsonType = GetType(typeInfo.Type);
                            arrayTypes.Add(jsonType);
                        }
                    }
                }
            }
            return arrayTypes;
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
                        bool hasIgnoreAttribute = false;
                        foreach(var attribute in attributes)
                        {
                            if(attribute.AttributeClass.Name == "JsonIgnoreAttribute" && attribute.AttributeClass.ContainingNamespace.Name == "JsonSrcGen")
                            {
                                hasIgnoreAttribute = true;
                                break;
                            }
                            if(attribute.AttributeClass.Name == "JsonNameAttribute" && attribute.AttributeClass.ContainingNamespace.Name == "JsonSrcGen")
                            {
                                jsonPropertyName = (string)attribute.ConstructorArguments.First().Value;
                            }
                        }
                        if(hasIgnoreAttribute)
                        {
                            continue;
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
            return symbol.GetAttributes().Any(ad => ad.AttributeClass.Name == "JsonAttribute" && ad.AttributeClass.ContainingNamespace.Name == "JsonSrcGen");
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
                    return new JsonType(name, name, FullNamespace(namedType.TypeArguments.First()), false, GetGenericArguments(typeSymbol));
                }
            }
            if(typeSymbol.TypeKind == TypeKind.Array)
            {
                var arraySymbol = typeSymbol as IArrayTypeSymbol;
                return new JsonType("Array", "", "", false, new List<JsonType>(){GetType(arraySymbol.ElementType)});
            }
            bool isCustomType = HasJsonClassAttribute(typeSymbol);
            return new JsonType(isCustomType ? "Custom" : typeSymbol.Name, typeSymbol.Name, FullNamespace(typeSymbol), isCustomType, GetGenericArguments(typeSymbol));
        }

        string FullNamespace(ITypeSymbol symbol)
        {
            var namespaceBuilder = new List<string>();
            var containingNamespace  = symbol.ContainingNamespace;
            while(true)
            {
                if(containingNamespace.Name != "")
                {
                    namespaceBuilder.Add(containingNamespace.Name);
                }
                if(containingNamespace.ContainingNamespace != null)
                {
                    containingNamespace = containingNamespace.ContainingNamespace;
                    continue;
                }
                break;
            }
            namespaceBuilder.Reverse();

            string fullNamespace = string.Join(".", namespaceBuilder);
            return fullNamespace;
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

        public void Initialize(GeneratorInitializationContext context)
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
        public List<ClassDeclarationSyntax> CandidateClasses { get; } = new List<ClassDeclarationSyntax>();

        public List<AttributeSyntax> CandidateAttributes { get; } = new List<AttributeSyntax>();

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
                CandidateClasses.Add(classDeclarationSyntax);
            }

            if (syntaxNode is AttributeSyntax attributeSyntax)
            {
                CandidateAttributes.Add(attributeSyntax);
            }
        }
    }
}
