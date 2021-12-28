using DiDemo.Generator.Generator.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Diagnostics;
using System.Text;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DiDemo.Generator.Generator.Models;
using System.Collections.Generic;

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
                var serviceRegistrationSource = service.ToGeneratedCodeInstance(context, syntaxReceiver.Services); // O(n^2)
                serviceRegistrationSources.Add(serviceRegistrationSource);
            }

            // Find the main method
            var mainMethod = context.Compilation.GetEntryPoint(context.CancellationToken);

            var namespaces = serviceRegistrationSources.GetSourceCodeForNamespaces();
            var maps = serviceRegistrationSources.GetSourceCodeForMaps();

            // build up the source code
            string source = $@"
using System;
using Microsoft.Extensions.DependencyInjection;
{namespaces}

namespace {mainMethod.ContainingNamespace.ToDisplayString()}
{{
    internal static partial class GeneratedBuilder
    {{
        static partial void BuildGeneratedInternal(IServiceCollection serviceCollection)
        {{
            {maps}
        }}
    }}
}}
";

            context.AddSource($"GeneratedBuilder_Generated.cs", source);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new DependencyRegistrationReceiver());
        }
    }
}
