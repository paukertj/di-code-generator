using DiDemo.Generator.Primitives.Extensions;
using DiDemo.Types.Scoped;
using DiDemo.Types.Singleton;
using DiDemo.Types.Transient;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DiDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection()
                .AddGeneratedTransient<ITransient, Transient>()
                //.AddGeneratedScoped<IScoped, Scoped>()
                .AddScoped<IScoped, Scoped>()
                .AddGeneratedSingleton<ISingleton, Singleton>()
                .BuildGenerated();

            var provider = serviceCollection.BuildServiceProvider();

            var transient = provider.GetRequiredService<ITransient>();

            Console.ReadLine();
        }

        //public static void Main(string[] args)
        //{
        //    var sw = new Stopwatch();

        //    var elapsed = new List<long>(1000); 

        //    for (int i = 0; i < 1000; i++)
        //    {
        //        //var serviceCollection = new ServiceCollection()
        //        //    .AddTransient<ITransient, Transient>()
        //        //    .AddScoped<IScoped, Scoped>()
        //        //    .AddSingleton<ISingleton, Singleton>();

        //        var serviceCollection = new ServiceCollection()
        //            .AddGeneratedTransient<ITransient, Transient>()
        //            .AddGeneratedScoped<IScoped, Scoped>()
        //            .AddGeneratedSingleton<ISingleton, Singleton>()
        //            .BuildGenerated();

        //        sw.Start();

        //        var provider = serviceCollection.BuildServiceProvider();

        //        sw.Stop();

        //        var transient = provider.GetService<ITransient>();

        //        elapsed.Add(sw.ElapsedTicks);

        //        sw.Reset();
        //    }

        //    var result = elapsed.Average();

        //    Console.WriteLine(result);
        //    Console.ReadLine();
        //}
    }
}