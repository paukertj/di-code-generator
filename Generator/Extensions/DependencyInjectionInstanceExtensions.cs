using DiDemo.Generator.Generator.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using DiDemo.Generator.Generator.Enums;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;
using System.Text;

namespace DiDemo.Generator.Generator.Extensions
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
            GeneratorExecutionContext context,
            IReadOnlyList<DependencyInjectionInstance> services)
        {
            if (instance == null)
            {
                return null;
            }

            var service = instance.Service?.ToGeneratedCodeInstance(context);
            var implementation = instance.Implementation?.ToGeneratedCodeInstance(context);

            var references = instance.Implementation.GetReferenceGeneratedCodeInstance(context, services);

            return new DependencyInjectionGeneratedCodeInstance(service, implementation, instance.ServiceLifetime, references);
        }

        internal static string GetSourceCodeForNamespaces(this List<DependencyInjectionGeneratedCodeInstance> generatedCodeInstances)
        {
            if (generatedCodeInstances == null)
            {
                return string.Empty;
            }

            var serviceNamespaces = generatedCodeInstances
                .Select(i => i.Service.Namespace)
                .ToList();
            var implementationNamespaces = generatedCodeInstances
                .Select(i => i.Implementation.Namespace)
                .ToList();

            var namespaces = new List<string>(serviceNamespaces.Count + implementationNamespaces.Count);
            namespaces.AddRange(serviceNamespaces);
            namespaces.AddRange(implementationNamespaces);

            var sb = new StringBuilder(1024);

            foreach (var ns in namespaces.Distinct())
            {
                sb.AppendLine($"using {ns};");
            }

            return sb.ToString();
        }

        internal static string GetSourceCodeForMaps(this List<DependencyInjectionGeneratedCodeInstance> generatedCodeInstances)
        {
            if (generatedCodeInstances == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder(2048);

            foreach (var generatedCodeInstance in generatedCodeInstances)
            {
                sb.AppendLine(generatedCodeInstance.ToSourceCode());
            }

            return sb.ToString();
        }
    }
}
