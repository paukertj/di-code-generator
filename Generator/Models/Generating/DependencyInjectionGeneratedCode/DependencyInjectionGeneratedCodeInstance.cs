using DiCodeGenerator.Generator.Enums;
using DiCodeGenerator.Generator.Extensions;
using DiCodeGenerator.Generator.Models.Generating.ReferenceGeneratedCode;
using DiCodeGenerator.Generator.Models.Generating.TypeGeneratedCode;
using System.Collections.Generic;
using System.Linq;

namespace DiCodeGenerator.Generator.Models.Generating.DependencyInjectionGeneratedCode
{
    internal class DependencyInjectionGeneratedCodeInstance : IDependencyInjectionGeneratedCodeInstance
    {
        public ITypeGeneratedCodeInstance Service { get; }
        public ITypeGeneratedCodeInstance Implementation { get; }
        public ServiceLifetime ServiceLifetime { get; }

        public IReadOnlyList<IReferenceGeneratedCodeInstance> References { get; }

        internal DependencyInjectionGeneratedCodeInstance(
            ITypeGeneratedCodeInstance service,
            ITypeGeneratedCodeInstance implementation,
            ServiceLifetime serviceLifetime) : this(service, implementation, serviceLifetime, null)
        {
        }

        internal DependencyInjectionGeneratedCodeInstance(
            ITypeGeneratedCodeInstance service,
            ITypeGeneratedCodeInstance implementation,
            ServiceLifetime serviceLifetime,
            IReadOnlyList<IReferenceGeneratedCodeInstance> references)
        {
            Service = service;
            Implementation = implementation;
            ServiceLifetime = serviceLifetime;
            References = references;
        }

        public ITypeGeneratedCodeInstance GetService()
        {
            return Service ?? Implementation;
        }

        public ITypeGeneratedCodeInstance GetImplementation()
        {
            return Implementation ?? Service;
        }

        public IEnumerable<string> GetAllNamespaces()
        {
            var namespaces = new List<string>();

            namespaces.AddIfNotEmpty(Service?.Namespace);
            namespaces.AddIfNotEmpty(Implementation?.Namespace);

            if (References != null)
            {
                foreach (var reference in References)
                {
                    namespaces.AddIfNotEmpty(reference?.Service?.Namespace);
                }
            }

            return namespaces
                .Distinct();
        }
    }
}
