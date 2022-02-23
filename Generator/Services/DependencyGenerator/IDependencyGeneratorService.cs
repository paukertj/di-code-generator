using DiCodeGenerator.Generator.Models.Analysis.DependencyInjection;
using DiCodeGenerator.Generator.Models.Generating.DependencyInjectionGeneratedCode;

namespace DiCodeGenerator.Generator.Services.DependencyGenerator
{
    public interface IDependencyGeneratorService
    {
        IDependencyInjectionGeneratedCodeInstance CreateGeneratedCodeInstance(IDependencyInjectionInstance instance);
    }
}
