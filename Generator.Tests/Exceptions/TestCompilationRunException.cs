using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace Generator.Tests.Exceptions
{
    public class TestCompilationRunException : Exception
    {
        public IEnumerable<Diagnostic> Diagnostic { get; }

        public TestCompilationRunException(IEnumerable<Diagnostic> diagnostic)
        {
            Diagnostic = diagnostic;
        }
    }
}
