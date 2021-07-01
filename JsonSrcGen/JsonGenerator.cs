using JsonSrcGen.TypeGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JsonSrcGen
{
    internal class JsonSyntaxReceiver : ISyntaxReceiver
    {
        public List<TypeDeclarationSyntax> Targets = new();

        public List<AttributeSyntax> CandidateAttributes = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
 
            if (syntaxNode is AttributeSyntax attrDeclarationSyntax)
            {
                CandidateAttributes.Add(attrDeclarationSyntax);
            }
            else if (syntaxNode is StructDeclarationSyntax structDeclarationSyntax)
            {
                Targets.Add(structDeclarationSyntax);
            }
            else if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
            {
                Targets.Add(classDeclarationSyntax);
            }
        }
    }

    [Generator]
    public class JsonGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new JsonSyntaxReceiver());
        }

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
            throw new Exception($"Unsupported type {type.FullName} in from json generator, {type.GeneratorId}");
        }

        public void Generate(GeneratorExecutionContext context)
        {

            // retreive the populated receiver
            if (context.SyntaxReceiver is not JsonSyntaxReceiver receiver)
                return;

            Compilation compilation = context.Compilation;

            compilation = GenerateFromResource("GenerationOutputFolderAttribute.cs", context, compilation, null);

            GenerationFolder = GetGenerationOutputFolder(receiver.CandidateAttributes, compilation);
            if(!Directory.Exists(GenerationFolder))
            {
                GenerationFolder = null;
            }
            if(!string.IsNullOrEmpty(GenerationFolder))
            {
                if(File.Exists(Path.Combine(GenerationFolder, "output.log")))
                {
                    File.Delete(Path.Combine(GenerationFolder, "output.log"));
                }
            }

            compilation = GenerateFromResource("InvalidJsonException.cs", context, compilation, GenerationFolder);
            compilation = GenerateFromResource("JsonArrayAttribute.cs", context, compilation, GenerationFolder);
            compilation = GenerateFromResource("JsonValueAttribute.cs", context, compilation, GenerationFolder);
            compilation = GenerateFromResource("JsonAttribute.cs", context, compilation, GenerationFolder);
            compilation = GenerateFromResource("JsonIgnoreNullAttribute.cs", context, compilation, GenerationFolder);
            compilation = GenerateFromResource("JsonDictionaryAttribute.cs", context, compilation, GenerationFolder);
            compilation = GenerateFromResource("JsonIgnoreAttribute.cs", context, compilation, GenerationFolder);
            compilation = GenerateFromResource("JsonOptionalAttribute.cs", context, compilation, GenerationFolder);
            compilation = GenerateFromResource("JsonListAttribute.cs", context, compilation, GenerationFolder);
            compilation = GenerateFromResource("JsonNameAttribute.cs", context, compilation, GenerationFolder);
            compilation = GenerateFromResource("JsonSpanExtensions.cs", context, compilation, GenerationFolder);
            compilation = GenerateFromResource("JsonUtf8SpanExtensions.cs", context, compilation, GenerationFolder);
            compilation = GenerateFromResource("ICustomConverter.cs", context, compilation, GenerationFolder);
            compilation = GenerateFromResource("CustomConverterAttribute.cs", context, compilation, GenerationFolder);
            compilation = GenerateFromResource("JsonUtf8Builder.cs", context, compilation, GenerationFolder);
            compilation = GenerateFromResource("IJsonBuilder.cs", context, compilation, GenerationFolder);
            compilation = GenerateFromResource("JsonStringBuilder.cs", context, compilation, GenerationFolder);
        
            var utf8Literals = new Utf8Literals();

            var classBuilder = new CodeBuilder(utf8Literals);

            classBuilder.Append(@"
using System;
using System.Text;
using System.Collections.Generic;


namespace JsonSrcGen
{
#nullable enable
    public class JsonConverter
    {
        [ThreadStatic]
        JsonStringBuilder? Builder;

        [ThreadStatic]
        JsonUtf8Builder? Utf8Builder;
");
            var classes = GetJsonClassInfo(receiver.Targets, compilation);

            var generators = new IJsonGenerator[] 
            {
                new DateTimeGenerator(),
                new DateTimeOffsetGenerator(),
                new NullableDateTimeGenerator(),
                new NullalbeDateTimeOffsetGenerator(),
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
                new AppendReadGenerator("Decimal"),
                new AppendReadGenerator("Single") {ReadType="Double"},
                new NullableAppendReadGenerator("UInt64?"),
                new NullableAppendReadGenerator("UInt32?"),
                new NullableAppendReadGenerator("UInt16?") {ReadType="UInt32?"},
                new NullableAppendReadGenerator("Byte?") {ReadType="UInt32?"},
                new NullableAppendReadGenerator("Int32?"),
                new NullableAppendReadGenerator("Int16?") {ReadType="Int32?"},
                new NullableAppendReadGenerator("Int64?"),
                new NullableAppendReadGenerator("Double?"),
                new NullableAppendReadGenerator("Decimal?"),
                new NullableAppendReadGenerator("Single?") {ReadType="Double?"},
                new BoolGenerator(),
                new NullableBoolGenerator(),
                new StringGenerator(),
                new ListGenerator(type => GetGeneratorForType(type)),
                new ArrayGenerator(type => GetGeneratorForType(type), new CodeBuilder(utf8Literals)),
                new DictionaryGenerator(type => GetGeneratorForType(type)),
                new CharGenerator()
            };

            _generators = new Dictionary<string, IJsonGenerator>();
            foreach(var generator in generators)
            {
                _generators.Add(generator.GeneratorId, generator);
            }

            foreach(var customClass in classes)
            {
                _generators.Add(customClass.FullName, new CustomTypeGenerator(customClass.FullName));
            }

            var customTypeConverters = GetCustomTypeConverters(receiver.Targets, compilation, utf8Literals);
            foreach (var customTypeConverter in customTypeConverters)
            {
                LogLine($"Adding customTypeConverter GeneratorId: {customTypeConverter.GeneratorId}");

                if(_generators.ContainsKey(customTypeConverter.GeneratorId))
                {
                    LogLine($"overriding existing");
                    _generators[customTypeConverter.GeneratorId] = customTypeConverter;
                }
                else
                {
                    LogLine($"new generator");
                    _generators.Add(customTypeConverter.GeneratorId, customTypeConverter);
                }
            }

            var toJsonGenerator = new ToJsonGenerator(GetGeneratorForType, utf8Literals);
            var fromJsonGenerator = new FromJsonGenerator(GetGeneratorForType ,utf8Literals);

            var listTypes = GetListAttributesInfo(receiver.CandidateAttributes, compilation);
            foreach(var listType in listTypes) 
            {
                toJsonGenerator.GenerateList(listType, classBuilder);
                toJsonGenerator.GenerateListUtf8(listType, classBuilder);
                fromJsonGenerator.GenerateList(listType, classBuilder);
                fromJsonGenerator.GenerateListUtf8(listType, classBuilder);
            }

            var arrayTypes = GetArrayAttributesInfo(receiver.CandidateAttributes, compilation);
            foreach(var arrayType in arrayTypes)
            {
                toJsonGenerator.GenerateArray(arrayType, classBuilder);
                toJsonGenerator.GenerateArrayUtf8(arrayType, classBuilder); 
                fromJsonGenerator.GenerateArray(arrayType, classBuilder);
                fromJsonGenerator.GenerateArrayUtf8(arrayType, classBuilder);
            }

            var dictionaryTypes = GetDictionaryAttributesInfo(receiver.CandidateAttributes, compilation);
            foreach(var dictionaryType in dictionaryTypes)
            {
                toJsonGenerator.GenerateDictionary(dictionaryType.Item1, dictionaryType.Item2, classBuilder);
                toJsonGenerator.GenerateDictionaryUtf8(dictionaryType.Item1, dictionaryType.Item2, classBuilder);
                fromJsonGenerator.GenerateDictionary(dictionaryType.Item1, dictionaryType.Item2, classBuilder);
                fromJsonGenerator.GenerateDictionaryUtf8(dictionaryType.Item1, dictionaryType.Item2, classBuilder);
            }

            foreach (var jsonClass in classes)
            {
                toJsonGenerator.Generate(jsonClass, classBuilder);
                toJsonGenerator.GenerateUtf8(jsonClass, classBuilder);
                fromJsonGenerator.Generate(jsonClass, classBuilder);
                fromJsonGenerator.GenerateUtf8(jsonClass, classBuilder);
            }

            var valueTypes = GetValueAttributesInfo(receiver.CandidateAttributes, compilation);
            foreach(var valueType in valueTypes)
            {
                toJsonGenerator.GenerateValue(valueType, classBuilder);
                toJsonGenerator.GenerateValueUtf8(valueType, classBuilder);
                fromJsonGenerator.GenerateValue(valueType, classBuilder); 
                fromJsonGenerator.GenerateValueUtf8(valueType, classBuilder); 
            }

            foreach(var generator in _generators)
            {
                var codeBuilder = generator.Value.ClassLevelBuilder;
                if(codeBuilder != null)
                {
                    classBuilder.Append(codeBuilder.ToString());
                }
            }

            utf8Literals.Generate(classBuilder);

            classBuilder.AppendLine(1, "}");

            classBuilder.AppendLine(1, "public partial class JsonUtf8Builder");
            classBuilder.AppendLine(1, "{");
            utf8Literals.GenerateCopy(classBuilder);
            classBuilder.AppendLine(1, "}");

            classBuilder.AppendLine(0, "}");
            classBuilder.AppendLine(0, "#nullable restore");
            

            if(GenerationFolder != null)
            {
                try
                {
                    File.WriteAllText(Path.Combine(GenerationFolder, "Generated.cs"), classBuilder.ToString());
                }
                catch(DirectoryNotFoundException)
                {
                    //Don't fail the generation as this makes the CI Unit Tests fail
                }
            }

            context.AddSource("JsonConverter", SourceText.From(classBuilder.ToString(), Encoding.UTF8));
        }

        void LogLine(string line)
        {
            if(GenerationFolder == null)
            {
                return;
            }
            File.AppendAllText(Path.Combine(GenerationFolder, "output.log"), $"{line}{Environment.NewLine}");
        }

        Compilation GenerateFromResource(string name, GeneratorExecutionContext context, Compilation compilation, string GenerationFolder)
        {
            var assembly = typeof(JsonGenerator).Assembly;
            using(Stream resource = assembly.GetManifestResourceStream($"JsonSrcGen.{name}"))
            using(StreamReader reader = new StreamReader(resource))
            {
                string content = reader.ReadToEnd();
                context.AddSource(name, SourceText.From(content, Encoding.UTF8));

                CSharpParseOptions options = (context.Compilation as CSharpCompilation).SyntaxTrees[0].Options as CSharpParseOptions;
                compilation = compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(content, Encoding.UTF8), options));

                if(GenerationFolder != null)
                {
                    try
                    {
                        File.WriteAllText(Path.Combine(GenerationFolder, name), content);
                    }
                    catch(DirectoryNotFoundException)
                    {
                        //Don't fail the generation as this makes the CI Unit Tests fail
                    }
                }

                return compilation;
            }
        }

        IReadOnlyCollection<JsonType> GetListAttributesInfo(List<AttributeSyntax> attributeDeclarations, Compilation compilation)
        {
            var listTypes = new List<JsonType>();
            foreach(var attribute in attributeDeclarations)
            {
                if(attribute.Name.ToString().Contains("JsonList")) 
                {
                    SemanticModel model = compilation.GetSemanticModel(attribute.SyntaxTree);

                    foreach (AttributeArgumentSyntax arg in attribute.ArgumentList.Arguments)
                    {
                        ExpressionSyntax expr = arg.Expression;
                        if(expr is TypeOfExpressionSyntax typeofExpr)
                        {
                            TypeSyntax typeSyntax = typeofExpr.Type;
                            var typeInfo = model.GetTypeInfo(typeSyntax);
                            var jsonType = GetType(typeInfo.Type, model);
                            listTypes.Add(jsonType);
                        }
                    }
                }
            }
            return listTypes;
        }

        string GetGenerationOutputFolder(List<AttributeSyntax> attributeDeclarations, Compilation compilation)
        {
            foreach(AttributeSyntax attribute in attributeDeclarations)
            {
                if(attribute.Name.ToString() == "GenerationOutputFolder") 
                {
                    SemanticModel model = compilation.GetSemanticModel(attribute.SyntaxTree);
                    foreach (AttributeArgumentSyntax arg in attribute.ArgumentList.Arguments)
                    {
                        ExpressionSyntax expr = arg.Expression;

                        Optional<object> value = model.GetConstantValue(expr);
                        return value.ToString();
                    }
                }
            }
            return null;
        }



        IReadOnlyCollection<(JsonType, JsonType)> GetDictionaryAttributesInfo(List<AttributeSyntax> attributeDeclarations, Compilation compilation)
        {
            var listTypes = new List<(JsonType, JsonType)>();
            foreach(var attribute in attributeDeclarations)
            {
                if(attribute.Name.ToString().Contains("JsonDictionary"))
                {
                    SemanticModel model = compilation.GetSemanticModel(attribute.SyntaxTree);
                    var keyType = GetJsonType(attribute.ArgumentList.Arguments[0], model);
                    if(keyType.FullName != "System.String")
                    {
                        throw new NotSupportedException($"JsonSrcGen only supports Dictionary with String keys but was {keyType.FullName}.");
                    }
                    var valueType = GetJsonType(attribute.ArgumentList.Arguments[1], model);
                    listTypes.Add((keyType, valueType));
                }
            }
            return listTypes;
        }

        JsonType GetJsonType(AttributeArgumentSyntax attributeArgumentSyntax, SemanticModel model)
        {
            ExpressionSyntax expr = attributeArgumentSyntax.Expression;
            if(expr is TypeOfExpressionSyntax typeofExpr)
            {
                TypeSyntax typeSyntax = typeofExpr.Type;
                var typeInfo = model.GetTypeInfo(typeSyntax);
                var jsonType = GetType(typeInfo.Type, model);
                return jsonType;
            }
            return null;
        }

        IReadOnlyCollection<JsonType> GetValueAttributesInfo(List<AttributeSyntax> attributeDeclarations, Compilation compilation)
        {
            var arrayTypes = new List<JsonType>();
            foreach(var attribute in attributeDeclarations)
            {
                if(attribute.Name.ToString().Contains("JsonValue"))
                {
                    SemanticModel model = compilation.GetSemanticModel(attribute.SyntaxTree);

                    foreach (AttributeArgumentSyntax arg in attribute.ArgumentList.Arguments)
                    {
                        ExpressionSyntax expr = arg.Expression;
                        if(expr is TypeOfExpressionSyntax typeofExpr)
                        {
                            TypeSyntax typeSyntax = typeofExpr.Type;
                            var typeInfo = model.GetTypeInfo(typeSyntax);
                            var jsonType = GetType(typeInfo.Type, model);
                            arrayTypes.Add(jsonType);
                        }
                    }
                }
            }
            return arrayTypes;
        }

        IReadOnlyCollection<JsonType> GetArrayAttributesInfo(List<AttributeSyntax> attributeDeclarations, Compilation compilation)
        {
            var arrayTypes = new List<JsonType>();
            foreach(var attribute in attributeDeclarations)
            {
                if(attribute.Name.ToString().Contains("JsonArray"))
                {
                    SemanticModel model = compilation.GetSemanticModel(attribute.SyntaxTree);

                    foreach (AttributeArgumentSyntax arg in attribute.ArgumentList.Arguments)
                    {
                        ExpressionSyntax expr = arg.Expression;
                        if(expr is TypeOfExpressionSyntax typeofExpr)
                        {
                            TypeSyntax typeSyntax = typeofExpr.Type;
                            var typeInfo = model.GetTypeInfo(typeSyntax);
                            var jsonType = GetType(typeInfo.Type, model);
                            arrayTypes.Add(jsonType);
                        }
                    }
                }
            }
            return arrayTypes;
        }

        IReadOnlyCollection<JsonClass> GetJsonClassInfo(List<TypeDeclarationSyntax> classDeclarations, Compilation compilation)
        {
            var jsonClasses = new List<JsonClass>();

            foreach (var candidateClass in classDeclarations)
            {
                SemanticModel model = compilation.GetSemanticModel(candidateClass.SyntaxTree);
                var classSymbol = model.GetDeclaredSymbol(candidateClass);

                if (HasJsonClassAttribute(classSymbol))
                {
                    string jsonClassName = classSymbol.Name;
                    string jsonClassNamespace = "";
                    if(!classSymbol.ContainingNamespace.IsGlobalNamespace)
                    {
                        jsonClassNamespace = classSymbol.ContainingNamespace.ToString();
                    }

                    bool ignoreNull = HasJsonIgnoreNullAttribute(classSymbol);

                    bool struct1 =  candidateClass.Kind() == SyntaxKind.StructDeclaration;
                    bool structRef = struct1 || candidateClass.Modifiers.Any(s => s.Value.ToString().ToLower() == "ref");

                    bool readOnly = candidateClass.Modifiers.Any(s => s.Value.ToString().ToLower() == "readonly");

                    var jsonProperties = new List<JsonProperty>();

                    foreach (var member in classSymbol.GetMembers().Where(member => member.Kind == SymbolKind.Property))
                    {
                        var property = member as IPropertySymbol;

                        string jsonPropertyName = null;
                        var attributes = property.GetAttributes();
                        bool hasIgnoreAttribute = false;
                        bool hasOptionalAttribute = false;
                        foreach (var attribute in attributes)
                        {
                            if (attribute.AttributeClass.Name == "JsonIgnoreAttribute" && attribute.AttributeClass.ContainingNamespace.Name == "JsonSrcGen")
                            {
                                hasIgnoreAttribute = true;
                                break;
                            }
                            if (attribute.AttributeClass.Name == "JsonNameAttribute" && attribute.AttributeClass.ContainingNamespace.Name == "JsonSrcGen")
                            {
                                jsonPropertyName = (string)attribute.ConstructorArguments.First().Value;
                            }
                            if (attribute.AttributeClass.Name == "JsonOptionalAttribute" && attribute.AttributeClass.ContainingNamespace.Name == "JsonSrcGen")
                            {
                                hasOptionalAttribute = true;
                            }
                        }
                        if (hasIgnoreAttribute)
                        {
                            continue;
                        }

                        string codePropertyName = member.Name;
                        var jsonPropertyType = GetType(member, model);
                        jsonProperties.Add(new JsonProperty(jsonPropertyType, jsonPropertyName ?? codePropertyName, codePropertyName, hasOptionalAttribute));
                    }

                    jsonClasses.Add(new JsonClass(jsonClassName, jsonClassNamespace, jsonProperties, ignoreNull, structRef, readOnly));
                }
            }
            return jsonClasses;
        }
        static string GenerationFolder;
        IReadOnlyCollection<IJsonGenerator> GetCustomTypeConverters(List<TypeDeclarationSyntax> classDeclarations, Compilation compilation, Utf8Literals utf8Literals)
        {
            var customTypeConverters = new List<IJsonGenerator>();

            foreach (var candidateClass in classDeclarations)
            {

                SemanticModel model = compilation.GetSemanticModel(candidateClass.SyntaxTree);
                var classSymbol = model.GetDeclaredSymbol(candidateClass);

                if (HasCustomConverterAttribute(classSymbol))
                {
                    string converterClassName = classSymbol.Name;
                    string converterNamespace = classSymbol.ContainingNamespace.ToString();

                    var targetType = GetCustomConverterTargetType(classSymbol, model);

                    if(ImplementsInterface(classSymbol, "JsonSrcGen.ICustomConverter"))
                    {
                        customTypeConverters.Add(new CustomConverterGenerator(
                            targetType.GeneratorId, 
                            targetType.FullName, 
                            $"{converterNamespace}.{converterClassName}", 
                            new CodeBuilder(utf8Literals)));
                    }
                }
            }
            return customTypeConverters; 
        }

        bool ImplementsInterface(INamedTypeSymbol symbol, string interfaceFullName)
        {
            foreach(var interfaceSymbol in symbol.Interfaces)
            {
                string actualInterfaceFullName = $"{interfaceSymbol.ContainingNamespace}.{interfaceSymbol.Name}";
                
                if(actualInterfaceFullName == interfaceFullName)
                {
                    return true;
                }
                if(ImplementsInterface(interfaceSymbol, interfaceFullName))
                {
                    return true;
                }
            }
            return false;
        }

        bool HasJsonClassAttribute(ISymbol symbol)
        {
            return symbol.GetAttributes().Any(ad => ad.AttributeClass.Name == "JsonAttribute" && ad.AttributeClass.ContainingNamespace.Name == "JsonSrcGen");
        }

        bool HasJsonIgnoreNullAttribute(ISymbol symbol)
        {
            return symbol.GetAttributes().Any(ad => ad.AttributeClass.Name == "JsonIgnoreNullAttribute" && ad.AttributeClass.ContainingNamespace.Name == "JsonSrcGen");
        }

        bool HasCustomConverterAttribute(ISymbol symbol)
        {
            return symbol.GetAttributes().Any(ad => ad.AttributeClass.Name == "CustomConverterAttribute" && ad.AttributeClass.ContainingNamespace.Name == "JsonSrcGen");
        }

        JsonType GetCustomConverterTargetType(ISymbol symbol, SemanticModel semanticModel)
        {
            var query = 
                from attribute in symbol.GetAttributes()
                where attribute.AttributeClass.Name == "CustomConverterAttribute" && attribute.AttributeClass.ContainingNamespace.Name == "JsonSrcGen"
                select attribute.ConstructorArguments.First().Value;
            var type = query.First();
            var typeSymbol = type as ITypeSymbol;
            if(typeSymbol == null)
            {
                throw new InvalidOperationException("CustomConverter parameter must be a type");
            }
            return GetType(typeSymbol, semanticModel);
        }

        JsonType GetType(ISymbol symbol, SemanticModel semanticModel)
        {
            var property = symbol as IPropertySymbol;
            if(property != null)
            {
                return GetType(property.Type, semanticModel);
            }
            throw new Exception($"unsupported member type {symbol} {symbol.GetType()}");
        }

        JsonType GetType(ITypeSymbol typeSymbol, SemanticModel semanticModel)
        {
            if(typeSymbol.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
            {
                var namedType = typeSymbol as INamedTypeSymbol;
                if(namedType != null)
                {
                    string name = $"{namedType.TypeArguments.First().Name}?";
                    return new JsonType(name, name, FullNamespace(namedType.TypeArguments.First()), false, GetGenericArguments(typeSymbol, semanticModel), true, false);
                }
            }
            if(typeSymbol.TypeKind == TypeKind.Array)
            {
                var arraySymbol = typeSymbol as IArrayTypeSymbol;
                return new JsonType("Array", "", "", false, new List<JsonType>(){GetType(arraySymbol.ElementType, semanticModel)}, true, true);
            }
            bool canBeNull = typeSymbol.IsReferenceType; 
            bool isCustomType = HasJsonClassAttribute(typeSymbol);

            return new JsonType(
                isCustomType ? $"{typeSymbol.ContainingNamespace}.{typeSymbol.Name}" : typeSymbol.Name, 
                typeSymbol.Name, 
                FullNamespace(typeSymbol), 
                isCustomType, 
                GetGenericArguments(typeSymbol, semanticModel), canBeNull, typeSymbol.IsReferenceType);
        }

        string FullNamespace(ITypeSymbol symbol)
        {
            var namespaceBuilder = new List<string>();
            var containingNamespace  = symbol.ContainingNamespace;
            while(true)
            {
                if(containingNamespace.Name != "" && !containingNamespace.IsGlobalNamespace)
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

        List<JsonType> GetGenericArguments(ITypeSymbol typeSymbol, SemanticModel model)
        {
            var list = new List<JsonType>();
            var namedType = typeSymbol as INamedTypeSymbol;
            if(namedType != null)
            {
                foreach(var typeArgument in namedType.TypeArguments)
                {
                    list.Add(GetType(typeArgument, model));
                }
            }
            return list;
        }
    }

}
