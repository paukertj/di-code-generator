using DiDemo.Generator.Generator.Models.Analysis;
using DiDemo.Generator.Generator.Models.Generating;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace DiDemo.Generator.Generator.Extensions
{
    internal static class TypeSyntaxExtensions
    {
        internal static TypeGeneratedCodeInstance ToGeneratedCodeInstance(this TypeSyntax typeSyntax, GeneratorExecutionContext context)
        {
            if (typeSyntax == null)
            {
                return null;
            }

            var semanticModel = context.Compilation.GetSemanticModel(typeSyntax.SyntaxTree);
            var typeinfo = semanticModel.GetTypeInfo(typeSyntax);

            return new TypeGeneratedCodeInstance(typeinfo.Type);
        }

        internal static IReadOnlyList<ReferenceGeneratedCodeInstance> GetReferenceGeneratedCodeInstance(
            this TypeSyntax typeSyntax, GeneratorExecutionContext context)
        {
            if (typeSyntax == null)
            {
                return null;
            }

            var semanticModel = context.Compilation.GetSemanticModel(typeSyntax.SyntaxTree);

            var namedTypeSymbol = (semanticModel.GetSymbolInfo(typeSyntax).Symbol as INamedTypeSymbol);

            if (namedTypeSymbol == null || namedTypeSymbol.Constructors.Length != 1)
            {
                // Does not have any relations
                return null;
            }

            var constructor = namedTypeSymbol.Constructors.Single();

            var result = new List<ReferenceGeneratedCodeInstance>();

            foreach (var parameter in constructor.Parameters)
            {
                var reference = parameter.ToReferenceGeneratedCodeInstance();
                //var r = services.SingleOrDefault(s => reference.Service.Equals(s.GetService().ToGeneratedCodeInstance(context)));

                //if (r == null)
                //{
                //    continue;
                //}

                //reference.ServiceLifetime = r.ServiceLifetime;

                result.Add(reference);
            }

            return result;
        }
    }
}
