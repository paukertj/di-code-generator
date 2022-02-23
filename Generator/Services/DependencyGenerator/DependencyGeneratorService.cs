using DiCodeGenerator.Generator.Models.Analysis.DependencyInjection;
using DiCodeGenerator.Generator.Models.Generating.DependencyInjectionGeneratedCode;
using DiCodeGenerator.Generator.Models.Generating.ReferenceGeneratedCode;
using DiCodeGenerator.Generator.Models.Generating.TypeGeneratedCode;
using DiCodeGenerator.Generator.Services.SemanticData;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace DiCodeGenerator.Generator.Services.DependencyGenerator
{
    internal class DependencyGeneratorService : IDependencyGeneratorService
    {
        private readonly ISemanticDataService _semanticDataService;

        public DependencyGeneratorService(ISemanticDataService semanticDataService)
        {
            _semanticDataService = semanticDataService;
        }

        public IDependencyInjectionGeneratedCodeInstance CreateGeneratedCodeInstance(IDependencyInjectionInstance instance)
        {
            if (instance == null)
            {
                return null;
            }

            var service = CreateGeneratedCodeInstance(instance.Service);
            var implementation = CreateGeneratedCodeInstance(instance.Implementation);

            var references = GetReferenceGeneratedCodeInstance(instance.Implementation);

            return new DependencyInjectionGeneratedCodeInstance(service, implementation, instance.ServiceLifetime, references);
        }

        private ITypeGeneratedCodeInstance CreateGeneratedCodeInstance(TypeSyntax typeSyntax)
        {
            if (typeSyntax == null)
            {
                return null;
            }

            var semanticModel = _semanticDataService.GetSemanticModel(typeSyntax.SyntaxTree);
            var typeinfo = semanticModel.GetTypeInfo(typeSyntax);

            return new TypeGeneratedCodeInstance(typeinfo.Type);
        }

        private IReadOnlyList<IReferenceGeneratedCodeInstance> GetReferenceGeneratedCodeInstance(TypeSyntax typeSyntax)
        {
            if (typeSyntax == null)
            {
                return null;
            }

            var semanticModel = _semanticDataService.GetSemanticModel(typeSyntax.SyntaxTree);

            var namedTypeSymbol = (semanticModel.GetSymbolInfo(typeSyntax).Symbol as INamedTypeSymbol);

            if (namedTypeSymbol == null || namedTypeSymbol.Constructors.Length != 1)
            {
                // Does not have any relations
                return null;
            }

            var constructor = namedTypeSymbol.Constructors.Single();

            var result = new List<IReferenceGeneratedCodeInstance>(constructor.Parameters.Length);

            foreach (var parameter in constructor.Parameters)
            {
                var reference = CreateReferenceGeneratedCodeInstance(parameter);

                if (reference == null)
                {
                    continue;
                }

                result.Add(reference);
            }

            return result;
        }

        private IReferenceGeneratedCodeInstance CreateReferenceGeneratedCodeInstance(IParameterSymbol parameterSymbol)
        {
            if (parameterSymbol?.Type == null)
            {
                return null;
            }

            var typeGeneratedCodeInstance = new TypeGeneratedCodeInstance(parameterSymbol.Type);

            return new ReferenceGeneratedCodeInstance(typeGeneratedCodeInstance);
        }
    }
}
