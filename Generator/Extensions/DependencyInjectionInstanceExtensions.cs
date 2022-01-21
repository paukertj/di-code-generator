using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using DiCodeGenerator.Generator.Enums;
using Microsoft.CodeAnalysis;
using DiCodeGenerator.Generator.Models.Analysis;
using DiCodeGenerator.Generator.Models.Generating;

namespace DiCodeGenerator.Generator.Extensions
{
    internal static class DependencyInjectionInstanceExtensions
    {
        internal static void AddNewService(this List<DependencyInjectionInstance> map, MemberAccessExpressionSyntax memberAccessExpressionSyntax, ServiceLifetime serviceLifetime)
        {
            var args = (memberAccessExpressionSyntax.Name as GenericNameSyntax)?.TypeArgumentList?.Arguments
                        .ToList();

            if (args.Count != 2)
            {
                return;
            }

            var service = new DependencyInjectionInstance(args[1], args[0], serviceLifetime, memberAccessExpressionSyntax);

            map.Add(service);
        }

        internal static DependencyInjectionGeneratedCodeInstance ToGeneratedCodeInstance(
            this DependencyInjectionInstance instance,
            GeneratorExecutionContext context)
        {
            if (instance == null)
            {
                return null;
            }

            var service = instance.Service?.ToGeneratedCodeInstance(context);
            var implementation = instance.Implementation?.ToGeneratedCodeInstance(context);

            var references = instance.Implementation.GetReferenceGeneratedCodeInstance(context);

            return new DependencyInjectionGeneratedCodeInstance(service, implementation, instance.ServiceLifetime, references);
        }
    }
}
