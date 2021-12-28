using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Diagnostics;
using System.Linq;
using DiDemo.Generator.Generator.Models;
using System.Collections.Generic;
using DiDemo.Generator.Primitives.Extensions;
using DiDemo.Generator.Generator.Extensions;
using DiDemo.Generator.Generator.Enums;

namespace DiDemo.Generator.Generator
{
    internal class DependencyRegistrationReceiver : ISyntaxReceiver
    {
        public List<DependencyInjectionInstance> Services { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            //Debugger.Launch();

            // TODO: Here I'll have to handle the order (ctor could be before service registration itself?)
            FindServices(syntaxNode);
            //BuildRelationships(syntaxNode);
        }

        //private void BuildRelationships(SyntaxNode syntaxNode)
        //{
        //    var constructors = syntaxNode.DescendantNodes().OfType<ConstructorDeclarationSyntax>().ToList();

        //    foreach (var constructor in constructors)
        //    {
        //        Services.AddRelationship(constructor);
        //    }
        //}

        private void FindServices(SyntaxNode syntaxNode)
        {
            if (syntaxNode is not MethodDeclarationSyntax)
            {
                return;
            }

            var methodCalls = syntaxNode.DescendantNodes().OfType<InvocationExpressionSyntax>();

            foreach (var call in methodCalls)
            {
                if (call.Expression is not MemberAccessExpressionSyntax memberAccessExpressionSyntax)
                {
                    continue;
                }

                string method = memberAccessExpressionSyntax.Name?.Identifier.ValueText;

                if (string.IsNullOrWhiteSpace(method))
                {
                    continue;
                }

                if (method == nameof(ServiceCollectionExtensions.AddGeneratedSingleton))
                {
                    Services.AddNewService(memberAccessExpressionSyntax, ServiceLifetime.Singleton);
                }
                else if (method == nameof(ServiceCollectionExtensions.AddGeneratedScoped))
                {
                    Services.AddNewService(memberAccessExpressionSyntax, ServiceLifetime.Scoped);
                }
                else if (method == nameof(ServiceCollectionExtensions.AddGeneratedTransient))
                {
                    Services.AddNewService(memberAccessExpressionSyntax, ServiceLifetime.Transient);
                }
            }
        }
    }
}
