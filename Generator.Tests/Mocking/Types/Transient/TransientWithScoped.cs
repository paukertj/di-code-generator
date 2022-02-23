using Generator.Tests.Mocking.Types.Scoped;

namespace Generator.Tests.Mocking.Types.Transient
{
    public class TransientWithScoped : ITransientWithScoped
    {
        public IScoped Scoped { get; }

        public TransientWithScoped(IScoped scoped)
        {
            Scoped = scoped;
        }
    }
}
