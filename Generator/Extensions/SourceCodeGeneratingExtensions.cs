using DiDemo.Generator.Generator.Models.Generating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiDemo.Generator.Generator.Extensions
{
    internal static class SourceCodeGeneratingExtensions
    {
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

        internal static string ToSourceCode(this IReadOnlyList<ReferenceGeneratedCodeInstance> instances)
        {
            if (instances == null)
            {
                return string.Empty;
            }

            var e = instances.Select(i => i.ToSourceCode());
            return string.Join(", ", e);
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
