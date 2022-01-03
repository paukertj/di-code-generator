using BenchmarkDotNet.Running;
using DiDemo.Benchmarks.Benchmarks;
using System;

namespace DiDemo.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ServiceCollectionBuild>();
        }
    }
}