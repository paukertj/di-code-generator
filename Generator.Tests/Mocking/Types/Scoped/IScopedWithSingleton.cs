using Generator.Tests.Mocking.Types.Singleton;

namespace Generator.Tests.Mocking.Types.Scoped
{
    public interface IScopedWithSingleton
    {
        ISingleton Singleton { get; }
    }
}
