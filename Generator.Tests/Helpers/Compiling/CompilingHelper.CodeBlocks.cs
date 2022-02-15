namespace Generator.Tests.Helpers.Compiling
{
    public partial class CompilingHelper
    {
        private const string SourceCodeEnvelope_1 = @"
using DiCodeGenerator.Generator.Primitives.Extensions;
using Generator.Tests.Mocking.Types.Scoped;
using Generator.Tests.Mocking.Types.Singleton;
using Generator.Tests.Mocking.Types.Transient;
using Microsoft.Extensions.DependencyInjection;
using System;

/// <summary>
/// Parameter
/// </summary>";
        private const string SourceCodeEnvelope_2 = @"
namespace Generator.Tests
{
    /// <summary>
    /// Parameter
    /// </summary>
    public class TestCaseClass
    {
        ";
        private const string SourceCodeEnvelope_3 = @"
    }

    /// <summary>
    /// Generated
    /// </summary>
    internal static partial class GeneratedBuilder
    {
        internal static partial void BuildGeneratedInternal(IServiceCollection serviceCollection);

        internal static IServiceCollection BuildGenerated(this IServiceCollection serviceCollection)
        {
            BuildGeneratedInternal(serviceCollection);

            return serviceCollection;
        }
    }
}";
    }
}
