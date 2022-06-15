using DiCodeGenerator.Generator.Enums;
using DiCodeGenerator.Generator.Models.Generating.ReferenceGeneratedCode;
using DiCodeGenerator.Generator.Models.Generating.TypeGeneratedCode;
using System.Collections.Generic;

namespace DiCodeGenerator.Generator.Models.Generating.DependencyInjectionGeneratedCode
{
    public interface IDependencyInjectionGeneratedCodeInstance
    {
        ITypeGeneratedCodeInstance Service { get; }
        ITypeGeneratedCodeInstance Implementation { get; }
        ServiceLifetime ServiceLifetime { get; }

        IReadOnlyList<IReferenceGeneratedCodeInstance> References { get; }

        ITypeGeneratedCodeInstance GetService();

        ITypeGeneratedCodeInstance GetImplementation();

        IEnumerable<string> GetAllNamespaces();
    }
}
