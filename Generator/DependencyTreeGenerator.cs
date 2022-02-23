using Microsoft.CodeAnalysis;
using DiCodeGenerator.Generator.Services.SourceCodeGenerator;
using DiCodeGenerator.Generator.Receivers.DependencyRegistration;
using System.Diagnostics;

namespace DiCodeGenerator.Generator
{
    [Generator]
    public class DependencyTreeGenerator : ISourceGenerator
    {
        private const string EntryMethodName = "BuildGeneratedInternal";

        public void Execute(GeneratorExecutionContext context)
        {
            var sourceCodeGeneratorService = new SourceCodeGeneratorService(context);

            var source = sourceCodeGeneratorService.GenerateSourceCode(EntryMethodName);

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
