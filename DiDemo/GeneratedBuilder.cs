using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiDemo
{
    internal static partial class GeneratedBuilder
    {
        static partial void BuildGeneratedInternal(IServiceCollection serviceCollection);

        internal static IServiceCollection BuildGenerated(this IServiceCollection serviceCollection)
        {
            BuildGeneratedInternal(serviceCollection);

            return serviceCollection;
        }
    }
}
