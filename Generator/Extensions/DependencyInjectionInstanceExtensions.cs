using DiDemo.Generator.Generator.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using DiDemo.Generator.Generator.Enums;

namespace DiDemo.Generator.Generator.Extensions
{
    internal static class DependencyInjectionInstanceExtensions
    {
        internal static void AddNewService(this Dictionary<DependencyInjectionInstance, List<string>> map, MemberAccessExpressionSyntax memberAccessExpressionSyntax, ServiceLifetime serviceLifetime)
        {
            var args = (memberAccessExpressionSyntax.Name as GenericNameSyntax)?.TypeArgumentList?.Arguments
                        .OfType<IdentifierNameSyntax>()
                        .Select(a => a.Identifier.ValueText)
                        .ToList();

            if (args.Count != 2)
            {
                return;
            }

            var service = new DependencyInjectionInstance(args[1], args[0], serviceLifetime);

            map.AddNewService(service);
        }

        internal static void AddNewService(this Dictionary<DependencyInjectionInstance, List<string>> map, DependencyInjectionInstance service)
        {
            if (map == null)
            {
                return;
            }

            if (map.ContainsKey(service))
            {
                return;
            }

            map.Add(service, new List<string>());
        }

        internal static void AddRelationship(this Dictionary<DependencyInjectionInstance, List<string>> map, ConstructorDeclarationSyntax constructorDeclarationSyntax)
        {
            // This is very naive
            var implementation = constructorDeclarationSyntax.Identifier.ValueText;
            var relationships = constructorDeclarationSyntax.ParameterList.Parameters
                .Select(p => p.Type as IdentifierNameSyntax)
                .Select(s => s.Identifier.ValueText);
        
            var services = map.SingleOrDefault(m => m.Key.Implementation == implementation).Value;

            if (services == null)
            {
                return;
            }

            // Here should be some distinct
            services.AddRange(relationships);
        }
    }
}
