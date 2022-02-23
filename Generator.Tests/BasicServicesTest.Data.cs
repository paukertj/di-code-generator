using System.Collections.Generic;

namespace Generator.Tests
{
    public partial class BasicServicesTest
    {
        public static IEnumerable<string> TestCases => new List<string>
        {
            @"
public static ServiceProvider Main()
{
    var serviceCollection = new ServiceCollection()
        .AddGeneratedTransient<ITransient, Transient>()
        .AddGeneratedScoped<IScoped, Scoped>()
        .AddGeneratedSingleton<ISingleton, Singleton>()
        .BuildGenerated();

    return serviceCollection.BuildServiceProvider();
}",
                        @"
public static ServiceProvider Main()
{
    var serviceCollection = new ServiceCollection();
        
    serviceCollection.AddGeneratedTransient<ITransient, Transient>();
    serviceCollection.AddGeneratedScoped<IScoped, Scoped>();
    serviceCollection.AddGeneratedSingleton<ISingleton, Singleton>();
    
    serviceCollection.BuildGenerated();

    return serviceCollection.BuildServiceProvider();
}",
                        @"
public static ServiceProvider Main()
{
    var serviceCollection = new ServiceCollection();

    var test = new Action<ServiceCollection>((sc) => 
    {
        sc
            .AddGeneratedTransient<ITransient, Transient>()
            .AddGeneratedScoped<IScoped, Scoped>()
            .AddGeneratedSingleton<ISingleton, Singleton>();
    });

    test(serviceCollection);
    serviceCollection.BuildGenerated();

    return serviceCollection.BuildServiceProvider();
}",
            @"
public static ServiceProvider Main()
{
    var serviceCollection = new ServiceCollection();

    AddServices(serviceCollection);

    serviceCollection.BuildGenerated();

    return serviceCollection.BuildServiceProvider();
}

private static void AddServices(ServiceCollection sc)
{
    sc
        .AddGeneratedTransient<ITransient, Transient>()
        .AddGeneratedScoped<IScoped, Scoped>()
        .AddGeneratedSingleton<ISingleton, Singleton>();
}",
                        @"
public static ServiceProvider Main()
{
    var provider = BuildServiceProvider();

    return provider;
}

private static ServiceProvider BuildServiceProvider()
{
    return new ServiceCollection()
        .AddGeneratedTransient<ITransient, Transient>()
        .AddGeneratedScoped<IScoped, Scoped>()
        .AddGeneratedSingleton<ISingleton, Singleton>()
        .BuildGenerated()
        .BuildServiceProvider();
}",
        };
    }
}
