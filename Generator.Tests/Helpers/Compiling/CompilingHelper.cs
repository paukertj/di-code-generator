using DiCodeGenerator.Generator;
using DiCodeGenerator.Generator.Primitives.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Generator.Tests.Helpers.Compiling
{
    public partial class CompilingHelper
    {
        private Compilation _compilationOutput = null;
        private IReadOnlyList<Diagnostic> _diagnosticOutput = null;

        private readonly StringBuilder _sbCodeBlock = new StringBuilder();
        private readonly StringBuilder _sbUsings = new StringBuilder();
        private readonly List<Assembly> _assemblies;

        public CompilingHelper()
        {
            _assemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(assembly => !assembly.IsDynamic && !string.IsNullOrWhiteSpace(assembly.Location))
                .ToList();

            _assemblies.Add(typeof(IServiceCollection).Assembly);
            _assemblies.Add(typeof(ServiceProvider).Assembly);
            _assemblies.Add(typeof(ServiceCollectionExtensions).Assembly);
        }

        public static CompilingHelper Create()
        {
            return new CompilingHelper();
        }

        public CompilingHelper AddSourceBlock(string block)
        {
            _sbCodeBlock.Append(block);

            return this;
        }

        public CompilingHelper AddUsing(string block)
        {
            _sbUsings.AppendLine(block);

            return this;
        }

        public CompilingHelper AddAssembly(Assembly assembly)
        {
            _assemblies.Add(assembly);

            return this;
        }

        public CompilingHelper Compile()
        {
            string sourceCode = GetSourceCode();

            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

            var references = _assemblies
                          .Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
                          .Cast<MetadataReference>();

            var compilation = CSharpCompilation.Create(
                "SourceGeneratorTests",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            // Source Generator to test 
            var generator = new DependencyTreeGenerator();

            CSharpGeneratorDriver.Create(generator)
                                 .RunGeneratorsAndUpdateCompilation(compilation, out var compilationOutput, out var diagnosticOutput);

            _compilationOutput = compilationOutput;
            _diagnosticOutput = diagnosticOutput;

            return this;
        }

        public IReadOnlyList<Diagnostic> GetDiagnosticOutput()
        {
            return _diagnosticOutput;
        }

        public Compilation GetCompilationOutput()
        {
            return _compilationOutput;
        }

        public string GetSourceCode()
        {
            string usings = _sbUsings.ToString();
            string codeBlock = _sbCodeBlock.ToString();

            string sourcCode = string
                .Concat(SourceCodeEnvelope_1, usings, SourceCodeEnvelope_2, codeBlock, SourceCodeEnvelope_3)
                .Trim();

            return sourcCode;
        }
    }
}
