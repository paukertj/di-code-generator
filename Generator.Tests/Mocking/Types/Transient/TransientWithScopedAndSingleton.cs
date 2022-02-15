using Generator.Tests.Mocking.Types.Scoped;
using Generator.Tests.Mocking.Types.Singleton;

namespace Generator.Tests.Mocking.Types.Transient
{
    public class TransientWithScopedAndSingleton : ITransientWithScopedAndSingleton
    {
        public IScoped Scoped { get; }
        public ISingleton Singleton { get; }

        public TransientWithScopedAndSingleton(IScoped scoped, ISingleton singleton)
        {
            Scoped = scoped;
            Singleton = singleton;
        }
    }
}
