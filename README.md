# DI Code Generator

## :hand: Disclaimer
This is my hobby project where I want to play with [Source Generators](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview) and try to find a ways, how to use them for future performance improvements. Currently, the project is (from the code quality and test coverage prespective) in "playground" stage, so feel free to inspire yourself with some ideas, but I **strongly do not recommend** to blindly copy something from this repository to the code, that has a production ambition.

## :book: Annotation
Simple example of compile-time [Microsoft DependencyInjection](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection) service registration. This example contains a simple [Source Generator](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview), that generates `ServiceDescriptor` for each service, that includes a service factory. The initial motivation was to measure performance impact of this change on building `ServiceProvider`.

## :innocent: TL; DR
1. Generated code seems to be faster :thumbsup: 
2. `git clone`
3. Demo project is `DiDemo`, if this project has compiled and runs without crash, everything works
4. Benchmark project is `Benchmarks`, run in `Release`, uses [BenchmarkDotNet :snail:](https://benchmarkdotnet.org/)
5. For debugging of generators, switch to `Debug_Generator` configuration and build


## :eyes: The problem
So, let's consider very simple example, where we have those two services:
### Signleton
```csharp
public interface ISingleton 
{ }

internal class Singleton : ISingleton
{ }
```
### Scoped
```csharp
public interface IScoped 
{ }

internal class Scoped : IScoped
{ 
    public Scoped(ISingleton singleton)
    { }
}
```
And we want to add these services into `ServiceCollection` and build a `ServiceProvider` from this collection. The very common approach is use extension methods from `Microsoft.Extensions.DependencyInjection` to do something like this:
```csharp
var serviceCollection = new ServiceCollection()
    .AddSingleton<ISingleton, Singleton>()
    .AddScoped<IScoped, Scoped>();

var provider = serviceCollection.BuildServiceProvider();
```
Here in `provider` variable we have the `ServiceProvider` instance, that provides us services in required lifetime:
```csharp
var singleton = provider.GetRequiredService<ISingleton>();
var scoped = provider.GetRequiredService<IScoped>();
```
But the way above is not the only way, how to add service into `ServiceCollection`. Since `ServiceCollection` implements `ICollection<ServiceDescriptor>` we should be able to do something like this:
```csharp
serviceCollection.Add(new ServiceDescriptor());
```
This is obviously incorrect, because there is no overload like this in [`ServiceDescriptor` constructor](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicedescriptor.-ctor?view=dotnet-plat-ext-6.0). But `ServiceDescriptor` has an [overload, that provides a factory](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicedescriptor.-ctor?view=dotnet-plat-ext-6.0#microsoft-extensions-dependencyinjection-servicedescriptor-ctor(system-type-system-func((system-iserviceprovider-system-object))-microsoft-extensions-dependencyinjection-servicelifetime)), that we can use for definition of service construction. This should save some time thats consumed by service constructor resolving. So in that case, the example above will looks like this:
```csharp
var serviceCollection = new ServiceCollection();
            
serviceCollection.Add(new ServiceDescriptor(typeof(ISingleton), (p) => new Singleton(), ServiceLifetime.Singleton));
serviceCollection.Add(new ServiceDescriptor(typeof(IScoped), (p) => new Scoped(p.GetRequiredService<ISingleton>()), ServiceLifetime.Scoped));

var provider = serviceCollection.BuildServiceProvider();
```
This solution has obvious disadvantage - we must resolve each dependency between services. At this point, the [Source Generators](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview) could be an option, how to deal with this problem.

## :bulb: How to use DI Code Generator
In my playground I developed a simple generator, that generates dependencies in `ServiceDescriptor` factory. Bellow I'll try to explain, how this generator could be used. The first step that you need to do is to add reference to `Generator` (generator itself) and `Primitives` (methods, that will be actually part of compiled code). So open csproj in target project and add something like this:
```xml
  <ItemGroup>
    <ProjectReference Include="..\Generator\Generator.csproj" ReferenceOutputAssembly="false" OutputItemType="Analyzer" />
    <ProjectReference Include="..\Primitives\Primitives.csproj" />
  </ItemGroup>
```
If we don't have (and do not want to have) `Debug_Generator` configuration in our solution, we can remove the debug part from generator:
```csharp
// See DependencyTreeGenerator.cs in Generator.csproj

public void Initialize(GeneratorInitializationContext context)
{
// You can comment this
// #if DEBUG_GENERATOR
//    Debugger.Launch();
// #endif

    context.RegisterForSyntaxNotifications(() => new DependencyRegistrationReceiver());
}
```
Now, we need to create a partial class, that we will use as placeholder for generated code itself. Let's call this class `GeneratedBuilder.cs`. This class should looks like this:
```csharp
internal static partial class GeneratedBuilder
{
    private static partial void BuildGeneratedInternal(IServiceCollection serviceCollection);

    internal static IServiceCollection BuildGenerated(this IServiceCollection serviceCollection)
    {
        BuildGeneratedInternal(serviceCollection);

        return serviceCollection;
    }
}
```
The `BuildGeneratedInternal` method is placeholder for generated code and the `BuildGenerated` is just a public wrapper. If you want to debug generated code, the `BuildGeneratedInternal(serviceCollection);` call is the good place, where you can put the breakpoint. Now everything is ready, so we can add services into `ServiceCollection`. Consider the example above, in that case, we will call extension methods from `DiDemo.Generator.Primitives.Extensions` in very similar way like in previous example:
```csharp
var serviceCollection = new ServiceCollection()
    .AddGeneratedSingleton<ISingleton, Singleton>()
    .AddGeneratedScoped<IScoped, Scoped>()
    .BuildGenerated();

var provider = serviceCollection.BuildServiceProvider();
```
Let's discuss this code a little. The `AddGenerated` methods are placeholders for generator itself, generator runs through the code and looks for those methods. The actual registration will happen in `BuildGenerated()` call, where the generated code will be invoked. You can easily combine those methods with extension methods from `Microsoft.Extensions.DependencyInjection` like that for example:
```csharp
var serviceCollection = new ServiceCollection()
    .AddGeneratedSingleton<ISingleton, Singleton>()
    .AddScoped<IScoped, Scoped>()
    .BuildGenerated();

var provider = serviceCollection.BuildServiceProvider();
```
And that's it!

## :runner: Performance
### Benchmark
The Benchmark scenario is quite simple:
```csharp
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
```
Here you can see extensions methods implementation. Instead of `SERVICE` the `Transient`, `Scope` or `Singleton` has been used.
```csharp
internal static class SERVICEs
{
    internal static void GetAllSERVICEs(this IServiceProvider serviceProvider)
    {
        if (serviceProvider == null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        serviceProvider.GetRequiredService<ISERVICE0>();
        // ...
        serviceProvider.GetRequiredService<ISERVICE999>();
    }

    internal static IServiceCollection AddSERVICEsGenerated(this IServiceCollection serviceCollection)
    {
        if (serviceCollection == null)
        {
            throw new ArgumentNullException(nameof(serviceCollection));
        }

        serviceCollection.AddGeneratedSERVICE<ISERVICE0, SERVICE0>();
        // ...
        serviceCollection.AddGeneratedSERVICE<ISERVICE999, SERVICE999>();

        return serviceCollection;
    }

    internal static IServiceCollection AddSERVICEs(this IServiceCollection serviceCollection)
    {
        if (serviceCollection == null)
        {
            throw new ArgumentNullException(nameof(serviceCollection));
        }

        serviceCollection.AddSERVICE<ISERVICE0, SERVICE0>();
        // ..
        serviceCollection.AddSERVICE<ISERVICE999, SERVICE999>();

        return serviceCollection;
    }
}
```

### Results

``` ini
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1415 (21H2)
Intel Core i7-7820HQ CPU 2.90GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
```
|                                   Method |      Mean |     Error |    StdDev |
|----------------------------------------- |----------:|----------:|----------:|
|          ServiceCollectionBuildBenchmark | 11.124 ms | 0.3771 ms | 1.0819 ms |
| ServiceCollectionBuildGeneratedBenchmark |  4.655 ms | 0.0891 ms | 0.0875 ms |
