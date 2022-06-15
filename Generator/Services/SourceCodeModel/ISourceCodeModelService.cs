using DiCodeGenerator.Generator.Models.Generating.DependencyInjectionGeneratedCode;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace DiCodeGenerator.Generator.Services.SourceCodeModel
{
    public interface ISourceCodeModelService
    {
        void AddService(IDependencyInjectionGeneratedCodeInstance dependencyInjectionGeneratedCodeInstance);
        void AddEntryMethod(MethodDeclarationSyntax entryMethod);
        string GetEntryMethodNamespace();
        IReadOnlyList<string> GetNamespaces();
        IReadOnlyList<IDependencyInjectionGeneratedCodeInstance> GetServices();
    }
}
