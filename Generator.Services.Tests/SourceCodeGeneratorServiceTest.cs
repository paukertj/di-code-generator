using DiCodeGenerator.Generator.Services.SourceCodeBuilder;
using NUnit.Framework;
using System;

namespace DiCodeGenerator.Generator.Services.Tests
{
    public partial class SourceCodeGeneratorServiceTest
    {
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void SingletonScopedTransient(string expected, Action<ISourceCodeBuilderService> builder)
        {
            var service = new SourceCodeBuilderService();

            builder(service);

            var sourceCode = service
                .ToSourceCode()
                .ToString()
                .Trim();

            expected = expected
                .Trim();

            Assert.That(sourceCode, Is.EqualTo(expected).NoClip);
        }
    }
}
