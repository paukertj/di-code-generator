using DiDemo.Generator.Generator.Enums;
using DiDemo.Generator.Generator.Extensions;
using System.Collections.Generic;

namespace DiDemo.Generator.Generator.Models
{
    internal class DependencyInjectionGeneratedCodeInstance
    {
        internal TypeGeneratedCodeInstance Service { get; }
        internal TypeGeneratedCodeInstance Implementation { get; }
        internal ServiceLifetime ServiceLifetime { get; }

        internal IReadOnlyList<ReferenceGeneratedCodeInstance> References { get; }

        internal DependencyInjectionGeneratedCodeInstance(
            TypeGeneratedCodeInstance service, 
            TypeGeneratedCodeInstance implementation, 
            ServiceLifetime serviceLifetime) : this(service, implementation, serviceLifetime, null)
        {
        }

        internal DependencyInjectionGeneratedCodeInstance(
            TypeGeneratedCodeInstance service, 
            TypeGeneratedCodeInstance implementation, 
            ServiceLifetime serviceLifetime, 
            IReadOnlyList<ReferenceGeneratedCodeInstance> references)
        {
            Service = service;
            Implementation = implementation;
            ServiceLifetime = serviceLifetime;
            References = references;
        }

        internal TypeGeneratedCodeInstance GetService()
        {
            return Service ?? Implementation;
        }

        internal TypeGeneratedCodeInstance GetImplementation()
        {
            return Implementation ?? Service;
        }

        internal string ToSourceCode()
        {
            var service = GetService();
            var implementation = GetImplementation();

            string parameters = References?.ToSourceCode();

            return $"serviceCollection.Add(new ServiceDescriptor(typeof({service.TypeName}), (p) => new {implementation.TypeName}({parameters}), ServiceLifetime.{ServiceLifetime}));";
        }
    }
}
