using DiCodeGenerator.Generator.Models.Analysis;
using DiCodeGenerator.Generator.Models.Analysis.DependencyInjection;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace DiCodeGenerator.Generator.Receivers.DependencyRegistration
{
    public interface IDependencyRegistrationReceiver : ISyntaxReceiver
    {
        IReadOnlyList<IDependencyInjectionInstance> Services { get; }
    }
}
