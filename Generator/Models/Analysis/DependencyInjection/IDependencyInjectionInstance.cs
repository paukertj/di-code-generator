using DiCodeGenerator.Generator.Enums;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DiCodeGenerator.Generator.Models.Analysis.DependencyInjection
{
    public interface IDependencyInjectionInstance
    {
        TypeSyntax Implementation { get; }
        TypeSyntax Service { get; }
        ServiceLifetime ServiceLifetime { get; }
        MemberAccessExpressionSyntax MemberAccessExpressionSyntax { get; }

        TypeSyntax GetService();
    }
}
