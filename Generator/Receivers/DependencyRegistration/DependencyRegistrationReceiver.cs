using DiCodeGenerator.Generator.Extensions;
using DiCodeGenerator.Generator.Models.Analysis.DependencyInjection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace DiCodeGenerator.Generator.Receivers.DependencyRegistration
{
    internal class DependencyRegistrationReceiver : IDependencyRegistrationReceiver
    {
        public IReadOnlyList<IDependencyInjectionInstance> Services => _services;

        private List<IDependencyInjectionInstance> _services { get; } = new();
        private HashSet<string> _alreadyGeneratedServices = new HashSet<string>();

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

            var methodCalls = syntaxNode
                .DescendantNodes()
                .OfType<InvocationExpressionSyntax>();

            foreach (var methodCall in methodCalls)
            {
                if (methodCall.Expression is not MemberAccessExpressionSyntax memberAccessExpressionSyntax)
                {
                    continue;
                }

                string method = memberAccessExpressionSyntax.Name?.Identifier.ValueText;

                if (string.IsNullOrWhiteSpace(method))
                {
                    continue;
                }

                if (_alreadyGeneratedServices.Contains(method))
                {
                    continue;
                }

                _alreadyGeneratedServices.Add(method);

                if (method.TryConvertToServiceLifetime(out var serviceLifetime))
                {
                    _services.AddNewService(memberAccessExpressionSyntax, serviceLifetime);
                }
            }
        }
    }
}
