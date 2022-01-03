using BenchmarkDotNet.Attributes;
using DiDemo.Benchmarks.Data;
using Microsoft.Extensions.DependencyInjection;

namespace DiDemo.Benchmarks.Benchmarks
{
    [RPlotExporter]
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

            provider.GetAllTransients();
            provider.GetAllScopes();
            provider.GetAllSingletons();
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

            provider.GetAllTransients();
            provider.GetAllScopes();
            provider.GetAllSingletons();
        }
    }
}
