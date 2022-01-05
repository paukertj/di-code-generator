using DiCodeGenerator.Tests.Services.Scopes;
using DiCodeGenerator.Tests.Services.Singletons;
using DiCodeGenerator.Tests.Services.Transients;
using DiCodeGenerator.Generator.Primitives.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DiCodeGenerator.Tests.Tests
{
    public class BasicServiceProviderBuildingTests
    {
        [Test]
        public void SimpleServiceProviderFullyGeneratedBuildTest()
        {
            var serviceCollection = new ServiceCollection()
                .AddGeneratedTransient<IEmptyTransientService, EmptyTransientService>()
                .AddGeneratedScoped<IEmptyScopedService, EmptyScopedService>()
                .AddGeneratedSingleton<IEmptySingletonService, EmptySingletonService>()
                .BuildGenerated();

            var provider = serviceCollection.BuildServiceProvider();

            var transient = provider.GetService<IEmptyTransientService>();
            Assert.IsNotNull(transient, "Unabble to get '{0}' from '{1}'", nameof(IEmptyTransientService), nameof(ServiceProvider));

            var scoped = provider.GetService<IEmptyScopedService>();
            Assert.IsNotNull(transient, "Unabble to get '{0}' from '{1}'", nameof(IEmptyScopedService), nameof(ServiceProvider));

            var singleton = provider.GetService<IEmptySingletonService>();
            Assert.IsNotNull(transient, "Unabble to get '{0}' from '{1}'", nameof(IEmptySingletonService), nameof(ServiceProvider));
        }

        [Test]
        public void SimpleServiceProviderPartiallyGeneratedBuildTest()
        {
            var serviceCollection = new ServiceCollection()
                .AddTransient<IEmptyTransientService, EmptyTransientService>()
                .AddGeneratedScoped<IEmptyScopedService, EmptyScopedService>()
                .AddGeneratedSingleton<IEmptySingletonService, EmptySingletonService>()
                .BuildGenerated();

            var provider = serviceCollection.BuildServiceProvider();

            var transient = provider.GetService<IEmptyTransientService>();
            Assert.IsNotNull(transient, "Unabble to get '{0}' from '{1}'", nameof(IEmptyTransientService), nameof(ServiceProvider));

            var scoped = provider.GetService<IEmptyScopedService>();
            Assert.IsNotNull(transient, "Unabble to get '{0}' from '{1}'", nameof(IEmptyScopedService), nameof(ServiceProvider));

            var singleton = provider.GetService<IEmptySingletonService>();
            Assert.IsNotNull(transient, "Unabble to get '{0}' from '{1}'", nameof(IEmptySingletonService), nameof(ServiceProvider));
        }
    }
}
