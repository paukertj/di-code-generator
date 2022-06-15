using Generator.Tests.Extensions;
using Generator.Tests.Mocking.Types.Scoped;
using Generator.Tests.Mocking.Types.Singleton;
using Generator.Tests.Mocking.Types.Transient;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Generator.Tests
{
    public partial class BasicServicesTest : BaseTest
    {
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void SingletonScopedTransient(string sourceBlock)
        {
            sourceBlock = sourceBlock
                .UseSingleton<ISingleton, Singleton>()
                .UseScoped<IScoped, Scoped>()
                .UseTransient<ITransient, Transient>();

            using (var serviceProvider = BuildServiceProvider(sourceBlock))
            {
                Assert.NotNull(serviceProvider.GetService<ISingleton>());
                Assert.NotNull(serviceProvider.GetService<IScoped>());
                Assert.NotNull(serviceProvider.GetService<ITransient>());
            }
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void SingletonIScopedWithSingletonTransient(string sourceBlock)
        {
            sourceBlock = sourceBlock
                .UseSingleton<ISingleton, Singleton>()
                .UseScoped<IScopedWithSingleton, ScopedWithSingleton>()
                .UseTransient<ITransient, Transient>();

            using (var serviceProvider = BuildServiceProvider(sourceBlock))
            {
                Assert.NotNull(serviceProvider.GetService<ISingleton>());

                var scoped = serviceProvider.GetService<IScopedWithSingleton>();
                Assert.NotNull(scoped);
                Assert.NotNull(scoped.Singleton);

                Assert.NotNull(serviceProvider.GetService<ITransient>());
            }
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void SingletonScopedTransientWithScopedAndSingleton(string sourceBlock)
        {
            sourceBlock = sourceBlock
                .UseSingleton<ISingleton, Singleton>()
                .UseScoped<IScoped, Scoped>()
                .UseTransient<ITransientWithScopedAndSingleton, TransientWithScopedAndSingleton>();

            using (var serviceProvider = BuildServiceProvider(sourceBlock))
            {
                Assert.NotNull(serviceProvider.GetService<ISingleton>());
                Assert.NotNull(serviceProvider.GetService<IScoped>());

                var transient = serviceProvider.GetService<ITransientWithScopedAndSingleton>();
                Assert.NotNull(transient);
                Assert.NotNull(transient.Singleton);
                Assert.NotNull(transient.Scoped);
            }
        }
    }
}