using Generator.Tests.Mocking.Types.Scoped;

namespace Generator.Tests.Mocking.Types.Transient
{
    public interface ITransientWithScoped
    {
        IScoped Scoped { get; }
    }
}
