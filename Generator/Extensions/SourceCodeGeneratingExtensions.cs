using DiDemo.Generator.Generator.Models.Generating;
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

            var namespaces = generatedCodeInstances
                .SelectMany(i => i.GetAllNamespaces())
                .Distinct();
            //var implementationNamespaces = generatedCodeInstances
            //    .Select(i => i.Implementation.Namespace)
            //    .ToList();
            //var referencesNamespaces = generatedCodeInstances
            //    .SelectMany(i => i.References)
            //    .Select(i => i.Service.Namespace)
            //    .ToList();

            //var namespaces = new List<string>(serviceNamespaces.Count + implementationNamespaces.Count);
            //namespaces.AddRange(serviceNamespaces);
            //namespaces.AddRange(implementationNamespaces);

            var sb = new StringBuilder(1024);

            foreach (var ns in namespaces/*.Distinct()*/)
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

        internal static string Tab(this string code, uint tab)
        {
            if (string.IsNullOrEmpty(code))
            {
                return code;
            }

            string prefix = "    ".Copy(tab);

            var lines = code.Split('\r');

            if (lines.Length <= 0)
            {
                return prefix + code.Trim();
            }

            var sb = new StringBuilder(1024);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                sb.AppendLine(prefix + line.ForceTrim());
            }

            return sb
                .ToString()
                .TrimEnd('\r', '\n');
        }

        internal static string SpaceTrim(this string s)
        {
            return s?.Trim('\t', ' ');
        }

        internal static string ForceTrim(this string s)
        {
            return s?
                .Trim('\r', '\n')?
                .SpaceTrim();
        }

        internal static string Copy(this string s, uint count)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            string output = string.Empty;

            for (int i = 0; i < count; i++)
            {
                output += s;
            }

            return output;
        }
    }
}
