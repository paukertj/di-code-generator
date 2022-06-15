using DiCodeGenerator.Generator.Models.Generating.DependencyInjectionGeneratedCode;
using DiCodeGenerator.Generator.Services.SourceCodeBuilder.Enums;
using DiCodeGenerator.Generator.Services.SourceCodeBuilder.Primitives;
using System.Collections.Generic;
using System.Text;

namespace DiCodeGenerator.Generator.Services.SourceCodeBuilder
{
    public interface ISourceCodeBuilderService
    {
        ISourceCodeBuilderService AddUsing(IEnumerable<string> value);
        ISourceCodeBuilderService AddUsing(string value);
        ISourceCodeBuilderService AddNamespace(string value);
        ISourceCodeBuilderService AddClass(Modifiers modifiers, string name);
        ISourceCodeBuilderService AddMethod(Modifiers modifiers, string name, params Argument[] arguments);
        ISourceCodeBuilderService AddMethod<TReturn>(Modifiers modifiers, string name, params Argument[] arguments);
        ISourceCodeBuilderService AddServiceCollection(string collection, IEnumerable<IDependencyInjectionGeneratedCodeInstance> instances);
        ISourceCodeBuilderService AddServiceCollection(string collection, IDependencyInjectionGeneratedCodeInstance instance);
        StringBuilder ToSourceCode();
        StringBuilder GetSourceBlock();
        IReadOnlyDictionary<string, ISourceCodeBuilderService> GetMethodsBlocks();
        IReadOnlyDictionary<string, ISourceCodeBuilderService> GetClassesBlocks();
        IReadOnlyDictionary<string, ISourceCodeBuilderService> GetNamespacesBlocks();
    }
}
