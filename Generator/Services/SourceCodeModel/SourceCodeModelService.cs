using DiCodeGenerator.Generator.Extensions;
using DiCodeGenerator.Generator.Models.Generating.DependencyInjectionGeneratedCode;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiCodeGenerator.Generator.Services.SourceCodeModel
{
    internal class SourceCodeModelService : ISourceCodeModelService
    {
        private readonly List<IDependencyInjectionGeneratedCodeInstance> _dependencyInjectionGeneratedCodeInstances = new List<IDependencyInjectionGeneratedCodeInstance>();
        private MethodDeclarationSyntax _entryMethod;

        public void AddEntryMethod(MethodDeclarationSyntax entryMethod)
        {
            if (_entryMethod != null)
            {
                throw new ArgumentException("Unable to register entry method, because it is already registered!");
            }

            _entryMethod = entryMethod;
        }

        public void AddService(IDependencyInjectionGeneratedCodeInstance dependencyInjectionGeneratedCodeInstance)
        {
            if (dependencyInjectionGeneratedCodeInstance == null)
            {
                return;
            }

            _dependencyInjectionGeneratedCodeInstances.Add(dependencyInjectionGeneratedCodeInstance);
        }

        public string GetEntryMethodNamespace()
        {
            var ns = _entryMethod.Parent?.Parent as NamespaceDeclarationSyntax;
            return ns?.Name?.ToString();
        }

        public IReadOnlyList<string> GetNamespaces()
        {
            var namespaces = new List<string>();

            var servicesNamespaces = GetServicesNamespaces();

            if (servicesNamespaces?.Any() == true)
            {
                namespaces.AddRange(servicesNamespaces);
            }

            return namespaces;
        }

        private IReadOnlyList<string> GetServicesNamespaces()
        {
            if (_dependencyInjectionGeneratedCodeInstances.Count <= 0)
            {
                return null;
            }

            return _dependencyInjectionGeneratedCodeInstances
                .SelectMany(d => GetDependencyInjectionGeneratedCodeInstanceNamespaces(d))
                .Distinct()
                .Where(d => !string.IsNullOrWhiteSpace(d))
                .ToList();
        }

        private IReadOnlyList<string> GetDependencyInjectionGeneratedCodeInstanceNamespaces(IDependencyInjectionGeneratedCodeInstance dependencyInjectionGeneratedCodeInstance)
        {
            if (dependencyInjectionGeneratedCodeInstance == null)
            {
                return null;
            }

            var namespaces = new List<string>();

            namespaces.AddIfNotEmpty(dependencyInjectionGeneratedCodeInstance.Service?.Namespace);
            namespaces.AddIfNotEmpty(dependencyInjectionGeneratedCodeInstance.Implementation?.Namespace);

            if (dependencyInjectionGeneratedCodeInstance.References != null)
            {
                foreach (var reference in dependencyInjectionGeneratedCodeInstance.References)
                {
                    namespaces.AddIfNotEmpty(reference?.Service?.Namespace);
                }
            }

            return namespaces
                .Distinct()
                .ToList();
        }

        public IReadOnlyList<IDependencyInjectionGeneratedCodeInstance> GetServices()
        {
            return _dependencyInjectionGeneratedCodeInstances;
        }
    }
}
