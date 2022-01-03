using DiDemo.Benchmarks.Data;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DiDemo.Benchmarks.Benchmarks
{
    internal class ServiceCollectionBuild
    {
        internal void ServiceCollectionBuildBenchmark()
        {
            var serviceCollection = new ServiceCollection()
                .AddTransients()
                .AddScopes()
                .AddSingletons();

            serviceCollection.BuildServiceProvider();
        }

        internal void ServiceCollectionBuildGeneratedBenchmark()
        {
            var serviceCollection = new ServiceCollection()
                .AddTransientsGenerated()
                .AddScopesGenerated()
                .AddSingletonsGenerated()
                .BuildGenerated();

            serviceCollection.BuildServiceProvider();
        }
    }
}
