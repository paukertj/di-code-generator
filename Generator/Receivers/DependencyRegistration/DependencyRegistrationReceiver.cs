using DiCodeGenerator.Generator.Extensions;
using DiCodeGenerator.Generator.Models.Analysis.DependencyInjection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace DiCodeGenerator.Generator.Receivers.DependencyRegistration
{
    internal class DependencyRegistrationReceiver : IDependencyRegistrationReceiver
    {
        public IReadOnlyList<IDependencyInjectionInstance> Services => _services;

        private List<IDependencyInjectionInstance> _services { get; } = new();
        private HashSet<string> _alreadyProcessedMethods = new HashSet<string>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            FindServices(syntaxNode);
        }

        private void FindServices(SyntaxNode syntaxNode)
        {
            if (syntaxNode is not InvocationExpressionSyntax methodCall)
            {
                return;
            }

            if (methodCall.Expression is not MemberAccessExpressionSyntax memberAccessExpressionSyntax)
            {
                return;
            }

            string method = memberAccessExpressionSyntax.Name?.Identifier.ValueText;

            if (string.IsNullOrWhiteSpace(method))
            {
                return;
            }

            if (_alreadyProcessedMethods.Contains(method))
            {
                return;
            }

            if (method.TryConvertToServiceLifetime(out var serviceLifetime))
            {
                _services.AddNewService(memberAccessExpressionSyntax, serviceLifetime);
            }

            _alreadyProcessedMethods.Add(method);
        }
    }
}
