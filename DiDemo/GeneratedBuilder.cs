using Microsoft.Extensions.DependencyInjection;

namespace DiCodeGenerator.DiDemo
{
    internal static partial class GeneratedBuilder
    {
        internal static partial void BuildGeneratedInternal(IServiceCollection serviceCollection);

        internal static IServiceCollection BuildGenerated(this IServiceCollection serviceCollection)
        {
            BuildGeneratedInternal(serviceCollection);

            return serviceCollection;
        }
    }
}
