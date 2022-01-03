using DiDemo.Generator.Generator.Extensions;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using DiDemo.Generator.Generator.Models.Generating;
using System.Diagnostics;

namespace DiDemo.Generator.Generator
{
    [Generator]
    internal class DependencyTreeGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            //Debugger.Launch();

            var syntaxReceiver = (DependencyRegistrationReceiver)context.SyntaxReceiver;

            var serviceRegistrationSources = new List<DependencyInjectionGeneratedCodeInstance>(syntaxReceiver.Services.Count);

            foreach (var service in syntaxReceiver.Services)
            {
                var serviceRegistrationSource = service.ToGeneratedCodeInstance(context);
                serviceRegistrationSources.Add(serviceRegistrationSource);
            }

            // Find the main method
            var mainMethod = context.Compilation.GetEntryPoint(context.CancellationToken);

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

namespace {mainMethod.ContainingNamespace.ToDisplayString()}
{{
    /// <summary>
    /// Automatically generated
    /// </summary>
    internal static partial class GeneratedBuilder
    {{
        static partial void BuildGeneratedInternal(IServiceCollection serviceCollection)
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
#if Debug_Generator
            Debugger.Launch();
#endif

            context.RegisterForSyntaxNotifications(() => new DependencyRegistrationReceiver());
        }
    }
}
