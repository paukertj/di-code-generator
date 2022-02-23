using DiCodeGenerator.Generator.Exceptions;
using DiCodeGenerator.Generator.Receivers.DependencyRegistration;
using DiCodeGenerator.Generator.Services.DependencyGenerator;
using DiCodeGenerator.Generator.Services.SemanticData;
using DiCodeGenerator.Generator.Services.SourceCodeBuilder;
using DiCodeGenerator.Generator.Services.SourceCodeBuilder.Enums;
using DiCodeGenerator.Generator.Services.SourceCodeBuilder.Primitives;
using DiCodeGenerator.Generator.Services.SourceCodeModel;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace DiCodeGenerator.Generator.Services.SourceCodeGenerator
{
    internal class SourceCodeGeneratorService : ISourceCodeGeneratorService
    {
        private readonly GeneratorExecutionContext _context;
        private readonly ISemanticDataService _semanticDataService;
        private readonly IDependencyGeneratorService _dependencyGeneratorService;
        private readonly ISourceCodeModelService _sourceCodeModelService;
        private readonly ISourceCodeBuilderService _sourceCodeBuilderService;

        public SourceCodeGeneratorService(GeneratorExecutionContext context)
        {
            _context = context;
            _semanticDataService = new SemanticDataService(context);
            _dependencyGeneratorService = new DependencyGeneratorService(_semanticDataService);
            _sourceCodeModelService = new SourceCodeModelService();
            _sourceCodeBuilderService = new SourceCodeBuilderService();
        }

        public string GenerateSourceCode(string entryMethodName)
        {
            FindEntryMethod(entryMethodName);

            BuildDependecyTreeSourceCodeModel();

            var servicesNamespaces = _sourceCodeModelService.GetNamespaces();
            var entryMethodNamespace = _sourceCodeModelService.GetEntryMethodNamespace();
            var services = _sourceCodeModelService.GetServices();

            if (services?.Any() != true)
            {
                throw new DiCodeGeneratorException($"There are no registered services!");
            }

            _sourceCodeBuilderService
                .AddUsing("System")
                .AddUsing("Microsoft.Extensions.DependencyInjection")
                .AddUsing(servicesNamespaces)
                .AddNamespace(entryMethodNamespace)
                .AddClass(Modifiers.Internal | Modifiers.Static | Modifiers.Partial, "GeneratedBuilder")
                .AddMethod(Modifiers.Internal | Modifiers.Static | Modifiers.Partial, entryMethodName, new Argument("IServiceCollection", "serviceCollection"))
                .AddServiceCollection("serviceCollection", services);
                
            return _sourceCodeBuilderService
                .ToSourceCode()
                .ToString();
        }

        private void BuildDependecyTreeSourceCodeModel()
        {
            var syntaxReceiver = (IDependencyRegistrationReceiver)_context.SyntaxReceiver;

            foreach (var service in syntaxReceiver.Services)
            {
                var serviceRegistrationSource = _dependencyGeneratorService.CreateGeneratedCodeInstance(service);

                _sourceCodeModelService.AddService(serviceRegistrationSource);
            }
        }

        private void FindEntryMethod(string entryMethodName)
        {
            var entryMethod = _context.Compilation.SyntaxTrees
                .SelectMany(st => st
                    .GetRoot()
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>())
                .SingleOrDefault(md => md.Identifier.ValueText == entryMethodName);

            if (entryMethodName == null)
            {
                throw new DiCodeGeneratorException($"Unable to find entry method '{entryMethodName}'!");
            }

            _sourceCodeModelService.AddEntryMethod(entryMethod);
        }
    }
}
