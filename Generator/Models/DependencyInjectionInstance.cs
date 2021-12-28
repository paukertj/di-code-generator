using DiDemo.Generator.Generator.Enums;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DiDemo.Generator.Generator.Models
{
    internal class DependencyInjectionInstance
    {
        internal TypeSyntax Implementation { get; }
        internal TypeSyntax Service { get; }
        internal ServiceLifetime ServiceLifetime { get; }
        internal MemberAccessExpressionSyntax MemberAccessExpressionSyntax { get; }

        internal DependencyInjectionInstance(TypeSyntax implementation, TypeSyntax service, ServiceLifetime serviceLifetime, MemberAccessExpressionSyntax memberAccessExpressionSyntax)
        {
            Implementation = implementation;
            Service = service;
            ServiceLifetime = serviceLifetime;
            MemberAccessExpressionSyntax = memberAccessExpressionSyntax;
        }

        internal TypeSyntax GetService()
        {
            return Service ?? Implementation;
        }
    }
}
