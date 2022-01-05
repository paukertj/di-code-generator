using Microsoft.Extensions.DependencyInjection;
using System;

namespace DiCodeGenerator.Generator.Primitives.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGeneratedSingleton<TService, TImplementation>(this IServiceCollection serviceCollection)
            where TImplementation : TService
        {
            return serviceCollection;
        }

        public static IServiceCollection AddGeneratedScoped<TService, TImplementation>(this IServiceCollection serviceCollection)
            where TImplementation : TService
        {
            return serviceCollection;
        }

        public static IServiceCollection AddGeneratedTransient<TService, TImplementation>(this IServiceCollection serviceCollection)
            where TImplementation : TService
        {
            return serviceCollection;
        }
    }
}
