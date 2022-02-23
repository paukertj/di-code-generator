using Generator.Tests.Mocking.Types.Scoped;
using Generator.Tests.Mocking.Types.Singleton;

namespace Generator.Tests.Mocking.Types.Transient
{
    public interface ITransientWithScopedAndSingleton
    {
        IScoped Scoped { get; }
        ISingleton Singleton { get; }
    }
}
