//using DiDemo.Types.Scoped;
//using DiDemo.Types.Singleton;
//using DiDemo.Types.Transient;
//using Microsoft.Extensions.DependencyInjection;

//namespace DiDemo
//{
//    internal static class StaticDiBuilder
//    {
//        internal static IServiceCollection BuildStaticly(this IServiceCollection serviceCollection)
//        {
//            serviceCollection.Add(new ServiceDescriptor(typeof(ISingleton), (p) => new Singleton(), ServiceLifetime.Singleton));
//            serviceCollection.Add(new ServiceDescriptor(typeof(IScoped), (p) => new Scoped(), ServiceLifetime.Scoped));
//            serviceCollection.Add(new ServiceDescriptor(typeof(ITransient), (p) => new Transient(p.GetRequiredService<IScoped>(), p.GetRequiredService<ISingleton>()), ServiceLifetime.Transient));

//            return serviceCollection;
//        }
//    }
//}
