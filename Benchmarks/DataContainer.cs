using System;

namespace DiDemo.Benchmarks
{
    internal class DataContainer
    {
        public Type Service { get; }

        public Type Implementation { get; }

        public DataContainer(Type service, Type implementation)
        {
            Service = service;
            Implementation = implementation;
        }
    }
}
