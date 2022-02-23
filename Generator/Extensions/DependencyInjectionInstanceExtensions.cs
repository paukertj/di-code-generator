using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using DiCodeGenerator.Generator.Enums;
using DiCodeGenerator.Generator.Models.Analysis.DependencyInjection;

namespace DiCodeGenerator.Generator.Extensions
{
    internal static class DependencyInjectionInstanceExtensions
    {
        internal static void AddNewService(this List<IDependencyInjectionInstance> map, MemberAccessExpressionSyntax memberAccessExpressionSyntax, ServiceLifetime serviceLifetime)
        {
            var args = (memberAccessExpressionSyntax.Name as GenericNameSyntax)?.TypeArgumentList?.Arguments
                        .ToList();

            if (args.Count != 2)
            {
                return;
            }

            var service = new DependencyInjectionInstance(args[1], args[0], serviceLifetime, memberAccessExpressionSyntax);

            map.Add(service);
        }
    }
}
