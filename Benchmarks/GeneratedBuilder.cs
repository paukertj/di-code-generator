using Microsoft.Extensions.DependencyInjection;

namespace DiDemo.Benchmarks
{
    internal static partial class GeneratedBuilder
    {
        private static partial void BuildGeneratedInternal(IServiceCollection serviceCollection);

        internal static IServiceCollection BuildGenerated(this IServiceCollection serviceCollection)
        {
            BuildGeneratedInternal(serviceCollection);

            return serviceCollection;
        }
    }
}
