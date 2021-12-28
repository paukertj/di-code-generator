using DiDemo.Generator.Primitives.Extensions;
using DiDemo.Types.Scoped;
using DiDemo.Types.Singleton;
using DiDemo.Types.Transient;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DiDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection()
                .AddGeneratedTransient<ITransient, Transient>()
                .AddScoped<IScoped, Scoped>()
                .AddGeneratedSingleton<ISingleton, Singleton>()
                .BuildGenerated();

            var provider = serviceCollection.BuildServiceProvider();

            var transient = provider.GetRequiredService<ITransient>();
            var scoped = provider.GetRequiredService<IScoped>();
            var singleton = provider.GetRequiredService<ISingleton>();

            Console.ReadLine();
        }
    }
}