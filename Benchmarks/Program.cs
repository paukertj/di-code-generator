using DiDemo.Benchmarks.Benchmarks;
using System;

namespace DiDemo.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serviceCollectionBuild = new ServiceCollectionBuild();

            serviceCollectionBuild.ServiceCollectionBuildBenchmark();
            serviceCollectionBuild.ServiceCollectionBuildGeneratedBenchmark();

            Console.ReadLine();
        }
    }
}