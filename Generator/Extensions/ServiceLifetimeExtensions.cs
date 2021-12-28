using DiDemo.Generator.Generator.Enums;
using DiDemo.Generator.Primitives.Extensions;

namespace DiDemo.Generator.Generator.Extensions
{
    internal static class ServiceLifetimeExtensions
    {
        internal static bool TryConvertToServiceLifetime(this string s, out ServiceLifetime serviceLifetime)
        {
            switch (s)
            {
                case nameof(ServiceCollectionExtensions.AddGeneratedSingleton):
                    serviceLifetime = ServiceLifetime.Singleton;
                    return true;

                case nameof(ServiceCollectionExtensions.AddGeneratedScoped):
                    serviceLifetime = ServiceLifetime.Scoped;
                    return true;

                case nameof(ServiceCollectionExtensions.AddGeneratedTransient):
                    serviceLifetime = ServiceLifetime.Transient;
                    return true;
            }

            serviceLifetime = default;
            return false;
        }
    }
}
