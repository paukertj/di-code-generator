using DiCodeGenerator.Generator.Models.Generating.TypeGeneratedCode;

namespace DiCodeGenerator.Generator.Models.Generating.ReferenceGeneratedCode
{
    public interface IReferenceGeneratedCodeInstance
    {
        ITypeGeneratedCodeInstance Service { get; }
    }
}
