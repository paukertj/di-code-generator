using DiCodeGenerator.Generator.Enums;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DiCodeGenerator.Generator.Models.Analysis.DependencyInjection
{
    public class DependencyInjectionInstance : IDependencyInjectionInstance
    {
        public TypeSyntax Implementation { get; }
        public TypeSyntax Service { get; }
        public ServiceLifetime ServiceLifetime { get; }
        public MemberAccessExpressionSyntax MemberAccessExpressionSyntax { get; }

        internal DependencyInjectionInstance(TypeSyntax implementation, TypeSyntax service, ServiceLifetime serviceLifetime, MemberAccessExpressionSyntax memberAccessExpressionSyntax)
        {
            Implementation = implementation;
            Service = service;
            ServiceLifetime = serviceLifetime;
            MemberAccessExpressionSyntax = memberAccessExpressionSyntax;
        }

        public TypeSyntax GetService()
        {
            return Service ?? Implementation;
        }
    }
}
