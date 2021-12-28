using DiDemo.Generator.Generator.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Diagnostics;
using System.Text;

namespace DiDemo.Generator.Generator
{
    [Generator]
    internal class DependencyTreeGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            //Debugger.Launch();

            var receiver = (DependencyRegistrationReceiver)context.SyntaxReceiver;

            // find the main method
            var mainMethod = context.Compilation.GetEntryPoint(context.CancellationToken);

            var sb = new StringBuilder(1024);

            foreach (var service in receiver.Services)
            {
                sb.AppendRecord(service, receiver.Services); // O(n^2)
            }

            // build up the source code
            string source = $@"
using System;
using Microsoft.Extensions.DependencyInjection;
using DiDemo.Types.Singleton; // TODO
using DiDemo.Types.Transient; // TODO
using DiDemo.Types.Scoped; // TODO

namespace {mainMethod.ContainingNamespace.ToDisplayString()}
{{
    internal static partial class GeneratedBuilder
    {{
        static partial void BuildGeneratedInternal(IServiceCollection serviceCollection)
        {{
            {sb.ToString()}
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
