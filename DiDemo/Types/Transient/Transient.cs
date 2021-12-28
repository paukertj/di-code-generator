using DiDemo.Types.Scoped;
using DiDemo.Types.Singleton;

namespace DiDemo.Types.Transient
{
    internal class Transient : ITransient
    {
        public Transient(IScoped scoped, ISingleton singleton)
        {

        }
    }
}
