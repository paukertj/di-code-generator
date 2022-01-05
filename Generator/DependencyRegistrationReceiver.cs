using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Collections.Generic;
using DiCodeGenerator.Generator.Extensions;
using DiCodeGenerator.Generator.Models.Analysis;

namespace DiCodeGenerator.Generator
{
    internal class DependencyRegistrationReceiver : ISyntaxReceiver
    {
        public List<DependencyInjectionInstance> Services { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            FindServices(syntaxNode);
        }

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

                if (method.TryConvertToServiceLifetime(out var serviceLifetime))
                {
                    Services.AddNewService(memberAccessExpressionSyntax, serviceLifetime);
                }
            }
        }
    }
}
