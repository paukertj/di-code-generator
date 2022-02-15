using Generator.Tests.Mocking.Types.Singleton;

namespace Generator.Tests.Mocking.Types.Scoped
{
    public class ScopedWithSingleton : IScopedWithSingleton
    {
        public ISingleton Singleton { get; }

        public ScopedWithSingleton(ISingleton singleton)
        {
            Singleton = singleton;
        }
    }
}
