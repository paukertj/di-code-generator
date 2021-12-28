using DiDemo.Generator.Generator.Enums;

namespace DiDemo.Generator.Generator.Models
{
    internal class DependencyInjectionInstance
    {
        internal string Implementation { get; }

        internal string Service { get; }

        internal ServiceLifetime ServiceLifetime { get; }

        internal DependencyInjectionInstance(string implementation, string service, ServiceLifetime serviceLifetime)
        {
            Implementation = implementation;
            Service = service;
            ServiceLifetime = serviceLifetime;
        }

        public override bool Equals(object obj)
        {
            if (obj is not DependencyInjectionInstance dependencyInjectionInstance)
            {
                return false;
            }

            return dependencyInjectionInstance.Service == Service;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
