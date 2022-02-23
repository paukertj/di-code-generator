using Generator.Tests.Extensions;
using Generator.Tests.Helpers.Compiling;
using Microsoft.Extensions.DependencyInjection;

namespace Generator.Tests
{
    public abstract class BaseTest
    {
        protected ServiceProvider BuildServiceProvider(string sourceBlock)
        {
            return CompilingHelper
                .Create()
                .AddSourceBlock(sourceBlock)
                .Compile()
                .GetCompilationOutput()
                .Run<ServiceProvider>();
        }
    }
}
