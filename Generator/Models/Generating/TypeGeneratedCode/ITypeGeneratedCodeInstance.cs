namespace DiCodeGenerator.Generator.Models.Generating.TypeGeneratedCode
{
    public interface ITypeGeneratedCodeInstance
    {
        string FullName { get; }
        string Namespace { get; }
        string TypeName { get; }
    }
}
