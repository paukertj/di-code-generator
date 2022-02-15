using DiCodeGenerator.Generator.Extensions;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using DiCodeGenerator.Generator.Models.Generating;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DiCodeGenerator.Generator.Exceptions;
using System.Diagnostics;

namespace DiCodeGenerator.Generator
{
    [Generator]
    public class DependencyTreeGenerator : ISourceGenerator // TODO: Internal
    {
        private const string EntryMethod = "BuildGeneratedInternal";

        public void Execute(GeneratorExecutionContext context)
        {
            var syntaxReceiver = (DependencyRegistrationReceiver)context.SyntaxReceiver;

            var serviceRegistrationSources = new List<DependencyInjectionGeneratedCodeInstance>(syntaxReceiver.Services.Count);

            foreach (var service in syntaxReceiver.Services)
            {
                var serviceRegistrationSource = service.ToGeneratedCodeInstance(context);

                if (serviceRegistrationSources.Any(s => s.Service.FullName == serviceRegistrationSource.Service.FullName)) // Compelxity
                {
                    continue;
                }

                serviceRegistrationSources.Add(serviceRegistrationSource);
            }

            var entryMethod = context.Compilation.SyntaxTrees
                .SelectMany(st => st
                    .GetRoot()
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>())
                .SingleOrDefault(md => md.Identifier.ValueText == EntryMethod);

            if (entryMethod == null)
            {
                throw new DiCodeGeneratorException($"Unable to find entry method '{EntryMethod}'!");
            }

            var mainNamespaceDeclaration = entryMethod.Parent?.Parent as NamespaceDeclarationSyntax;
            string mainNamespace = mainNamespaceDeclaration?.Name?.ToString();

            if (string.IsNullOrEmpty(mainNamespace))
            {
                throw new DiCodeGeneratorException($"Unable to find entry method '{EntryMethod}' namespace!");
            }

            var namespaces = serviceRegistrationSources
                .GetSourceCodeForNamespaces()
                .ForceTrim();
            var maps = serviceRegistrationSources
                .GetSourceCodeForMaps()
                .Tab(3);

            string source = $@"
using System;
using Microsoft.Extensions.DependencyInjection;
{namespaces}

namespace {mainNamespace}
{{
    /// <summary>
    /// Automatically generated
    /// </summary>
    internal static partial class GeneratedBuilder
    {{
        internal static partial void BuildGeneratedInternal(IServiceCollection serviceCollection)
        {{
{maps}
        }}
    }}
}}
".ForceTrim();

            context.AddSource($"GeneratedBuilder_Generated.cs", source);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG_GENERATOR
            Debugger.Launch();
#endif

            context.RegisterForSyntaxNotifications(() => new DependencyRegistrationReceiver());
        }
    }
}
