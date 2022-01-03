using BenchmarkDotNet.Attributes;
using DiDemo.Benchmarks.Data;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DiDemo.Benchmarks.Benchmarks
{
    public class ServiceCollectionBuild
    {
        [Benchmark]
        public void ServiceCollectionBuildBenchmark()
        {
            var serviceCollection = new ServiceCollection()
                .AddTransients()
                .AddScopes()
                .AddSingletons();

            var provider = serviceCollection.BuildServiceProvider();

            //SimpleConsistencyCheck(provider);
        }

        [Benchmark]
        public void ServiceCollectionBuildGeneratedBenchmark()
        {
            var serviceCollection = new ServiceCollection()
                .AddTransientsGenerated()
                .AddScopesGenerated()
                .AddSingletonsGenerated()
                .BuildGenerated();

            var provider = serviceCollection.BuildServiceProvider();

            //SimpleConsistencyCheck(provider);
        }

        private void SimpleConsistencyCheck(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var transient = serviceProvider.GetRequiredService<ITransient0>();
            var scoped = serviceProvider.GetRequiredService<IScoped0>();
            var singleton = serviceProvider.GetRequiredService<ISingleton0>();
        }
    }
}
